Imports System.IO

Public Class CambioUtente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CreaTabella()
        End If

        If Request.QueryString("Cambio") <> "" Then
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Dim Nome As String = Request.QueryString("Cambio")

                Sql = "Select * From Giocatori Where Upper(LTrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(Nome.Trim.ToUpper) & "' And Anno=" & DatiGioco.AnnoAttuale
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Session("CodGiocatore") = Rec("CodGiocatore").Value
                Session("Nick") = Rec("Giocatore").Value
                Session("Cognome") = Rec("Cognome").Value
                Session("Nome") = Rec("Nome").Value
                Session("Permesso") = Rec("idTipologia").Value
                Session("EMail") = Rec("EMail").Value
                Rec.Close()
                ConnSQL.Close()

                Response.Redirect("Principale.aspx")
            End If
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Giocatore From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore<>" & SistemaTestoPerDB(Session("CodGiocatore")) & " And Cancellato<>'S' Order By Giocatore"
        Gr.ImpostaCampi(Sql, grdUtenti)
        Gr = Nothing
    End Sub

    Private Sub grdUtenti_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUtenti.PageIndexChanging
        grdUtenti.PageIndex = e.NewPageIndex
        grdUtenti.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdUtenti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUtenti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim ImgTasto As ImageButton = DirectCast(e.Row.FindControl("imgVai"), ImageButton)
            Dim Nome As String = e.Row.Cells(1).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()

            ImgTasto.PostBackUrl = "CambioUtente.aspx?Cambio=" & Nome
            ImgTasto.DataBind()
        End If
    End Sub
End Class