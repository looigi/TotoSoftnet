Public Class PulsantieraStatistiche
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aColonne.Visible = False
        aSegniUsciti.Visible = False
        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
                aSegniUsciti.Visible = True
            Case ValoriStatoConcorso.Chiuso
            Case ValoriStatoConcorso.DaControllare
                aColonne.Visible = True
            Case ValoriStatoConcorso.Nessuno
            Case ValoriStatoConcorso.AnnoChiuso
        End Select
    End Sub

End Class