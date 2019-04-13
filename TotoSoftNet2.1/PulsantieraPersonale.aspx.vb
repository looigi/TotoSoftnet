Public Class PulsantieraPersonale
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aPropria.Visible = False
        aPortaAmico.Visible = False

        If DatiGioco.Giornata <= 4 Then
            aPortaAmico.Visible = True
        End If

        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
                aPropria.Visible = True
            Case ValoriStatoConcorso.Chiuso
            Case ValoriStatoConcorso.DaControllare
            Case ValoriStatoConcorso.Nessuno
            Case ValoriStatoConcorso.AnnoChiuso
        End Select
    End Sub

End Class