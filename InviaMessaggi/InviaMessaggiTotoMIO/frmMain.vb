Imports System.Net.Mail
Imports System.IO
Imports System.Threading

Public Class frmMain
    Private NotifyIcon1 As NotifyIcon = New NotifyIcon
    Private UltimoInvio As String
    Private myMenu As New ContextMenu()
    Private mnuApreMaschera As New GestioneMenu
    Private mnuUscita As New GestioneMenu

    Private screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
    Private screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height

    Private trd As Thread

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        mnuApreMaschera = New GestioneMenu("Verdana", 12, "Apre maschera", "Icone\Maschera_si.png", 24, New EventHandler(AddressOf mnuApre_Click), Nothing)
        mnuUscita = New GestioneMenu("Verdana", 12, "Uscita", "Icone\Uscita.png", 24, New EventHandler(AddressOf mnuUscita_Click), Nothing)

        myMenu.MenuItems.Add(mnuApreMaschera)
        myMenu.MenuItems.Add(mnuUscita)

        NotifyIcon1.Icon = New Icon("Icone\pr_in016.ico")
        NotifyIcon1.Text = "Invia messaggi"
        NotifyIcon1.ContextMenu = myMenu
        NotifyIcon1.Visible = True
    End Sub

    Private Sub EsegueInizio()
        Dim gf As New GestioneFilesDirectory
        Dim Campi() As String = gf.LeggeFileIntero(Application.StartupPath & "\file.connessione").Split("§")
        StringaConnessione = Campi(0)

        Dim PercorsoFile As String = "C:\AAA-SettaggiPerApp\PostaTOTOMIO.txt"

        Dim Cartella As String = gf.TornaNomeDirectoryDaPath(PercorsoFile) & "\"
        gf.CreaDirectoryDaPercorso(Cartella)
        Dim NomeFile As String = PercorsoFile
        Dim riga As String = ""
        If File.Exists(NomeFile) Then
            riga = gf.LeggeFileIntero(NomeFile)
        Else
            riga = "UTENTE:staff.totomio@gmail.com;PASSWORD:TotoMio227!"
            gf.CreaAggiornaFile(PercorsoFile, riga)
            If Not File.Exists(NomeFile) Then
                MsgBox("Problemi nella scrittura del file di posta.<br /><br />" & NomeFile)
                End
            End If
        End If
        If riga.Contains(";") Then
            Dim campi2() As String = riga.Split(";")

            If campi2.Length > 1 Then
                If campi2(0).Contains("UTENTE:") Then
                    CasellaDiPosta = campi2(0).Replace("UTENTE:", "")
                Else
                    MsgBox("Il primo campo del file di configurazione della posta<br /><br />" & NomeFile & "<br /><br />deve contenere la stringa 'UTENTE:'<br /><br />Contenuto: " & riga)
                    End
                End If
                If campi2(1).Contains("PASSWORD:") Then
                    PasswordCasellaDiPosta = campi2(1).Replace("PASSWORD:", "")
                Else
                    MsgBox("Il secondo campo del file di configurazione della posta<br /><br />" & NomeFile & "<br /><br />deve contenere la stringa 'PASSWORD:'<br /><br />Contenuto: " & riga)
                    End
                End If
            Else
                MsgBox("Problemi nella lettura del file di posta.<br /><br />" & NomeFile & "<br /><br />Campi non validi: " & riga & "<br />Dovrebbero essere nel formato: UTENTE:looigi@gmail.com;PASSWORD:xxxxx")
                End
            End If
        Else
            MsgBox("Problemi nella lettura del file di posta.<br /><br />" & NomeFile & "<br /><br />Campi non validi: " & riga & "<br />Dovrebbero essere nel formato: UTENTE:looigi@gmail.com;PASSWORD:xxxxx")
            End
        End If
        gf = Nothing
        If CasellaDiPosta = "" Or PasswordCasellaDiPosta = "" Then
            MsgBox("Utenza di posta non valida")
            End
        End If

        PercorsoApplicazione = "C:\inetpub\wwwroot\TotoMioII"

        ModalitaTest = ControllaTest()
        LeggeAnnoInCorso()
        ControlloEsistenzaTabella()
        LeggeChiusuraConcorso()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer3.Enabled = True

        Iconizzato = True

        Me.Left = -300
        Me.Top = -300
        Me.ShowInTaskbar = False
    End Sub

    Private Sub mnuApre_Click()
        If Iconizzato Then
            mnuApreMaschera.ImpostaImmagine("Icone\Maschera_no.png", 24)
            mnuApreMaschera.ImpostaTesto("Chiude maschera")
            Me.Left = (screenWidth / 2) - (Me.Width / 2)
            Me.Top = (screenHeight / 2) - (Me.Height / 2)
            Me.ShowInTaskbar = True
            Iconizzato = False
        Else
            mnuApreMaschera.ImpostaImmagine("Icone\Maschera_si.png", 24)
            mnuApreMaschera.ImpostaTesto("Apre maschera")
            Me.Left = -300
            Me.Top = -300
            Me.ShowInTaskbar = False
            Iconizzato = True
        End If
    End Sub

    Private Sub mnuUscita_Click()
        If MsgBox("Si vuole uscire ?", vbYesNo + vbInformation + vbDefaultButton1) = vbYes Then
            NotifyIcon1.Visible = False
            NotifyIcon1 = Nothing
            End
        End If
    End Sub

    Private Function ControllaTest() As Boolean
        If File.Exists(Application.StartupPath & "\Test.txt") Then
            lblTest.Text = "Modalità di test"
            Return True
        Else
            lblTest.Text = ""
            Return False
        End If
    End Function

    Private Sub ControlloEsistenzaTabella()
        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        If Not S.ControlloEsistenzaTabella(db, "InvioMessaggi") Then
            Dim Sql As String = "CREATE TABLE [dbo].[NotificheGiocatori](" &
                "[idAnno] [int] NOT NULL, " &
                "[CodGiocatore] [int] NOT NULL, " &
                "[Attivo] [varchar](1) NULL, " &
                "CONSTRAINT [PK_NotificheGiocatori] PRIMARY KEY CLUSTERED " &
                "( " &
                "[idAnno] ASC, " &
                "[CodGiocatore] Asc " &
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " &
                ") ON [PRIMARY]"
            S.EsegueSql(db, Sql)

            Dim rec As Object = S.LeggeQuery(db, "Select Distinct CodGiocatore From Giocatori Where Anno=" & Anno)
            Do Until rec.eof
                Sql = "Insert Into NotificheGiocatori Values (" & Anno & ", " & rec("CodGiocatore").Value & ", 'S')"
                S.EsegueSql(db, Sql)

                rec.movenext()
            Loop
            rec.close()
        End If
        db = Nothing
    End Sub

    Private Sub LeggeAnnoInCorso()
        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        Dim rec As Object = S.LeggeQuery(db, "Select * From Anni Order By Anno Desc")
        Anno = rec("Anno").Value
        lblCampionato.Text = rec("Descrizione").Value
        rec.close()
        S = Nothing
    End Sub

    Private Function ScriveTermine() As String
        Dim Ritorno As String = ""

        Dim d As Long = DateDiff(DateInterval.Second, Now, DataChiusura)
        If d < 0 Then
            Timer1.Enabled = False
            lblStatoConcorso.Text = "Chiuso"
            lblScadenza.Text = ""
            Ritorno = "Terminato"
            InviaMailDiChiusuraConcorso()
            Timer2.Enabled = True
        Else
            Differenza = TimeSpan.FromSeconds(d)
            DifferenzaTS = New DateTime(Differenza.Ticks)
            Ritorno = Format(DifferenzaTS.Day - 1, "00") & " " & Format(DifferenzaTS.Hour, "00") & ":" & Format(DifferenzaTS.Minute, "00") & ":" & Format(DifferenzaTS.Second, "00")
        End If

        Return Ritorno
    End Function

    Private Sub LeggeChiusuraConcorso()
        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        Dim rec As Object = S.LeggeQuery(db, "Select * From DatiDiGioco Where Anno=" & Anno)
        Timer1.Enabled = False
        Timer2.Enabled = False
        lblScadenza.Text = ""
        lblGiornata.Text = rec("Giornata").Value
        Giornata = rec("Giornata").Value
        If Not ModalitaTest Then
            cmdInvia.Enabled = False
        End If
        lblTermine.Text = ""
        Select Case rec("Stato").value
            Case "1"
                ' Concorso aperto
                DataChiusura = rec("ChiusuraConcorso").Value
                If DataChiusura <= Now Then
                    ' Chiuso
                    DataChiusura = Nothing
                    lblStatoConcorso.Text = "Chiuso"
                    Timer2.Enabled = True
                Else
                    lblStatoConcorso.Text = "Aperto"
                    Timer1.Enabled = True
                    lblScadenza.Text = Format(DataChiusura.Day, "00") & "/" & Format(DataChiusura.Month, "00") & "/" & DataChiusura.Year & " " & Format(DataChiusura.Hour, "00") & ":" & Format(DataChiusura.Minute, "00") & ":" & Format(DataChiusura.Second, "00")
                    lblTermine.Text = ScriveTermine()
                    If Not ModalitaTest Then
                        cmdInvia.Enabled = True
                    End If
                End If
            Case "2"
                ' Da controllare
                DataChiusura = Nothing
                lblStatoConcorso.Text = "Da controllare"
                Timer2.Enabled = True
            Case "3"
                ' Chiuso
                DataChiusura = Nothing
                lblStatoConcorso.Text = "Chiuso"
                Timer2.Enabled = True
        End Select
        StatoConcorso = lblStatoConcorso.Text
        rec.close()

        Dim Sql As String = "Select * From PartiteSpeciali Where idAnno=" & Anno & " And idGiornata Is Not Null"
        rec = S.LeggeQuery(db, Sql)
        If rec.Eof = True Then
            lblSpeciale.Text = "No"
        Else
            lblSpeciale.Text = "Sì"
        End If
        rec.Close()

        S = Nothing
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If StatoConcorso = "Aperto" Then
            lblTermine.Text = ScriveTermine()

            Dim Ok As Boolean = False
            Dim OraAttuale As String = Format(Now.Hour, "00") & Format(Now.Minute, "00")
            Dim Tipo As Integer

            If DifferenzaTS.Day - 1 > 2 Then
                If OraAttuale = "0000" Then
                    If UltimoInvio <> OraAttuale Then
                        UltimoInvio = OraAttuale
                        Ok = True
                        Tipo = 1
                    End If
                End If
            Else
                If DifferenzaTS.Day - 1 <= 2 And DifferenzaTS.Day - 1 > 0 Then
                    If OraAttuale = "0000" Or OraAttuale = "1200" Then
                        If UltimoInvio <> OraAttuale Then
                            UltimoInvio = OraAttuale
                            Ok = True
                            Tipo = 2
                        End If
                    End If
                Else
                    If (OraAttuale = "0000" Or OraAttuale = "0600" Or OraAttuale = "1200" Or OraAttuale = "1800") And DifferenzaTS.Hour > 10 Then
                        If UltimoInvio <> OraAttuale Then
                            UltimoInvio = OraAttuale
                            Ok = True
                            Tipo = 2
                        End If
                    Else
                        If (OraAttuale = "0000" Or OraAttuale = "0600" Or OraAttuale = "1200" Or OraAttuale = "1800") And DifferenzaTS.Hour < 10 And DifferenzaTS.Hour > 2 Then
                            If UltimoInvio <> OraAttuale Then
                                UltimoInvio = OraAttuale
                                Ok = True
                                Tipo = 2
                            End If
                        Else
                            If DifferenzaTS.Hour <= 2 Then
                                If Strings.Right(OraAttuale, 1) = "9" Then
                                    If UltimoInvio <> OraAttuale Then
                                        UltimoInvio = OraAttuale
                                        Ok = True
                                        Tipo = 3
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If Ok Then
                InviaMessaggi(Tipo)
            End If
        End If
    End Sub

    Private Sub InviaMessaggi(Tipo As Integer)
        Dim Sql As String = "Select A.CodGiocatore, A.Giocatore, A.EMail From Giocatori A " &
            "Left Join NotificheGiocatori B On A.Anno = B.idAnno And A.CodGiocatore = B.CodGiocatore " &
            "Where A.Anno=" & Anno & " And B.Attivo='S' And A.CodGiocatore Not In ( " &
            "Select Distinct CodGiocatore From Pronostici " &
            "Where Anno=" & Anno & " And Giornata=" & Giornata & ")"

        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        Dim sEmail() As String
        Dim q As Integer = 0

        If ModalitaTest Then
            ReDim Preserve sEmail(q)
            sEmail(q) = "looigi@gmail.com"
            q += 1
        Else
            Dim rec As Object = S.LeggeQuery(db, Sql)
            Do Until rec.eof
                ReDim Preserve sEmail(q)
                sEmail(q) = rec("Email").Value
                q += 1

                rec.movenext()
            Loop
            rec.close()
        End If
        S = Nothing

        If q > 0 Then
            Dim Speciale As Boolean

            If lblSpeciale.Text = "No" Then
                Speciale = False
            Else
                Speciale = True
            End If

            Select Case Tipo
                Case 1
                    InviaMailAvvisoChiusuraConcorsoSoft(lblGiornata.Text, sEmail, "Looigi", Speciale)
                Case 2
                    InviaMailAvvisoChiusuraConcorsoHard(lblGiornata.Text, sEmail, "Looigi", Speciale)
                Case 3
                    InviaMailAvvisoChiusuraConcorsoLast(lblGiornata.Text, sEmail, "Looigi", Speciale)
            End Select
        End If
    End Sub

    Private Sub InviaMailDiChiusuraConcorso()
        Dim Speciale As Boolean

        If lblSpeciale.Text = "No" Then
            Speciale = False
        Else
            Speciale = True
        End If

        Dim Testo As String = ""
        Dim EMail As String = ""

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "AVVISO AUTOMATICO<hr />Il concorso " & Altro & "TotoMIO numero " & lblGiornata.Text & " ha terminato la propria fase di inserimento.<br />"
        Testo += "Chi non ha compilato la schedina e' stato automaticamente tappato. Arriverà la mail con il dettaglio della chiusura.<br /><br />"
        Testo += "<hr />L'amministratore TotoMio: Looigi"

        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        Dim sEmail() As String
        Dim q As Integer = 0

        If ModalitaTest Then
            ReDim Preserve sEmail(q)
            sEmail(q) = "looigi@gmail.com"
        Else
            Dim rec As Object = S.LeggeQuery(db, "Select Email From Giocatori Where Anno=" & Anno)
            Do Until rec.eof
                ReDim Preserve sEmail(q)
                sEmail(q) = rec("Email").Value
                q += 1

                rec.movenext()
            Loop
            rec.close()
        End If
        S = Nothing

        For i As Integer = 0 To UBound(sEmail)
            EMail += sEmail(i) & ";"
        Next

        InviaMailTramiteGmail(EmailDiInvio, EMail, "", "TotoMIO - Avviso termine inserimento su concorso " & lblGiornata.Text, Testo)
    End Sub

    Private Sub InviaMailAvvisoChiusuraConcorsoSoft(Numero As Integer, sEmail() As String, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "AVVISO AUTOMATICO<hr />E' in essere il concorso " & Altro & "TotoMIO numero " & Numero & ".<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & lblScadenza.Text & ") per evitare spiacevoli problemi di 'tappaggio'.<br /><br />"

        Testo += "<hr />L'amministratore TotoMio: " & Amministratore

        For i As Integer = 0 To UBound(sEmail)
            EMail += sEmail(i) & ";"
        Next

        InviaMailTramiteGmail(EmailDiInvio, EMail, "", "TotoMIO - Avviso concorso in essere " & Numero, Testo)
    End Sub

    Private Sub InviaMailAvvisoChiusuraConcorsoHard(Numero As Integer, sEmail() As String, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "AVVISO AUTOMATICO<hr />Si ricorda che è in essere il concorso " & Altro & "TotoMIO numero " & Numero & " ed è vicina la data di chiusura.<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & lblScadenza.Text & ") per evitare spiacevoli problemi di 'tappaggio'." & ChiudeTesto() & "<br />"
        Testo += "<br />" & ApreTestoGrande() & "Si ricorda che anche una eventuale riapertura del concorso e una successiva compilazione della colonna propria, NON toglierà eventuali tappi immessi in modo automatico dopo la chiusura." & ChiudeTesto() & "<br /><br />"

        Testo += ApreTesto() & "<hr />L'amministratore TotoMio: " & Amministratore & ChiudeTesto()

        For i As Integer = 0 To UBound(sEmail)
            EMail += sEmail(i) & ";"
        Next

        InviaMailTramiteGmail(EmailDiInvio, EMail, "", "TotoMIO - Ultimo avviso chiusura concorso " & Numero, Testo)
    End Sub

    Private Sub InviaMailAvvisoChiusuraConcorsoLast(Numero As Integer, sEmail() As String, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "AVVISO AUTOMATICO<hr />Fra pochi minuti verrà chiuso il concorso " & Altro & "TotoMIO numero " & Numero & ".<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & lblScadenza.Text & ") per evitare spiacevoli problemi di 'tappaggio'." & ChiudeTesto() & "<br />"
        Testo += "<br />" & ApreTestoGrande() & "Si ricorda che anche una eventuale riapertura del concorso e una successiva compilazione della colonna propria, NON toglierà eventuali tappi immessi in modo automatico dopo la chiusura." & ChiudeTesto() & "<br /><br />"

        Testo += ApreTesto() & "<hr />L'amministratore TotoMio: " & Amministratore & ChiudeTesto()

        For i As Integer = 0 To UBound(sEmail)
            EMail += sEmail(i) & ";"
        Next

        InviaMailTramiteGmail(EmailDiInvio, EMail, "", "TotoMIO - Ultimo avviso chiusura concorso " & Numero, Testo)
    End Sub

    Private Sub InviaMailTramiteGmail(ByVal MailFrom As String, ByVal MailTo As String, ByVal MailCc As String, ByVal MailSubject As String, ByVal MailTesto As String)
        Try
            Dim Mail As New MailMessage
            Dim sMailTesto As String = MailTesto

            sMailTesto = ImmagineDiTestaMail & sMailTesto

            Mail.From = New MailAddress(CasellaDiPosta)

            Dim Destinatari() As String = MailTo.Split(";")
            For i As Integer = 0 To Destinatari.Length - 1
                If Destinatari(i) <> "" Then
                    Mail.To.Add(Destinatari(i))
                End If
            Next
            Mail.Subject = "[TotoMIO] " & MailSubject

            Dim Oggetto As String = ""

            Oggetto = MailSubject

            Dim ImmSfondo As String = PercorsoImmaginiRoot & "App_Themes/Standard/Images/bg.jpg"
            Dim Tutto As String = ""

            Tutto += "<html>"
            Tutto += "  <body>"
            Tutto += "      <form id=""frmMain"">"
            Tutto += "          <div style=""background: url('" & ImmSfondo & "'); padding: 13px; margin: -9px;"">"
            Tutto += "              <div style=" & Chr(34) & "width: 100%; text-align:center;" & Chr(34) & ">"
            Tutto += "                  <div style=" & Chr(34) & PrendeStilePerMail() & Chr(34) & ">"
            Tutto += "                      <center>"
            Tutto += sMailTesto
            Tutto += "<hr />" & ApreTesto() & IndirizzoSito & "" & "<br />" & Now & ChiudeTesto()
            Tutto += "                      </center>"
            Tutto += "                  </div>"
            Tutto += "              </div>"
            Tutto += "          </div>"
            Tutto += "      </form>"
            Tutto += "  </body>"
            Tutto += "</html>"

            Mail.IsBodyHtml = True
            Mail.Body = Tutto

            System.Net.ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf RemoteServerCertificateValidationCallback)

            Dim SMTP As New SmtpClient("smtp.gmail.com")

            SMTP.UseDefaultCredentials = False
            SMTP.DeliveryMethod = SmtpDeliveryMethod.Network
            SMTP.Credentials = New System.Net.NetworkCredential(CasellaDiPosta, PasswordCasellaDiPosta)
            SMTP.Port = "587"
            SMTP.EnableSsl = True

            SMTP.Send(Mail)
        Catch ex As Exception

        End Try

        InviaMessaggiPerApp(MailTo, MailTesto)
    End Sub

    Private Function RemoteServerCertificateValidationCallback(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Console.WriteLine(certificate)
        Return True
    End Function

    Private Function TornaIdGiocatoreDaMail(Mail As String) As Integer
        Dim id As Integer = -1
        Dim S As New SQLSERVER
        S.ImpostaConnessioneDirettamente(StringaConnessione)
        Dim db As Object = S.ApreDB
        Dim rec As Object = S.LeggeQuery(db, "Select * From Giocatori Where Anno=" & Anno & " And EMail='" & Mail.Replace("'", "''") & "'")
        If Not rec.eof Then
            id = rec("CodGiocatore").Value
        End If
        rec.close()
        S = Nothing

        Return id
    End Function

    Private Sub InviaMessaggiPerApp(A As String, Testo As String)
        Dim Destinatari() As String = A.Split(";")
        Dim Indirizzo As String
        Dim gf As New GestioneFilesDirectory
        Dim Contenuto As String
        Dim Percorso As String = PercorsoApplicazione & "\MessaggiApp\" & Anno & "\"
        Dim sTesto As String = ToglieTag(Testo)

        gf.CreaDirectoryDaPercorso(Percorso)

        For Each Nome As String In Destinatari
            Dim id As Integer = TornaIdGiocatoreDaMail(Nome)
            If id <> -1 Then
                Indirizzo = Percorso & id.ToString.Trim & ".txt"

                If File.Exists(Indirizzo) Then
                    Contenuto = gf.LeggeFileIntero(Indirizzo)
                    gf.EliminaFileFisico(Indirizzo)
                Else
                    Contenuto = ""
                End If
                Contenuto &= sTesto & "§"

                gf.CreaAggiornaFile(Indirizzo, Contenuto)
            End If
        Next
    End Sub

    Private Function ToglieTag(Testo As String) As String
        Dim sTesto As String = Testo
        Dim inizio As Integer = -1
        Dim fine As Integer = -1
        Dim Appoggio() As String
        Dim quanti As Integer = 0

        sTesto = sTesto.Replace("<br />", "***ACAPO***").Replace("<br/>", "***ACAPO***")
        sTesto = sTesto.Replace("<hr />", "***ACAPO***").Replace("<hr/>", "***ACAPO***")
        sTesto = sTesto.Replace("§", "_")
        For i As Integer = 1 To sTesto.Length
            If inizio = -1 Then
                If Mid(sTesto, i, 1) = "<" Then
                    inizio = i
                End If
            Else
                If Mid(sTesto, i, 1) = ">" Then
                    fine = i
                End If

                If inizio > -1 And fine > -1 Then
                    quanti += 1
                    ReDim Preserve Appoggio(quanti)
                    Appoggio(quanti) = Mid(sTesto, inizio, fine - inizio + 1)

                    inizio = -1
                    fine = -1
                End If
            End If
        Next

        For i As Integer = 1 To quanti
            sTesto = sTesto.Replace(Appoggio(i), " ")
        Next

        Do While sTesto.Contains("  ")
            sTesto = sTesto.Replace("  ", " ")
        Loop

        Return sTesto
    End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        LeggeChiusuraConcorso()
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Timer3.Enabled = False

        EsegueInizio()
    End Sub

    Private Sub cmdInvia_Click(sender As Object, e As EventArgs) Handles cmdInvia.Click
        Dim Tipo As Integer

        Dim Ritorno As String = InputBox("1-Soft, 2-Hard, 3-Last, 4-Chiusura")
        If Ritorno <> "" Then
            If Ritorno = "1" Or Ritorno = "2" Or Ritorno = "3" Then
                Tipo = Val(Ritorno)
                InviaMessaggi(Tipo)

                MsgBox("Messaggio inviato", vbInformation)
            Else
                If Ritorno = "4" Then
                    InviaMailDiChiusuraConcorso()

                    MsgBox("Messaggio inviato", vbInformation)
                Else
                    MsgBox("Valore non valido", vbInformation)
                End If
            End If
        End If
    End Sub
End Class
