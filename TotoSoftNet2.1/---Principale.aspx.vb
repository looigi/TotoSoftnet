Imports System.IO

Public Class Principale
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
                ControllaSchedinaGiocata()
            Else
                divControlloSchedina.Visible = False
            End If

            If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
                ControllaVincitori()
            Else
                divVincitoriPerdenti.Visible = False
            End If

            ControllaTappo()
            ControllaMessaggi()
            ControllaSondaggi()

            LeggeStatistiche()

            'If ModalitaLocale = True Then
            '    If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Or DatiGioco.StatoConcorso = ValoriStatoConcorso.Nessuno Then
            '        Button1.Visible = True
            '    Else
            '        Button1.Visible = False
            '    End If
            'Else
            Button1.Visible = False
            'End If
        End If

        AzionaCountdown()
    End Sub

    Private Sub AzionaCountdown()
        divCountdown.Visible = False

        If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
            If IsDate(DatiGioco.ChiusuraConcorso) = True Then
                divCountdown.Visible = True

                Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

                If DataArrivo > Now Then
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script type='text/javascript' language='javascript'>")
                    sb.Append("     AzionaCountdown(" & DataArrivo.Year & ", " & DataArrivo.Month - 1 & ", " & DataArrivo.Day & ", " & DataArrivo.Hour & ", " & DataArrivo.Minute & ", " & DataArrivo.Second & ");")
                    sb.Append("</script>")

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
                Else
                    divCountdown.Visible = False
                End If
            End If
        End If
    End Sub

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

            Sql = "Select Count(*) From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Quanti = 0
            Else
                Quanti = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select idGioc, Sum(PuntiTot) From AppoClass " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata<=" & DatiGioco.Giornata & " " &
                "Group By idGioc " &
                "Order By 2 Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Posizione += 1
                If Rec("idGioc").Value = Session("CodGiocatore") Then
                    Exit Do
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            lblPosTot.Text = Posizione & "/" & Quanti

            Posizione = 0
            Sql = "Select idGioc, Sum(PuntiRis) From AppoClass " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata<=" & DatiGioco.Giornata & " " &
                "Group By idGioc " &
                "Order By 2 Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Posizione += 1
                If Rec("idGioc").Value = Session("CodGiocatore") Then
                    Exit Do
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            lblPosRis.Text = Posizione & "/" & Quanti

            Dim Rifai As Boolean = False

            Posizione = 0
            Sql = "Select AllaGiornata From AppoScontriDiretti " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocVisua=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                If Rec(0).Value < DatiGioco.Giornata Then
                    Rifai = True
                End If
            Else
                Rifai = True
            End If
            Rec.Close()

            If Rifai = True Then
                Dim sDiretti As New ScontriDiretti
                sDiretti.CreaTabellaClassificaScontriDiretti(DatiGioco.Giornata, Session("CodGiocatore"))
                sDiretti = Nothing
            End If

            Sql = "Select * From AppoScontriDiretti Where Anno=" & DatiGioco.AnnoAttuale & "  " &
                "And CodGiocVisua=" & Session("CodGiocatore") & " " &
                "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Posizione += 1
                If Rec("CodGiocatore").Value = Session("CodGiocatore") Then
                    Exit Do
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()
            If Posizione = 0 Then
                lblPosCamp.Visible = False
                lblDesc3.Visible = False
                barra.Visible = False
                'liSpazioCamp.Visible = False
            Else
                lblPosCamp.Text = Posizione & "/" & Quanti
            End If

            Dim Champions As Boolean = False
            Dim ChampionsTurni As Boolean = False
            Dim EuropaLeague As Boolean = False
            Dim InterToto As Boolean = False
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

            ulSuddenDeath.Visible = SuddenDeath
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
                        Testo += "<hr />" & Rec("SquadraCasa").Value & "-" & Rec("SquadraFuori").Value
                        Testo += "<hr />Partita da giocare"
                        'ulSuddenDeath.Visible = False
                    Else
                        Testo += "<hr />" & Rec("SquadraCasa").Value & "-" & Rec("SquadraFuori").Value & " " & Rec("Risultato").Value & "<hr />Punti ottenuti: " & Rec("Punti").Value
                    End If
                    lblDescSD.Text = "Ultima partita Sudden Death"
                    Rec.Close()

                    Sql = "Select * From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        Testo = "Uscito dal torneo alla giornata " & Rec("Giornata").Value
                    End If

                    lblSD.Text = Testo
                Else
                    ulSuddenDeath.Visible = False
                End If
                Rec.Close()
            End If

            ulDerelitti.Visible = Derelitti
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
                    lblDescDer.Text = "Ultima partita Pippettero"
                    lblDer.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    ulDerelitti.Visible = False
                End If
                Rec.Close()
            End If

            ulCoppaItalia.Visible = CoppaItalia
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

                    lblDescCI.Text = "Ultima partita C. Italia"
                    lblCI.Text = Casa & "-" & Fuori & " " & Risu
                    Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteCoppaItaliaTurni", Rec("Giornata").Value)
                    lblTurnoCI.Text = Turno
                Else
                    ulCoppaItalia.Visible = False
                End If
                Rec.Close()
            End If

            ulIntertoto.Visible = InterToto
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
                    lblDescIT.Text = "Ultima partita Intertoto"
                    lblIT.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    ulIntertoto.Visible = False
                End If
                Rec.Close()
            End If

            ulEuropaleague.Visible = EuropaLeague
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
                    lblDescEL.Text = "Ultima partita Europa League"
                    lblEL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                Else
                    ulEuropaleague.Visible = False
                End If
                Rec.Close()
            Else
                ulEuropaleague.Visible = EuropaLeagueTurni
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
                        lblDescEL.Text = "Ultima partita turni Europa League"
                        lblEL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                        Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteEuropaLeagueTurni", Rec("Giornata").Value)
                        lblTurnoEL.Text += Turno
                    Else
                        ulEuropaleague.Visible = False
                    End If
                    Rec.Close()
                End If
            End If

            ulChampions.Visible = Champions
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
                    lblDescCL.Text = "Ultima partita Champions girone A"
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
                        lblDescCL.Text = "Ultima partita Champions girone B"
                        lblCL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                    Else
                        ulChampions.Visible = False
                    End If
                    Rec.Close()
                End If
            Else
                ulChampions.Visible = ChampionsTurni
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
                        lblDescCL.Text = "Ultima partita turni Champion's League"
                        lblCL.Text = Rec("Casa").Value.ToString.ToUpper & "-" & Rec("Fuori").Value.ToString.ToUpper & " " & Rec("RisultatoUfficiale").Value
                        Turno = RitornaTurnoCoppe(Db, ConnSQL, "PartiteChampionsTurni", Rec("Giornata").Value)
                        lblTurnoCL.Text += Turno
                    Else
                        ulChampions.Visible = False
                    End If
                    Rec.Close()
                End If
            End If

            ' Riconoscimenti / Disonori
            Dim Conta As Integer = 0
            Dim Conta2 As Integer = 0
            Dim CiSonoRD As Boolean = False

            Sql = "Select Giornata, B.Immagine, B.Punti, B.Descrizione From RiconDisonGioc A " &
                "Left Join RiconDison B On A.idPremio=B.idPremio " &
                "Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGioc=" & Session("CodGiocatore") & " Order By Giornata"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            ltRicDis.Text = ""
            Do Until Rec.Eof
                ltRicDis.Text += "<img id='ImageRD" & Conta2 & "' src='App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value & "' alt='' height='26px' width='26px' title='Giornata " & Rec("Giornata").Value & " - " & Rec("Descrizione").Value & ": " & Rec("Punti").Value & "' />&nbsp;"
                Conta += 1
                If Conta = 6 Then
                    Conta = 0
                    ltRicDis.Text += "<br />"
                End If
                Conta2 += 1
                CiSonoRD = True

                Rec.MoveNext()
            Loop
            Rec.Close()

            If CiSonoRD = True Then
                BachecaTrofei.Visible = True
            Else
                BachecaTrofei.Visible = False
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaVincitori()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Giocatore As New DataColumn("Giocatore")
            Dim PuntiTot As New DataColumn("PuntiTot")
            Dim PuntiSegni As New DataColumn("PuntiSegni")
            Dim PuntiRis As New DataColumn("PuntiRis")
            Dim PuntiJolly As New DataColumn("PuntiJolly")
            Dim PuntiQuote As New DataColumn("PuntiQuote")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Giocatore)
            dttTabella.Columns.Add(PuntiTot)
            dttTabella.Columns.Add(PuntiSegni)
            dttTabella.Columns.Add(PuntiRis)
            dttTabella.Columns.Add(PuntiJolly)
            dttTabella.Columns.Add(PuntiQuote)

            ' Primi
            Sql = "Select A.*, B.Giocatore, C.PuntiSegni, C.PuntiRisultati, C.PuntiJolly, C.PuntiQuote, (C.PuntiSegni + C.PuntiRisultati + C.PuntiJolly + C.PuntiQuote) As Totale " &
                "From Primi A " &
                "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " &
                "Left Join Risultati C On A.CodGiocatore=C.CodGiocatore And A.Anno=C.Anno And A.Giornata = C.Concorso " &
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & DatiGioco.Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value
                riga(1) = Rec("Totale").Value
                riga(2) = Rec("PuntiSegni").Value
                riga(3) = Rec("PuntiRisultati").Value
                riga(4) = Rec("PuntiJolly").Value
                riga(5) = Rec("PuntiQuote").Value
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdVincitori.DataSource = dttTabella
            grdVincitori.DataBind()

            ' Secondi
            dttTabella.Clear()

            Sql = "Select A.*, B.Giocatore, C.PuntiSegni, C.PuntiRisultati, C.PuntiJolly, C.PuntiQuote, (C.PuntiSegni + C.PuntiRisultati + C.PuntiJolly + C.PuntiQuote) As Totale " &
                "From Secondi A " &
                "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " &
                "Left Join Risultati C On A.CodGiocatore=C.CodGiocatore And A.Anno=C.Anno And A.Giornata = C.Concorso " &
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & DatiGioco.Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value
                riga(1) = Rec("Totale").Value
                riga(2) = Rec("PuntiSegni").Value
                riga(3) = Rec("PuntiRisultati").Value
                riga(4) = Rec("PuntiJolly").Value
                riga(5) = Rec("PuntiQuote").Value
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdSecondi.DataSource = dttTabella
            grdSecondi.DataBind()

            ' Ultimi
            dttTabella.Clear()

            Sql = "Select A.*, B.Giocatore, C.PuntiSegni, C.PuntiRisultati, C.PuntiJolly, C.PuntiQuote, (C.PuntiSegni + C.PuntiRisultati + C.PuntiJolly + C.PuntiQuote) As Totale " &
                "From Ultimi A " &
                "Left Join Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " &
                "Left Join Risultati C On A.CodGiocatore=C.CodGiocatore And A.Anno=C.Anno And A.Giornata = C.Concorso " &
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & DatiGioco.Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value
                riga(1) = Rec("Totale").Value
                riga(2) = Rec("PuntiSegni").Value
                riga(3) = Rec("PuntiRisultati").Value
                riga(4) = Rec("PuntiJolly").Value
                riga(5) = Rec("PuntiQuote").Value
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdUltimi.DataSource = dttTabella
            grdUltimi.DataBind()
        End If
    End Sub

    Private Sub grdVincitori_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdVincitori.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(6).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(1).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            Dim g As New Giocatori

            g = Nothing

            e.Row.Cells(6).Visible = False
        End If
    End Sub

    Private Sub grdSecondi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdSecondi.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(6).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(1).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            Dim g As New Giocatori

            g = Nothing

            e.Row.Cells(6).Visible = False
        End If
    End Sub

    Private Sub grdUltimi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUltimi.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(6).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(1).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            Dim g As New Giocatori

            g = Nothing

            e.Row.Cells(6).Visible = False
        End If
    End Sub

    Private Sub ControllaSchedinaGiocata()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Icona As String
            Dim Testo As String

            Sql = "Select * From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore") & " And Giornata=" & DatiGioco.Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Icona = "App_Themes/Standard/Images/Icone/PolliceGiu.png"
                Testo = "Colonna propria ancora da compilare"
            Else
                Icona = "App_Themes/Standard/Images/Icone/PolliceSu.png"
                Testo = "Colonna propria già compilata"
            End If
            Rec.Close()

            imgSchedina1.ImageUrl = Icona
            imgSchedina2.ImageUrl = Icona
            lblSchedina.Text = Testo

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaTappo()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Icona As String
            Dim Testo As String

            Sql = "Select * From Tappi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Icona = "App_Themes/Standard/Images/Icone/PolliceGiu.png"
                Testo = "Colonna tappo ancora da compilare"
            Else
                Icona = ""
                Testo = ""
            End If
            Rec.Close()

            If Testo <> "" Then
                imgTappo1.ImageUrl = Icona
                imgTappo2.ImageUrl = Icona
                lblTappo.Text = Testo

                divControlloTappo.Visible = True
            Else
                divControlloTappo.Visible = False
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaMessaggi()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Icona As String
            Dim Testo As String

            Sql = "Select Count(*) From Messaggi " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And idDestinatario=" & Session("CodGiocatore") & " " &
                "And Letto='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Icona = ""
                Testo = ""
            Else
                If Rec(0).Value > 0 Then
                    Icona = "App_Themes/Standard/Images/Icone/Messaggi.png"
                    Testo = "Messaggi in attesa di lettura: " & Rec(0).Value
                Else
                    Icona = ""
                    Testo = ""
                End If
            End If
            Rec.Close()

            If Testo <> "" Then
                imgMessaggi1.ImageUrl = Icona
                imgMessaggi2.ImageUrl = Icona
                lblMessaggi.Text = Testo

                divMessaggi.Visible = True
            Else
                divMessaggi.Visible = False
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ControllaSondaggi()
        Dim Db As New GestioneDB

        divSondaggi.Visible = False
        hdnIdSondaggio.Value = ""
        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
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
                    idRispostaSondaggio.Visible = True
                    Select Case Rec("Risposta").Value
                        Case "1"
                            lblRisposta.Text = cmdRisposta1.Text
                        Case "2"
                            lblRisposta.Text = cmdRisposta2.Text
                        Case "3"
                            lblRisposta.Text = cmdRisposta3.Text
                    End Select
                    lblRisposta.Text = "Hai risposto '" & lblRisposta.Text & "' in data " & Rec("DataRisposta").Value

                    CreaImmagineSondaggio(Db, ConnSQL, idSondaggio, Anno)
                Else
                    idTastiSondaggio.Visible = True
                    idRispostaSondaggio.Visible = False
                End If
                Rec.Close()
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Update DatiDiGioco Set Giornata=Giornata+1, Stato=1, PartitaJolly = 7"
            Db.EsegueSql(ConnSQL, Sql)

            Response.Redirect("Default.aspx")

            Dim Messaggi() As String = {"Fatto..."}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta1_Click(sender As Object, e As EventArgs) Handles cmdRisposta1.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" &
                " " & hdnIdSondaggio.Value & ", " &
                " " & Session("CodGiocatore") & ", " &
                "1, " &
                "'" & ConverteData(Now) & "' " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ConnSQL.Close()

            ControllaSondaggi()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta2_Click(sender As Object, e As EventArgs) Handles cmdRisposta2.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" &
                " " & hdnIdSondaggio.Value & ", " &
                " " & Session("CodGiocatore") & ", " &
                "2, " &
                "'" & ConverteData(Now) & "' " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ConnSQL.Close()

            ControllaSondaggi()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdRisposta3_Click(sender As Object, e As EventArgs) Handles cmdRisposta3.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into SondaggiRisposte Values (" &
                " " & hdnIdSondaggio.Value & ", " &
                " " & Session("CodGiocatore") & ", " &
                "3, " &
                "'" & ConverteData(Now) & "' " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Messaggi() As String = {"Votato", "Grazie mille"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            ConnSQL.Close()

            ControllaSondaggi()
        End If

        Db = Nothing
    End Sub

End Class