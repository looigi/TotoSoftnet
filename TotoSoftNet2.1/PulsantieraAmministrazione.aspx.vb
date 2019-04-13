Public Class PulsantieraAmministrazione
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aChiusuraAnno.Visible = False

        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
            Case ValoriStatoConcorso.Chiuso
                If DatiGioco.Giornata = 38 Then
                    aChiusuraAnno.Visible = True
                End If
            Case ValoriStatoConcorso.DaControllare
            Case ValoriStatoConcorso.Nessuno
            Case ValoriStatoConcorso.AnnoChiuso
        End Select
    End Sub
End Class