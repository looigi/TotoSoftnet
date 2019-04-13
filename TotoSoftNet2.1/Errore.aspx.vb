Public Class Errore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TipoErrore As String = Request.QueryString("Errore")
        Dim Utente As String = Request.QueryString("Utente")
        Dim Chiamante As String = Request.QueryString("Chiamante")
        Dim SQL As String = Request.QueryString("Sql")

        lblErrore.Text = TipoErrore
        lblUtente.Text = ""
        lblChiamante.Text = Chiamante
        lblSQL.Text = SQL
    End Sub

End Class