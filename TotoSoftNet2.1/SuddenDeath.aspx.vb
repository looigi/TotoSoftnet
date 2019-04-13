Public Class SuddenDeath
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
            Dim Sql As String
            Dim Giornata As Integer

            Sql = "Select Max(Giornata) From SuddenDeathDett Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Giornata = -1
            Else
                Giornata = Rec(0).Value
            End If
            Rec.Close()

            'If Giornata < 1 Then
            '    lblAvviso.Text = "La competizione non è ancora cominciata"
            '    divApertoChiuso.Visible = True
            '    divDettaglio1.Visible = False
            '    divDettaglio2.Visible = False
            '    divDettaglio3.Visible = False
            '    divVincente.Visible = False
            'Else
            '    divApertoChiuso.Visible = False
            divDettaglio1.Visible = True
            divDettaglio2.Visible = True
            divDettaglio3.Visible = True
            divVincente.Visible = True
            CaricaCombo(Db, ConnSQL)
            VisualizzaDati()
            ControllaVincitore(Db, ConnSQL)
            'End If

            Menu1.Items(0).Selected = True

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub CaricaCombo(Db As GestioneDB, ConnSql As Object)
        Dim Sql As String
        Dim Rec As Object = Server.CreateObject("ADODB.RecordSet")

        Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        cmbGiocatori.Items.Clear()
        Do Until Rec.Eof
            cmbGiocatori.Items.Add(Rec("Giocatore").value)

            Rec.MoveNext
        Loop
        Rec.Close

        cmbGiocatori.Text = Session("Nick")

        CaricaPartite()
    End Sub

    Private Sub CaricaPartite()
        Dim Chi As String = cmbGiocatori.Text
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim g As New Giocatori
        Dim id As Integer = g.TornaIdGiocatore(Chi)
        g = Nothing

        Sql = "Select A.Giornata As Giornata, A.Squadra, B.SquadraCasa+'-'+B.SquadraFuori As Partita, B.Risultato, C.Punti " &
            "From SuddenDeathDett A " &
            "Left Join Schedine B On A.Anno=B.Anno And A.Giornata=B.Giornata " &
            "And (A.Squadra = B.SquadraCasa Or A.Squadra =B.SquadraFuori) " &
            "Left Join SuddenDeathPunti C On A.Anno=C.Anno And A.Giornata=C.Giornata And A.CodGiocatore=C.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.CodGiocatore=" & id & " Order By A.Giornata"
        Gr.ImpostaCampi(Sql, grdSquadre)
        Gr = Nothing
    End Sub

    Private Sub grdSquadre_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdSquadre.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgSquadra"), Image)
            Dim Nome As String = e.Row.Cells(2).Text
            Dim gmail As New GestioneMail

            ImgUtente.ImageUrl = gmail.RitornaImmagineSquadra(Nome, True)
            ImgUtente.DataBind()

            gmail = Nothing
        End If
    End Sub

    Private Sub grdClassifica_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdClassifica.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grdEsclusi_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdEsclusi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select A.*, Giocatore From SuddenDeathVinc A " &
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale
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

    Private Sub VisualizzaDati()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select B.Giocatore, Sum(Punti) As Punti From SuddenDeathPunti A " &
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
            "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore Not In (Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ") " &
            "Group By B.CodGiocatore, B.Giocatore " &
            "Order By 2 Desc"
        Gr.ImpostaCampi(Sql, grdClassifica)
        Gr = Nothing

        Gr = New Griglie
        Sql = "Select Giornata, Giocatore From SuddenDeathEsclusi A " &
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " Order By Giornata"
        Gr.ImpostaCampi(Sql, grdEsclusi)
        Gr = Nothing
    End Sub

    Protected Sub cmbGiocatori_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbGiocatori.SelectedIndexChanged
        CaricaPartite()
    End Sub
End Class