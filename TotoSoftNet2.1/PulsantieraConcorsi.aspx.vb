Public Class PulsantieraConcorsi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aNuovoConcorso.Visible = False
        aChiudeConcorso.Visible = False
        aAggiornaConcorso.Visible = False
        aModificaConcorso.Visible = False
        aControlloSchedine.Visible = False
        aRiapreConcorso.Visible = False
        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
                aChiudeConcorso.Visible = True
                aModificaConcorso.Visible = True
                aAggiornaConcorso.Visible = True
            Case ValoriStatoConcorso.Chiuso
                aNuovoConcorso.Visible = True
                If DatiGioco.Giornata = 38 Then
                End If
            Case ValoriStatoConcorso.DaControllare
                aControlloSchedine.Visible = True
                aRiapreConcorso.Visible = True
            Case ValoriStatoConcorso.Nessuno
                aNuovoConcorso.Visible = True
            Case ValoriStatoConcorso.AnnoChiuso
        End Select
    End Sub

End Class