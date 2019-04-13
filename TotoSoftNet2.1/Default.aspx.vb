Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PercorsoApplicazione = Server.MapPath(".")
        PercorsoImmaginiRoot = Request.Url.AbsoluteUri
        Dim Pagina As String = Request.Url.AbsolutePath
        For i As Integer = Pagina.Length To 1 Step -1
            If Mid(Pagina, i, 1) = "/" Or Mid(Pagina, i, 1) = "\" Then
                Pagina = Mid(Pagina, i + 1, Pagina.Length)
                Exit For
            End If
        Next
        PercorsoImmaginiRoot = PercorsoImmaginiRoot.Replace(Pagina, "")
        If Right(PercorsoImmaginiRoot, 1) <> "/" Then
            PercorsoImmaginiRoot += "/"
        End If
        If PercorsoImmaginiRoot.ToUpper.IndexOf("LOCALHOST") > -1 Then
            PercorsoImmaginiRoot = ""
        End If

        If File.Exists(Server.MapPath(".") & "/MailDaLocalHost.dat") = True Then
            SitoInLocale = True
            IndirizzoSito = IndirizzoPerMailLocalHost
            EmailDiInvio = EmailLocalHost
        Else
            SitoInLocale = False
            IndirizzoSito = IndirizzoPerMailWEB
            EmailDiInvio = EMailWEB
        End If

        LetturaChiaviSuWebConfig()

        'ControllaModalitaDiLavoro(Server.MapPath(".") & "\ModalitaLavoro.txt")

        If Page.IsPostBack = False Then
            If Request.QueryString("NuovoAnno") <> "" Then
                Dim Messaggi() As String = {"Nuovo anno creato"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
            End If
            If Request.QueryString("Anno") <> "" Then
                DatiGioco.AnnoAttuale = Request.QueryString("Anno")
            End If

            RiempieComboAnni()

            IncrementaAccessi()

            Session("CodGiocatore") = ""
            Session("Nick") = ""
            Session("Cognome") = ""
            Session("Nome") = ""
            Session("Permesso") = ""
            Session("EMail") = ""
        End If

        VisualizzaContatore()
    End Sub

    Private Sub LetturaChiaviSuWebConfig()
        'CasellaDiPosta = ConfigurationManager.AppSettings("CasellaMail").ToString
        'PasswordCasellaDiPosta = ConfigurationManager.AppSettings("PasswordMail").ToString

        Dim PercorsoFile As String = "C:\AAA-SettaggiPerApp\PostaTOTOMIO.txt"

        Dim gf As New GestioneFilesDirectory
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
                Response.Write("Problemi nella scrittura del file di posta.<br /><br />" & NomeFile)
                Response.End()
            End If
        End If
        If riga.Contains(";") Then
            Dim campi() As String = riga.Split(";")

            If campi.Length > 1 Then
                If campi(0).Contains("UTENTE:") Then
                    CasellaDiPosta = campi(0).Replace("UTENTE:", "")
                Else
                    Response.Write("Il primo campo del file di configurazione della posta<br /><br />" & NomeFile & "<br /><br />deve contenere la stringa 'UTENTE:'<br /><br />Contenuto: " & riga)
                    Response.End()
                End If
                If campi(1).Contains("PASSWORD:") Then
                    PasswordCasellaDiPosta = campi(1).Replace("PASSWORD:", "")
                Else
                    Response.Write("Il secondo campo del file di configurazione della posta<br /><br />" & NomeFile & "<br /><br />deve contenere la stringa 'PASSWORD:'<br /><br />Contenuto: " & riga)
                    Response.End()
                End If
            Else
                Response.Write("Problemi nella lettura del file di posta.<br /><br />" & NomeFile & "<br /><br />Campi non validi: " & riga & "<br />Dovrebbero essere nel formato: UTENTE:looigi@gmail.com;PASSWORD:xxxxx")
                Response.End()
            End If
        Else
            Response.Write("Problemi nella lettura del file di posta.<br /><br />" & NomeFile & "<br /><br />Campi non validi: " & riga & "<br />Dovrebbero essere nel formato: UTENTE:looigi@gmail.com;PASSWORD:xxxxx")
            Response.End()
        End If
        gf = Nothing
    End Sub

    Private Sub VisualizzaContatore()
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select Sum(Accessi) From Accessi"
            Dim Quanti As String = "00000"
            Dim Accessi As Long

            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                Accessi = Rec(0).Value
                Do While Accessi > 99999
                    Accessi -= 99999
                Loop
                Quanti = Format(Accessi, "00000")
            End If
            Rec.Close()

            Dim Perc As String = "App_Themes/Standard/Images/Icone/"

            'img1.ImageUrl = Perc & Mid(Quanti, 1, 1) & ".png"
            'img2.ImageUrl = Perc & Mid(Quanti, 2, 1) & ".png"
            'img3.ImageUrl = Perc & Mid(Quanti, 3, 1) & ".png"
            'img4.ImageUrl = Perc & Mid(Quanti, 4, 1) & ".png"
            'img5.ImageUrl = Perc & Mid(Quanti, 5, 1) & ".png"

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Private Sub RiempieComboAnni()
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select * From Anni Order By Anno"
            Dim Massimo As Integer = 0
            Dim sAnno As String = ""

            Rec = DB.LeggeQuery(ConnSQL, Sql)
            cmbAnno.Items.Clear()
            Do Until Rec.Eof
                cmbAnno.Items.Add(Rec("Descrizione").Value)
                If Rec("Anno").Value > Massimo Then
                    Massimo = Rec("Anno").Value
                    sAnno = Rec("Descrizione").Value
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            If "" & Session("CodGiocatore") <> "" Then
                Sql = "Select * From AnniVisualizzati Where CodGiocatore=" & Session("CodGiocatore")
                Rec = DB.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    Massimo = Rec("AnnoVisualizzato").Value
                    Rec.Close()

                    Sql = "Select * From Anni Where Anno=" & Massimo
                    Rec = DB.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        sAnno = Rec("Descrizione").Value
                    End If
                End If
                Rec.Close()
            Else
                If Request.QueryString("Anno") <> "" Then
                    Massimo = Request.QueryString("Anno")

                    Sql = "Select * From Anni Where Anno=" & Massimo
                    Rec = DB.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        sAnno = Rec("Descrizione").Value
                    End If
                    Rec.Close()
                End If
            End If

            cmbAnno.Text = sAnno
            LeggeDatiDiGioco(Massimo)

            If DatiGioco.Giornata > 4 Then
                cmdReg.Visible = False
            End If

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Protected Sub cmdLogin_Click(sender As Object, e As EventArgs) Handles cmdLogin.Click
        Dim DB As New GestioneDB

        If txtUtente.Text.Trim = "" Then
            Dim Messaggi() As String = {"Inserire l'utente"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtPassword.Text.Trim = "" Then
            Dim Messaggi() As String = {"Inserire la password"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Password As String
            Dim PassInserita As String = txtPassword.Text.Replace("'", "''")
            Dim Utente As String = SistemaTestoPerDB(txtUtente.Text.ToUpper.Trim)
            Dim Anno As Integer = 1

            Sql = "Select * From Anni Where Descrizione='" & cmbAnno.Text & "'"
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Anno = Rec("Anno").Value
            End If
            Rec.Close()

            Sql = "Select * From Giocatori Where Upper(LTrim(Rtrim(Giocatore)))='" & Utente & "' And Anno=" & Anno
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Password = DecriptaPassword(Rec("Password").Value.ToString)
                If PassInserita = Password Then
                    Session("CodGiocatore") = Rec("CodGiocatore").Value
                    Session("Nick") = Rec("Giocatore").Value
                    Session("Cognome") = Rec("Cognome").Value
                    Session("Nome") = Rec("Nome").Value
                    Session("Permesso") = Rec("idTipologia").Value
                    Session("EMail") = Rec("EMail").Value

                    Rec.Close()

                    Sql = "Select * From AnniVisualizzati Where CodGiocatore=" & Session("CodGiocatore")
                    Rec = DB.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        Sql = "Update AnniVisualizzati Set AnnoVisualizzato=" & DatiGioco.AnnoAttuale & " Where CodGiocatore=" & Session("CodGiocatore")
                    Else
                        Sql = "Insert Into AnniVisualizzati Values (" & Session("CodGiocatore") & ", " & DatiGioco.AnnoAttuale & ")"
                    End If
                    Rec.Close()

                    DB.EsegueSql(ConnSQL, Sql)

                    ConnSQL.Close()

                    IncrementaAccessiGiocatore()

                    Response.Redirect("Principale.aspx")
                Else
                    Dim Messaggi() As String = {"Password non valida"}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                End If
            Else
                Dim Messaggi() As String = {"Utente non valido"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Private Sub IncrementaAccessiGiocatore()
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Quanti As Integer = 0

            Sql = "Select * From AccessiDett Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Sql = "Insert Into AccessiDett Values (" & DatiGioco.AnnoAttuale & ", " & Session("CodGiocatore") & ", 1, '" & ConverteData(Now) & "')"
            Else
                Quanti = Rec("Accessi").Value
                Sql = "Update AccessiDett Set Accessi=Accessi+1, Ultimo='" & ConverteData(Now) & "' Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            End If
            Rec.Close()

            DB.EsegueSql(ConnSQL, Sql)

            Select Case Quanti
                Case 100
                    Dim rd As New RiconoscimentiDisonori
                    Dim Ritorno As String

                    ' Controllo premio 28
                    Ritorno = rd.CreaRigaDaScrivere(DB, ConnSQL, 28, Session("CodGiocatore"), Session("Nick"))
                    ' Controllo premio 28
                Case 200
                    Dim rd As New RiconoscimentiDisonori
                    Dim Ritorno As String

                    ' Controllo premio 29
                    Ritorno = rd.CreaRigaDaScrivere(DB, ConnSQL, 29, Session("CodGiocatore"), Session("Nick"))
                    ' Controllo premio 29
                Case 500
                    Dim rd As New RiconoscimentiDisonori
                    Dim Ritorno As String

                    ' Controllo premio 30
                    Ritorno = rd.CreaRigaDaScrivere(DB, ConnSQL, 30, Session("CodGiocatore"), Session("Nick"))
                    ' Controllo premio 30
                Case 1000
                    Dim rd As New RiconoscimentiDisonori
                    Dim Ritorno As String

                    ' Controllo premio 31
                    Ritorno = rd.CreaRigaDaScrivere(DB, ConnSQL, 31, Session("CodGiocatore"), Session("Nick"))
                    ' Controllo premio 31
            End Select

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Private Sub IncrementaAccessi()
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Accessi Where Anno=" & DatiGioco.AnnoAttuale
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Sql = "Insert Into Accessi Values (" & DatiGioco.AnnoAttuale & ", 1)"
            Else
                Sql = "Update Accessi Set Accessi=Accessi+1 Where Anno=" & DatiGioco.AnnoAttuale
            End If
            Rec.Close()

            DB.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Protected Sub cmdReg_Click(sender As Object, e As EventArgs) Handles cmdReg.Click
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim vAnno As Integer

            Sql = "Select * From Anni Where Descrizione='" & SistemaTestoPerDB(cmbAnno.Text) & "'"
            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                vAnno = Rec("Anno").Value
            End If
            Rec.Close()

            Response.Redirect("NuovoUtente.aspx?Anno=" & vAnno)

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Protected Sub cmdRicorda_Click(sender As Object, e As EventArgs) Handles cmdRicorda.Click
        If txtUtente.Text = "" Then
            Dim Messaggi() As String = {"Inserire l'utente"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim gm As New GestioneMail
        Dim g As New Giocatori
        Dim id As Integer = g.TornaIdGiocatore(txtUtente.Text)
        If id <> -1 Then
            gm.InviaMailRicordaPassword(id)

            Dim Messaggi() As String = {"La password è stata inviata all'account di posta predefinito per l'utente"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        Else
            Dim Messaggi() As String = {"Utente non registrato nel sistema"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If
        g = Nothing
        gm = Nothing
    End Sub

    Protected Sub cmbAnno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAnno.SelectedIndexChanged
        Dim DB As New GestioneDB
        Dim Anno As Integer = -1

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select * From Anni Where Descrizione='" & cmbAnno.Text & "'"

            Rec = DB.LeggeQuery(ConnSQL, Sql)
            Anno = Rec("Anno").Value()
            Rec.Close()

            LeggeDatiDiGioco(Anno)

            ConnSQL.Close()
        End If

        DB = Nothing

        If Anno <> -1 Then
            Response.Redirect("Default.aspx?Anno=" & Anno)
        End If
    End Sub

    Protected Sub cmdRegolamento_Click(sender As Object, e As EventArgs) Handles cmdRegolamento.Click
        Response.Redirect("RegolamentoEsterno.aspx")
    End Sub
End Class