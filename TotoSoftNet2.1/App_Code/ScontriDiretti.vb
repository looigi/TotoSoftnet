Public Class ScontriDiretti

    Public Function CreaTabellaClassificaScontriDiretti(Giornata As Integer, Chi As Integer, Optional RitornaTesto As Boolean = False) As String
        Dim sGiocate As Integer
        Dim sMedia As Single
        Dim GoalFatti As Integer
        Dim GoalSubiti As Integer
        Dim DifferenzaGoal As Integer
        Dim Testo As String = ""
        Dim NomeTabAppoggio As String = PrefissoTabelle & "ClassCamp_" & Chi & ""

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Delete From AppoScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocVisua=" & Chi
            Db.EsegueSql(ConnSQL, Sql)

            Try
                Sql = "Create Table " & NomeTabAppoggio & " (Anno smallint, Gioc smallint, Punti smallint)"
                Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
            Catch ex As Exception

            End Try

            Sql = "Delete From " & NomeTabAppoggio
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "SELECT Anno, Gioc, SUM(Punti) AS Punti "
            Sql &= "FROM (SELECT Anno, Vincitore AS Gioc, COUNT(Vincitore) * 3 AS Punti "
            Sql &= "FROM ScontriDiretti "
            Sql &= "WHERE Vincitore <> -1 And Vincitore <> 0 And Giornata<=" & Giornata & " "
            Sql &= "GROUP BY Anno, Vincitore "
            Sql &= "UNION ALL "
            Sql &= "SELECT Anno, GiocatoreCasa AS Gioc, COUNT(GiocatoreCasa) AS Punti "
            Sql &= "FROM ScontriDiretti AS ScontriDiretti_2 "
            Sql &= "WHERE Vincitore = 0 And Giornata<=" & Giornata & " "
            Sql &= "GROUP BY Anno, GiocatoreCasa "
            Sql &= "UNION ALL "
            Sql &= "SELECT Anno, GiocatoreFuori AS Gioc, COUNT(GiocatoreFuori) AS Punti "
            Sql &= "FROM ScontriDiretti AS ScontriDiretti_1 "
            Sql &= "WHERE Vincitore = 0 And Giornata<=" & Giornata & " "
            Sql &= "GROUP BY Anno, GiocatoreFuori) AS A "
            Sql &="GROUP BY Anno, Gioc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Sql = "Insert Into " & NomeTabAppoggio & " Values ("
                Sql &= " " & Rec("Anno").Value & ", "
                Sql &= " " & Rec("Gioc").Value & ", "
                Sql &= " " & Rec("Punti").Value & " "
                Sql &= ")"
                Db.EsegueSql(ConnSQL, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select A.*, B.Giocatore, B.Testo, B.CodGiocatore From " & NomeTabAppoggio & " A "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.Gioc=B.CodGiocatore "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' And B.Cancellato='N' Order By Punti Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Sql = "Select Count(*) As Quante From ScontriDiretti "
                Sql &= "Where Anno = " & DatiGioco.AnnoAttuale & " "
                Sql &= "And (GiocatoreCasa = " & Rec("CodGiocatore").Value & " Or GiocatoreFuori = " & Rec("CodGiocatore").Value & ") "
                Sql &="And Vincitore Is Not Null And Giornata<=" & Giornata
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
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
                Sql &= "Cast(SUBSTRING(RisultatoReale,3,1) As int) As Subiti From ScontriDiretti "
                Sql &= "Where GiocatoreCasa = " & Rec("CodGiocatore").Value & " "
                Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0 "
                Sql &= "Union all "
                Sql &= "Select Anno, Cast(SUBSTRING(RisultatoReale,3,1) As Int) As Fatti, "
                Sql &= "Cast( Substring(RisultatoReale,1,1) As Int) As Subiti From ScontriDiretti "
                Sql &= "Where GiocatoreFuori=" & Rec("CodGiocatore").Value & " "
                Sql &= "And Vincitore Is Not Null And CHARINDEX('X',RisultatoReale)=0) A Where "
                Sql &= "Anno=" & DatiGioco.AnnoAttuale & " Group By Anno"
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
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

                Sql = "Insert Into AppoScontriDiretti Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & Rec("CodGiocatore").Value & ", "
                Sql &= "'" & SistemaTestoPerDB(Rec("Giocatore").Value) & "', "
                Sql &= "'" & SistemaTestoPerDB("" & Rec("Testo").Value) & "', "
                Sql &= " " & Rec("Punti").Value & ", "
                Sql &= " " & sGiocate & ", "
                Sql &= " " & sMedia.ToString.Replace(",", ".") & ", "
                Sql &= " " & GoalFatti & ", "
                Sql &= " " & GoalSubiti & ", "
                Sql &= " " & DifferenzaGoal & ", "
                Sql &= " " & Chi & ", "
                Sql &= " " & Giornata & " "
                Sql &=")"
                Db.EsegueSql(ConnSQL, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()

            If RitornaTesto = True Then
                Dim CiSonoScontriDiretti As Boolean = False

                Sql = "Select * From ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    CiSonoScontriDiretti = True
                End If
                Rec.Close()

                If CiSonoScontriDiretti = True Then
                    Dim VecchiPunti As Integer = -1
                    Dim Posiz As Integer
                    Dim Riga As String
                    Dim gMail As New GestioneMail
                    Dim StringaIn As String = ""
                    Dim Imm As String

                    Testo = "<br />" & gMail.ApreTesto() & "Classifica per gli scontri diretti" & gMail.ChiudeTesto() & "<br />"
                    Testo += gMail.ApreTabella
                    Riga = "Posizione;;Giocatore;Punti;Giocate;Media;Goal Fatti;Goal Subiti;Differenza;"
                    Testo += gMail.ConverteTestoInRigaTabella(Riga, True)
                    Sql = "Select A.* From AppoScontriDiretti A "
                    Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
                    Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' And CodGiocVisua=" & Chi & " "
                    Sql &="Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until Rec.Eof
                        StringaIn += Rec("CodGiocatore").Value & ", "

                        If VecchiPunti <> Rec("Punti").Value Then
                            Posiz += 1
                            VecchiPunti = Rec("Punti").Value
                        End If

                        Imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)

                        Riga = Posiz & ";" & Imm & ";" & Rec("Giocatore").Value & ";" & Rec("Punti").Value & ";" & Rec("Giocate").Value & ";" & Rec("Media").Value & ";" & Rec("GFatti").Value & ";" & Rec("GSubiti").Value & ";" & Rec("Differenza").Value & ";"

                        Testo += gMail.ConverteTestoInRigaTabella(Riga)

                        Rec.MoveNext()
                    Loop

                    Sql = ""
                    If StringaIn.Length > 0 Then
                        StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
                        Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In (" & StringaIn & ") And Cancellato='N' And Pagante='S' Order By Giocatore"
                    Else
                        If StringaIn = "" Then
                            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' And Pagante='S' Order By Giocatore"
                        End If
                    End If
                    If Sql <> "" Then
                        Dim Sql2 As String = "Select Max(Giocate) From AppoScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocVisua=" & Chi
                        Dim Giocate As Integer
                        Rec = Db.LeggeQuery(ConnSQL, Sql2)
                        If Rec(0).Value Is DBNull.Value = True Then
                            Giocate = 0
                        Else
                            Giocate = Rec(0).Value
                        End If
                        Rec.Close()

                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        Do Until Rec.Eof
                            Imm = gMail.RitornaImmagineGiocatore(Rec("Giocatore").Value)
                            Riga = Posiz & ";" & Imm & ";" & Rec("Giocatore").Value & ";0;" & Giocate & ";0;" & "0;0;0;"

                            Testo += gMail.ConverteTestoInRigaTabella(Riga)

                            Rec.MoveNext()
                        Loop
                        Rec.Close()
                    End If

                    Testo += "</table> "

                    gMail = Nothing
                End If
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Testo
    End Function

    Public Function ControlloScontriDiretti(Db As GestioneDB, ConnSql As Object) As String
        Dim TestoMail As String = ""
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Riga As String = ""
        Dim gMail As New GestioneMail

        If GiornataEventoCreazione = -1 Then
            Sql = "Select * From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " And Descrizione='CREAZIONE SCONTRI DIRETTI'"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            GiornataEventoCreazione = Rec("Giornata").Value
            Rec.Close()
        End If

        Sql = "Select * From vwScontriDiretti Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            TestoMail += "<hr />" & gMail.ApreTestoTitolo() & gMail.RitornaImmagineScontriDiretti & " " & "SCONTRI DIRETTI Giornata " & DatiGioco.Giornata - GiornataEventoCreazione & gMail.RitornaImmagineScontriDiretti & gMail.ChiudeTesto() & "<br />"
            TestoMail += gMail.ApreTabella
            Riga = ";Casa;;Fuori;Risultato Ufficiale;Risultato Reale;;Vincente;"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
        End If
        Do Until Rec.Eof
            Riga = ControllaRisultatiFraDueGiocPerSegni(Db, Rec, ConnSql, "ScontriDiretti", DatiGioco.Giornata, "GiocatoreCasa", "GiocatoreFuori")
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga, False)

            Rec.MoveNext()
        Loop
        Rec.Close()
        If TestoMail <> "" Then
            TestoMail += "</table> "
        End If
        gMail = Nothing

        Return TestoMail
    End Function

    Public Function ControllaRisultatiFraDueGiocPerSegni(Db As Object, Rec As Object, ConnSql As Object, NomeTabella As String, Giornata As Integer, NomeCampoGiocCasa As String, NomeCampoGiocFuori As String, Optional ModalitaPippettero As Boolean = False) As String
        Dim Ritorno As String = ""
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim PuntiCasa As Single
        Dim PuntiFuori As Single
        Dim RisReale As String
        Dim RisUfficiale As String
        Dim sCasa As String
        Dim sFuori As String
        Dim Vincitore As String
        Dim sVincitore As String
        Dim Cancellato1 As Boolean = False
        Dim Cancellato2 As Boolean = False

        Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(NomeCampoGiocCasa).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = False Then
            If Rec2("Cancellato").Value = "S" Then
                Cancellato1 = True
            End If
        End If
        Rec2.Close()

        Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(NomeCampoGiocFuori).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = False Then
            If Rec2("Cancellato").Value = "S" Then
                Cancellato2 = True
            End If
        End If
        Rec2.Close()

        Dim TappoCasa As Boolean = False
        Dim TappoFuori As Boolean = False

        If ModalitaPippettero = True Then
            ' Nel caso di pippettero vede se il giocatore ha giocato un tappo ed eventualmente moltiplica il risultato per 2
            Sql = "Select * From QuandoTappi Where " & _
                "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "Giornata=" & DatiGioco.Giornata & " And " & _
                "CodGiocatore=" & Rec(NomeCampoGiocCasa).Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = False Then
                TappoCasa = True
            End If
            Rec2.Close()

            Sql = "Select * From QuandoTappi Where " & _
                "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "Giornata=" & DatiGioco.Giornata & " And " & _
                "CodGiocatore=" & Rec(NomeCampoGiocFuori).Value
            Rec2 = Db.LeggeQuery(ConnSql, Sql)
            If Rec2.Eof = False Then
                TappoFuori = True
            End If
            Rec2.Close()
            ' Nel caso di pippettero vede se il giocatore ha giocato un tappo ed eventualmente moltiplica il risultato per 2
        End If

        Sql = "Select PuntiSegni From Risultati Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "Concorso=" & DatiGioco.Giornata & " And " & _
            "CodGiocatore=" & Rec(NomeCampoGiocCasa).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = True Then
            PuntiCasa = -1
        Else
            Dim n As String = Rec2(0).Value.ToString.Replace(",00", "")

            If n.IndexOf(".") > -1 Or n.IndexOf(",") > -1 Then
                Dim n2 As Integer

                If n.IndexOf(".") > -1 Then
                    n2 = Mid(n, 1, n.IndexOf("."))
                Else
                    n2 = Mid(n, 1, n.IndexOf(","))
                End If

                PuntiCasa = n2 + 1
            Else
                PuntiCasa = Rec2(0).Value
            End If
        End If
        Rec2.Close()

        If ModalitaPippettero = True Then
            If TappoCasa = True Then
                PuntiCasa *= 2
                If PuntiCasa > 14 Then
                    PuntiCasa = 14
                End If
            End If
            'PuntiCasa = 14 - PuntiCasa
        End If

        Sql = "Select PuntiSegni From Risultati Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "Concorso=" & DatiGioco.Giornata & " And " & _
            "CodGiocatore=" & Rec(NomeCampoGiocFuori).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = True Then
            PuntiFuori = -1
        Else
            Dim n As String = Rec2(0).Value.ToString.Replace(",00", "")

            If n.IndexOf(".") > -1 Or n.IndexOf(",") > -1 Then
                Dim n2 As Integer

                If n.IndexOf(".") > -1 Then
                    n2 = Mid(n, 1, n.IndexOf("."))
                Else
                    n2 = Mid(n, 1, n.IndexOf(","))
                End If

                PuntiFuori = n2 + 1
            Else
                PuntiFuori = Rec2(0).Value
            End If
        End If
        Rec2.Close()

        If ModalitaPippettero = True Then
            If TappoCasa = True Then
                PuntiFuori *= 2
                If PuntiFuori > 14 Then
                    PuntiFuori = 14
                End If
            End If
            'PuntiFuori = 14 - PuntiFuori
        End If

        If PuntiCasa <> -1 And PuntiFuori <> -1 Then
            RisReale = PuntiCasa.ToString.Trim & "-" & PuntiFuori.ToString.Trim
        Else
            If PuntiCasa = -1 Then
                RisReale = "X-" & PuntiFuori.ToString.Trim
            Else
                RisReale = PuntiCasa.ToString.Trim & "-X"
            End If
        End If

        PuntiCasa = PrendePuntiUfficialiPerSegni(PuntiCasa)
        PuntiFuori = PrendePuntiUfficialiPerSegni(PuntiFuori)

        If PuntiCasa <> -1 And PuntiFuori <> -1 Then
            RisUfficiale = PuntiCasa.ToString.Trim & "-" & PuntiFuori.ToString.Trim
        Else
            If PuntiCasa = -1 Then
                RisUfficiale = "X-" & PuntiFuori.ToString.Trim
            Else
                RisUfficiale = PuntiCasa.ToString.Trim & "-X"
            End If
        End If

        If PuntiCasa = -1 Or PuntiFuori = -1 Then
            Vincitore = "-1"
            If PuntiCasa = -1 Then
                If Cancellato1 = True Then
                    sVincitore = ""
                Else
                    If ModalitaPippettero = True Then
                        sVincitore = Rec("Casa").Value
                    Else
                        sVincitore = Rec("Fuori").Value
                    End If
                End If
            Else
                If Cancellato2 = True Then
                    sVincitore = ""
                Else
                    If ModalitaPippettero = True Then
                        sVincitore = Rec("Fuori").Value
                    Else
                        sVincitore = Rec("Casa").Value
                    End If
                End If
            End If
        Else
            If PuntiCasa > PuntiFuori Then
                If Cancellato2 = True Then
                    Vincitore = ""
                    sVincitore = ""
                Else
                    Vincitore = Rec(NomeCampoGiocCasa).Value
                    If Rec("Casa").Value Is DBNull.Value Then
                        sVincitore = "Morto"
                    Else
                        If ModalitaPippettero = True Then
                            sVincitore = Rec("Fuori").Value
                        Else
                            sVincitore = Rec("Casa").Value
                        End If
                    End If
                End If
            Else
                If PuntiCasa < PuntiFuori Then
                    If Cancellato1 = True Then
                        Vincitore = ""
                        sVincitore = ""
                    Else
                        Vincitore = Rec(NomeCampoGiocFuori).Value
                        If Rec("Fuori").Value Is DBNull.Value Then
                            sVincitore = "Morto"
                        Else
                            If ModalitaPippettero = True Then
                                sVincitore = Rec("Casa").Value
                            Else
                                sVincitore = Rec("Fuori").Value
                            End If
                        End If
                    End If
                Else
                    Vincitore = 0
                    sVincitore = "Pareggio"
                End If
            End If
        End If

        sCasa = "" & Rec("Casa").Value
        If sCasa = "" Or Cancellato1 = True Then
            sCasa = "Riposo"
        End If
        sFuori = "" & Rec("Fuori").Value
        If sFuori = "" Or Cancellato2 = True Then
            sFuori = "Riposo"
        End If

        Dim gMail As New GestioneMail

        Dim ImmC As String
        If Cancellato1 = True Then
            ImmC = "<img src='App_Themes/Standard/Images/Giocatori/Riposo.Jpg' width='30px' height='30px' />"
        Else
            ImmC = gMail.RitornaImmagineGiocatore("" & Rec("Casa").Value)
        End If

        Dim ImmF As String
        If Cancellato2 = True Then
            ImmC = "<img src='App_Themes/Standard/Images/Giocatori/Riposo.Jpg' width='30px' height='30px' />"
        Else
            ImmF = gMail.RitornaImmagineGiocatore("" & Rec("Fuori").Value)
        End If

        Dim ImmV As String = ""

        If NomeTabella.Trim.ToUpper <> "SCONTRIDIRETTI" Then
            If Giornata / 2 = Int(Giornata / 2) Then
                If Cancellato1 = True Or Cancellato2 = True Then
                    ImmV = ""
                Else
                    ImmV = gMail.RitornaImmagineGiocatore(sVincitore)
                End If
            Else
                sVincitore = ""
            End If
        Else
            If Cancellato1 = True Or Cancellato2 = True Then
                ImmV = ""
            Else
                ImmV = gMail.RitornaImmagineGiocatore(sVincitore)
            End If
        End If
        If sVincitore = "Pareggio" Or sCasa.Trim.ToUpper = "RIPOSO" Or sFuori.ToUpper.Trim = "RIPOSO" Then
            sVincitore = ""
            ImmV = ""
        End If

        gMail = Nothing

        Ritorno += ImmC & ";" & sCasa & ";" & ImmF & ";" & sFuori & ";" & RisUfficiale & ";" & RisReale & ";" & ImmV & ";" & sVincitore & ";"

        Sql = "Update " & PrefissoTabelle & NomeTabella & " Set " & _
            "RisultatoReale='" & RisReale & "', " & _
            "RisultatoUfficiale='" & RisUfficiale & "', " & _
            "Vincitore=" & Vincitore & " " & _
            "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata & " And Partita=" & Rec("Partita").Value
        Db.EsegueSql(ConnSql, Sql)

        Return Ritorno
    End Function

    Public Function ControllaRisultatiFraDueGiocPerRisultati(Db As Object, Rec As Object, ConnSql As Object, NomeTabella As String, Giornata As Integer, NomeCampoGiocCasa As String, NomeCampoGiocFuori As String) As String
        Dim Ritorno As String = ""
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim PuntiCasa As Single
        Dim PuntiFuori As Single
        Dim RisReale As String
        Dim RisUfficiale As String
        Dim sCasa As String
        Dim sFuori As String
        Dim Vincitore As String
        Dim sVincitore As String
        Dim Cancellato1 As Boolean = False
        Dim Cancellato2 As Boolean = False

        Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(NomeCampoGiocCasa).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = False Then
            If Rec2("Cancellato").Value = "S" Then
                Cancellato1 = True
            End If
        End If
        Rec2.Close()

        Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(NomeCampoGiocFuori).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = False Then
            If Rec2("Cancellato").Value = "S" Then
                Cancellato2 = True
            End If
        End If
        Rec2.Close()

        Sql = "Select PuntiRisultati From Risultati Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "Concorso=" & DatiGioco.Giornata & " And " & _
            "CodGiocatore=" & Rec(NomeCampoGiocCasa).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = True Then
            PuntiCasa = -1
        Else
            Dim n As String = Rec2(0).Value.ToString.Replace(",00", "")

            If n.IndexOf(".") > -1 Or n.IndexOf(",") > -1 Then
                Dim n2 As Integer

                If n.IndexOf(".") > -1 Then
                    n2 = Mid(n, 1, n.IndexOf("."))
                Else
                    n2 = Mid(n, 1, n.IndexOf(","))
                End If

                PuntiCasa = n2 + 1
            Else
                PuntiCasa = Rec2(0).Value
            End If
        End If
        Rec2.Close()

        Sql = "Select PuntiRisultati From Risultati Where " & _
            "Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "Concorso=" & DatiGioco.Giornata & " And " & _
            "CodGiocatore=" & Rec(NomeCampoGiocFuori).Value
        Rec2 = Db.LeggeQuery(ConnSql, Sql)
        If Rec2.Eof = True Then
            PuntiFuori = -1
        Else
            Dim n As String = Rec2(0).Value.ToString.Replace(",00", "")

            If n.IndexOf(".") > -1 Or n.IndexOf(",") > -1 Then
                Dim n2 As Integer

                If n.IndexOf(".") > -1 Then
                    n2 = Mid(n, 1, n.IndexOf("."))
                Else
                    n2 = Mid(n, 1, n.IndexOf(","))
                End If

                PuntiFuori = n2 + 1
            Else
                PuntiFuori = Rec2(0).Value
            End If
        End If
        Rec2.Close()

        If PuntiCasa <> -1 And PuntiFuori <> -1 Then
            RisReale = PuntiCasa.ToString.Trim & "-" & PuntiFuori.ToString.Trim
        Else
            If PuntiCasa = -1 Then
                RisReale = "X-" & PuntiFuori.ToString.Trim
            Else
                RisReale = PuntiCasa.ToString.Trim & "-X"
            End If
        End If

        PuntiCasa = PrendePuntiUfficialiPerRisultato(PuntiCasa)
        PuntiFuori = PrendePuntiUfficialiPerRisultato(PuntiFuori)

        If PuntiCasa <> -1 And PuntiFuori <> -1 Then
            RisUfficiale = PuntiCasa.ToString.Trim & "-" & PuntiFuori.ToString.Trim
        Else
            If PuntiCasa = -1 Then
                RisUfficiale = "X-" & PuntiFuori.ToString.Trim
            Else
                RisUfficiale = PuntiCasa.ToString.Trim & "-X"
            End If
        End If

        If PuntiCasa = -1 Or PuntiFuori = -1 Then
            Vincitore = "-1"
            If PuntiCasa = -1 Then
                If Cancellato1 = True Then
                    sVincitore = ""
                Else
                    sVincitore = Rec("Fuori").Value
                End If
            Else
                If Cancellato2 = True Then
                    sVincitore = ""
                Else
                    sVincitore = Rec("Casa").Value
                End If
            End If
        Else
            If PuntiCasa > PuntiFuori Then
                If Cancellato2 = True Then
                    Vincitore = ""
                    sVincitore = ""
                Else
                    Vincitore = Rec(NomeCampoGiocCasa).Value
                    sVincitore = Rec("Casa").Value
                End If
            Else
                If PuntiCasa < PuntiFuori Then
                    If Cancellato1 = True Then
                        Vincitore = ""
                        sVincitore = ""
                    Else
                        Vincitore = Rec(NomeCampoGiocFuori).Value
                        sVincitore = Rec("Fuori").Value
                    End If
                Else
                    Vincitore = 0
                    sVincitore = "Pareggio"
                End If
            End If
        End If

        sCasa = "" & Rec("Casa").Value
        If sCasa = "" Or Cancellato1 = True Then
            sCasa = "Riposo"
        End If
        sFuori = "" & Rec("Fuori").Value
        If sFuori = "" Or Cancellato2 = True Then
            sFuori = "Riposo"
        End If

        Dim gMail As New GestioneMail

        Dim ImmC As String
        If Cancellato1 = True Then
            ImmC = "<img src='App_Themes/Standard/Images/Giocatori/Riposo.Jpg' width='30px' height='30px' />"
        Else
            ImmC = gMail.RitornaImmagineGiocatore("" & Rec("Casa").Value)
        End If

        Dim ImmF As String
        If Cancellato2 = True Then
            ImmC = "<img src='App_Themes/Standard/Images/Giocatori/Riposo.Jpg' width='30px' height='30px' />"
        Else
            ImmF = gMail.RitornaImmagineGiocatore("" & Rec("Fuori").Value)
        End If

        Dim ImmV As String = ""

        If NomeTabella.Trim.ToUpper <> "SCONTRIDIRETTI" Then
            If Giornata / 2 = Int(Giornata / 2) Then
                If Cancellato1 = True Or Cancellato2 = True Then
                    ImmV = ""
                Else
                    ImmV = gMail.RitornaImmagineGiocatore(sVincitore)
                End If
            Else
                sVincitore = ""
            End If
        Else
            If Cancellato1 = True Or Cancellato2 = True Then
                ImmV = ""
            Else
                ImmV = gMail.RitornaImmagineGiocatore(sVincitore)
            End If
        End If
        If sVincitore = "Pareggio" Or sCasa.Trim.ToUpper = "RIPOSO" Or sFuori.ToUpper.Trim = "RIPOSO" Then
            sVincitore = ""
            ImmV = ""
        End If

        gMail = Nothing

        Ritorno += ImmC & ";" & sCasa & ";" & ImmF & ";" & sFuori & ";" & RisUfficiale & ";" & RisReale & ";" & ImmV & ";" & sVincitore & ";"

        Sql = "Update " & PrefissoTabelle & NomeTabella & " Set " & _
            "RisultatoReale='" & RisReale & "', " & _
            "RisultatoUfficiale='" & RisUfficiale & "', " & _
            "Vincitore=" & Vincitore & " " & _
            "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata & " And Partita=" & Rec("Partita").Value
        Db.EsegueSql(ConnSql, Sql)

        Return Ritorno
    End Function

    Public Function TornaMaxGiornateScontriDiretti() As Integer
        Dim Ritorno As Integer = 0
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Max(Giornata) From ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value <> True Then
                Ritorno = Rec(0).Value
            End If
            Rec.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Public Function TornaMinGiornateScontriDiretti() As Integer
        Dim Ritorno As Integer = 0
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Min(Giornata) From ScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value <> True Then
                Ritorno = Rec(0).Value
            End If
            Rec.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function
End Class
