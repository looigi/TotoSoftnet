Imports System.IO

Public Class NuovoUtente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            DatiGioco.AnnoAttuale = Request.QueryString("Anno")

            lblMessaggio.Visible = False

            If Request.QueryString("Registrazione") <> "" Then
                SalvaGiocatore(Request.QueryString("Registrazione"))
            End If

        End If
    End Sub

    Private Function EffettuaControlli() As Boolean
        Dim Ok As Boolean = True

        If txtNick.Text.Trim = "" Then
            'Dim Messaggi() As String = {"Inserire il nome utente"}
            'VisualizzaMessaggioInPopup(Messaggi, Me)
            lblMessaggio.Text = "Inserire il nome utente"
            lblMessaggio.Visible = True
            Ok = False
        Else
            txtNick.Text = txtNick.Text.Replace("à", "a").Replace("è", "e").Replace("é", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u").Replace(" ", "_")
            For i As Integer = 1 To txtNick.Text.Length
                Dim c As String = Mid(txtNick.Text, i, 1)
                If LettereValide.IndexOf(c) = -1 Then
                    'Dim Messaggi() As String = {"Caratteri non validi nel nome utente: " & c}
                    'VisualizzaMessaggioInPopup(Messaggi, Me)
                    lblMessaggio.Text = "Caratteri non validi nel nome utente: " & c
                    lblMessaggio.Visible = True

                    Ok = False
                    Exit For
                End If
            Next
            txtNick.Text = txtNick.Text.ToUpper
        End If
        If Ok = True Then
            If txtNome.Text = "" Then
                'Dim Messaggi() As String = {"Inserire il nome"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Inserire il nome"
                lblMessaggio.Visible = True
                Ok = False
            End If
        End If
        If Ok = True Then
            If txtCognome.Text = "" Then
                'Dim Messaggi() As String = {"Inserire il cognome"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Inserire il cognome"
                lblMessaggio.Visible = True
                Ok = False
            End If
        End If
        If Ok = True Then
            If txtEMail.Text = "" Then
                'Dim Messaggi() As String = {"Inserire l'E-Mail"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Inserire l'e-mail"
                lblMessaggio.Visible = True
                Ok = False
            Else
                If txtEMail.Text.IndexOf(".") = -1 Or txtEMail.Text.IndexOf("@") = -1 Or txtEMail.Text.Length < 5 Then
                    'Dim Messaggi() As String = {"E-Mail non valida"}
                    'VisualizzaMessaggioInPopup(Messaggi, Me)
                    lblMessaggio.Text = "E-mail non valida"
                    lblMessaggio.Visible = True
                    Ok = False
                End If
            End If
        End If

        Return Ok
    End Function

    Protected Sub cmdRegistra_Click(sender As Object, e As EventArgs) Handles cmdRegistra.Click
        If EffettuaControlli() = False Then
            Exit Sub
        End If

        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(txtNick.Text.ToUpper.Trim) & "' And Anno=" & DatiGioco.AnnoAttuale
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                'Dim Messaggi() As String = {"Utente già esistente"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Utenza già esistente"
                lblMessaggio.Visible = True
                Rec.Close()

                ConnSQL.Close()

                DB = Nothing
                Exit Sub
            End If
            Rec.Close()

            Sql = "Select * From AppoGiocatori Where Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(txtNick.Text.ToUpper.Trim) & "' And Anno=" & DatiGioco.AnnoAttuale
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                'Dim Messaggi() As String = {"Utente già in fase di registrazione"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Utenza già in fase di registrazione"
                lblMessaggio.Visible = True
                Rec.Close()

                ConnSQL.Close()

                DB = Nothing
                Exit Sub
            End If
            Rec.Close()

            Sql = "Insert Into AppoGiocatori Values (" & _
                " " & DatiGioco.AnnoAttuale & ", " & _
                "'" & SistemaTestoPerDB(txtNick.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtPassword.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtCognome.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtNome.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtEMail.Text) & "' " & _
                ")"
            DB.EsegueSql(ConnSQL, Sql)

            Dim g As New GestioneMail
            Dim gf As New GestioneFilesDirectory

            ' Effettua controlli sulla presentazione dell'amico
            Dim TrovatoAmico As Integer = -1

            Sql = "Select * From Amici Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(txtEMail.Text).Trim.ToUpper & "' And Registrato='N'"
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                TrovatoAmico = Rec("CodGiocatore").Value
            End If
            Rec.Close()

            If TrovatoAmico <> -1 Then
                Sql = "Update Amici " & _
                    "Set Registrato='S' " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(txtEMail.Text).Trim.ToUpper & "' And Registrato='N'"
                DB.EsegueSql(ConnSQL, Sql)

                Sql = "Update Bilancio " & _
                    "Set Amici=Amici+3, TotVersamento=TotVersamento-3 " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & TrovatoAmico
                DB.EsegueSql(ConnSQL, Sql)

                g.InviaMailPerAmicoRegistrato(TrovatoAmico, txtNick.Text & " (" & txtNome.Text & " " & txtCognome.Text & ")")
            End If
            ' Effettua controlli sulla presentazione dell'amico

            g.InviaMailRegsitrazioneGiocatore(txtNick.Text, txtEMail.Text, txtNome.Text, txtCognome.Text)

            ConnSQL.Close()

            gf = Nothing
            g = Nothing

            'Dim sMessaggi() As String = {"E' stata inviata una richiesta di conferma all'E-Mail immessa"}
            'VisualizzaMessaggioInPopup(sMessaggi, Me)

            cmdRegistra.Visible = False
            txtNome.Text = ""
            txtCognome.Text = ""
            txtPassword.Text = ""
            txtEMail.Text = ""

            lblMessaggio.Text = "E' stata inviata una richiesta di conferma all'E-Mail immessa"
            lblMessaggio.Visible = True
        End If

        DB = Nothing

    End Sub

    Private Sub SalvaGiocatore(Nick As String)
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idGiocatore As Integer

            DatiGioco.AnnoAttuale = Request.QueryString("Anno")

            Sql = "Select Max(CodGiocatore)+1 From Giocatori Where Anno=" & DatiGioco.AnnoAttuale
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                idGiocatore = 1
            Else
                idGiocatore = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select * From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & SistemaTestoPerDB(Nick) & "'"

            'Response.Write(Sql)

            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                'Dim Messaggi() As String = {"Problemi nella registrazione dell'account"}
                'VisualizzaMessaggioInPopup(Messaggi, Me)
                lblMessaggio.Text = "Problemi nella registrazione dell'account"
                lblMessaggio.Visible = True
                Rec.Close()
                ConnSQL.Close()
                DB = Nothing
                Exit Sub
            End If

            Dim Nome As String = MetteMaiuscole(Rec("Nome").Value)
            Dim Cognome As String = MetteMaiuscole(Rec("Cognome").Value)
            Dim Mail As String = Rec("EMail").Value

            Sql = "Insert Into Giocatori Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & idGiocatore & ", " &
                "'" & SistemaTestoPerDB(Nick.ToUpper.Trim) & "', " &
                "'" & SistemaTestoPerDB(CriptaPassword(Rec("Password").Value)) & "', " &
                " " & DatiGioco.Giornata & ", " &
                "'" & SistemaTestoPerDB(Cognome) & "', " &
                "'" & SistemaTestoPerDB(Nome) & "', " &
                " " & Permessi.Giocatore & ", " &
                "'" & SistemaTestoPerDB(Mail) & "', " &
                "'N', " &
                "'', " &
                "'S', " &
                "'S' " &
                ")"
            Rec.Close()
            DB.EsegueSql(ConnSQL, Sql)

            Sql = "Delete From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & SistemaTestoPerDB(Nick) & "'"
            DB.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into DettaglioGiocatori Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & idGiocatore & ", " &
                "0, " &
                "0, " &
                "0, " &
                "0 " &
                ")"
            DB.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into NotificheGiocatori Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & idGiocatore & ", " &
                "'S' " &
                ")"
            DB.EsegueSql(ConnSQL, Sql)

            Dim Quante As Integer

            Quante = DatiGioco.Giornata
            Select Case DatiGioco.StatoConcorso
                Case ValoriStatoConcorso.Aperto
                Case ValoriStatoConcorso.Chiuso
                    Quante -= 1
                Case ValoriStatoConcorso.DaControllare
                    Quante -= 1
                Case ValoriStatoConcorso.Nessuno
            End Select

            If Quante < 0 Then
                Quante = 0
            End If

            Dim Totale As Single = ((NumeroGiornateTotali - Quante) * QuotaGiocoSettimanale) + QuotaPerSpeciali

            Sql = "Insert Into Bilancio Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & idGiocatore & ", " &
                " " & Totale.ToString.Replace(",", ".") & ", " &
                "0, " &
                "0, " &
                "0, " &
                "0 " &
                ")"
            DB.EsegueSql(ConnSQL, Sql)

            Dim g As New GestioneMail
            Dim gf As New GestioneFilesDirectory

            ' Effettua controlli sulla presentazione dell'amico
            Dim TrovatoAmico As Integer = -1

            Sql = "Select * From Amici Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(txtEMail.Text).Trim.ToUpper & "' And Registrato='N'"
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                TrovatoAmico = Rec("CodGiocatore").Value
            End If
            Rec.Close()

            If TrovatoAmico <> -1 Then
                Sql = "Update Amici " &
                    "Set Registrato='S' " &
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(txtEMail.Text).Trim.ToUpper & "' And Registrato='N'"
                DB.EsegueSql(ConnSQL, Sql)

                Sql = "Update Bilancio " &
                    "Set Amici=Amici+3, TotVersamento=TotVersamento-3 " &
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & TrovatoAmico
                DB.EsegueSql(ConnSQL, Sql)

                g.InviaMailPerAmicoRegistrato(TrovatoAmico, txtNick.Text & " (" & txtNome.Text & " " & txtCognome.Text & ")")
            End If
            ' Effettua controlli sulla presentazione dell'amico

            ' Prende eventuale immagine dell'anno precedente
            Dim vecchioID As String = ""

            Sql = "Select Giocatore From Giocatori Where (" &
                "(Cognome='" & SistemaTestoPerDB(Cognome) & "' And Nome='" & SistemaTestoPerDB(Nome) & "') Or Giocatore='" & SistemaTestoPerDB(Nick.ToUpper.Trim) & "') And Anno<" & DatiGioco.AnnoAttuale & " Order By anno Desc"
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                vecchioID = Rec("Giocatore").Value
            End If
            Rec.Close()

            If vecchioID <> "" Then
                Dim PathAnnoPrec As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & (DatiGioco.AnnoAttuale - 1).ToString.Trim & "\"
                Dim PathAnnoAtt As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & (DatiGioco.AnnoAttuale).ToString.Trim & "\"
                gf.CreaDirectoryDaPercorso(PathAnnoPrec)
                gf.CreaDirectoryDaPercorso(PathAnnoAtt)
                PathAnnoPrec += vecchioID.ToString.Trim & ".jpg"
                PathAnnoAtt += Nick.ToUpper.Trim & ".jpg"
                If File.Exists(PathAnnoPrec) = True Then
                    FileCopy(PathAnnoPrec, PathAnnoAtt)
                End If
            End If
            ' Prende eventuale immagine dell'anno precedente

            ConnSQL.Close()

            DB = Nothing

            g.InviaMailGiocatoreRegistrato(idGiocatore)

            gf = Nothing
            g = Nothing

            'Dim sMessaggi() As String = {"Giocatore registrato correttamente"}
            'VisualizzaMessaggioInPopup(sMessaggi, Me)

            cmdRegistra.Visible = False
            txtNick.Text = Nick
            txtNome.Text = ""
            txtCognome.Text = ""
            txtPassword.Text = ""
            txtEMail.Text = ""

            lblMessaggio.Text = "Giocatore registrato correttamente"
            lblMessaggio.Visible = True
        End If

        DB = Nothing
    End Sub

    Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        Response.Redirect("Default.aspx")
    End Sub

End Class