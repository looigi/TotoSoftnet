Imports System.IO
Imports System.Net

Public Class prova
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If "" & Session("CodGiocatore") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            LeggePagamenti(Db, ConnSQL)
            Dim ControlloMailCompleto As String = LeggeSeControlloMailCompleto(Db, ConnSQL)

            If Page.IsPostBack = False Then
                If Request.QueryString("AvanzaGiornata") <> "" Then
                    AvanzaGiornata()
                End If
                If Request.QueryString("AvanzaSpeciale") <> "" Then
                    AvanzaSpeciale()
                End If

                divColonnaPropriaDaEffettuare.Visible = False
                divColonnaPropriaGiaCompilata.Visible = False
                divChiusuraConcorso.Visible = False
                divSondaggiGest.Visible = False
                divVincitore.Visible = False
                divAggiornaRisultati.Visible = False

                Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
                Dim Altro As String = ""

                If Speciale = True Then
                    Altro = "Speciale: "
                    lblGiornata.Text = "Concorso " & DatiGioco.GiornataSpeciale
                Else
                    lblGiornata.Text = "Concorso " & DatiGioco.Giornata
                End If

                'cmdMischiaAbbinamenti.Visible = False
                'If Session("Permesso") = Permessi.Amministratore Then
                '    cmdMischiaAbbinamenti.Visible = True
                'End If

                'If Session("Permesso") = Permessi.Amministratore Then
                '    divPartiteGiornata.Visible = True
                'Else
                '    divPartiteGiornata.Visible = False
                'End If

                Select Case DatiGioco.StatoConcorso
                    Case ValoriStatoConcorso.Nessuno
                        lblModalitaConcorso.Text = "Nessuno"
                    Case ValoriStatoConcorso.Aperto
                        AzionaCountdown()
                        ControllaSchedinaGiocata(Db, ConnSQL)

                        lblModalitaConcorso.Text = Altro & "Aperto"
                    Case ValoriStatoConcorso.DaControllare
                        If Speciale = False Then
                            PrendeRisultati(Db, ConnSQL, DatiGioco.Giornata, Speciale)
                        Else
                            PrendeRisultati(Db, ConnSQL, DatiGioco.GiornataSpeciale, Speciale)
                        End If

                        If Session("Permesso") = Permessi.Amministratore Then
                            PrendePartiteAggiornate(Db, ConnSQL, DatiGioco.GiornataSpeciale, Speciale)

                            divAggiornaRisultati.Visible = True
                        End If

                        lblModalitaConcorso.Text = Altro & "Da controllare"
                    Case ValoriStatoConcorso.Chiuso
                        lblModalitaConcorso.Text = Altro & "Chiuso"

                        If Speciale = False Then
                            PrendeVincitoriUltimo(Db, ConnSQL, Speciale, DatiGioco.Giornata)
                        Else
                            PrendeVincitoriUltimo(Db, ConnSQL, Speciale, DatiGioco.GiornataSpeciale)
                        End If
                    Case ValoriStatoConcorso.AnnoChiuso
                        lblModalitaConcorso.Text = "Anno chiuso"
                End Select

                If DatiGioco.StatoConcorso <> ValoriStatoConcorso.Chiuso Or (DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso And (ControlloMailCompleto = "S" Or LeggeSeControllato(Db, ConnSQL))) Then
                    PrendeClassifiche(Db, ConnSQL, "Generale", divClassificaGenerale,
                    lblPosClass1, lblClassifica1, lblPunti1, imgClass1,
                    lblPosClass2, lblclassifica2, lblPunti2, imgClass2,
                    lblPosClass3, lblClassifica3, lblPunti3, imgClass3,
                    lblPosClass4, lblClassifica4, lblPunti4, imgClass4,
                    lblPosClass5, lblClassifica5, lblPunti5, imgClass5
                    )

                    PrendeClassifiche(Db, ConnSQL, "Campionato", divClassificaCampionato,
                    lblPosCamp1, lblClassificaCamp1, lblPuntiCamp1, imgPosiCamp1,
                    lblPosCamp2, lblClassificaCamp2, lblPuntiCamp2, imgPosiCamp2,
                    lblPosCamp3, lblClassificaCamp3, lblPuntiCamp3, imgPosiCamp3,
                    lblPosCamp4, lblClassificaCamp4, lblPuntiCamp4, imgPosiCamp4,
                    lblPosCamp5, lblClassificaCamp5, lblPuntiCamp5, imgPosiCamp5
                    )

                    PrendeClassifiche(Db, ConnSQL, "Risultati", divClassificaRisultati,
                    lblPosRis1, lblClassificaRis1, lblPuntiRis1, imgPosiRis1,
                    lblPosRis2, lblClassificaRis2, lblPuntiRis2, imgPosiRis2,
                    lblPosRis3, lblClassificaRis3, lblPuntiRis3, imgPosiRis3,
                    lblPosRis4, lblClassificaRis4, lblPuntiRis4, imgPosiRis4,
                    lblPosRis5, lblClassificaRis5, lblPuntiRis5, imgPosiRis5
                    )

                    PrendeClassifiche(Db, ConnSQL, "Speciali", divClassificaSpeciali,
                    lblPosSpec1, lblClassificaSpec1, lblPuntiSpec1, imgPosiSpec1,
                    lblPosSpec2, lblClassificaSpec2, lblPuntiSpec2, imgPosiSpec2,
                    lblPosSpec3, lblClassificaSpec3, lblPuntiSpec3, imgPosiSpec3,
                    lblPosSpec4, lblClassificaSpec4, lblPuntiSpec4, imgPosiSpec4,
                    lblPosSpec5, lblClassificaSpec5, lblPuntiSpec5, imgPosiSpec5
                    )
                End If

                ControllaTappo(Db, ConnSQL)
                ControllaSondaggi(Db, ConnSQL)

                If Request.QueryString("VisualizzaSondaggio") <> "" Then
                    divSondaggiGest.Visible = True
                End If

                PrendeRiconDison(Db, ConnSQL)
                VisualizzaPosizioni(Db, ConnSQL)
                LeggeStatistiche()
                ControllaEventi(Speciale)

                'ControllaModalitaDiTest()

                Dim VisualizzaStatistiche As Boolean = True

                If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
                    If ControlloMailCompleto = "N" Then
                        If Not LeggeSeControllato(Db, ConnSQL) Then
                            VisualizzaStatistiche = False
                        End If
                    End If
                End If

                If Not VisualizzaStatistiche Then
                    divClassificaGenerale.Visible = False
                    divClassificaCampionato.Visible = False
                    divClassificaRisultati.Visible = False
                    divClassificaSpeciali.Visible = False
                    divVincitore.Visible = False
                    divPosizioni.Visible = False
                Else
                    If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
                        divVincitore.Visible = True
                    End If
                End If
            End If

            Dim Sql As String = "Delete From SemaforoPerControllo Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And CodGiocatore=" & Session("CodGiocatore")
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaEventi(Speciale As Boolean)
        If DatiGioco.StatoConcorso = ValoriStatoConcorso.AnnoChiuso Or DatiGioco.StatoConcorso = ValoriStatoConcorso.Nessuno Or Speciale = True Then
            divEventi.Visible = False
            Exit Sub
        End If

        Dim Testo As String

        Dim ev As New Eventi
        Dim g As Integer

        If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
            ScrittaScorrevole = "Prossimi eventi:"
            g = DatiGioco.Giornata + 1
        Else
            ScrittaScorrevole = "Eventi giornata:"
            g = DatiGioco.Giornata
        End If
        Testo = ev.ControllaPresenzaEventi(True, g)

        lblEventi.Text = ScrittaScorrevole

        Dim Righe() As String = Testo.Split("-")
        Dim s As StringBuilder = New StringBuilder

        s.Append("<ul>")
        For i As Integer = 0 To Righe.Length - 1
            s.Append("<li>" & Righe(i).Trim & "</li>")
        Next
        s.Append("</ul>")

        ltlEventi.Text = s.ToString

        ev = Nothing
    End Sub

    Private Sub PrendeClassifiche(Db As GestioneDB, ConnSQL As Object, TipoClassifica As String, nomeDiv As HtmlGenericControl,
                                    lblPosClass1 As Label, lblClassifica1 As Label, lblpunti1 As Label, imgClass1 As Image,
                                    lblPosClass2 As Label, lblClassifica2 As Label, lblpunti2 As Label, imgClass2 As Image,
                                    lblPosClass3 As Label, lblClassifica3 As Label, lblpunti3 As Label, imgClass3 As Image,
                                    lblPosClass4 As Label, lblClassifica4 As Label, lblpunti4 As Label, imgClass4 As Image,
                                    lblPosClass5 As Label, lblClassifica5 As Label, lblpunti5 As Label, imgClass5 As Image
                                    )
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String = ""
        Dim gc As New StringheClassifiche
        Dim g As New GestioneMail
        Dim Quale As Integer = -1
        Dim Quanti As Integer = 0

        Select Case TipoClassifica
            Case "Generale"
                Sql = gc.RitornaStringaClassificaGenerale

            Case "Campionato"
                Sql = "Select A.CodGiocatore, A.Giocatore As Giocatore, Punti As TotPunti From AppoScontriDiretti A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And A.CodGiocVisua = 1 " &
                    "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"

            Case "Risultati"
                Sql = "Select PuntiRisultati As TotPunti, CodGiocatore, Giocatore From (Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore  From Risultati A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & DatiGioco.Giornata & " And B.Cancellato='N' " &
                    "Group By B.CodGiocatore, B.Giocatore " &
                    ") A Order By PuntiRisultati Desc"

            Case "Speciali"
                Sql = "Select Sum(PuntiTot) As TotPunti, B.CodGiocatore, B.Giocatore From ClassificaSpeciale A " &
                    "Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore = B.CodGiocatore " &
                    "Where A.idAnno = " & DatiGioco.AnnoAttuale & " And B.Cancellato='N' " &
                    "Group By B.CodGiocatore, B.Giocatore " &
                    "Order By 1 Desc"
        End Select

        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            Quanti += 1
            If Rec("CodGiocatore").Value = Session("CodGiocatore") Then
                Quale = Quanti
            End If

            Rec.MoveNext()
        Loop

        nomeDiv.Visible = False

        If Quanti = 0 Then
            Exit Sub
        End If

        nomeDiv.Visible = True

        Dim Inizio As Integer = Quale - 2
        If Inizio < 1 Then
            Inizio = 1
        End If

        Dim Fine As Integer = Quale + 2
        If Fine > Quanti Then
            Inizio = Quanti - 4
        End If

        Rec.MoveFirst()
        For i As Integer = 1 To Inizio - 1
            Rec.MoveNext()
        Next

        Dim Posi As String
        Dim Gioc As String
        Dim totPunti As String
        Dim Colore As System.Drawing.Color

        For i As Integer = 1 To 5
            If Session("CodGiocatore") = Rec("CodGiocatore").Value Then
                Colore = System.Drawing.ColorTranslator.FromHtml("#D6D84F")
            Else
                Colore = System.Drawing.ColorTranslator.FromHtml("#ffffff")
            End If
            Posi = (Inizio + (i - 1)).ToString.Trim
            If Posi.Length = 1 Then Posi = "0" & Posi
            Gioc = Rec("Giocatore").Value
            If Gioc.Length > 13 Then
                Gioc = Mid(Gioc, 1, 12) & "..."
            End If
            Gioc = "- " & Gioc
            totPunti = Rec("TotPunti").Value.ToString.Trim
            'For k As Integer = totPunti.Length To 5
            '    totPunti = "0" & totPunti
            'Next

            Dim Immagine As String ' = g.RitornaImmagineGiocatore(Rec("Giocatore").Value, True)

            Immagine = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/" & Rec("Giocatore").Value.ToString.Trim & ".jpg"
            If File.Exists(Server.MapPath(".") & "\" & Immagine.Replace("/", "\")) = False Then
                Immagine = "App_Themes/Standard/Images/Giocatori/Sconosciuto.png"
            End If

            Select Case i
                Case 1
                    lblPosClass1.Text = Posi
                    lblClassifica1.Text = Gioc
                    lblpunti1.Text = totPunti

                    lblPosClass1.ForeColor = Colore
                    lblClassifica1.ForeColor = Colore
                    lblpunti1.ForeColor = Colore

                    imgClass1.ImageUrl = Immagine
                Case 2
                    lblPosClass2.Text = Posi
                    lblClassifica2.Text = Gioc
                    lblpunti2.Text = totPunti

                    lblPosClass2.ForeColor = Colore
                    lblClassifica2.ForeColor = Colore
                    lblpunti2.ForeColor = Colore

                    imgClass2.ImageUrl = Immagine
                Case 3
                    lblPosClass3.Text = Posi
                    lblClassifica3.Text = Gioc
                    lblpunti3.Text = totPunti

                    lblPosClass3.ForeColor = Colore
                    lblClassifica3.ForeColor = Colore
                    lblpunti3.ForeColor = Colore

                    imgClass3.ImageUrl = Immagine
                Case 4
                    lblPosClass4.Text = Posi
                    lblClassifica4.Text = Gioc
                    lblpunti4.Text = totPunti

                    lblPosClass4.ForeColor = Colore
                    lblClassifica4.ForeColor = Colore
                    lblpunti4.ForeColor = Colore

                    imgClass4.ImageUrl = Immagine
                Case 5
                    lblPosClass5.Text = Posi
                    lblClassifica5.Text = Gioc
                    lblpunti5.Text = totPunti

                    lblPosClass5.ForeColor = Colore
                    lblClassifica5.ForeColor = Colore
                    lblpunti5.ForeColor = Colore

                    imgClass5.ImageUrl = Immagine
            End Select

            Rec.MoveNext()
        Next

        Rec.Close()

        g = Nothing
        gc = Nothing
    End Sub

    Private Sub VisualizzaPosizioni(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Posizioni As String = ""

        Sql = "Select * From Posizioni Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "Giornata<=" & DatiGioco.Giornata & " " & _
            "And CodGiocatore=" & Session("CodGiocatore") & " " & _
            "Order By Giornata"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            Posizioni += Rec("Posizione").Value & ","

            Rec.MoveNext()
        Loop
        Rec.Close()

        If Posizioni <> "" Then
            Posizioni = Mid(Posizioni, 1, Posizioni.Length - 1)

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script type='text/javascript' language='javascript'>")
            sb.Append("     VisuaChart('" & Posizioni & "');")
            sb.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JVis", sb.ToString(), False)
        Else
            divPosizioni.Visible = False
        End If
    End Sub

    Private Function LeggeSeControlloMailCompleto(Db As GestioneDB, ConnSQL As Object) As String
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Ritorno As String = "S"

        Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = False Then
            Ritorno = Rec("InvioMailCompleta").Value
        End If
        Rec.Close()

        Return Ritorno
    End Function

    Private Function LeggeSeControllato(Db As GestioneDB, ConnSQL As Object) As Boolean
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Ritorno As Boolean = False

        Sql = "Select * From ControlliConcorsoGiocatore Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiocatore=" & Session("CodGiocatore") & " And idGiornata=" & DatiGioco.Giornata
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = False Then
            Ritorno = True
        End If
        Rec.Close()

        Return Ritorno
    End Function

    Private Sub LeggePagamenti(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim totVers As Single
        Dim Pagato As Single
        Dim perc As Integer

        Sql = "Select * From Bilancio Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = False Then
            totVers = Rec("TotVersamento").Value
            Pagato = Rec("Reali").Value + Rec("Vinti").Value + Rec("Amici").Value
            If totVers > 0 Then
                perc = ((Pagato / totVers) * 100)
            Else
                perc = 0
            End If
        End If
        Rec.Close()

        If perc < 50 Then
            APag.Attributes("class") = "quick-button metro red span3"
        Else
            If perc > 49 And perc < 76 Then
                APag.Attributes("class") = "quick-button metro yellow span3"
            Else
                If perc > 75 Then
                    APag.Attributes("class") = "quick-button metro green span3"
                End If
            End If
        End If

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     ImpostaPagamenti('" & perc.ToString.Trim & "', '" & totVers.ToString.Trim & "', '" & Pagato & "');")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JPag", sb.ToString(), False)
    End Sub

    Private Sub PrendeVincitoriUltimo(Db As GestioneDB, ConnSQL As Object, Speciale As Boolean, Giornata As Integer)
        Dim g As New GestioneMail
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String

        If Speciale = False Then
            Sql = "Select B.Giocatore From Primi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Else
            divVincitore.Visible = False
            Exit Sub
            ' Sql = "Select Top 1 B.Giocatore From ClassificaSpeciale A Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore =B.CodGiocatore Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiornata = " & Giornata + 100 & " Order By PuntiTot Desc"
        End If

        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = False Then
            divVincitore.Visible = True
            imgVincitore.ImageUrl = g.RitornaImmagineGiocatore(Rec("Giocatore").Value, True)
            lblVincitore.Text = "I - " & Rec("Giocatore").Value
        End If
        Rec.Close()

        If Speciale = False Then
            Sql = "Select B.Giocatore From Secondi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                imgSecondo.ImageUrl = g.RitornaImmagineGiocatore(Rec("Giocatore").Value, True)
                lblSecondo.Text = "II - " & Rec("Giocatore").Value
            End If
            Rec.Close()

            Sql = "Select B.Giocatore From Ultimi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                imgUltimo.ImageUrl = g.RitornaImmagineGiocatore(Rec("Giocatore").Value, True)
                lblUltimo.Text = "U. - " & Rec("Giocatore").Value
            End If
            Rec.Close()
        End If

        g = Nothing
    End Sub

    Private Sub PrendeRiconDison(Db As GestioneDB, ConnSQL As Object)
        Dim Conta As Integer = 0
        Dim Conta2 As Integer = 0
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim CiSonoRD As Boolean = False
        Dim Sql As String

        Sql = "Select Giornata, B.Immagine, B.Punti, B.Descrizione From RiconDisonGioc A " &
            "Left Join RiconDison B On A.idPremio=B.idPremio " &
            "Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGioc=" & Session("CodGiocatore") & " Order By Giornata"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        ltRicDis.Text = ""
        Do Until Rec.Eof
            ltRicDis.Text += "<img id='ImageRD" & Conta2 & "' src='App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value & "' alt='' height='26px' width='26px' title='Giornata " & Rec("Giornata").Value & " - " & Rec("Descrizione").Value & ": " & Rec("Punti").Value & "' />&nbsp;"
            Conta += 1
            If Conta = 7 Then
                Conta = 0
                ltRicDis.Text += "<br />"
            End If
            Conta2 += 1
            CiSonoRD = True

            Rec.MoveNext()
        Loop
        Rec.Close()

        If CiSonoRD = True Then
            divRD.Visible = True
        Else
            divRD.Visible = False
        End If
    End Sub

    Private Sub AvanzaGiornata()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim Jolly As Integer
            Dim Oggi As Date = Now.AddDays(5)
            Dim sOggi As String = ConverteData(Oggi)

            Randomize()
            Jolly = Int(Rnd(1) * 14) + 1
            If Jolly > 14 Then Jolly = 7

            ' Pulizia tabella partita speciale
            Sql = "Delete From PartiteSpeciali Where idAnno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)
            ' Pulizia tabella partita speciale

            Sql = "Update DatiDiGioco Set Giornata=Giornata+1, Stato=1, ChiusuraConcorso='" & sOggi & "', PartitaJolly = " & Jolly & " Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            Response.Redirect("Default.aspx")

            Dim Messaggi() As String = {"Fatto..."}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    Private Sub AvanzaSpeciale()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim Jolly As Integer
            Dim Oggi As Date = Now.AddDays(5)
            Dim sOggi As String = ConverteData(Oggi)

            Randomize()
            Jolly = Int(Rnd(1) * 14) + 1
            If Jolly > 14 Then Jolly = 7

            ' Pulizia tabella partita speciale
            Sql = "Delete From PartiteSpeciali Where idAnno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)
            ' Pulizia tabella partita speciale

            Sql = "Update DatiDiGioco Set GiornataSpeciale=GiornataSpeciale+1, Stato=1, ChiusuraConcorso='" & sOggi & "', PartitaJolly = " & Jolly & " Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into PartiteSpeciali Values (" & DatiGioco.AnnoAttuale & ", 1)"
            Db.EsegueSql(ConnSQL, Sql)

            Response.Redirect("Default.aspx")

            Dim Messaggi() As String = {"Fatto..."}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    'Private Sub ControllaModalitaDiTest()
    '    If ModalitaLocale = True Then
    '        If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Or DatiGioco.StatoConcorso = ValoriStatoConcorso.Nessuno Then
    '            divAvanzaGiornata.Visible = True
    '            divAvanzaSpeciale.Visible = True
    '        Else
    '            divAvanzaGiornata.Visible = False
    '            divAvanzaSpeciale.Visible = False
    '        End If
    '    Else
    '        divAvanzaGiornata.Visible = False
    '        divAvanzaSpeciale.Visible = False
    '    End If
    'End Sub

    Private Sub ControllaTappo(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select * From Tappi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = True Then
            divControlloTappo.Visible = True
        Else
            divControlloTappo.Visible = False
        End If
        Rec.Close()
    End Sub

    Private Sub ControllaSchedinaGiocata(Db As GestioneDB, ConnSQL As Object)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
        Dim gg As Integer

        If Speciale = True Then
            gg = DatiGioco.GiornataSpeciale + 100
        Else
            gg = DatiGioco.Giornata
        End If

        Sql = "Select * From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore") & " And Giornata=" & gg
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = True Then
            divColonnaPropriaDaEffettuare.Visible = True
            divColonnaPropriaGiaCompilata.Visible = False
        Else
            divColonnaPropriaDaEffettuare.Visible = False
            divColonnaPropriaGiaCompilata.Visible = True
        End If
        Rec.Close()
    End Sub

    Private Sub AzionaCountdown()
        If IsDate(DatiGioco.ChiusuraConcorso) = True Then
            divChiusuraConcorso.Visible = True

            Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

            If DataArrivo > Now Then
                lblScadenzaSch.Text = "Chiusura concorso fra:"

                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script type='text/javascript' language='javascript'>")
                sb.Append("     setEnd(" & DataArrivo.Year & ", " & DataArrivo.Month & ", " & DataArrivo.Day & ", " & DataArrivo.Hour & ", " & DataArrivo.Minute & ", " & DataArrivo.Second & ");")
                sb.Append("</script>")

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
            Else
                lblScadenzaSch.Text = "Concorso chiuso"

                imgG1.Visible = False
                imgG2.Visible = False

                imgH1.Visible = False
                imgH2.Visible = False

                imgM1.Visible = False
                imgM2.Visible = False

                imgS1.Visible = False
                imgS2.Visible = False
            End If
        Else
            divChiusuraConcorso.Visible = False
        End If
    End Sub

    ' SONDAGGI
    Private Sub ControllaSondaggi(Db As GestioneDB, ConnSQL As Object)
        divSondaggi.Visible = False
        hdnIdSondaggio.Value = ""
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim DataAtt As String = ConverteData(Now)
        Dim idSondaggio As Integer = -1
        Dim Anno As Integer

        Sql = "Select * From Sondaggi Where DataFine>='" & DataAtt & "' And Chiuso='N'"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec.Eof = False Then
            divSondaggi.Visible = True
            idSondaggio = Rec("idSondaggio").Value
            Anno = Rec("Anno").Value
            lblSondaggio.Text = Rec("TestoSondaggio").Value
            lblTitoloSondaggio.Text = "Sondaggio N. " & idSondaggio & " - Scadenza " & Rec("DataFine").Value
            cmdRisposta1.Text = Rec("Risposta1").Value
            cmdRisposta2.Text = Rec("Risposta2").Value
            cmdRisposta3.Text = Rec("Risposta3").Value
            hdnIdSondaggio.Value = idSondaggio
        End If
        Rec.Close()

        If idSondaggio > -1 Then
            Sql = "Select * From SondaggiRisposte Where idSondaggio=" & idSondaggio & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                idTastiSondaggio.Visible = False
                imgStatSondaggio.Visible = True
                Select Case Rec("Risposta").Value
                    Case "1"
                        lblRisposta.Text = cmdRisposta1.Text
                    Case "2"
                        lblRisposta.Text = cmdRisposta2.Text
                    Case "3"
                        lblRisposta.Text = cmdRisposta3.Text
                End Select
                lblGestSondaggio.Text = "Hai già risposto '" & lblRisposta.Text & "'"
                lblRisposta.Text = "Hai risposto '" & lblRisposta.Text & "' in data " & Rec("DataRisposta").Value

                CreaImmagineSondaggio(Db, ConnSQL, idSondaggio, Anno)
            Else
                idTastiSondaggio.Visible = True
                imgStatSondaggio.Visible = False
                lblGestSondaggio.Text = "Attesa risposta"
            End If
            Rec.Close()
        End If
    End Sub

    Private Sub CreaImmagineSondaggio(Db As GestioneDB, ConnSql As Object, id As Integer, Anno As Integer)
        Dim Sql As String
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Giocatori As Integer
        Dim Votanti As Integer
        Dim Risposte(2) As Integer
        Dim RispStringa() As String = {cmdRisposta1.Text, cmdRisposta2.Text, cmdRisposta3.Text}

        Sql = "Select Count(*) From Giocatori Where Anno=" & Anno
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Giocatori = Rec(0).Value
        Rec.Close()

        Sql = "Select Count(*) From SondaggiRisposte Where idSondaggio=" & id
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Votanti = Rec(0).Value
        Rec.Close()

        Sql = "Select Risposta, Count(*) From SondaggiRisposte Where idSondaggio=" & id & " Group By Risposta"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Risposte(Rec("Risposta").Value - 1) = Rec(1).Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        Dim NomeImmagineStat As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale.ToString.Trim & "\" & Session("Nick") & ".stat.png"
        Dim NomeImmagineUrl As String = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale.ToString.Trim & "/" & Session("Nick") & ".stat.png"

        Try
            Kill(NomeImmagineStat)
        Catch ex As Exception

        End Try

        Dim gi As New GestioneImmagini
        gi.DisegnaStatisticheSondaggi(Giocatori, Votanti, Risposte, RispStringa, NomeImmagineStat)
        gi = Nothing

        imgStatSondaggio.ImageUrl = NomeImmagineUrl
    End Sub

    Protected Sub cmdRisposta1_Click(sender As Object, e As EventArgs) Handles cmdRisposta1.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" & _
                " " & hdnIdSondaggio.Value & ", " & _
                " " & Session("CodGiocatore") & ", " & _
                "1, " & _
                "'" & ConverteData(Now) & "' " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille..."}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ControllaSondaggi(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta2_Click(sender As Object, e As EventArgs) Handles cmdRisposta2.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" & _
                " " & hdnIdSondaggio.Value & ", " & _
                " " & Session("CodGiocatore") & ", " & _
                "2, " & _
                "'" & ConverteData(Now) & "' " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ControllaSondaggi(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta3_Click(sender As Object, e As EventArgs) Handles cmdRisposta3.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" & _
                " " & hdnIdSondaggio.Value & ", " & _
                " " & Session("CodGiocatore") & ", " & _
                "3, " & _
                "'" & ConverteData(Now) & "' " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ControllaSondaggi(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta4_Click(sender As Object, e As EventArgs) Handles cmdRisposta4.Click, cmdChiudeSondaggio.Click
        Response.Redirect("Principale.aspx")
    End Sub

    ' SONDAGGI

    Private Sub LeggeStatistiche()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Posizione As Integer = 0
            Dim Quanti As Integer = 0
            Dim Giorn As Integer

            If DatiGioco.StatoConcorso = ValoriStatoConcorso.DaControllare Or DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Or DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
                Giorn = DatiGioco.Giornata - 1
            Else
                Giorn = DatiGioco.Giornata
            End If

            Dim Champions As Boolean = False
            Dim ChampionsTurni As Boolean = False
            Dim EuropaLeague As Boolean = False
            Dim InterToto As Boolean = False
            Dim QuoteCUP As Boolean = False
            Dim EuropaLeagueTurni As Boolean = False
            Dim CoppaItalia As Boolean = False
            Dim Derelitti As Boolean = False
            Dim SuddenDeath As Boolean = False
            Dim Turno As String = ""

            Sql = "Select * From DerelittiSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Derelitti = True
            End If
            Rec.Close()

            Sql = "Select * From CoppaItaliaTurniSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                CoppaItalia = True
            End If
            Rec.Close()

            If DatiGioco.Giornata > 5 Then
                QuoteCUP = True
            End If

            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Champions = True
            End If
            Rec.Close()

            Sql = "Select * From ChampionsTurniSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                ChampionsTurni = True
                Champions = False
            End If
            Rec.Close()

            Sql = "Select * From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                EuropaLeague = True
            End If
            Rec.Close()

            Sql = "Select * From EuropaLeagueTurniSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                EuropaLeague = False
                EuropaLeagueTurni = True
            End If
            Rec.Close()

            Sql = "Select * From InterTotoSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                InterToto = True
            End If
            Rec.Close()

            Sql = "Select * From SuddenDeathDett Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                SuddenDeath = True
            End If
            Rec.Close()

            divSuddenDeath.Visible = SuddenDeath
            If SuddenDeath = True Then
                ' Controlla il torneo Sudden Death
                Sql = "Select Top 1 A.Giornata, A.Squadra, SquadraCasa, SquadraFuori , Risultato, Segno, Punti " _
                    & "From SuddenDeathDett A " _
                    & "Left Join Schedine B On A.Anno=B.Anno And A.Giornata = B.Giornata And " _
                    & "(A.Squadra=B.SquadraCasa Or A.Squadra =B.SquadraFuori ) " _
                    & "Left Join SuddenDeathPunti C On A.Anno=C.Anno And A.Giornata=C.Giornata And A.CodGiocatore=C.CodGiocatore " _
                    & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.CodGiocatore=" & Session("CodGiocatore") & " " _
                    & "Order By A.Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    Dim Testo As String

                    Testo = "Giornata " & Rec("Giornata").Value & " - " & Rec("Squadra").value
                    If "" & Rec("SquadraCasa").Value = "" Or "" & Rec("SquadraFuori").Value = "" Or "" & Rec("Risultato").Value = "" Or "" & Rec("Segno").Value = "" Or "" & Rec("Punti").Value = "" Then
                        Testo += "<br />" & Rec("SquadraCasa").Value & "-" & Rec("SquadraFuori").Value
                        Testo += "<br />Partita da giocare"
                        'ulSuddenDeath.Visible = False
                    Else
                        Testo += "<br />" & Rec("SquadraCasa").Value & "-" & Rec("SquadraFuori").Value & " " & Rec("Risultato").Value & "<br />Punti ottenuti: " & Rec("Punti").Value
                    End If
                    lblDescSD.Text = "Sudden Death"
                    Rec.Close()

                    aSuddendeath.Attributes("class") = "quick-button metro green span3"

                    Sql = "Select * From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        Testo = "Uscito alla giornata " & Rec("Giornata").Value

                        aSuddendeath.Attributes("class") = "quick-button metro red span3"
                    End If

                    lblSD.Text = Testo
                Else
                    divSuddenDeath.Visible = False
                End If
                Rec.Close()
            End If

            divDerelitti.Visible = Derelitti
            If Derelitti = True Then
                ' Controlla ultimo risultato Pippettero
                Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteDerelitti A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                    "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                    "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                    "Order By Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblDescDer.Text = "Pippettero"
                    lblDer.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    divDerelitti.Visible = False
                End If

                Rec.Close()
            End If

            divCoppaItalia.Visible = CoppaItalia
            lblTurnoCI.Text = ""
            If CoppaItalia = True Then
                ' Controlla ultimo risultato Coppa Italia
                Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteCoppaItaliaTurni A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                    "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                    "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                    "Order By Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    Dim Casa As String = Rec("Casa").Value.ToString.ToUpper
                    Dim Fuori As String = Rec("Fuori").Value.ToString.ToUpper
                    Dim Risu As String = Rec("RisultatoUfficiale").Value

                    If Casa = "" Then Casa = "RIPOSO" : Risu = ""
                    If Fuori = "" Then Fuori = "RIPOSO" : Risu = ""

                    lblDescCI.Text = "Coppa Italia"
                    lblCI.Text = Casa & "-" & Fuori & " " & Risu
                    Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteCoppaItaliaTurni", Rec("Giornata").Value)
                    lblTurnoCI.Text = Turno
                Else
                    divCoppaItalia.Visible = False
                End If
                Rec.Close()
            End If

            divInterToto.Visible = InterToto
            If InterToto = True Then
                ' Controlla ultimo risultato Intertoto
                Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteIntertoto A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                    "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                    "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                    "Order By Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblDescIT.Text = "Intertoto"
                    lblIT.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    divInterToto.Visible = False
                End If
                Rec.Close()
            End If

            divEuropaLeague.Visible = EuropaLeague
            lblTurnoEL.Text = ""
            If EuropaLeague = True Then
                ' Controlla ultimo risultato EuropaLeague - Girone
                Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteEuropaLeague A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                    "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                    "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                    "Order By Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblDescEL.Text = "Europa League"
                    lblEL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    divEuropaLeague.Visible = False
                End If
                Rec.Close()
            Else
                divEuropaLeague.Visible = EuropaLeagueTurni
                If EuropaLeagueTurni = True Then
                    ' Controlla ultimo risultato EuropaLeagueTurni
                    Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteEuropaLeagueTurni A " &
                        "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                        "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                        "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                        "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                        "Order By Giornata Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        lblDescEL.Text = "Europa League"
                        lblEL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                        Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteEuropaLeagueTurni", Rec("Giornata").Value)
                        lblTurnoEL.Text += Turno
                    Else
                        divEuropaLeague.Visible = False
                    End If
                    Rec.Close()
                End If
            End If

            divChampions.Visible = Champions
            lblTurnoCL.Text = ""
            If Champions = True Then
                ' Controlla ultimo risultato Champions girone A
                Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampionsA A " &
                    "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                    "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                    "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                    "Order By Giornata Desc"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblDescCL.Text = "Champions gir. A"
                    lblCL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    Rec.Close()
                    Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampionsB A " &
                        "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                        "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                        "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                        "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                        "Order By Giornata Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        lblDescCL.Text = "Champions gir. B"
                        lblCL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                    Else
                        divChampions.Visible = False
                    End If
                    Rec.Close()
                End If
            Else
                divChampions.Visible = ChampionsTurni
                If ChampionsTurni = True Then
                    ' Controlla ultimo risultato ChampionsTurni
                    Sql = "Select Top 1 A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampionsTurni A " &
                        "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " &
                        "Left Join Giocatori C On A.Anno=B.Anno And A.GiocFuori=C.CodGiocatore " &
                        "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Vincitore Is Not Null And " &
                        "(GiocCasa = " & Session("CodGiocatore") & " Or GiocFuori = " & Session("CodGiocatore") & ") " &
                        "Order By Giornata Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        lblDescCL.Text = "Champion's"
                        lblCL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                        Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteChampionsTurni", Rec("Giornata").Value)
                        lblTurnoCL.Text += Turno
                    Else
                        divChampions.Visible = False
                    End If
                    Rec.Close()
                End If
            End If

            divQuoteCUP.Visible = QuoteCUP
            If QuoteCUP = True Then
                Dim Progressivo As Integer = -1
                Dim cod1 As Integer = -1
                Dim cod2 As Integer = -1
                Dim Ris1 As Single = -1
                Dim Ris2 As Single = -1
                Dim sRis1 As String = ""
                Dim sRis2 As String = ""
                Dim sq1 As String = ""
                Dim sq2 As String = ""
                Dim Altro As String = ""
                Dim g As Integer = DatiGioco.Giornata - 5

                If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Or DatiGioco.StatoConcorso = ValoriStatoConcorso.DaControllare Then
                    g -= 1
                End If

                If g < QuantePartiteQuoteCUP Then
                    Sql = "Select Progressivo From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        Progressivo = Rec(0).Value
                    End If
                    Rec.Close()

                    Sql = "Select CodGiocatore, CodAvversario From QuoteCUP_Abbinamenti Where Giornata = " & g & " And (CodGiocatore = " & Progressivo & " Or CodAvversario = " & Progressivo & ")"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        cod1 = Rec(0).Value
                        cod2 = Rec(1).Value
                    End If
                    Rec.Close()

                    If cod1 <> Progressivo Then
                        sq2 = Session("Nick")

                        Sql = "Select Giocatore From QuoteCUP_Squadre A " & _
                            "Left Join Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                            "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Progressivo = " & cod1
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            sq1 = "" & Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Else
                        sq1 = Session("Nick")

                        Sql = "Select Giocatore From QuoteCUP_Squadre A " & _
                            "Left Join Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                            "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Progressivo = " & cod2
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            sq2 = "" & Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    End If

                    If Progressivo <> -1 Then
                        If cod1 <> -1 And cod2 <> -2 Then
                            Sql = "Select * From QuoteCUP_Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & g & " And CodGiocatore=" & cod1
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                Ris1 = Rec("PuntiTot").Value
                            End If
                            Rec.Close()

                            Sql = "Select * From QuoteCUP_Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & g & " And CodGiocatore=" & cod2
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                Ris2 = Rec("PuntiTot").Value
                            End If
                            Rec.Close()

                            If Ris1 <> -1 Then
                                sRis1 = Ris1
                            Else
                                sRis1 = ""
                            End If

                            If Ris2 <> -1 Then
                                sRis2 = Ris2
                            Else
                                sRis2 = ""
                            End If

                            If sRis1 <> "" And sRis2 <> "" Then
                                If Ris1 > Ris2 Then
                                    Altro = "1"
                                Else
                                    If Ris2 > Ris1 Then
                                        Altro = "2"
                                    Else
                                        Altro = "X"
                                    End If
                                End If
                            End If

                            If sq1 = "" Then sq1 = "Morto" : Altro = ""
                            If sq2 = "" Then sq2 = "Morto" : Altro = ""

                            lblPartitaQuoteCUP.Text = sq1 & "-" & sq2 & " " & Altro
                            lblQuoteCUP.Text = "Quote CUP"
                            lblGiornataQuoteCUP.Text = "Giornata " & g
                        Else
                            divQuoteCUP.Visible = False
                        End If
                    Else
                        divQuoteCUP.Visible = False
                    End If
                Else
                    Progressivo = -1
                    g = DatiGioco.Giornata

                    Sql = "Select Progressivo From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        Progressivo = Rec(0).Value
                    End If
                    Rec.Close()

                    If Progressivo <> -1 Then
                        cod1 = -1
                        cod2 = -1

                        Sql = "Select GiocCasa, GiocFuori, IsNull(RisCasa,-1) As RisCasa, IsNull(RisFuori,-1) As RisFuori From QuoteCUP_ScontriDiretti " & _
                            "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And (GiocCasa=" & Progressivo & " Or GiocFuori=" & Progressivo & ")"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            If Rec("GiocCasa").Value = Progressivo Then
                                cod1 = Rec("GiocCasa").Value
                                cod2 = Rec("GiocFuori").Value

                                Ris1 = Rec("RisCasa").Value
                                Ris2 = Rec("RisFuori").Value
                            Else
                                cod1 = Rec("GiocFuori").Value
                                cod2 = Rec("GiocCasa").Value

                                Ris1 = Rec("RisFuori").Value
                                Ris2 = Rec("RisCasa").Value
                            End If
                        End If
                        Rec.Close()

                        If cod1 <> -1 And cod2 <> -1 Then
                            Sql = "Select Giocatore From QuoteCUP_Squadre A " & _
                                "Left Join Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore = " & cod1
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                sq1 = Rec("Giocatore").Value
                            End If
                            Rec.Close()

                            Sql = "Select Giocatore From QuoteCUP_Squadre A " & _
                                "Left Join Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore = " & cod2
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                sq2 = Rec("Giocatore").Value
                            End If
                            Rec.Close()

                            If Ris1 <> -1 Then
                                sRis1 = Ris1
                            Else
                                sRis1 = ""
                            End If

                            If Ris2 <> -1 Then
                                sRis2 = Ris2
                            Else
                                sRis2 = ""
                            End If

                            If sRis1 <> "" And sRis2 <> "" Then
                                If Ris1 > Ris2 Then
                                    Altro = "1"
                                Else
                                    If Ris2 > Ris1 Then
                                        Altro = "2"
                                    Else
                                        Altro = "X"
                                    End If
                                End If
                            End If

                            lblPartitaQuoteCUP.Text = sq1 & "-" & sq2 & " " & Altro
                            lblQuoteCUP.Text = "Quote CUP"
                            lblGiornataQuoteCUP.Text = "Giornata " & g
                        Else
                            divQuoteCUP.Visible = False
                        End If
                    Else
                        divQuoteCUP.Visible = False
                    End If
                End If
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub PrendePartiteAggiornate(Db As GestioneDB, ConnSQL As Object, gGiornata As Integer, Speciale As Boolean)
        Dim Giornata As Integer = gGiornata

        If Speciale = True Then
            Giornata += 100
        End If

        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")

        Sql = "Select Count(*) From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata & " And Segno<>'' And Segno is Not Null And Segno<>'S'"
        Rec = Db.LeggeQuery(ConnSQL, Sql)
        If Rec(0).Value Is DBNull.Value = False Then
            lblAggiornate.Text = "Partite aggiornate: " & Rec(0).Value
        Else
            lblAggiornate.Text = "Partite aggiornate: 0"
        End If
        Rec.Close()
    End Sub

    Protected Sub PrendeRisultati(Db As GestioneDB, ConnSQL As Object, gGiornata As Integer, Speciale As Boolean)
        Dim Giornata As Integer = gGiornata

        If Speciale = True Then
            Giornata += 100
        End If

        Dim NomeFileRisultati As String = Server.MapPath(".") & "\Appoggio\Risultati-" & DatiGioco.AnnoAttuale.ToString.Trim & "-" & Giornata.ToString.Trim & ".txt"

        If File.Exists(NomeFileRisultati) = True Then
            Exit Sub
        End If

        Try
            My.Computer.FileSystem.CreateDirectory(Server.MapPath(".") & "\Appoggio")
        Catch ex As Exception

        End Try

        Try
            Dim myWebClient As New WebClient()

            myWebClient.DownloadFile("http://www.repubblica.it/sport/dirette/listadirette/calcio/?refresh_cens", NomeFileRisultati)
        Catch ex As Exception

        End Try

        If File.Exists(NomeFileRisultati) = True Then
            Dim gf As New GestioneFilesDirectory
            Dim Filetto As String = gf.LeggeFileIntero(NomeFileRisultati)
            Dim Inizio As Long = -1
            Dim Fine As Long = -1
            Dim Partita As String = ""
            Dim sPartita As String = ""
            Dim sPartita2 As String = ""
            Dim Ancora As Boolean = True
            Dim Sql As String
            Dim nPartita As Integer = 0
            Dim sq1 As String
            Dim sq2 As String
            Dim ris1 As String
            Dim ris2 As String

            Sql = "Delete From RisultatiSito Where " & _
                "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "Concorso=" & Giornata
            Db.EsegueSql(ConnSQL, Sql)

            Do While Ancora = True
                Ancora = False
                Inizio = Filetto.IndexOf("<table width=")
                Fine = Filetto.IndexOf("</table>")

                If Inizio > -1 And Fine > -1 And Inizio < Fine Then
                    sPartita = Mid(Filetto, Inizio + 1, (Fine + 9) - (Inizio + 1))

                    Filetto = Filetto.Replace(sPartita, "")

                    sPartita = sPartita.Replace("<span>", "")
                    sPartita = sPartita.Replace("	", "")
                    sPartita = sPartita.Replace("</span>", "")

                    Inizio = sPartita.IndexOf(Chr(34) & "ls-team first" & Chr(34) & ">")
                    Fine = sPartita.IndexOf("</tr>")
                    Do While Inizio > -1 And Fine > -1
                        sPartita2 = Mid(sPartita, Inizio, (Fine - Inizio) + 7)

                        sPartita = sPartita.Replace(sPartita2, "")

                        If Mid(sPartita2, 1, 7).Trim = "<a href" Then
                        Else
                            sPartita2 = Mid(sPartita2, 18, sPartita2.Length).Trim
                        End If

                        If sPartita2.IndexOf("href=" & Chr(34) & "http://www.repubblica.it") = -1 Then
                            sPartita2 = sPartita2.Replace("</td>", ";")
                            sPartita2 = sPartita2.Replace("</tr>", "")
                            sPartita2 = sPartita2.Replace("<td class=" & Chr(34) & "ls-score" & Chr(34) & " width=" & Chr(34) & "52" & Chr(34) & ">", ";")
                            sPartita2 = sPartita2.Replace("<td class=" & Chr(34) & "ls-score" & Chr(34) & " width=" & Chr(34) & "52" & Chr(34) & ">", ";")
                            sPartita2 = sPartita2.Replace("<td width=" & Chr(34) & "213" & Chr(34) & " class=" & Chr(34) & "ls-team" & Chr(34) & ">", ";")
                            sPartita2 = sPartita2.Replace("<td width=" & Chr(34) & "55" & Chr(34) & " title=" & Chr(34) & "diretta della partita" & Chr(34) & ">", "")
                        Else
                            sPartita2 = Mid(sPartita2, sPartita2.IndexOf(">") + 2, sPartita2.Length)
                            sPartita2 = sPartita2.Replace("</a>", ";")
                            sPartita2 = sPartita2.Replace("</td>", ";")
                            sPartita2 = sPartita2.Replace("</tr>", "")
                            sPartita2 = sPartita2.Replace("Live", "")
                            sPartita2 = sPartita2.Replace("<td class=" & Chr(34) & "ls-score" & Chr(34) & " width=" & Chr(34) & "52" & Chr(34) & ">", ";")
                            sPartita2 = sPartita2.Replace("<td width=" & Chr(34) & "213" & Chr(34) & " class=" & Chr(34) & "ls-team" & Chr(34) & ">", ";")
                            Inizio = sPartita2.IndexOf("<")
                            Fine = sPartita2.IndexOf(">")
                            sPartita2 = Mid(sPartita2, 1, Inizio) & Mid(sPartita2, Fine + 2, sPartita2.Length)
                            Inizio = sPartita2.IndexOf("<td width=" & Chr(34) & "55" & Chr(34) & " title=" & Chr(34) & "diretta della partita" & Chr(34) & ">")
                            sPartita2 = Mid(sPartita2, 1, Inizio)
                        End If
                        'sPartita2 = sPartita2.Replace("-", ";")
                        sPartita2 += ";"

                        Dim Campi() As String = sPartita2.Split(";")

                        sPartita2 = ""
                        For i As Integer = 0 To Campi.Length - 1
                            sPartita2 += Campi(i).Trim & ";"
                        Next
                        Do While sPartita2.IndexOf(";;") > -1
                            sPartita2 = sPartita2.Replace(";;", ";")
                        Loop
                        Campi = sPartita2.Split(";")

                        If Campi.Length > 3 Then
                            sq1 = Campi(0)
                            If IsNumeric(Campi(1)) = True Then
                                ris1 = Campi(1)
                            Else
                                ris1 = "null"
                            End If
                            If IsNumeric(Campi(2)) = True Then
                                ris2 = Campi(2)
                            Else
                                ris2 = "null"
                            End If
                            sq2 = Campi(3)
                        Else
                            sq1 = Campi(0)
                            sq2 = Campi(1)
                            ris1 = "null"
                            ris2 = "null"
                        End If

                        nPartita += 1
                        Sql = "Insert Into RisultatiSito Values (" & _
                            " " & DatiGioco.AnnoAttuale & ", " & _
                            " " & Giornata & ", " & _
                            " " & nPartita & ", " & _
                            "'" & SistemaTestoPerDB(sq1) & "', " & _
                            "'" & SistemaTestoPerDB(sq2) & "', " & _
                            " " & ris1 & ", " & _
                            " " & ris2 & " " & _
                            ")"
                        Try
                            Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
                        Catch ex As Exception

                        End Try

                        Inizio = sPartita.IndexOf(Chr(34) & "ls-team first" & Chr(34) & ">")
                        Fine = sPartita.IndexOf("</tr>")
                    Loop

                    Ancora = True
                End If
            Loop

            ' gf.EliminaFileFisico(NomeFileRisultati)

            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = Server.CreateObject("ADODB.Recordset")
            Dim Update() As String = {}
            Dim qUpdate As Integer = 0
            Dim Ris As String
            Dim Segno As String = ""

            Sql = "Select * From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Sql = "Select * From RisultatiSito Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Concorso=" & Giornata & " And " & _
                    "Ltrim(Rtrim(Upper(Squadra1)))='" & SistemaTestoPerDB(Rec("SquadraCasa").Value) & "' And " & _
                    "Ltrim(Rtrim(Upper(Squadra2)))='" & SistemaTestoPerDB(Rec("SquadraFuori").Value) & "'"
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2.Eof = False Then
                    If Rec2("Ris1").Value Is DBNull.Value = False And Rec2("Ris2").Value Is DBNull.Value = False Then
                        Ris = Rec2("Ris1").Value.ToString.Trim & "-" & Rec2("Ris2").Value.ToString.Trim

                        Select Case Val(Rec2("Ris1").Value) - Val(Rec2("Ris2").Value)
                            Case Is > 0
                                Segno = "1"
                            Case Is < 0
                                Segno = "2"
                            Case Is = 0
                                Segno = "X"
                        End Select

                        qUpdate += 1
                        ReDim Preserve Update(qUpdate)

                        Update(qUpdate) = "Update Schedine Set " &
                            "Risultato='" & Ris & "' , " &
                            "Segno='" & Segno & "' " &
                            "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata & " And Partita=" & Rec("Partita").Value
                    End If
                End If
                Rec2.Close()

                Rec.MoveNext()
            Loop
            Rec.Close()

            For i As Integer = 1 To qUpdate
                Db.EsegueSql(ConnSQL, Update(i))
            Next

            PrendePartiteAggiornate(Db, ConnSQL, gGiornata, Speciale)
        End If
    End Sub

    Protected Sub imgAggiornaRisultati_Click(sender As Object, e As ImageClickEventArgs) Handles imgAggiornaRisultati.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim NomeFileRisultati As String
            Dim gf As New GestioneFilesDirectory

            If Speciale = False Then
                NomeFileRisultati = Server.MapPath(".") & "\Appoggio\Risultati-" & DatiGioco.AnnoAttuale & "-" & DatiGioco.Giornata.ToString.Trim & ".txt"
                gf.EliminaFileFisico(NomeFileRisultati)
                PrendeRisultati(Db, ConnSQL, DatiGioco.Giornata, Speciale)
            Else
                NomeFileRisultati = Server.MapPath(".") & "\Appoggio\Risultati-" & DatiGioco.AnnoAttuale & "-" & (DatiGioco.GiornataSpeciale + 100).ToString.Trim & ".txt"
                gf.EliminaFileFisico(NomeFileRisultati)
                PrendeRisultati(Db, ConnSQL, DatiGioco.GiornataSpeciale, Speciale)
            End If

            gf = Nothing

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdMischiaAbbinamenti_Click(sender As Object, e As EventArgs) Handles cmdMischiaAbbinamenti.Click
        Dim Db As New GestioneDB
        Dim x As Integer
        Dim y As Integer

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            For i As Integer = 1 To 20
                x = -1
                y = -1
                Do While x = y
                    Randomize()
                    x = Int(Rnd(1) * 21) + 1
                    y = Int(Rnd(1) * 21) + 1
                    If x < 1 Then x = 1
                    If x > 21 Then x = 21
                    If y < 1 Then y = 1
                    If y > 21 Then y = 21
                Loop

                Sql = "Update QuoteCUP_Abbinamenti " & _
                    "Set Giornata=99 " & _
                    "Where Giornata=" & x
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Update QuoteCUP_Abbinamenti " & _
                    "Set Giornata=" & x & " " & _
                    "Where Giornata=" & y
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Update QuoteCUP_Abbinamenti " & _
                    "Set Giornata=" & y & " " & _
                    "Where Giornata=99"
                Db.EsegueSql(ConnSQL, Sql)
            Next

            Dim Vecchio As Integer
            Dim Nuovo As Integer

            For i As Integer = 1 To 12 Step 4
                Sql = "Select * From QuoteCUP_Abbinamenti Where CodGiocatore=" & i & " Or CodAvversario=" & i & " Order By Giornata"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("CodGiocatore").Value = i Then
                    Vecchio = 1
                Else
                    Vecchio = 2
                End If
                Rec.MoveNext()
                Do Until Rec.Eof
                    If Rec("CodGiocatore").Value = i Then
                        Nuovo = 1
                    Else
                        Nuovo = 2
                    End If

                    If Vecchio = Nuovo Then
                        Sql = "Update QuoteCUP_Abbinamenti " & _
                            "Set CodGiocatore=" & Rec("CodAvversario").Value & ", CodAvversario=" & Rec("CodGiocatore").Value & " " & _
                            "Where Giornata=" & Rec("Giornata").Value & " And CodGiocatore=" & Rec("CodGiocatore").Value & " And CodAvversario=" & Rec("CodAvversario").Value
                        Db.EsegueSql(ConnSQL, Sql)
                    End If
                    Vecchio = Nuovo

                    Rec.MoveNext()
                Loop
                Rec.Close()
            Next

            ConnSQL.Close()
        End If

        Db = Nothing

        Dim m() As String = {"Fatto"}
        VisualizzaMessaggioInPopup(m, Master)
    End Sub
End Class