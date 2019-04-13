Imports System.IO

Public Class ColonnaPropria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

        If DataArrivo < Now Then
            divApertoChiuso.Visible = True
            divContenitoreColonna.Visible = False
        Else
            divApertoChiuso.Visible = False
            divContenitoreColonna.Visible = True
        End If

        If Page.IsPostBack = False Then
            divVelo.Visible = False
            divStorico.Visible = False

            Try
                clsColonna(Session("CodGiocatore")) = New clsColonnaPropria
            Catch ex As Exception
                ReDim Preserve clsColonna(Session("CodGiocatore"))
                clsColonna(Session("CodGiocatore")) = New clsColonnaPropria
            End Try

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

            clsColonna(Session("CodGiocatore")).ImpostaCampi(divApertoChiuso, divColonna, divStat, divContenitoreColonna, Session("CodGiocatore"))
            clsColonna(Session("CodGiocatore")).ImpostaSchermata()
            clsColonna(Session("CodGiocatore")).CaricaPartite(grdPartite, Speciale)
        End If
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        clsColonna(Session("CodGiocatore")).GestioneRigaGridview(e)
    End Sub

    Protected Sub cmdUsaTappo_Click(sender As Object, e As EventArgs) Handles cmdUsaTappo.Click
        Dim Ritorno As String = clsColonna(Session("CodGiocatore")).UsaTappo(grdPartite)

        Dim Messaggi() As String = {Ritorno}
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub

    Protected Sub cmdRandom_Click(sender As Object, e As EventArgs) Handles cmdRandom.Click
        Dim Ritorno1 As String = clsColonna(Session("CodGiocatore")).UsaRandom(grdPartite)
        Dim Ritorno2 As String = clsColonna(Session("CodGiocatore")).RandomRis(grdPartite)

        Dim Messaggi() As String = {Ritorno2}
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub

    'Protected Sub cmdRandomRis_Click(sender As Object, e As EventArgs) Handles cmdRandomRis.Click
    '    Dim Ritorno As String = clsColonna(Session("CodGiocatore")).RandomRis(grdPartite)

    '    Dim Messaggi() As String = {Ritorno}
    '    VisualizzaMessaggioInPopup(Messaggi, Me)
    'End Sub

    Protected Sub SelezionaPartita(sender As Object, e As EventArgs)
        Dim Ritorno As String = clsColonna(Session("CodGiocatore")).SelezionaPartita(sender)

        divDifferenze.InnerHtml = Ritorno
    End Sub

    Protected Sub cmdRitorno_Click(sender As Object, e As EventArgs) Handles cmdRitorno.Click
        Response.Redirect("Principale.aspx")
    End Sub

    Protected Sub StoricoPartita(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim txtCasa As Label = DirectCast(row.FindControl("txtCasa"), Label)
        Dim txtFuori As Label = DirectCast(row.FindControl("txtFuori"), Label)
        Dim Rilevazioni() As String = {}
        Dim qRilevazioni As Integer = 0

        Dim g As New ACCESS
        g.LeggeImpostazioniDiBase(Server.MapPath("."))
        Dim conn As Object = "ADODB.Connection"
        conn = g.ApreDB()
        Dim sql As String = "Select B.Descrizione, A.Concorso, A.Segno From Schedine A Left Join Anni B On A.Anno=B.Anno " &
            "Where Ltrim(Rtrim(Ucase(Casa)))='" & SistemaTestoPerDB(txtCasa.Text.Trim.ToUpper) & "' And  Ltrim(Rtrim(Ucase(Fuori)))='" & SistemaTestoPerDB(txtFuori.Text.Trim.ToUpper) & "' And Segno Is Not Null And Segno <>'S' And Segno <>'' " &
            "Order By A.Anno, A.Concorso"
        Dim rec As Object = "ADODB.Recordset"
        rec = g.LeggeQuery(conn, sql)
        Do Until rec.eof
            qRilevazioni += 1
            ReDim Preserve Rilevazioni(qRilevazioni)
            Rilevazioni(qRilevazioni) = rec("Descrizione").Value & ";" & rec("Concorso").Value & ";" & rec("Segno").Value & ";-;"

            rec.movenext
        Loop
        rec.Close
        conn.Close
        g = Nothing

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            sql = "Select B.Descrizione, A.Giornata, A.Segno, A.Risultato From Schedine A Left Join Anni B On A.Anno=B.Anno " &
            "Where Ltrim(Rtrim(Upper(SquadraCasa)))='" & SistemaTestoPerDB(txtCasa.Text.Trim.ToUpper) & "' And  Ltrim(Rtrim(Upper(SquadraFuori)))='" & SistemaTestoPerDB(txtFuori.Text.Trim.ToUpper) & "' And A.Giornata<50 And Segno Is Not Null And Segno <>'S' And Segno <>'' " &
            "Order By A.Anno, A.Giornata"
            rec = Db.LeggeQuery(ConnSQL, sql)
            Do Until rec.eof
                qRilevazioni += 1
                ReDim Preserve Rilevazioni(qRilevazioni)
                Rilevazioni(qRilevazioni) = rec("Descrizione").Value & ";" & rec("Giornata").Value & ";" & rec("Segno").Value & ";" & rec("Risultato").Value & ";"

                rec.movenext
            Loop
            rec.Close

            sql = "Select B.Descrizione, A.Giornata, A.Segno, A.Risultato From Schedine A Left Join Anni B On A.Anno=B.Anno " &
            "Where Ltrim(Rtrim(Upper(SquadraCasa)))='" & SistemaTestoPerDB(txtCasa.Text.Trim.ToUpper) & "' And  Ltrim(Rtrim(Upper(SquadraFuori)))='" & SistemaTestoPerDB(txtFuori.Text.Trim.ToUpper) & "' And A.Giornata>50 And Segno Is Not Null And Segno <>'S' And Segno <>'' " &
            "Order By A.Anno, A.Giornata"
            rec = Db.LeggeQuery(ConnSQL, sql)
            Do Until rec.eof
                qRilevazioni += 1
                ReDim Preserve Rilevazioni(qRilevazioni)
                Rilevazioni(qRilevazioni) = rec("Descrizione").Value & " (Speciale);" & rec("Giornata").Value & ";" & rec("Segno").Value & ";" & rec("Risultato").Value & ";"

                rec.movenext
            Loop
            rec.Close

            ConnSQL.Close
        End If

        Db = Nothing

        divVelo.Visible = True
        divStorico.Visible = True

        Dim Path As String
        Dim gm As New GestioneMail

        Path = gm.RitornaImmagineSquadra(txtCasa.Text, True)
        imgCasa.ImageUrl = Path

        Path = gm.RitornaImmagineSquadra(txtFuori.Text, True)
        imgFuori.ImageUrl = Path

        lblStorico.Text = txtCasa.Text & "-" & txtFuori.Text

        Dim dDesc As New DataColumn("Descrizione")
        Dim dGiornata As New DataColumn("Giornata")
        Dim dSegno As New DataColumn("Segno")
        Dim dRisu As New DataColumn("Risultato")

        Dim riga As DataRow
        Dim dttTabella As New DataTable()

        dttTabella.Columns.Add(dDesc)
        dttTabella.Columns.Add(dGiornata)
        dttTabella.Columns.Add(dSegno)
        dttTabella.Columns.Add(dRisu)

        Dim Campi() As String

        For i As Integer = qRilevazioni To 1 Step -1
            Campi = Rilevazioni(i).Split(";")

            riga = dttTabella.NewRow()
            riga(0) = Campi(0)
            riga(1) = Campi(1)
            riga(2) = Campi(2)
            riga(3) = Campi(3)
            dttTabella.Rows.Add(riga)
        Next

        grdStorico.DataSource = dttTabella
        grdStorico.DataBind()
        grdStorico.SelectedIndex = -1

        gm = Nothing
    End Sub

    Protected Sub imgChiudi_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudi.Click
        divVelo.Visible = False
        divStorico.Visible = False
    End Sub

    'Protected Sub CalcolaQuoteCheck(sender As Object, e As EventArgs)
    '    Dim row As GridViewRow = DirectCast(DirectCast(sender, CheckBox).NamingContainer, GridViewRow)
    '    Dim check1 As CheckBox = DirectCast(row.FindControl("chk1"), CheckBox)
    '    Dim checkx As CheckBox = DirectCast(row.FindControl("chkx"), CheckBox)
    '    Dim check2 As CheckBox = DirectCast(row.FindControl("chk2"), CheckBox)

    '    If check1.Checked = True And checkx.Checked = True And check2.Checked = True Then
    '        Dim Messaggi() As String = {"Triple non ammesse"}
    '        VisualizzaMessaggioInPopup(Messaggi, Me)
    '        check2.Checked = False
    '    End If

    '    EffettuaCalcoli(row)
    'End Sub

    'Protected Sub CalcolaQuoteText(sender As Object, e As EventArgs)
    '    Dim row As GridViewRow = DirectCast(DirectCast(sender, TextBox).NamingContainer, GridViewRow)

    '    EffettuaCalcoli(row)
    'End Sub

    Private Sub EffettuaCalcoli(row As GridViewRow)
        Dim Partita As Integer = row.Cells(0).Text
        Dim lblStato As Label = DirectCast(row.FindControl("lblStat"), Label)
        Dim imgJolly As Boolean = DirectCast(row.FindControl("imgJolly"), Image).Visible
        Dim Risultato As String = DirectCast(row.FindControl("txtRisultato"), TextBox).Text
        Dim GoalCasa As String = ""
        Dim GoalFuori As String = ""
        If Risultato.IndexOf("-") <> -1 Then
            GoalCasa = Mid(Risultato, 1, Risultato.IndexOf("-"))
            GoalFuori = Mid(Risultato, Risultato.IndexOf("-") + 2, Risultato.Length)
            If Val(GoalCasa) > 4 Or Val(GoalFuori) > 4 Then
                Risultato = "rAltro"
            Else
                Risultato = "r" & GoalCasa & "_" & GoalFuori
            End If
        Else
            Risultato = ""
        End If
        Dim Segno1 As Boolean = DirectCast(row.FindControl("chk1"), CheckBox).Checked
        Dim SegnoX As Boolean = DirectCast(row.FindControl("chkX"), CheckBox).Checked
        Dim Segno2 As Boolean = DirectCast(row.FindControl("chk2"), CheckBox).Checked
        Dim FR As Boolean = DirectCast(row.FindControl("chkFR"), CheckBox).Checked
        Dim SegnoImmesso As String = "r" & IIf(Segno1 = True, "1", "") & IIf(SegnoX = True, "X", "") & IIf(Segno2 = True, "2", "")
        If SegnoImmesso = "r" Then
            SegnoImmesso = ""
            Dim checkAttiva As CheckBox
            If GoalCasa > GoalFuori Then
                Segno1 = True
                checkAttiva = DirectCast(row.FindControl("chk1"), CheckBox)
            Else
                If GoalCasa < GoalFuori Then
                    checkAttiva = DirectCast(row.FindControl("chk2"), CheckBox)
                Else
                    checkAttiva = DirectCast(row.FindControl("chkX"), CheckBox)
                End If
            End If
            checkAttiva.Checked = True
        End If
        Dim Db As New GestioneDB
        Dim QuotaRisultato As Single = 0
        Dim QuotaSegno As Single = 0

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim gg As Integer
            Dim rec As Object = "ADODB.Recordset"

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If
            Dim Ricerca As String = ""
            If Risultato <> "" Then
                Ricerca = Risultato
            Else
                Ricerca = "0"
            End If
            If SegnoImmesso <> "" Then
                Ricerca += ", " & SegnoImmesso
            Else
                Ricerca += ", 0"
            End If
            Dim sql As String = "Select " & Ricerca & " From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And Partita=" & Partita
            rec = Db.LeggeQuery(ConnSQL, sql)
            If rec.Eof = False Then
                QuotaRisultato = rec(0).Value
                QuotaSegno = rec(1).Value
            End If
            rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        If FR = True Then
            QuotaSegno *= 2
            QuotaRisultato *= 2
        End If
        If imgJolly = True Then
            QuotaSegno *= 2
            QuotaRisultato *= 2
        End If

        lblStato.Text = "Segno: " & CInt(QuotaSegno) & "<br />Ris.: " & CInt(QuotaRisultato)

        Dim lblStatoTot As Label = Nothing
        Dim totSegno As Integer = 0
        Dim totRis As Integer = 0
        Dim Campi() As String = {}
        Dim QuantiRilevati As Integer = 0

        For i As Integer = 0 To 13
            lblStatoTot = DirectCast(grdPartite.Rows(i).FindControl("lblStat"), Label)
            If lblStatoTot.Text <> "" Then
                QuantiRilevati += 1
                Campi = lblStatoTot.Text.Split("<br />")
                totSegno += Val(Campi(0).Replace("Segno: ", "")) / QuantiRilevati
                totRis += Val(Campi(1).Replace("br />Ris.: ", "")) / QuantiRilevati
            End If
        Next

        lblTotQuote.Text = " - Stat. Quote: Segno: " & totSegno & " - Risultato: " & totRis & " - Totale: " & totSegno + totRis
    End Sub

    Protected Sub ApriChiudiCasa(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        ApreChiudeRiga(row)
    End Sub

    Protected Sub ApriChiudiFuori(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        ApreChiudeRiga(row)
    End Sub

    Private Sub ApreChiudeRiga(row As GridViewRow)
        Dim divvettoC As HtmlGenericControl = DirectCast(row.FindControl("divPrecCasa"), HtmlGenericControl)
        Dim divvettoF As HtmlGenericControl = DirectCast(row.FindControl("divPrecFuori"), HtmlGenericControl)
        Dim hdn As HiddenField = DirectCast(row.FindControl("hdnCasa"), HiddenField)
        If hdn.Value = "" Then
            hdn.Value = "Aperto"
            divvettoC.Attributes.Add("style", "display:block")
            divvettoF.Attributes.Add("style", "display:block")
        Else
            hdn.Value = ""
            divvettoC.Attributes.Add("style", "display:none")
            divvettoF.Attributes.Add("style", "display:none")
        End If
    End Sub

    Protected Sub cmdStat_Click(sender As Object, e As EventArgs) Handles cmdStat.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim rec As Object = "ADODB.Recordset"
            Dim Sql As String
            Dim Ok As Boolean = True
            Dim Doppie As Integer = 0
            Dim MaxQuota As Single = 0
            Dim QualeFR As Integer = -1

            Sql = "Select Count(*) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And Partita=1"
            rec = Db.LeggeQuery(ConnSQL, Sql)
            If rec(0).Value Is DBNull.Value Then
                Ok = False
            Else
                If rec(0).Value < 3 Then
                    Ok = False
                End If
            End If
            rec.close

            If Ok Then
                For i As Integer = 0 To 13
                    Dim row As GridViewRow = grdPartite.Rows(i)
                    Dim Segno1 As CheckBox = DirectCast(row.FindControl("chk1"), CheckBox)
                    Dim SegnoX As CheckBox = DirectCast(row.FindControl("chkX"), CheckBox)
                    Dim Segno2 As CheckBox = DirectCast(row.FindControl("chk2"), CheckBox)

                    Segno1.Checked = False
                    SegnoX.Checked = False
                    Segno2.Checked = False

                    Dim txtRisultato As TextBox = DirectCast(row.FindControl("txtRisultato"), TextBox)

                    txtRisultato.Text = ""

                    Dim chkFR As CheckBox = DirectCast(row.FindControl("chkFR"), CheckBox)

                    chkFR.Checked = False

                    Dim Q(2) As Integer
                    Dim S() As String = {"1", "X", "2"}
                    Dim RisCasa(9) As Integer
                    Dim RisFuori(9) As Integer
                    Dim RisC() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
                    Dim RisF() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

                    Sql = "Select * From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And Partita=" & i + 1
                    rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until rec.eof
                        Dim Segni As String = rec("Segni").Value
                        Dim Ris As String = rec("Risultato").Value
                        Dim r() As String = {"0", "0"}
                        If Ris <> "" And Ris.Contains("-") Then
                            r = Ris.Split("-")
                        End If

                        If Segni.Contains("1") Then
                            Q(0) += 1
                        End If
                        If Segni.Contains("X") Then
                            Q(1) += 1
                        End If
                        If Segni.Contains("2") Then
                            Q(2) += 1
                        End If

                        RisCasa(Val(r(0))) += 1
                        RisFuori(Val(r(1))) += 1

                        rec.movenext
                    Loop
                    rec.close

                    ' Calcolo segni
                    For ii As Integer = 0 To 2
                        For k As Integer = ii + 1 To 2
                            If Q(ii) < Q(k) Then
                                Dim appoggio As Integer = Q(ii)
                                Q(ii) = Q(k)
                                Q(k) = appoggio
                                Dim apps As String = S(ii)
                                S(ii) = S(k)
                                S(k) = apps
                            End If
                        Next
                    Next

                    If S(0) = "1" Then
                        Segno1.Checked = True

                        If Doppie < 6 Then
                            If Q(0) = Q(1) And Q(0) > Q(2) Then
                                SegnoX.Checked = True
                                Doppie += 1
                            Else
                                If Q(0) = Q(1) And Q(0) = Q(2) Then
                                    Randomize()
                                    Dim x As Integer = Int(Rnd(1) * 2) + 1
                                    If x = 1 Then
                                        SegnoX.Checked = True
                                        Doppie += 1
                                    Else
                                        Segno2.Checked = True
                                        Doppie += 1
                                    End If
                                End If
                            End If
                        End If
                    Else
                        If S(0) = "X" Then
                            SegnoX.Checked = True

                            If Doppie < 6 Then
                                If Q(1) = Q(2) Then
                                    Segno2.Checked = True
                                    Doppie += 1
                                End If
                            End If
                        Else
                            If S(0) = "2" Then
                                Segno2.Checked = True
                            End If
                        End If
                    End If
                    'Calcolo segni

                    'Calcolo risultati
                    For ii As Integer = 0 To 9
                        For k As Integer = ii + 1 To 9
                            If RisCasa(ii) < RisCasa(k) Then
                                Dim appoggio As Integer = RisCasa(ii)
                                RisCasa(ii) = RisCasa(k)
                                RisCasa(k) = appoggio

                                appoggio = RisC(ii)
                                RisC(ii) = RisC(k)
                                RisC(k) = appoggio
                            End If
                            If RisFuori(ii) < RisFuori(k) Then
                                Dim appoggio As Integer = RisFuori(ii)
                                RisFuori(ii) = RisFuori(k)
                                RisFuori(k) = appoggio

                                appoggio = RisF(ii)
                                RisF(ii) = RisF(k)
                                RisF(k) = appoggio
                            End If
                        Next
                    Next

                    txtRisultato.Text = RisC(0) & "-" & RisF(0)

                    Dim diff As Integer = RisC(0) - RisF(0)
                    If diff = 0 Then
                        If Not SegnoX.Checked Then
                            If Doppie < 6 Then
                                Doppie += 1
                                SegnoX.Checked = True
                            Else
                                Segno1.Checked = False
                                SegnoX.Checked = True
                                Segno2.Checked = False
                            End If
                        End If
                    Else
                        If diff > 0 Then
                            If Not Segno1.Checked Then
                                If Doppie < 6 Then
                                    Doppie += 1
                                    Segno1.Checked = True
                                Else
                                    Segno1.Checked = True
                                    SegnoX.Checked = False
                                    Segno2.Checked = False
                                End If
                            End If
                        Else
                            If diff < 0 Then
                                If Not Segno2.Checked Then
                                    If Doppie < 6 Then
                                        Doppie += 1
                                        Segno2.Checked = True
                                    Else
                                        Segno1.Checked = False
                                        SegnoX.Checked = False
                                        Segno2.Checked = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                    'Calcolo risultati

                    'Controllo per quote
                    Dim NomeColonnaSegni As String = "r"

                    If Segno1.Checked Then
                        NomeColonnaSegni += "1"
                    End If
                    If SegnoX.Checked Then
                        NomeColonnaSegni += "x"
                    End If
                    If Segno2.Checked Then
                        NomeColonnaSegni += "2"
                    End If

                    Dim NomeColonnaRisultato As String = "r" & RisC(0).ToString & "_" & RisF(0).ToString

                    If RisC(0) > 4 Or RisF(0) > 4 Then
                        NomeColonnaRisultato = "rAltro"
                    End If

                    Dim Quota As Single = 0

                    Sql = "Select " & NomeColonnaSegni & ", " & NomeColonnaRisultato & " From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And Partita=" & i + 1
                    rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Not rec.eof Then
                        Quota = rec(0).Value + rec(1).Value
                    End If
                    rec.close

                    If Quota > MaxQuota Then
                        MaxQuota = Quota
                        QualeFR = i
                    End If
                    'Controllo per quote

                    EffettuaCalcoli(row)
                Next

                If QualeFR > -1 Then
                    Dim row As GridViewRow = grdPartite.Rows(QualeFR)

                    Dim chkFR As CheckBox = DirectCast(row.FindControl("chkFR"), CheckBox)

                    chkFR.Checked = True

                    EffettuaCalcoli(row)
                End If

                Dim sMessaggi() As String = {"Risultati calcolati"}
                VisualizzaMessaggioInPopup(sMessaggi, Me)
            Else
                Dim sMessaggi() As String = {"Poche schedine immesse per le statistiche"}
                VisualizzaMessaggioInPopup(sMessaggi, Me)
            End If

            ConnSQL.Close
        End If

        Db = Nothing
    End Sub

    'Protected Sub CalcolaQuoteCheck(sender As Object, e As EventArgs)
    '    Dim row As GridViewRow = DirectCast(DirectCast(sender, CheckBox).NamingContainer, GridViewRow)
    '    Dim check1 As CheckBox = DirectCast(row.FindControl("chk1"), CheckBox)
    '    Dim checkx As CheckBox = DirectCast(row.FindControl("chkx"), CheckBox)
    '    Dim check2 As CheckBox = DirectCast(row.FindControl("chk2"), CheckBox)

    '    If check1.Checked = True And checkx.Checked = True And check2.Checked = True Then
    '        Dim Messaggi() As String = {"Triple non ammesse"}
    '        VisualizzaMessaggioInPopup(Messaggi, Me)
    '        check2.Checked = False
    '    End If

    '    EffettuaCalcoli(row)
    'End Sub

    'Protected Sub CalcolaQuoteText(sender As Object, e As EventArgs)
    '    Dim row As GridViewRow = DirectCast(DirectCast(sender, TextBox).NamingContainer, GridViewRow)

    '    EffettuaCalcoli(row)
    'End Sub

    Protected Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        For i As Integer = 0 To 13
            Dim row As GridViewRow = grdPartite.Rows(i)
            Dim check1 As CheckBox = DirectCast(row.FindControl("chk1"), CheckBox)
            Dim checkx As CheckBox = DirectCast(row.FindControl("chkx"), CheckBox)
            Dim check2 As CheckBox = DirectCast(row.FindControl("chk2"), CheckBox)

            If check1.Checked = True And checkx.Checked = True And check2.Checked = True Then
                Dim Messaggi() As String = {"Triple non ammesse alla partita " & (i + 1)}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                ' check2.Checked = False
                Exit For
            End If

            EffettuaCalcoli(row)
        Next
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

        If DataArrivo < Now Then
            divApertoChiuso.Visible = True
            divContenitoreColonna.Visible = False
            Dim sMessaggi() As String = {"Il concorso è chiuso"}
            VisualizzaMessaggioInPopup(sMessaggi, Me)
            Exit Sub
        Else
            divApertoChiuso.Visible = False
        End If

        Dim Ritorno() As String = clsColonna(Session("CodGiocatore")).Salva(grdPartite)
        Dim Messaggi() As String = {}

        For i As Integer = 0 To Ritorno.Length - 1
            ReDim Preserve Messaggi(i)
            Messaggi(i) = Ritorno(i)
        Next
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub
End Class