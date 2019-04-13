Public Class RiapreConcorso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdRiapre_Click(sender As Object, e As EventArgs) Handles cmdRiapre.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto
            AggiornaDatiDiGioco(Db)

            Dim gg As Integer
            Dim Speciale As Boolean

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
                Speciale = True
            Else
                gg = DatiGioco.Giornata
                Speciale = False
            End If

            ConnSQL.Close()

            Dim gMail As New GestioneMail

            gMail.InviaMailRiaperturaConcorso(Session("Nick"), gg, Speciale)

            gMail = Nothing
        End If

        Db = Nothing

        Response.Redirect("Principale.aspx")
    End Sub
End Class