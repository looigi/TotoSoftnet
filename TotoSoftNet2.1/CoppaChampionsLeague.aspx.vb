Public Class CoppaChampionsLeague
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaDati()
        End If
    End Sub

    Private Sub CaricaDati()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Giornata As Integer
            Dim Cosa As String = ""

            Giornata = DatiGioco.GiornataChampionsLeague
            If Giornata = 0 Then Giornata = 1
            hdnGirone.Value = "A"

            If Giornata < 1 Then
                lblAvviso.Text = "La competizione non è ancora cominciata"
                divApertoChiuso.Visible = True
                divQualificate.Visible = False
                divDettaglio.Visible = False
                divVincente.Visible = False
            Else
                divApertoChiuso.Visible = False
                divQualificate.Visible = True
                divDettaglio.Visible = True
                divVincente.Visible = True

                If Giornata > 6 Then
                    divQualificate.Visible = False
                    VisualizzaDiretti(Db, ConnSQL, Giornata)
                Else
                    divQualificate.Visible = True
                    Dim g As Integer = DatiGioco.GiornataChampionsLeague
                    If g > 6 Then g = 6
                    If g = 0 Then g = 1
                    VisualizzaTurni(Db, ConnSQL, g)
                End If

                PrendeQualificate(Db, ConnSQL)
                ControllaVincitore(Db, ConnSQL)
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select A.GiocCasa, B.Giocatore From " & PrefissoTabelle & "PartiteChampionsTurni A " & _
            "Left Join " & PrefissoTabelle & "Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = True Then
            divVincente.Visible = False
        Else
            divVincente.Visible = True
            lblVincitore.Text = Rec("Giocatore").Value
            imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
        End If
        Rec.Close()
    End Sub

    Private Sub PrendeQualificate(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Posizione As New DataColumn("Posizione")
        Dim Squadra As New DataColumn("Squadra")
        Dim Provenienza As New DataColumn("Provenienza")
        Dim Punti As New DataColumn("Punti")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim Contatore As Integer = 0
        Dim Girone As String = hdnGirone.Value

        dttTabella.Columns.Add(Posizione)
        dttTabella.Columns.Add(Squadra)
        dttTabella.Columns.Add(Punti)
        dttTabella.Columns.Add(Provenienza)

        Dim StringaIn As String = ""

        Sql = "Select * From (" & _
            "Select 'A' As Girone, B.*, A.DaDove From " & PrefissoTabelle & "ChampionsSquadre A " & _
            "Left Join " & PrefissoTabelle & "AppoChampionsA B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Left Join " & PrefissoTabelle & "Giocatori E On A.Anno=E.Anno And A.CodGiocatore=E.CodGiocatore " & _
            "Where B.Giocatore Is Not Null And E.Cancellato='N' " & _
            "Union All " & _
            "Select 'B' As Girone, C.*, A.DaDove From " & PrefissoTabelle & "ChampionsSquadre A " & _
            "Left Join " & PrefissoTabelle & "AppoChampionsB C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
            "Left Join " & PrefissoTabelle & "Giocatori D On A.Anno=D.Anno And A.CodGiocatore=D.CodGiocatore " & _
            "Where C.Giocatore Is Not Null And D.Cancellato='N') S " & _
            "Where Anno=" & DatiGioco.AnnoAttuale & " And Girone='" & Girone & "' " & _
            "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            StringaIn += Rec("CodGiocatore").Value & ", "

            Contatore += 1

            riga = dttTabella.NewRow()
            riga(0) = Contatore
            riga(1) = Rec("Giocatore").Value
            riga(2) = Rec("Punti").Value
            riga(3) = Rec("DaDove").Value.ToString.Replace(" ", "<br />")

            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop
        Rec.Close()

        ' Prende i giocatori che non hanno punti in classifica
        Dim Rec2 As Object = Server.CreateObject("ADODB.RecordSet")
        Dim DaDove As String

        Try
            Sql = "Drop Table " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore")
            Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
        Catch ex As Exception

        End Try

        Sql = "Create Table " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore") & " (CodGiocatore Integer, DaDove Varchar(50))"
        Db.EsegueSql(ConnSQL, Sql)

        Sql = "Select Distinct(GiocCasa) From " & PrefissoTabelle & "PartiteChampions" & Girone & " " & _
            "Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            Sql = "Select DaDove From " & PrefissoTabelle & "ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
            Rec2 = Db.LeggeQuery(ConnSQL, Sql)
            If Rec2.Eof = True Then
                DaDove = ""
            Else
                DaDove = Rec2("DaDove").Value.ToString.Replace(" ", "<br />")
            End If
            Rec2.Close()

            Sql = "Insert Into " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore") & " Values (" & Rec(0).Value & ", '" & DaDove & "')"
            Db.EsegueSql(ConnSQL, Sql)

            Rec.MoveNext()
        Loop
        Rec.Close()
        
        Sql = ""
        If StringaIn.Length > 0 Then
            StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
            Sql = "Select A.*, B.Giocatore From " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore") & " A " & _
                "Left Join " & PrefissoTabelle & "Giocatori B On A.CodGiocatore=B.CodGiocatore " & _
                "Where A.CodGiocatore Not In (" & StringaIn & ") Order By Giocatore"
        Else
            If StringaIn = "" Then
                Sql = "Select A.*, B.Giocatore From " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore") & " A " & _
                    "Left Join " & PrefissoTabelle & "Giocatori B On A.CodGiocatore=B.CodGiocatore " & _
                        "Order By Giocatore"
            End If
        End If
        If Sql <> "" Then
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Contatore += 1

                riga = dttTabella.NewRow()
                riga(0) = Contatore
                riga(1) = Rec("Giocatore").Value
                riga(2) = 0
                riga(3) = Rec("DaDove").Value.ToString.Replace(" ", "<br />")

                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If

        Sql = "Drop Table " & PrefissoTabelle & "AppoChampions" & Session("CodGiocatore")
        Db.EsegueSql(ConnSQL, Sql)
        ' Prende i giocatori che non hanno punti in classifica

        grdQualificate.DataSource = dttTabella
        grdQualificate.DataBind()
    End Sub

    Private Sub VisualizzaDiretti(Db As GestioneDB, ConnSql As Object, Giornata As Integer)
        grdPartiteTurni.Visible = False
        grdPartiteDiretti.Visible = True
        cmdIndietroG.Visible = False
        cmdAvantiG.Visible = False

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim RisAndata As New DataColumn("RisAndata")
        Dim RisRitorno As New DataColumn("RisRitorno")
        Dim Vincente As New DataColumn("Vincente")
        Dim Passato As Integer
        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        Dim GiornataAndata As Integer
        Dim GiornataRitorno As Integer

        If Giornata / 2 = Int(Giornata / 2) Then
            GiornataAndata = Giornata - 1
            GiornataRitorno = Giornata
        Else
            GiornataAndata = Giornata
            GiornataRitorno = Giornata + 1
        End If

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(RisAndata)
        dttTabella.Columns.Add(RisRitorno)
        dttTabella.Columns.Add(Vincente)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From " & PrefissoTabelle & "PartiteChampionsTurni A " & _
            "Left Join " & PrefissoTabelle & "Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join " & PrefissoTabelle & "Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & GiornataAndata & " Order By A.Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Rec("GiocCasa").Value <> 99 Or Rec("GiocFuori").Value <> 99 Then
                Trovate = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Partita").Value
                risu = Rec("RisultatoUfficiale").Value & "<br />(" & Rec("RisultatoReale").Value & ")"
                If Rec("GiocCasa").Value = 99 Then
                    riga(1) = "RIPOSO"
                    risu = ""
                Else
                    riga(1) = Rec("Casa").Value
                End If
                If Rec("GiocFuori").Value = 99 Then
                    riga(2) = "RIPOSO"
                    risu = ""
                Else
                    riga(2) = Rec("Fuori").Value
                End If
                If risu.Trim = "<br />()" Then risu = ""
                riga(3) = risu

                Sql = "Select * From " & PrefissoTabelle & "PartiteChampionsTurni Where Anno=" & DatiGioco.AnnoAttuale & " " & _
                    "And Giornata=" & GiornataRitorno & " And GiocCasa=" & Rec("GiocFuori").Value & " And GiocFuori=" & Rec("GiocCasa").Value
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = False Then
                    Dim Risu1 As String = "" & Rec2("RisultatoUfficiale").Value
                    Dim Risu2 As String = "" & Rec2("RisultatoReale").Value

                    If Risu1 <> "" Then
                        Risu1 = Mid(Risu1, Risu1.IndexOf("-") + 2, Risu1.Length) & "-" & Mid(Risu1, 1, Risu1.IndexOf("-"))
                    End If
                    If Risu2 <> "" Then
                        Risu2 = Mid(Risu2, Risu2.IndexOf("-") + 2, Risu2.Length) & "-" & Mid(Risu2, 1, Risu2.IndexOf("-"))
                    End If

                    risu = Risu1 & "<br />(" & Risu2 & ")"
                    If risu.Trim = "<br />()" Then risu = ""
                    If riga(1) = "RIPOSO" Or riga(2) = "RIPOSO" Then
                        risu = ""
                    End If

                    riga(4) = risu

                    If GiornataRitorno = 10 Then
                        Rec2.Close()

                        Sql = "Select GiocCasa From " & PrefissoTabelle & "PartiteChampionsTurni Where Anno=" & DatiGioco.AnnoAttuale & " " & _
                            "And GiocFuori=-1"
                        Rec2 = Db.LeggeQuery(ConnSql, Sql)
                        If Rec2.Eof = False Then
                            Passato = Rec2("GiocCasa").Value
                        Else
                            Passato = -1
                        End If
                    Else
                        If risu <> "" Then
                            Passato = ev.ControllaVincente(Db, ConnSql, "" & Rec("RisultatoUfficiale").Value, "PartiteChampionsTurni", Rec("GiocCasa").Value, Rec("GiocFuori").Value, GiornataRitorno, "" & Rec("RisultatoReale").Value)
                            riga(5) = g.TornaNickGiocatore(Passato)
                        Else
                            Passato = -1
                            riga(5) = ""
                        End If
                    End If
                    riga(5) = g.TornaNickGiocatore(Passato)
                Else
                    riga(4) = ""
                    riga(5) = ""
                End If
                Rec2.Close()

                dttTabella.Rows.Add(riga)
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        If Trovate = False Then
            lblAvviso.Text = "La competizione non ha valori per questo turno"
            divApertoChiuso.Visible = True
            divQualificate.Visible = False
            divDettaglio.Visible = False
            divVincente.Visible = False
        Else
            divApertoChiuso.Visible = False
            divQualificate.Visible = True
            divVincente.Visible = True
            divDettaglio.Visible = True
        End If

        ev = Nothing
        g = Nothing

        Dim TurnoAttuale As String = RitornaTurnoCoppe(Db, ConnSql, "PartiteChampionsTurni", GiornataAndata)

        lblAttuale.Text = "Champion's League: " & TurnoAttuale

        grdPartiteDiretti.DataSource = dttTabella
        grdPartiteDiretti.DataBind()
    End Sub

    Private Sub VisualizzaTurni(Db As GestioneDB, ConnSql As Object, Giornata As Integer)
        grdPartiteDiretti.Visible = False
        grdPartiteTurni.Visible = True
        cmdIndietroG.Visible = True
        cmdAvantiG.Visible = True

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim Risultato As New DataColumn("Risultato")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim Girone As String = hdnGirone.Value

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(Risultato)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim Risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From " & PrefissoTabelle & "PartiteChampions" & Girone & " A " & _
            "Left Join " & PrefissoTabelle & "Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join " & PrefissoTabelle & "Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata & " Order By A.Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Rec("GiocCasa").Value <> 99 Or Rec("GiocFuori").Value <> 99 Then
                Trovate = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Partita").Value
                Risu = Rec("RisultatoUfficiale").Value & "<br />(" & Rec("RisultatoReale").Value & ")"
                If Rec("GiocCasa").Value = 99 Then
                    riga(1) = "RIPOSO"
                    Risu = ""
                Else
                    riga(1) = Rec("Casa").Value
                End If
                If Rec("GiocFuori").Value = 99 Then
                    riga(2) = "RIPOSO"
                    Risu = ""
                Else
                    riga(2) = Rec("Fuori").Value
                End If
                If Risu.Trim = "<br />()" Then Risu = ""

                riga(3) = Risu

                dttTabella.Rows.Add(riga)
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        If Trovate = False Then
            lblAvviso.Text = "La competizione non ha valori per questo turno"
            divApertoChiuso.Visible = True
            divQualificate.Visible = False
            divDettaglio.Visible = False
            divVincente.Visible = False
        Else
            divApertoChiuso.Visible = False
            divQualificate.Visible = True
            divVincente.Visible = True
            divDettaglio.Visible = True
        End If

        ev = Nothing
        g = Nothing

        lblAttuale.Text = "Champion's League: Giornata " & Giornata & " - Girone " & Girone

        grdPartiteTurni.DataSource = dttTabella
        grdPartiteTurni.DataBind()

        hdnGiornata.Value = Giornata
    End Sub

    Private Sub grdPartiteTurni_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartiteTurni.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatoreC As String = e.Row.Cells(2).Text
            Dim ImgGiocC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)

            Dim NomeGiocatoreF As String = e.Row.Cells(4).Text
            Dim ImgGiocF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)

            If NomeGiocatoreC = Session("Nick") Or NomeGiocatoreF = Session("Nick") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Select Cancellato From " & PrefissoTabelle & "Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreC & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("Cancellato").Value = "S" Then
                    e.Row.Cells(1).Text = "Riposo"
                    NomeGiocatoreC = "Riposo"
                End If
                Rec.Close()

                Sql = "Select Cancellato From " & PrefissoTabelle & "Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("Cancellato").Value = "S" Then
                    e.Row.Cells(4).Text = "Riposo"
                    NomeGiocatoreF = "Riposo"
                End If
                Rec.Close()

                ConnSQL.Close()
            End If

            Db = Nothing

            ImgGiocC.ImageUrl = RitornaImmagine(Server.MapPath(".").Replace("\", "/"), NomeGiocatoreC)
            ImgGiocC.DataBind()

            ImgGiocF.ImageUrl = RitornaImmagine(Server.MapPath(".").Replace("\", "/"), NomeGiocatoreF)
            ImgGiocF.DataBind()

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Sub grdPartiteDiretti_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartiteDiretti.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatoreC As String = e.Row.Cells(2).Text
            Dim ImgGiocC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)

            Dim NomeGiocatoreF As String = e.Row.Cells(4).Text
            Dim ImgGiocF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)

            If NomeGiocatoreC = Session("Nick") Or NomeGiocatoreF = Session("Nick") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Select Cancellato From " & PrefissoTabelle & "Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreC & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    If Rec("Cancellato").Value = "S" Then
                        e.Row.Cells(1).Text = "Riposo"
                        NomeGiocatoreC = "Riposo"
                    End If
                End If
                Rec.Close()

                Sql = "Select Cancellato From " & PrefissoTabelle & "Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    If Rec("Cancellato").Value = "S" Then
                        e.Row.Cells(4).Text = "Riposo"
                        NomeGiocatoreF = "Riposo"
                    End If
                End If
                Rec.Close()

                ConnSQL.Close()
            End If

            Db = Nothing

            ImgGiocC.ImageUrl = RitornaImmagine(Server.MapPath(".").Replace("\", "/"), NomeGiocatoreC)
            ImgGiocC.DataBind()

            ImgGiocF.ImageUrl = RitornaImmagine(Server.MapPath(".").Replace("\", "/"), NomeGiocatoreF)
            ImgGiocF.DataBind()

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Sub grdQualificate_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdQualificate.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(2).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath(".").Replace("\", "/"), NomeGiocatore)
            ImgGioc.DataBind()

            If NomeGiocatore = Session("Nick") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub cmdIndietroG_Click(sender As Object, e As EventArgs) Handles cmdIndietroG.Click
        If Val(hdnGiornata.Value) > 1 Then
            hdnGiornata.Value -= 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurni(Db, ConnSQL, hdnGiornata.Value)
                ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdAvantiG_Click(sender As Object, e As EventArgs) Handles cmdAvantiG.Click
        If Val(hdnGiornata.Value) < 6 Then
            hdnGiornata.Value += 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurni(Db, ConnSQL, hdnGiornata.Value)
                ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdTurniA_Click(sender As Object, e As EventArgs) Handles cmdTurniA.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            hdnGirone.Value = "A"
            divQualificate.Visible = True
            PrendeQualificate(Db, ConnSQL)

            Dim gg As New Giocatori
            Dim Nume As Integer = gg.PrendeNumeroGiocatori
            Dim sPerc As Perc = gg.PrendePercentualiCoppe(Nume)
            gg = Nothing

            Dim MaxGiornate As Integer = sPerc.QuantiEuropaLeague + sPerc.QuantiIntertoto

            Dim g As Integer = DatiGioco.GiornataChampionsLeague
            If g > MaxGiornate Then g = MaxGiornate
            If g < 1 Then g = 1

            VisualizzaTurni(Db, ConnSQL, g)
            ControllaVincitore(Db, ConnSQL)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdTurniB_Click(sender As Object, e As EventArgs) Handles cmdTurniB.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            hdnGirone.Value = "B"
            divQualificate.Visible = True
            PrendeQualificate(Db, ConnSQL)

            Dim gg As New Giocatori
            Dim Nume As Integer = gg.PrendeNumeroGiocatori
            Dim sPerc As Perc = gg.PrendePercentualiCoppe(Nume)
            gg = Nothing

            Dim MaxGiornate As Integer = sPerc.QuantiEuropaLeague + sPerc.QuantiIntertoto

            Dim g As Integer = DatiGioco.GiornataChampionsLeague
            If g > MaxGiornate Then g = MaxGiornate
            If g < 1 Then g = 1

            VisualizzaTurni(Db, ConnSQL, g)
            ControllaVincitore(Db, ConnSQL)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdSemifinale_Click(sender As Object, e As EventArgs) Handles cmdSemifinale.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            divQualificate.Visible = False
            VisualizzaDiretti(Db, ConnSQL, 7)
            ControllaVincitore(Db, ConnSQL)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdFinale_Click(sender As Object, e As EventArgs) Handles cmdFinale.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            divQualificate.Visible = False
            VisualizzaDiretti(Db, ConnSQL, 9)
            ControllaVincitore(Db, ConnSQL)
        End If

        Db = Nothing
    End Sub
End Class