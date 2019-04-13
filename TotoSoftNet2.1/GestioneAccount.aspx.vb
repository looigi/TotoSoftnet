Public Class GestioneAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaDati()
        End If
    End Sub

    Private Sub CaricaDati()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            txtNick.Text = Rec("Giocatore").Value
            txtPassword.Text = DecriptaPassword(Rec("Password").Value)
            txtNome.Text = Rec("Nome").Value
            txtCognome.Text = Rec("Cognome").Value
            txtMotto.Text = "" & Rec("Testo").Value
            txtEMail.Text = Rec("EMail").Value
            If Rec("InvioMailCompleta").Value = "S" Then
                chkControlloCompleto.Checked = True
            Else
                chkControlloCompleto.Checked = False
            End If
            Rec.Close()

            Dim NonCE As Boolean = False

            Sql = "Select * From NotificheGiocatori Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                If Rec("Attivo").Value = "S" Then
                    chkNotifiche.Checked = True
                Else
                    chkNotifiche.Checked = False
                End If
            Else
                NonCE = True
                chkNotifiche.Checked = True
            End If
            Rec.Close()

            If NonCE Then
                Sql = "Insert Into NotificheGiocatori Values (" &
                    " " & DatiGioco.AnnoAttuale & ", " &
                    " " & Session("CodGiocatore") & ", " &
                    "'S' " &
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If txtNome.Text = "" Then
            Dim Messaggi() As String = {"Inserire il nome"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtNick.Text = "" Then
            Dim Messaggi() As String = {"Inserire il Nick Name"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            txtNick.Text = txtNick.Text.Replace("à", "a").Replace("è", "e").Replace("é", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u").Replace(" ", "_")
            For i As Integer = 1 To txtNick.Text.Length
                Dim c As String = Mid(txtNick.Text, i, 1)

                If LettereValide.IndexOf(c) = -1 Then
                    Dim Messaggi() As String = {"Caratteri non validi nel nick: " & c}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                    Exit Sub
                End If
            Next
            txtNick.Text = txtNick.Text.ToUpper
        End If
        If txtPassword.Text = "" Then
            Dim Messaggi() As String = {"Inserire la password"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtCognome.Text = "" Then
            Dim Messaggi() As String = {"Inserire il cognome"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtEMail.Text = "" Then
            Dim Messaggi() As String = {"Inserire l'E-Mail"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            If txtEMail.Text.IndexOf(".") = -1 Or txtEMail.Text.IndexOf("@") = -1 Or txtEMail.Text.Length < 5 Then
                Dim Messaggi() As String = {"E-Mail non valida"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
            End If
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim InvioMail As String = ""

            If chkControlloCompleto.Checked Then
                InvioMail = "S"
            Else
                InvioMail = "N"
            End If

            Sql = "Update Giocatori Set " &
                "Giocatore='" & SistemaTestoPerDB(txtNick.Text.ToUpper.Trim) & "' , " &
                "Password='" & SistemaTestoPerDB(CriptaPassword(txtPassword.Text)) & "' , " &
                "Nome='" & SistemaTestoPerDB(txtNome.Text) & "' , " &
                "Cognome='" & SistemaTestoPerDB(txtCognome.Text) & "' , " &
                "EMail='" & SistemaTestoPerDB(txtEMail.Text) & "' , " &
                "Testo='" & SistemaTestoPerDB(txtMotto.Text) & "', " &
                "InvioMailCompleta='" & InvioMail & "'" &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Db.EsegueSql(ConnSQL, Sql)

            Dim Attivo As String

            If chkNotifiche.Checked Then
                Attivo = "S"
            Else
                Attivo = "N"
            End If
            Sql = "Update NotificheGiocatori Set Attivo='" & Attivo & "' Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        'Dati immagine Avatar

        If FileUpload3.HasFile Then
            Dim Percorso As String
            Dim Nome As String = Session("Nick")

            Percorso = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\"

            Dim f As New GestioneFilesDirectory

            f.CreaDirectoryDaPercorso(Percorso)

            f = Nothing

            Percorso += Nome & ".jpg"

            FileUpload3.SaveAs(Percorso)

            Dim gi As New GestioneImmagini
            gi.Ridimensiona(Percorso, Percorso, 120, 120)
            gi.RidimensionaEArrotondaIcona(Percorso)
            gi = Nothing
        End If
        ' fine 

        'immagine sfondo
        If FileUpload2.HasFile Then
            Dim Percorso As String
            Dim Nome As String = Session("Nick")

            Percorso = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\Sfondi\"

            Dim f As New GestioneFilesDirectory

            f.CreaDirectoryDaPercorso(Percorso)

            f = Nothing

            Percorso += Nome & "_BN.jpg"

            Dim PercorsoAppoggio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\Sfondi\" & Nome & ".Jpg"

            FileUpload2.SaveAs(PercorsoAppoggio)
        End If
        'fine

        Db = Nothing

        Dim sMessaggi() As String = {"Dati account modificati"}
        VisualizzaMessaggioInPopup(sMessaggi, Me)
    End Sub
End Class