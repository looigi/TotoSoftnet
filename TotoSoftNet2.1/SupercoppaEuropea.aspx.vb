Public Class SupercoppaEuropea
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
            Dim Sql As String
            Dim Giornata As Integer

            Sql = "Select * From PartiteSuperCoppaEuropeaTurni " &
                "Where GiocCasa<>-1 And GiocFuori<>-1 "
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                If Rec(0).Value Is DBNull.Value = True Then
                    Giornata = -1
                Else
                    Giornata = Rec(0).Value
                End If
            Else
                Giornata = -1
            End If
            Rec.Close()

            If Giornata < 1 Then
                lblAvviso.Text = "La competizione non è ancora cominciata"
                divApertoChiuso.Visible = True
                divDettaglio.Visible = False
                divVincente.Visible = False
            Else
                divApertoChiuso.Visible = False
                divDettaglio.Visible = True
                divVincente.Visible = True
                VisualizzaSquadre(Db, ConnSQL, 38)
                ControllaVincitore(Db, ConnSQL)
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteSuperCoppaEuropeaTurni A " &
            "Left Join Giocatori B On A.Anno=B.Anno And A.Vincitore=B.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori<>-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = True Then
            divVincente.Visible = False
            lblVincitore.Text = ""
        Else
            divVincente.Visible = True
            lblVincitore.Text = Rec("Giocatore").Value
            imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
        End If
        Rec.Close()
    End Sub

    Private Sub VisualizzaSquadre(Db As GestioneDB, ConnSql As Object, Giornata As Integer)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Partita As New DataColumn("Partita")
        Dim Casa As New DataColumn("Casa")
        Dim Fuori As New DataColumn("Fuori")
        Dim RisAndata As New DataColumn("RisAndata")
        Dim Vincente As New DataColumn("Vincente")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        dttTabella.Columns.Add(Partita)
        dttTabella.Columns.Add(Casa)
        dttTabella.Columns.Add(Fuori)
        dttTabella.Columns.Add(RisAndata)
        dttTabella.Columns.Add(Vincente)

        Dim ev As New Eventi
        Dim g As New Giocatori
        Dim Trovate As Boolean = False
        Dim risu As String

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteSuperCoppaEuropeaTurni A " &
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=38 " &
            "Order By A.Partita"
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

        If Trovate = False Then
            lblAvviso.Text = "La competizione non ha valori per questo turno"
            divApertoChiuso.Visible = True
            divDettaglio.Visible = False
            divVincente.Visible = False
        Else
            divApertoChiuso.Visible = False
            divDettaglio.Visible = True
            divVincente.Visible = True
        End If

        ev = Nothing
        g = Nothing

        lblAttuale.Text = "Supercoppa Europea: Finale secca"

        grdPartite.DataSource = dttTabella
        grdPartite.DataBind()
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
End Class