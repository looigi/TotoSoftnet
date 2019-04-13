Imports System.Data

Public Class clsCassa
    Public Sub CaricaDati(grdBilancio As GridView, grdvittorie As GridView, Permesso As Integer, codGiocatore As Integer)
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Altro As String = ""

        Select Case Permesso
            Case Permessi.Amministratore
            Case Permessi.Giocatore
                Altro = "And A.CodGiocatore=" & codGiocatore
            Case Permessi.Cassiere
            Case Permessi.Aggiornatore
                Altro = "And A.CodGiocatore=" & codGiocatore
        End Select

        ' , ((Reali+Vinti)-Presi)-TotVersamento As Totale
        Sql = "Select B.Giocatore, A.TotVersamento, A.Reali, A.Vinti, A.Presi, ((Reali+Vinti)-Presi)-TotVersamento As Bilancio, Amici " & _
            "From Bilancio A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " & Altro & " And Cancellato='N' " & _
            "And B.Pagante='S' " & _
            "Order By ((Reali+Vinti)-Presi)-TotVersamento, Giocatore"
        Gr.ImpostaCampi(Sql, grdBilancio)
        Gr = Nothing

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim c As New clsCassa

            c.ControllaVincite(Db, ConnSQL, grdvittorie)

            c = Nothing
        End If

        Db = Nothing
    End Sub

    Public Sub ControllaVincite(Db As GestioneDB, ConnSql As Object, grdVittorie As GridView)
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")

        Sql = "Select (Sum(TotVersamento)-Sum(Vinti))-Sum(Presi) As PremioFinaleVirtuale From Bilancio A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Dim PremioFinaleVirtuale As Single = Rec("PremioFinaleVirtuale").Value
        Rec.Close()

        Sql = "Select Count(*) From  Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato<>'S' And Pagante='S'"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Dim PremioFinaleSpeciale As Single
        If Rec(0).Value Is DBNull.Value = True Then
            PremioFinaleSpeciale = 0
        Else
            PremioFinaleSpeciale = Rec(0).Value
            Rec.Close()

            Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SPECIALI'"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            PremioFinaleSpeciale *= Rec("Perc").Value
        End If
        Rec.Close()

        If PremioFinaleSpeciale < PremioFinaleVirtuale Then
            PremioFinaleVirtuale -= PremioFinaleSpeciale
        Else
            PremioFinaleSpeciale = 0
        End If

        Dim Tipologia As New DataColumn("Tipologia")
        Dim TotaleV As New DataColumn("TotDareV")
        Dim Vincitore As New DataColumn("Vincitore")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        dttTabella.Columns.Add(Tipologia)
        dttTabella.Columns.Add(TotaleV)
        dttTabella.Columns.Add(Vincitore)

        riga = dttTabella.NewRow()
        riga(0) = "Saldo attuale"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        riga = dttTabella.NewRow()
        riga(0) = ""
        riga(1) = ""
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        ' Campionato
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='CAMPIONATO'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Campionato"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Risultati
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='RISULTATI'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Class. Risultati"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Coppa Italia
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='CITALIA'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Coppa Italia"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Champion's
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='CLEAGUE'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Champion's League"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Europa League
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='ELEAGUE'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Europa League"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Pippettero
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='DERELITTI'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Pippettero"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Sudden Death
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SUDDEN DEATH'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Sudden Death"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Supercoppa Italiana
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SUPERCOPPA ITALIANA'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Supercoppa Italiana"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Supercoppa Europea
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SUPERCOPPA EUROPEA'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Supercoppa Europea"
        riga(1) = ScriveNumeroFormattato(PremioFinaleVirtuale * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Calcolo percentuale sul premio finale
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='PREMIO FINALE'"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        Dim PercPremioFinale As Integer = Rec("Perc").Value
        Dim PremioFinalePV As Single = PremioFinaleVirtuale * PercPremioFinale / 100
        Rec.Close()

        ' Primo
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='PRIMOPOSTO'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Primo"
        riga(1) = ScriveNumeroFormattato(PremioFinalePV * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Secondo
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SECONDOPOSTO'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Secondo"
        riga(1) = ScriveNumeroFormattato(PremioFinalePV * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Terzo
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='TERZOPOSTO'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Terzo"
        riga(1) = ScriveNumeroFormattato(PremioFinalePV * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        ' Speciali
        riga = dttTabella.NewRow()
        riga(0) = "Speciali"
        riga(1) = ScriveNumeroFormattato(PremioFinaleSpeciale)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        ' QuoteCUP
        Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='QUOTECUP'"
        Rec = Db.LeggeQuery(ConnSql, Sql)

        riga = dttTabella.NewRow()
        riga(0) = "Quote CUP"
        riga(1) = ScriveNumeroFormattato(PremioFinalePV * Rec("Perc").Value / 100)
        riga(2) = ""
        dttTabella.Rows.Add(riga)

        Rec.Close()

        grdVittorie.DataSource = dttTabella
        grdVittorie.DataBind()
    End Sub

    Public Sub CaricamentoRigaVittorie(sender As Object, e As GridViewRowEventArgs, Percorso As String, grdBilancio As GridView, AggiornaBilancio As Boolean)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Tipologia As String = e.Row.Cells(1).Text
            Dim img As Image = DirectCast(e.Row.FindControl("imgTipologia"), Image)
            Dim imgV As Image = DirectCast(e.Row.FindControl("imgAvatarV"), Image)
            Dim Path1 As String = ""
            Dim Path2 As String = ""
            Dim Sql As String
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Nome As String = ""

                Select Case Tipologia.ToUpper.Trim
                    Case "SALDO ATTUALE"
                        Path1 = "App_themes/Standard/Images/Icone/Incasso.png"
                    Case "CAMPIONATO"
                        Path1 = "App_themes/Standard/Images/Icone/Campionato.png"

                        Sql = "Select Top 1 * From AppoScontriDiretti A " & _
                            "LEFT JOIN Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' And B.Cancellato='N' " & _
                            "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "CLASS. RISULTATI"
                        Path1 = "App_themes/Standard/Images/Icone/Risultati.png"

                        Dim sc As New StringheClassifiche

                        Sql = sc.RitornaStringaClassificaGenerale
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "COPPA ITALIA"
                        Path1 = "App_themes/Standard/Images/Icone/CoppaItalia.png"

                        Sql = "Select A.GiocCasa, B.Giocatore From PartiteCoppaItaliaTurni A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "CHAMPION'S LEAGUE"
                        Path1 = "App_themes/Standard/Images/Icone/Champions.png"

                        Sql = "Select A.GiocCasa, B.Giocatore From PartiteChampionsTurni A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "EUROPA LEAGUE"
                        Path1 = "App_themes/Standard/Images/Icone/EuropaLeague.png"

                        Sql = "Select A.GiocCasa, B.Giocatore From PartiteEuropaLeagueTurni A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "QUOTE CUP"
                        Path1 = "App_themes/Standard/Images/Icone/quotecup.png"

                        Sql = "Select A.CodGiocatore, B.Giocatore From QuoteCUP_Vincenti A " & _
                            "LEFT JOIN Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "PIPPETTERO"
                        Path1 = "App_themes/Standard/Images/Icone/Pippettero.png"

                        Sql = "Select B.Giocatore From DerelittiSquadre A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                            "Where A.CodGiocatore Not In " & _
                            "(Select CodGiocatore From AppoDerelitti) And A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' And B.Cancellato='N'"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        Else
                            Sql = "Select Top 1 * From AppoDerelitti A " & _
                                "LEFT JOIN Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' And B.Cancellato='N' " & _
                                "Order By Punti, GFatti, GSubiti Desc, Differenza"
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                e.Row.Cells(3).Text = Rec("Giocatore").Value
                                Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                                Nome = Rec("Giocatore").Value
                            End If
                        End If
                        Rec.Close()
                    Case "SUPERCOPPA ITALIANA"
                        Path1 = "App_themes/Standard/Images/Icone/SuperCoppaItaliana.png"

                        Sql = "Select A.GiocCasa, B.Giocatore From PartiteSuperCoppaItalianaTurni A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "SUPERCOPPA EUROPEA"
                        Path1 = "App_themes/Standard/Images/Icone/SuperCoppaEuropea.png"

                        Sql = "Select A.GiocCasa, B.Giocatore From PartiteSuperCoppaEuropeaTurni A " & _
                            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                    Case "SUDDEN DEATH"
                        Path1 = "App_themes/Standard/Images/Icone/SuddenDeath.png"

                        Sql = "Select A.*, B.Giocatore From SuddenDeathVinc A " & _
                                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Pagante='S' And B.Cancellato='N'"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)
                        Else
                            Rec.Close()

                            Sql = "Select B.Giocatore, Sum(Punti) As Punti From SuddenDeathPunti A " & _
                                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And B.CodGiocatore Not In (Select CodGiocatore From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & ") " & _
                                "And B.Pagante='S' And B.Cancellato='N' " & _
                                "Group By B.CodGiocatore, B.Giocatore " & _
                                "Order By 2 Desc"
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                Dim Chi As String = ""
                                Dim PuntiMax As Integer = 0

                                Do Until Rec.Eof
                                    If Rec("Punti").Value >= PuntiMax Then
                                        PuntiMax = Rec("Punti").value
                                        Chi += Rec("Giocatore").Value & ";"
                                    Else
                                        Exit Do
                                    End If

                                    Rec.MoveNext()
                                Loop

                                Dim Primi() As String = Chi.Split(";")
                                Dim sc As New StringheClassifiche
                                Dim Massimo As Single = 0
                                Dim g As New Giocatori

                                For i As Integer = 0 To Primi.Length - 1
                                    If Primi(i) <> "" Then
                                        Sql = sc.RitornaStringaClassificaGeneralePerGiocatore(g.TornaIdGiocatore(Primi(i)))
                                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                                        If Rec("TotPunti").Value > Massimo Then
                                            Massimo = Rec("TotPunti").Value
                                            Chi = Primi(i)
                                        End If
                                        Rec.Close()
                                    End If
                                Next

                                e.Row.Cells(3).Text = Chi
                                Path2 = RitornaImmagine(Percorso, Chi)

                                Nome += Chi & ";"
                            End If
                        End If
                    Case "PRIMO", "SECONDO", "TERZO"
                        Path1 = "App_themes/Standard/Images/Icone/Generale.png"

                        Dim sc As New StringheClassifiche

                        Sql = sc.RitornaStringaClassificaGeneralePerCassa

                        Dim Quale As Integer = 0
                        Dim Quanti As Integer = 0
                        Dim TotQuale As Integer

                        Select Case Tipologia.ToUpper.Trim
                            Case "PRIMO"
                                TotQuale = 1
                            Case "SECONDO"
                                TotQuale = 2
                            Case "TERZO"
                                TotQuale = 3
                        End Select

                        Dim Vecchio As Integer = -1
                        Dim Giocatori As String = ""

                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        Do Until Rec.Eof
                            If Rec("TotPunti").Value <> Vecchio Then
                                Quale += 1
                                If Quale > TotQuale Then
                                    Exit Do
                                End If
                                Quanti = 0
                                Giocatori = ""
                            End If

                            Giocatori += Rec("Giocatore").Value & "<br />"
                            Quanti += 1

                            Rec.MoveNext()
                        Loop
                        Rec.Close()

                        If Giocatori <> "" Then
                            Giocatori = Mid(Giocatori, 1, Giocatori.Length - 6)

                            e.Row.Cells(3).Text = Giocatori
                            If Quanti = 1 Then
                                Path2 = RitornaImmagine(Percorso, Giocatori)
                            End If

                            Nome = Giocatori
                        End If
                    Case "SPECIALI"
                        Path1 = "App_themes/Standard/Images/Icone/Speciali.png"

                        'Sql = "Select A.CodGiocatore, B.Giocatore From ClassificaSpeciale A " &
                        '    "Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                        '    "Where A.idAnno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' Order By PuntiTot Desc"

                        Sql = "Select A.CodGiocatore, B.Giocatore, Sum(PuntiTot) From ClassificaSpeciale A  "
                        Sql &= "Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore=B.CodGiocatore "
                        Sql &= "Where A.idAnno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And B.Pagante='S' "
                        Sql &= "Group By A.CodGiocatore, B.Giocatore "
                        Sql &= "Order By 3 Desc"

                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        If Rec.Eof = False Then
                            e.Row.Cells(3).Text = Rec("Giocatore").Value
                            Path2 = RitornaImmagine(Percorso, Rec("Giocatore").Value)

                            Nome = Rec("Giocatore").Value
                        End If
                        Rec.Close()
                End Select

                If Path1 <> "" Then
                    img.ImageUrl = Path1
                Else
                    img.Visible = False
                End If

                If Path2 <> "" Then
                    imgV.ImageUrl = Path2
                Else
                    imgV.Visible = False
                End If

                '' Aggiorna totalone nella maschera di bilancio
                'If AggiornaBilancio = True Then
                '    If Nome <> "" Then
                '        Dim Importo As Single = 0

                '        If e.Row.Cells(2).Text.IndexOf("-") = -1 Then
                '            Importo = e.Row.Cells(2).Text.ToString.Replace("&nbsp;", "").Replace("€", "").Trim
                '        End If

                '        Dim Chi As String
                '        Dim Importone As Single

                '        For i As Integer = 0 To grdBilancio.Rows.Count - 1
                '                Chi = grdBilancio.Rows(i).Cells(1).Text
                '                If Chi = Nome Then
                '                    Importone = grdBilancio.Rows(i).Cells(8).Text.ToString.Replace("&nbsp;", "").Replace("€", "").Trim + Importo
                '                    grdBilancio.Rows(i).Cells(8).Text = Importone
                '                    Exit For
                '                End If
                '            Next
                '        End If
                '        ' Aggiorna totalone nella maschera di bilancio
                '    End If
            End If
        End If
    End Sub

    Public Sub CaricamentoRigaBilancio(sender As Object, e As GridViewRowEventArgs, Percorso As String, Permesso As Integer, Optional AltriTasti As Boolean = True)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(1).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Percorso, NomeGiocatore)
            ImgGioc.DataBind()

            Dim Bilancio As Single = e.Row.Cells(6).Text
            Dim Colore As System.Drawing.Color

            If Bilancio < 0 Then
                Colore = Drawing.Color.DarkRed
            Else
                If Bilancio = 0 Then
                    Colore = Drawing.Color.Blue
                Else
                    Colore = Drawing.Color.DarkGreen
                End If
            End If

            For i As Integer = 0 To 8
                e.Row.Cells(i).ForeColor = Colore
            Next

            If AltriTasti = True Then
                Dim imgPaga As ImageButton = DirectCast(e.Row.FindControl("imgPaga"), ImageButton)
                Dim imgIncassa As ImageButton = DirectCast(e.Row.FindControl("imgIncassa"), ImageButton)
                Dim ImgMod As Image = DirectCast(e.Row.FindControl("imgModifica"), Image)

                imgPaga.Visible = False
                imgIncassa.Visible = False
                ImgMod.Visible = False

                Select Case Permesso
                    Case Permessi.Amministratore
                        imgPaga.Visible = True
                        imgIncassa.Visible = True
                        ImgMod.Visible = True
                    Case Permessi.Cassiere
                        imgPaga.Visible = True
                        imgIncassa.Visible = True
                        ImgMod.Visible = True
                End Select

                Dim sBilancio As Single = e.Row.Cells(6).Text.Replace(".", ",")

                If sBilancio <= 0 Then
                    imgIncassa.Visible = False
                End If
            End If
        End If
    End Sub
End Class
