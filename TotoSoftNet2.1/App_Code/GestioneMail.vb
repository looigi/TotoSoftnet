Imports System.IO
Imports System.Net.Mail
Imports System.Security.Cryptography.X509Certificates

Public Class GestioneMail
    Private Logo As String = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/logo.png"
    Private ImmagineDiTestaMail As String = "<img src='" & Logo & "' width='400px' height='150px' /><hr />"

    Public Function ApreTestoTitolo() As String
        Return "<span style=""border: double; border-color: #423030;font-family:Tahoma; background-color: #EFCCCC;font-size: 13px; color: #AA0000; font-weight:bold; display: block; padding: 3px;"">"
    End Function

    Public Function ApreTesto() As String
        Return "<span style=""font-family:Tahoma; font-size: 13px; color: #000000; font-weight:bold;"">"
    End Function

    Public Function ApreTestoVerde() As String
        Return "<span style=""font-family:Tahoma; font-size: 13px; color: #00AA00; font-weight:bold;"">"
    End Function

    Public Function ApreTestoVerdeScuro() As String
        Return "<span style=""font-family:Tahoma; font-size: 13px; color: #3F8C55; font-weight:bold;"">"
    End Function

    Public Function ApreTestoMess() As String
        Return "<span style=""font-family:Tahoma; font-size: 13px; color: #0000AA; font-weight:bold; font-style:italic; "">"
    End Function

    Public Function ApreTestoGrande() As String
        Return "<span style=""font-family:Tahoma; font-size: 16px; color: #AA0000; font-weight:bold;"">"
    End Function

    Public Function ChiudeTesto() As String
        Return "</span>"
    End Function

    Public Function ApreTabella() As String
        Return "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & " style=""vertical-align: top; ?^?^LARGH?^?^ text-align: center;"">"
    End Function

    Public Function RitornaImmagineSquadra(Squadra As String, Optional SoloPath As Boolean = False) As String
        Dim Percorso As String
        Dim PercorsoTond As String

        ' Controllo sulla tondità dell'immagine
        Percorso = PercorsoApplicazione.Replace("\", "/") & "/App_Themes/Standard/Images/Stemmi/" & Squadra & ".Jpg"
        PercorsoTond = PercorsoApplicazione.Replace("\", "/") & "/App_Themes/Standard/Images/Stemmi/" & Squadra & "_Tonda.Jpg"
        If File.Exists(PercorsoTond) = False And File.Exists(Percorso) = False Then
            ' Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Stemmi/Niente.png"
            Percorso = "App_Themes/Standard/Images/Stemmi/Niente.png"
        Else
            If File.Exists(PercorsoTond) = False And File.Exists(Percorso) = True Then
                Dim i As New GestioneImmagini

                FileCopy(Percorso, PercorsoTond)
                i.Ridimensiona(PercorsoTond, PercorsoTond & ".jpg", 120, 120)
                i.RidimensionaEArrotondaIcona(PercorsoTond & ".jpg")
                i = Nothing
                Kill(PercorsoTond)

                Dim p2 As String = PercorsoApplicazione.Replace("\", "/") & "/App_Themes/Standard/Images/Stemmi/Backup/" & Squadra & ".Jpg"

                Try
                    MkDir(PercorsoApplicazione.Replace("\", "/") & "/App_Themes/Standard/Images/Stemmi/Backup")
                Catch ex As Exception

                End Try
                FileCopy(Percorso, p2)
                Kill(Percorso)

                FileCopy(PercorsoTond & ".jpg", PercorsoTond)
                Kill(PercorsoTond & ".jpg")
                Percorso = PercorsoTond
            Else
                Percorso = PercorsoTond
            End If
        End If
        ' Controllo sulla tondità dell'immagine

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
            Percorso = Percorso.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
        Else
        End If

        Dim Ritorno As String

        If SoloPath = False Then
            If Percorso.IndexOf(PercorsoImmaginiRoot) = -1 Then
                Percorso = PercorsoImmaginiRoot & Percorso
            End If
            Percorso = Percorso.Replace("\", "/").Replace("'", "''").Replace(" ", "%20")

            Ritorno = "<img src='" & Percorso & "' width='30px' height='30px' />"
        Else
            Ritorno = Percorso
        End If

        Return Ritorno
    End Function

    Public Function RitornaImmagineGiocatore(Giocatore As String, Optional SoloPath As Boolean = False) As String
        Dim Percorso As String

        If Giocatore.Trim.ToUpper = "RIPOSO" Or Giocatore.Trim.ToUpper = "MORTO" Then
            Percorso = PercorsoImmaginiRoot & RitornaImmagine(PercorsoApplicazione, "Riposo")
        Else
            Percorso = PercorsoImmaginiRoot & RitornaImmagine(PercorsoApplicazione, Giocatore)
        End If

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String

        If SoloPath = True Then
            Ritorno = Percorso
        Else
            Ritorno = "<img src='" & Percorso & "' width='30px' height='30px' />"
        End If

        Return Ritorno
    End Function

    Public Function RitornaImmagineScontriDiretti() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Campionato.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineFilRouge() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Fortuna.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmaginePippettero() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Pippettero.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineQuoteCUP() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/quotecup.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineCItalia() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/CoppaItalia.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineSupercoppaItaliana() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/SupercoppaItaliana.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineSupercoppaEuropea() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/SupercoppaEuropea.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineCoppe() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Coppe.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineIntertoto() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/intertoto.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineELeague() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/EuropaLeague.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Function RitornaImmagineChampions() As String
        Dim Percorso As String

        Percorso = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Champions.png"

        If Percorso.ToUpper.IndexOf("HTTP") = -1 Then
            Percorso = Percorso.Replace("/", "\")
        End If

        Dim Ritorno As String = "<img src='" & Percorso & "' width='30px' height='30px' />"

        Return Ritorno
    End Function

    Public Sub InviaMailPerRiconoscimentoDisonore(aChi As Integer, idPremio As Integer)
        Dim Testo As String = ""
        Dim G As New Giocatori
        Dim Utente As String = G.TornaNickGiocatore(aChi)
        Dim EMail As String = G.TornaMailGiocatore(aChi)
        G = Nothing

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Punti As Single = 0
            Dim Descrizione As String = ""
            Dim Immagine As String = ""

            Sql = "Select * From RiconDison Where idPremio=" & idPremio
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
            Else
                Punti = Rec("Punti").Value
                Descrizione = Rec("Descrizione").Value
                Immagine = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value
            End If
            Rec.Close()
            ConnSQL.Close()

            If Descrizione <> "" Then
                If Punti > 0 Then
                    Testo = ApreTesto() & "Hai ottenuto un riconoscimento che ti porterà dei punti in classifica generale:<br /><br />"
                Else
                    Testo = ApreTesto() & "Hai ottenuto un disonore:<br /><br />"
                End If
                Testo += "<img src='" & Immagine & "' width='50px' height='50px' />"
                Testo += "&nbsp;&nbsp;" & ChiudeTesto() & ApreTestoVerde() & Descrizione & ChiudeTesto() & ApreTesto()
                Testo += "&nbsp;&nbsp;<img src='" & Immagine & "' width='50px' height='50px' /><br />"
                If Punti > 0 Then
                    Testo += "Tutto ciò, ti porterà " & Punti & " punti in più"
                End If
            End If

            InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Riconoscimenti / Disonori", Testo)
        End If

        Db = Nothing
    End Sub

    Public Sub InviaMailAperturaNuovoConcorso(Numero As Integer, Scadenza As String, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale"
        End If

        Testo = ApreTesto() & "E' stato aperto il concorso " & Altro & " TotoMIO numero " & Numero & " con scadenza " & Scadenza & ".<br />"
        Testo += "Si prega di compilare la propria colonna prima della scadenza per evitare spiacevoli problemi di 'tappaggio'.<br /><br />"

        Dim sTesto As String = ""

        If Speciale = False Then
            Testo += "Eventi della giornata:<hr />"

            Dim ev As New Eventi
            Dim g As Integer

            g = DatiGioco.Giornata
            sTesto = ev.ControllaPresenzaEventi(True, g)
            sTesto = sTesto.Replace(" - ", "<br />")

            ev = Nothing
        Else
            sTesto = ""
        End If

        Testo += sTesto & "<hr />L'amministratore TotoMio: " & Amministratore

        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""
        Dim Campi() As String

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Apertura nuovo concorso: " & Numero, Testo)
    End Sub

    Public Sub InviaMailChiusuraAnno(TestoMail As String)
        Dim Testo As String = ""

        Testo = ApreTesto() & "Anche quest'anno siamo giunti alla conclusione del campionato TotoMIO.<br />"
        Testo += "Di seguito troverete le tabelle con i risultati finali e con il dettaglio del bilancio di tutti i giocatori:<hr />"

        Testo += TestoMail

        Testo += ApreTesto() & "<hr />Al prossimo anno..."

        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""
        Dim Campi() As String

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Chiusura Anno", Testo)
    End Sub

    Public Sub InviaMailChiusuraConcorso(Numero As Integer, Tappati() As String, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim Campi() As String
        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "E' stato chiuso il concorso " & Altro & "TotoMIO numero " & Numero & ".<br />"
        Testo += "Chi non ha compilato la schedina è stato automaticamente 'tappato'.<br /><br />"

        If UBound(Tappati) > 0 Then
            Testo += "Giocatori 'tappati':<hr />"
            For i As Integer = 1 To UBound(Tappati)
                Campi = Tappati(i).Split(";")
                Testo += Campi(1) & "<br />"
            Next
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Riga As String

            If DatiGioco.Giornata > 3 And Speciale = False Then
                Sql = "Select A.Squadra, B.CodGiocatore, B.Giocatore From SuddenDeathDett A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & DatiGioco.Giornata
                Rec = Db.LeggeQuery(ConnSQL, Sql)

                If Rec.Eof = False Then
                    Testo += ApreTesto() & "<br />Squadre assegnate ai giocatori per il torneo Sudden Death: " & ChiudeTesto()
                    Testo += ApreTabella()
                    Riga = ";Giocatore;;Squadra;"
                    Testo += ConverteTestoInRigaTabella(Riga, True)

                    Do Until Rec.Eof
                        Riga = RitornaImmagineGiocatore(Rec("Giocatore").Value) & ";" & Rec("Giocatore").Value & ";" & RitornaImmagineSquadra(Rec("Squadra").Value) & ";" & Rec("Squadra").Value & ";"
                        Testo += ConverteTestoInRigaTabella(Riga)

                        Rec.MoveNext()
                    Loop
                    Rec.Close()

                    Testo += "</table> "
                End If
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Testo += "<hr />L'amministratore TotoMio: " & Amministratore

        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Chiusura concorso " & Numero, Testo)
    End Sub

    Public Sub InviaMailAvvisoChiusuraConcorsoSoft(Numero As Integer, Giocatori() As Integer, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""
        Dim g As New Giocatori

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "E' in essere il concorso " & Altro & "TotoMIO numero " & Numero & ".<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & DatiGioco.ChiusuraConcorso & ") per evitare spiacevoli problemi di 'tappaggio'.<br /><br />"

        Testo += "<hr />L'amministratore TotoMio: " & Amministratore

        For i As Integer = 1 To UBound(Giocatori)
            EMail += g.TornaMailGiocatore(Giocatori(i)) & ";"
        Next

        g = Nothing

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Avviso chiusura concorso " & Numero, Testo)
    End Sub

    Public Sub InviaMailAvvisoChiusuraConcorsoHard(Numero As Integer, Giocatori() As Integer, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""
        Dim g As New Giocatori

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "Si ricorda che è in essere il concorso " & Altro & "TotoMIO numero " & Numero & " ed è vicina la data di chiusura.<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & DatiGioco.ChiusuraConcorso & ") per evitare spiacevoli problemi di 'tappaggio'." & ChiudeTesto() & "<br />"
        Testo += "<br />" & ApreTestoGrande() & "Si ricorda che anche una eventuale riapertura del concorso e una successiva compilazione della colonna propria, NON toglierà eventuali tappi immessi in modo automatico dopo la chiusura." & ChiudeTesto() & "<br /><br />"

        Testo += ApreTesto() & "<hr />L'amministratore TotoMio: " & Amministratore & ChiudeTesto()

        For i As Integer = 1 To UBound(Giocatori)
            EMail += g.TornaMailGiocatore(Giocatori(i)) & ";"
        Next

        g = Nothing

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Ultimo avviso chiusura concorso " & Numero, Testo)
    End Sub

    Public Sub InviaMailAvvisoChiusuraConcorsoLast(Numero As Integer, Giocatori() As Integer, Amministratore As String, Speciale As Boolean)
        Dim Testo As String = ""
        Dim EMail As String = ""
        Dim g As New Giocatori

        Dim Altro As String = ""

        If Speciale = True Then
            Altro = "speciale "
        End If

        Testo = ApreTesto() & "Fra pochi minuti verrà chiuso il concorso " & Altro & "TotoMIO numero " & Numero & ".<br />"
        Testo += "Chi non ha compilato la schedina e' pregato di effettuare l'operazione prima della scadenza del concorso (" & DatiGioco.ChiusuraConcorso & ") per evitare spiacevoli problemi di 'tappaggio'." & ChiudeTesto() & "<br />"
        Testo += "<br />" & ApreTestoGrande() & "Si ricorda che anche una eventuale riapertura del concorso e una successiva compilazione della colonna propria, NON toglierà eventuali tappi immessi in modo automatico dopo la chiusura." & ChiudeTesto() & "<br /><br />"

        Testo += ApreTesto() & "<hr />L'amministratore TotoMio: " & Amministratore & ChiudeTesto()

        For i As Integer = 1 To UBound(Giocatori)
            EMail += g.TornaMailGiocatore(Giocatori(i)) & ";"
        Next

        g = Nothing

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Ultimo avviso chiusura concorso " & Numero, Testo)
    End Sub

    Public Sub InviaMailNuovoSondaggio(Numero As Integer, sTesto As String, DataChiusura As String, Amministratore As String)
        Dim Testo As String = ""
        Dim Campi() As String

        Testo = ApreTesto() & "E' stato aperto un nuovo sondaggio. Il numero " & Numero & "." & ChiudeTesto() & "<hr />"
        Testo += ApreTestoMess() & sTesto & ChiudeTesto() & "<hr />"
        Testo += ApreTesto() & "Il sondaggio avrà scadenza " & DataChiusura & "."

        Testo += "<hr />L'amministratore TotoMio: " & Amministratore

        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Apertura sondaggio " & Numero, Testo)
    End Sub

    Public Sub InviaMailChiusuraSondaggio(Numero As Integer, sTesto As String, Amministratore As String, RisposteTesto() As String, Risposte() As Integer, Giocatori As Integer, Votanti As Integer)
        Dim Testo As String = ""
        Dim Campi() As String

        Testo = ApreTesto() & "E' stato chiuso il sondaggio numero " & Numero & "." & ChiudeTesto() & "<hr />"
        Testo += ApreTestoMess() & sTesto & ChiudeTesto() & "<hr />"
        Testo += ApreTesto() & "I risultati del sondaggio sono stati i seguenti:" & ChiudeTesto() & "<br /><br />"

        Dim Perc As Integer

        If Giocatori > 0 Then
            Perc = (Votanti / Giocatori) * 100
        Else
            Perc = 0
        End If

        Testo += "Giocatori: " & Giocatori & " - Votanti: " & Votanti & " (" & Perc & "%)<br /><br />"

        For i As Integer = 0 To 2
            Testo += ApreTestoMess() & RisposteTesto(i) & ": voti " & Risposte(i) & ChiudeTesto() & "<br />"
        Next

        Testo += "<hr />L'amministratore TotoMio: " & Amministratore

        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Chiusura sondaggio " & Numero, Testo)
    End Sub

    Public Sub InviaMailRegsitrazioneGiocatore(Nick As String, EMail As String, Nome As String, Cognome As String)
        Dim Testo As String = ""

        Testo = ApreTesto() & "E' stata richiesta la registrazione di un utente sul sito TotoMIO con le seguenti credenziali:<br /><br />"
        Testo += "NICK: " & Nick & "<br />"
        Testo += "E-Mail: " & EMail & "<br />"
        Testo += "Nome: " & Nome & "<br />"
        Testo += "Cognome: " & Cognome & "<br />"
        Testo += "<br />Per confermare la registrazione utilizzare il seguente link:<br />"
        Testo += IndirizzoSito & "/NuovoUtente.aspx?Registrazione=" & Nick & "&Anno=" & DatiGioco.AnnoAttuale

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Nuovo utente", Testo)

        Testo = ApreTesto() & "E' stata richiesta la registrazione di un utente sul sito TotoMIO con le seguenti credenziali:<br /><br />"
        Testo += "NICK: " & Nick & "<br />"
        Testo += "E-Mail: " & EMail & "<br />"
        Testo += "Nome: " & Nome & "<br />"
        Testo += "Cognome: " & Cognome & "<br />"

        Dim Amministratori() As String = RitornaTuttiIGiocatoriELeMail(True)
        Dim Campi() As String
        Dim EMailAmm As String = ""

        For i As Integer = 1 To UBound(Amministratori)
            Campi = Amministratori(i).Split(";")
            If Campi(1).Trim <> "" Then
                EMailAmm += ";" & Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, EMailAmm, "", "TotoMIO - Richiesta registrazione nuovo utente", Testo)
    End Sub

    Public Sub InviaMailColonnaGiocata(idGiocatore As Integer, Testo As String, Giornata As Integer, Speciale As Boolean)
        Dim G As New Giocatori
        Dim Utente As String = G.TornaNickGiocatore(idGiocatore)
        Dim EMail As String = G.TornaMailGiocatore(idGiocatore)
        G = Nothing

        Dim sTesto As String = ""
        Dim Altro As String

        If Speciale = True Then
            Altro = " speciale"
        Else
            Altro = ""
        End If

        sTesto = ApreTesto() & "Il giocatore " & Utente & " ha inserito oppure modificato la propria schedina per il concorso" & Altro & " numero " & Giornata & ".<br /><br />"
        sTesto += Testo

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Compilazione colonna '" & Utente & "' - Concorso" & Altro & " numero " & Giornata, sTesto)

        Dim Amministratori() As String = RitornaTuttiIGiocatoriELeMail(True)
        Dim Campi() As String
        Dim EMailAmm As String = ""

        For i As Integer = 1 To UBound(Amministratori)
            Campi = Amministratori(i).Split(";")
            If Campi(1).Trim <> "" Then
                EMailAmm += ";" & Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Compilazione colonna '" & Utente & "' - Concorso" & Altro & " numero " & Giornata, sTesto)
        InviaMailCdoSys(EmailDiInvio, EMailAmm, "", "TotoMIO - Compilazione colonna '" & Utente & "' - Concorso" & Altro & " numero " & Giornata, sTesto)
    End Sub

    Public Sub InviaMailRiaperturaConcorso(Amministratore As String, Giornata As Integer, Speciale As Boolean)
        Dim Testo As String = ""
        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""
        Dim Campi() As String

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        Dim g As Integer = Giornata
        Dim Altro As String = ""

        If Speciale = True Then
            g -= 100
            Altro = " speciale"
        End If

        Testo = ApreTesto() & "E' stato riaperto il concorso" & Altro & " " & g & ".<br />"
        Testo += "I giocatori che non avevano compilato la propria colonna lo potranno di nuovo fare ma il numero dei tappi non varierà.<br />"
        Testo += "<br />L'amministatore TotoMio: " & Amministratore

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Riapertura concorso" & Altro & " numero " & g, Testo)
    End Sub

    Public Sub InviaMailPerPagamentoGiocatore(Importo As String, idGiocatore As Integer, idCassiere As Integer, Bilancio As String)
        Dim g As New Giocatori
        Dim Utente As String = g.TornaNickGiocatore(idGiocatore)
        Dim Cassiere As String = g.TornaNickGiocatore(idCassiere)
        Dim EMail As String = g.TornaMailGiocatore(idGiocatore)
        Dim EMailCassiere As String = g.TornaMailGiocatore(idCassiere)
        g = Nothing

        Dim Testo As String = ""

        Testo = ApreTesto() & "Il giocatore '" & Utente & "' ha effettuato un pagamento di Euro " & Importo & ".<br />"
        Testo += "Situazione contabile giocatore:<br />" & Bilancio
        Testo += "<br />Il cassiere: " & Cassiere

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Pagamento giocatore '" & Utente & "'", Testo)
        InviaMailCdoSys(EmailDiInvio, EMailCassiere, "", "TotoMIO - Pagamento giocatore '" & Utente & "'", Testo)
    End Sub

    Public Sub InviaMailPerIncassoGiocatore(Importo As String, idGiocatore As Integer, idCassiere As Integer, Bilancio As String)
        Dim g As New Giocatori
        Dim Utente As String = g.TornaNickGiocatore(idGiocatore)
        Dim Cassiere As String = g.TornaNickGiocatore(idCassiere)
        Dim EMail As String = g.TornaMailGiocatore(idGiocatore)
        Dim EMailCassiere As String = g.TornaMailGiocatore(idCassiere)
        g = Nothing

        Dim Testo As String = ""

        Testo = ApreTesto() & "Il giocatore '" & Utente & "' ha effettuato un prelievo di Euro " & Importo & "."
        Testo += "Situazione contabile giocatore:<br />" & Bilancio
        Testo += "<br />Il cassiere: " & Cassiere

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Prelievo giocatore '" & Utente & "'", Testo)
        InviaMailCdoSys(EmailDiInvio, EMailCassiere, "", "TotoMIO - Prelievo giocatore '" & Utente & "'", Testo)
    End Sub

    Public Sub InviaMailPerNotificaMessaggio(idDestinatario As String, idMittente As String, TestoMess As String)
        Dim g As New Giocatori
        Dim Mittente As String = g.TornaNickGiocatore(idMittente)
        Dim EMail As String = ""
        Dim idC() As String = idDestinatario.Split(";")
        For i As Integer = 0 To UBound(idC) - 1
            EMail += g.TornaMailGiocatore(idC(i)) & ";"
        Next
        g = Nothing

        Dim Testo As String = ""

        Testo = ApreTesto() & "Il giocatore '" & Mittente & "' ti ha inviato il seguente messaggio:" & ChiudeTesto() & "<hr />"
        Testo += RitornaImmagineGiocatore(Mittente) & ApreTestoMess() & ": " & TestoMess & ChiudeTesto()
        Testo += "<hr />" & ApreTesto() & "Per rispondere o eliminare il messaggio utilizzare la pagina principale ed andare nella sezione apposita:<br /><br />http://looigi.no-ip.biz:12345/TotoMioII" & ChiudeTesto()

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Notifica messaggio", Testo)
    End Sub

    Public Sub InviaMailGiocatoreRegistrato(idUtente As Integer)
        Dim g As New Giocatori
        Dim Utente As String = g.TornaNickGiocatore(idUtente)
        Dim EMail As String = g.TornaMailGiocatore(idUtente)
        Dim Password As String = g.TornaPasswordGiocatore(idUtente)
        g = Nothing

        Dim Testo As String = ""
        Dim TestoAmm As String = ""

        Testo = ApreTesto() & "Il giocatore '" & Utente & "' è stato registrato correttamente all'interno del sito TotoMIO."
        TestoAmm = Testo
        Testo += "<br />Per effettuare l'accesso utilizzare la pagina principale:<br /><br />http://looigi.no-ip.biz:12345/TotoMioII"

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Conferma registrazione", Testo)

        Dim Amministratori() As String = RitornaTuttiIGiocatoriELeMail(True)
        Dim Campi() As String
        Dim EMailAmm As String = ""

        For i As Integer = 1 To UBound(Amministratori)
            Campi = Amministratori(i).Split(";")
            If Campi(1).Trim <> "" Then
                EMailAmm += ";" & Campi(1) & ";"
            End If
        Next

        InviaMailCdoSys(EmailDiInvio, EMailAmm, "", "TotoMIO - Conferma registrazione nuovo utente", TestoAmm)
    End Sub

    Public Sub InviaMailPerAmicoRegistrato(AChi As Integer, Utente As String)
        Dim g As New Giocatori
        Dim EMail As String = g.TornaMailGiocatore(AChi)
        g = Nothing

        Dim Testo As String = ""

        Testo = ApreTesto() & "Il giocatore '" & Utente & "' si è registrato correttamente all'interno del sito TotoMIO."
        Testo += "<br />Questa operazione ti ha permesso di guadagnare 3 Euro in bonus pagamenti."

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Conferma registrazione amico", Testo)
    End Sub

    Public Sub InviaMailRicordaPassword(idUtente As Integer)
        Dim g As New Giocatori
        Dim Utente As String = g.TornaNickGiocatore(idUtente)
        Dim EMail As String = g.TornaMailGiocatore(idUtente)
        Dim Password As String = g.TornaPasswordGiocatore(idUtente)
        g = Nothing

        Dim Testo As String = ""

        Testo = ApreTesto() & "La password utilizzata dall'utente " & Utente & " nel sito TotoMIO è:<br /><br />" & Password & "<br /><br />Si consiglia di cambiarla al nuovo accesso."

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Ricordami la password", Testo)
    End Sub

    Private Function RitornaTuttiIGiocatoriELeMail(Optional SoloAmministratori As Boolean = False) As String()
        Dim Gioc() As String = {}
        Dim qGioc As Integer = 0
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Altro As String = ""

            If SoloAmministratori = True Then
                Altro = "And idTipologia=1 "
            End If

            Sql = "Select Giocatore, Email, InvioMailCompleta, idTipologia From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " " & Altro & " And Cancellato='N' Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                qGioc += 1
                ReDim Preserve Gioc(qGioc)
                Gioc(qGioc) = Rec("Giocatore").Value & ";" & Rec("EMail").Value & ";" & Rec("InvioMailCompleta").Value & ";" & Rec("idTipologia").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Gioc
    End Function

    Public Sub InviaMailPresentaAmico(CodGiocatore As Integer, EMail As String)
        Dim g As New Giocatori
        Dim UtenteInviante As String = g.TornaNickGiocatore(CodGiocatore)
        Dim NomeInviante As String = g.TornaNomeGiocatore(CodGiocatore)
        g = Nothing

        Dim TestoMail As String = ""

        TestoMail = ApreTesto() & "L'Utente '" & UtenteInviante & "' (" & NomeInviante & ") ti invita a giocare a TotoMIO.<br />"
        TestoMail += "Partecipa con noi allo splendido campionato e agli avvincenti tornei che ti permetteranno di vincere piccoli premi in denaro.<br />"
        TestoMail += "Tutto ciò che si dovrà fare, sarà compilare una colonna basata sulla giornata del campionato di calcio e sperare di fare un punto in più degli altri giocatori. "
        TestoMail += "Ci saranno premi settimanali e premi finali. Tutti potranno vincere...<br /><br />"
        TestoMail += "Indirizzo sito: http://www.looigi.it<br /><hr />"
        TestoMail += "Regolamento alla pagina: http://looigi.no-ip.biz:12345/TotoMioII/Regolamento.aspx"
        TestoMail += "<br /><hr />" & ChiudeTesto() & ApreTestoGrande() & "Attenzione:" & ChiudeTesto() & ApreTesto() & " per permettere al tuo amico di ricevere il premio di presentazione utlizza la presente E-Mail in fase di registrazione.<br />"
        TestoMail += "Sarà possibile cambiarla successivamente." & ChiudeTesto()

        InviaMailCdoSys(EmailDiInvio, EMail, "", "TotoMIO - Presenta amico", TestoMail)
    End Sub

    Public Sub InviaMailCommentoAllaGiornata(Testo As String)
        Dim TestoMail As String = ""
        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""
        Dim Campi() As String

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        TestoMail = ApreTesto() & "E' stato rilasciato il commento alla giornata TotoMio n° " & DatiGioco.Giornata & ".<hr />" & Mid(Testo, 1, 100) & "...<hr />"
        TestoMail += "Per leggerlo entrare nel sito e selezionare la voce 'Commenti alla giornata' del menù 'Statistiche'"

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Commento alla giornata " & DatiGioco.Giornata, TestoMail)
    End Sub

    Public Sub InviaMailAggiornamentoRisultati(Percorso As String, TestoMail As String, Giornata As Integer, Speciale As Boolean)
        Dim Amministratori() As String = RitornaTuttiIGiocatoriELeMail(False)
        Dim Campi() As String
        Dim EMailAmmCompleta As String = ""
        Dim EMailAmmParziale As String = ""

        For i As Integer = 1 To UBound(Amministratori)
            Campi = Amministratori(i).Split(";")
            If Campi(1).Trim <> "" Then
                If Campi(2).Trim = "S" Then
                    EMailAmmCompleta += ";" & Campi(1)
                Else
                    EMailAmmParziale += ";" & Campi(1)
                End If
            End If
        Next

        If EMailAmmCompleta <> "" Then
            InviaMailAggiornamentoRisultatiCompleta(Percorso, TestoMail, Giornata, Speciale, EMailAmmCompleta)
        End If
        If EMailAmmParziale <> "" Then
            InviaMailAggiornamentoRisultatiParziale(Giornata, Speciale, EMailAmmParziale)
        End If
    End Sub

    Public Sub InviaMailAggiornamentoRisultatiParziale(Giornata As Integer, Speciale As Boolean, EmailAmm As String)
        Dim f As New GestioneFilesDirectory
        'Percorso += "\" & DatiGioco.AnnoAttuale & "\"
        'Dim NomeFile As String = Giornata & ".htm"
        Dim sTesto As String

        'f.CreaDirectoryDaPercorso(Percorso)
        'f.ImpostaPercorsoAttuale(Percorso)
        'sTesto = TestoMail.Replace("§§§§", "<td style=""background-color: #aaaaaa; width: 3px;"">&nbsp;</td>")
        'sTesto = sTesto.Replace("**§§**", "</td></tr></table>")
        'Do While sTesto.IndexOf("?^?^LARGH?^?^") > -1
        '    sTesto = sTesto.Replace("?^?^LARGH?^?^", " width: 100%; ")
        'Loop
        'f.CreaAggiornaFile(NomeFile, sTesto)

        'f = Nothing

        'Dim Cosa As String = "</tr></table>"

        'Cosa += "<br /><br /><hr />" & ApreTestoTitolo() & "Dettaglio schedine giocatori: " & ChiudeTesto() & "<hr />"

        'sTesto = TestoMail.Replace("§§§§", Cosa)
        'sTesto = sTesto.Replace("**§§**", "")
        'Do While sTesto.IndexOf("?^?^LARGH?^?^") > -1
        '    sTesto = sTesto.Replace("?^?^LARGH?^?^", " ")
        'Loop
        'Do While sTesto.IndexOf("width: 59%;") > -1
        '    sTesto = sTesto.Replace("width: 59%;", " ")
        'Loop

        Dim g As Integer
        Dim Altro As String

        If Speciale = True Then
            g = Giornata - 100
            Altro = " speciale"
        Else
            g = Giornata
            Altro = ""
        End If

        sTesto = ApreTesto() & "E' stato effettuato il controllo alla giornata" & Altro & " TotoMio n° " & Giornata & ".<hr />"
        sTesto += "Per vedere l'andamento e i risultati entrare nel sito e selezionare la voce 'Controllo' dal menù.<hr />"

        InviaMailCdoSys(EmailDiInvio, EmailAmm, "", "TotoMIO - Controllo parziale concorso" & Altro & " " & g, sTesto)
    End Sub

    Public Sub InviaMailAggiornamentoRisultatiCompleta(Percorso As String, TestoMail As String, Giornata As Integer, Speciale As Boolean, EmailAmm As String)
        Dim f As New GestioneFilesDirectory
        Percorso += "\" & DatiGioco.AnnoAttuale & "\"
        Dim NomeFile As String = Giornata & ".htm"
        Dim sTesto As String

        f.CreaDirectoryDaPercorso(Percorso)
        f.ImpostaPercorsoAttuale(Percorso)
        sTesto = TestoMail.Replace("§§§§", "<td style=""background-color: #aaaaaa; width: 3px;"">&nbsp;</td>")
        sTesto = sTesto.Replace("**§§**", "</td></tr></table>")
        Do While sTesto.IndexOf("?^?^LARGH?^?^") > -1
            sTesto = sTesto.Replace("?^?^LARGH?^?^", " width: 100%; ")
        Loop
        f.CreaAggiornaFile(NomeFile, sTesto)

        f = Nothing

        Dim Cosa As String = "</tr></table>"

        Cosa += "<br /><br /><hr />" & ApreTestoTitolo() & "Dettaglio schedine giocatori: " & ChiudeTesto() & "<hr />"

        sTesto = TestoMail.Replace("§§§§", Cosa)
        sTesto = sTesto.Replace("**§§**", "")
        Do While sTesto.IndexOf("?^?^LARGH?^?^") > -1
            sTesto = sTesto.Replace("?^?^LARGH?^?^", " ")
        Loop
        Do While sTesto.IndexOf("width: 59%;") > -1
            sTesto = sTesto.Replace("width: 59%;", " ")
        Loop

        Dim g As Integer
        Dim Altro As String

        If Speciale = True Then
            g = Giornata - 100
            Altro = " speciale"
        Else
            g = Giornata
            Altro = ""
        End If

        InviaMailCdoSys(EmailDiInvio, EmailAmm, "", "TotoMIO - Controllo completo concorso" & Altro & " " & g, sTesto)
    End Sub

    Public Sub InviaMailAvvisoAndamentoConcorso(sTestoMail As String, Giornata As Integer, Speciale As Boolean)
        Dim sEmail() As String = RitornaTuttiIGiocatoriELeMail()
        Dim Email As String = ""
        Dim Campi() As String

        For i As Integer = 1 To UBound(sEmail)
            Campi = sEmail(i).Split(";")
            If Campi(1) <> "" Then
                Email += Campi(1) & ";"
            End If
        Next

        Dim TestoMail As String = ""
        Dim Altro As String

        If Speciale = True Then
            Altro = " speciale"
        Else
            Altro = ""
        End If

        TestoMail = ApreTesto() & "Andamento concorso" & Altro & " " & Giornata & ":"
        TestoMail += "<hr />" & ChiudeTesto()
        TestoMail += sTestoMail
        TestoMail += "<hr />"

        InviaMailCdoSys(EmailDiInvio, Email, "", "TotoMIO - Andamento concorso" & Altro & " " & Giornata, sTestoMail)
    End Sub

    Public Function ConverteTestoInRigaTabella(Riga As String, Optional PrimaRiga As Boolean = False) As String
        Dim Ritorno As String = Riga
        Dim TestoPrimaRiga As String = ""

        TestoPrimaRiga = "style=" & Chr(34) & "font-family:Tahoma; font-size: 12px; "
        If PrimaRiga = True Then
            TestoPrimaRiga += " background-color: #aaaaaa;"
            TestoPrimaRiga += " color: #FFFFFF; "
        Else
            TestoPrimaRiga += " color: #000000; "
        End If
        TestoPrimaRiga += Chr(34)
        TestoPrimaRiga += " align=" & Chr(34) & "center" & Chr(34)

        Dim ApreCampo As String = "<td>"

        Ritorno = "<tr " & TestoPrimaRiga & ">" & ApreCampo & Ritorno.Replace(";", "</td>" & ApreCampo)
        Ritorno = Mid(Ritorno, 1, Ritorno.Length - (ApreCampo.Length)) & "</tr>"

        Return Ritorno
    End Function

    Private Function PrendeStilePerMail() As String
        Dim Stile As String = ""

        Stile += "opacity:0.82; "
        Stile += "background-color: #FFFFFF; "
        Stile += "padding: 4px; "
        Stile += "margin: 0 auto; "
        Stile += "padding-bottom: 7px; "

        Stile += "-webkit-box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "
        Stile += "-moz-box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "
        Stile += "box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "

        Stile += "-webkit-border-radius: 0.45em 0.45em 0.45em 0.45em; "
        Stile += "-moz-border-radius: 0.45em 0.45em 0.45em 0.45em; "
        Stile += "border-radius: 0.35em 0.45em 0.45em 0.45em; "

        Stile += "display: inline-block;"

        Return Stile
    End Function

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
            Dim StringaPassaggio As String
            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance

            StringaPassaggio = "?Errore=Errore mail: " & Err.Description.Replace(" ", "%20").Replace(vbCrLf, "")
            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("Nick")
            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try

        InviaMessaggiPerApp(MailTo, MailTesto)
    End Sub

    Private Function RemoteServerCertificateValidationCallback(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Console.WriteLine(certificate)
        Return True
    End Function

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

    Private Sub InviaMessaggiPerApp(A As String, Testo As String)
        Dim Destinatari() As String = A.Split(";")
        Dim Indirizzo As String
        Dim g As New Giocatori
        Dim gf As New GestioneFilesDirectory
        Dim Contenuto As String
        Dim Percorso As String = PercorsoApplicazione & "\MessaggiApp\" & DatiGioco.AnnoAttuale & "\"
        Dim sTesto As String = ToglieTag(Testo)

        gf.CreaDirectoryDaPercorso(Percorso)

        For Each Nome As String In Destinatari
            Dim id As Integer = g.TornaIdGiocatoreDaMail(Nome)
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

    Private Sub InviaMailCdoSys(ByVal MailFrom As String, ByVal MailTo As String, ByVal MailCc As String, ByVal MailSubject As String, ByVal MailTesto As String)
        If SitoInLocale = True Then
            InviaMailTramiteGmail(MailFrom, MailTo, MailCc, MailSubject, MailTesto)
            Exit Sub
        End If

        Dim Tutto As String = ""
        Dim Oggetto As String = ""

        Try
            Dim pMail As Object
            Dim sMailTesto As String = MailTesto

            sMailTesto = ImmagineDiTestaMail & sMailTesto

            pMail = CreateObject("CDO.Message")
            pMail.from = MailFrom
            'pMail.BodyEncoding = Encoding.UTF8
            pMail.AutoGenerateTextBody = False
            pMail.to = MailTo
            If Len(MailCc) > 0 Then pMail.Bcc = MailCc
            pMail.subject = "[TotoMIO] " & MailSubject
            Oggetto = MailSubject

            Dim ImmSfondo As String = PercorsoImmaginiRoot & "App_Themes/Standard/Images/bg.jpg"

            Tutto += "<html>"
            Tutto += "  <body>"
            Tutto += "      <form id=""frmMain"">"
            Tutto += "          <div style=""background: url('" & ImmSfondo & "'); padding: 13px; margin: -9px;"">"
            Tutto += "              <div style=" & Chr(34) & "width: 100%; text-align:center;" & Chr(34) & ">"
            Tutto += "                  <div style=" & Chr(34) & PrendeStilePerMail() & Chr(34) & ">"
            Tutto += "                      <center>"
            Tutto += sMailTesto
            Tutto += "<hr />" & ApreTesto() & IndirizzoSito & "<br />" & Now & ChiudeTesto()
            Tutto += "                      </center>"
            Tutto += "                  </div>"
            Tutto += "              </div>"
            Tutto += "          </div>"
            Tutto += "      </form>"
            Tutto += "  </body>"
            Tutto += "</html>"

            pMail.HTMLBody = Tutto

            'ScriveMailPerDebug(MailTo, Oggetto, Tutto)

            With pMail.Configuration
                .Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                .Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "localhost"
                .Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
                .Fields.Update()
            End With

            pMail.send()

            pMail = Nothing
        Catch ex As Exception
            'If ModalitaLocale = False Then
            Dim StringaPassaggio As String
                Dim H As HttpApplication = HttpContext.Current.ApplicationInstance

                StringaPassaggio = "?Errore=Errore mail: " & Err.Description.Replace(" ", "%20").Replace(vbCrLf, "")
                StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("Nick")
                StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
                H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            'End If
        End Try

        InviaMessaggiPerApp(MailTo, MailTesto)
    End Sub

    Private Sub ScriveMailPerDebug(MailTo As String, Oggetto As String, Tutto As String)
        Dim Db As New GestioneDB
        Dim Speciale As Boolean

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                Speciale = True
            Else
                Speciale = False
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Dim g As Integer

        If Speciale = True Then
            g = DatiGioco.GiornataSpeciale + 100
        Else
            g = DatiGioco.Giornata
        End If

        Dim f As New GestioneFilesDirectory
        Dim Percorso As String = HttpContext.Current.Server.MapPath(".") & "\InvioMail\MailsLocali\" & DatiGioco.AnnoAttuale & "\" & g & "\"
        Dim a1 As Integer = Oggetto.IndexOf(" - ")
        If a1 > -1 Then
            Oggetto = Trim(Mid(Oggetto, a1 + 3, Oggetto.Length))
        End If
        Oggetto = Oggetto.Replace(" ", "_")
        If Oggetto.Length > 20 Then
            Oggetto = Mid(Oggetto, 1, 20)
        End If
        'Oggetto &= "_" & Now.Year & Format(Now.Month, "00") & Format(Now.Day, "00") & Format(Now.Hour, "00") & Format(Now.Minute, "00") & Format(Now.Second, "00")
        Dim NomeFile As String = Oggetto & ".htm"
        Dim sTesto As String

        f.CreaDirectoryDaPercorso(Percorso)
        f.ImpostaPercorsoAttuale(Percorso)
        sTesto = "Destinatari: " & MailTo & "<hr />"
        sTesto += Tutto
        f.CreaAggiornaFile(NomeFile, sTesto)

        f = Nothing
    End Sub
End Class
