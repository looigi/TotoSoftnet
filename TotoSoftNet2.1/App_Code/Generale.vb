Imports System.IO
Imports System.Globalization
Imports System.Net

Module Generale
    Public StringaConnessioneAccess As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=***\Mdb\Totosoft.mdb;Persist Security Info=False;"

    'Public ModalitaLocale As Boolean = False
    Public PercorsoImmaginiRoot As String
    Public Const ColoreSfondoRigaPropria As String = "#E4A6A6"
    Public Const ColoreTestoRigaPropria As String = "#B33838"

    Public Const IndirizzoPerMailLocalHost As String = "http://looigi.no-ip.biz:12345/TotoMioII"
    Public Const EmailLocalHost As String = "looigi@gmail.com"
    Public Const IndirizzoPerMailWEB As String = "http://www.looigi.it/TotoMio2"
    Public Const EMailWEB As String = "postmaster@looigi.it"

    Public SitoInLocale As Boolean
    Public IndirizzoSito As String
    Public EmailDiInvio As String
    Public CasellaDiPosta As String
    Public PasswordCasellaDiPosta As String
    Public LettereValide As String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ)(/&$£!°§*_.,:;-+#"

    'Public Sub ControllaModalitaDiLavoro(Percorso As String)
    '    If File.Exists(Percorso) = True Then
    '        Dim gf As New GestioneFilesDirectory
    '        Dim riga As String = gf.LeggeFileIntero(Percorso)
    '        If riga = "DEBUG" Then
    '            ModalitaLocale = True
    '        Else
    '            ModalitaLocale = False
    '        End If
    '    Else
    '        ModalitaLocale = False
    '    End If
    'End Sub

    Public Const PrefissoTabelle As String = ""

    Public Structure ValoriStatoConcorso
        Dim TipologiaConcorso As Integer
        Const Nessuno = 0
        Const Aperto = 1
        Const DaControllare = 2
        Const Chiuso = 3
        Const AnnoChiuso = 4
    End Structure

    Public Structure DdGioco
        Dim AnnoAttuale As Integer
        Dim Giornata As Integer
        Dim NomeCampionato As String
        Dim StatoConcorso As Integer
        Dim ChiusuraConcorso As String
        Dim PartitaJolly As Integer
        Dim GiornataInterToto As Integer
        Dim GiornataEuropaLeague As Integer
        Dim GiornataChampionsLeague As Integer
        Dim GiornataDerelitti As Integer
        Dim GiornataCoppaItalia As Integer
        Dim GiornataSpeciale As Integer
    End Structure

    Public DatiGioco As DdGioco

    Public Structure Permessi
        Dim TipologiaPermessi As Integer
        Const Amministratore = 1
        Const Giocatore = 2
        Const Aggiornatore = 3
        Const Cassiere = 4
        Const Commentantore = 5
    End Structure

    Public Classifica() As String

    Public QuotaGiocoSettimanale As Single
    Public QuotaPerSpeciali As Single
    Public NumeroGiornateTotali As Single
    Public PuntoVirgola As String
    Public MaxDoppie As Integer
    Public GiornataSemifinale As Integer
    Public GiornataFinale As Integer

    Public QuantiPrimi As Integer = 0
    Public QuantiSecondi As Integer = 0
    Public PremioPrimo As Single
    Public PremioSecondo As Single

    Public Structure Perc
        Dim QuantiChampions As Integer
        Dim PassaggioCL As Integer
        Dim QuantiEuropaLeague As Integer
        Dim PassaggioEL As Integer
        Dim QuantiIntertoto As Integer
        Dim PassaggioIT As Integer
        Dim QuantiDerelitti As Integer
    End Structure

    Public AppoRow As GridViewRow

    Public ImmaginiAlbum() As String
    Public QuanteImmaginiAlbum As Integer = 0

    Public ScrittaScorrevole As String = ""
    Public PercorsoApplicazione As String

    Public GiornataEventoCreazione As Integer = -1

    Public TipoBrowser As String
    Public clsColonna() As clsColonnaPropria
    Public clsCash() As clsCassa

    Public PremutoOption As Boolean

    Public TitoliGruppiQuoteCUP() As String = {"Gruppo 1 - Girone A", "Gruppo 1 - Girone B", "Gruppo 2 - Girone C", "Gruppo 2 - Girone D"}
    Public NumeriGruppiQuoteCUP() As String = {"1,2,3", "4,5,6", "7,8,9", "10,11,12"}

    Public Const QuantePartiteQuoteCUP As Integer = 22

    Public Function ControllaConcorsoSpeciale(Db As GestioneDB, ConnSQL As Object) As Boolean
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Ritorno As Boolean

        Sql = "Select * From PartiteSpeciali Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiornata Is Not Null"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = True Then
            Ritorno = False
        Else
            Ritorno = True
        End If
        Rec.Close()

        Return Ritorno
    End Function

    Public Function FormattaNumeroConVirgola(Numero As Single) As String
        Dim Ritorno As String

        Ritorno = Numero.ToString("000.00", CultureInfo.InvariantCulture)
        Do While Left(Ritorno, 1) = "0"
            Ritorno = Mid(Ritorno, 2, Ritorno.Length)
        Loop
        If Left(Ritorno.Trim, 1) = "." Then
            Ritorno = "0" & Ritorno
        End If

        Return Ritorno
    End Function

    Public Function RimetteCaratteriStraniNelNome(Nome As String) As String
        Dim Ritorno As String = Nome
        Dim Numero As Integer
        Dim Cambio As String

        Do While Ritorno.IndexOf("&#") > -1
            Numero = Val(Mid(Ritorno, Ritorno.IndexOf("&#") + 3, 3))
            Cambio = "&#" & Mid(Ritorno, Ritorno.IndexOf("&#") + 3, 3) & ";"

            Ritorno = Ritorno.Replace(Cambio, Chr(Numero))
        Loop

        Return Ritorno
    End Function

    Public Function MetteaCapo(Stringa As String, Caratteri As Integer) As String
        Dim Ritorno As String = ""
        Dim i As Integer = 0
        Dim inizio As Integer = 1
        Dim conta As Integer = 0

        Do While i <= Stringa.Length
            i += 1
            conta += 1
            If conta = Caratteri Then
                conta = 0
                For k As Integer = inizio + i To 1 Step -1
                    If Mid(Stringa, k, 1) = " " Then
                        Ritorno += "&nbsp;" & Mid(Stringa, inizio, k - inizio) & "&nbsp;<br /><br />"
                        i = k + 1
                        inizio = i
                        Exit For
                    End If
                Next
            End If
        Loop
        If inizio < Stringa.Length And inizio > 1 Then
            Ritorno += "&nbsp;" & Mid(Stringa, inizio - 1, Stringa.Length)
        End If

        Return Ritorno
    End Function

    Public Function RitornaImmagine(Path As String, NomeGiocatore As String, Optional Anno As Integer = -1) As String
        Dim Percorso As String = ""
        Dim sAnno As String

        If Anno = -1 Then
            sAnno = DatiGioco.AnnoAttuale.ToString.Trim
        Else
            sAnno = Anno.ToString.Trim
        End If

        If NomeGiocatore.ToUpper.Trim = "RIPOSO" Or NomeGiocatore.Trim = "" Or NomeGiocatore.Trim = "&nbsp;" Then
            Percorso = "App_Themes/Standard/Images/Giocatori/Riposo.Jpg"
        Else
            Percorso = Path.Replace("\", "/") & "/App_Themes/Standard/Images/Giocatori/" & sAnno & "/" & NomeGiocatore & ".Jpg"

            If File.Exists(Percorso) = True Then
                Percorso = "App_Themes/Standard/Images/Giocatori/" & sAnno & "/" & NomeGiocatore & ".Jpg"
            Else
                Percorso = "App_Themes/Standard/Images/Giocatori/Sconosciuto.png"
            End If
        End If

        Return Percorso
    End Function

    Public Function RitornaImmagineSfondo(Path As String, NomeGiocatore As String) As String
        Dim Percorso As String = ""

        Percorso = Path.Replace("\", "/") & "/App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & NomeGiocatore & ".Jpg"

        If File.Exists(Percorso) = True Then
            Percorso = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & NomeGiocatore & ".Jpg"
        Else
            Percorso = ""
        End If

        Return Percorso
    End Function

    Public Function TornaStatoStringa() As String
        Dim Ritorno As String = ""

        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
                Ritorno = "Aperto"
            Case ValoriStatoConcorso.Chiuso
                Ritorno = "Chiuso"
            Case ValoriStatoConcorso.DaControllare
                Ritorno = "Da controllare"
            Case ValoriStatoConcorso.AnnoChiuso
                Ritorno = "Anno chiuso"
            Case ValoriStatoConcorso.Nessuno
                Ritorno = ""
        End Select

        Return Ritorno
    End Function

    Public Function TornaRisultatoRandom(Segno As String) As String
        Dim Risultato As String

        Randomize()
        Dim g1 As Integer = Int(Rnd(1) * 5)
        Dim g2 As Integer = Int(Rnd(1) * 5)
        Dim appo As Integer

        Select Case Segno
            Case "1"
                If g1 < g2 Then
                    appo = g1
                    g1 = g2
                    g2 = appo
                End If
                If g1 = g2 Then
                    Randomize()
                    g1 += Int(Rnd(1) * 2) + 1
                End If
            Case "1X"
                If g1 < g2 Then
                    appo = g1
                    g1 = g2
                    g2 = appo
                End If
            Case "12"
                If g1 = g2 Then
                    Randomize()
                    appo = Int(Rnd(1) * 3) + 1
                    If appo = 2 Then
                        g1 += Int(Rnd(1) * 2) + 1
                    Else
                        g2 += Int(Rnd(1) * 2) + 1
                    End If
                End If
            Case "X"
                If g1 <> g2 Then
                    g1 = g2
                End If
            Case "X2"
                If g1 > g2 Then
                    appo = g1
                    g1 = g2
                    g2 = appo
                End If
            Case "2"
                If g1 > g2 Then
                    appo = g1
                    g1 = g2
                    g2 = appo
                End If
                If g1 = g2 Then
                    Randomize()
                    g2 += Int(Rnd(1) * 2) + 1
                End If
        End Select

        Risultato = g1.ToString.Trim & "-" & g2.ToString.Trim

        Return Risultato
    End Function

    Public Function TornaColonnaRandom() As String
        Dim Colonna As String = ""
        Dim Segni As String = "1X2"

        For i As Integer = 1 To 14
            Randomize()
            Dim g1 As Integer = Int(Rnd(1) * 3) + 1
            If g1 < 1 Then g1 = 1
            If g1 > 3 Then g1 = 3

            Colonna += Mid(Segni, g1, 1)
        Next

        Return Colonna
    End Function

    Public Function PrendePuntiUfficialiPerSegni(Cosa As Integer) As Integer
        Dim Punti As Integer = Cosa

        Select Case Punti
            Case -1
                Punti = -1
            Case 0 To 3
                Punti = 0
            Case 4 To 5
                Punti = 1
            Case 6 To 7
                Punti = 2
            Case 8 To 9
                Punti = 3
            Case 10 To 12
                Punti = 4
            Case Else
                Punti = 5
        End Select

        Return Punti
    End Function

    Public Function PrendePuntiUfficialiPerRisultato(Cosa As Integer) As Integer
        Dim Punti As Integer = Cosa

        Select Case Punti
            Case -1
                Punti = -1
            Case 0 To 3
                Punti = 0
            Case 4 To 5
                Punti = 1
            Case 6 To 7
                Punti = 2
            Case 8 To 9
                Punti = 3
            Case 10 To 12
                Punti = 4
            Case Else
                Punti = 5
        End Select

        Return Punti
    End Function

    Public CompletataChampionsA As Boolean
    Public CompletataChampionsB As Boolean

    Public Sub LeggeDatiDiGioco(Optional Anno As Integer = 1)
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.*,B.Descrizione From DatiDiGioco A " & _
                "Left Join Anni B " & _
                "On A.Anno=B.Anno " & _
                "Where A.Anno=" & Anno
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                DatiGioco.AnnoAttuale = Rec("Anno").Value
                DatiGioco.Giornata = Rec("Giornata").Value
                DatiGioco.NomeCampionato = Rec("Descrizione").Value
                DatiGioco.StatoConcorso = Rec("Stato").Value
                DatiGioco.ChiusuraConcorso = "" & Rec("ChiusuraConcorso").Value
                DatiGioco.PartitaJolly = "" & Rec("PartitaJolly").Value
                DatiGioco.GiornataInterToto = Val("" & Rec("GiornataIT").Value)
                DatiGioco.GiornataEuropaLeague = Val("" & Rec("GiornataEL").Value)
                DatiGioco.GiornataChampionsLeague = Val("" & Rec("GiornataCL").Value)
                DatiGioco.GiornataDerelitti = Val("" & Rec("GiornataDer").Value)
                DatiGioco.GiornataCoppaItalia = Val("" & Rec("GiornataCI").Value)
                DatiGioco.GiornataSpeciale = Val("" & Rec("GiornataSpeciale").Value)
            End If
            Rec.Close()

            Sql = "Select * From AnnoConfig Where Anno=" & Anno
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                QuotaPerSpeciali = Rec("QuotaPerSpeciali").Value
                QuotaGiocoSettimanale = Rec("CostoGiocata").Value
                NumeroGiornateTotali = Rec("Giornate").Value
                PuntoVirgola = Rec("PuntoVirgola").Value
                MaxDoppie = Rec("Doppie").Value
                GiornataSemifinale = Rec("GiornataSemifinale").Value
                GiornataFinale = Rec("GiornataFinale").Value
            Else
                QuotaPerSpeciali = 3
                QuotaGiocoSettimanale = 1.5
                NumeroGiornateTotali = 38
                PuntoVirgola = ","
                MaxDoppie = 6
                GiornataSemifinale = 33
                GiornataFinale = 36
            End If
            Rec.Close()

            ConnSQL.Close()
        End If
        Db = Nothing
    End Sub

    Public Sub AggiornaDatiDiGioco(Db As GestioneDB)
        Dim ConnSQL As Object = Db.ApreDB()
        Dim Sql As String

        Sql = "Update DatiDiGioco Set " & _
            "Giornata=" & DatiGioco.Giornata & ", " & _
            "Stato=" & DatiGioco.StatoConcorso & ",  " & _
            "ChiusuraConcorso='" & SistemaTestoPerDB(DatiGioco.ChiusuraConcorso) & "', " & _
            "PartitaJolly=" & DatiGioco.PartitaJolly & ", " & _
            "GiornataIT=" & DatiGioco.GiornataInterToto & ",  " & _
            "GiornataEL=" & DatiGioco.GiornataEuropaLeague & ",  " & _
            "GiornataCI=" & DatiGioco.GiornataCoppaItalia & ",  " & _
            "GiornataDER=" & DatiGioco.GiornataDerelitti & ",  " & _
            "GiornataCL=" & DatiGioco.GiornataChampionsLeague & ", " & _
            "GiornataSpeciale=" & DatiGioco.GiornataSpeciale & " " & _
            "Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSQL, Sql)
    End Sub

    'Public Sub VisualizzaMessaggioInPopup(ByVal MessaggioPopup As String, ByVal PaginaMaster As MasterPage)
    '    Dim Pannello As UpdatePanel = DirectCast(PaginaMaster.FindControl("uppPopup"), UpdatePanel)
    '    Dim divBloccaFinestra As HtmlGenericControl = DirectCast(Pannello.FindControl("divBloccaFinestra"), HtmlGenericControl)
    '    Dim divErrore As HtmlGenericControl = DirectCast(Pannello.FindControl("divPopup"), HtmlGenericControl)
    '    Dim ulPopup As HtmlGenericControl = DirectCast(Pannello.FindControl("ulPopup"), HtmlGenericControl)
    '    Dim Ritorno As String = ""

    '    Ritorno = "<li><span class=""etichettapopup"">" & MessaggioPopup & "</span></li>"
    '    If Left(Ritorno, 4).ToUpper.Trim <> "<LI>" Then
    '        Ritorno = "<li>" & Ritorno
    '    End If
    '    If Right(Ritorno, 5).ToUpper.Trim <> "</LI>" Then
    '        Ritorno = Ritorno & "</li>"
    '    End If
    '    ulPopup.InnerHtml = Ritorno

    '    divBloccaFinestra.Visible = True
    '    divErrore.Visible = True
    'End Sub

    Public Sub VisualizzaMessaggioInPopup(Messaggi() As String, Controllo As Control)
        'Dim Pannello As UpdatePanel = DirectCast(PaginaMaster.FindControl("uppPopup"), UpdatePanel)
        'Dim divBloccaFinestra As HtmlGenericControl = DirectCast(Pannello.FindControl("divBloccaFinestra"), HtmlGenericControl)
        'Dim divErrore As HtmlGenericControl = DirectCast(Pannello.FindControl("divPopup"), HtmlGenericControl)
        'Dim ulPopup As HtmlGenericControl = DirectCast(Pannello.FindControl("ulPopup"), HtmlGenericControl)
        Dim Ritorno As String = "<ul>"

        For i As Integer = 0 To Messaggi.Length - 1
            Ritorno += "<li><span class=""etichettapopup"">" & Messaggi(i) & "</span></li>"
        Next

        Ritorno += "</ul>"
        'ulPopup.InnerHtml = Ritorno

        'divBloccaFinestra.Visible = True
        'divErrore.Visible = True

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     ApreMessaggioPopup('" & Ritorno & "');")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Controllo, Controllo.GetType(), "JVis", sb.ToString(), False)
    End Sub

    Public Function CriptaPassword(ByVal Stringa As String) As String
        Dim PassMDB As String = Trim(Stringa)
        Dim VecchiaPass As String = ""

        Randomize()
        Dim Chiave As Integer = Int(Rnd(1) * 64)
        VecchiaPass = Chr(Chiave)
        For i = 1 To Len(PassMDB)
            VecchiaPass = VecchiaPass & Chr(Asc(Mid(PassMDB, i, 1)) + Chiave)
        Next

        Return VecchiaPass
    End Function

    Public Function DecriptaPassword(ByVal Stringa As String) As String
        Dim Appoggio As String = ""
        Dim Chiave As Integer

        Try
            Chiave = Asc(Left(Trim(Stringa), 1))
            For i = 2 To Len(Stringa)
                Appoggio = Appoggio & Chr(Asc(Mid(Stringa, i, 1)) - Chiave)
            Next
            Appoggio = Trim(Appoggio)
        Catch ex As Exception
            Appoggio = ""
        End Try

        Return Appoggio
    End Function

    Public Function MetteMaiuscoleDopoPunto(Cosa As String) As String
        Dim Ritorno As String = Cosa.ToLower.Trim

        If Ritorno <> "" Then
            If Asc(Mid(Ritorno, 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, 1, 1)) <= Asc("z") Then
                Ritorno = Chr(Asc(Mid(Ritorno, 1, 1)) - 32) & Mid(Ritorno, 2, Len(Ritorno))
            End If
            Ritorno = Ritorno.Replace("  ", " ")
            Ritorno = Ritorno.Replace(". ", ".")
            For i As Integer = 2 To Len(Ritorno)
                If Mid(Ritorno, i, 1) = "." Then
                    If i + 1 < Ritorno.Length Then
                        If Asc(Mid(Ritorno, i + 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, i + 1, 1)) <= Asc("z") Then
                            Ritorno = Mid(Ritorno, 1, i) & Chr(Asc(Mid(Ritorno, i + 1, 1)) - 32) & Mid(Ritorno, i + 2, Len(Ritorno))
                        End If
                    End If
                End If
            Next
            Ritorno = Ritorno.Replace(".", ". ")
        End If

        Return Ritorno
    End Function

    Public Function MetteMaiuscole(Cosa As String) As String
        Dim Ritorno As String = Cosa.ToLower.Trim

        If Ritorno <> "" Then
            If Asc(Mid(Ritorno, 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, 1, 1)) <= Asc("z") Then
                Ritorno = Chr(Asc(Mid(Ritorno, 1, 1)) - 32) & Mid(Ritorno, 2, Len(Ritorno))
            End If
            Ritorno = Ritorno.Replace("  ", " ")
            For i As Integer = 2 To Len(Ritorno)
                If Mid(Ritorno, i, 1) = " " Then
                    If Asc(Mid(Ritorno, i + 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, i + 1, 1)) <= Asc("z") Then
                        Ritorno = Mid(Ritorno, 1, i) & Chr(Asc(Mid(Ritorno, i + 1, 1)) - 32) & Mid(Ritorno, i + 2, Len(Ritorno))
                    End If
                End If
            Next
        End If

        Return Ritorno
    End Function

    Public Function SistemaTestoPerDB(Cosa As String) As String
        Dim Ritorno As String = Cosa

        Ritorno = Ritorno.Replace("'", "''")

        Return Ritorno
    End Function

    Public Function RitornaTurnoCoppe(Db As GestioneDB, ConnSql As Object, Tabella As String, Giornata As Integer) As String
        Dim Ritorno As String = ""
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim QuanteSquadre As Integer

        Sql = "Select Count(*) From " & PrefissoTabelle & Tabella & " Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec(0).Value Is DBNull.Value = False Then
            QuanteSquadre = Rec(0).Value
        Else
            QuanteSquadre = 0
        End If
        Rec.Close()

        Select Case QuanteSquadre
            Case 1
                Ritorno = "Finale"
            Case 2
                Ritorno = "Semifinale"
            Case 4
                Ritorno = "Quarti"
            Case 8
                Ritorno = "Ottavi"
            Case 16
                Ritorno = "Sedicesimi"
            Case 32
                Ritorno = "Trentaduesimi"
            Case Else
                Ritorno = ""
        End Select

        Return Ritorno
    End Function

    Public Function ConverteData(Datella As Date) As String
        Return Datella.Year & "-" & Format(Datella.Month, "00") & "-" & Format(Datella.Day, "00") & " " & Format(Datella.Hour, "00") & ":" & Format(Datella.Minute, "00") & ":" & Format(Datella.Second, "00") & ".000"
    End Function

    Public Sub ScriveMovimentoBilancio(Db As GestioneDB, ConnSql As Object, CodGiocatore As Integer, Importo As String, Tipologia As String, Modalita As String)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Progressivo As Integer = 0

        Sql = "Select Max(Progressivo) From BilancioDettaglio Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & CodGiocatore
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec(0).Value Is DBNull.Value = False Then
            Progressivo = Rec(0).Value
        End If
        Rec.Close()

        Progressivo += 1

        Dim Datella As Date = Now
        Dim sDatella As String = ConverteData(Datella)

        Sql = "Insert Into BilancioDettaglio Values (" & _
            " " & DatiGioco.AnnoAttuale & ", " & _
            " " & CodGiocatore & ", " & _
            " " & Progressivo & ", " & _
            " " & Importo & ", " & _
            "'" & Tipologia & "', " & _
            "'" & sDatella & "', " & _
            "'" & Modalita & "', " & _
            "null, " & _
            "null " & _
            ")"
        Db.EsegueSql(ConnSql, Sql)
    End Sub

    Public Function ScriveNumeroFormattato(ByVal Numero As Single) As String
        Dim Ritorno As String = Numero.ToString("R")
        Dim A As Integer

        'Ritorno = Numero.ToString.Trim
        ' A = InStr(Ritorno, ",")
        A = InStr(Ritorno, PuntoVirgola)

        Dim PrimaParte As Single
        If A > 0 Then
            PrimaParte = Mid(Ritorno, 1, A - 1)
        Else
            PrimaParte = Ritorno
        End If
        Dim sPrimaParte As String = PrimaParte.ToString.Trim
        Dim sPrimo As String = ""
        Dim Contatore As Integer = 0

        For i As Integer = sPrimaParte.Length To 1 Step -1
            Contatore += 1
            If Contatore = 4 Then
                Contatore = 0
                sPrimo += "." & Mid(sPrimaParte, i, 1)
            Else
                sPrimo += Mid(sPrimaParte, i, 1)
            End If
        Next
        sPrimaParte = ""
        For i As Integer = sPrimo.Length To 1 Step -1
            sPrimaParte += Mid(sPrimo, i, 1)
        Next

        Dim sSecondaParte As String = "00"

        If A > 0 Then
            sSecondaParte = Mid(Ritorno, A + 1, 2)
            'sSecondaParte = CInt(SecondaParte).ToString.Trim
            If sSecondaParte.Length = 1 Then
                sSecondaParte += "0"
            Else
                If sSecondaParte.Length > 2 Then
                    sSecondaParte = Mid(sSecondaParte, 1, 2)
                End If
            End If
        End If

        Ritorno = "€ " & sPrimaParte & "," & sSecondaParte

        Return Ritorno
    End Function

    Public Sub DisegnaTabellone(NumeroTurni As Integer, NomeImmagineCoppa As String, NomeCoppa As String, NomeTabella As String, divContenuto As System.Web.UI.HtmlControls.HtmlGenericControl)
        Dim a As New GeneraTabellone
        a.ImpostaQuantiTurni(NumeroTurni)
        Dim Codice As String = a.Esegui()

        Dim Db As New GestioneDB

        Codice = Codice.Replace("****TITOLO****", "Tabellone " & NomeCoppa)
        Codice = Codice.Replace("****IMMCOPPA****", "<img src=""App_Themes/Standard/Images/Icone/" & NomeImmagineCoppa & ".png"" width=""100%"" style=""max-width: 100px;""  />")

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giornata As Integer
            Dim GoalCasaU As String
            Dim GoalFuoriU As String
            Dim RisuUff As String
            Dim GoalCasaR As String
            Dim GoalFuoriR As String
            Dim RisuReale As String
            Dim Gm As New GestioneMail
            Dim Gc As String
            Dim Gf As String
            Dim Ricerca As String
            Dim Scontro As String
            Dim Vincitore As String

            Sql = "Select Min(Giornata) From " & PrefissoTabelle & NomeTabella
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim Minimo As Integer = 0
            If Rec.Eof = False Then
                Minimo = (Rec(0).Value) ' - 2
            End If
            Rec.Close()

            Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori, D.Giocatore As Vincitore From " & PrefissoTabelle & NomeTabella & " A "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
            Sql &= "Left Join Giocatori C On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
            Sql &= "Left Join Giocatori D On A.Anno=D.Anno And D.CodGiocatore=A.Vincitore "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " Order By A.Giornata, A.Partita"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Giornata = ((Rec("Giornata").Value) - Minimo) + 1

                RisuUff = "" & Rec("RisultatoUfficiale").Value
                RisuReale = "" & Rec("RisultatoReale").Value

                If RisuUff.IndexOf("X") > -1 Then
                    GoalCasaU = ""
                    GoalFuoriU = ""

                    GoalCasaR = ""
                    GoalFuoriR = ""
                Else
                    If RisuUff = "" Then
                        GoalCasaU = ""
                        GoalFuoriU = ""
                    Else
                        GoalCasaU = Mid(RisuUff, 1, RisuUff.IndexOf("-"))
                        GoalFuoriU = Mid(RisuUff, RisuUff.IndexOf("-") + 2, 20)
                    End If

                    If RisuReale = "" Then
                        GoalCasaR = ""
                        GoalFuoriR = ""
                    Else
                        GoalCasaR = Mid(RisuReale, 1, RisuReale.IndexOf("-"))
                        GoalFuoriR = Mid(RisuReale, RisuReale.IndexOf("-") + 2, 20)
                    End If
                End If

                Gc = "" & Rec("Casa").Value
                Gf = "" & Rec("Fuori").Value

                If Giornata / 2 <> Int(Giornata / 2) Then
                    Ricerca = "****" & Giornata.ToString & "," & (Giornata + 1).ToString & "-" & Rec("Partita").Value & "****"

                    Scontro = "<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width: 100%; text-align: center; border: 1px solid #C3C3C3; background-color: #D0D0D0;"">"
                    Scontro += "<tr>"
                    Scontro += "<td>"
                    If Gc <> "" Then
                        Scontro += Gm.RitornaImmagineGiocatore(Gc).Replace("30px", "40px")
                    Else
                        Scontro += Gm.RitornaImmagineGiocatore("MORTO").Replace("30px", "40px")
                    End If
                    Scontro += "</td>"
                    Scontro += "<td>"
                    If Gc <> "" Then
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "</span>"
                    Else
                        Scontro += "<span class=""ScrittaTurni"">MORTO</span>"
                    End If
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Scontro += "<tr>"
                    Scontro += "<td>"
                    If Gf <> "" Then
                        Scontro += Gm.RitornaImmagineGiocatore(Gf).Replace("30px", "40px")
                    Else
                        Scontro += Gm.RitornaImmagineGiocatore("MORTO").Replace("30px", "40px")
                    End If
                    Scontro += "</td>"
                    Scontro += "<td>"
                    If Gf <> "" Then
                        Scontro += "<span class=""ScrittaTurni"">" & Gf & "</span>"
                    Else
                        Scontro += "<span class=""ScrittaTurni"">MORTO</span>"
                    End If
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Scontro += "<tr>"
                    Scontro += "<td align=""center"">"
                    If GoalCasaU <> "" And GoalFuoriU <> "" Then
                        Scontro += "<hr />"
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "-" & Gf & "<br />" & GoalCasaU & "-" & GoalFuoriU & "</span>"
                        Scontro += "<span class=""ScrittaTurni""> (" & GoalCasaR & "-" & GoalFuoriR & ")</span>"
                        Scontro += "<hr />"
                    End If
                    Scontro += "</td>"

                    Scontro += "<td align=""center"">"
                    Dim sRicerca As String = "****" & (Giornata + 1).ToString & "-" & Rec("Partita").Value & "****"
                    Scontro += "<span class=""ScrittaTurni"">" & sRicerca & "</span>"
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Scontro += "</table>"
                Else
                    Vincitore = "" & Rec("Vincitore").Value

                    Ricerca = "****" & (Giornata).ToString & "-" & Rec("Partita").Value & "****"

                    If GoalCasaU <> "" And GoalFuoriU <> "" Then
                        Scontro = "<hr />"
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "-" & Gf & "<br />" & GoalCasaU & "-" & GoalFuoriU & "</span>"
                        Scontro += "<span class=""ScrittaTurni""> (" & GoalCasaR & "-" & GoalFuoriR & ")</span>"
                        Scontro += "<hr />"
                    Else
                        Scontro = ""
                    End If

                    If Vincitore <> "" Then
                        Scontro += "<tr style=""border: 1px solid #ADADAD;"">"
                        Scontro += "<td style=""background-color: #A2D0A1;"">"
                        If Gf <> "" Then
                            Scontro += Gm.RitornaImmagineGiocatore(Vincitore).Replace("30px", "40px")
                        End If
                        Scontro += "</td>"
                        Scontro += "<td style=""background-color: #A2D0A1;"">"
                        If Gf <> "" Then
                            Dim r1 As String = Rec("RisultatoUfficiale").Value
                            Dim r2 As String = ""
                            Dim re1 As String = Rec("RisultatoReale").Value
                            Dim re2 As String = ""
                            Sql = "Select * From " & PrefissoTabelle & NomeTabella & " Where Anno=" & DatiGioco.AnnoAttuale & " And GiocCasa=" & Rec("GiocFuori").Value & " And GiocFuori=" & Rec("GiocCasa").Value & " And Partita=" & Rec("Partita").Value
                            Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                            If Not Rec2.Eof Then
                                r2 = Rec2("RisultatoUfficiale").Value
                                re2 = Rec2("RisultatoReale").Value
                            End If
                            Rec2.Close
                            Dim ris As String = ""
                            Dim risR As String = ""
                            If r1.Contains("-") And r2.Contains("-") And re1.Contains("-") And re2.Contains("-") Then
                                Dim rr1() As String = r1.Split("-")
                                Dim rr2() As String = r2.Split("-")
                                Dim t1 As String = (Val(rr1(0)) + Val(rr2(1))).ToString.Trim
                                Dim t2 As String = (Val(rr1(1)) + Val(rr2(0))).ToString.Trim
                                ris = t1 & "-" & t2

                                Dim rre1() As String = re1.Split("-")
                                Dim rre2() As String = re2.Split("-")
                                Dim tr1 As String = (Val(rre1(0)) + Val(rre2(1))).ToString.Trim
                                Dim tr2 As String = (Val(rre1(1)) + Val(rre2(0))).ToString.Trim
                                risR = tr1 & "-" & tr2

                                ris = "<br /> " & ris & " (" & risR & ")"
                            End If

                            Scontro += "<span class=""ScrittaTurni"">" & Vincitore & ris & "</span>"
                        End If
                        Scontro += "</td>"
                        Scontro += "</tr>"
                    End If
                End If

                Codice = Codice.Replace(Ricerca, Scontro)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            Dim Inizio As Integer
            Dim Fine As Integer
            Dim Cambio As String

            Do While Codice.IndexOf("****") > -1
                Inizio = Codice.IndexOf("****")
                For i As Integer = Inizio To Codice.Length
                    If Mid(Codice, i, 4) = "****" Then
                        Fine = i + 4
                        Exit For
                    End If
                Next
                Cambio = Mid(Codice, Inizio, Fine - Inizio)
                Codice = Codice.Replace(Cambio, "")
            Loop

            Codice = Codice.Replace(">-<", "><")
            Codice = Codice.Replace("> (-)<", "><")

            divContenuto.InnerHtml = Codice
        End If

        Db = Nothing
    End Sub

    Public Sub DisegnaTabelloneQuoteCUP(divContenuto As System.Web.UI.HtmlControls.HtmlGenericControl)
        Dim a As New GeneraTabellone
        a.ImpostaQuantiTurni(2)
        Dim Codice As String = a.Esegui()

        Dim Db As New GestioneDB

        Codice = Codice.Replace("****TITOLO****", "Tabellone " & "Quote CUP")
        Codice = Codice.Replace("****IMMCOPPA****", "<img src=""App_Themes/Standard/Images/Icone/quotecup.png"" width=""100%"" style=""max-width: 100px;""  />")

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giornata As Integer
            Dim GoalCasaU As String = ""
            Dim GoalFuoriU As String = ""
            Dim RisuUff As String = ""
            Dim GoalCasaR As String = ""
            Dim GoalFuoriR As String = ""
            Dim RisuReale As String = ""
            Dim Gm As New GestioneMail
            Dim Gc As String
            Dim Gf As String
            Dim Ricerca As String
            Dim Scontro As String
            Dim Vincitore As String

            'Sql = "Select Min(Giornata) From QuoteCUP_ScontriDiretti"
            'Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim Minimo As Integer = 0
            'If Rec.Eof = False Then
            '    Minimo = (Rec(0).Value) ' - 2
            'End If
            'Rec.Close()

            Minimo = 1

            Sql = "Select A.Anno, A1.Progressivo As Giornata, A.Progressivo, A.GiocCasa, A.GiocFuori, B.Giocatore As Casa, C.Giocatore As Fuori, "
            Sql &= "B.Giocatore As Vincitore, "
            Sql &= "( "
            Sql &= "Select "
            Sql &= "	Case "
            Sql &= "		WHEN SUM(Ris1)-SUM(Ris2) > 0 Then B.Giocatore "
            Sql &= "		WHEN SUM(Ris1)-SUM(Ris2) < 0 Then C.Giocatore "
            Sql &= "		WHEN SUM(Ris1)-SUM(Ris2) = 0 Then '' "
            Sql &= "                    End "
            Sql &= "From ( "
            Sql &= "Select RisCasa As Ris1, RisFuori As Ris2 From QuoteCUP_ScontriDiretti Where Anno=A.Anno And Giornata=A.Giornata And Progressivo=A.Progressivo "
            Sql &= "Union All "
            Sql &= "Select RisFuori As Ris1, RisCasa As Ris2 From QuoteCUP_ScontriDiretti Where Anno=A.Anno And Giornata=A.Giornata+1 And Progressivo=A.Progressivo "
            Sql &= ") T1 ) As Vincitore "
            Sql &= "From QuoteCUP_ScontriDiretti A  "
            Sql &= "Left Join (Select ROW_NUMBER() OVER(ORDER BY Giornata) As Progressivo, Giornata "
            Sql &= "From QuoteCUP_ScontriDiretti "
            Sql &= "Group By Giornata "
            Sql &= ") A1 On A.Giornata=A1.Giornata "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa Left Join Giocatori C On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " "
            Sql &="Order By A.Giornata, A.Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Giornata = ((Rec("Giornata").Value) - Minimo) + 1

                Gc = "" & Rec("Casa").Value
                Gf = "" & Rec("Fuori").Value

                If Giornata / 2 <> Int(Giornata / 2) Then
                    Ricerca = "****" & Giornata.ToString & "," & (Giornata + 1).ToString & "-" & Rec("Progressivo").Value & "****"

                    Scontro = "<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width: 100%; text-align: center; border: 1px solid #C3C3C3; background-color: #D0D0D0;"">"
                    Scontro += "<tr>"
                    Scontro += "<td>"
                    If Gc <> "" Then
                        Scontro += Gm.RitornaImmagineGiocatore(Gc).Replace("30px", "40px")
                    End If
                    Scontro += "</td>"
                    Scontro += "<td>"
                    If Gc <> "" Then
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "</span>"
                    End If
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Scontro += "<tr>"
                    Scontro += "<td>"
                    If Gf <> "" Then
                        Scontro += Gm.RitornaImmagineGiocatore(Gf).Replace("30px", "40px")
                    End If
                    Scontro += "</td>"
                    Scontro += "<td>"
                    If Gf <> "" Then
                        Scontro += "<span class=""ScrittaTurni"">" & Gf & "</span>"
                    End If
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Sql = "Select RisCasa, RisFuori From QuoteCUP_ScontriDiretti Where Anno=" & Rec("Anno").Value & " And Progressivo=" & Rec("Progressivo").Value & "  And GiocCasa=" & Rec("GiocCasa").Value & " And GiocFuori=" & Rec("GiocFuori").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Not Rec2.Eof Then
                        GoalCasaU = "" & Rec2("RisCasa").Value
                        GoalFuoriU = "" & Rec2("RisFuori").Value
                    End If
                    Rec2.Close

                    If GoalCasaU <> "" And GoalFuoriU <> "" Then
                        Scontro += "<tr>"
                        Scontro += "<td align=""center"">"
                        Scontro += "<hr />"
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "-" & Gf & "<br />" & GoalCasaU & "-" & GoalFuoriU & "</span>"
                        Scontro += "<hr />"
                        Scontro += "</td>"
                    End If

                    Scontro += "<td align=""center"">"
                    Dim sRicerca As String = "****" & (Giornata + 1).ToString & "-" & Rec("Progressivo").Value & "****"
                    Scontro += "<span class=""ScrittaTurni"">" & sRicerca & "</span>"
                    Scontro += "</td>"
                    Scontro += "</tr>"

                    Scontro += "</table>"
                Else
                    Vincitore = "" & Rec("Vincitore").Value

                    Sql = "Select RisCasa, RisFuori From QuoteCUP_ScontriDiretti Where Anno=" & Rec("Anno").Value & " And Progressivo=" & Rec("Progressivo").Value & "  And GiocCasa=" & Rec("GiocCasa").Value & " And GiocFuori=" & Rec("GiocFuori").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Not Rec2.Eof Then
                        GoalCasaU = "" & Rec2("RisCasa").Value
                        GoalFuoriU = "" & Rec2("RisFuori").Value
                    End If
                    Rec2.Close

                    Ricerca = "****" & (Giornata).ToString & "-" & Rec("Progressivo").Value & "****"

                    'Scontro = "<span class=""ScrittaTurni"">" & GoalCasaU & "-" & GoalFuoriU & "</span>"
                    'Scontro += "<span class=""ScrittaTurni""> (" & GoalCasaR & "-" & GoalFuoriR & ")</span>"

                    If GoalCasaU <> "" And GoalFuoriU <> "" Then
                        Scontro = "<hr />"
                        Scontro += "<span class=""ScrittaTurni"">" & Gc & "-" & Gf & "<br />" & GoalCasaU & "-" & GoalFuoriU & "</span>"
                        Scontro += "<hr />"
                    Else
                        Scontro = ""
                    End If

                    If Vincitore <> "" Then
                        Scontro += "<tr style=""border: 1px solid #ADADAD;"">"
                        Scontro += "<td style=""background-color: #A2D0A1;"">"
                        If Gf <> "" Then
                            Scontro += Gm.RitornaImmagineGiocatore(Vincitore).Replace("30px", "40px")
                        End If
                        Scontro += "</td>"
                        Scontro += "<td style=""background-color: #A2D0A1;"">"
                        If Gf <> "" Then


                            Dim rc2 As String = ""
                            Dim rf2 As String = ""
                            Sql = "Select RisCasa, RisFuori From QuoteCUP_ScontriDiretti Where Anno=" & Rec("Anno").Value & " And Progressivo=" & Rec("Progressivo").Value & "  And GiocCasa=" & Rec("GiocFuori").Value & " And GiocFuori=" & Rec("GiocCasa").Value
                            Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                            If Not Rec2.Eof Then
                                rc2 = Rec2("RisCasa").Value
                                rf2 = Rec2("RisFuori").Value
                            End If
                            Rec2.Close

                            Dim tc As Single = Val(GoalCasaU.Replace(",", ".")) + Val(rf2.Replace(",", "."))
                            Dim tf As Single = Val(GoalFuoriU.Replace(",", ".")) + Val(rc2.Replace(",", "."))

                            Dim ris As String = ""
                            If tc > 0 And tf > 0 Then
                                ris = "<br />" & tc & "-" & tf
                            End If

                            'Dim re1 As String = Rec("RisFuori").Value
                            'Dim re2 As String = ""
                            'Sql = "Select * From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And GiocCasa=" & Rec("GiocFuori").Value & " And GiocFuori=" & Rec("GiocCasa").Value & " And Partita=" & Rec("Partita").Value
                            'Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                            'If Not Rec2.Eof Then
                            '    r2 = Rec2("RisultatoUfficiale").Value
                            '    re2 = Rec2("RisultatoReale").Value
                            'End If
                            'Rec2.Close
                            'Dim ris As String = ""
                            'Dim risR As String = ""
                            'If r1.Contains("-") And r2.Contains("-") And re1.Contains("-") And re2.Contains("-") Then
                            '    Dim rr1() As String = r1.Split("-")
                            '    Dim rr2() As String = r2.Split("-")
                            '    Dim t1 As String = (Val(rr1(0)) + Val(rr2(1))).ToString.Trim
                            '    Dim t2 As String = (Val(rr1(1)) + Val(rr2(0))).ToString.Trim
                            '    ris = t1 & "-" & t2

                            '    Dim rre1() As String = re1.Split("-")
                            '    Dim rre2() As String = re2.Split("-")
                            '    Dim tr1 As String = (Val(rre1(0)) + Val(rre2(1))).ToString.Trim
                            '    Dim tr2 As String = (Val(rre1(1)) + Val(rre2(0))).ToString.Trim
                            '    risR = tr1 & "-" & tr2

                            '    ris = "<br /> " & ris & " (" & risR & ")"
                            'End If


                            Scontro += "<span class=""ScrittaTurni"">" & Vincitore & ris & "</span>"
                        End If
                        Scontro += "</td>"
                        Scontro += "</tr>"
                    End If
                End If

                Codice = Codice.Replace(Ricerca, Scontro)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            Dim Inizio As Integer
            Dim Fine As Integer
            Dim Cambio As String

            Do While Codice.IndexOf("****") > -1
                Inizio = Codice.IndexOf("****")
                For i As Integer = Inizio To Codice.Length
                    If Mid(Codice, i, 4) = "****" Then
                        Fine = i + 4
                        Exit For
                    End If
                Next
                Cambio = Mid(Codice, Inizio, Fine - Inizio)
                Codice = Codice.Replace(Cambio, "")
            Loop

            Codice = Codice.Replace(">-<", "><")
            Codice = Codice.Replace("> (-)<", "><")

            divContenuto.InnerHtml = Codice
        End If

        Db = Nothing
    End Sub

    Public Sub PrendePartiteDellaGiornata(Percorso As String, NomeFile As String, Giornata As Integer)
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            If ControllaConcorsoSpeciale(Db, ConnSQL) = False Then
                Dim NomeFilePartite As String = Percorso & "\" & NomeFile
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String = ""
                Dim Errore As Boolean = False

                Try
                    My.Computer.FileSystem.CreateDirectory(Percorso)
                Catch ex As Exception

                End Try

                Try
                    Dim myWebClient As New WebClient()

                    myWebClient.DownloadFile("http://www.calendarioseriea.biz/giornata.php?id=" & Giornata, NomeFilePartite)
                Catch ex As Exception
                    Errore = True
                End Try

                'Sql = "Create Table PartiteGiornata (Anno Int NOT NULL, Giornata Int NOT NULL, Partita Int NOT NULL, Casa Varchar(30), Fuori Varchar(30), Ris1 Int, Ris2 Int"
                'Sql &= " Constraint [PK_PartiteGiornata] PRIMARY KEY CLUSTERED "
                'Sql &= "( "
                'Sql &= "	[Anno] Asc, "
                'Sql &= "	[Giornata] Asc, "
                'Sql &= "	[Partita] Asc "
                'Sql &= ") WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] "
                'Sql &= ") ON [PRIMARY]"
                'Try
                '    Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
                'Catch ex As Exception
                'End Try

                Dim gf As New GestioneFilesDirectory
                Dim Filetto As String
                Dim Partita As Integer = 0

                If (Not Errore) Then
                    Sql = "Delete From PartiteGiornata Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata
                    Db.EsegueSql(ConnSQL, Sql)

                    Filetto = gf.LeggeFileIntero(NomeFilePartite)
                    Dim Blocco As String
                    Dim Blocco2 As String = ""
                    Dim Quanti As Integer

                    Dim identificatoreInizio As String = "<div class=""td col-xs-12 col-sm-7"">"

                    Do While Filetto.IndexOf(identificatoreInizio) > -1
                        Blocco = Mid(Filetto, Filetto.IndexOf(identificatoreInizio), Filetto.Length)

                        Quanti = 0
                        Blocco2 = ""
                        For i = 1 To Blocco.Length
                            If Mid(Blocco, i, 6) = "</div>" Then
                                Quanti += 1
                                If Quanti = 5 Then
                                    Blocco2 = Mid(Blocco, 1, i)
                                    Exit For
                                End If
                            End If
                        Next

                        Blocco = Blocco2
                        If Blocco2 <> "" Then
                            Dim Prima As String = Mid(Blocco2, Blocco2.IndexOf("<a href="""), Blocco2.Length)
                            Prima = Mid(Prima, 1, Prima.IndexOf("</a>"))

                            Blocco2 = Blocco2.Replace(Prima, "")

                            Prima = Mid(Prima, Prima.IndexOf(">") + 2).Trim

                            Dim Seconda As String = Mid(Blocco2, Blocco2.IndexOf("<a href="""), Blocco2.Length)
                            Seconda = Mid(Seconda, 1, Seconda.IndexOf("</a>"))
                            Seconda = Mid(Seconda, Seconda.IndexOf(">") + 2).Trim

                            Dim Risultato As String = Mid(Blocco2, Blocco2.IndexOf("<div class=""fourth col-xs-12 col-sm-3 col-sm-pull-45"">"), Blocco2.Length)
                            Risultato = Mid(Risultato, Risultato.IndexOf(">") + 2, Risultato.Length)
                            Risultato = Mid(Risultato, 1, Risultato.IndexOf("<")).Trim

                            If Risultato.IndexOf("-") > -1 Then
                                Dim risCasa As String = Mid(Risultato, 1, Risultato.IndexOf("-")).Trim
                                Dim risFuori As String = Mid(Risultato, Risultato.IndexOf("-") + 2, Risultato.Length).Trim

                                Partita += 1

                                Sql = "Insert Into PartiteGiornata Values ("
                                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                                Sql &= " " & Giornata & ", "
                                Sql &= " " & Partita & ", "
                                Sql &= "'" & SistemaTestoPerDB(Prima) & "', "
                                Sql &= "'" & SistemaTestoPerDB(Seconda) & "', "
                                Sql &= " " & risCasa & ", "
                                Sql &= " " & risFuori & " "
                                Sql &= ")"
                                Db.EsegueSql(ConnSQL, Sql)
                            End If
                        Else
                            Exit Do
                        End If

                        Filetto = Filetto.Replace(Blocco, "")
                    Loop
                End If

                Dim AnnoInizio As String = ""
                Dim Serie() As String = {"a", "b", "c"}

                Sql = "Select AnnoInizio From Anni Where Anno=" & DatiGioco.AnnoAttuale
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                AnnoInizio = Rec("AnnoInizio").Value
                Rec.Close()

                For i As Integer = 0 To 2
                    Errore = False

                    Try
                        Kill(NomeFilePartite)
                    Catch ex As Exception

                    End Try

                    Try
                        Dim myWebClient As New WebClient()

                        myWebClient.DownloadFile("http://www.calcio.com/tutte_le_partite/ita-lega-pro-girone-" & Serie(i) & "-" & AnnoInizio & "-" & Val(AnnoInizio) + 1.ToString.Trim, NomeFilePartite)
                    Catch ex As Exception
                        Errore = True
                    End Try

                    If (Not Errore) Then
                        Filetto = gf.LeggeFileIntero(NomeFilePartite)
                        Dim Ricerca1 As String = Giornata.ToString.Trim & ". Giornata"
                        Dim Ricerca2 As String = (Giornata + 1).ToString.Trim & ". Giornata"
                        Dim Campo As String
                        Dim Squadra1 As String = ""
                        Dim Squadra2 As String = ""
                        Dim Ris As String = ""
                        Dim Ok As Boolean

                        If Filetto.IndexOf(Ricerca1) > -1 Then
                            Filetto = Mid(Filetto, Filetto.IndexOf(Ricerca1), Filetto.Length)
                            If Filetto.IndexOf(Ricerca2) > -1 Then
                                Filetto = Mid(Filetto, 1, Filetto.IndexOf(Ricerca2))

                                For k As Long = 1 To Filetto.Length
                                    If Mid(Filetto, k, 9) = "/squadre/" Then
                                        Campo = Mid(Filetto, k, Filetto.Length)
                                        If Campo.IndexOf("</a>") > -1 Then
                                            Campo = Mid(Campo, 1, Campo.IndexOf("</a>"))
                                            Ok = False
                                            For z As Integer = Campo.Length To 1 Step -1
                                                If Mid(Campo, z, 1) = ">" Then
                                                    Campo = Mid(Campo, z + 1, Campo.Length)
                                                    Ok = True
                                                    Exit For
                                                End If
                                            Next
                                            If Ok Then
                                                If Squadra1 = "" Then
                                                    Squadra1 = Campo
                                                Else
                                                    If Squadra2 = "" Then
                                                        Squadra2 = Campo
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        If Mid(Filetto, k, 10) = "Tabellino " Then
                                            Campo = Mid(Filetto, k, Filetto.Length)
                                            If Campo.IndexOf("</a>") > -1 Then
                                                Campo = Mid(Campo, 1, Campo.IndexOf("</a>"))
                                                Ok = False
                                                For z As Integer = Campo.Length To 1 Step -1
                                                    If Mid(Campo, z, 1) = ">" Then
                                                        Campo = Mid(Campo, z + 1, Campo.Length)
                                                        Ok = True
                                                        Exit For
                                                    End If
                                                Next
                                                If Ok Then
                                                    If Ris = "" Then
                                                        Ris = Campo

                                                        If Ris.IndexOf("(") > -1 Then
                                                            Ris = Mid(Ris, 1, Ris.IndexOf("(")).Trim

                                                            Dim riss() As String = Ris.Split(":")

                                                            If Squadra1 <> "" And Squadra2 <> "" And Ris <> "" Then
                                                                Partita += 1

                                                                Sql = "Insert Into PartiteGiornata Values ("
                                                                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                                                                Sql &= " " & Giornata & ", "
                                                                Sql &= " " & Partita & ", "
                                                                Sql &= "'" & SistemaTestoPerDB(Squadra1) & "', "
                                                                Sql &= "'" & SistemaTestoPerDB(Squadra2) & "', "
                                                                Sql &= " " & riss(0) & ", "
                                                                Sql &= " " & riss(1) & " "
                                                                Sql &= ")"
                                                                Db.EsegueSql(ConnSQL, Sql)
                                                            End If
                                                        End If

                                                        Squadra1 = ""
                                                        Squadra2 = ""
                                                        Ris = ""
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If
        End If
    End Sub
End Module
