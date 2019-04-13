Imports System.IO

Public Class Commenti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        divCommento.Visible = False
        divCommenti.Visible = True
        Select Case Session("Permesso")
            Case Permessi.Commentantore
                divCommento.Visible = True
                divCommenti.Visible = False
            Case Permessi.Amministratore
                divCommento.Visible = True
                divCommenti.Visible = False
        End Select

        If Page.IsPostBack = False Then
            hdnGiornata.Value = DatiGioco.Giornata

            CaricaGiornata()
        End If
    End Sub

    Private Sub CaricaGiornata()
        Dim NomeCommento As String = hdnGiornata.Value & ".txt"
        Dim Percorso As String = PercorsoApplicazione.Replace("\", "/") & "/Commenti/" & DatiGioco.AnnoAttuale.ToString.Trim & "/"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim g As New GestioneFilesDirectory
        g.CreaDirectoryDaPercorso(Percorso)
        If File.Exists(Percorso & NomeCommento) = True Then
            If Session("Permesso") = Permessi.Amministratore Or Session("Permesso") = Permessi.Commentantore Then
                FreeTextBox1.Text = g.LeggeFileIntero(Percorso & NomeCommento)
            Else
                FreeTextBox2.Text = g.LeggeFileIntero(Percorso & NomeCommento)
            End If
        Else
            FreeTextBox1.Text = ""
            FreeTextBox2.Text = ""
        End If
        lblAttuale.Text = "Commento alla giornata " & hdnGiornata.Value
        lblCommento.Text = "Commento alla giornata " & hdnGiornata.Value
        g = Nothing
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim Testo As String = FreeTextBox1.Text
        Dim NomeCommento As String = DatiGioco.Giornata & ".txt"
        Dim Percorso As String = PercorsoApplicazione.Replace("\", "/") & "/Commenti/" & DatiGioco.AnnoAttuale.ToString.Trim & "/"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim gf As New GestioneFilesDirectory
        gf.CreaDirectoryDaPercorso(Percorso)
        Try
            Kill(Percorso & NomeCommento)
        Catch ex As Exception

        End Try
        gf.CreaAggiornaFile (Percorso & NomeCommento, Testo)
        gf = Nothing

        Dim gm As New GestioneMail
        gm.InviaMailCommentoAllaGiornata(Testo)
        gm = Nothing
    End Sub

    Protected Sub cmdIndietroG_Click(sender As Object, e As EventArgs) Handles cmdIndietroG.Click
        Dim Giorn As Integer = hdnGiornata.Value

        If Giorn > 1 Then
            Giorn -= 1
            hdnGiornata.Value = Giorn
            CaricaGiornata()
        End If
    End Sub

    Protected Sub cmdAvantiG_Click(sender As Object, e As EventArgs) Handles cmdAvantiG.Click
        Dim Giorn As Integer = hdnGiornata.Value

        If Giorn < 38 Then
            Giorn += 1
            hdnGiornata.Value = Giorn
            CaricaGiornata()
        End If
    End Sub

    Protected Sub cmdIndietroA_Click(sender As Object, e As EventArgs) Handles cmdIndietroA.Click
        Dim Giorn As Integer = hdnGiornata.Value

        If Giorn > 1 Then
            Giorn -= 1
            hdnGiornata.Value = Giorn
            CaricaGiornata()
        End If
    End Sub

    Protected Sub cmdAvantiA_Click(sender As Object, e As EventArgs) Handles cmdAvantiA.Click
        Dim Giorn As Integer = hdnGiornata.Value

        If Giorn < 38 Then
            Giorn += 1
            hdnGiornata.Value = Giorn
            CaricaGiornata()
        End If
    End Sub
End Class