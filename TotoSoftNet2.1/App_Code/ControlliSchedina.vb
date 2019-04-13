Public Class ControlliSchedina
    Private Db As GestioneDB
    Private ConnSQL As Object
    Private Sql As String
    Private Speciale As Boolean
    Private Riga As String
    Private Rec As Object = CreateObject("ADODB.Recordset")
    Private Rec2 As Object = CreateObject("ADODB.Recordset")
    Private gMail As New GestioneMail
    Private immCasa As String
    Private immFuori As String
    Private gg As Integer
    Private g As New Giocatori
    Private sc As New StringheClassifiche
    Private CodGiocatore As Integer
    Private Percorso As String
    Private grdPartite As System.Web.UI.WebControls.GridView
    Private grdClassvinc As System.Web.UI.WebControls.GridView
    Private grdVincitori As System.Web.UI.WebControls.GridView
    Private grdUltimi As System.Web.UI.WebControls.GridView

    Public Sub ImpostaParametriControllo(CodiceGioc As Integer, Perc As String, _
                                         pGrdPartite As System.Web.UI.WebControls.GridView, _
                                         pGrdCV As System.Web.UI.WebControls.GridView, _
                                         pGrdVinc As System.Web.UI.WebControls.GridView, _
                                         pGrdUltimi As System.Web.UI.WebControls.GridView _
                                         )
        Db = New GestioneDB
        gMail = New GestioneMail
        g = New Giocatori
        sc = New StringheClassifiche
        CodGiocatore = CodiceGioc
        Percorso = Perc
        grdPartite = pGrdPartite
        grdClassvinc = pGrdCV
        grdVincitori = pGrdVinc
        grdUltimi = pGrdUltimi

        If Db.LeggeImpostazioniDiBase() = True Then
            ConnSQL = Db.ApreDB()

            Speciale = ControllaConcorsoSpeciale(Db, ConnSQL)
        End If
    End Sub

    Public Function VisualizzaPartiteGiornataERisultati() As String
        Dim TestoMail As String = "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & " style=""vertical-align: top; ?^?^LARGH?^?^""><tr><td style=""vertical-align: top; width: 57%; text-align: center;"">"
        Dim Casa As Label
        Dim Fuori As Label
        Dim Ris As Label
        Dim Segno As Label
        Dim Quanti As Integer

        If Speciale = False Then
            TestoMail += gMail.ApreTestoTitolo() & "Partite giornata " & DatiGioco.Giornata & " " & gMail.ChiudeTesto() & "<br />"
        Else
            TestoMail += gMail.ApreTestoTitolo() & "Partite giornata speciale " & DatiGioco.GiornataSpeciale & " " & gMail.ChiudeTesto() & "<br />"
        End If
        TestoMail += gMail.ApreTabella
        Riga = "Partita;;Casa;;Fuori;Risultato;Segno;"
        TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
        For i As Integer = 0 To grdPartite.Rows.Count - 1
            Casa = DirectCast(grdPartite.Rows(i).FindControl("txtCasa"), Label)
            Fuori = DirectCast(grdPartite.Rows(i).FindControl("txtFuori"), Label)
            Ris = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), Label)
            Segno = DirectCast(grdPartite.Rows(i).FindControl("txtSegno"), Label)
            immCasa = gMail.RitornaImmagineSquadra(Casa.Text)
            immFuori = gMail.RitornaImmagineSquadra(Fuori.Text)

            Riga = grdPartite.Rows(i).Cells(0).Text & ";" & immCasa & ";" & Casa.Text & ";" & immFuori & ";" & Fuori.Text & ";" & Ris.Text & ";" & Segno.Text & ";"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
        Next
        TestoMail += "</table> "

        TestoMail += gMail.ApreTesto() & "<br />Risultati giocatori: " & gMail.ChiudeTesto()
        TestoMail += gMail.ApreTabella
        Riga = "Posizione;;Giocatore;Punti Totali;Punti Risultato;Punti Segno;Punti Jolly;Punti Quote;Punti FR;Pagante;Tappato;"

        TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
        For i As Integer = 0 To grdClassvinc.Rows.Count - 1
            Riga = ""
            For k As Integer = 0 To 9
                If k = 1 Then
                    Riga += gMail.RitornaImmagineGiocatore(grdClassvinc.Rows(i).Cells(2).Text) & ";"
                Else
                    Riga += grdClassvinc.Rows(i).Cells(k).Text & ";"
                End If
            Next
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
        Next
        TestoMail += "</table> "

        If Speciale = False Then
            TestoMail += "<hr />" & gMail.ApreTesto() & "Vincitori:" & gMail.ChiudeTesto
            TestoMail += "<table style=""?^?^LARGH?^?^ text-align: center;""><tr><td>" & gMail.ApreTesto() & "Primi:" & gMail.ChiudeTesto() & "</td><td>" & gMail.ApreTesto() & QuantiPrimi & gMail.ChiudeTesto() & "</td><td>" & gMail.ApreTesto() & " (" & ScriveNumeroFormattato(PremioPrimo) & " a persona)" & gMail.ChiudeTesto() & "</td></tr>"
            TestoMail += "<tr><td>" & gMail.ApreTesto() & "Secondi:" & gMail.ChiudeTesto() & "</td><td>" & gMail.ApreTesto() & QuantiSecondi & gMail.ChiudeTesto() & "</td><td>" & gMail.ApreTesto() & " (" & ScriveNumeroFormattato(PremioSecondo) & " a persona)" & gMail.ChiudeTesto() & "</td></tr></table>"
            Quanti = grdVincitori.Rows.Count - 1
            'Else
            '    TestoMail += "<hr />" & gMail.ApreTesto() & "Vincitore:" & gMail.ChiudeTesto
            '    Quanti = 0
            TestoMail += "<br />" & gMail.ApreTabella
            Riga = "Posizione;;Giocatore;Punti Totali;Punti Risultato;Punti Segno;Punti Jolly;Punti Quote;Punti FR;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
            For i As Integer = 0 To Quanti
                Riga = ""
                If grdVincitori.Rows(i).Cells(0).Text = "" Or grdVincitori.Rows(i).Cells(0).Text = "&nbsp;" Then
                    TestoMail += gMail.ConverteTestoInRigaTabella(";;;;;;;;;")
                Else
                    For k As Integer = 0 To 7
                        If k = 1 Then
                            Riga += gMail.RitornaImmagineGiocatore(grdVincitori.Rows(i).Cells(2).Text) & ";"
                        Else
                            Riga += grdVincitori.Rows(i).Cells(k).Text & ";"
                        End If
                    Next
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
                End If
            Next
            TestoMail += "</table> "
        End If

        If Speciale = False Then
            TestoMail += "<br />" & gMail.ApreTesto() & "<center>Ultimi:</center>" & gMail.ChiudeTesto() & "<br />"
            TestoMail += gMail.ApreTabella
            Riga = "Posizione;;Giocatore;Punti Totali;Punti Risultato;Punti Segno;Punti Jolly;Punti Quote;Punti FR;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
            For i As Integer = 0 To grdUltimi.Rows.Count - 1
                Riga = ""
                For k As Integer = 0 To 7
                    If k = 1 Then
                        Riga += gMail.RitornaImmagineGiocatore(grdUltimi.Rows(i).Cells(2).Text) & ";"
                    Else
                        Riga += grdUltimi.Rows(i).Cells(k).Text & ";"
                    End If
                Next
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
            Next
            TestoMail += "</table> "
        End If

        Return TestoMail
    End Function

    Public Sub PrendeDatiDiDettaglio()
        Dim Giocatore As String
        Dim idGiocatore As Integer
        Dim Progressivo As Integer = 0

        If Speciale = True Then
            gg = DatiGioco.GiornataSpeciale + 100
        Else
            gg = DatiGioco.Giornata
        End If

        If Speciale = False Then
            Dim Cosa As String = "Vittorie"
            Dim Cosa2 As String = "Primi"

            Sql = "Delete From Primi Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & gg
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Delete From Secondi Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & gg
            Db.EsegueSql(ConnSQL, Sql)

            For i As Integer = 0 To grdVincitori.Rows.Count - 1
                If grdVincitori.Rows(i).Cells(2).Text.Trim = "" Or grdVincitori.Rows(i).Cells(2).Text.Trim = "&nbsp;" Then
                    Cosa = "SecondiPosti"
                    Cosa2 = "Secondi"
                    Progressivo = 0
                Else
                    Giocatore = grdVincitori.Rows(i).Cells(2).Text
                    idGiocatore = g.TornaIdGiocatore(Giocatore)

                    Sql = "Update DettaglioGiocatori Set " & Cosa & "=" & Cosa & "+1 Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGiocatore
                    Db.EsegueSql(ConnSQL, Sql)

                    Progressivo += 1
                    Sql = "Insert Into " & PrefissoTabelle & Cosa2 & " Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & gg & ", " & _
                        " " & Progressivo & ", " & _
                        " " & idGiocatore & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)
                End If
            Next

            Cosa = "UltimiPosti"
            Cosa2 = "Ultimi"
            Progressivo = 0
            Sql = "Delete From " & PrefissoTabelle & Cosa2 & " Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & gg
            Db.EsegueSql(ConnSQL, Sql)
            For i As Integer = 0 To grdUltimi.Rows.Count - 1
                Giocatore = grdUltimi.Rows(i).Cells(2).Text
                idGiocatore = g.TornaIdGiocatore(Giocatore)

                Sql = "Update DettaglioGiocatori Set " & Cosa & "=" & Cosa & "+1 Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGiocatore
                Db.EsegueSql(ConnSQL, Sql)

                Progressivo += 1
                Sql = "Insert Into " & PrefissoTabelle & Cosa2 & " Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    " " & gg & ", " & _
                    " " & Progressivo & ", " & _
                    " " & idGiocatore & " " & _
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            Next
        End If
    End Sub

    Public Sub AggiornaMovimentiDiBilancio()
        If Speciale = False Then
            Dim sPremioPrimo As String = PremioPrimo.ToString.Replace(".", ",")
            sPremioPrimo = sPremioPrimo.Replace(",", ".")

            Dim sPremioSecondo As String = PremioSecondo.ToString.Replace(".", ",")
            sPremioSecondo = sPremioSecondo.Replace(",", ".")

            ' Primi
            Sql = "Select * From Primi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                ScriveMovimentoBilancio(Db, ConnSQL, Rec("CodGiocatore").Value, sPremioPrimo, "D", "Vincita")

                Sql = "Update Bilancio Set Vinti=Vinti+" & sPremioPrimo & " " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Db.EsegueSql(ConnSQL, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ' Secondi
            Sql = "Select * From Secondi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                ScriveMovimentoBilancio(Db, ConnSQL, Rec("CodGiocatore").Value, sPremioSecondo, "D", "Vincita")

                Sql = "Update Bilancio Set Vinti=Vinti+" & sPremioSecondo & " " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Db.EsegueSql(ConnSQL, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If
    End Sub

    Public Function RiconoscimentiDisonori() As String
        Dim TestoMail As String = ""

        If Speciale = False Then
            Dim rd As New RiconoscimentiDisonori
            TestoMail = rd.ControlliSuControlloSchedina
            rd = Nothing
        End If

        Return TestoMail
    End Function

    Public Function VisualizzaClassifica() As String
        Dim TestoMail As String = ""

        If Speciale = False Then
            Dim Posiz As Integer = 0
            Dim Posizione As Integer = 0
            Dim TotPunti As Single
            Dim VecchiPunti As Single

            Sql = "Delete From Posizioni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata
            Db.EsegueSql(ConnSQL, Sql)

            TestoMail += "<hr />" & gMail.ApreTestoTitolo() & "Classifica attuale: " & gMail.ChiudeTesto() & "<br />"
            TestoMail += gMail.ApreTabella
            Riga = "P.;;Gioc.;Punti Totali;P. Risultato;P. Segno;P. Jolly;P. Quote;P. FR;Tappi;Vitt.;1;2;RD;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
            Sql = sc.RitornaStringaClassificaGenerale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Posiz += 1
                immCasa = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Riga = Posiz & ";" & immCasa & ";" & Rec("Giocatore").Value & ";" & Rec("TotPunti").Value & ";" & Rec("PRisultati").Value & ";" & Rec("PSegni").Value & ";" & Rec("PJolly").Value & ";" & Rec("PQuote").Value & ";" & Rec("PFR").Value & ";" & Rec("NumeroTappi").Value & ";" & Rec("Vittorie").Value & ";" & Rec("UltimiPosti").Value & ";" & Rec("SecondiPosti").Value & ";" & Rec("RiconDison").Value & ";"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                TotPunti = Rec("TotPunti").Value
                If TotPunti <> VecchiPunti Then
                    Posizione += 1
                    VecchiPunti = TotPunti
                End If

                ' Scrive la posizione del giocatore
                Sql = "Insert Into Posizioni Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    " " & DatiGioco.Giornata & ", " & _
                    " " & Rec("CodGiocatore").Value & ", " & _
                    " " & Posizione & " " & _
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
                ' Scrive la posizione del giocatore

                Rec.MoveNext()
            Loop
            Rec.Close()
            TestoMail += "</table> "
        Else
            Dim Posiz As Integer = 0

            TestoMail += "<hr />" & gMail.ApreTestoTitolo() & "Classifica speciale attuale: " & gMail.ChiudeTesto() & "<br />"
            TestoMail += gMail.ApreTabella
            Riga = "P.;;Gioc.;Punti Totali;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
            Sql = "Select Sum(PuntiTot) As PuntiRisultati, B.CodGiocatore, B.Giocatore From ClassificaSpeciale A " & _
                "Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                "Where A.idAnno = " & DatiGioco.AnnoAttuale & " And B.Cancellato='N' " & _
                "Group By B.CodGiocatore, B.Giocatore " & _
                "Order By 1 Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Posiz += 1
                immCasa = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Riga = Posiz & ";" & immCasa & ";" & Rec("Giocatore").Value & ";" & Rec("PuntiRisultati").Value & ";"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                Rec.MoveNext()
            Loop
            Rec.Close()
            TestoMail += "</table> "
        End If

        Return TestoMail
    End Function

    Public Function VisualizzaClassificaRisultati() As String
        Dim TestoMail As String = ""

        If Speciale = False Then
            Dim Giocate As Integer
            Dim UltimoRisultato As Integer
            Dim Precedente As Integer
            Dim TotPunti As Integer
            Dim vMedia As Single
            Dim StringaIn As String = ""
            Dim VecchiPunti As Single
            Dim Posizione As Integer = 0

            TestoMail += "<hr />" & gMail.ApreTestoTitolo() & "Classifica per risultati: " & gMail.ChiudeTesto() & "<br />"
            TestoMail += gMail.ApreTabella

            Riga = "Posizione;;Giocatore;Punti Risultato;Ultimo Risultato;Precedente;Giocate;Media;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)

            Sql = "Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore From Risultati A " & _
               "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore " & _
               "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & gg & " And B.Cancellato='N' " & _
               "Group By B.CodGiocatore, B.Giocatore " & _
               "Order By PuntiRisultati Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                StringaIn += Rec("CodGiocatore").Value & ", "

                Sql = "Select Count(Giornata) From Pronostici " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value & " " & _
                    "And Partita=1 And Giornata<=" & gg
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2.Eof = True Then
                    Giocate = 0
                Else
                    If Rec2(0) Is DBNull.Value = True Then
                        Giocate = 0
                    Else
                        Giocate = Rec2(0).Value
                    End If
                End If
                Rec2.Close()

                Sql = "Select PuntiRisultati As TotPunti From Risultati  Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & gg & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2.Eof = True Then
                    UltimoRisultato = 0
                Else
                    If Rec2(0) Is DBNull.Value = True Then
                        UltimoRisultato = 0
                    Else
                        UltimoRisultato = Rec2(0).Value
                    End If
                End If
                Rec2.Close()

                Sql = "Select PuntiRisultati As TotPunti From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & gg - 1 & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2.Eof = True Then
                    Precedente = 0
                Else
                    If Rec2(0) Is DBNull.Value = True Then
                        Precedente = 0
                    Else
                        Precedente = Rec2(0).Value
                    End If
                End If
                Rec2.Close()

                TotPunti = Rec("PuntiRisultati").Value
                If Giocate <> 0 Then
                    vMedia = TotPunti / Giocate
                Else
                    vMedia = 0
                End If

                If TotPunti <> VecchiPunti Then
                    Posizione += 1
                    VecchiPunti = TotPunti
                End If

                immCasa = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Riga = Posizione & ";" & immCasa & ";" & Rec("Giocatore").Value & ";" & FormattaNumeroConVirgola(Rec("PuntiRisultati").Value) & _
                    ";" & FormattaNumeroConVirgola(UltimoRisultato) & ";" & FormattaNumeroConVirgola(Precedente) & ";" & _
                    Giocate & ";" & FormattaNumeroConVirgola(vMedia) & ";"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = ""
            If StringaIn.Length > 0 Then
                StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
                Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In (" & StringaIn & ") And Cancellato='N' Order By Giocatore"
            Else
                If StringaIn = "" Then
                    Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
                End If
            End If
            If Sql <> "" Then
                Posizione += 1

                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    immCasa = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                    Riga = Posizione & ";" & immCasa & ";" & 0 & ";" & 0 & _
                        ";" & 0 & ";" & 0 & ";" & _
                        0 & ";" & 0 & ";"
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            TestoMail += "</table> "
        End If

        Return TestoMail
    End Function

    Public Function ControlloSuddenDeath() As String
        Dim TestoMail As String = ""

        If gg >= 4 And gg < 100 And Speciale = False Then
            Dim QuantiPerSD As Integer

            Sql = "Select Count(*) From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In " & _
                "(Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ") And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                QuantiPerSD = Rec(0).Value
            Else
                QuantiPerSD = 0
            End If
            Rec.Close()

            If QuantiPerSD > 1 Then
                'Dim Rec2 As Object = Server.CreateObject("ADODB.Recordset")
                Dim Punti As Integer

                Sql = "Delete From SuddenDeathPunti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Delete From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
                Db.EsegueSql(ConnSQL, Sql)

                TestoMail += "<hr />" & gMail.ApreTestoTitolo() & "Risultati torneo Sudden Death: " & gMail.ChiudeTesto() & "<br />"
                TestoMail += gMail.ApreTabella()
                Riga = ";Giocatore;;Squadra Assegnata;Partita;Risultato;Punti;"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)

                Sql = "Select A.*, B.Giocatore From SuddenDeathDett A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & gg
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Punti = 0
                    Sql = "Select Segno, SquadraCasa, SquadraFuori, Risultato From Schedine Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                        "Giornata=" & gg & " And " & _
                        "(SquadraCasa='" & SistemaTestoPerDB(Rec("Squadra").Value) & "' Or SquadraFuori='" & SistemaTestoPerDB(Rec("Squadra").Value) & "')"
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2("SquadraCasa").Value = Rec("Squadra").Value Then
                        If Rec2("Segno").Value = "1" Then
                            Punti = 3
                        Else
                            If Rec2("Segno").Value = "X" Then
                                Punti = 1
                            End If
                        End If
                    Else
                        If Rec2("Segno").Value = "2" Then
                            Punti = 3
                        Else
                            If Rec2("Segno").Value = "X" Then
                                Punti = 1
                            End If
                        End If
                    End If

                    Sql = "Insert Into SuddenDeathPunti Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & gg & ", " & _
                        " " & Rec("CodGiocatore").Value & ", " & _
                        " " & Punti & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Riga = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value) & ";" & Rec("Giocatore").Value & ";" & gMail.RitornaImmagineSquadra(Rec("Squadra").Value) & ";" & Rec("Squadra").Value & ";" & Rec2("SquadraCasa").Value & "-" & Rec2("SquadraFuori").Value & ";" & Rec2("Risultato").Value & " (" & Rec2("Segno").Value & ");" & Punti & ";"
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                    Rec2.Close()

                    Rec.MoveNext()
                Loop
                Rec.Close()

                TestoMail += "</table> "

                Sql = "Select B.CodGiocatore, B.Giocatore, Sum(Punti) As PuntiTotali From SuddenDeathPunti A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore Not In (Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ") " & _
                    "Group By B.CodGiocatore, B.Giocatore " & _
                    "Order By 3 Desc"

                TestoMail += gMail.ApreTesto() & "<br />Classifica torneo Sudden Death: " & gMail.ChiudeTesto()
                TestoMail += gMail.ApreTabella()
                Riga = ";Giocatore;Punti;"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)

                Dim Puntazzi As Integer = 999
                Dim Ultimo As String = ""
                Dim Rimasti As Integer

                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Riga = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value) & ";" & Rec("Giocatore").Value & ";" & Rec("PuntiTotali").Value & ";"
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga)

                    If Rec("PuntiTotali").Value < Puntazzi Then
                        Puntazzi = Rec("PuntiTotali").Value
                        Ultimo = Rec("CodGiocatore").Value & ";"
                    Else
                        Ultimo += Rec("CodGiocatore").Value & ";"
                    End If
                    Rimasti += 1

                    Rec.MoveNext()
                Loop
                Rec.Close()

                TestoMail += "</table> "

                If gg > 4 And gg < 100 And (gg / 2 = Int(gg / 2)) Then
                    ' Elimino l'ultimo giocatore
                    Dim Ultimi() As String = Ultimo.Split(";")
                    Dim Chi As Integer

                    If Ultimi.Length = 1 Then
                        Chi = Ultimi(0)
                    Else
                        Dim Minimo As Single = 999

                        For i As Integer = 0 To Ultimi.Length - 1
                            If Ultimi(i) <> "" Then
                                Sql = sc.RitornaStringaClassificaGeneralePerGiocatore(Ultimi(i))
                                Rec = Db.LeggeQuery(ConnSQL, Sql)
                                If Rec("TotPunti").Value < Minimo Then
                                    Minimo = Rec("TotPunti").Value
                                    Chi = Ultimi(i)
                                End If

                                Rec.Close()
                            End If
                        Next
                    End If

                    Sql = "Insert Into SuddenDeathEsclusi Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & gg & ", " & _
                        " " & Chi & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Dim GiocatoreEscluso As String = g.TornaNickGiocatore(Chi)

                    TestoMail += gMail.ApreTesto() & "<br />Giocatore escluso: " & gMail.ChiudeTesto()
                    TestoMail += "<br />" & gMail.RitornaImmagineGiocatore(GiocatoreEscluso) & " " & GiocatoreEscluso

                    Rimasti -= 1
                    If Rimasti = 1 Then
                        ' Vincitore Sudden Death
                        Sql = "Select B.CodGiocatore, B.Giocatore, Sum(Punti) As PuntiTotali From SuddenDeathPunti A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                            "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore Not In (Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ") " & _
                            "Group By B.CodGiocatore, B.Giocatore " & _
                            "Order By 3 Desc"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        Dim Vincitore As Integer = Rec("CodGiocatore").Value
                        Rec.Close()

                        Sql = "Insert Into SuddenDeathVinc Values (" & _
                            " " & DatiGioco.AnnoAttuale & ", " & _
                            " " & Vincitore & " " & _
                            ")"
                        Db.EsegueSql(ConnSQL, Sql)
                    End If
                    ' Elimino l'ultimo giocatore
                End If
            End If

        End If

        Return TestoMail
    End Function

    Public Function ControlloScontriDiretti() As String
        Dim TestoMail As String = ""

        If Speciale = False Then
            If DatiGioco.Giornata > 4 Then
                Dim sDiretti As New ScontriDiretti

                TestoMail += sDiretti.ControlloScontriDiretti(Db, ConnSQL)
                TestoMail += sDiretti.CreaTabellaClassificaScontriDiretti(DatiGioco.Giornata, CodGiocatore, True)

                sDiretti = Nothing
            End If
        End If

        Return TestoMail
    End Function

    Public Function GestioneEventi() As String
        Dim TestoMail As String = ""

        If Speciale = False Then
            Dim Ev As New Eventi

            Ev.ImpostaPercorsoApplicazione(Percorso)
            Dim Ritorno As String = Ev.ControllaPresenzaEventi()

            Ev = Nothing

            TestoMail += Ritorno
        End If

        Return TestoMail
    End Function

    Public Sub ChiusuraControlli(sTestoMail As String)
        Dim TestoMail As String = sTestoMail

        DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso
        DatiGioco.ChiusuraConcorso = ""
        DatiGioco.PartitaJolly = -1
        AggiornaDatiDiGioco(Db)

        TestoMail += "</td>§§§§<td style=""vertical-align: top; width: 46%; text-align: center;"">"

        TestoMail += PrendeColonneGiocatori(Db, ConnSQL)

        TestoMail += "**§§**"

        gMail.InviaMailAggiornamentoRisultati(Percorso & "\InvioMail", TestoMail, gg, Speciale)
    End Sub

    Public Function ControlloQuoteCUP() As String
        Dim TestoMail As String = ""
        Dim Punti1 As Integer
        Dim Punti2 As Integer

        If Speciale = False Then
            If DatiGioco.Giornata - 5 < QuantePartiteQuoteCUP And DatiGioco.Giornata >= 5 And DatiGioco.Giornata - 5 > 0 Then
                ' Fase preliminare
                Dim Ris1 As Single
                Dim Ris2 As Single

                TestoMail += "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineQuoteCUP & " " & "Quote CUP Giornata " & DatiGioco.Giornata - 5 & gMail.RitornaImmagineQuoteCUP & gMail.ChiudeTesto() & "<br />"
                TestoMail += gMail.ApreTabella

                Riga = ";Casa;;Fuori;Risultato 1;Risultato 2;;Vincente;"
                TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)

                Sql = "Delete From QuoteCUP_Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & DatiGioco.Giornata - 5
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Select B.Progressivo As pCasa, C.Progressivo As pFuori, D.Giocatore As Casa, E.Giocatore As Fuori, "
                Sql &= "B.CodGiocatore As CodGiocCasa, C.CodGiocatore As CodGiocFuori, "
                Sql &= "F.PuntiQuote * 14 As Ris1, G.PuntiQuote * 14 As Ris2 "
                Sql &= "From QuoteCUP_Abbinamenti A "
                Sql &= "Left Join QuoteCUP_Squadre B On A.CodGiocatore = B.Progressivo And B.Anno=" & DatiGioco.AnnoAttuale & " "
                Sql &= "Left Join QuoteCUP_Squadre C On A.CodAvversario = C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & " "
                Sql &= "Left Join Giocatori D On B.CodGiocatore=D.CodGiocatore And B.Anno=D.Anno "
                Sql &= "Left Join Giocatori E On C.CodGiocatore=E.CodGiocatore And C.Anno=E.Anno "
                Sql &= "Left Join Risultati F On D.CodGiocatore=F.CodGiocatore And A.Giornata=F.Concorso-5 And D.Anno=F.Anno "
                Sql &= "Left Join Risultati G On E.CodGiocatore=G.CodGiocatore And A.Giornata=G.Concorso-5 And E.Anno=G.Anno "
                Sql &= "Where A.Giornata = " & DatiGioco.Giornata - 5 & " And B.Anno = " & DatiGioco.AnnoAttuale
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Riga = ""
                    If Rec("CodGiocCasa").Value = -1 Or Rec("CodGiocFuori").Value = -1 Then
                        ' Gioco col morto
                        If Rec("CodGiocCasa").Value = -1 And Rec("CodGiocFuori").Value = -1 Then
                        Else
                            If Rec("CodGiocCasa").Value = -1 Then
                                ' Morto in casa
                                Ris1 = 0
                                Ris2 = 3
                                Punti1 = 0
                                Punti2 = 3

                                Riga += gMail.RitornaImmagineGiocatore("Morto") & ";Morto;"
                                Riga += gMail.RitornaImmagineGiocatore(Rec("Fuori").Value) & ";" & Rec("Fuori").Value & ";"
                                Riga += Ris1 & ";" & Ris2 & ";"
                            Else
                                ' Morto fuori casa
                                Ris1 = 3
                                Ris2 = 0
                                Punti1 = 3
                                Punti2 = 0

                                Riga += gMail.RitornaImmagineGiocatore(Rec("Casa").Value) & ";" & Rec("Casa").Value & ";"
                                Riga += gMail.RitornaImmagineGiocatore("Morto") & ";Morto;"
                                Riga += Ris1 & ";" & Ris2 & ";"
                            End If
                        End If
                    Else
                        Ris1 = Rec("Ris1").Value
                        Ris2 = Rec("Ris2").Value

                        Riga += gMail.RitornaImmagineGiocatore(Rec("Casa").Value) & ";" & Rec("Casa").Value & ";"
                        Riga += gMail.RitornaImmagineGiocatore(Rec("Fuori").Value) & ";" & Rec("Fuori").Value & ";"
                        Riga += Rec("Ris1").Value & ";" & Rec("Ris2").Value & ";"

                        Dim Diffe As Single = Rec("Ris1").Value - Rec("Ris2").Value

                        Select Case Diffe
                            Case Is > 0.5
                                Punti1 = 3
                                Punti2 = 0
                                Riga += gMail.RitornaImmagineGiocatore(Rec("Casa").Value) & ";" & Rec("Casa").Value & ";"
                            Case Is < -0.5
                                Punti1 = 0
                                Punti2 = 3
                                Riga += gMail.RitornaImmagineGiocatore(Rec("Fuori").Value) & ";" & Rec("Fuori").Value & ";"
                            Case Else
                                Punti1 = 1
                                Punti2 = 1
                                Riga += ";;"
                        End Select
                    End If

                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga, False)

                    Sql = "Insert Into QuoteCUP_Risultati Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & DatiGioco.Giornata - 5 & ", " & _
                        " " & Rec("pCasa").Value & ", " & _
                        " " & Ris1.ToString.Replace(",", ".") & ", " & _
                        " " & Punti1 & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Insert Into QuoteCUP_Risultati Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & DatiGioco.Giornata - 5 & ", " & _
                        " " & Rec("pFuori").Value & ", " & _
                        " " & Ris2.ToString.Replace(",", ".") & ", " & _
                        " " & Punti2 & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Rec.MoveNext()
                Loop
                Rec.Close()

                TestoMail += "</table> "

                Dim Progressivo As Integer = 0

                TestoMail += "<br />" & gMail.ApreTesto() & "Classifiche Quote CUP alla giornata " & DatiGioco.Giornata - 5 & gMail.ChiudeTesto() & "<br />"

                For i As Integer = 0 To 3
                    TestoMail += "<br />" & gMail.ApreTesto() & TitoliGruppiQuoteCUP(i) & "<br />"
                    TestoMail += gMail.ApreTabella
                    Riga = "P.;;Gioc.;Punti Totali;"
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
                    Sql = "Select B.Giocatore, SUM(Punti) As Punti, SUM(PuntiTot) As PuntiTot From QuoteCUP_Risultati A "
                    Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & " "
                    Sql &= "Left Join Giocatori B On A.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
                    Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Concorso<=" & DatiGioco.Giornata - 5 & " And C.Progressivo In (" & NumeriGruppiQuoteCUP(i) & ") "
                    Sql &= "And Giocatore Is Not Null "
                    Sql &= "Group By B.Giocatore "
                    Sql &= "Order By 2 Desc, 3 Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until Rec.Eof
                        Progressivo += 1

                        Riga = Progressivo & ";"
                        Riga += gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value) & ";" & Rec("Giocatore").Value & ";"
                        Riga += Rec("Punti").Value & " (Tot. : " & Rec("PuntiTot").Value & ");"

                        TestoMail += gMail.ConverteTestoInRigaTabella(Riga, False)

                        Rec.MoveNext()
                    Loop
                    TestoMail += "</table> "
                Next

                Rec.Close()

                If DatiGioco.Giornata - 5 = QuantePartiteQuoteCUP - 1 Then
                    Dim CodGioc1(3) As Integer
                    Dim CodGioc2(3) As Integer
                    Dim sCodGioc1(3) As String
                    Dim sCodGioc2(3) As String
                    Dim c As Integer = 0
                    Dim p As Integer = 0

                    ' Terminate le partite preliminari creo gli scontri diretti
                    Sql = "Select Top 4 B.CodGiocatore, B.Giocatore, SUM(Punti) As Punti, SUM(PuntiTot) As PuntiTot From QuoteCUP_Risultati A "
                    Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & " "
                    Sql &= "Left Join Giocatori B On A.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
                    Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Concorso<=" & DatiGioco.Giornata - 5 & " And "
                    Sql &= "C.Progressivo In (" & NumeriGruppiQuoteCUP(0) & "," & NumeriGruppiQuoteCUP(1) & ") "
                    Sql &= "And Giocatore Is Not Null "
                    Sql &= "Group By B.CodGiocatore, B.Giocatore "
                    Sql &= "Order By 3 Desc, 4 Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until Rec.Eof
                        CodGioc1(c) = Rec("CodGiocatore").Value
                        sCodGioc1(c) = Rec("Giocatore").Value
                        c += 1

                        Rec.MoveNext()
                    Loop
                    Rec.Close()

                    c = 0
                    Sql = "Select Top 4 B.CodGiocatore, B.Giocatore, SUM(Punti) As Punti, SUM(PuntiTot) As PuntiTot From QuoteCUP_Risultati A "
                    Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & " "
                    Sql &= "Left Join Giocatori B On A.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
                    Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Concorso<=" & DatiGioco.Giornata - 5 & " And "
                    Sql &= "C.Progressivo In (" & NumeriGruppiQuoteCUP(2) & "," & NumeriGruppiQuoteCUP(3) & ") "
                    Sql &= "And Giocatore Is Not Null "
                    Sql &= "Group By B.CodGiocatore, B.Giocatore "
                    Sql &= "Order By 3 Desc, 4 Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until Rec.Eof
                        CodGioc2(c) = Rec("CodGiocatore").Value
                        sCodGioc2(c) = Rec("Giocatore").Value
                        c += 1

                        Rec.MoveNext()
                    Loop
                    Rec.Close()

                    TestoMail += gMail.ApreTesto() & "<br />Giocatori passati agli scontri diretti: " & gMail.ChiudeTesto()

                    c = DatiGioco.Giornata + 2
                    For i As Integer = 0 To 3
                        TestoMail += "<br />" & gMail.RitornaImmagineGiocatore(sCodGioc1(i)) & " " & sCodGioc1(i)
                        TestoMail += " Vs. " & gMail.RitornaImmagineGiocatore(sCodGioc2(3 - i)) & " " & sCodGioc2(3 - i)

                        p += 1
                        Sql = "Insert Into QuoteCUP_ScontriDiretti Values (" & _
                            " " & DatiGioco.AnnoAttuale & ", " & _
                            " " & c & ", " & _
                            " " & p & ", " & _
                            " " & CodGioc1(i) & ", " & _
                            " " & CodGioc2(3 - i) & ", " & _
                            "null, " & _
                            "null " & _
                            ")"
                        Db.EsegueSql(ConnSQL, Sql)

                        Sql = "Insert Into QuoteCUP_ScontriDiretti Values (" & _
                            " " & DatiGioco.AnnoAttuale & ", " & _
                            " " & c + 1 & ", " & _
                            " " & p & ", " & _
                            " " & CodGioc2(3 - i) & ", " & _
                            " " & CodGioc1(i) & ", " & _
                            "null, " & _
                            "null " & _
                            ")"
                        Db.EsegueSql(ConnSQL, Sql)
                    Next
                    ' Terminate le partite preliminari creo gli scontri diretti
                End If
            Else
                ' Fase scontri diretti
                Dim g As Integer = DatiGioco.Giornata ' - 5
                Dim Ris1 As Single
                Dim Ris2 As Single

                Dim Passa1 As Integer = -1
                Dim Passa2 As Integer = -1
                Dim sPassa1 As String = ""
                Dim sPassa2 As String = ""

                Dim pp As Integer = 0
                Dim RigaPassaggio As String = ""
                Dim Altro As String = ""

                Dim gp As Integer = DatiGioco.Giornata + 3

                Sql = "Delete From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata>=" & gp
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Select Progressivo, B.Giocatore As Casa, C.Giocatore As Fuori, B.CodGiocatore As CodCasa, C.CodGiocatore As CodFuori "
                Sql &= "From QuoteCUP_ScontriDiretti A "
                Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore "
                Sql &= "Left Join Giocatori C On A.Anno=C.Anno And A.GiocFuori=C.CodGiocatore "
                Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata = " & g & " "
                Sql &="Order By Progressivo"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    If g / 2 = Int(g / 2) Then
                        Altro = " di andata"
                    Else
                        Altro = " di ritorno"
                    End If

                    TestoMail += "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineQuoteCUP & " " & "Quote CUP Scontri diretti" & Altro & gMail.RitornaImmagineQuoteCUP & gMail.ChiudeTesto() & "<br />"
                    TestoMail += gMail.ApreTabella

                    If g / 2 = Int(g / 2) Then
                        Riga = ";Casa;;Fuori;Risultato 1;Risultato 2;"
                    Else
                        Riga = ";Casa;;Fuori;Risultato 1;Risultato 2;;Vincente;"
                    End If
                    TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)

                    Do Until Rec.Eof
                        Riga = gMail.RitornaImmagineGiocatore(Rec("Casa").Value) & ";" & Rec("Casa").Value & ";"
                        Riga += gMail.RitornaImmagineGiocatore(Rec("Fuori").Value) & ";" & Rec("Fuori").Value & ";"

                        Sql = "Select PuntiQuote * 14 As Ris " & _
                            "From Risultati " & _
                            "Where Concorso = " & DatiGioco.Giornata & " And Anno = " & DatiGioco.AnnoAttuale & " And CodGiocatore = " & Rec("CodCasa").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = False Then
                            Ris1 = Rec2("Ris").Value
                        Else
                            Ris1 = 0
                        End If
                        Rec2.Close()

                        Sql = "Select PuntiQuote * 14 As Ris " & _
                            "From Risultati " & _
                            "Where Concorso = " & DatiGioco.Giornata & " And Anno = " & DatiGioco.AnnoAttuale & " And CodGiocatore = " & Rec("CodFuori").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = False Then
                            Ris2 = Rec2("Ris").Value
                        Else
                            Ris2 = 0
                        End If
                        Rec2.Close()

                        Riga += Ris1 & ";" & Ris2 & ";"

                        Sql = "Update QuoteCUP_ScontriDiretti Set " & _
                            "RisCasa=" & Ris1.ToString.Replace(",", ".") & ", " & _
                            "RisFuori=" & Ris2.ToString.Replace(",", ".") & " " & _
                            "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And Progressivo=" & Rec("Progressivo").Value
                        Db.EsegueSql(ConnSQL, Sql)

                        If g / 2 <> Int(g / 2) Then
                            ' Passaggio al turno successivo
                            Dim oldRis1 As Single
                            Dim oldRis2 As Single

                            Dim p As Integer = Rec("Progressivo").Value

                            Sql = "Select RisCasa,RisFuori From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g - 1
                            Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                            oldRis1 = Rec2("RisCasa").Value
                            oldRis2 = Rec2("RisFuori").Value
                            Rec2.Close()

                            Dim v As Integer = 0

                            If Ris1 + oldRis2 > Ris2 + oldRis1 Then
                                ' Vince il giocatore attualmente in casa
                                v = 0
                            Else
                                If Ris1 + oldRis2 < Ris2 + oldRis1 Then
                                    ' Vince il giocatore attualmente fuori casa
                                    v = 1
                                Else
                                    If Ris2 > oldRis2 Then
                                        ' Parità del totale, maggiore fuori casa
                                        v = 1
                                    Else
                                        If Ris2 < oldRis2 Then
                                            ' Parità del totale, maggiore in casa
                                            v = 0
                                        Else
                                            ' Random.. Tutto uguale
                                            Randomize()
                                            v = Int(Rnd(0) * 1)
                                            If v < 0 Then v = 0
                                            If v > 1 Then v = 1
                                        End If
                                    End If
                                End If
                            End If

                            Select Case v
                                Case 0
                                    Riga += gMail.RitornaImmagineGiocatore(Rec("Casa").Value) & ";" & Rec("Casa").Value & ";"
                                    If p / 2 <> Int(p / 2) Then
                                        Passa1 = Rec("CodCasa").Value
                                        sPassa1 = Rec("Casa").Value
                                    Else
                                        Passa2 = Rec("CodCasa").Value
                                        sPassa2 = Rec("Casa").Value
                                    End If
                                Case 1
                                    Riga += gMail.RitornaImmagineGiocatore(Rec("Fuori").Value) & ";" & Rec("Fuori").Value & ";"
                                    If p / 2 <> Int(p / 2) Then
                                        Passa1 = Rec("CodFuori").Value
                                        sPassa1 = Rec("Fuori").Value
                                    Else
                                        Passa2 = Rec("CodFuori").Value
                                        sPassa2 = Rec("Fuori").Value
                                    End If
                            End Select

                            ' Scrive il turno successivo
                            If p / 2 = Int(p / 2) Then
                                ' Dim gp As Integer = DatiGioco.Giornata + 3
                                pp += 1

                                'Sql = "Delete From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata>=" & gp
                                'Db.EsegueSql(ConnSQL, Sql)

                                'Sql = "Delete From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata>=" & gp + 1
                                'Db.EsegueSql(ConnSQL, Sql)

                                Sql = "Insert Into QuoteCUP_ScontriDiretti Values ("
                                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                                Sql &= " " & gp & ", "
                                Sql &= " " & pp & ", "
                                Sql &= " " & Passa1 & ", "
                                Sql &= " " & Passa2 & ", "
                                Sql &= "null, "
                                Sql &= "null "
                                Sql &=")"
                                Db.EsegueSql(ConnSQL, Sql)

                                Sql = "Insert Into QuoteCUP_ScontriDiretti Values ("
                                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                                Sql &= " " & gp + 1 & ", "
                                Sql &= " " & pp & ", "
                                Sql &= " " & Passa2 & ", "
                                Sql &= " " & Passa1 & ", "
                                Sql &= "null, "
                                Sql &= "null "
                                Sql &= ")"
                                Db.EsegueSql(ConnSQL, Sql)
                                ' Scrive il turno successivo

                                RigaPassaggio += "<br />" & gMail.RitornaImmagineGiocatore(sPassa1) & " " & sPassa1
                                RigaPassaggio += " Vs. " & gMail.RitornaImmagineGiocatore(sPassa2) & " " & sPassa2

                                Passa1 = -1
                                Passa2 = -1
                            End If
                            ' Passaggio al turno successivo
                        End If

                        TestoMail += gMail.ConverteTestoInRigaTabella(Riga, False)

                        Rec.MoveNext()
                    Loop

                    If pp = 0 And (Passa1 <> -1 Or Passa2 <> -1) Then
                        Dim sPassa As String
                        Dim Passa As Integer

                        If Passa1 <> -1 Then
                            sPassa = sPassa1
                            Passa = Passa1
                        Else
                            sPassa = sPassa2
                            Passa = Passa2
                        End If

                        ' Vincitore Quote CUP
                        RigaPassaggio += gMail.ApreTestoGrande() & "<br />Il giocatore " & gMail.RitornaImmagineGiocatore(sPassa1) & " " & sPassa1 & " si è aggiudicato il trofeo<br />" & gMail.ChiudeTesto()

                        Sql = "Delete From QuoteCUP_Vincenti Where Anno=" & DatiGioco.AnnoAttuale
                        Db.EsegueSql(ConnSQL, Sql)

                        Sql = "Insert Into QuoteCUP_Vincenti Values (" & DatiGioco.AnnoAttuale & ", " & Passa & ")"
                        Db.EsegueSql(ConnSQL, Sql)
                        ' Vincitore Quote CUP
                    End If

                    TestoMail += "</table>"

                    If RigaPassaggio <> "" Then
                        RigaPassaggio = gMail.ApreTesto() & "<br />Giocatori passati al turno successivo: " & gMail.ChiudeTesto() & RigaPassaggio
                        TestoMail += RigaPassaggio
                    End If
                End If

                Rec.Close()
            End If
        End If

        Return TestoMail
    End Function

    Private Function PrendeColonneGiocatori(Db As GestioneDB, ConnSql As Object) As String
        Dim Ritorno As String = ""
        Dim Giocatori() As Integer
        Dim NomeGioc() As String
        Dim qGioc As Integer = 0
        Dim CasaSchedina(14) As String
        Dim FuoriSchedina(14) As String
        Dim RisultatoSchedina(14) As String
        Dim Segno(14) As String
        Dim PronRisultatoSchedina(14) As String
        Dim PronSegno(14) As String
        Dim Riga As String
        Dim PSegni As String
        Dim PRis As String
        Dim PJolly As String
        Dim PFR As String
        Dim PS As String
        Dim PD As String
        Dim PQuote As String
        Dim Totale As Single
        Dim Tappo As String

        If Speciale = True Then
            gg = DatiGioco.GiornataSpeciale + 100
        Else
            gg = DatiGioco.Giornata
        End If

        Sql = "Select Distinct A.CodGiocatore, B.Giocatore From Pronostici A " & _
             "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
             "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & gg & " And B.Cancellato='N' Order By B.Giocatore"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            ReDim Preserve Giocatori(qGioc)
            ReDim Preserve NomeGioc(qGioc)
            Giocatori(qGioc) = Rec("CodGiocatore").Value
            NomeGioc(qGioc) = Rec("Giocatore").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Select * From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " Order By Partita"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            CasaSchedina(Rec("Partita").Value) = "" & Rec("SquadraCasa").Value
            FuoriSchedina(Rec("Partita").Value) = "" & Rec("SquadraFuori").Value
            RisultatoSchedina(Rec("Partita").Value) = "" & Rec("Risultato").Value
            Segno(Rec("Partita").Value) = Rec("Segno").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        For i As Integer = 1 To qGioc
            Sql = "Select * From QuandoTappi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Giocatori(i) & " And Giornata=" & gg
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec.Eof = True Then
                Tappo = ""
            Else
                Tappo = "(Tappato)"
            End If
            Rec.Close()

            Ritorno += gMail.ApreTestoTitolo & gMail.RitornaImmagineGiocatore(NomeGioc(i)) & " Schedina " & NomeGioc(i) & " " & Tappo & " " & gMail.RitornaImmagineGiocatore(NomeGioc(i)) & gMail.ChiudeTesto
            Ritorno += "<hr />"

            Ritorno += gMail.ApreTabella
            Riga = ";Casa;;Fuori;Segno;Ris.;Segno Pron.;Ris. Pron;FR;"
            Ritorno += gMail.ApreTesto
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

            For k As Integer = 1 To 14
                PronRisultatoSchedina(k) = ""
                PronSegno(k) = ""
            Next

            Dim filRouge As Integer = -1

            Sql = "Select * From FilsRouge Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And CodGiocatore=" & Giocatori(i)
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec.Eof = False Then
                filRouge = Rec("FilRouge").Value
            End If
            Rec.Close()

            Sql = "Select * From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And CodGiocatore=" & Giocatori(i) & " Order By Partita"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            Do Until Rec.Eof
                PronRisultatoSchedina(Rec("Partita").Value) = Rec("Risultato").Value
                PronSegno(Rec("Partita").Value) = Rec("Segni").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & gg & " And CodGiocatore=" & Giocatori(i)
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec.Eof = False Then
                PSegni = Rec("PuntiSegni").Value
                PRis = Rec("PuntiRisultati").Value
                PJolly = Rec("PuntiJolly").Value
                PQuote = Rec("PuntiQuote").Value
                PFR = Rec("PuntiFR").Value
                PS = Rec("PuntiSomma").Value
                PD = Rec("PuntiDifferenza").Value
                Totale = Rec("PuntiSegni").Value + Rec("PuntiRisultati").Value + Rec("PuntiJolly").Value + Rec("PuntiQuote").Value + Rec("PuntiFR").Value
            Else
                PSegni = ""
                PRis = ""
                PJolly = ""
                PQuote = ""
                PFR = ""
                PS = ""
                Pd = ""
                Totale = 0
            End If
            Rec.Close()

            For k As Integer = 1 To 14
                Riga = gMail.RitornaImmagineSquadra(CasaSchedina(k)) & ";" & CasaSchedina(k) & ";"
                Riga += gMail.RitornaImmagineSquadra(FuoriSchedina(k)) & ";" & FuoriSchedina(k) & ";"

                Riga += Segno(k) & ";"
                Riga += RisultatoSchedina(k) & ";"

                If Segno(k) = PronSegno(k) Then
                    Riga += gMail.ApreTestoVerde.Replace(";", "§")
                    Riga += PronSegno(k) & ";"
                    Riga += gMail.ChiudeTesto.Replace(";", "§")
                Else
                    If PronSegno(k).IndexOf(Segno(k)) > -1 Then
                        Dim a As Integer = InStr(PronSegno(k), Segno(k))

                        Riga += Mid(PronSegno(k), 1, a - 1)
                        Riga += gMail.ApreTestoVerdeScuro.Replace(";", "§")
                        Riga += Mid(PronSegno(k), a, 1)
                        Riga += gMail.ChiudeTesto.Replace(";", "§")
                        Riga += Mid(PronSegno(k), a + 1, PronSegno(k).Length) & ";"
                    Else
                        Riga += PronSegno(k) & ";"
                    End If
                End If

                If RisultatoSchedina(k) = PronRisultatoSchedina(k) Then
                    Riga += gMail.ApreTestoVerde.Replace(";", "§")
                    Riga += PronRisultatoSchedina(k) & ";"
                    Riga += gMail.ChiudeTesto.Replace(";", "§")
                Else
                    Dim gp1 As Integer
                    Dim fp1 As Integer

                    If RisultatoSchedina(k).IndexOf("-") > -1 Then
                        gp1 = Mid(RisultatoSchedina(k), 1, InStr(RisultatoSchedina(k), "-") - 1)
                        fp1 = Mid(RisultatoSchedina(k), InStr(RisultatoSchedina(k), "-") + 1, 2)
                    Else
                        gp1 = -1
                        fp1 = -1
                    End If

                    Dim gr1 As Integer
                    Dim fr1 As Integer

                    If PronRisultatoSchedina(k).IndexOf("-") > -1 Then
                        gr1 = Mid(PronRisultatoSchedina(k), 1, InStr(PronRisultatoSchedina(k), "-") - 1)
                        fr1 = Mid(PronRisultatoSchedina(k), InStr(PronRisultatoSchedina(k), "-") + 1, 2)
                    Else
                        gr1 = -2
                        fr1 = -2
                    End If

                    If gp1 = gr1 Then
                        Riga += gMail.ApreTestoVerdeScuro.Replace(";", "§")
                        Riga += gr1.ToString.Trim
                        Riga += gMail.ChiudeTesto.Replace(";", "§")
                        Riga += "-" & fr1.ToString.Trim & ";"
                    Else
                        If fp1 = fr1 Then
                            Riga += gr1.ToString.Trim & "-"
                            Riga += gMail.ApreTestoVerdeScuro.Replace(";", "§")
                            Riga += fr1.ToString.Trim & ";"
                            Riga += gMail.ChiudeTesto.Replace(";", "§")
                        Else
                            Riga += PronRisultatoSchedina(k) & ";"
                        End If
                    End If
                End If

                If k = filRouge Then
                    Riga += gMail.RitornaImmagineFilRouge & ";"
                Else
                    Riga += ";"
                End If

                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)
            Next

            Riga = ";;;;;;;;"
            Riga &= ";"

            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            If Speciale = False Then
                Riga = ";Punti Segni;;;;;" & PSegni & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";P. Risultati (*);;;;;" & PRis & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";Punti Jolly;;;;;" & PJolly & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";Punti Quote;;;;;" & PQuote & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";Fil Rouge;;;;;" & PFR & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";* P. Somma;;;;;" & PS & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";* P. Diff.;;;;;" & PD & ";;;;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                Riga = ";Totale;;;;;" & Totale & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)
            Else
                Riga = ";Totale;;;;;" & Totale & ";;"
                Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)
            End If

            Ritorno += gMail.ChiudeTesto
            Ritorno += "</table><hr />"
        Next

        Ritorno = Ritorno.Replace("§", ";")

        Return Ritorno
    End Function

    Public Sub ChiudeTutto()
        gMail = Nothing
        g = Nothing
        sc = Nothing

        ConnSQL.Close()

        Db = Nothing
    End Sub
End Class
