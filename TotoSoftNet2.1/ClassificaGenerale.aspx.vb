Imports System.IO

Public Class ClassificaGenerale
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divStatistiche.Visible = False

            'Dim Speciale As Boolean = ControllaSpeciale()

            If DatiGioco.StatoConcorso <> ValoriStatoConcorso.Chiuso And DatiGioco.StatoConcorso <> ValoriStatoConcorso.Nessuno And DatiGioco.StatoConcorso <> ValoriStatoConcorso.AnnoChiuso Then
                hdnGiornata.Value = DatiGioco.Giornata - 1
            Else
                hdnGiornata.Value = DatiGioco.Giornata
            End If
            If hdnGiornata.Value < 0 Then hdnGiornata.Value = 0

            CaricaComboStatistiche()
            DisegnaClassifica()
        End If
    End Sub

    'Private Function ControllaSpeciale() As Boolean
    '    Dim Db As New GestioneDB
    '    Dim Speciale As Boolean

    '    If Db.LeggeImpostazioniDiBase() = True Then
    '        Dim ConnSQL As Object = Db.ApreDB()

    '        Speciale = ControllaConcorsoSpeciale(Db, ConnSQL)

    '        ConnSQL.Close()
    '    End If

    '    Db = Nothing

    '    Return Speciale
    'End Function

    Private Sub CaricaComboStatistiche()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato<>'S' Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            cmbGiocatore.Items.Clear()
            Do Until Rec.Eof
                cmbGiocatore.Items.Add(Rec("Giocatore").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub DisegnaClassifica()
        Dim Db As New GestioneDB

        lblGiornata.Text = "Giornata: " & hdnGiornata.Value

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Posiz As New DataColumn("Posizione")
            Dim Giocatore As New DataColumn("Giocatore")
            Dim PuntiTot As New DataColumn("PuntiTot")
            Dim PuntiSegni As New DataColumn("PuntiSegni")
            Dim PuntiRis As New DataColumn("PuntiRis")
            Dim PuntiJolly As New DataColumn("PuntiJolly")
            Dim PuntiQuote As New DataColumn("PuntiQuote")
            Dim PuntiFR As New DataColumn("PuntiFR")
            Dim UltRis As New DataColumn("UltRis")
            Dim Preced As New DataColumn("Precedente")
            Dim Gioc As New DataColumn("Giocate")
            Dim Vittorie As New DataColumn("Vittorie")
            Dim Secondo As New DataColumn("Secondo")
            Dim Ultimo As New DataColumn("Ultimo")
            Dim Tappi As New DataColumn("Tappi")
            Dim Media As New DataColumn("Media")
            Dim RiconDison As New DataColumn("RiconDison")
            Dim Posizione As Integer = 0
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            Dim UltimoRisultato As Single
            Dim Precedente As Single
            Dim vMedia As Single
            Dim Giocate As Single
            Dim TotPunti As Single
            Dim NumeroTappi As Integer
            Dim VecchiPunti As Single

            dttTabella.Columns.Add(Posiz)
            dttTabella.Columns.Add(Giocatore)
            dttTabella.Columns.Add(PuntiTot)
            dttTabella.Columns.Add(PuntiSegni)
            dttTabella.Columns.Add(PuntiRis)
            dttTabella.Columns.Add(PuntiJolly)
            dttTabella.Columns.Add(PuntiQuote)
            dttTabella.Columns.Add(PuntiFR)
            dttTabella.Columns.Add(UltRis)
            dttTabella.Columns.Add(Preced)
            dttTabella.Columns.Add(Gioc)
            dttTabella.Columns.Add(Vittorie)
            dttTabella.Columns.Add(Secondo)
            dttTabella.Columns.Add(Ultimo)
            dttTabella.Columns.Add(Tappi)
            dttTabella.Columns.Add(Media)
            dttTabella.Columns.Add(RiconDison)

            Dim StringaIn As String = ""

            Dim sc As New StringheClassifiche

            Db.EsegueSql(ConnSQL, "Delete From Posizioni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value)

            Sql = sc.RitornaStringaClassificaGenerale(hdnGiornata.Value)
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                StringaIn += Rec("CodGiocatore").Value & ", "

                Sql = "Select Count(Giornata) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value & " And Partita=1 And Giornata<=" & hdnGiornata.Value
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

                Sql = "Select PuntiSegni+PuntiRisultati+PuntiJolly+PuntiQuote+PuntiFR As TotPunti From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & hdnGiornata.Value & " And CodGiocatore=" & Rec("CodGiocatore").Value
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

                Sql = "Select PuntiSegni+PuntiRisultati+PuntiJolly+PuntiQuote+PuntiFR As TotPunti From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & hdnGiornata.Value - 1 & " And CodGiocatore=" & Rec("CodGiocatore").Value
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

                Sql = "Select Count(*) From QuandoTappi Where Anno = " & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2.Eof = True Then
                    NumeroTappi = 0
                Else
                    If Rec2(0) Is DBNull.Value = True Then
                        NumeroTappi = 0
                    Else
                        NumeroTappi = Rec2(0).Value
                    End If
                End If
                Rec2.Close()

                TotPunti = Rec("TotPunti").Value
                If Giocate <> 0 Then
                    vMedia = (TotPunti / Giocate)
                Else
                    vMedia = 0
                End If

                If TotPunti <> VecchiPunti Then
                    Posizione += 1
                    VecchiPunti = TotPunti
                End If

                riga = dttTabella.NewRow()
                riga(0) = Posizione
                riga(1) = Rec("Giocatore").Value
                riga(2) = FormattaNumeroConVirgola(TotPunti)
                riga(3) = FormattaNumeroConVirgola(Rec("PSegni").Value)
                riga(4) = FormattaNumeroConVirgola(Rec("PRisultati").Value)
                riga(5) = FormattaNumeroConVirgola(Rec("PJolly").Value)
                riga(6) = FormattaNumeroConVirgola(Rec("PQuote").Value)
                riga(7) = FormattaNumeroConVirgola(Rec("PFR").Value)
                riga(8) = FormattaNumeroConVirgola(UltimoRisultato)
                riga(9) = FormattaNumeroConVirgola(Precedente)
                riga(10) = Giocate
                riga(11) = IIf(Rec("Vittorie").Value.ToString.Trim = "", "0", Rec("Vittorie").Value.ToString.Trim)
                riga(12) = IIf(Rec("SecondiPosti").Value.ToString.Trim = "", "0", Rec("SecondiPosti").Value.ToString.Trim)
                riga(13) = IIf(Rec("UltimiPosti").Value.ToString.Trim = "", "0", Rec("UltimiPosti").Value.ToString.Trim)
                riga(14) = NumeroTappi
                riga(15) = FormattaNumeroConVirgola(vMedia)
                riga(16) = Rec("RiconDison").Value
                dttTabella.Rows.Add(riga)

                ' Scrive la posizione del giocatore
                Sql = "Insert Into Posizioni Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    " " & hdnGiornata.Value & ", " & _
                    " " & Rec("CodGiocatore").Value & ", " & _
                    " " & Posizione & " " & _
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
                ' Scrive la posizione del giocatore

                Rec.MoveNext()
            Loop
            Rec.Close()

            ' Prende i giocatori che non hanno punti in classifica
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
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Posizione += 1

                    riga = dttTabella.NewRow()
                    riga(0) = Posizione
                    riga(1) = Rec("Giocatore").Value
                    riga(2) = 0
                    riga(3) = 0
                    riga(4) = 0
                    riga(5) = 0
                    riga(6) = 0
                    riga(7) = 0
                    riga(8) = 0
                    riga(9) = 0
                    riga(10) = 0
                    riga(11) = 0
                    riga(12) = 0
                    riga(13) = 0
                    riga(14) = 0
                    riga(15) = 0
                    dttTabella.Rows.Add(riga)

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If
            ' Prende i giocatori che non hanno punti in classifica

            grdClassifica.DataSource = dttTabella
            grdClassifica.DataBind()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub grdClassifica_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdClassifica.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(2).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            'Dim lblMotto As Label = DirectCast(e.Row.FindControl("lblmotto"), Label)
            Dim lblMotto As String
            Dim g As New Giocatori
            'lblMotto.CssClass = "etichettamotto"
            Dim idGiocatore As Integer = g.TornaIdGiocatore(NomeGiocatore)
            lblMotto = g.TornaMottoGiocatore(idGiocatore)
            'If lblMotto.Text = "" Then
            '    lblMotto.Visible = False
            'End If
            g = Nothing

            ImgGioc.ToolTip = lblMotto

            If idGiocatore = Session("CodGiocatore") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            'Dim Immagine As String = RitornaImmagineSfondo(Server.MapPath("."), NomeGiocatore)

            'If Immagine <> "" Then
            '    e.Row.Cells(3).Style.Add("background", "transparent url('" & Immagine & "')")
            '    e.Row.Cells(3).Style.Add("background-repeat", "no-repeat")
            '    e.Row.Cells(3).Style.Add("background-position", "center center")
            '    e.Row.Cells(3).Style.Add("background-size", "90% 90%")
            '    e.Row.Cells(3).Style.Add("opacity", "0.7")

            '    ' Mette il colore di sfondo simile a quello dell'immagine visualizzata
            '    Dim bm As System.Drawing.Bitmap = New System.Drawing.Bitmap(Server.MapPath(".") & "\" & Immagine)
            '    Dim colore As System.Drawing.Color = bm.GetPixel(1, bm.Height / 2)
            '    Dim colorello As String = "#" & Hex(colore.R) & Hex(colore.G) & Hex(colore.B)
            '    e.Row.Cells(3).Style.Add("background-color", colorello)
            '    bm.Dispose()
            '    ' Mette il colore di sfondo simile a quello dell'immagine visualizzata
            'End If

            '' Riconoscimenti / Disonori
            'Dim Contenitore As Literal = DirectCast(e.Row.FindControl("ltlImmagini"), Literal)

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim Conta As Integer = 0

                'Sql = "Select Giornata, B.Immagine, B.Punti, B.Descrizione From RiconDisonGioc A " & _
                '    "Left Join RiconDison B On A.idPremio=B.idPremio " & _
                '    "Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGioc=" & idGiocatore & " And Giornata<=" & hdnGiornata.Value & " " & _
                '    "Order By Giornata"
                'Rec = Db.LeggeQuery(ConnSQL, Sql)
                'Do Until Rec.Eof
                '    Contenitore.Text += "<img id='Image3' src='App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value & "' alt='' height='20px' width='20px' title='Giornata " & Rec("Giornata").Value & " - " & Rec("Descrizione").Value & ": " & Rec("Punti").Value & "' />"
                '    Conta += 1
                '    If Conta = 5 Then
                '        Conta = 0
                '        Contenitore.Text += "<br />"
                '    End If

                '    Rec.MoveNext()
                'Loop
                'Rec.Close()

                ' Visualizza la differenza di posizione
                Dim ImgDiff As Image = DirectCast(e.Row.FindControl("imgDifferenza"), Image)
                'Dim lblDiff As Label = DirectCast(e.Row.FindControl("lblDiff"), Label)
                Dim lblDiff As String
                Dim iconcina As String

                Sql = "Select * From Posizioni Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & hdnGiornata.Value - 1 & " And " & _
                    "CodGiocatore=" & idGiocatore
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = True Then
                    iconcina = "App_Themes/Standard/Images/Icone/PosizioneUguale.png"
                    lblDiff = ""
                Else
                    Dim PosAttuale As Integer = e.Row.Cells(0).Text
                    Dim PosPrima As Integer = Rec("Posizione").value

                    If PosAttuale > PosPrima Then
                        iconcina = "App_Themes/Standard/Images/Icone/PosizionePeggiore.png"
                        lblDiff = "-" & PosAttuale - PosPrima
                    Else
                        If PosAttuale < PosPrima Then
                            iconcina = "App_Themes/Standard/Images/Icone/PosizioneMigliore.png"
                            lblDiff = "+" & PosPrima - PosAttuale
                        Else
                            iconcina = "App_Themes/Standard/Images/Icone/PosizioneUguale.png"
                            lblDiff = ""
                        End If
                    End If
                End If
                Rec.Close()

                ImgDiff.ImageUrl = iconcina
                ImgDiff.ToolTip = lblDiff
                ImgDiff.DataBind()
                ' Visualizza la differenza di posizione

                ConnSQL.Close()
            End If

            Db = Nothing
        End If
    End Sub

    'Protected Sub imgStatPunti_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatPunti.Click
    '    cmbGiocatore.Text = Session("Nick")

    '    CaricaStatistica(1)
    'End Sub

    'Protected Sub imgStatRisultati_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatRisultati.Click
    '    cmbGiocatore.Text = Session("Nick")

    '    CaricaStatistica(2)
    'End Sub

    'Protected Sub imgStatSegni_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatSegni.Click
    '    cmbGiocatore.Text = Session("Nick")

    '    CaricaStatistica(3)
    'End Sub

    'Protected Sub imgStatJolly_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatJolly.Click
    '    cmbGiocatore.Text = Session("Nick")

    '    CaricaStatistica(4)
    'End Sub

    'Protected Sub imgStatPosizioni_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatPosizioni.Click
    '    cmbGiocatore.Text = Session("Nick")

    '    CaricaStatistica(5)
    'End Sub

    Private Sub CaricaStatistica(NumeroStatistica As Integer)
        Dim nValore As String
        Dim nGiornata As String
        Dim Db As New GestioneDB
        Dim Testo1 As String

        hdnQualeStat.Value = NumeroStatistica
        nValore = ""
        nGiornata = ""

        imgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), cmbGiocatore.Text)

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim g As New Giocatori
            Dim CodGiocatore As Integer = g.TornaIdGiocatore(cmbGiocatore.Text)
            g = Nothing

            Select Case NumeroStatistica
                Case 1
                    ' Punti Totali
                    Sql = "Select Concorso, PuntiSegni+PuntiRisultati+PuntiJolly As Punti From Risultati Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                        "CodGiocatore=" & CodGiocatore & " " & _
                        "Order By Concorso"
                    Testo1 = "Punti Totali"
                Case 2
                    ' Risultati
                    Sql = "Select Concorso, PuntiRisultati As Punti From Risultati Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                        "CodGiocatore=" & CodGiocatore & " " & _
                        "Order By Concorso"
                    Testo1 = "Punti Risultati"
                Case 3
                    ' Segni
                    Sql = "Select Concorso, PuntiSegni As Punti From Risultati Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                        "CodGiocatore=" & CodGiocatore & " " & _
                        "Order By Concorso"
                    Testo1 = "Punti Segni"
                Case 4
                    ' Jolly
                    Sql = "Select Concorso, PuntiJolly As Punti From Risultati Where " & _
                        "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                        "CodGiocatore=" & CodGiocatore & " " & _
                        "Order By Concorso"
                    Testo1 = "Punti Jolly"
                Case 5
                    ' Posizioni
                    Dim Posizione As Integer

                    For i As Integer = 1 To Val(hdnGiornata.Value)
                        Sql = "Select idGioc, Sum(Totale) As TotPunti From AppoClassGiornata " &
                            "Where Anno = " & DatiGioco.AnnoAttuale & " And Giornata <= " & i & " " &
                            "Group By idGioc " &
                            "Order By 2 Desc"
                        Rec = Db.LeggeQuery(ConnSQL, Sql)
                        Posizione = 0
                        Do Until Rec.Eof
                            Posizione += 1
                            If Rec("idGioc").Value = CodGiocatore Then
                                nValore += "" & Posizione.ToString.Replace(",", ".") & ","
                                nGiornata += "'" & i.ToString.Trim & "',"

                                Exit Do
                            End If

                            Rec.MoveNext()
                        Loop
                        Rec.Close()

                        Testo1 = "Posizione"
                    Next
            End Select

            If NumeroStatistica <> 5 Then
                Dim Valore As Single

                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    Valore = Rec("Punti").Value

                    nValore += "" & Valore.ToString.Replace(",", ".") & ","
                    nGiornata += "'" & Rec("Concorso").Value.ToString.Trim & "',"

                    Rec.MoveNext()
                Loop
                Rec.Close()
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        If nValore.Length > 1 Then
            nValore = Mid(nValore, 1, Len(nValore) - 1)
        End If

        If nGiornata.Length > 1 Then
            nGiornata = Mid(nGiornata, 1, Len(nGiornata) - 1)
        End If

        hdnPassaggio1.Value = nGiornata
        hdnPassaggio2.Value = nValore

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     DisegnaStatistica('divVisualizzato', 'Giornata','" & Testo1 & "','" & Testo1 & "','');")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)

        divStatistiche.Visible = True
    End Sub

    Protected Sub imgChiudiStat_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudiStat.Click
        divStatistiche.Visible = False
    End Sub

    Protected Sub cmbGiocatore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbGiocatore.SelectedIndexChanged
        CaricaStatistica(hdnQualeStat.Value)
    End Sub

    Protected Sub cmdAvanti_Click(sender As Object, e As EventArgs) Handles cmdAvanti.Click
        Dim Maxim As Integer

        If DatiGioco.StatoConcorso <> ValoriStatoConcorso.Chiuso Then
            Maxim = DatiGioco.Giornata - 1
        Else
            Maxim = DatiGioco.Giornata
        End If

        If Val(hdnGiornata.Value) < Maxim Then
            hdnGiornata.Value += 1

            DisegnaClassifica()
        End If
    End Sub

    Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        If Val(hdnGiornata.Value) > 1 Then
            hdnGiornata.Value -= 1

            DisegnaClassifica()
        End If
    End Sub
End Class