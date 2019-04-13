Public Class CoppaInterToto
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

    Private Sub CaricaDati()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Giornata As Integer
            Dim Cosa As String = ""

            Giornata = DatiGioco.GiornataInterToto

            'If Giornata < 1 Then
            '    'lblAvviso.Text = "La competizione non è ancora cominciata"
            '    'divApertoChiuso.Visible = True
            '    divDettaglio.Visible = False
            '    divDettaglio2.Visible = False
            '    divVincente.Visible = False
            'Else
            'divApertoChiuso.Visible = False
            divDettaglio.Visible = True
            divDettaglio2.Visible = True
            divVincente.Visible = True

            VisualizzaDiretti(Db, ConnSQL, 2)

            PrendeQualificate(Db, ConnSQL)
            ControllaVincitore(Db, ConnSQL)
            'End If

            Menu1.Items(0).Selected = True

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteInterToto A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        lblVincitore.Text = ""
        If Rec.Eof = True Then
            divVincente.Visible = False
        Else
            divVincente.Visible = True
            lblVincitore.Text = Rec("Giocatore").Value
            imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
        End If
        Rec.Close()
    End Sub

    Private Sub VisualizzaDiretti(Db As GestioneDB, ConnSql As Object, Giornata As Integer)
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

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteInterToto A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & GiornataAndata & " Order By A.Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Rec("GiocCasa").Value <> 99 Or Rec("GiocFuori").Value <> 99 Then
                Trovate = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Partita").Value
                risu = Rec("RisultatoUfficiale").Value & " (" & Rec("RisultatoReale").Value & ")"
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
                If risu.Trim = " ()" Then risu = ""
                riga(3) = risu

                Sql = "Select * From PartiteInterToto Where Anno=" & DatiGioco.AnnoAttuale & " " & _
                    "And Giornata=" & GiornataRitorno & " And GiocCasa=" & Rec("GiocFuori").Value & " And GiocFuori=" & Rec("GiocCasa").Value
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = False Then
                    Dim Risu1 As String = "" & Rec2("RisultatoUfficiale").Value
                    Dim Risu2 As String = "" & Rec2("RisultatoReale").Value

                    If Risu1 <> "" Then
                        Risu1 = Mid(Risu1, Risu1.IndexOf("-") + 2, Risu1.Length) & "-" & Mid(Risu1, 1, Risu1.IndexOf("-"))
                    Else
                        Risu1 = ""
                    End If

                    If Risu2 <> "" Then
                        Risu2 = Mid(Risu2, Risu2.IndexOf("-") + 2, Risu2.Length) & "-" & Mid(Risu2, 1, Risu2.IndexOf("-"))
                    Else
                        Risu2 = ""
                    End If

                    risu = Risu1 & " (" & Risu2 & ")"
                    If risu.Trim = " ()" Then risu = ""
                    If riga(1) = "RIPOSO" Or riga(2) = "RIPOSO" Then
                        risu = ""
                    End If

                    riga(4) = risu
                    'If GiornataRitorno = 2 Then
                    '    Rec2.Close()

                    '    Sql = "Select GiocCasa From PartiteInterToto Where Anno=" & DatiGioco.AnnoAttuale & " " & _
                    '        "And GiocFuori=-1"
                    '    Rec2 = Db.LeggeQuery(ConnSql, Sql)
                    '    If Rec2.Eof = False Then
                    '        Passato = Rec2("GiocCasa").Value
                    '    Else
                    '        Passato = -1
                    '    End If
                    'Else
                    Passato = ev.ControllaVincente(Db, ConnSql, "" & Rec("RisultatoUfficiale").Value, "PartiteInterToto", Rec("GiocCasa").Value, Rec("GiocFuori").Value, GiornataRitorno, "" & Rec("RisultatoReale").Value)
                    'End If

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

        'If Trovate = False Then
        '    lblAvviso.Text = "La competizione non ha valori per questo turno"
        '    divApertoChiuso.Visible = True
        '    divDettaglio.Visible = False
        '    divDettaglio2.Visible = False
        '    divVincente.Visible = False
        'Else
        '    divApertoChiuso.Visible = False
        '    divDettaglio.Visible = True
        '    divDettaglio2.Visible = True
        '    divVincente.Visible = True
        'End If

        ev = Nothing
        g = Nothing

        Dim TurnoAttuale As String = RitornaTurnoCoppe(Db, ConnSql, "PartiteInterToto", GiornataAndata)

        lblAttuale.Text = "InterToto" ' : " & TurnoAttuale

        grdPartite.DataSource = dttTabella
        grdPartite.DataBind()
    End Sub

    Private Sub PrendeQualificate(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Posizione As New DataColumn("Posizione")
        Dim Squadra As New DataColumn("Squadra")
        Dim Provenienza As New DataColumn("Provenienza")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        dttTabella.Columns.Add(Posizione)
        dttTabella.Columns.Add(Squadra)
        dttTabella.Columns.Add(Provenienza)

        Sql = "Select A.*, B.Giocatore From InterTotoSquadre A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " & _
            "Order By Posizione"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            riga(0) = Rec("Posizione").Value
            riga(1) = Rec("Giocatore").Value
            riga(2) = Rec("DaDove").Value.ToString '.Replace(" ", "<br />")

            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop
        Rec.Close()

        grdQualificate.DataSource = dttTabella
        grdQualificate.DataBind()
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
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
                If Rec.Eof = False Then
                    If Rec("Cancellato").Value = "S" Then
                        e.Row.Cells(1).Text = "Riposo"
                        NomeGiocatoreC = "Riposo"
                    End If
                End If
                Rec.Close()

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
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

End Class