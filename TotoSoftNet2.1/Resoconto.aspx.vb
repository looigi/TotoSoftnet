Imports System.IO

Public Class Resoconto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Dim Db As New GestioneDB
            Dim gg As Integer

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    gg = DatiGioco.GiornataSpeciale + 100

                    PremutoOption = True
                    optNormale.Checked = False
                    PremutoOption = True
                    optSpeciale.Checked = True
                Else
                    gg = DatiGioco.Giornata

                    PremutoOption = True
                    optNormale.Checked = True
                    PremutoOption = True
                    optSpeciale.Checked = False
                End If

                ConnSQL.Close()
            End If

            Db = Nothing

            PremutoOption = False
            hdnGiornata.Value = gg

            CaricaDati()
        End If
    End Sub

    Private Sub CaricaDati()
        Dim NomeFile As String = hdnGiornata.Value & ".htm"
        Dim Path As String = "InvioMail\" & DatiGioco.AnnoAttuale & "\"
        Dim gf As New GestioneFilesDirectory
        Dim gMail As New GestioneMail

        Dim Testo As String

        If File.Exists(Server.MapPath(Path) & NomeFile) = True Then
            Testo = gf.LeggeFileIntero(Server.MapPath(Path) & NomeFile)
        Else
            Testo = "" ' gMail.ApreTesto() & "Nessun file di resoconto" & gMail.ChiudeTesto()
        End If
        Testo = "<center>" & Testo & "</center>"

        divTesto.InnerHtml = Testo

        gf = Nothing
        gMail = Nothing

        Dim g As Integer = hdnGiornata.Value
        If g > 100 Then g -= 100

        lblAttuale.Text = "Giornata " & g
    End Sub

    Protected Sub cmdIndietroG_Click(sender As Object, e As EventArgs) Handles cmdIndietroG.Click
        If hdnGiornata.Value > 1 Then
            hdnGiornata.Value -= 1
        End If
        If hdnGiornata.Value = 0 Then hdnGiornata.Value = 1

        CaricaDati()
    End Sub

    Protected Sub cmdAvantiG_Click(sender As Object, e As EventArgs) Handles cmdAvantiG.Click
        Dim Massima As Integer = 38

        If optSpeciale.Checked = True Then
            Massima = 120
        End If

        If hdnGiornata.Value < massima Then
            hdnGiornata.Value += 1
        End If

        CaricaDati()
    End Sub

    Protected Sub optNormale_CheckedChanged(sender As Object, e As EventArgs) Handles optNormale.CheckedChanged
        If PremutoOption = False Then
            PremutoOption = True

            optSpeciale.Checked = False
            hdnGiornata.Value = DatiGioco.Giornata

            CaricaDati()

            PremutoOption = False
        End If
    End Sub

    Protected Sub optSpeciale_CheckedChanged(sender As Object, e As EventArgs) Handles optSpeciale.CheckedChanged
        If PremutoOption = False Then
            PremutoOption = True

            optNormale.Checked = False
            hdnGiornata.Value = DatiGioco.GiornataSpeciale + 100

            CaricaDati()

            PremutoOption = False
        End If
    End Sub
End Class