Public Class Eventi
    Private Percorso As String

    Public Sub ImpostaPercorsoApplicazione(Dove As String)
        Percorso = Dove
    End Sub

    Public Function ControllaPresenzaEventi(Optional ModalitaSoloVisualizzazione As Boolean = False, Optional Giornata As Integer = -1) As String
        Dim context As HttpContext = HttpContext.Current
        Dim Ritorno As String = ""
        Dim Db As New GestioneDB
        Dim Sql As String
        Dim nG As Integer
        Dim gm As New GestioneMail
        Dim CreaSuperCoppe As Boolean = False
        Dim CreaGironiDiChampions As Boolean = False
        Dim QuantiPerSD As Integer
        Dim GiornataQuoteCUP As Integer

        If ModalitaSoloVisualizzazione = True Then
            If GiornataEventoCreazione = -1 Then
                If Db.LeggeImpostazioniDiBase() = True Then
                    Dim ConnSQL As Object = Db.ApreDB()
                    Dim Rec As Object = CreateObject("ADODB.Recordset")
                    Sql = "Select * From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " And Descrizione='CREAZIONE SCONTRI DIRETTI'"

                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        GiornataEventoCreazione = Rec("Giornata").Value
                    End If
                    Rec.Close()

                    ConnSQL.close()
                End If
            End If

            If Giornata <> -1 Then
                nG = Giornata
            Else
                nG = DatiGioco.Giornata
            End If
            GiornataQuoteCUP = DatiGioco.Giornata - 5

            Dim sd As New ScontriDiretti
            Dim MaxPartite As Integer = sd.TornaMaxGiornateScontriDiretti
            sd = Nothing

            Ritorno += " Giornata " & nG & " - "
            If nG < MaxPartite And nG > GiornataEventoCreazione Then
                Ritorno += "Giorn. " & nG - GiornataEventoCreazione & " Camp. - "
            End If

            If DatiGioco.Giornata > 3 Then
                If Db.LeggeImpostazioniDiBase() = True Then
                    Dim ConnSQL As Object = Db.ApreDB()
                    Dim Rec As Object = CreateObject("ADODB.Recordset")
                    Sql = "Select Count(*) From Giocatori " _
                        & "Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' And " _
                        & "CodGiocatore Not In (Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ")"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec(0).Value Is DBNull.Value = False Then
                        QuantiPerSD = Rec(0).Value
                    Else
                        QuantiPerSD = 0
                    End If
                    Rec.Close()

                    If QuantiPerSD > 1 Then
                        Ritorno += " Torneo Sudden Death "
                        If DatiGioco.Giornata / 2 = Int(DatiGioco.Giornata / 2) And DatiGioco.Giornata > 4 Then
                            Ritorno += " con eliminazione"
                        End If
                        Ritorno += " - "
                    End If

                    If GiornataQuoteCUP >= 0 And GiornataQuoteCUP < QuantePartiteQuoteCUP Then
                        Sql = "Select * From QuoteCUP_Abbinamenti Where Giornata=" & GiornataQuoteCUP
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            Ritorno += "Giornata " & GiornataQuoteCUP & " QuoteCUP - "
                        End If
                        Rec.Close()

                        'Dim Progressivo As Integer
                        'Dim cod1 As Integer
                        'Dim cod2 As Integer

                        'Sql = "Select Progressivo From TSN2_QuoteCUP_Squadre Where Anno=1 And CodGiocatore=" & context.Session("CodGiocatore")
                        'Rec = Db.LeggeQuery(ConnSQL, Sql)
                        'If Rec.Eof = False Then
                        '    Progressivo = Rec(0).Value
                        'End If
                        'Rec.Close()

                        'Sql = "Select CodGiocatore, CodAvversario From TSN2_QuoteCUP_Abbinamenti Where Giornata = " & GiornataQuoteCUP & " And (CodGiocatore = " & Progressivo & " Or CodAvversario = " & Progressivo & ")"
                        'Rec = Db.LeggeQuery(ConnSQL, Sql)
                        'If Rec.Eof = False Then
                        '    cod1 = Rec(0).Value
                        '    cod2 = Rec(1).Value
                        'End If
                        'Rec.Close()

                        'If cod1 <> Progressivo Then
                        '    Sql = "Select Giocatore From TSN2_QuoteCUP_Squadre A " & _
                        '        "Left Join TSN2_Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                        '        "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Progressivo = " & cod1
                        '    Rec = Db.LeggeQuery(ConnSQL, Sql)
                        '    If Rec.Eof = False Then
                        '        Ritorno += "Giornata " & GiornataQuoteCUP & " QuoteCUP fuori casa contro " & Rec("Giocatore").Value & " - "
                        '    End If
                        '    Rec.Close()
                        'Else
                        '    Sql = "Select Giocatore From TSN2_QuoteCUP_Squadre A " & _
                        '        "Left Join TSN2_Giocatori B On A.Anno = B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                        '        "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Progressivo = " & cod2
                        '    Rec = Db.LeggeQuery(ConnSQL, Sql)
                        '    If Rec.Eof = False Then
                        'Ritorno += "Giornata " & GiornataQuoteCUP & " QuoteCUP in casa contro " & Rec("Giocatore").Value & " - "
                        'End If
                        'Rec.Close()
                        'End If
                    Else
                        If GiornataQuoteCUP > QuantePartiteQuoteCUP Then
                            Dim g As Integer = DatiGioco.Giornata

                            If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
                                g += 1
                            End If

                            Sql = "Select * From QuoteCUP_ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                If g / 2 = Int(g / 2) Then
                                    Ritorno += "Scontri diretti di andata QuoteCUP - "
                                Else
                                    Ritorno += "Scontri diretti di ritorno QuoteCUP - "
                                End If
                            End If
                            Rec.Close()
                        End If
                    End If

                    ConnSQL.Close()
                End If
            End If
        Else
            nG = DatiGioco.Giornata
        End If

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")

            Sql = "Select * From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Select Case Rec("Descrizione").Value
                    Case "CREAZIONE SCONTRI DIRETTI"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione scontri diretti - "
                        Else
                            Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineScontriDiretti & " Creati scontri diretti " & gm.RitornaImmagineScontriDiretti & gm.ChiudeTesto() & "<br />"
                            Ritorno += CreaScontriDiretti(Db, ConnSQL)
                        End If
                    Case "CREAZIONE TORNEO PIPPETTERO"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione Torneo Pippettero - "
                        Else
                            Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmaginePippettero & " Creato Torneo PIPPETTERO" & gm.RitornaImmaginePippettero & gm.ChiudeTesto() & "<br /><br />"
                            Ritorno += CreaTorneoPippettero(Db, ConnSQL)
                        End If
                    Case "CREAZIONE COPPA ITALIA"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione coppa Italia - "
                        Else
                            Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineCItalia & " Creata Coppa Italia" & gm.ChiudeTesto() & gm.RitornaImmagineCItalia & "<br />"
                            Ritorno += CreaTurniCoppaItalia(Db, ConnSQL)
                        End If
                    Case "ACQUISIZIONE SQUADRE COPPE"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione coppe europee - "
                        Else
                            Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineCoppe & " Qualificazione coppe europee" & gm.RitornaImmagineCoppe & gm.ChiudeTesto() & "<br /><br />"
                            Ritorno += CreaCoppeEuropee(Db, ConnSQL)
                        End If
                    Case "CREAZIONE GIRONI CHAMPIONS"
                        CreaGironiDiChampions = True
                    Case "CREAZIONE SUPERCOPPE"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione Supercoppe - "
                        Else
                            CreaSuperCoppe = True
                        End If
                    Case "CREAZIONE QUOTECUP"
                        If ModalitaSoloVisualizzazione = True Then
                            Ritorno += "Creazione Torneo Quote CUP - "
                        Else
                            Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineQuoteCUP & " Creato Torneo Quote CUP" & gm.RitornaImmagineQuoteCUP & gm.ChiudeTesto() & "<br /><br />"
                            Ritorno += CreaTorneoQuoteCUP(Db, ConnSQL)
                        End If
                End Select

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiDerelitti Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro torneo Pippettero - "
                Else
                    Ritorno += GiocaPippettero(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiCoppaItaliaTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro di Coppa Italia - "
                Else
                    Ritorno += GiocaCoppaItalia(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiInterToto Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro di Intertoto - "
                Else
                    Ritorno += GiocaInterToto(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiEuropaLeague Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro di Europa League - "
                Else
                    Ritorno += GiocaEuropaLeague(Db, ConnSQL)
                End If


                Rec.MoveNext()
            Loop
            Rec.Close()

            If CreaGironiDiChampions = True Then
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Creazione gironi Champion's League - "
                Else
                    Ritorno += "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineChampions & " Creazione gironi Champion's League" & gm.RitornaImmagineChampions & gm.ChiudeTesto() & "<br /><br />"
                    Ritorno += CreaGironiChampions(Db, ConnSQL)
                End If
            End If

            Sql = "Select * From EventiEuropaLeagueTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Turno di Europa League - "
                Else
                    Ritorno += GiocaEuropaLeagueTurni(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim GiocataChampions As Boolean = False
            CompletataChampionsA = False
            CompletataChampionsB = False

            Sql = "Select * From EventiChampionsA Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro Champion's League Girone A - "
                Else
                    Ritorno += GiocaChampions(Db, ConnSQL, "A")
                    GiocataChampions = True
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiChampionsB Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro Champion's League Girone B - "
                Else
                    Ritorno += GiocaChampions(Db, ConnSQL, "B")
                    GiocataChampions = True
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            If CompletataChampionsA = True And CompletataChampionsB = True Then
                Ritorno += CreaTurniChampions(Db, ConnSQL)
                GiocataChampions = False
            End If

            If GiocataChampions = True Then
                DatiGioco.GiornataChampionsLeague += 1
                AggiornaDatiDiGioco(Db)
            End If

            Sql = "Select * From EventiChampionsTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Turno Champion's League - "
                Else
                    Ritorno += GiocaChampionsTurni(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiSuperCoppaItalianaTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro Supercoppa Italiana - "
                Else
                    Ritorno += GiocaSuperCoppaItaliana(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From EventiSuperCoppaEuropeaturni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & nG & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If ModalitaSoloVisualizzazione = True Then
                    Ritorno += "Incontro Supercoppa Europea - "
                Else
                    Ritorno += GiocaSuperCoppaEuropea(Db, ConnSQL)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            If CreaSuperCoppe = True Then
                Dim sc As New Supercoppe

                Ritorno += sc.PrendeSquadreSuperCoppaEuropea()
                Ritorno += sc.PrendeSquadreSuperCoppaItaliana()

                sc = Nothing
            End If

            ConnSQL.Close()
        End If
        If ModalitaSoloVisualizzazione = True Then
            If Ritorno <> "" Then
                Ritorno = Mid(Ritorno, 1, Ritorno.Length - 3)
            End If
        End If

        Db = Nothing
        gm = Nothing

        Return Ritorno
    End Function

    Private Function CreaTorneoQuoteCUP(Db As GestioneDB, ConnSql As Object) As String
        Dim gm As New GestioneMail
        Dim Ritorno = gm.ApreTesto() & "Giocatori qualificati:" & gm.ChiudeTesto() & "<br />"
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim codGioc(12) As Integer
        Dim nomeGioc(12) As String
        Dim qGioc As Integer = 0

        Sql = "Delete From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.Anno, A.idGioc, Sum(Totale) As PuntiTot, Sum(Segni) As PuntiSegni, "
        Sql &= "Sum(Risultati) As PuntiRis, Sum(Jolly) As PuntiJolly, Sum(Quote) As PuntiQuote, B.Giocatore, B.CodGiocatore "
        Sql &= "From AppoClassGiornata A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.idGioc=B.CodGiocatore "
        Sql &= "Where B.Cancellato='N' And A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' "
        Sql &= "Group By A.Anno, A.idGioc, B.Giocatore, B.CodGiocatore "
        Sql &= "Order By 3 Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            If qGioc >= 13 Then
                Exit Do
            End If

            codGioc(qGioc) = Rec("CodGiocatore").Value
            nomeGioc(qGioc) = Rec("Giocatore").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        If qGioc < 12 Then
            ' Mancano giocatori... Ne Appizzo tot finti
            For i As Integer = qGioc + 1 To 12
                codGioc(i) = -1
                nomeGioc(i) = "Morto"
            Next
        End If

        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 1, codGioc(1), nomeGioc(1))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 2, codGioc(8), nomeGioc(8))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 3, codGioc(12), nomeGioc(12))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 4, codGioc(4), nomeGioc(4))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 5, codGioc(5), nomeGioc(5))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 6, codGioc(9), nomeGioc(9))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 7, codGioc(2), nomeGioc(2))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 8, codGioc(7), nomeGioc(7))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 9, codGioc(11), nomeGioc(11))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 10, codGioc(3), nomeGioc(3))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 11, codGioc(6), nomeGioc(6))
        Ritorno += AggiungeGiocatoreSuQuoteCUP(Db, ConnSql, gm, 12, codGioc(10), nomeGioc(10))

        gm = Nothing

        Return Ritorno
    End Function

    Private Function AggiungeGiocatoreSuQuoteCUP(Db As GestioneDB, ConnSQL As Object, gm As GestioneMail, Progressivo As Integer, NumeroGioc As Integer, NomeGioc As String) As String
        Dim imm As String
        Dim Sql As String

        imm = gm.RitornaImmagineGiocatore(NomeGioc)
        Dim Ritorno As String = gm.ApreTesto() & imm & " " & NomeGioc & gm.ChiudeTesto() & "<br />"

        Sql = "Insert Into QuoteCUP_Squadre Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & Progressivo & ", "
        Sql &= " " & NumeroGioc & " "
        Sql &= ")"
        Db.EsegueSql(ConnSQL, Sql)

        Return Ritorno
    End Function

    Private Function CreaTorneoPippettero(Db As GestioneDB, ConnSql As Object) As String
        Dim gm As New GestioneMail
        Dim Ritorno = gm.ApreTesto() & "Giocatori qualificati:" & gm.ChiudeTesto() & "<br />"
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim idGioc() As Integer = {}
        Dim qGioc As Integer = 0
        Dim imm As String

        Dim g As New Giocatori
        Dim num As Integer = g.PrendeNumeroGiocatori
        Dim nPerc As Perc = g.PrendePercentualiCoppe(num)
        g = Nothing

        Sql = "Select A.Anno, A.idGioc, Sum(Totale) As PuntiTot, Sum(Segni) As PuntiSegni "
        Sql &= ", Sum(Risultati) As PuntiRis, Sum(Jolly) As PuntiJolly, Sum(Quote) As PuntiQuote, B.Giocatore "
        Sql &= "From AppoClassGiornata A Left Join Giocatori B On A.Anno=B.Anno And A.idGioc=B.CodGiocatore "
        Sql &= "Where B.Cancellato='N' And A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' "
        Sql &= "Group By A.Anno, A.idGioc, B.Giocatore "
        Sql &= "Order By 3"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = Rec("idGioc").Value

            imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)
            Ritorno += gm.ApreTesto() & imm & " " & Rec("Giocatore").Value & gm.ChiudeTesto() & "<br />"

            If qGioc = nPerc.QuantiDerelitti Then
                Exit Do
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        If qGioc < nPerc.QuantiDerelitti Then
            For i As Integer = qGioc + 1 To nPerc.QuantiDerelitti
                qGioc += 1
                ReDim Preserve idGioc(qGioc)
                idGioc(qGioc) = 99
            Next
        End If

        Dim a As Single = qGioc / 2

        If a <> Int(a) Then
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = 99
        End If

        ' Mischia i giocatori
        Dim Appo As Integer
        Dim X As Integer
        Dim Y As Integer

        For i As Integer = 1 To 15
            Randomize()
            X = Int(Rnd(1) * qGioc) + 1
            Y = Int(Rnd(1) * qGioc) + 1
            If X < 1 Then X = 1
            If X > qGioc Then X = qGioc
            If Y < 1 Then Y = 1
            If Y > qGioc Then Y = qGioc

            Appo = idGioc(X)
            idGioc(X) = idGioc(Y)
            idGioc(Y) = Appo
        Next
        ' Mischia i giocatori

        Dim TotPartiteAndata As Integer = qGioc
        Dim NomeFile As String = Percorso & "\Scontri\" & qGioc.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0
        Dim Cosa As String = "Derelitti"

        gFile.ApreFilePerLettura(NomeFile)

        Sql = "Delete From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        For i As Integer = 1 To qGioc ' - 1
            Sql = "Insert Into " & PrefissoTabelle & Cosa & "Squadre" & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & i & ", "
            Sql &= " " & idGioc(i) & ", "
            Sql &= "'DERELITTI' "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        Sql = "Delete From Partite" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Dim Numerello As Integer = 0

        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                Numerello += 1

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGioc(Casa)
                    Fuori = idGioc(Fuori)

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &= ")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If
        Loop Until sLine Is Nothing

        Sql = "Delete From Eventi" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        DatiGioco.GiornataDerelitti = 0

        For i As Integer = 1 To (qGioc - 1)
            Sql = "Insert Into Eventi" & Cosa & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & DatiGioco.Giornata + i & ", "
            Sql &= "1 "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        AggiornaDatiDiGioco(Db)

        gFile.ChiudeFile()
        gm = Nothing

        Return Ritorno
    End Function

    Private Function CreaTurniCoppaItalia(Db As GestioneDB, ConnSql As Object) As String
        Dim gm As New GestioneMail
        Dim Ritorno = gm.ApreTesto() & "Giocatori partecipanti:" & gm.ChiudeTesto() & "<br />"
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim idGioc() As Integer = {}
        Dim qGioc As Integer = 0
        Dim Cosa As String = "CoppaItaliaTurni"
        Dim imm As String

        Sql = "Delete From Partite" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "SELECT TOP (100) PERCENT  A_1.Anno, A_1.CodGiocatore, A_1.Giocatore, A_1.Testo, A_1.TotPunti, A_1.PSegni, A_1.PRisultati, A_1.PJolly, B.NumeroTappi, B.Vittorie, B.UltimiPosti, B.SecondiPosti "
        Sql &= "FROM (SELECT A.Anno, A.CodGiocatore, A.Giocatore, A.Testo, SUM(B.PuntiSegni) + SUM(B.PuntiRisultati) + SUM(B.PuntiJolly) AS TotPunti, SUM(B.PuntiSegni) AS PSegni, "
        Sql &= " SUM(B.PuntiRisultati) AS PRisultati, SUM(B.PuntiJolly) AS PJolly "
        Sql &= " FROM Giocatori AS A LEFT OUTER JOIN "
        Sql &= " Risultati AS B ON A.Anno = B.Anno AND A.CodGiocatore = B.CodGiocatore "
        Sql &= " WHERE(B.Anno=" & DatiGioco.AnnoAttuale & " And B.Concorso <= " & DatiGioco.Giornata & " And A.Cancellato='N') "
        Sql &= " GROUP BY A.Anno, A.Giocatore, A.CodGiocatore, A.Testo) AS A_1 LEFT OUTER JOIN "
        Sql &= " DettaglioGiocatori AS B ON A_1.Anno=B.Anno And A_1.CodGiocatore = B.CodGiocatore "
        Sql &= "ORDER BY TotPunti DESC, PSegni DESC, PRisultati DESC, NumeroTappi, Vittorie DESC, "
        Sql &="UltimiPosti, SecondiPosti DESC,PJolly DESC"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = Rec("CodGiocatore").Value
            imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)
            Ritorno += gm.ApreTesto() & imm & " " & Rec("Giocatore").Value & gm.ChiudeTesto() & "<br />"

            Rec.MoveNext()
        Loop
        Rec.Close()

        Dim SediciTrentaDue As Integer

        If qGioc > 16 Then
            For i As Integer = qGioc + 1 To 32
                qGioc += 1
                ReDim Preserve idGioc(qGioc)
                idGioc(qGioc) = "99"
            Next
            SediciTrentaDue = 0
        Else
            For i As Integer = qGioc + 1 To 16
                qGioc += 1
                ReDim Preserve idGioc(qGioc)
                idGioc(qGioc) = "99"
            Next
            SediciTrentaDue = 2
        End If

        Dim TotPartiteAndata As Integer = qGioc
        Dim NomeFile As String = Percorso & "\Scontri\Preimpostati\" & qGioc.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0

        gFile.ApreFilePerLettura(NomeFile)

        Sql = "Delete From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        For i As Integer = 1 To qGioc ' - 1
            Sql = "Insert Into " & PrefissoTabelle & Cosa & "Squadre" & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & i & ", "
            Sql &= " " & idGioc(i) & ", "
            Sql &= "'COPPA ITALIA' "
            Sql &=    ")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        Dim Numerello As Integer = 0

        Numerello += (1 + SediciTrentaDue)

        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGioc(Casa)
                    Fuori = idGioc(Fuori)

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=    ")"
                    Db.EsegueSql(ConnSql, Sql)

                    Giornate += 1

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello + 1 & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &= ")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If

            Exit Do

        Loop Until sLine Is Nothing

        Sql = "Delete From Eventi" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        DatiGioco.GiornataCoppaItalia = SediciTrentaDue

        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 1 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 2 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)

        AggiornaDatiDiGioco(Db)

        gFile.ChiudeFile()
        gm = Nothing

        Return Ritorno
    End Function

    Private Function GiocaChampionsTurni(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataChampionsLeague + 1
        Dim Turno As String = RitornaTurnoCoppe(Db, ConnSql, "PartiteChampionsTurni", Giornata)
        If Giornata / 2 = Int(Giornata / 2) Then
            Turno += " di ritorno"
        Else
            Turno += " di andata"
        End If
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineChampions & "Giornata Champion's League. " & Turno & gMail.RitornaImmagineChampions & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampionsTurni A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteChampionsTurni", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        If Giornata / 2 = Int(Giornata / 2) Then
            Ritorno += GestionePartiteScontriDaCalendario(Db, ConnSql, Giornata, "PartiteChampionsTurni", "ChampionsTurni", 1)
        End If

        DatiGioco.GiornataChampionsLeague = Giornata
        AggiornaDatiDiGioco(Db)

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function GiocaChampions(Db As GestioneDB, ConnSql As Object, Girone As String) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataChampionsLeague + 1
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineChampions & "Giornata Champions " & Giornata & " - Girone " & Girone & gMail.RitornaImmagineChampions & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteChampions" & Girone & " A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteChampions" & Girone, Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        Dim TotGiornate As Integer

        Sql = "Select Max(Giornata) From PartiteChampions" & Girone & " Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        TotGiornate = Rec(0).Value
        Rec.Close()

        Ritorno += CreaClassificaChampions(Db, ConnSql, Girone)

        If Giornata = TotGiornate Then
            If Girone = "A" Then
                CompletataChampionsA = True
            Else
                CompletataChampionsB = True
            End If
        End If

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function CreaTurniChampions(Db As GestioneDB, ConnSql As Object) As String
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim idGioc() As Integer = {}
        Dim qGioc As Integer = 0
        Dim Cosa As String = "ChampionsTurni"

        Sql = "Select Top 2 A.* From AppoChampionsA A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' "
        Sql &= "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = Rec("CodGiocatore").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Select Top 2 A.* From AppoChampionsB A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' "
        Sql &= "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = Rec("CodGiocatore").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Delete From Partite" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        If qGioc / 2 <> Int(qGioc / 2) Then
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = "99"
        End If

        ' Sposta squadre per accoppiamenti
        Dim Appo As Integer

        Appo = idGioc(4)
        idGioc(4) = idGioc(2)
        idGioc(2) = Appo
        ' Sposta squadre per accoppiamenti

        Dim TotPartiteAndata As Integer = qGioc
        Dim NomeFile As String = Percorso & "\Scontri\" & qGioc.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0

        gFile.ApreFilePerLettura(NomeFile)

        Sql = "Delete From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        For i As Integer = 1 To qGioc ' - 1
            Sql = "Insert Into " & PrefissoTabelle & Cosa & "Squadre" & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & i & ", "
            Sql &= " " & idGioc(i) & ", "
            Sql &= "'CHAMPIONS' "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        Dim Numerello As Integer = DatiGioco.GiornataChampionsLeague + 1

        If Numerello / 2 = Int(Numerello / 2) Then
            Numerello += 1
        End If

        Dim gMail As New GestioneMail
        Dim g As New Giocatori
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineChampions & "Scontri per il turno successivo" & gMail.RitornaImmagineChampions & gMail.ChiudeTesto() & "<br />"
        Dim Riga As String
        Dim Imm As String
        Dim NomeGiocatore As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGioc(Casa)
                    Fuori = idGioc(Fuori)

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    NomeGiocatore = g.TornaNickGiocatore(Casa)
                    Imm = gMail.RitornaImmagineGiocatore(NomeGiocatore)
                    Riga = Imm & ";" & NomeGiocatore & ";"
                    NomeGiocatore = g.TornaNickGiocatore(Fuori)
                    Imm = gMail.RitornaImmagineGiocatore(NomeGiocatore)
                    Riga += Imm & ";" & NomeGiocatore & ";"

                    Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                    Giornate += 1

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello + 1 & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If

            Exit Do

        Loop Until sLine Is Nothing

        Ritorno += "</table>"

        Sql = "Delete From Eventi" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 1 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 2 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)

        DatiGioco.GiornataChampionsLeague = Numerello - 1

        AggiornaDatiDiGioco(Db)

        gFile.ChiudeFile()

        Return Ritorno
    End Function

    Private Function CreaClassificaChampions(Db As GestioneDB, ConnSql As Object, Girone As String) As String
        Dim sGiocate As Integer
        Dim sMedia As Single
        Dim GoalFatti As Integer
        Dim GoalSubiti As Integer
        Dim DifferenzaGoal As Integer
        Dim Testo As String = ""

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim StringaIn As String = ""

        Sql = "Delete From AppoChampions" & Girone & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.*, B.Giocatore, B.Testo, B.CodGiocatore From vwClassificaChampions" & Girone & " A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.Gioc=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' Order By Punti Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            StringaIn += Rec("CodGiocatore").Value & ", "

            Sql = "Select Count(*) As Quante From PartiteChampions" & Girone & " "
            Sql &= "Where Anno = " & DatiGioco.AnnoAttuale & " "
            Sql &= "And (GiocCasa = " & Rec("CodGiocatore").Value & " Or GiocFuori = " & Rec("CodGiocatore").Value & ") "
            Sql &="And Vincitore Is Not Null"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                sGiocate = Rec2(0).Value
            Else
                sGiocate = 0
            End If
            Rec2.Close()

            If sGiocate <> 0 Then
                sMedia = Rec("Punti").Value / sGiocate
            Else
                sMedia = 0
            End If

            Sql = "Select Anno,  Sum(Fatti) As Fatti , Sum(Subiti) As Subiti From ("
            Sql &= "Select Anno, Cast(Substring(RisultatoReale,1,1) As Int) As Fatti, "
            Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From PartiteChampions" & Girone & " "
            Sql &= "Where GiocCasa = " & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
            Sql &= "Union all "
            Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
            Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From PartiteChampions" & Girone & " "
            Sql &= "Where GiocFuori=" & Rec("CodGiocatore").Value & " "
            Sql &="And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                GoalFatti = Rec2("Fatti").Value
                GoalSubiti = Rec2("Subiti").Value
                DifferenzaGoal = GoalFatti - GoalSubiti
            Else
                GoalFatti = 0
                GoalSubiti = 0
                DifferenzaGoal = 0
            End If
            Rec2.Close()

            Sql = "Insert Into AppoChampions" & Girone & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & Rec("CodGiocatore").Value & ", "
            Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
            Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
            Sql &= " " & Rec("Punti").Value & ", "
            Sql &= " " & sGiocate & ", "
            Sql &= " " & sMedia.ToString.Replace(",", ".") & ", "
            Sql &= " " & GoalFatti & ", "
            Sql &= " " & GoalSubiti & ", "
            Sql &= " " & DifferenzaGoal & " "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)

            Rec.MoveNext()
        Loop
        Rec.Close()

        If StringaIn <> "" Then
            StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
            Sql = "Select Distinct GiocCasa As Gioc, B.Giocatore, B.Testo From PartiteChampions" & Girone & " A "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocCasa Not In (" & StringaIn & ") And GiocCasa<>99"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            Do Until Rec.Eof
                Sql = "Select Count(*) As Quante From PartiteChampions" & Girone & " "
                Sql &= "Where Anno = " & DatiGioco.AnnoAttuale & " "
                Sql &= "And (GiocCasa = " & Rec("Gioc").Value & " Or GiocFuori = " & Rec("Gioc").Value & ") "
                Sql &="And Vincitore Is Not Null"
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2(0).Value Is DBNull.Value = False Then
                    sGiocate = Rec2(0).Value
                Else
                    sGiocate = 0
                End If
                Rec2.Close()

                sMedia = 0

                Sql = "Select Anno,  Sum(Fatti) As Fatti , Sum(Subiti) As Subiti From ("
                Sql &= "Select Anno, Cast(Substring(RisultatoReale,1,1) As Int) As Fatti, "
                Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From PartiteChampions" & Girone & " "
                Sql &= "Where GiocCasa = " & Rec("Gioc").Value & " "
                Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
                Sql &= "Union all "
                Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
                Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From PartiteChampions" & Girone & " "
                Sql &= "Where GiocFuori=" & Rec("Gioc").Value & " "
                Sql &="And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = True Then
                    GoalFatti = 0
                    GoalSubiti = 0
                    DifferenzaGoal = 0
                Else
                    If Rec2(0).Value Is DBNull.Value = False Then
                        GoalFatti = Rec2("Fatti").Value
                        GoalSubiti = Rec2("Subiti").Value
                        DifferenzaGoal = GoalFatti - GoalSubiti
                    Else
                        GoalFatti = 0
                        GoalSubiti = 0
                        DifferenzaGoal = 0
                    End If
                End If
                Rec2.Close()

                Sql = "Insert Into AppoChampions" & Girone & " Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & Rec("Gioc").Value & ", "
                Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
                Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
                Sql &= "0, "
                Sql &= " " & sGiocate & ", "
                Sql &= " " & sMedia.ToString.Replace(",", ".") & ", "
                Sql &= " " & GoalFatti & ", "
                Sql &= " " & GoalSubiti & ", "
                Sql &= " " & DifferenzaGoal & " "
                Sql &= ")"
                Db.EsegueSql(ConnSql, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If

        Dim VecchiPunti As Integer = -1
        Dim Posiz As Integer
        Dim Riga As String
        Dim gMail As New GestioneMail

        Dim g As New Giocatori
        Dim numGioc As Integer = g.PrendeNumeroGiocatori
        Dim vPerc As Perc = g.PrendePercentualiCoppe(numGioc)
        g = Nothing
        Dim idPassate(vPerc.PassaggioEL + 1) As Integer
        Dim Passate(vPerc.PassaggioEL + 1) As String
        Dim Passaggio As Integer = 0
        Dim idPassateTurni(5) As Integer
        Dim PassateTurni(5) As String
        Dim PassaggioTurni As Integer = 0
        Dim imm As String

        StringaIn = ""

        Testo = "<br />" & gMail.ApreTesto() & "Classifica attuale per la Champions Girone " & Girone & gMail.ChiudeTesto() & "<br />"
        Testo += gMail.ApreTabella
        Riga = "Posizione;;Giocatore;Punti;Giocate;Media;Goal Fatti;Goal Subiti;Differenza;"
        Testo += gMail.ConverteTestoInRigaTabella(Riga, True)
        Sql = "Select A.* From AppoChampions" & Girone & " A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
        Sql &="Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            StringaIn += Rec("CodGiocatore").Value & ", "

            If VecchiPunti <> Rec("Punti").Value Then
                Posiz += 1
                VecchiPunti = Rec("Punti").Value
            End If

            imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

            Riga = Posiz & ";" & imm & ";" & Rec("Giocatore").Value & ";" & Rec("Punti").Value & ";" & Rec("Giocate").Value & ";" & Rec("Media").Value & ";" & Rec("GFatti").Value & ";" & Rec("GSubiti").Value & ";" & Rec("Differenza").Value & ";"

            Testo += gMail.ConverteTestoInRigaTabella(Riga)

            Passaggio += 1
            If Passaggio <= vPerc.PassaggioEL Then
                idPassate(Passaggio) = Rec("CodGiocatore").Value
                Passate(Passaggio) = Rec("Giocatore").Value
            End If

            PassaggioTurni += 1
            If PassaggioTurni <= 2 Then
                idPassateTurni(PassaggioTurni) = Rec("CodGiocatore").Value
                PassateTurni(PassaggioTurni) = Rec("Giocatore").Value
            End If

            Rec.MoveNext()
        Loop

        ' Prende i giocatori che non hanno punti in classifica
        Dim DaDove As String

        Try
            Sql = "Drop Table AppoChampions99"
            Db.EsegueSqlSenzaTRY(ConnSql, Sql)
        Catch ex As Exception

        End Try

        Sql = "Create Table AppoChampions99 (CodGiocatore Integer, DaDove Varchar(50))"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select Distinct(GiocCasa) From PartiteChampions" & Girone & " A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select DaDove From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = False Then
                DaDove = Rec2("DaDove").Value
            Else
                DaDove = ""
            End If
            Rec2.Close()

            Sql = "Insert Into AppoChampions99 Values (" & Rec(0).Value & ", '" & DaDove & "')"
            Db.EsegueSql(ConnSql, Sql)

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = ""
        If StringaIn.Length > 0 Then
            StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
            Sql = "Select A.*, B.Giocatore From AppoChampions99 A "
            Sql &= "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore "
            Sql &= "Where A.CodGiocatore Not In (" & StringaIn & ") And B.Pagante='S' Order By Giocatore"
        Else
            If StringaIn = "" Then
                Sql = "Select A.*, B.Giocatore From AppoChampions99 A "
                Sql &= "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore "
                Sql &= "Where B.Pagante='S' "
                Sql &="Order By Giocatore"
            End If
        End If
        If Sql <> "" Then
            Rec = Db.LeggeQuery(ConnSql, Sql)
            Do Until Rec.Eof
                imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)
                Riga = Posiz & ";" & imm & ";" & Rec("Giocatore").Value & ";0;0;0;0;0;0;"

                Testo += gMail.ConverteTestoInRigaTabella(Riga)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If

        Sql = "Drop Table AppoChampions99"
        Db.EsegueSql(ConnSql, Sql)
        ' Prende i giocatori che non hanno punti in classifica

        Testo += "</table><br />"

        gMail = Nothing

        Return Testo
    End Function

    Private Function CreaGironiChampions(Db As GestioneDB, ConnSql As Object) As String
        Dim Ritorno As String = ""
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim GironeA() As Integer = {}
        Dim GironeB() As Integer = {}
        Dim giocA() As String = {}
        Dim giocB() As String = {}
        Dim qA As Integer = 0
        Dim qB As Integer = 0
        Dim gm As New GestioneMail

        ' Legge squadre qualificate e le mette nei due gironi
        Sql = "Select A.*, B.Giocatore From ChampionsSquadre A "
        Sql &= "Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &="Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' "
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If Val(Rec("Posizione").Value) / 2 = Int(Val(Rec("Posizione").Value) / 2) Then
                qA += 1
                ReDim Preserve GironeA(qA)
                ReDim Preserve giocA(qA)
                GironeA(qA) = Rec("CodGiocatore").Value
                giocA(qA) = Rec("Giocatore").Value
            Else
                qB += 1
                ReDim Preserve GironeB(qB)
                ReDim Preserve giocB(qB)
                GironeB(qB) = Rec("CodGiocatore").Value
                giocB(qB) = Rec("Giocatore").Value
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        Ritorno += gm.ApreTesto() & "Girone A:" & gm.ChiudeTesto() & "<br />"
        For i As Integer = 1 To qA
            Ritorno += gm.ApreTesto() & giocA(i) & gm.ChiudeTesto() & "<br />"
        Next
        Ritorno += "<br />"
        Ritorno += gm.ApreTesto() & "Girone B:" & gm.ChiudeTesto() & "<br />"
        For i As Integer = 1 To qB
            Ritorno += gm.ApreTesto() & giocB(i) & gm.ChiudeTesto() & "<br />"
        Next
        Ritorno += "<br />"

        Ritorno += CreaAccoppiamentiGironeChampions(Db, ConnSql, qA, GironeA, "A")
        Ritorno += CreaAccoppiamentiGironeChampions(Db, ConnSql, qB, GironeB, "B")

        DatiGioco.GiornataChampionsLeague = 0
        AggiornaDatiDiGioco(Db)

        gm = Nothing

        Return Ritorno
    End Function

    Private Function CreaAccoppiamentiGironeChampions(Db As GestioneDB, ConnSql As Object, QuantiGioc As Integer, idGiocatore() As Integer, Girone As String) As String
        Dim Sql As String
        Dim Ritorno As String = ""

        Sql = "Delete From PartiteChampions" & Girone & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        If QuantiGioc / 2 <> Int(QuantiGioc / 2) Then
            QuantiGioc += 1
            ReDim Preserve idGiocatore(QuantiGioc)
            idGiocatore(QuantiGioc) = "99"
        End If

        Dim NomeFile As String = Percorso & "\Scontri\" & QuantiGioc.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0

        gFile.ApreFilePerLettura(NomeFile)

        Dim Numerello As Integer = 0

        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                Numerello += 1

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGiocatore(Casa)
                    Fuori = idGiocatore(Fuori)

                    Sql = "Insert Into PartiteChampions" & Girone & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    Giornate += 1

                    Sql = "Insert Into PartiteChampions" & Girone & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello + (QuantiGioc - 1) & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If
        Loop Until sLine Is Nothing

        Sql = "Delete From EventiChampions" & Girone & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        For i As Integer = 1 To (QuantiGioc - 1) * 2
            Sql = "Insert Into EventiChampions" & Girone & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & DatiGioco.Giornata + i & ", "
            Sql &= "1 "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        gFile.ChiudeFile()

        Return Ritorno
    End Function

    Private Function GiocaEuropaLeague(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataEuropaLeague + 1
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineELeague & "Giornata Europa League " & Giornata & gMail.RitornaImmagineELeague & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteEuropaLeague A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteEuropaleague", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        Dim TotGiornate As Integer

        Sql = "Select Max(Giornata) From PartiteEuropaLeague Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        TotGiornate = Rec(0).Value
        Rec.Close()

        Ritorno += CreaClassificaEuropaLeague(Db, ConnSql)

        If Giornata = TotGiornate Then
        Else
            DatiGioco.GiornataEuropaLeague = Giornata
            AggiornaDatiDiGioco(Db)
        End If

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function CreaClassificaEuropaLeague(Db As GestioneDB, ConnSql As Object) As String
        Dim sGiocate As Integer
        Dim sMedia As Single
        Dim GoalFatti As Integer
        Dim GoalSubiti As Integer
        Dim DifferenzaGoal As Integer
        Dim Testo As String = ""

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Delete From AppoEuropaLeague Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.*, B.Giocatore, B.Testo, B.CodGiocatore From vwClassificaEuropaLeague A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.Gioc=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' Order By Punti Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select Count(*) As Quante From PartiteEuropaLeague "
            Sql &= "Where Anno = " & DatiGioco.AnnoAttuale & " "
            Sql &= "And (GiocCasa = " & Rec("CodGiocatore").Value & " Or GiocFuori = " & Rec("CodGiocatore").Value & ") "
            Sql &="And Vincitore Is Not Null"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                sGiocate = Rec2(0).Value
            Else
                sGiocate = 0
            End If
            Rec2.Close()

            If sGiocate <> 0 Then
                sMedia = Rec("Punti").Value / sGiocate
            Else
                sMedia = 0
            End If

            Sql = "Select Anno,  Sum(Fatti) As Fatti , Sum(Subiti) As Subiti From ("
            Sql &= "Select Anno, Cast(Substring(RisultatoReale,1,1) As Int) As Fatti, "
            Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From PartiteEuropaLeague "
            Sql &= "Where GiocCasa = " & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
            Sql &= "Union all "
            Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
            Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From PartiteEuropaLeague "
            Sql &= "Where GiocFuori=" & Rec("CodGiocatore").Value & " "
            Sql &="And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                GoalFatti = Rec2("Fatti").Value
                GoalSubiti = Rec2("Subiti").Value
                DifferenzaGoal = GoalFatti - GoalSubiti
            Else
                GoalFatti = 0
                GoalSubiti = 0
                DifferenzaGoal = 0
            End If
            Rec2.Close()

            Sql = "Insert Into AppoEuropaLeague Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & Rec("CodGiocatore").Value & ", "
            Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
            Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
            Sql &= " " & Rec("Punti").Value & ", "
            Sql &= " " & sGiocate & ", "
            Sql &= " " & sMedia.ToString.Replace(",", ".") & ", "
            Sql &= " " & GoalFatti & ", "
            Sql &= " " & GoalSubiti & ", "
            Sql &= " " & DifferenzaGoal & " "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)

            Rec.MoveNext()
        Loop

        Dim VecchiPunti As Integer = -1
        Dim Posiz As Integer
        Dim Riga As String
        Dim gMail As New GestioneMail

        Dim g As New Giocatori
        Dim numGioc As Integer = g.PrendeNumeroGiocatori
        Dim vPerc As Perc = g.PrendePercentualiCoppe(numGioc)
        g = Nothing
        Dim idPassate(vPerc.PassaggioEL + 1) As Integer
        Dim Passate(vPerc.PassaggioEL + 1) As String
        Dim Passaggio As Integer = 0
        Dim idPassateTurni(5) As Integer
        Dim PassateTurni(5) As String
        Dim PassaggioTurni As Integer = 0
        Dim StringaIn As String = ""
        Dim imm As String

        Testo = "<br />" & gMail.ApreTesto() & "Classifica attuale per l'Europa League" & gMail.ChiudeTesto() & "<br />"
        Testo += gMail.ApreTabella
        Riga = "Posizione;;Giocatore;Punti;Giocate;Media;Goal Fatti;Goal Subiti;Differenza;"
        Testo += gMail.ConverteTestoInRigaTabella(Riga, True)
        Sql = "Select * From AppoEuropaLeague A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
        Sql &="Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            StringaIn += Rec("CodGiocatore").Value & ", "

            If VecchiPunti <> Rec("Punti").Value Then
                Posiz += 1
                VecchiPunti = Rec("Punti").Value
            End If

            imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

            Riga = Posiz & ";" & imm & ";" & Rec("Giocatore").Value & ";" & Rec("Punti").Value & ";" & Rec("Giocate").Value & ";" & Rec("Media").Value & ";" & Rec("GFatti").Value & ";" & Rec("GSubiti").Value & ";" & Rec("Differenza").Value & ";"

            Testo += gMail.ConverteTestoInRigaTabella(Riga)

            Passaggio += 1
            If Passaggio <= vPerc.PassaggioEL Then
                idPassate(Passaggio) = Rec("CodGiocatore").Value
                Passate(Passaggio) = Rec("Giocatore").Value
            End If

            PassaggioTurni += 1
            If PassaggioTurni <= 4 Then
                idPassateTurni(PassaggioTurni) = Rec("CodGiocatore").Value
                PassateTurni(PassaggioTurni) = Rec("Giocatore").Value
            End If

            Rec.MoveNext()
        Loop

        ' Prende i giocatori che non hanno punti in classifica
        Sql = ""
        If StringaIn.Length > 0 Then
            StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
            Sql = "Select A.*, B.Giocatore From EuropaLeagueSquadre A "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.CodGiocatore Not In (" & StringaIn & ") And B.Cancellato='N' And B.Pagante='S' Order By B.Giocatore"
        Else
            If StringaIn = "" Then
                Sql = "Select A.*, B.Giocatore From EuropaLeagueSquadre A "
                Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
                Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' Order By B.Giocatore"
            End If
        End If
        If Sql <> "" Then
            Rec = Db.LeggeQuery(ConnSql, Sql)
            Do Until Rec.Eof
                Posiz += 1

                imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Riga = Posiz & ";" & imm & ";" & Rec("Giocatore").Value & ";0;0;0;0;0;0;"

                Testo += gMail.ConverteTestoInRigaTabella(Riga)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If
        ' Prende i giocatori che non hanno punti in classifica

        Testo += "</table>"

        Dim GiornateEL As Integer = (vPerc.QuantiEuropaLeague + vPerc.QuantiIntertoto) ' - 1

        If GiornateEL = DatiGioco.GiornataEuropaLeague + 1 Then
            If vPerc.PassaggioEL > 0 Then
                'Dim GiornataCL As Integer = -1

                'Sql = "Select * From Eventi Where Descrizione='CREAZIONE GIRONI CHAMPIONS'"
                'Rec = Db.LeggeQuery(ConnSql, Sql)
                'If Rec.Eof = False Then
                '    GiornataCL = Rec("Giornata").Value
                'End If
                'Rec.Close()

                Sql = "Delete From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And DaDove='EUROPALEAGUE'"
                Db.EsegueSql(ConnSql, Sql)

                Testo += gMail.ApreTesto() & "<br />Giocatori qualificati per la Champion's" & gMail.ChiudeTesto() & "<br />"
                For i As Integer = 1 To vPerc.PassaggioEL
                    ' Inserimento squadre in Champion's (Eventuali)

                    imm = gMail.RitornaImmagineGiocatore(Passate(i))

                    Testo += gMail.ApreTesto() & imm & " " & Passate(i) & gMail.ChiudeTesto() & "<br />"

                    Sql = "Insert Into ChampionsSquadre Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & i + vPerc.QuantiChampions & ", "
                    Sql &= " " & idPassate(i) & ", "
                    Sql &= "'EUROPALEAGUE' "
                    Sql &= ")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If

            ' Creazione tabellone e calendario fase finale Europa League
            Testo += CreaTurniEuropaLeague(Db, ConnSql, idPassateTurni)
        End If

        gMail = Nothing

        Return Testo
    End Function

    Private Function CreaClassificaDerelitti(Db As GestioneDB, ConnSql As Object) As String
        Dim sGiocate As Integer
        Dim sMedia As Single
        Dim GoalFatti As Integer
        Dim GoalSubiti As Integer
        Dim DifferenzaGoal As Integer
        Dim Testo As String = ""

        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Delete From AppoDerelitti Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.*, B.Giocatore, B.Testo, B.CodGiocatore From vwClassificaDerelitti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.Gioc=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' Order By Punti Desc"

        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select Count(*) As Quante From PartiteDerelitti "
            Sql &= "Where Anno = " & DatiGioco.AnnoAttuale & " And (GiocCasa = " & Rec("CodGiocatore").Value & " Or GiocFuori = " & Rec("CodGiocatore").Value & ") And Vincitore Is Not Null"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                sGiocate = Rec2(0).Value
            Else
                sGiocate = 0
            End If
            Rec2.Close()

            If sGiocate <> 0 Then
                sMedia = Rec("Punti").Value / sGiocate
            Else
                sMedia = 0
            End If

            Sql = "Select Anno, Sum(Fatti) As Fatti , Sum(Subiti) As Subiti From ("
            Sql &= "Select Anno, Cast(Substring(RisultatoReale,1,1) As Int) As Fatti, "
            Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From PartiteDerelitti "
            Sql &= "Where GiocCasa = " & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
            Sql &= "Union all "
            Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
            Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From PartiteDerelitti "
            Sql &= "Where GiocFuori=" & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                GoalFatti = Rec2("Fatti").Value
                GoalSubiti = Rec2("Subiti").Value
                DifferenzaGoal = GoalFatti - GoalSubiti
            Else
                GoalFatti = 0
                GoalSubiti = 0
                DifferenzaGoal = 0
            End If
            Rec2.Close()

            Sql = "Insert Into AppoDerelitti Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & Rec("CodGiocatore").Value & ", "
            Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
            Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
            Sql &= " " & Rec("Punti").Value & ", "
            Sql &= " " & sGiocate & ", "
            Sql &= " " & sMedia.ToString.Replace(",", ".") & ", "
            Sql &= " " & GoalFatti & ", "
            Sql &= " " & GoalSubiti & ", "
            Sql &= " " & DifferenzaGoal & " "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)

            Rec.MoveNext()
        Loop
        Rec.Close

        ' Mette i giocatori a 0 punti
        Sql = "Select * From DerelittiSquadre A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.CodGiocatore Not In (Select Gioc From vwClassificaDerelitti Where Anno=" & DatiGioco.AnnoAttuale & ")"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            ' Giocate
            Sql = "Select Count(*) From PartiteDerelitti Where Anno=" & DatiGioco.AnnoAttuale & " And (GiocCasa=" & Rec("CodGiocatore").Value & " Or GiocFuori=" & Rec("CodGiocatore").Value & ") And RisultatoReale Is Not Null"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                sGiocate = Rec(0).Value
            Else
                sGiocate = 0
            End If
            Rec2.Close()

            Sql = "Select Anno, Sum(Fatti) As Fatti , Sum(Subiti) As Subiti From ("
            Sql &= "Select Anno, Cast(Substring(RisultatoReale,1,1) As Int) As Fatti, "
            Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From PartiteDerelitti "
            Sql &= "Where GiocCasa = " & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
            Sql &= "Union all "
            Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
            Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From PartiteDerelitti "
            Sql &= "Where GiocFuori=" & Rec("CodGiocatore").Value & " "
            Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2(0).Value Is DBNull.Value = False Then
                GoalFatti = Rec2("Fatti").Value
                GoalSubiti = Rec2("Subiti").Value
                DifferenzaGoal = GoalFatti - GoalSubiti
            Else
                GoalFatti = 0
                GoalSubiti = 0
                DifferenzaGoal = 0
            End If
            Rec2.Close()

            Sql = "Insert Into AppoDerelitti Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & Rec("CodGiocatore").Value & ", "
            Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
            Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
            Sql &= "0, "
            Sql &= " " & sGiocate & ", "
            Sql &= "0, "
            Sql &= " " & GoalFatti & ", "
            Sql &= " " & GoalSubiti & ", "
            Sql &= " " & DifferenzaGoal & " "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)

            Rec.MoveNext
        Loop
        Rec.Close
        ' Mette i giocatori a 0 punti

        Dim VecchiPunti As Integer = -1
        Dim Posiz As Integer
        Dim Riga As String
        Dim gMail As New GestioneMail

        Dim g As New Giocatori
        Dim numGioc As Integer = g.PrendeNumeroGiocatori
        Dim vPerc As Perc = g.PrendePercentualiCoppe(numGioc)
        g = Nothing
        Dim idPassate(vPerc.PassaggioEL + 1) As Integer
        Dim Passate(vPerc.PassaggioEL + 1) As String
        Dim Passaggio As Integer = 0
        Dim idPassateTurni(5) As Integer
        Dim PassateTurni(5) As String
        Dim PassaggioTurni As Integer = 0
        Dim imm As String

        Testo = gMail.ApreTestoTitolo() & "Classifica attuale per il torneo Pippettero" & gMail.ChiudeTesto() & "<br />"
        Testo += gMail.ApreTabella
        Riga = "Posizione;;Giocatore;Punti;Giocate;Media;Goal Fatti;Goal Subiti;Differenza;"
        Testo += gMail.ConverteTestoInRigaTabella(Riga, True)
        Sql = "Select * From AppoDerelitti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
        Sql &= "Order By Punti, GFatti, GSubiti Desc, Differenza, Media"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            If VecchiPunti <> Rec("Punti").Value Then
                Posiz += 1
                VecchiPunti = Rec("Punti").Value
            End If

            imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

            Riga = Posiz & ";" & imm & ";" & Rec("Giocatore").Value & ";" & Rec("Punti").Value & ";" & Rec("Giocate").Value & ";" & Rec("Media").Value & ";" & Rec("GFatti").Value & ";" & Rec("GSubiti").Value & ";" & Rec("Differenza").Value & ";"

            Testo += gMail.ConverteTestoInRigaTabella(Riga)

            Passaggio += 1
            If Passaggio <= vPerc.PassaggioEL Then
                idPassate(Passaggio) = Rec("CodGiocatore").Value
                Passate(Passaggio) = Rec("Giocatore").Value
            End If

            PassaggioTurni += 1
            If PassaggioTurni <= 4 Then
                idPassateTurni(PassaggioTurni) = Rec("CodGiocatore").Value
                PassateTurni(PassaggioTurni) = Rec("Giocatore").Value
            End If

            Rec.MoveNext()
        Loop
        Testo += "</table><br />"

        gMail = Nothing

        Return Testo
    End Function

    Private Function GiocaEuropaLeagueTurni(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataEuropaLeague + 1
        Dim Turno As String = RitornaTurnoCoppe(Db, ConnSql, "PartiteEuropaLeagueTurni", Giornata)
        If Giornata / 2 = Int(Giornata / 2) Then
            Turno += " di ritorno"
        Else
            Turno += " di andata"
        End If
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineELeague & "Giornata Europa League: " & Turno & gMail.RitornaImmagineELeague & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteEuropaLeagueTurni A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteEuropaLeagueTurni", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        If Giornata / 2 = Int(Giornata / 2) Then
            Ritorno += GestionePartiteScontriDaCalendario(Db, ConnSql, Giornata, "PartiteEuropaLeagueTurni", "EuropaLeagueTurni", 1)
        End If

        DatiGioco.GiornataEuropaLeague = Giornata
        AggiornaDatiDiGioco(Db)

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function GiocaCoppaItalia(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataCoppaItalia + 1
        Dim Turno As String = RitornaTurnoCoppe(Db, ConnSql, "PartiteCoppaItaliaTurni", Giornata)
        If Giornata / 2 = Int(Giornata / 2) Then
            Turno += " di ritorno"
        Else
            Turno += " di andata"
        End If
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineCItalia & "Giornata Coppa Italia. " & Turno & gMail.RitornaImmagineCItalia & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteCoppaItaliaTurni A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerRisultati(Db, Rec, ConnSql, "PartiteCoppaItaliaTurni", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        If Giornata / 2 = Int(Giornata / 2) Then
            Ritorno += GestionePartiteScontriDaCalendario(Db, ConnSql, Giornata, "PartiteCoppaItaliaTurni", "CoppaItaliaTurni", 4)
        End If

        DatiGioco.GiornataCoppaItalia = Giornata
        AggiornaDatiDiGioco(Db)

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function GiocaPippettero(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataDerelitti + 1
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmaginePippettero & " Giornata torneo Pippettero " & Giornata & " " & gMail.RitornaImmaginePippettero & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteDerelitti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &="Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteDerelitti", Giornata, "GiocCasa", "GiocFuori", True)
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()

        Ritorno += "</table>"

        DatiGioco.GiornataDerelitti = Giornata
        AggiornaDatiDiGioco(Db)

        Sql = "Select Max(Giornata) From PartiteDerelitti Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Dim MaxG As Integer = Rec(0).Value
        Rec.Close()

        'If MaxG = Giornata Then
        ' Chiusura campionato Derelitti. Creo classifica
        Ritorno += CreaClassificaDerelitti(Db, ConnSql)
        'End If

        gMail = Nothing

        Return Ritorno
    End Function

    Private Function GiocaInterToto(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = DatiGioco.GiornataInterToto + 1
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineIntertoto & "Giornata InterToto " & Giornata & gMail.RitornaImmagineIntertoto & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteInterToto A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteInterToto", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        Ritorno += "</table>"

        If Giornata / 2 = Int(Giornata / 2) Then
            Ritorno += GestionePartiteScontriDaCalendario(Db, ConnSql, Giornata, "PartiteInterToto", "InterToto", 1)
        End If

        DatiGioco.GiornataInterToto = Giornata
        AggiornaDatiDiGioco(Db)

        gMail = Nothing

        Return Ritorno
    End Function

    Public Function ControllaVincente(Db As GestioneDB, ConnSql As Object, RisUff As String, Tabella As String, idGiocCasa As Integer, idGiocFuori As Integer, Giornata As Integer, RisRealeA As String) As Integer
        Dim RisU As String = RisUff
        Dim RisR As String
        Dim gfc1 As Integer
        Dim gsc1 As Integer
        Dim gfc2 As Integer
        Dim gsc2 As Integer
        Dim gff1 As Integer
        Dim gsf1 As Integer
        Dim gff2 As Integer
        Dim gsf2 As Integer
        Dim Passa As Integer = -1
        Dim Sql As String
        Dim Rec2 As Object = CreateObject("ADODB.RecordSet")
        Dim RisRealeB As String

        If RisU <> "" Then
            If Mid(RisU, 1, RisU.IndexOf("-")) <> "X" Then
                gfc1 = Mid(RisU, 1, RisU.IndexOf("-"))
            Else
                gfc1 = -1
            End If
            If Mid(RisU, RisU.IndexOf("-") + 2, RisU.Length) <> "X" Then
                gsc1 = Mid(RisU, RisU.IndexOf("-") + 2, RisU.Length)
            Else
                gsc1 = -1
            End If
        Else
            gfc1 = -1
            gsc1 = -1
        End If

        gff2 = gsc1
        gsf2 = gfc1

        Sql = "Select * From " & PrefissoTabelle & Tabella & " "
        Sql &= "Where "
        Sql &= "Anno=" & DatiGioco.AnnoAttuale & " And GiocCasa=" & idGiocFuori & " "
        Sql &= "And GiocFuori=" & idGiocCasa & " And Giornata=" & Giornata
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = True Then
            ' Non c'è il ritorno. Partita secca
            RisU = RisUff
            RisRealeB = RisRealeA
        Else
            RisU = "" & Rec2("RisultatoUfficiale").Value
            RisRealeB = "" & Rec2("RisultatoReale").Value
        End If

        If RisU <> "" Then
            If Mid(RisU, RisU.IndexOf("-") + 2, RisU.Length) <> "X" Then
                gff1 = Mid(RisU, RisU.IndexOf("-") + 2, RisU.Length)
            Else
                gff1 = -1
            End If
            If Mid(RisU, 1, RisU.IndexOf("-")) <> "X" Then
                gsf1 = Mid(RisU, 1, RisU.IndexOf("-"))
            Else
                gsf1 = -1
            End If
        Else
            gsf1 = -1
            gff1 = -1
        End If

        gfc2 = gsf1
        gsc2 = gff1

        If gfc1 + gff1 > gfc2 + gff2 Then
            ' Numero di goal in casa più fuori della prima maggiore del numero di goal in casa più fuori della seconda
            ' Passa la prima
            Passa = idGiocCasa
        Else
            If gfc1 + gff1 < gfc2 + gff2 Then
                ' Numero di goal in casa più fuori della prima minore del numero di goal in casa più fuori della seconda
                ' Passa la seconda
                Passa = idGiocFuori
            Else
                If gfc1 + gff1 < gfc2 + gff2 Then
                    ' Numero di goal pari. Controllo goal fuori casa di tutte e due
                Else
                    If gff1 > gff2 Then
                        ' Passa la prima
                        Passa = idGiocCasa
                    Else
                        If gff1 < gff2 Then
                            ' Passa la seconda
                            Passa = idGiocFuori
                        Else
                            ' Goal pari anche in casa e fuori. Controllo i risultati reali
                            RisR = RisRealeA

                            If RisR <> "" Then
                                If Mid(RisR, 1, RisR.IndexOf("-")) <> "X" Then
                                    gfc1 = Mid(RisR, 1, RisR.IndexOf("-"))
                                Else
                                    gfc1 = -1
                                End If

                                If Mid(RisR, RisR.IndexOf("-") + 2, RisR.Length) <> "X" Then
                                    gsc1 = Mid(RisR, RisR.IndexOf("-") + 2, RisR.Length)
                                Else
                                    gsc1 = -1
                                End If
                            Else
                                gfc1 = -1
                                gsc1 = -1
                            End If

                            gff2 = gsc1
                            gsf2 = gfc1

                            RisR = RisRealeB

                            If RisR <> "" Then
                                If Mid(RisR, RisR.IndexOf("-") + 2, RisR.Length) <> "X" Then
                                    gff1 = Mid(RisR, RisR.IndexOf("-") + 2, RisR.Length)
                                Else
                                    gff1 = -1
                                End If

                                If Mid(RisR, 1, RisR.IndexOf("-")) <> "X" Then
                                    gsf1 = Mid(RisR, 1, RisR.IndexOf("-"))
                                Else
                                    gsf1 = -1
                                End If
                            Else
                                gff1 = -1
                                gsf1 = -1
                            End If

                            gfc2 = gsf1
                            gsc2 = gff1

                            If gfc1 + gff1 > gfc2 + gff2 Then
                                ' Numero di goal in casa più fuori della prima maggiore del numero di goal in casa più fuori della seconda
                                ' Passa la prima
                                Passa = idGiocCasa
                            Else
                                If gfc1 + gff1 < gfc2 + gff2 Then
                                    ' Numero di goal in casa più fuori della prima minore del numero di goal in casa più fuori della seconda
                                    ' Passa la seconda
                                    Passa = idGiocFuori
                                Else
                                    'If gfc1 + gff1 < gfc2 + gff2 Then
                                    ' Numero di goal pari. Controllo goal fuori casa di tutte e due
                                    If gff1 > gff2 Then
                                        ' Passa la prima
                                        Passa = idGiocCasa
                                    Else
                                        If gff1 < gff2 Then
                                            ' Passa la seconda
                                            Passa = idGiocFuori
                                        Else
                                            ' Goal pari anche qui... Non ne posso più genero random
                                            Randomize()
                                            Dim x As Integer = Int(Rnd(1) * 2) + 1
                                            If x = 1 Then
                                                Passa = idGiocCasa
                                            Else
                                                Passa = idGiocFuori
                                            End If
                                        End If
                                    End If
                                    'End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return Passa
    End Function

    Private Function GestionePartiteScontriDaCalendario(Db As GestioneDB, ConnSql As Object, Giornata As Integer, Tabella As String, Cosa As String, QuanteGiornateDiAttesa As Integer) As String
        Dim gm As New GestioneMail
        Dim Ritorno As String = "" ' "<br />" & gm.ApreTesto() & "Classificazione " & Cosa & gm.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim QuantePartite As Integer = 0

        Sql = "Select Max(Giornata) From " & PrefissoTabelle & Tabella & " Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        QuantePartite = Rec(0).Value
        Rec.Close()

        QuantePartite += 1

        Sql = "Delete From " & PrefissoTabelle & Tabella & " Where "
        Sql &= "Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & QuantePartite
        Db.EsegueSql(ConnSql, Sql)

        Dim PrimaSquadra As Integer = -1
        Dim SecondaSquadra As Integer = -1
        Dim NumPartita As Integer = 0
        Dim g As New Giocatori
        Dim Inserite As Boolean = False
        Dim Rimaste As Integer = 0
        Dim numRimaste() As Integer = {}
        Dim Passa As Integer
        Dim RisRealeA As String
        Dim imm1 As String
        Dim imm2 As String

        Sql = "Select * From " & PrefissoTabelle & Tabella & " "
        Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata - 1
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            RisRealeA = "" & Rec("RisultatoReale").Value

            Passa = ControllaVincente(Db, ConnSql, "" & Rec("RisultatoUfficiale").Value, Tabella, Rec("GiocCasa").Value, Rec("GiocFuori").Value, Giornata, RisRealeA)

            If PrimaSquadra = -1 Then
                PrimaSquadra = Passa
            Else
                SecondaSquadra = Passa

                NumPartita += 1

                Sql = "Insert Into " & PrefissoTabelle & Tabella & " Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & QuantePartite & ", "
                Sql &= " " & NumPartita & ", "
                Sql &= " " & PrimaSquadra & ", "
                Sql &= " " & SecondaSquadra & ", "
                Sql &= "null, "
                Sql &= "null, "
                Sql &= "null "
                Sql &=")"
                Db.EsegueSql(ConnSql, Sql)

                Sql = "Insert Into " & PrefissoTabelle & Tabella & " Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & QuantePartite + 1 & ", "
                Sql &= " " & NumPartita & ", "
                Sql &= " " & SecondaSquadra & ", "
                Sql &= " " & PrimaSquadra & ", "
                Sql &= "null, "
                Sql &= "null, "
                Sql &= "null "
                Sql &=")"
                Db.EsegueSql(ConnSql, Sql)

                imm1 = gm.RitornaImmagineGiocatore(g.TornaNickGiocatore(PrimaSquadra))
                imm2 = gm.RitornaImmagineGiocatore(g.TornaNickGiocatore(SecondaSquadra))

                Ritorno += gm.ApreTesto() & "<br />Prossimo scontro: " & imm1 & " " & g.TornaNickGiocatore(PrimaSquadra) & " - " & imm2 & " " & g.TornaNickGiocatore(SecondaSquadra) & gm.ChiudeTesto()

                Inserite = True

                Rimaste += 1
                ReDim Preserve numRimaste(Rimaste)
                numRimaste(Rimaste) = PrimaSquadra

                Rimaste += 1
                ReDim Preserve numRimaste(Rimaste)
                numRimaste(Rimaste) = SecondaSquadra

                PrimaSquadra = -1
                SecondaSquadra = -1
            End If

            Rec.MoveNext()
        Loop

        If PrimaSquadra <> -1 And SecondaSquadra = -1 Then
            NumPartita += 1

            Sql = "Insert Into " & PrefissoTabelle & Tabella & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & QuantePartite & ", "
            Sql &= " " & NumPartita & ", "
            Sql &= " " & PrimaSquadra & ", "
            Sql &= "-1, "
            Sql &= "null, "
            Sql &= "null, "
            Sql &= "null "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)

            Sql = "Insert Into " & PrefissoTabelle & Tabella & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & QuantePartite + 1 & ", "
            Sql &= " " & NumPartita & ", "
            Sql &= "-1, "
            Sql &= " " & PrimaSquadra & ", "
            Sql &= "null, "
            Sql &= "null, "
            Sql &= "null "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)

            imm1 = gm.RitornaImmagineGiocatore(g.TornaNickGiocatore(PrimaSquadra))

            Ritorno += gm.ApreTesto() & "<br />Giocatore qualificato: " & imm1 & " " & g.TornaNickGiocatore(PrimaSquadra) & gm.ChiudeTesto()

            Rimaste += 1
            ReDim Preserve numRimaste(Rimaste)
            numRimaste(Rimaste) = PrimaSquadra
        End If
        Rec.Close()

        Dim NumeroGiocatori As Integer = g.PrendeNumeroGiocatori
        Dim PercCoppe As Perc = g.PrendePercentualiCoppe(NumeroGiocatori)
        Dim QuanteDaPassare As Integer

        Select Case Cosa.ToUpper.Trim
            Case "INTERTOTO"
                QuanteDaPassare = PercCoppe.QuantiIntertoto
        End Select

        If Rimaste = 1 Then
            ' Vincente del trofeo
            imm1 = gm.RitornaImmagineGiocatore(g.TornaNickGiocatore(numRimaste(1)))

            Ritorno += gm.ApreTestoGrande() & "<br />Il giocatore " & imm1 & " " & g.TornaNickGiocatore(numRimaste(1)) & " si è aggiudicato il trofeo<br />" & gm.ChiudeTesto()
        End If

        If QuanteDaPassare = Rimaste Then
            ' Lo scontro si chiude qui perchè si è raggiunto il numero di squadre selezionato nella conf
            Inserite = False

            Select Case Cosa.ToUpper.Trim
                Case "INTERTOTO"
                    ' Aggiungo le squadre rimaste dell'InterToto all'Europa league e ne genero il calendario
                    Ritorno += gm.ApreTesto() & "<br />Giocatori passati ai gironi di Europa League:<br />" & gm.ChiudeTesto()
                    For i As Integer = 1 To Rimaste
                        imm1 = gm.RitornaImmagineGiocatore(g.TornaNickGiocatore(numRimaste(i)))

                        Ritorno += gm.ApreTesto() & imm1 & " " & g.TornaNickGiocatore(numRimaste(i)) & gm.ChiudeTesto() & "<br />"
                    Next

                    Dim SquadreEL As Integer = 0

                    Sql = "Select Max(Posizione)+1 From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale
                    Rec = Db.LeggeQuery(ConnSql, Sql)
                    If Rec(0).Value Is DBNull.Value = True Then
                        SquadreEL = 1
                    Else
                        SquadreEL = Rec(0).Value
                    End If
                    Rec.Close()

                    For i As Integer = 1 To Rimaste
                        Sql = "Insert Into EuropaLeagueSquadre Values ("
                        Sql &= " " & DatiGioco.AnnoAttuale & ", "
                        Sql &= " " & SquadreEL + (i - 1) & ", "
                        Sql &= " " & numRimaste(i) & ", "
                        Sql &= "'INTERTOTO' "
                        Sql &=")"
                        Db.EsegueSql(ConnSql, Sql)
                    Next

                    CreaCalendarioCoppe(Db, ConnSql, (PercCoppe.QuantiEuropaLeague) + (Rimaste), "EUROPALEAGUE")
            End Select
        End If

        If Inserite = True Then
            Dim GiornataDiDestinazioneEvento As Integer

            If Cosa.ToUpper.Trim = "INTERTOTO" Then
                GiornataDiDestinazioneEvento = DatiGioco.Giornata + QuanteGiornateDiAttesa
            Else
                If Rimaste = 2 Then
                    ' Imposta la finale alle ultime due giornate
                    GiornataDiDestinazioneEvento = GiornataFinale
                Else
                    If Rimaste = 4 Then
                        ' Imposta la semifinale alle ultime quattro giornate
                        GiornataDiDestinazioneEvento = GiornataSemifinale
                    Else
                        GiornataDiDestinazioneEvento = DatiGioco.Giornata + QuanteGiornateDiAttesa
                    End If
                End If
            End If

            Sql = "Delete From Eventi" & Cosa & " "
            Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & GiornataDiDestinazioneEvento
            Db.EsegueSql(ConnSql, Sql)

            Sql = "Insert Into Eventi" & Cosa & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & GiornataDiDestinazioneEvento & ", "
            Sql &= "1 "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)

            Sql = "Delete From Eventi" & Cosa & " "
            Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & GiornataDiDestinazioneEvento + 1
            Db.EsegueSql(ConnSql, Sql)

            Sql = "Insert Into Eventi" & Cosa & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & GiornataDiDestinazioneEvento + 1 & ", "
            Sql &= "1 "
            Sql &= ")"
            Db.EsegueSql(ConnSql, Sql)
        End If

        g = Nothing
        gm = Nothing

        Return Ritorno
    End Function

    Private Function CreaCoppeEuropee(Db As GestioneDB, ConnSql As Object) As String
        Dim Ritorno As String = ""
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Posizione As Integer = 0
        Dim Testo As String = ""

        Dim g As New Giocatori
        Dim gm As New GestioneMail
        Dim NumeroGiocatori As Integer = g.PrendeNumeroGiocatori
        Dim PercCoppe As Perc = g.PrendePercentualiCoppe(NumeroGiocatori)
        g = Nothing

        Dim QuantiMessiChampions As Integer = 0
        Dim QuantiMessiEL As Integer = 0
        Dim QuantiMessiIT As Integer = 0

        ' Acquisizione Champion's League
        Testo = gm.ApreTesto() & "Giocatori qualificati per la Champion's League" & gm.ChiudeTesto() & "<br />"
        Sql = "Delete From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Dim Squadre1 As Integer = Int(PercCoppe.QuantiChampions / 2)
        Dim Squadre2 As Integer = (PercCoppe.QuantiChampions - Squadre1)
        Dim imm As String

        'Sql = "Select Top " & Squadre1 & " A.* From AppoScontriDiretti A " & _
        '    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
        '    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata=" & DatiGioco.Giornata & " " & _
        '    "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"

        Sql = "Select Top " & Squadre1 & " A.CodGiocatore, B.Giocatore, Max(Punti) As Pt From AppoScontriDiretti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore  "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata=" & DatiGioco.Giornata & " "
        Sql &= "Group By A.CodGiocatore, B.Giocatore "
        Sql &= "Order By Pt Desc"

        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Posizione += 1
            Sql = "Insert Into ChampionsSquadre Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & Posizione & ", "
            Sql &= " " & Rec("CodGiocatore").Value & ", "
            Sql &= "'SCONTRI DIRETTI'"
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)

            QuantiMessiChampions += 1

            imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

            Testo += gm.ApreTesto() & Posizione & ": " & imm & " " & Rec("Giocatore").Value & " - Campionato" & gm.ChiudeTesto() & "<br />"

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore From Risultati A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & DatiGioco.Giornata & " And Cancellato='N' "
        Sql &= "Group By B.CodGiocatore, B.Giocatore "
        Sql &= "Order By PuntiRisultati Desc"
        Posizione = 0
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = True Then
                Posizione += 1

                Sql = "Insert Into ChampionsSquadre Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & Posizione + Squadre1 & ", "
                Sql &= " " & Rec("CodGiocatore").Value & ", "
                Sql &= "'RISULTATI ESATTI'"
                Sql &=")"
                Db.EsegueSql(ConnSql, Sql)

                imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Testo += gm.ApreTesto() & Posizione + Squadre1 & ": " & imm & " " & Rec("Giocatore").Value & " - Risultati" & gm.ChiudeTesto() & "<br />"

                QuantiMessiChampions += 1

                If Posizione = Squadre2 Then
                    Exit Do
                End If
            End If
            Rec2.Close()

            Rec.MoveNext()
        Loop
        Rec.Close()
        ' Acquisizione Champion's League

        ' Acquisizione Europa league
        Squadre1 = Int(PercCoppe.QuantiEuropaLeague / 2)
        Squadre2 = (PercCoppe.QuantiEuropaLeague - Squadre1)

        Testo += gm.ApreTesto() & "<br />Giocatori qualificati per l'Europa League" & gm.ChiudeTesto() & "<br />"
        Sql = "Delete From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Posizione = 0
        'Sql = "Select A.* From AppoScontriDiretti A " & _
        '    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
        '    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata=" & DatiGioco.Giornata & "  " & _
        '    "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"

        Sql = "Select A.CodGiocatore, B.Giocatore, Max(Punti) As Pt From AppoScontriDiretti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore  "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata=" & DatiGioco.Giornata & " "
        Sql &= "Group By A.CodGiocatore, B.Giocatore "
        Sql &= "Order By Pt Desc"

        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = True Then
                Posizione += 1
                Sql = "Insert Into EuropaLeagueSquadre Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & Posizione & ", "
                Sql &= " " & Rec("CodGiocatore").Value & ", "
                Sql &= "'SCONTRI DIRETTI'"
                Sql &=")"
                Db.EsegueSql(ConnSql, Sql)

                QuantiMessiEL += 1

                imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                Testo += gm.ApreTesto() & Posizione & ": " & imm & " " & Rec("Giocatore").Value & " - Campionato" & gm.ChiudeTesto() & "<br />"

                If Posizione = Squadre1 Then
                    Exit Do
                End If
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore From Risultati A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & DatiGioco.Giornata & " And Cancellato='N' "
        Sql &= "Group By B.CodGiocatore, B.Giocatore "
        Sql &= "Order By PuntiRisultati Desc"
        Posizione = 0
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = True Then
                Rec2.Close()

                Sql = "Select * From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = True Then
                    Posizione += 1

                    Sql = "Insert Into EuropaLeagueSquadre Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Posizione + Squadre1 & ", "
                    Sql &= " " & Rec("CodGiocatore").Value & ", "
                    Sql &= "'RISULTATI ESATTI'"
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    QuantiMessiEL += 1

                    imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                    Testo += gm.ApreTesto() & Posizione + Squadre1 & ": " & imm & " " & Rec("Giocatore").Value & " - Risultati" & gm.ChiudeTesto() & "<br />"

                    If Posizione = Squadre2 Then
                        Exit Do
                    End If
                End If
            End If
            Rec2.Close()

            Rec.MoveNext()
        Loop
        Rec.Close()

        Testo += gm.ApreTesto() & "Giocatori "
        For i As Integer = 1 To PercCoppe.QuantiIntertoto
            Testo += PercCoppe.QuantiEuropaLeague + i & " e "
        Next
        Testo = Mid(Testo, 1, Testo.Length - 3) & ": provenienza Intertoto" & gm.ChiudeTesto() & "<br />"

        'Testo += ApreTesto() & "Giocatori " & PercCoppe.QuantiEuropaLeague + 1 & " e " & PercCoppe.QuantiEuropaLeague + 2 & " provenienti dal torneo Intertoto" & ChiudeTesto() & "<br />"
        ' Acquisizione Europa league

        ' Intertoto
        Squadre1 = Int(PercCoppe.PassaggioIT / 2)
        Squadre2 = (PercCoppe.PassaggioIT - Squadre1)

        Testo += "<br />" & gm.ApreTesto() & "Giocatori qualificati per l'Intertoto" & gm.ChiudeTesto() & "<br />"
        Sql = "Delete From IntertotoSquadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Posizione = 0
        'Sql = "Select A.* From AppoScontriDiretti A " & _
        '    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
        '    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata=" & DatiGioco.Giornata & "  " & _
        '    "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"

        Dim sDiretti As New ScontriDiretti

        sDiretti.CreaTabellaClassificaScontriDiretti((DatiGioco.Giornata) + GiornataEventoCreazione, 1, False)

        sDiretti = Nothing

        Sql = "Select A.CodGiocatore, A.Giocatore, Pt, A.Anno, Max(B.Differenza) As Diff From ("
        Sql &= "Select A.CodGiocatore, B.Giocatore, Max(Punti) As Pt, A.Anno From AppoScontriDiretti A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.Cancellato ='N' And AllaGiornata<" & (DatiGioco.Giornata + GiornataEventoCreazione) & " "
        Sql &= "Group By A.Anno, A.CodGiocatore, B.Giocatore "
        Sql &= ") A Left Join AppoScontriDiretti B On A.Anno = B.Anno And A.Giocatore = B.Giocatore "
        Sql &= "Group By A.CodGiocatore, A.Giocatore, A.Pt, A.Anno "
        Sql &= "Order By Pt Desc, Diff Desc"

        'Sql = "Select A.CodGiocatore, B.Giocatore, Max(Punti) As Pt From AppoScontriDiretti A "
        'Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
        'Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And AllaGiornata<" & (DatiGioco.Giornata + GiornataEventoCreazione) & " "
        'Sql &= "Group By A.CodGiocatore, B.Giocatore "
        'Sql &= "Order By Pt Desc"

        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = True Then
                Rec2.Close()

                Sql = "Select * From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = True Then
                    Posizione += 1
                    Sql = "Insert Into IntertotoSquadre Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Posizione & ", "
                    Sql &= " " & Rec("CodGiocatore").Value & ", "
                    Sql &= "'SCONTRI DIRETTI'"
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    QuantiMessiIT += 1

                    imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                    Testo += gm.ApreTesto() & Posizione & ": " & imm & " " & Rec("Giocatore").Value & " - Campionato" & gm.ChiudeTesto() & "<br />"

                    If Posizione = Squadre1 Then
                        Exit Do
                    End If
                End If
            End If

            Rec.MoveNext()
        Loop
        Rec.Close()

        Sql = "Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore From Risultati A "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & DatiGioco.Giornata & " And Cancellato='N' "
        Sql &= "Group By B.CodGiocatore, B.Giocatore "
        Sql &= "Order By PuntiRisultati Desc"
        Posizione = 0
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Sql = "Select * From ChampionsSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = True Then
                Rec2.Close()

                Sql = "Select * From EuropaLeagueSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSql, Sql)
                If Rec2.Eof = True Then

                    Sql = "Select * From IntertotoSquadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSql, Sql)
                    If Rec2.Eof = True Then
                        Posizione += 1

                        Sql = "Insert Into IntertotoSquadre Values ("
                        Sql &= " " & DatiGioco.AnnoAttuale & ", "
                        Sql &= " " & Posizione + Squadre1 & ", "
                        Sql &= " " & Rec("CodGiocatore").Value & ", "
                        Sql &= "'RISULTATI ESATTI'"
                        Sql &=")"
                        Db.EsegueSql(ConnSql, Sql)

                        QuantiMessiIT += 1

                        imm = gm.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                        Testo += gm.ApreTesto() & Posizione + Squadre1 & ": " & imm & " " & Rec("Giocatore").Value & " - Risultati" & gm.ChiudeTesto() & "<br />"

                        If Posizione = Squadre2 Then
                            Exit Do
                        End If
                    End If
                End If
            End If
            Rec2.Close()

            Rec.MoveNext()
        Loop
        Rec.Close()
        ' Intertoto

        CreaCalendarioCoppe(Db, ConnSql, PercCoppe.PassaggioIT * 2, "INTERTOTO")

        gm = Nothing

        Return Testo
    End Function

    Private Function CreaTurniEuropaLeague(Db As GestioneDB, ConnSql As Object, Squadre() As Integer) As String
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim idGioc() As Integer = Squadre
        Dim qGioc As Integer = UBound(idGioc) - 1
        Dim Cosa As String = "EuropaLeagueTurni"

        Sql = "Delete From Partite" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        If qGioc / 2 <> Int(qGioc / 2) Then
            qGioc += 1
            ReDim Preserve idGioc(qGioc)
            idGioc(qGioc) = "99"
        End If

        ' Sposta squadre per accoppiamenti
        Dim Appo As Integer

        Appo = idGioc(3)
        idGioc(3) = idGioc(2)
        idGioc(2) = Appo
        ' Sposta squadre per accoppiamenti

        Dim TotPartiteAndata As Integer = qGioc
        Dim NomeFile As String = Percorso & "\Scontri\" & qGioc.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0

        gFile.ApreFilePerLettura(NomeFile)

        Sql = "Delete From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        For i As Integer = 1 To qGioc ' - 1
            Sql = "Insert Into " & PrefissoTabelle & Cosa & "Squadre" & " Values ("
            Sql &= " " & DatiGioco.AnnoAttuale & ", "
            Sql &= " " & i & ", "
            Sql &= " " & idGioc(i) & ", "
            Sql &= "'EUROPA LEAGUE' "
            Sql &=")"
            Db.EsegueSql(ConnSql, Sql)
        Next

        Dim Numerello As Integer = DatiGioco.GiornataEuropaLeague + 2

        If Numerello / 2 = Int(Numerello / 2) Then
            Numerello += 1
        End If

        Dim gMail As New GestioneMail
        Dim g As New Giocatori
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineChampions & "Scontri per il turno successivo" & gMail.RitornaImmagineChampions & gMail.ChiudeTesto() & "<br />"
        Dim Riga As String
        Dim Imm As String
        Dim NomeGiocatore As String

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        'Sql = "Select * From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale & " Order By Posizione"
        'Rec = Db.LeggeQuery(ConnSql, Sql)
        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGioc(Casa)
                    Fuori = idGioc(Fuori)

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    NomeGiocatore = g.TornaNickGiocatore(Casa)
                    Imm = gMail.RitornaImmagineGiocatore(NomeGiocatore)
                    Riga = Imm & ";" & NomeGiocatore & ";"
                    NomeGiocatore = g.TornaNickGiocatore(Fuori)
                    Imm = gMail.RitornaImmagineGiocatore(NomeGiocatore)
                    Riga += Imm & ";" & NomeGiocatore & ";"

                    Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

                    Giornate += 1

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Numerello + 1 & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If

            Exit Do

        Loop Until sLine Is Nothing
        'Rec.Close()

        Ritorno += "</table>"

        Sql = "Delete From Eventi" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        DatiGioco.GiornataEuropaLeague = Numerello - 1

        'For i As Integer = 1 To qGioc '- 1
        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 1 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Insert Into Eventi" & Cosa & " Values ("
        Sql &= " " & DatiGioco.AnnoAttuale & ", "
        Sql &= " " & DatiGioco.Giornata + 2 & ", "
        Sql &= "1 "
        Sql &= ")"
        Db.EsegueSql(ConnSql, Sql)
        'Next

        AggiornaDatiDiGioco(Db)

        gFile.ChiudeFile()

        Return Ritorno
    End Function

    Private Sub CreaCalendarioCoppe(Db As GestioneDB, ConnSql As Object, QuantiGiocatori As Integer, Cosa As String)
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim QuantiGiocIT As Integer
        Dim idGioc() As Integer = {}
        Dim qGioc As Integer = 0

        Sql = "Delete From Partite" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select * From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        QuantiGiocIT = 0
        Do Until Rec.Eof
            QuantiGiocIT += 1
            ReDim Preserve idGioc(QuantiGiocIT)
            idGioc(QuantiGiocIT) = Rec("CodGiocatore").Value

            Rec.MoveNext()
        Loop
        Rec.Close()

        If QuantiGiocatori / 2 <> Int(QuantiGiocatori / 2) Then
            QuantiGiocatori += 1
            QuantiGiocIT += 1
            ReDim Preserve idGioc(QuantiGiocIT)
            idGioc(QuantiGiocIT) = "99"
        End If

        Dim TotPartiteAndata As Integer = QuantiGiocIT
        Dim NomeFile As String = Percorso & "\Scontri\" & QuantiGiocIT.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As Integer
        Dim Fuori As Integer
        Dim Giornate As Integer = 0

        gFile.ApreFilePerLettura(NomeFile)

        'Sql = "Select * From " & PrefissoTabelle & Cosa & "Squadre Where Anno=" & DatiGioco.AnnoAttuale & " Order By Posizione"
        'Rec = Db.LeggeQuery(ConnSql, Sql)
        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")

                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Val(Mid(Partite(i), 1, Partite(i).IndexOf("-")))
                    Fuori = Val(Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length))

                    Casa = idGioc(Casa)
                    Fuori = idGioc(Fuori)

                    Sql = "Insert Into Partite" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Giornata & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & Casa & ", "
                    Sql &= " " & Fuori & ", "
                    Sql &= "null, "
                    Sql &= "null, "
                    Sql &= "null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    Giornate += 1

                    If Cosa <> "EUROPALEAGUE" Then
                        If Cosa = "INTERTOTO" Then
                            Sql = "Insert Into Partite" & Cosa & " Values ("
                            Sql &= " " & DatiGioco.AnnoAttuale & ", "
                            Sql &= " " & Giornata + 1 & ", "
                            Sql &= " " & i & ", "
                            Sql &= " " & Fuori & ", "
                            Sql &= " " & Casa & ", "
                            Sql &= "null, "
                            Sql &= "null, "
                            Sql &= "null "
                            Sql &=")"
                        Else
                            Sql = "Insert Into Partite" & Cosa & " Values ("
                            Sql &= " " & DatiGioco.AnnoAttuale & ", "
                            Sql &= " " & Giornata + (TotPartiteAndata - 1) & ", "
                            Sql &= " " & i & ", "
                            Sql &= " " & Fuori & ", "
                            Sql &= " " & Casa & ", "
                            Sql &= "null, "
                            Sql &= "null, "
                            Sql &= "null "
                            Sql &=")"
                        End If

                        Db.EsegueSql(ConnSql, Sql)

                        Giornate += 1
                    End If
                Next

                If Cosa = "INTERTOTO" Then
                    Exit Do
                End If
            End If
        Loop Until sLine Is Nothing
        'Rec.Close()

        Sql = "Delete From Eventi" & Cosa & " Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        Select Case Cosa.ToUpper.Trim
            Case "INTERTOTO"
                DatiGioco.GiornataInterToto = 0

                'For i As Integer = 1 To (QuantiGiocIT - 1) * 2
                Sql = "Insert Into Eventi" & Cosa & " Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & DatiGioco.Giornata + 1 & ", "
                Sql &= "1 "
                Sql &= ")"
                Db.EsegueSql(ConnSql, Sql)

                Sql = "Insert Into Eventi" & Cosa & " Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & DatiGioco.Giornata + 2 & ", "
                Sql &= "1 "
                Sql &= ")"
                Db.EsegueSql(ConnSql, Sql)
                'Next
            Case "EUROPALEAGUE"
                DatiGioco.GiornataEuropaLeague = 0

                For i As Integer = 1 To QuantiGiocIT - 1
                    Sql = "Insert Into Eventi" & Cosa & " Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & DatiGioco.Giornata + i & ", "
                    Sql &= "1 "
                    Sql &= ")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
        End Select
        AggiornaDatiDiGioco(Db)

        gFile.ChiudeFile()
    End Sub

    Private Function CreaScontriDiretti(Db As GestioneDB, ConnSql As Object) As String
        Dim Ritorno As String = ""
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Quantigiocatori As Integer
        Dim gm As New GestioneMail

        Sql = "Select Count(Giocatore) From Giocatori "
        Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N'"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Quantigiocatori = Rec(0).Value
        Rec.Close()

        Ritorno = gm.ApreTesto() & "Giocatori: " & Quantigiocatori & gm.ChiudeTesto() & "<br />"

        If Quantigiocatori / 2 <> Int(Quantigiocatori / 2) Then
            Quantigiocatori += 1
        End If

        Dim CodGioc(Quantigiocatori) As Integer
        Dim Conta As Integer = 0

        Sql = "Select * From Giocatori "
        Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N'"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Conta += 1
            CodGioc(Conta) = Rec("CodGiocatore").Value
            Rec.MoveNext()
        Loop
        Rec.Close()

        Dim TotPartiteAndata As Integer = Quantigiocatori
        Dim NomeFile As String = Percorso & "\Scontri\" & Quantigiocatori.ToString.Trim & ".txt"
        Dim gFile As New GestioneFilesDirectory
        Dim sLine As String
        Dim Giornata As String
        Dim Partite() As String
        Dim Casa As String
        Dim Fuori As String

        Ritorno += gm.ApreTesto() & "Numero partite: " & (TotPartiteAndata - 1) * 2 & gm.ChiudeTesto()
        Ritorno += "<br />"

        Sql = "Delete From ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale
        Db.EsegueSql(ConnSql, Sql)

        gFile.ApreFilePerLettura(NomeFile)
        Do
            sLine = gFile.RitornaRiga
            If sLine <> "" Then
                Giornata = Mid(sLine, 1, 2)
                sLine = Mid(sLine, 4, sLine.Length)
                Partite = sLine.Split(" ")
                For i As Integer = 1 To UBound(Partite) - 1
                    Casa = Mid(Partite(i), 1, Partite(i).IndexOf("-"))
                    Fuori = Mid(Partite(i), Partite(i).IndexOf("-") + 2, Partite.Length)

                    Sql = "Insert Into ScontriDiretti Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Giornata + DatiGioco.Giornata & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & CodGioc(Casa) & ", "
                    Sql &= " " & CodGioc(Fuori) & ", "
                    Sql &= "'', "
                    Sql &= "'', "
                    Sql &= "Null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)

                    Sql = "Insert Into ScontriDiretti Values ("
                    Sql &= " " & DatiGioco.AnnoAttuale & ", "
                    Sql &= " " & Val(Giornata) + (TotPartiteAndata - 1) + DatiGioco.Giornata & ", "
                    Sql &= " " & i & ", "
                    Sql &= " " & CodGioc(Fuori) & ", "
                    Sql &= " " & CodGioc(Casa) & ", "
                    Sql &= "'', "
                    Sql &= "'', "
                    Sql &= "Null "
                    Sql &=")"
                    Db.EsegueSql(ConnSql, Sql)
                Next
            End If
        Loop Until sLine Is Nothing
        gFile.ChiudeFile()
        gm = Nothing

        Return Ritorno
    End Function

    Private Function GiocaSuperCoppaItaliana(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = 38
        Dim Turno As String = "Finale secca"
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineSupercoppaItaliana & "Supercoppa Italiana. " & Turno & gMail.RitornaImmagineSupercoppaItaliana & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String
        Dim g As New Giocatori
        Dim imm1 As String
        Dim Passato As Integer

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Delete PartiteSuperCoppaItalianaTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=39"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteSuperCoppaItalianaTurni A Left Join Giocatori B "
        Sql &= "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa "
        Sql &= "Left Join Giocatori C "
        Sql &= "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori "
        Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteSuperCoppaItalianaTurni", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Passato = ControllaVincente(Db, ConnSql, "" & Rec("RisultatoUfficiale").Value, "PartiteSuperCoppaItalianaTurni", Rec("GiocCasa").Value, Rec("GiocFuori").Value, 38, "" & Rec("RisultatoReale").Value)

            Sql = "Insert Into PartiteSuperCoppaItalianaTurni Values (" & DatiGioco.AnnoAttuale & ", 39, 1, " & Passato & ", -1, null,null,null)"
            Db.EsegueSql(ConnSql, Sql)

            imm1 = gMail.RitornaImmagineGiocatore(g.TornaNickGiocatore(Passato))

            Exit Do
        Loop
        Rec.Close()
        Ritorno += "</table>"

        Ritorno += gMail.ApreTestoGrande() & "<br />Il giocatore " & imm1 & " " & g.TornaNickGiocatore(Passato) & " si è aggiudicato il trofeo<br />" & gMail.ChiudeTesto()

        gMail = Nothing
        g = Nothing

        Return Ritorno
    End Function

    Private Function GiocaSuperCoppaEuropea(Db As GestioneDB, ConnSql As Object) As String
        Dim gMail As New GestioneMail
        Dim Giornata As Integer = 38
        Dim Turno As String = "Finale secca"
        Dim Ritorno As String = "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineSupercoppaEuropea & "Supercoppa Europea. " & Turno & gMail.RitornaImmagineSupercoppaEuropea & gMail.ChiudeTesto() & "<br />"
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim sd As New ScontriDiretti
        Dim Riga As String
        Dim g As New Giocatori
        Dim imm1 As String
        Dim Passato As Integer

        Ritorno += gMail.ApreTabella
        Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
        Ritorno += gMail.ConverteTestoInRigaTabella(Riga, True)

        Sql = "Delete From  PartiteSuperCoppaEuropeaTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=39"
        Db.EsegueSql(ConnSql, Sql)

        Sql = "Select A.*, B.Giocatore As Casa, C.Giocatore As Fuori From PartiteSuperCoppaEuropeaTurni A Left Join Giocatori B " _
            & "On A.Anno=B.Anno And B.CodGiocatore=A.GiocCasa " _
            & "Left Join Giocatori C " _
            & "On A.Anno=C.Anno And C.CodGiocatore=A.GiocFuori " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Do Until Rec.Eof
            Riga = sd.ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "PartiteSuperCoppaEuropeaTurni", Giornata, "GiocCasa", "GiocFuori")
            Ritorno += gMail.ConverteTestoInRigaTabella(Riga, False)

            Passato = ControllaVincente(Db, ConnSql, "" & Rec("RisultatoUfficiale").Value, "PartiteSuperCoppaEuropeaTurni", Rec("GiocCasa").Value, Rec("GiocFuori").Value, 38, "" & Rec("RisultatoReale").Value)
            Sql = "Insert Into PartiteSuperCoppaEuropeaTurni Values (" & DatiGioco.AnnoAttuale & ", 39, 1, " & Passato & ", -1, null,null,null)"
            Db.EsegueSql(ConnSql, Sql)

            imm1 = gMail.RitornaImmagineGiocatore(g.TornaNickGiocatore(Passato))

            Exit Do
        Loop
        Rec.Close()
        Ritorno += "</table>"

        Ritorno += gMail.ApreTestoGrande() & "<br />Il giocatore " & imm1 & " " & g.TornaNickGiocatore(Passato) & " si è aggiudicato il trofeo<br />" & gMail.ChiudeTesto()

        gMail = Nothing
        g = Nothing

        Return Ritorno
    End Function

End Class
