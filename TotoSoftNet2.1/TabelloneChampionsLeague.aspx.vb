Public Class TabelloneChampionsLeague
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaDati()
        End If
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Menu3_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu3.MenuItemClick
        MultiView3.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Private Sub CaricaDati()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Giornata As Integer

            Giornata = DatiGioco.GiornataChampionsLeague
            If Giornata = 0 Then Giornata = 1
            If Giornata > 6 Then Giornata = 6

            hdnGiornataA.Value = Giornata
            hdnGiornataB.Value = Giornata

            Menu1.Items(0).Selected = True
            Menu2.Items(0).Selected = True
            Menu3.Items(0).Selected = True

            PrendeQualificate(Db, ConnSQL, "B")
            VisualizzaTurniA(Db, ConnSQL)

            PrendeQualificate(Db, ConnSQL, "A")
            VisualizzaTurniB(Db, ConnSQL)

            DisegnaTabellone(1, "Champions", "Champion's League", "PartiteChampionsTurni", divScontriDiretti)

            ControllaVincitore(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteChampionsTurni A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
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

    Private Sub PrendeQualificate(Db As GestioneDB, ConnSQL As Object, Girone As String)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Posizione As New DataColumn("Posizione")
        Dim Squadra As New DataColumn("Squadra")
        Dim Provenienza As New DataColumn("Provenienza")
        Dim Punti As New DataColumn("Punti")
        Dim riga As DataRow
        Dim GF As New DataColumn("GF")
        Dim GS As New DataColumn("GS")
        Dim Giocate As New DataColumn("Giocate")
        Dim dttTabella As New DataTable()
        Dim Contatore As Integer = 0

        dttTabella.Columns.Add(Posizione)
        dttTabella.Columns.Add(Squadra)
        dttTabella.Columns.Add(Punti)
        dttTabella.Columns.Add(GF)
        dttTabella.Columns.Add(GS)
        dttTabella.Columns.Add(Giocate)
        dttTabella.Columns.Add(Provenienza)

        Dim StringaIn As String = ""

        Sql = "Select * From (" & _
            "Select 'A' As Girone, B.*, A.DaDove From ChampionsSquadre A " & _
            "Left Join AppoChampionsA B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Left Join Giocatori E On A.Anno=E.Anno And A.CodGiocatore=E.CodGiocatore " & _
            "Where B.Giocatore Is Not Null And E.Cancellato='N' " & _
            "Union All " & _
            "Select 'B' As Girone, C.*, A.DaDove From ChampionsSquadre A " & _
            "Left Join AppoChampionsB C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
            "Left Join Giocatori D On A.Anno=D.Anno And A.CodGiocatore=D.CodGiocatore " & _
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
            riga(3) = Rec("GFatti").Value
            riga(4) = Rec("GSubiti").Value
            riga(5) = Rec("Giocate").Value
            riga(6) = Rec("DaDove").Value.ToString ' .Replace(" ", "<br />")

            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop
        Rec.Close()

        ' Prende i giocatori che non hanno punti in classifica
        Dim Rec2 As Object = Server.CreateObject("ADODB.RecordSet")
        Dim DaDove As String

        Try
            Sql = "Drop Table AppoChampions" & Session("CodGiocatore")
            Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
        Catch ex As Exception

        End Try

        Sql = "Create Table AppoChampions" & Session("CodGiocatore") & " (CodGiocatore Integer, DaDove Varchar(50))"
        Db.EsegueSql(ConnSQL, Sql)

        Sql = "Select Distinct(GiocCasa) From PartiteChampions" & Girone & " " & _
            "Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            Sql = "Select DaDove From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
            Rec2 = Db.LeggeQuery(ConnSQL, Sql)
            If Rec2.Eof = True Then
                DaDove = ""
            Else
                DaDove = Rec2("DaDove").Value.ToString '.Replace(" ", "<br />")
            End If
            Rec2.Close()

            Sql = "Insert Into AppoChampions" & Session("CodGiocatore") & " Values (" & Rec(0).Value & ", '" & DaDove & "')"
            Db.EsegueSql(ConnSQL, Sql)

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = ""
        If StringaIn.Length > 0 Then
            StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
            Sql = "Select A.*, B.Giocatore From AppoChampions" & Session("CodGiocatore") & " A " & _
                "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore " & _
                "Where A.CodGiocatore Not In (" & StringaIn & ") Order By Giocatore"
        Else
            If StringaIn = "" Then
                Sql = "Select A.*, B.Giocatore From AppoChampions" & Session("CodGiocatore") & " A " & _
                    "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore " & _
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
                riga(3) = 0
                riga(4) = 0
                riga(5) = 0
                riga(6) = Rec("DaDove").Value.ToString '.Replace(" ", "<br />")

                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If

        Sql = "Drop Table AppoChampions" & Session("CodGiocatore")
        Db.EsegueSql(ConnSQL, Sql)
        ' Prende i giocatori che non hanno punti in classifica

        If Girone = "A" Then
            grdQualificateA.DataSource = dttTabella
            grdQualificateA.DataBind()
        Else
            grdQualificateB.DataSource = dttTabella
            grdQualificateB.DataBind()
        End If
    End Sub

    Private Sub VisualizzaTurniA(Db As GestioneDB, ConnSql As Object)
        'grdPartiteDirettiA.Visible = False
        grdPartiteTurniA.Visible = True
        cmdIndietroGA.Visible = True
        cmdAvantiGA.Visible = True

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim Risultato As New DataColumn("Risultato")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim Girone As String = "A"
        Dim Giornata As Integer = hdnGiornataA.Value

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(Risultato)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim Risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampions" & Girone & " A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata & " Order By A.Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Rec("GiocCasa").Value <> 99 Or Rec("GiocFuori").Value <> 99 Then
                Trovate = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Partita").Value
                Risu = Rec("RisultatoUfficiale").Value & " (" & Rec("RisultatoReale").Value & ")"
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
                If Risu.Trim = " ()" Then Risu = ""

                riga(3) = Risu

                dttTabella.Rows.Add(riga)
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        'If Trovate = False Then
        '    lblAvviso.Text = "La competizione non ha valori per questo turno"
        '    divApertoChiuso.Visible = True
        '    divQualificate.Visible = False
        '    divDettaglio.Visible = False
        '    divVincente.Visible = False
        'Else
        '    divApertoChiuso.Visible = False
        '    divQualificate.Visible = True
        '    divVincente.Visible = True
        '    divDettaglio.Visible = True
        'End If

        ev = Nothing
        g = Nothing

        lblAttualeA.Text = "Champion's League: Giornata " & Giornata & " - Girone " & Girone

        grdPartiteTurniA.DataSource = dttTabella
        grdPartiteTurniA.DataBind()

        hdnGiornataA.Value = Giornata
    End Sub

    Private Sub VisualizzaTurniB(Db As GestioneDB, ConnSql As Object)
        'grdPartiteDirettiA.Visible = False
        grdPartiteTurniB.Visible = True
        cmdIndietroGB.Visible = True
        cmdAvantiGB.Visible = True

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim Risultato As New DataColumn("Risultato")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim Girone As String = "B"
        Dim Giornata As Integer = hdnGiornataB.Value

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(Risultato)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim Risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampions" & Girone & " A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata & " Order By A.Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Rec("GiocCasa").Value <> 99 Or Rec("GiocFuori").Value <> 99 Then
                Trovate = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Partita").Value
                Risu = Rec("RisultatoUfficiale").Value & " (" & Rec("RisultatoReale").Value & ")"
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
                If Risu.Trim = " ()" Then Risu = ""

                riga(3) = Risu

                dttTabella.Rows.Add(riga)
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        'If Trovate = False Then
        '    lblAvviso.Text = "La competizione non ha valori per questo turno"
        '    divApertoChiuso.Visible = True
        '    divQualificate.Visible = False
        '    divDettaglio.Visible = False
        '    divVincente.Visible = False
        'Else
        '    divApertoChiuso.Visible = False
        '    divQualificate.Visible = True
        '    divVincente.Visible = True
        '    divDettaglio.Visible = True
        'End If

        ev = Nothing
        g = Nothing

        lblAttualeB.Text = "Champion's League: Giornata " & Giornata & " - Girone " & Girone

        grdPartiteTurniB.DataSource = dttTabella
        grdPartiteTurniB.DataBind()

        hdnGiornataB.Value = Giornata
    End Sub

    Private Sub grdPartiteTurniA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartiteTurniA.RowDataBound
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

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreC & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("Cancellato").Value = "S" Then
                    e.Row.Cells(1).Text = "Riposo"
                    NomeGiocatoreC = "Riposo"
                End If
                Rec.Close()

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
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

    Private Sub grdPartiteTurniB_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartiteTurniB.RowDataBound
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

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreC & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("Cancellato").Value = "S" Then
                    e.Row.Cells(1).Text = "Riposo"
                    NomeGiocatoreC = "Riposo"
                End If
                Rec.Close()

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
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

    Private Sub grdQualificateA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdQualificateA.RowDataBound
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

    Private Sub grdQualificateB_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdQualificateB.RowDataBound
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

    Protected Sub cmdIndietroGA_Click(sender As Object, e As EventArgs) Handles cmdIndietroGA.Click
        If Val(hdnGiornataA.Value) > 1 Then
            hdnGiornataA.Value -= 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurniA(Db, ConnSQL)
                'ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdIndietroGB_Click(sender As Object, e As EventArgs) Handles cmdIndietroGB.Click
        If Val(hdnGiornataB.Value) > 1 Then
            hdnGiornataB.Value -= 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurniB(Db, ConnSQL)
                'ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdAvantiGA_Click(sender As Object, e As EventArgs) Handles cmdAvantiGA.Click
        If Val(hdnGiornataA.Value) < 6 Then
            hdnGiornataA.Value += 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurniA(Db, ConnSQL)
                'ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdAvantiGB_Click(sender As Object, e As EventArgs) Handles cmdAvantiGB.Click
        If Val(hdnGiornataB.Value) < 6 Then
            hdnGiornataB.Value += 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurniB(Db, ConnSQL)
                'ControllaVincitore(Db, ConnSQL)
            End If

            Db = Nothing
        End If
    End Sub
End Class