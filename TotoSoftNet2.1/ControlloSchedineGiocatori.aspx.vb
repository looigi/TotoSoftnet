Imports System.IO
Imports System.Net

Public Class ControlloSchedineGiocatori
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divRisultati.Visible = False

            Dim Db As New GestioneDB
            Dim Speciale As Boolean

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    lblSpeciale.Visible = True
                    Speciale = True
                Else
                    lblSpeciale.Visible = False
                    Speciale = False
                End If

                ConnSQL.Close()
            End If

            Db = Nothing

            CaricaPartite(Speciale)

            'cmdAggiorna.Visible = False
            divRisultati.Visible = False
            divVincitori.Visible = False
            divUltimoPosto.Visible = False
            'cmdRiepilogo.Visible = False
            cmdMostraTutti.Visible = False
            'cmdAvvisa.Visible = False
        End If
    End Sub

    Private Sub CaricaPartite(Speciale As Boolean)
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim g As Integer

        If Speciale = True Then
            g = DatiGioco.GiornataSpeciale + 100
        Else
            g = DatiGioco.Giornata
        End If

        Sql = "Select A.Partita From Schedine A " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
            "A.Giornata=" & g
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ' e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As Label = DirectCast(e.Row.FindControl("TxtCasa"), Label)
            Dim TxtFuori As Label = DirectCast(e.Row.FindControl("TxtFuori"), Label)
            Dim TxtRis As Label = DirectCast(e.Row.FindControl("TxtRisultato"), Label)
            Dim TxtSegno As Label = DirectCast(e.Row.FindControl("TxtSegno"), Label)
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim ImgJolly As Image = DirectCast(e.Row.FindControl("imgJolly"), Image)
            Dim numRiga As Integer = e.Row.Cells(0).Text
            Dim lblSerieC As Label = DirectCast(e.Row.FindControl("lblSerieC"), Label)
            Dim lblSerieF As Label = DirectCast(e.Row.FindControl("lblSerieF"), Label)
            Dim Segno As String = ""
            Dim Db As New GestioneDB
            Dim gM As New GestioneMail

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim g As Integer

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    g = DatiGioco.GiornataSpeciale + 100
                Else
                    g = DatiGioco.Giornata
                End If

                Sql = "Select A.* From Schedine A " &
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
                    "A.Giornata=" & g & " And " &
                    "A.Partita=" & numRiga
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    TxtCasa.Text = Rec("SquadraCasa").Value
                    TxtFuori.Text = Rec("SquadraFuori").Value
                    TxtRis.Text = "" & Rec("Risultato").Value
                    TxtSegno.Text = "" & Rec("Segno").Value
                Else
                    TxtCasa.Text = ""
                    TxtFuori.Text = ""
                    TxtRis.Text = ""
                    TxtSegno.Text = ""
                End If
                Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " &
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtCasa.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieC.Text = Rec("Descrizione").Value
                Else
                    lblSerieC.Text = ""
                End If
                Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " &
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtFuori.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieF.Text = Rec("Descrizione").Value
                Else
                    lblSerieF.Text = ""
                End If
                Rec.Close()

                Dim Path As String

                Path = gM.RitornaImmagineSquadra(TxtCasa.Text, True)
                Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
                'Path = "App_Themes/Standard/Images/Stemmi/" & TxtCasa.Text & ".jpg"
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgCasa.ImageUrl = Path
                Else
                    ImgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = gM.RitornaImmagineSquadra(TxtFuori.Text, True)
                Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
                'Path = "App_Themes/Standard/Images/Stemmi/" & TxtFuori.Text & ".jpg"
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgFuori.ImageUrl = Path
                Else
                    ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = "App_Themes/Standard/Images/Icone/Jolly.png"

                If numRiga = DatiGioco.PartitaJolly Then
                    ImgJolly.ImageUrl = Path
                    ImgJolly.Visible = True
                Else
                    ImgJolly.ImageUrl = ""
                    ImgJolly.Visible = False
                End If

                ConnSQL.Close()
            End If

            Db = Nothing

            ' e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Function RitornaValoreQuota(Db As GestioneDB, ConnSql As Object, NomeCampo As String, Partita As Integer) As Single
        Dim Ritorno As Single
        Dim Sql As String
        Dim RecQ As Object = CreateObject("ADODB.Recordset")
        Dim g As Integer

        If ControllaConcorsoSpeciale(Db, ConnSql) = True Then
            g = DatiGioco.GiornataSpeciale + 100
        Else
            g = DatiGioco.Giornata
        End If

        Sql = "Select " & NomeCampo & " From Quote Where " &
            "Anno=" & DatiGioco.AnnoAttuale & " And " &
            "Giornata=" & g & " And " &
            "Partita=" & Partita
        RecQ = Db.LeggeQuery(ConnSql, Sql)
        If RecQ.Eof = False Then
            Ritorno = RecQ(0).Value
        Else
            Ritorno = 0.1
        End If
        RecQ.Close()

        Return Ritorno
    End Function

    Protected Sub cmdControlla_Click(sender As Object, e As EventArgs) Handles cmdControlla.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim RecG As Object = CreateObject("ADODB.Recordset")
            Dim RecI As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giocatori() As String = {}
            Dim PuntiDifferenza() As Single = {}
            Dim PuntiSomma() As Single = {}
            Dim QuantiGioc As Integer = 0
            Dim PuntiSegni() As Single = {}
            Dim PuntiFR() As Single = {}
            Dim PuntiRisultati() As Single = {}
            Dim PuntiJolly() As Single = {}
            Dim PuntiQuote() As Single = {}
            Dim PuntiQuoteS() As Single = {}
            Dim PuntiQuoteR() As Single = {}
            Dim Pagante() As String = {}
            Dim Tappato() As Boolean = {}
            Dim g As Integer

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                g = DatiGioco.GiornataSpeciale + 100
            Else
                g = DatiGioco.Giornata
            End If

            ' Prende tutti i giocatori validi
            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & "And Cancellato='N'"
            RecG = Db.LeggeQuery(ConnSQL, Sql)
            Do Until RecG.Eof
                ReDim Preserve Giocatori(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiSegni(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiRisultati(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiJolly(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiQuote(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiQuoteR(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiFR(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiQuoteS(RecG("CodGiocatore").Value)
                ReDim Preserve Pagante(RecG("CodGiocatore").Value)
                ReDim Preserve Tappato(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiSomma(RecG("CodGiocatore").Value)
                ReDim Preserve PuntiDifferenza(RecG("CodGiocatore").Value)

                Giocatori(RecG("CodGiocatore").Value) = RecG("Giocatore").Value
                PuntiSegni(RecG("CodGiocatore").Value) = 0
                PuntiFR(RecG("CodGiocatore").Value) = 0
                PuntiRisultati(RecG("CodGiocatore").Value) = 0
                PuntiJolly(RecG("CodGiocatore").Value) = 0
                PuntiQuote(RecG("CodGiocatore").Value) = 0
                PuntiQuoteR(RecG("CodGiocatore").Value) = 0
                PuntiQuoteS(RecG("CodGiocatore").Value) = 0
                Pagante(RecG("CodGiocatore").Value) = RecG("Pagante").Value
                Tappato(RecG("CodGiocatore").Value) = False
                PuntiDifferenza(RecG("CodGiocatore").Value) = 0
                PuntiSomma(RecG("CodGiocatore").Value) = 0

                RecG.MoveNext
            Loop
            RecG.Close

            Sql = "Select * From Giocatori Where " &
                "Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' "
            RecG = Db.LeggeQuery(ConnSQL, Sql)
            Do Until RecG.Eof
                Sql = "Select * From QuandoTappi Where " &
                   "Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & RecG("CodGiocatore").Value & " " &
                   "And Giornata=" & g
                RecI = Db.LeggeQuery(ConnSQL, Sql)
                If RecI.Eof = True Then
                    Tappato(RecG("CodGiocatore").Value) = False
                Else
                    Tappato(RecG("CodGiocatore").Value) = True
                End If
                RecI.Close()

                If RecG("CodGiocatore").Value > QuantiGioc Then
                    QuantiGioc = RecG("CodGiocatore").Value
                End If

                RecG.MoveNext()
            Loop
            RecG.Close()
            ' Prende tutti i giocatori

            ' Prende punti fatti
            Dim RisultatoUscito As String
            Dim GoalCasaUscito As String
            Dim GoalFuoriUscito As String
            Dim RisultatoGiocato As String
            Dim GoalCasaGiocato As String
            Dim GoalFuoriGiocato As String
            Dim SegnoUscito As String
            Dim SegnoGiocato As String
            Dim ValoreQuoteSegno As Single
            Dim ValoreQuoteRisultato As Single
            Dim NomeCampo As String
            Dim filRouge As Integer
            Dim DifferenzaGoalUscita As Integer
            Dim TotaleGoalUscita As Integer
            Dim DifferenzaGoalGiocato As Integer
            Dim TotaleGoalGiocato As Integer
            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)

            Sql = "Select * From Schedine Where " &
                "Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " " &
                "Order By Partita"
            RecG = Db.LeggeQuery(ConnSQL, Sql)
            Do Until RecG.Eof
                SegnoUscito = "" & RecG("Segno").Value
                If SegnoUscito <> "S" Then
                    RisultatoUscito = "" & RecG("Risultato").Value
                    If RisultatoUscito.IndexOf("-") = -1 Then
                        Dim Messaggi() As String = {"Risultati non immessi correttamente"}
                        VisualizzaMessaggioInPopup(Messaggi, Me)
                        RecG.Close()
                        Db = Nothing
                        Exit Sub
                    Else
                        GoalCasaUscito = Mid(RisultatoUscito, 1, RisultatoUscito.IndexOf("-")).Trim
                        GoalFuoriUscito = Mid(RisultatoUscito, RisultatoUscito.IndexOf("-") + 2, RisultatoUscito.Length).Trim

                        DifferenzaGoalUscita = Math.Abs(Val(GoalCasaUscito) - Val(GoalFuoriUscito))
                        TotaleGoalUscita = Val(GoalCasaUscito) + Val(GoalFuoriUscito)

                        For i As Integer = 0 To QuantiGioc
                            If Giocatori(i) <> "" Then
                                Sql = "Select * From FilsRouge " &
                                "Where " &
                                "Anno=" & DatiGioco.AnnoAttuale & " " &
                                "And Giornata=" & g & " " &
                                "And CodGiocatore=" & i
                                RecI = Db.LeggeQuery(ConnSQL, Sql)
                                filRouge = -1
                                If RecI.Eof = False Then
                                    filRouge = RecI("FilRouge").Value
                                End If
                                RecI.Close()

                                Sql = "Select * From Pronostici A " _
                                & "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " _
                                & "Where " _
                                & "A.Anno=" & DatiGioco.AnnoAttuale & " " _
                                & "And A.Giornata=" & g & " " _
                                & "And A.Partita=" & RecG("Partita").Value & " " _
                                & "And B.Cancellato='N' " _
                                & "And A.CodGiocatore=" & i
                                RecI = Db.LeggeQuery(ConnSQL, Sql)
                                Do Until RecI.Eof
                                    SegnoGiocato = RecI("Segni").Value

                                    If SegnoGiocato.ToUpper.Trim = SegnoUscito.ToUpper.Trim Then
                                        ' Legge la quota per segno 1X2 preso
                                        NomeCampo = "r" & SegnoGiocato.ToUpper.Trim
                                        ValoreQuoteSegno = RitornaValoreQuota(Db, ConnSQL, NomeCampo, RecG("Partita").Value)

                                        ' Preso segno senza doppie
                                        'If Tappato(i) = True Then
                                        '    PuntiSegni(RecI("CodGiocatore").Value) += 0.5
                                        '    ValoreQuoteSegno /= 2
                                        '    If filRouge = RecG("Partita").Value Then
                                        '        PuntiFR(RecI("CodGiocatore").Value) += 0.5
                                        '    End If
                                        'Else
                                        PuntiSegni(RecI("CodGiocatore").Value) += 1
                                        If filRouge = RecG("Partita").Value Then
                                            PuntiFR(RecI("CodGiocatore").Value) += 1
                                        End If
                                        'End If

                                        ' Controlla la quota per sommare il valore
                                        PuntiQuote(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                        PuntiQuoteS(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                        If filRouge = RecG("Partita").Value Then
                                            PuntiFR(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                        End If

                                        If RecG("Partita").Value = DatiGioco.PartitaJolly Then
                                            PuntiJolly(i) += 0.33
                                            If filRouge = RecG("Partita").Value Then
                                                PuntiFR(RecI("CodGiocatore").Value) += 0.33
                                            End If
                                        End If
                                    Else
                                        If SegnoGiocato.Length = 2 Then
                                            If SegnoGiocato.IndexOf(SegnoUscito) > -1 Then
                                                ' Preso segno con doppie
                                                PuntiSegni(RecI("CodGiocatore").Value) += 0.5
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.5
                                                End If

                                                ' Legge la quota per segno 1X2 preso ma con doppia
                                                NomeCampo = "r" & SegnoGiocato.ToUpper.Trim
                                                ValoreQuoteSegno = RitornaValoreQuota(Db, ConnSQL, NomeCampo, RecG("Partita").Value)
                                                ' Controlla la quota per sommare il valore
                                                PuntiQuote(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                                PuntiQuoteS(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += ValoreQuoteSegno
                                                End If

                                                If RecG("Partita").Value = DatiGioco.PartitaJolly Then
                                                    PuntiJolly(i) += 0.2
                                                    If filRouge = RecG("Partita").Value Then
                                                        PuntiFR(RecI("CodGiocatore").Value) += 0.2
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If

                                    RisultatoGiocato = RecI("Risultato").Value

                                    If RisultatoUscito = RisultatoGiocato Then
                                        ' Preso risultato uscito
                                        'If Tappato(i) = True Then
                                        '    PuntiRisultati(RecI("CodGiocatore").Value) += 1
                                        '    ValoreQuoteRisultato /= 2
                                        '    If filRouge = RecG("Partita").Value Then
                                        '        PuntiFR(RecI("CodGiocatore").Value) += 1
                                        '    End If
                                        'Else

                                        PuntiRisultati(RecI("CodGiocatore").Value) += 2
                                        If filRouge = RecG("Partita").Value Then
                                            PuntiFR(RecI("CodGiocatore").Value) += 2
                                        End If

                                        Dim p As Single = DifferenzaGoalGiocato / 2
                                        p = CInt(p * 100) / 100

                                        If Tappato(i) = True Then
                                            p = p / 2
                                            p = CInt(p * 100) / 100
                                        End If
                                        If p <= 0 Then p = 0.3

                                        PuntiSomma(RecI("CodGiocatore").Value) += p
                                        PuntiDifferenza(RecI("CodGiocatore").Value) += p

                                        PuntiRisultati(RecI("CodGiocatore").Value) += p
                                        If filRouge = RecG("Partita").Value Then
                                            PuntiFR(RecI("CodGiocatore").Value) += p
                                        End If

                                        ' Legge la quota per risultato
                                        ' NomeCampo = "r" & RisultatoUscito.ToUpper.Trim.Replace("-", "_")
                                        NomeCampo = "r" & RisultatoUscito.ToUpper.Trim.Replace("-", "_")
                                        If InStr(NomeCampo, "5") Then
                                            NomeCampo = "rAltro"
                                        End If
                                        ValoreQuoteRisultato = RitornaValoreQuota(Db, ConnSQL, NomeCampo, RecG("Partita").Value)

                                        ' Controlla la quota per sommare il valore
                                        PuntiQuote(RecI("CodGiocatore").Value) += ValoreQuoteRisultato
                                        PuntiQuoteR(RecI("CodGiocatore").Value) += ValoreQuoteRisultato
                                        If filRouge = RecG("Partita").Value Then
                                            PuntiFR(RecI("CodGiocatore").Value) += ValoreQuoteRisultato
                                        End If
                                        'End If

                                        If RecG("Partita").Value = DatiGioco.PartitaJolly Then
                                            PuntiJolly(i) += 0.5
                                            If filRouge = RecG("Partita").Value Then
                                                PuntiFR(RecI("CodGiocatore").Value) += 0.5
                                            End If
                                        End If
                                    Else
                                        GoalCasaGiocato = Mid(RisultatoGiocato, 1, RisultatoGiocato.IndexOf("-")).Trim
                                        GoalFuoriGiocato = Mid(RisultatoGiocato, RisultatoGiocato.IndexOf("-") + 2, RisultatoGiocato.Length).Trim

                                        DifferenzaGoalGiocato = Math.Abs(Val(GoalCasaGiocato) - Val(GoalFuoriGiocato))
                                        TotaleGoalGiocato = Val(GoalCasaGiocato) + Val(GoalFuoriGiocato)

                                        If DifferenzaGoalUscita = DifferenzaGoalGiocato Then
                                            Dim p As Single = DifferenzaGoalGiocato / 3
                                            p = CInt(p * 100) / 100
                                            If p <= 0 Then p = 0.3

                                            If Tappato(i) = True Then
                                                p = p / 2
                                                p = CInt(p * 100) / 100
                                                If p <= 0 Then p = 0.3

                                                PuntiDifferenza(RecI("CodGiocatore").Value) += p

                                                PuntiRisultati(RecI("CodGiocatore").Value) += p
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += p
                                                End If
                                            Else
                                                PuntiDifferenza(RecI("CodGiocatore").Value) += p

                                                PuntiRisultati(RecI("CodGiocatore").Value) += p
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += p
                                                End If
                                            End If
                                        End If

                                        If TotaleGoalUscita = TotaleGoalGiocato Then
                                            Dim p As Single = DifferenzaGoalGiocato / 4
                                            p = CInt(p * 100) / 100
                                            If p <= 0 Then p = 0.3

                                            If Tappato(i) = True Then
                                                p = p / 2
                                                p = CInt(p * 100) / 100
                                                If p <= 0 Then p = 0.3

                                                PuntiSomma(RecI("CodGiocatore").Value) += p

                                                PuntiRisultati(RecI("CodGiocatore").Value) += p
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += p
                                                End If
                                            Else
                                                PuntiSomma(RecI("CodGiocatore").Value) += p

                                                PuntiRisultati(RecI("CodGiocatore").Value) += p
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += p
                                                End If
                                            End If
                                        End If

                                        If GoalCasaGiocato = GoalCasaUscito Then
                                            ' Preso goal in casa
                                            If Tappato(i) = True Then
                                                PuntiRisultati(RecI("CodGiocatore").Value) += 0.25
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.25
                                                End If
                                            Else
                                                PuntiRisultati(RecI("CodGiocatore").Value) += 0.5
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.5
                                                End If
                                            End If

                                            If RecG("Partita").Value = DatiGioco.PartitaJolly Then
                                                PuntiJolly(i) += 0.1
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.1
                                                End If
                                            End If
                                        End If

                                        If GoalFuoriGiocato = GoalFuoriUscito Then
                                            ' Preso goal fuori casa
                                            If Tappato(i) = True Then
                                                PuntiRisultati(RecI("CodGiocatore").Value) += 0.25
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.25
                                                End If
                                            Else
                                                PuntiRisultati(RecI("CodGiocatore").Value) += 0.5
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.5
                                                End If
                                            End If
                                            If RecG("Partita").Value = DatiGioco.PartitaJolly Then
                                                PuntiJolly(i) += 0.1
                                                If filRouge = RecG("Partita").Value Then
                                                    PuntiFR(RecI("CodGiocatore").Value) += 0.1
                                                End If
                                            End If
                                        End If
                                    End If

                                    RecI.MoveNext()
                                Loop

                                RecI.Close()
                            End If
                        Next
                    End If
                End If

                RecG.MoveNext()
            Loop
            RecG.Close()
            ' Prende punti fatti

            ' Aggiorna i RisultatiPerGioc nella tabella
            Sql = "Delete From RisultatiPerGioc Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & g
            Db.EsegueSql(ConnSQL, Sql)

            If Speciale = True Then
                Sql = "Delete From ClassificaSpecialePerGioc Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiornata=" & g
            End If
            Db.EsegueSql(ConnSQL, Sql)

            ' Scrive log delle quote rilevate
            'Dim TestoFile As String

            'TestoFile = "CodGioc;Giocatore;Quote Totali;Quote Ris.;Quote Segni;QT/14;QR/14;QS/14;Punti Totali;Punti Senza Quote;Punti Somma;Punti Differenza;" & vbCrLf
            'For i As Integer = 0 To QuantiGioc
            '    If Giocatori(i) <> "" Then
            '        Dim pt As Single
            '        Dim pts As Single

            '        Dim qti As Single = 0
            '        Dim qri As Single = 0
            '        Dim qsi As Single = 0

            '        Dim qt As Single = 0
            '        Dim qr As Single = 0
            '        Dim qs As Single = 0

            '        'If Tappato(i) = False Then
            '        '    qti = PuntiQuote(i)
            '        '    qri = PuntiQuoteR(i)
            '        '    qsi = PuntiQuoteS(i)

            '        '    qt = PuntiQuote(i) / 14
            '        '    qr = PuntiQuoteR(i) / 14
            '        '    qs = PuntiQuoteS(i) / 14

            '        '    pt = PuntiSegni(i) + PuntiRisultati(i) + PuntiJolly(i) + (PuntiQuote(i) / 14)
            '        '    pts = PuntiSegni(i) + PuntiRisultati(i) + PuntiJolly(i)
            '        'Else
            '        pt = PuntiSegni(i) + PuntiRisultati(i) + PuntiJolly(i) + (PuntiQuote(i) / 14)
            '        pts = PuntiSegni(i) + PuntiRisultati(i) + PuntiJolly(i)
            '        'End If

            '        TestoFile += i & ";" & Giocatori(i) & ";" & Tappato(i) & ";" & qti & ";" & qri & ";" & qsi & ";" & qt & ";" & qr & ";" & qs & ";" & pt & ";" & pts & ";" & PuntiSomma(i) & ";" & PuntiDifferenza(i) & ";" & vbCrLf
            '    End If
            'Next

            'Dim gf As New GestioneFilesDirectory
            'Dim NomeFile As String = Server.MapPath(".") & "\Appoggio\Valori_Quote_Giornata_" & DatiGioco.Giornata & ".Csv"
            'gf.CreaAggiornaFile(NomeFile, TestoFile)
            'gf = Nothing
            ' Scrive log delle quote rilevate

            Dim PuntiTot As Single

            For i As Integer = 0 To QuantiGioc
                If Giocatori(i) <> "" Then
                    ' Divide le quote per 14 (solo per tappo=no)
                    'If Tappato(i) = False Then
                    '    PuntiQuote(i) /= 14
                    'Else
                    '    PuntiQuote(i) = 0
                    'End If
                    ' Divide le quote per 14 (solo per tappo=no)

                    PuntiSegni(i) = Int(PuntiSegni(i) * 100) / 100
                    PuntiRisultati(i) = Int(PuntiRisultati(i) * 100) / 100
                    PuntiJolly(i) = Int(PuntiJolly(i) * 100) / 100
                    PuntiQuote(i) = Int(PuntiQuote(i) * 100) / 100
                    PuntiFR(i) = Int(PuntiFR(i) * 100) / 100
                    PuntiSomma(i) = Int(PuntiSomma(i) * 100) / 100
                    PuntiDifferenza(i) = Int(PuntiDifferenza(i) * 100) / 100

                    Sql = "Insert Into RisultatiPerGioc Values (" &
                        " " & DatiGioco.AnnoAttuale & ", " &
                        " " & g & ", " &
                        " " & i & ", " &
                        " " & PuntiSegni(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiRisultati(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiJolly(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiQuote(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiFR(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiSomma(i).ToString.Replace(",", ".") & ", " &
                        " " & PuntiDifferenza(i).ToString.Replace(",", ".") & " " &
                        ")"

                    Db.EsegueSql(ConnSQL, Sql)

                    If Speciale = True Then
                        PuntiTot = PuntiSegni(i) + PuntiRisultati(i) + PuntiJolly(i) + PuntiQuote(i) + PuntiFR(i)

                        Sql = "Insert Into ClassificaSpecialePerGioc Values (" &
                            " " & DatiGioco.AnnoAttuale & ", " &
                            " " & g & ", " &
                            " " & i & ", " &
                            " " & PuntiTot.ToString.Replace(",", ".") & " " &
                            ")"

                        Db.EsegueSql(ConnSQL, Sql)
                    End If
                End If
            Next
            ' Aggiorna i RisultatiPerGioc nella tabella

            MostraRisultati(Giocatori, Pagante, PuntiSegni, PuntiRisultati, PuntiJolly, PuntiQuote, PuntiFR, PuntiSomma, PuntiDifferenza, Tappato)

            ConnSQL.Close()
        End If

        divVincitori.Visible = False
        divUltimoPosto.Visible = False
        divClass.Visible = True
        cmdMostraTutti.Visible = True
        'cmdAggiorna.Visible = False
        'cmdRiepilogo.Visible = False
        'cmdAvvisa.Visible = False

        Db = Nothing
    End Sub

    Private Sub MostraRisultati(Giocatori() As String, Pagante() As String, PuntiSegni() As Single, PuntiRisultati() As Single, PuntiJolly() As Single, PuntiQuote() As Single,
                                PuntiFR() As Single, PuntiSomma() As Single, PuntiDifferenza() As Single, Tappato() As Boolean)
        Dim Posiz As New DataColumn("Posizione")
        Dim Gioc As New DataColumn("Giocatore")
        Dim Totale As New DataColumn("PuntiTotali")
        Dim Risultati As New DataColumn("PuntiRis")
        Dim Segni As New DataColumn("PuntiSegni")
        Dim Jolly As New DataColumn("PuntiJolly")
        Dim Quote As New DataColumn("PuntiQuote")
        Dim FR As New DataColumn("PuntiFR")
        Dim PS As New DataColumn("PuntiSomma")
        Dim PD As New DataColumn("PuntiDifferenza")
        Dim sPagante As New DataColumn("Pagante")
        Dim Tapp As New DataColumn("Tappato")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim I As Integer
        Dim PosizioneReale As Integer = 0
        Dim Rec As Object = CreateObject("ADODB.Recordset")

        Dim QuantiGioc As Integer = UBound(Giocatori)
        'Dim Appo As String
        'Dim AppoB As Boolean
        'Dim AppoVal As Single

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String = ""

            Dim gg As Integer
            Dim idGioc As Integer
            Dim g As New Giocatori

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            'Sql = "Create Table AppoClassGiornataPerGioc (Posiz Int, CodGioc Varchar(20), Totale Numeric(8,3), RisultatiPerGioc Numeric(8,3), " &
            '    "Segni Numeric(8,3), Jolly Numeric(8,3), Quote Numeric(8,3), Fr Numeric(8,3), Pagante Char(1), Tappato Char(1), Anno Int NOT NULL, Giornata Int NOT NULL, idGioc Int NOT NULL " &
            '    " Constraint [PK_AppoClassGiornata] PRIMARY KEY CLUSTERED " &
            '    "( " &
            '    "	[Anno] Asc, " &
            '    "	[Giornata] Asc, " &
            '    "	[idGioc] Asc " &
            '    ") WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " &
            '    ") ON [PRIMARY]"
            'Try
            '    Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
            'Catch ex As Exception
            'End Try

            Sql = "Delete From AppoClassGiornataPerGioc Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
            Db.EsegueSqlSenzaTRY(ConnSQL, Sql)

            For I = 1 To QuantiGioc
                idGioc = g.TornaIdGiocatore(Giocatori(I))

                Sql = "Insert Into AppoClassGiornataPerGioc Values (" &
                    " " & I & ", " &
                    "'" & Giocatori(I) & "', " &
                    " " & (PuntiRisultati(I) + PuntiSegni(I) + PuntiJolly(I) + PuntiQuote(I) + PuntiFR(I)).ToString.Replace(",", ".") & ", " &
                    " " & PuntiRisultati(I).ToString.Replace(",", ".") & ", " &
                    " " & PuntiSegni(I).ToString.Replace(",", ".") & ", " &
                    " " & PuntiJolly(I).ToString.Replace(",", ".") & ", " &
                    " " & PuntiQuote(I).ToString.Replace(",", ".") & ", " &
                    " " & PuntiFR(I).ToString.Replace(",", ".") & ", " &
                    "'" & Pagante(I) & "', " &
                    "'" & IIf(Tappato(I), "S", "N") & "', " &
                    " " & DatiGioco.AnnoAttuale & ", " &
                    " " & gg & ", " &
                    " " & idGioc & ", " &
                    " " & PuntiSomma(I).ToString.Replace(",", ".") & ", " &
                    " " & PuntiDifferenza(I).ToString.Replace(",", ".") & " " &
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            Next

            dttTabella.Columns.Add(Posiz)
            dttTabella.Columns.Add(Gioc)
            dttTabella.Columns.Add(Totale)
            dttTabella.Columns.Add(Risultati)
            dttTabella.Columns.Add(Segni)
            dttTabella.Columns.Add(Jolly)
            dttTabella.Columns.Add(Quote)
            dttTabella.Columns.Add(FR)
            dttTabella.Columns.Add(PS)
            dttTabella.Columns.Add(PD)
            dttTabella.Columns.Add(sPagante)
            dttTabella.Columns.Add(Tapp)

            Erase Classifica
            Erase Giocatori
            Erase PuntiRisultati
            Erase PuntiSegni
            Erase PuntiJolly
            Erase PuntiQuote
            Erase PuntiFR
            Erase PuntiSomma
            Erase PuntiDifferenza
            Erase Pagante
            Erase Tappato

            Sql = "Select * From AppoClassGiornataPerGioc Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " Order By Tappato, Totale Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                PosizioneReale += 1

                riga = dttTabella.NewRow()
                riga(0) = PosizioneReale
                riga(1) = "****"
                riga(2) = "*" '  PuntiRisultati(I) + PuntiSegni(I) + PuntiJolly(I)
                riga(3) = "*" ' PuntiRisultati(I)
                riga(4) = "*" ' PuntiSegni(I)
                riga(5) = "*" ' PuntiJolly(I)
                riga(6) = "*" ' PuntiQuote(I)
                riga(8) = "*" ' PuntiSomma(I)
                riga(9) = "*" ' PuntiDifferenza(I)
                riga(10) = "*"
                riga(11) = "*"
                dttTabella.Rows.Add(riga)

                ReDim Preserve Classifica(PosizioneReale)
                ReDim Preserve Giocatori(PosizioneReale)
                ReDim Preserve PuntiRisultati(PosizioneReale)
                ReDim Preserve PuntiSegni(PosizioneReale)
                ReDim Preserve PuntiJolly(PosizioneReale)
                ReDim Preserve PuntiQuote(PosizioneReale)
                ReDim Preserve PuntiFR(PosizioneReale)
                ReDim Preserve PuntiSomma(PosizioneReale)
                ReDim Preserve PuntiDifferenza(PosizioneReale)
                ReDim Preserve Pagante(PosizioneReale)
                ReDim Preserve Tappato(PosizioneReale)

                Classifica(PosizioneReale) = PosizioneReale.ToString.Trim & ";" & Rec(1).Value & ";" &
                    (Rec(2).Value).ToString.Trim & ";" &
                    Rec(3).Value & ";" & Rec(4).Value & ";" & Rec(5).Value & ";" & Rec(6).Value & ";" & Rec(7).Value & ";" &
                    Rec(13).value & ";" & Rec(14).Value & ";" & Rec(8).Value & ";" & Rec(9).Value & ";"

                Giocatori(PosizioneReale) = Rec(1).Value
                PuntiRisultati(PosizioneReale) = Rec(3).Value
                PuntiSegni(PosizioneReale) = Rec(4).Value
                PuntiJolly(PosizioneReale) = Rec(5).Value
                PuntiQuote(PosizioneReale) = Rec(6).Value
                PuntiFR(PosizioneReale) = Rec(7).Value
                PuntiSomma(PosizioneReale) = Rec(10).Value
                PuntiDifferenza(PosizioneReale) = Rec(11).Value
                Pagante(PosizioneReale) = Rec(8).Value
                Tappato(PosizioneReale) = IIf(Rec(9).Value = "S", True, False)

                Rec.MoveNext
            Loop

            grdClassvinc.DataSource = dttTabella
            grdClassvinc.DataBind()

            divRisultati.Visible = True

            'Sql = "Drop Table AppoClassGiornata"
            'Db.EsegueSql(ConnSQL, Sql)

            g = Nothing
        End If
    End Sub

    Protected Sub MostraGioc(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Giocatore() As String = Classifica(Val(row.Cells(0).Text)).Split(";")
        Dim Nick As String = Giocatore(1)
        Dim Pagante As String = Giocatore(10)
        Dim Tappato As String = Giocatore(11)

        row.Cells(2).Text = Nick
        row.Cells(3).Text = Giocatore(2)
        row.Cells(4).Text = Giocatore(3)
        row.Cells(5).Text = Giocatore(4)
        row.Cells(6).Text = Giocatore(5)
        row.Cells(7).Text = Giocatore(6)
        row.Cells(8).Text = Giocatore(7)
        row.Cells(9).Text = Giocatore(8)
        row.Cells(10).Text = Giocatore(9)
        row.Cells(11).Text = Pagante
        row.Cells(12).Text = Tappato

        Dim TastoImmagine As ImageButton = DirectCast(row.FindControl("imgMostra"), ImageButton)

        TastoImmagine.Visible = False

        Classifica(Val(row.Cells(0).Text)) += "^^^"

        Dim Img As Image = DirectCast(row.FindControl("imgUtente"), Image)

        If Nick = "****" Then
            Img.Visible = False
        Else
            Img.Visible = True
            Img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nick)
            Img.DataBind()
        End If

        Dim Ok As Boolean = True

        For i As Integer = 1 To UBound(Classifica)
            If Right(Classifica(i), 3) <> "^^^" Then
                Ok = False
                Exit For
            End If
        Next

        If Ok = True Then
            'cmdRiepilogo.Visible = True
            'cmdAvvisa.Visible = True
            cmdMostraTutti.Visible = False

            ScriveControllato()
        End If
    End Sub

    Private Sub MostraVincitoriEUltimi()
        'Dim Campi() As String
        Dim Db As New GestioneDB
        'Dim idGioc As String

        If Db.LeggeImpostazioniDiBase() = True Then
            divClass.Visible = False

            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim g As New Giocatori
            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim gg As Integer

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Dim PuntiAtt As Single
            Dim Posiz As New DataColumn("Posizione")
            Dim Gioc As New DataColumn("Giocatore")
            Dim Totale As New DataColumn("PuntiTotali")
            Dim Risultati As New DataColumn("PuntiRis")
            Dim Segni As New DataColumn("PuntiSegni")
            Dim Jolly As New DataColumn("PuntiJolly")
            Dim Quote As New DataColumn("PuntiQuote")
            Dim FR As New DataColumn("PuntiFR")
            Dim PD As New DataColumn("PuntiDifferenza")
            Dim PS As New DataColumn("PuntiSomma")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Posiz)
            dttTabella.Columns.Add(Gioc)
            dttTabella.Columns.Add(Totale)
            dttTabella.Columns.Add(Risultati)
            dttTabella.Columns.Add(Segni)
            dttTabella.Columns.Add(Jolly)
            dttTabella.Columns.Add(Quote)
            dttTabella.Columns.Add(FR)
            dttTabella.Columns.Add(PS)
            dttTabella.Columns.Add(PD)

            QuantiPrimi = 0
            QuantiSecondi = 0

            Dim Tappato As String = "N"

            ' Primi 
            Sql = "Select A.*, B.Giocatore " &
                "From AppoClassGiornataPerGioc A Left Join Giocatori B On A.Anno=B.Anno And A.idGioc=B.CodGiocatore " &
                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata = " & gg & " " &
                "Order By Tappato, A.Totale Desc, Segni Desc, RisultatiPerGioc Desc, Jolly Desc, Quote Desc, FR Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                PuntiAtt = Rec("Totale").Value
            End If
            Do Until Rec.Eof
                If Rec("Totale").Value < PuntiAtt Or Rec("Tappato").Value = "S" Then
                    Tappato = Rec("Tappato").Value
                    Exit Do
                End If

                QuantiPrimi += 1

                riga = dttTabella.NewRow()
                riga(0) = "1"
                riga(1) = Rec("Giocatore").Value
                riga(2) = Rec("Totale").Value
                riga(3) = Rec("Risultati").Value
                riga(4) = Rec("Segni").Value
                riga(5) = Rec("Jolly").Value
                riga(6) = Rec("Quote").Value
                riga(7) = Rec("Fr").Value
                riga(8) = Rec("PuntiSomma").Value
                riga(9) = Rec("PuntiDifferenza").Value
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            ' Primi 

            If Speciale = False Then
                ' Secondo
                riga = dttTabella.NewRow()
                riga(0) = ""
                riga(1) = ""
                riga(2) = ""
                riga(3) = ""
                riga(4) = ""
                riga(5) = ""
                riga(6) = ""
                riga(7) = ""
                riga(8) = ""
                riga(9) = ""
                dttTabella.Rows.Add(riga)

                If Rec.Eof = False Then
                    PuntiAtt = Rec("Totale").Value
                End If
                Do Until Rec.Eof
                    If Rec("Totale").Value < PuntiAtt Or Rec("Tappato").Value <> Tappato Then
                        Exit Do
                    End If

                    QuantiSecondi += 1

                    riga = dttTabella.NewRow()
                    riga(0) = "2"
                    riga(1) = Rec("Giocatore").Value
                    riga(2) = Rec("Totale").Value
                    riga(3) = Rec("Risultati").Value
                    riga(4) = Rec("Segni").Value
                    riga(5) = Rec("Jolly").Value
                    riga(6) = Rec("Quote").Value
                    riga(7) = Rec("FR").Value
                    riga(8) = Rec("PuntiSomma").Value
                    riga(9) = Rec("PuntiDifferenza").Value
                    dttTabella.Rows.Add(riga)

                    Rec.MoveNext()
                Loop
                ' Secondo
            End If

            grdVincitori.DataSource = dttTabella
            grdVincitori.DataBind()

            Rec.Close()

            ' Ultimi
            dttTabella.Clear()

            'Sql = "Select A.*, B.Giocatore, PuntiTot + PuntiSegni + PuntiRis + PuntiJolly + PuntiQuote + PuntiFR From AppoClass A " &
            '    "Left Join Giocatori B On A.Anno=B.Anno And A.idGioc=B.CodGiocatore " &
            '    "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata = " & gg & " " &
            '    "Order By PuntiTot, PuntiSegni, PuntiRis, PuntiJolly, PuntiQuote, PuntiFR "

            Sql = "Select A.*, B.Giocatore " &
                "From AppoClassGiornataPerGioc A Left Join Giocatori B On A.Anno=B.Anno And A.idGioc=B.CodGiocatore " &
                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata = " & gg & " " &
                "Order By A.Totale, Segni, Risultati, Jolly, Quote, FR"

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                PuntiAtt = Rec("Totale").Value
            End If
            Do Until Rec.Bof
                If Rec("Totale").Value > PuntiAtt Then
                    Exit Do
                End If

                riga = dttTabella.NewRow()
                riga(0) = "Ultimo"
                riga(1) = Rec("Giocatore").Value
                riga(2) = Rec("Totale").Value
                riga(3) = Rec("Risultati").Value
                riga(4) = Rec("Segni").Value
                riga(5) = Rec("Jolly").Value
                riga(6) = Rec("Quote").Value
                riga(7) = Rec("FR").Value
                riga(8) = Rec("PuntiSomma").Value
                riga(9) = Rec("PuntiDifferenza").Value
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            ' Ultimi

            grdUltimi.DataSource = dttTabella
            grdUltimi.DataBind()

            Rec.Close()

            ConnSQL.Close()

            If Speciale = False Then
                divVincitori.Visible = True
                divUltimoPosto.Visible = True
                lblPremioSecondo.Visible = True
                lblPremioVincitore.Visible = True
            Else
                divVincitori.Visible = True
                divUltimoPosto.Visible = False
                lblPremioSecondo.Visible = False
                lblPremioVincitore.Visible = False
            End If

            CalcolaPremio()

            g = Nothing
        End If

        Db = Nothing
    End Sub

    Private Sub CalcolaPremio()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim QuantiGiocatoriHannoGiocato As Integer
            Dim g As Integer

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                g = DatiGioco.GiornataSpeciale + 100
            Else
                g = DatiGioco.Giornata
            End If

            Sql = "Select Count(*) As Giocatori From (Select A.CodGiocatore From Pronostici A " &
                "LEFT JOIN Giocatori B On A.CodGiocatore = B.CodGiocatore And A.Anno = B.Anno " &
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And B.Pagante='S' And B.Cancellato='N' " &
                "Group By A.Anno, Giornata, A.CodGiocatore ) A"

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                QuantiGiocatoriHannoGiocato = 0
            Else
                QuantiGiocatoriHannoGiocato = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='VINCITORE SETTIMANALE'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim PercVinc As Integer = Rec("Perc").Value
            Rec.Close()

            Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='PRIMO'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim PercPrimo As Integer = Rec("Perc").Value
            Rec.Close()

            Sql = "Select * From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SECONDO'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim PercSecondo As Integer = Rec("Perc").Value
            Rec.Close()

            Dim Premio As Single = QuantiGiocatoriHannoGiocato * ((QuotaGiocoSettimanale * PercVinc) / 100)
            PremioPrimo = Premio * PercPrimo / 100
            PremioSecondo = Premio * PercSecondo / 100

            PremioPrimo /= QuantiPrimi
            PremioSecondo /= QuantiSecondi

            lblPremioVincitore.Text = "Premio primi: " & ScriveNumeroFormattato(PremioPrimo)
            lblPremioSecondo.Text = "Premio secondi: " & ScriveNumeroFormattato(PremioSecondo)

            ConnSQL.Close()

            Db = Nothing
        End If
    End Sub

    'Protected Sub cmdAggiorna_Click(sender As Object, e As EventArgs) Handles cmdAggiorna.Click
    '    Dim TestoMail As String = ""

    '    Dim CS As New ControlliSchedina

    '    CS.ImpostaParametriControllo(Session("CodGiocatore"), Server.MapPath("."), grdPartite, grdClassvinc, grdVincitori, grdUltimi)

    '    ' Prende dati di dettaglio dei giocatori per la giornata - solo in caso di partita non speciale
    '    CS.PrendeDatiDiDettaglio()
    '    ' Prende dati di dettaglio dei giocatori per la giornata - solo in caso di partita non speciale

    '    TestoMail += CS.VisualizzaPartiteGiornataERisultati()

    '    ' Aggiorna dati di bilancio
    '    CS.AggiornaMovimentiDiBilancio()
    '    ' Aggiorna dati di bilancio

    '    ' Controlla eventuali valori per riconoscimenti disonori
    '    TestoMail += CS.RiconoscimentiDisonori
    '    ' Controlla eventuali valori per riconoscimenti disonori

    '    ' Visualizza classifica attuale o speciale
    '    TestoMail += CS.VisualizzaClassifica()
    '    ' Visualizza classifica attuale o speciale

    '    ' Visualizza classifica risultati
    '    TestoMail += CS.VisualizzaClassificaRisultati
    '    ' Visualizza classifica risultati

    '    ' Controllo Sudden Death
    '    TestoMail += CS.ControlloSuddenDeath
    '    ' Controllo Sudden Death

    '    ' Gestione scontri diretti
    '    TestoMail += CS.ControlloScontriDiretti()
    '    ' Gestione scontri diretti

    '    ' Gestione Quote CUP
    '    TestoMail += CS.ControlloQuoteCUP
    '    ' Gestione Quote CUP

    '    ' Gestione eventi
    '    TestoMail += CS.GestioneEventi()
    '    ' Gestione eventi

    '    CS.ChiusuraControlli(TestoMail)

    '    PrendePartiteDellaGiornata(Server.MapPath(".") & "\Appoggio", "Partite.txt", DatiGioco.Giornata)

    '    Response.Redirect("Principale.aspx")
    'End Sub

    Protected Sub cmdMostraTutti_Click(sender As Object, e As EventArgs) Handles cmdMostraTutti.Click
        Dim Nick As String
        Dim Pagante As String
        Dim Tappato As String
        Dim Giocatori() As String

        For i As Integer = 0 To UBound(Classifica)
            If Classifica(i) <> "" Then
                Dim TastoImmagine As ImageButton = DirectCast(grdClassvinc.Rows(i - 1).FindControl("imgMostra"), ImageButton)

                TastoImmagine.Visible = False

                Giocatori = Classifica(i).Split(";")
                Nick = Giocatori(1)
                Pagante = Giocatori(10)
                Tappato = Giocatori(11)

                grdClassvinc.Rows(i - 1).Cells(2).Text = Nick
                grdClassvinc.Rows(i - 1).Cells(3).Text = Giocatori(2)
                grdClassvinc.Rows(i - 1).Cells(4).Text = Giocatori(3)
                grdClassvinc.Rows(i - 1).Cells(5).Text = Giocatori(4)
                grdClassvinc.Rows(i - 1).Cells(6).Text = Giocatori(5)
                grdClassvinc.Rows(i - 1).Cells(7).Text = Giocatori(6)
                grdClassvinc.Rows(i - 1).Cells(8).Text = Giocatori(7)
                grdClassvinc.Rows(i - 1).Cells(9).Text = Giocatori(8)
                grdClassvinc.Rows(i - 1).Cells(10).Text = Giocatori(9)
                grdClassvinc.Rows(i - 1).Cells(11).Text = Pagante
                grdClassvinc.Rows(i - 1).Cells(12).Text = Tappato

                Dim Img As Image = DirectCast(grdClassvinc.Rows(i - 1).FindControl("imgUtente"), Image)

                If Nick = "****" Then
                    Img.Visible = False
                Else
                    Img.Visible = True
                    Img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nick)
                    Img.DataBind()
                End If
            End If
        Next

        'cmdRiepilogo.Visible = True
        'cmdAvvisa.Visible = True
        cmdMostraTutti.Visible = False

        ScriveControllato()
    End Sub

    Private Sub MostraDivVincUlt()
        'cmdAggiorna.Visible = True
        cmdMostraTutti.Visible = False
        MostraVincitoriEUltimi()
        divRisultati.Visible = True
        'cmdRiepilogo.Visible = False
        'cmdAvvisa.Visible = False
    End Sub

    Private Sub ScriveControllato()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            Dim Sql As String

            Sql = "Delete From ControlliConcorsoGiocatore Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiocatore=" & Session("CodGiocatore") & " And idGiornata=" & DatiGioco.Giornata
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into ControlliConcorsoGiocatore Values (" & DatiGioco.AnnoAttuale & ", " & Session("CodGiocatore") & ", " & DatiGioco.Giornata & ", 'S')"
            Db.EsegueSql(ConnSQL, Sql)
        End If
    End Sub

    Private Sub grdVincitori_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdVincitori.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Img As Image = DirectCast(e.Row.FindControl("imgUtente"), Image)
            Dim Giocatore As String = e.Row.Cells(2).Text

            If Giocatore = "****" Or Giocatore = "" Or Giocatore = "&nbsp;" Then
                Img.Visible = False
            Else
                Img.Visible = True
                Img.ImageUrl = RitornaImmagine(Server.MapPath("."), Giocatore)
                Img.DataBind()
            End If
        End If
    End Sub

    Private Sub grdUltimi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUltimi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Img As Image = DirectCast(e.Row.FindControl("imgUtente"), Image)
            Dim Giocatore As String = e.Row.Cells(2).Text

            If Giocatore = "****" Then
                Img.Visible = False
            Else
                Img.Visible = True
                Img.ImageUrl = RitornaImmagine(Server.MapPath("."), Giocatore)
                Img.DataBind()
            End If
        End If
    End Sub

    'Protected Sub cmdRiepilogo_Click(sender As Object, e As EventArgs) Handles cmdRiepilogo.Click
    '    MostraDivVincUlt()
    'End Sub

    Private Sub grdClassVinc_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdClassvinc.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Img As Image = DirectCast(e.Row.FindControl("imgUtente"), Image)
            Dim Giocatore As String = e.Row.Cells(2).Text

            If Giocatore = "****" Then
                Img.Visible = False
            Else
                Img.Visible = True
                Img.ImageUrl = RitornaImmagine(Server.MapPath("."), Giocatore)
                Img.DataBind()
            End If
        End If
    End Sub

    'Protected Sub cmdAvvisa_Click(sender As Object, e As EventArgs) Handles cmdAvvisa.Click
    '    Dim TestoMail As String = ""
    '    Dim gMail As New GestioneMail
    '    Dim Riga As String
    '    Dim Casa As Label
    '    Dim Fuori As Label
    '    Dim Ris As Label
    '    Dim Segno As Label
    '    Dim immCasa As String = ""
    '    Dim immFuori As String = ""
    '    Dim Speciale As Boolean
    '    Dim Db As New GestioneDB

    '    If Db.LeggeImpostazioniDiBase() = True Then
    '        Dim ConnSQL As Object = Db.ApreDB()

    '        If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
    '            Speciale = True
    '        Else
    '            Speciale = False
    '        End If

    '        ConnSQL.Close()
    '    End If

    '    Dim gg As Integer

    '    If Speciale = True Then
    '        gg = DatiGioco.GiornataSpeciale + 100
    '    Else
    '        gg = DatiGioco.Giornata
    '    End If

    '    TestoMail = "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & " style=""vertical-align: top; ?^?^LARGH?^?^""><tr><td style=""vertical-align: top; width: 59%; text-align: center;"">"

    '    If Speciale = False Then
    '        TestoMail += gMail.ApreTestoTitolo() & "Partite giornata " & DatiGioco.Giornata & " " & gMail.ChiudeTesto() & "<br />"
    '    Else
    '        TestoMail += gMail.ApreTestoTitolo() & "Partite giornata speciale " & DatiGioco.GiornataSpeciale & " " & gMail.ChiudeTesto() & "<br />"
    '    End If
    '    TestoMail += gMail.ApreTabella
    '    Riga = "Partita;;Casa;;Fuori;Risultato;Segno;"
    '    TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
    '    For i As Integer = 0 To grdPartite.Rows.Count - 1
    '        Casa = DirectCast(grdPartite.Rows(i).FindControl("txtCasa"), Label)
    '        Fuori = DirectCast(grdPartite.Rows(i).FindControl("txtFuori"), Label)
    '        Ris = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), Label)
    '        Segno = DirectCast(grdPartite.Rows(i).FindControl("txtSegno"), Label)
    '        immCasa = gMail.RitornaImmagineSquadra(Casa.Text)
    '        immFuori = gMail.RitornaImmagineSquadra(Fuori.Text)

    '        Riga = grdPartite.Rows(i).Cells(0).Text & ";" & immCasa & ";" & Casa.Text & ";" & immFuori & ";" & Fuori.Text & ";" & Ris.Text & ";" & Segno.Text & ";"
    '        TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
    '    Next
    '    TestoMail += "</table> "

    '    TestoMail += gMail.ApreTestoTitolo() & "<br />Risultati giocatori: " & gMail.ChiudeTesto()
    '    TestoMail += gMail.ApreTabella
    '    Riga = "Posizione;;Giocatore;Punti Totali;Punti Risultato;Punti Segno;Punti Jolly;Punti Quote;Pagante;Tappato;"
    '    TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
    '    For i As Integer = 0 To grdClassvinc.Rows.Count - 1
    '        Riga = ""
    '        For k As Integer = 0 To 8
    '            If k = 1 Then
    '                Riga += gMail.RitornaImmagineGiocatore(grdClassvinc.Rows(i).Cells(2).Text) & ";"
    '            Else
    '                Riga += grdClassvinc.Rows(i).Cells(k).Text & ";"
    '            End If
    '        Next
    '        TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
    '    Next
    '    TestoMail += "</table> "

    '    gMail.InviaMailAvvisoAndamentoConcorso(TestoMail, gg, Speciale)
    'End Sub

    Protected Sub cmdUscita_Click(sender As Object, e As EventArgs) Handles cmdUscita.Click
        Response.Redirect("Principale.aspx")
    End Sub
End Class