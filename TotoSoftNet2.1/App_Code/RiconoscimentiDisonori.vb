Public Class RiconoscimentiDisonori

    Public Structure Premi
        Dim TipologiaPremio As Integer
        Const Invio1Giocatore As Integer = 1
        Const Invio3Giocatori As Integer = 2
        Const Invio5Giocatori As Integer = 3
        Const PiuDi20Punti As Integer = 4
        Const MenoDi5Punti As Integer = 5
        Const PiuDi9PuntiSegni As Integer = 6
        Const PiuDi11PuntiSegni As Integer = 7
        Const PiuDi13PuntiSegni As Integer = 8
        Const MenoDi3PuntiSegni As Integer = 9
        Const MenoDi2PuntiSegni As Integer = 10
        Const ZeroPuntiSegni As Integer = 11
        Const PiuDi50Punti As Integer = 12
        Const MenoDi20Punti As Integer = 13
        Const CinqueVoltePrimo As Integer = 14
        Const DieciVoltePrimo As Integer = 15
        Const CinqueVolteSecondo As Integer = 16
        Const DieciVolteSecondo As Integer = 17
        Const MaiVintoDopo20Giornate As Integer = 18
        Const MaiVintoDopo30Giornate As Integer = 19
        Const CinqueVolteUltimo As Integer = 20
        Const DieciVolteUltimo As Integer = 21
        Const MaiTappiDopoVentesima As Integer = 22
        Const MaiTappiDopoTrentesima As Integer = 23
        Const Pagato50PerCentoAllaDecima As Integer = 24
        Const Pagato80PerCentoAlla19 As Integer = 25
        Const PagatoNullaDecima As Integer = 26
        Const PagatoNullaVentesima As Integer = 27
        Const CentoAccessi As Integer = 28
    End Structure

    Public Function ControlliSuControlloSchedina() As String
        Dim Ritorno As String = ""
        Dim g As New GestioneMail
        Dim Db As New GestioneDB
        Dim TestoMail As String = ""
        Dim Trovati As Boolean = False

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim gMail As New GestioneMail

            TestoMail += "<hr />" & gMail.ApreTestoTitolo() & "Riconoscimenti / Disonori della giornata:" & gMail.ChiudeTesto()
            TestoMail += gMail.ApreTabella
            Ritorno = ";Giocatore;Premio;;Punti;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno, True)

            ' Elimina tutti i riconoscimenti della giornata che non devono duplicarsi
            Sql = "Delete From RiconDisonGioc Where idAnno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And idPremio>3 And idPremio<28"
            Db.EsegueSql(ConnSQL, Sql)
            ' Elimina tutti i riconoscimenti della giornata che non devono duplicarsi

            ' Controlla premio 4 - 11
            Sql = "Select A.*, B.Giocatore, PuntiSegni, PuntiRisultati, PuntiJolly From Risultati A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore And A.Concorso=C.Giornata " & _
                "Where " & _
                "A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "A.Concorso=" & DatiGioco.Giornata & " And B.Cancellato='N' " & _
                "And C.Giornata Is Null"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If Rec("PuntiSegni").Value + Rec("PuntiRisultati").Value + Rec("PuntiJolly").Value >= 20 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PiuDi20Punti, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                    Trovati = True
                End If
                If Rec("PuntiSegni").Value + Rec("PuntiRisultati").Value + Rec("PuntiJolly").Value <= 3 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MenoDi5Punti, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                    Trovati = True
                End If

                Dim PuntiSegni As Single = Rec("PuntiSegni").Value

                If PuntiSegni >= 9 And PuntiSegni < 11 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PiuDi9PuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                    Trovati = True
                Else
                    If PuntiSegni >= 11 And PuntiSegni < 13 Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PiuDi11PuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    Else
                        If PuntiSegni >= 13 Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PiuDi13PuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                    End If
                End If

                If PuntiSegni = 3 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MenoDi3PuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                    Trovati = True
                End If
                If PuntiSegni = 2 Or PuntiSegni = 1 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MenoDi2PuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                    Trovati = True
                End If
                If PuntiSegni = 0 Then
                    Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.ZeroPuntiSegni, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                    Trovati = True
                    TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            ' Controlla premio 4 - 11

            ' Controllo premio 12 - 13
            If DatiGioco.Giornata > 3 Then
                Sql = "Select A.CodGiocatore, B.Giocatore, Sum(PuntiSegni)+Sum(PuntiRisultati)+Sum(PuntiJolly) As PuntiTot From Risultati A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore  " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore And A.Concorso=C.Giornata " & _
                    "Where " & _
                    "A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "A.Concorso>" & DatiGioco.Giornata - 3 & " And Concorso<=" & DatiGioco.Giornata & " And B.Cancellato='N' And " & _
                    "C.Giornata Is Null " & _
                    "Group By A.CodGiocatore, B.Giocatore"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If Rec("PuntiTot").Value >= 50 Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PiuDi50Punti, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    If Rec("PuntiTot").Value <= 20 Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MenoDi20Punti, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 12 - 13 

            Dim Rec2 As Object = CreateObject("ADODB.Recordset")

            ' Controllo premio 14 - 15
            Sql = "Select A.CodGiocatore, B.Giocatore, Count(Progressivo) As Quante From Primi A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore And A.Giornata=C.Giornata " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata is Null Group By A.CodGiocatore, B.Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If Rec("Quante").Value >= 5 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                       "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                       "idPremio=14 And " & _
                       "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.CinqueVoltePrimo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If
                If Rec("Quante").Value >= 10 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                       "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                       "idPremio=15 And " & _
                       "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Db.EsegueSql(ConnSQL, "Delete From RiconDisonGioc Where " & _
                                      "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                                      "CodGioc=" & Rec("CodGiocatore").Value & " And " & _
                                      "idPremio=" & Premi.CinqueVoltePrimo & " ")
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.DieciVoltePrimo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            ' Controllo premio 14 - 15

            ' Controllo premio 16 - 17
            Sql = "Select A.CodGiocatore, B.Giocatore, Count(Progressivo) As Quante From Secondi A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore And A.Giornata=C.Giornata " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null Group By A.CodGiocatore, B.Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If Rec("Quante").Value >= 5 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                       "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                       "idPremio=16 And " & _
                       "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.CinqueVolteSecondo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If
                If Rec("Quante").Value >= 10 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                       "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                       "idPremio=17 And " & _
                       "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Db.EsegueSql(ConnSQL, "Delete From RiconDisonGioc Where " & _
                                      "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                                      "CodGioc=" & Rec("CodGiocatore").Value & " And " & _
                                      "idPremio=" & Premi.CinqueVolteSecondo & " ")
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.DieciVolteSecondo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            ' Controllo premio 16 - 17

            ' Controllo premio 18
            If DatiGioco.Giornata = 20 Then
                Sql = "Select CodGiocatore,Giocatore From Giocatori " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In " & _
                    "(Select CodGiocatore From Primi Where Anno=" & DatiGioco.AnnoAttuale & ") And Cancellato='N'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Sql = "Select * From RiconDisonGioc Where " & _
                        "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                        "idPremio=18 And " & _
                        "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MaiVintoDopo20Giornate, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 18

            ' Controllo premio 19
            If DatiGioco.Giornata = 30 Then
                Sql = "Select CodGiocatore,Giocatore From Giocatori " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In " & _
                    "(Select CodGiocatore From Primi Where Anno=" & DatiGioco.AnnoAttuale & ") And Cancellato='N'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Sql = "Select * From RiconDisonGioc Where " & _
                        "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                        "idPremio=19 And " & _
                        "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MaiVintoDopo30Giornate, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 19

            ' Controllo premio 20 - 21
            Sql = "Select A.CodGiocatore, B.Giocatore, Count(Progressivo) As Quante From Ultimi A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore And A.Giornata=C.Giornata " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null Group By A.CodGiocatore, B.Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If Rec("Quante").Value >= 5 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                        "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                        "idPremio=20 And " & _
                        "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.CinqueVolteUltimo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If
                If Rec("Quante").Value >= 10 Then
                    Sql = "Select * From RiconDisonGioc Where " & _
                        "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                        "idPremio=21 And " & _
                        "CodGioc=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        Db.EsegueSql(ConnSQL, "Delete From RiconDisonGioc Where " & _
                                      "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                                      "CodGioc=" & Rec("CodGiocatore").Value & " And " & _
                                      "idPremio=" & Premi.CinqueVolteUltimo & " ")
                        Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.DieciVolteUltimo, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                        TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                        Trovati = True
                    End If
                    Rec2.Close()
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            ' Controllo premio 20 - 21

            ' Controllo premio 22
            If DatiGioco.Giornata = 20 Then
                ' And A.Giornata=C.Giornata " & _
                Sql = "Select A.*, B.Giocatore From DettaglioGiocatori A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And NumeroTappi=0 And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If "" & Rec("CodGiocatore").Value <> "" And "" & Rec("Giocatore").Value <> "" Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=22 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MaiTappiDopoVentesima, "" & Rec("CodGiocatore").Value, "" & Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 22

            ' Controllo premio 23
            If DatiGioco.Giornata = 30 Then
                Sql = "Select A.*, B.Giocatore From DettaglioGiocatori A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And NumeroTappi=0 And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If "" & Rec("CodGiocatore").Value <> "" And "" & Rec("Giocatore").Value <> "" Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=23 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.MaiTappiDopoTrentesima, "" & Rec("CodGiocatore").Value, "" & Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 23

            ' Controllo premio 24
            If DatiGioco.Giornata = 10 Then
                Sql = "Select A.CodGiocatore, Convert(Integer, ((Reali+1)/TotVersamento)*100)-1 As Perc, B.Giocatore From Bilancio A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If Rec("Perc").Value >= 50 Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=24 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.Pagato50PerCentoAllaDecima, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 24

            ' Controllo premio 25
            If DatiGioco.Giornata = 19 Then
                Sql = "Select A.CodGiocatore, Convert(Integer, ((Reali+1)/TotVersamento)*100)-1 As Perc, B.Giocatore From Bilancio A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If Rec("Perc").Value >= 80 Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=25 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.Pagato80PerCentoAlla19, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 25

            ' Controllo premio 26
            If DatiGioco.Giornata = 10 Then
                Sql = "Select A.CodGiocatore, Reali, Vinti, B.Giocatore From Bilancio A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If Rec("Reali").Value = 0 And Rec("Vinti").Value = 0 Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=26 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PagatoNullaDecima, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 26

            ' Controllo premio 27
            If DatiGioco.Giornata = 20 Then
                Sql = "Select A.CodGiocatore, Reali, Vinti, B.Giocatore From Bilancio A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                    "Left Join QuandoTappi C On A.Anno=C.Anno And A.CodGiocatore=C.CodGiocatore " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And C.Giornata Is Null"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    If Rec("Reali").Value = 0 And Rec("Vinti").Value = 0 Then
                        Sql = "Select * From RiconDisonGioc Where " & _
                            "idAnno=" & DatiGioco.AnnoAttuale & " And " & _
                            "idPremio=27 And " & _
                            "CodGioc=" & Rec("CodGiocatore").Value
                        Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec2.Eof = True Then
                            Ritorno = CreaRigaDaScrivere(Db, ConnSQL, Premi.PagatoNullaVentesima, Rec("CodGiocatore").Value, Rec("Giocatore").Value)
                            TestoMail += gMail.ConverteTestoInRigaTabella(Ritorno)
                            Trovati = True
                        End If
                        Rec2.Close()
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Controllo premio 27
        End If

        Db = Nothing
        g = Nothing

        TestoMail += "<hr /></table> "

        If Trovati = False Then
            TestoMail = ""
        End If

        Return TestoMail
    End Function

    Public Function CreaRigaDaScrivere(Db As GestioneDB, ConnSql As Object, idPremio As Integer, Chi As Integer, Giocatore As String) As String
        Dim Ritorno As String = ""
        Dim G As New GestioneMail

        ScriveRiga(Db, ConnSql, idPremio, Chi, False)
        Ritorno = G.RitornaImmagineGiocatore(Giocatore) & ";" & Giocatore & ";" & RitornaDescrizionePremio(Db, ConnSql, idPremio) & ";" & RitornaImmaginePremio(Db, ConnSql, idPremio) & ";" & RitornaPuntiPremio(Db, ConnSql, idPremio) & ";"
        G = Nothing

        Return Ritorno
    End Function

    Private Function RitornaPuntiPremio(Db As GestioneDB, ConnSql As Object, idPremio As Integer) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Punti As String = ""

        Sql = "Select Punti From RiconDison Where idPremio=" & idPremio
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            Punti = Rec("Punti").Value
        End If
        Rec.Close()

        Return Punti
    End Function

    Private Function RitornaDescrizionePremio(Db As GestioneDB, ConnSql As Object, idPremio As Integer) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Descrizione As String = ""

        Sql = "Select Descrizione From RiconDison Where idPremio=" & idPremio
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            Descrizione = Rec("Descrizione").Value
        End If
        Rec.Close()

        Return Descrizione
    End Function

    Private Function RitornaImmaginePremio(Db As GestioneDB, ConnSql As Object, idPremio As Integer) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Immagine As String = ""

        Sql = "Select Immagine From RiconDison Where idPremio=" & idPremio
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            Immagine = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value
            Immagine = "<img src='" & Immagine & "' width='30px' height='30px' />"
        End If
        Rec.Close()

        Return Immagine
    End Function

    Public Sub ControllaRegistrazioneAmici(Chi As Integer)
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Quanti As Integer

            Sql = "Select Count(*) As Quanti From Amici Where " & _
                "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "CodGiocatore=" & Chi
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Quanti = 0
            Else
                Quanti = Rec("Quanti").Value
            End If
            Rec.Close()

            Dim Elimina As Boolean = False
            Dim idPremio As Integer

            Select Case Quanti
                Case 1
                    idPremio = Premi.Invio1Giocatore
                    Elimina = True
                Case 3
                    idPremio = Premi.Invio3Giocatori
                    Elimina = True
                Case 5
                    idPremio = Premi.Invio5Giocatori
                    Elimina = True
            End Select

            If Elimina = True Then
                Sql = "Delete From RiconDisonGioc Where " & _
                    "idPremio=" & Premi.Invio1Giocatore & " Or " & _
                    "idPremio=" & Premi.Invio3Giocatori & " Or " & _
                    "idPremio=" & Premi.Invio5Giocatori
                Db.EsegueSql(ConnSQL, Sql)

                ScriveRiga(Db, ConnSQL, idPremio, Chi, True)
            End If
            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ScriveRiga(Db As GestioneDB, ConnSQL As Object, idPremio As Integer, aChi As Integer, InvioMail As Boolean)
        Dim Sql As String

        Sql = "Insert Into RiconDisonGioc Values (" & _
            " " & DatiGioco.AnnoAttuale & ", " & _
            " " & aChi & ", " & _
            " " & idPremio & ", " & _
            " " & DatiGioco.Giornata & " " & _
            ")"
        Db.EsegueSql(ConnSQL, Sql)

        If InvioMail = True Then
            Dim gm As New GestioneMail
            gm.InviaMailPerRiconoscimentoDisonore(aChi, idPremio)
            gm = Nothing
        End If
    End Sub
End Class
