Public Class CoppaPippettero
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

            Giornata = DatiGioco.GiornataDerelitti

            'If Giornata < 1 Then
            '    lblAvviso.Text = "La competizione non è ancora cominciata"
            '    divApertoChiuso.Visible = True
            '    divDettaglio.Visible = False
            '    divDettaglio2.Visible = False
            '    divVincente.Visible = False
            'Else
            '    divApertoChiuso.Visible = False
            divDettaglio.Visible = True
            divDettaglio2.Visible = True
            divVincente.Visible = True

            VisualizzaTurni(Db, ConnSQL, Giornata)

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

        ' Controlla se c'è qualche giocatore con 0 punti
        Sql = "Select B.Giocatore From DerelittiSquadre A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where B.Giocatore Is Not Null And A.CodGiocatore Not In " & _
            "(Select CodGiocatore From AppoDerelitti) And A.Anno=" & DatiGioco.AnnoAttuale
        ' Controlla se c'è qualche giocatore con 0 punti
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            divVincente.Visible = True
            If Rec("Giocatore").Value Is DBNull.Value = True Then
                lblVincitore.Text = ""
            Else
                lblVincitore.Text = Rec("Giocatore").Value
                imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
            End If
        Else
            Sql = "Select Top 1 * From AppoDerelitti Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore Is Not Null " & _
                "Order By Punti, GFatti, GSubiti Desc, Differenza"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec.Eof = True Then
                divVincente.Visible = False
            Else
                divVincente.Visible = True
                If Rec("Giocatore").Value Is DBNull.Value = True Then
                    lblVincitore.Text = ""
                Else
                    lblVincitore.Text = Rec("Giocatore").Value
                    imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
                End If
            End If
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

        dttTabella.Columns.Add(Posizione)
        dttTabella.Columns.Add(Squadra)
        dttTabella.Columns.Add(Provenienza)
        dttTabella.Columns.Add(Punti)

        Sql = "Select A.*, C.Giocatore, IsNull(B.Punti,0) As Punti From DerelittiSquadre A " &
            "Left Join AppoDerelitti B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
            "Left Join Giocatori C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " &
            "Order By B.Punti, B.GFatti, B.GSubiti Desc, B.Differenza"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            If Rec("Giocatore").Value Is DBNull.Value = False Then
                riga = dttTabella.NewRow()
                riga(0) = Rec("Posizione").Value
                riga(1) = Rec("Giocatore").Value
                riga(2) = Rec("DaDove").Value
                riga(3) = Rec("Punti").Value

                dttTabella.Rows.Add(riga)
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        grdQualificate.DataSource = dttTabella
        grdQualificate.DataBind()
    End Sub

    Private Sub VisualizzaTurni(Db As GestioneDB, ConnSql As Object, Giornata As Integer)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim Risultato As New DataColumn("Risultato")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(Risultato)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteDerelitti A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata & " Order By A.Partita"
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

        lblAttuale.Text = "Torneo Er Pippettero: Giornata " & Giornata

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

    Protected Sub cmdIndietroG_Click(sender As Object, e As EventArgs) Handles cmdIndietroG.Click
        If Val(hdnGiornata.Value) > 1 Then
            hdnGiornata.Value -= 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurni(Db, ConnSQL, hdnGiornata.Value)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdAvantiG_Click(sender As Object, e As EventArgs) Handles cmdAvantiG.Click
        If Val(hdnGiornata.Value) < 3 Then
            hdnGiornata.Value += 1

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                VisualizzaTurni(Db, ConnSQL, hdnGiornata.Value)
            End If

            Db = Nothing
        End If
    End Sub

End Class