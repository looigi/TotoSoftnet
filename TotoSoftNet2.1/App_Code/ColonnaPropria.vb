Imports System.IO

Public Class clsColonnaPropria
    Inherits System.Web.UI.Page

    Private divApertoChiuso As HtmlControl
    Private divColonna As HtmlControl
    Private divStat As HtmlControl
    Private divTasti As HtmlControl
    Private codGiocatore As Integer

    Public Sub ImpostaCampi(divAC As HtmlControl, divC As HtmlControl, divS As HtmlControl, divT As HtmlControl, g As Integer)
        divApertoChiuso = divAC
        divColonna = divC
        divStat = divS
        divTasti = divT
        codGiocatore = g
    End Sub

    Public Sub ImpostaSchermata()
        divApertoChiuso.Visible = False
        divColonna.Visible = True
        divStat.Visible = True
        divTasti.Visible = True

        If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
            If IsDate(DatiGioco.ChiusuraConcorso) = True Then
                Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

                If DataArrivo <= Now Then
                    divApertoChiuso.Visible = True
                    divColonna.Visible = False
                    divStat.Visible = False
                    divTasti.Visible = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Public Sub CaricaPartite(grdP As GridView, Speciale As Boolean)
        Dim grdPartite As GridView = grdP
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim g As Integer

        If Speciale = True Then
            g = DatiGioco.GiornataSpeciale + 100
        Else
            g = DatiGioco.Giornata
        End If

        Sql = "Select A.Partita From Schedine A " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "A.Giornata=" & g & " Order By A.Partita"
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing
    End Sub

    Private Function EffettuaControlli(grdP As GridView) As String()
        Dim grdPartite As GridView = grdP
        Dim Ok() As String = {}
        Dim qOK As Integer = 0
        Dim chk1 As CheckBox
        Dim chkX As CheckBox
        Dim chk2 As CheckBox
        Dim chkFR As CheckBox
        Dim txtRis As TextBox
        Dim Doppie As Integer = 0
        Dim AppoSegni As String
        Dim Goal1 As String
        Dim Goal2 As String
        Dim FilRouge As Integer = -1

        For i As Integer = 0 To grdPartite.Rows.Count - 1
            chk1 = DirectCast(grdPartite.Rows(i).FindControl("chk1"), CheckBox)
            chkX = DirectCast(grdPartite.Rows(i).FindControl("chkX"), CheckBox)
            chk2 = DirectCast(grdPartite.Rows(i).FindControl("chk2"), CheckBox)
            chkFR = DirectCast(grdPartite.Rows(i).FindControl("chkFR"), CheckBox)

            If chkFR.Checked = True Then
                If FilRouge = -1 Then
                    FilRouge = i + 1
                Else
                    ReDim Preserve Ok(qOK)
                    Ok(qOK) += "Ci sono trpppi Fils Rouge selezionati"
                    qOK += 1
                    Exit For
                End If
            End If

            If chk1.Checked = False And chkX.Checked = False And chk2.Checked = False Then
                ReDim Preserve Ok(qOK)
                Ok(qOK) += "Selezionare un segno per la partita " & i + 1
                qOK += 1
                Exit For
            End If

            If chk1.Checked = True And chkX.Checked = True And chk2.Checked = True Then
                ReDim Preserve Ok(qOK)
                Ok(qOK) += "Troppi segni per la partita " & i + 1
                qOK += 1
                Exit For
            End If

            AppoSegni = IIf(chk1.Checked = True, "1", "") & IIf(chkX.Checked = True, "X", "") & IIf(chk2.Checked = True, "2", "")

            If AppoSegni.Length = 2 Then
                Doppie += 1
                If Doppie > MaxDoppie Then
                    ReDim Preserve Ok(qOK)
                    Ok(qOK) += "Sono state giocate troppe doppie: massimo " & MaxDoppie
                    qOK += 1
                    Exit For
                End If
            End If

            txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)
            If txtRis.Text.Trim <> "" Then
                If txtRis.Text.IndexOf("-") = -1 Then
                    ReDim Preserve Ok(qOK)
                    Ok(qOK) += "Risultato della partita " & i + 1 & " non valido<br />Inserirlo nel formato Goal1-Goal2 (es. 2-1)"
                    qOK += 1
                    Exit For
                Else
                    Goal1 = Mid(txtRis.Text, 1, txtRis.Text.IndexOf("-")).Trim
                    Goal2 = Mid(txtRis.Text, txtRis.Text.IndexOf("-") + 2, txtRis.Text.Length).Trim

                    If IsNumeric(Goal1) = False Then
                        ReDim Preserve Ok(qOK)
                        Ok(qOK) += "Goal in casa nel risultato della partita " & i + 1 & " non valido"
                        qOK += 1
                        Exit For
                    End If

                    If IsNumeric(Goal2) = False Then
                        ReDim Preserve Ok(qOK)
                        Ok(qOK) += "Goal fuori casa nel risultato della partita " & i + 1 & " non valido"
                        qOK += 1
                        Exit For
                    End If

                    ' Sondaggio N° 1 - I risultati devono essere congruenti con il segno
                    Dim Segno As String = ""
                    Dim Errore As Boolean = False

                    If chk1.Checked = True Then Segno += "1"
                    If chkX.Checked = True Then Segno += "X"
                    If chk2.Checked = True Then Segno += "2"

                    Select Case Segno
                        Case "1"
                            If Goal1 = Goal2 Or Goal1 < Goal2 Then
                                Errore = True
                            End If
                        Case "1X"
                            If Goal1 < Goal2 Then
                                Errore = True
                            End If
                        Case "12"
                            If Goal1 = Goal2 Then
                                Errore = True
                            End If
                        Case "X"
                            If Goal1 > Goal2 Or Goal1 < Goal2 Then
                                Errore = True
                            End If
                        Case "X2"
                            If Goal1 > Goal2 Then
                                Errore = True
                            End If
                        Case "2"
                            If Goal1 = Goal2 Or Goal1 > Goal2 Then
                                Errore = True
                            End If
                    End Select
                    If Errore = True Then
                        ReDim Preserve Ok(qOK)
                        Ok(qOK) += "Risultato della partita " & i + 1 & " non congruente con i segni immessi"
                        qOK += 1
                        Exit For
                    End If
                    ' Sondaggio N° 1 - I risultati devono essere congruenti con il segno
                End If
            End If
        Next

        If FilRouge = -1 Then
            ReDim Preserve Ok(qOK)
            Ok(qOK) += "Impostare un Fil Rouge"
            qOK += 1
        End If

        'If Doppie < MaxDoppie Then
        '    VisualizzaMessaggioInPopup("WARNING:<br />giocato un numero di doppie inferiore<br />al massimo (" & Doppie.ToString & "/" & MaxDoppie.ToString & ")", Master)
        'End If

        Return Ok
    End Function

    Public Function Salva(grdP As GridView) As String()
        Dim grdPartite As GridView = grdP
        Dim Ritorno() As String = EffettuaControlli(grdPartite)
        Dim Speciale As Boolean

        If Ritorno.Length = 0 Then
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim gg As Integer

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    Speciale = True
                    gg = DatiGioco.GiornataSpeciale + 100
                Else
                    Speciale = False
                    gg = DatiGioco.Giornata
                End If

                Sql = "Delete From Pronostici Where "
                Sql += "Anno=" & DatiGioco.AnnoAttuale & " And "
                Sql += "Giornata=" & gg & " And "
                Sql += "CodGiocatore=" & codGiocatore
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Delete From FilsRouge Where "
                Sql += "Anno=" & DatiGioco.AnnoAttuale & " And "
                Sql += "Giornata=" & gg & " And "
                Sql +="CodGiocatore=" & codGiocatore
                Db.EsegueSql(ConnSQL, Sql)

                Dim chk1 As CheckBox
                Dim chkX As CheckBox
                Dim chk2 As CheckBox
                Dim chkFR As CheckBox
                Dim txtRis As TextBox
                Dim AppoSegni As String
                Dim Risultato As String
                Dim Testo As String
                Dim Riga As String
                Dim gMail As New GestioneMail
                Dim Segno As String
                Dim FilRouge As Integer = -1

                Testo = "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & ">"
                Riga = "Partita;;Casa;;Fuori;Segno;Risultato;FR;"

                Testo += gMail.ConverteTestoInRigaTabella(Riga, True)
                For i As Integer = 0 To grdPartite.Rows.Count - 1
                    Segno = ""

                    chk1 = DirectCast(grdPartite.Rows(i).FindControl("chk1"), CheckBox)
                    chkX = DirectCast(grdPartite.Rows(i).FindControl("chkX"), CheckBox)
                    chk2 = DirectCast(grdPartite.Rows(i).FindControl("chk2"), CheckBox)
                    chkFR = DirectCast(grdPartite.Rows(i).FindControl("chkFR"), CheckBox)

                    If chk1.Checked = True Then Segno += "1"
                    If chkX.Checked = True Then Segno += "X"
                    If chk2.Checked = True Then Segno += "2"

                    txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)

                    Risultato = txtRis.Text.Trim
                    If Risultato = "" Then
                        Risultato = TornaRisultatoRandom(Segno)

                        txtRis.Text = Risultato
                    End If

                    AppoSegni = IIf(chk1.Checked = True, "1", "") & IIf(chkX.Checked = True, "X", "") & IIf(chk2.Checked = True, "2", "")

                    Sql = "Insert Into Pronostici Values ("
                    Sql += " " & DatiGioco.AnnoAttuale & ", "
                    Sql += " " & gg & ", "
                    Sql += " " & codGiocatore & ", "
                    Sql += " " & i + 1 & ", "
                    Sql += "'" & AppoSegni & "', "
                    Sql += "'" & Risultato & "' "
                    Sql +=")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Select * From Schedine Where "
                    Sql += "Anno=" & DatiGioco.AnnoAttuale & " And "
                    Sql += "Giornata=" & gg & " And "
                    Sql +="Partita=" & i + 1
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Riga = (i + 1).ToString & ";" & gMail.RitornaImmagineSquadra(Rec("SquadraCasa").Value) & ";" & Rec("SquadraCasa").Value & ";" & gMail.RitornaImmagineSquadra(Rec("SquadraFuori").Value) & ";" & Rec("SquadraFuori").Value & ";" & AppoSegni & ";" & Risultato & ";"
                    Rec.Close()

                    If chkFR.Checked = True Then
                        If FilRouge = -1 Then
                            FilRouge = i + 1
                            Riga += gMail.RitornaImmagineFilRouge() & ";"
                        End If
                    Else
                        Riga += ";"
                    End If

                    Testo += gMail.ConverteTestoInRigaTabella(Riga)
                Next
                Testo += "</table> "

                Sql = "Insert Into FilsRouge Values ("
                Sql += " " & DatiGioco.AnnoAttuale & ", "
                Sql += " " & gg & ", "
                Sql += " " & codGiocatore & ", "
                Sql += " " & FilRouge & " "
                Sql +=")"
                Db.EsegueSql(ConnSQL, Sql)

                If gg > 100 Then gg -= 100
                gMail.InviaMailColonnaGiocata(codGiocatore, Testo, gg, Speciale)

                gMail = Nothing
            End If

            Db = Nothing

            ReDim Preserve Ritorno(0)
            If Speciale = True Then
                Ritorno(0) = "Colonna speciale salvata"
            Else
                Ritorno(0) = "Colonna salvata"
            End If
        End If

        Return Ritorno
    End Function

    Public Sub GestioneRigaGridview(e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            ' e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As Label = DirectCast(e.Row.FindControl("TxtCasa"), Label)
            Dim TxtFuori As Label = DirectCast(e.Row.FindControl("TxtFuori"), Label)
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim ImgJolly As Image = DirectCast(e.Row.FindControl("imgJolly"), Image)
            Dim chk1 As CheckBox = DirectCast(e.Row.FindControl("chk1"), CheckBox)
            Dim chkX As CheckBox = DirectCast(e.Row.FindControl("chkX"), CheckBox)
            Dim chk2 As CheckBox = DirectCast(e.Row.FindControl("chk2"), CheckBox)
            Dim chkFR As CheckBox = DirectCast(e.Row.FindControl("chkFR"), CheckBox)
            Dim txtRis As TextBox = DirectCast(e.Row.FindControl("txtRisultato"), TextBox)
            Dim r As Integer
            If IsNumeric(e.Row.Cells(0).Text) Then
                r = Val(e.Row.Cells(0).Text)
            Else
                r = -1
            End If
            Dim numRiga As Integer = r
            Dim lblSerieC As Label = DirectCast(e.Row.FindControl("lblSerieC"), Label)
            Dim lblSerieF As Label = DirectCast(e.Row.FindControl("lblSerieF"), Label)
            Dim lblStat As Label = DirectCast(e.Row.FindControl("lblStat"), Label)
            Dim divvettoC As HtmlGenericControl = DirectCast(e.Row.FindControl("divPrecCasa"), HtmlGenericControl)
            Dim divvettoF As HtmlGenericControl = DirectCast(e.Row.FindControl("divPrecFuori"), HtmlGenericControl)
            Dim Db As New GestioneDB
            Dim gm As New GestioneMail

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim gg As Integer

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    gg = DatiGioco.GiornataSpeciale + 100
                Else
                    gg = DatiGioco.Giornata
                End If

                chkFR.Checked = False
                Sql = "Select A.* From FilsRouge A "
                Sql += "Where A.Anno=" & DatiGioco.AnnoAttuale & " And "
                Sql += "A.Giornata=" & gg & " And "
                Sql +="A.CodGiocatore=" & codGiocatore
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    Dim fr As Integer = Rec("FilRouge").Value
                    If fr = numRiga Then
                        chkFR.Checked = True
                    End If
                End If
                Rec.Close()

                Sql = "Select A.* From Schedine A " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "A.Giornata=" & gg & " And " & _
                    "A.Partita=" & numRiga
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    TxtCasa.Text = Rec("SquadraCasa").Value
                    TxtFuori.Text = Rec("SquadraFuori").Value
                Else
                    TxtCasa.Text = ""
                    TxtFuori.Text = ""
                End If
                Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " & _
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtCasa.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieC.Text = Rec("Descrizione").Value
                Else
                    lblSerieC.Text = ""
                End If
                Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " & _
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtFuori.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieF.Text = Rec("Descrizione").Value
                Else
                    lblSerieF.Text = ""
                End If
                Rec.Close()

                Dim Path As String

                Path = gm.RitornaImmagineSquadra(TxtCasa.Text, True)
                ImgCasa.ImageUrl = Path
                ImgCasa.Visible = True

                Path = gm.RitornaImmagineSquadra(TxtFuori.Text, True)
                ImgFuori.ImageUrl = Path
                ImgFuori.Visible = True

                chk1.Checked = False
                chkX.Checked = False
                chk2.Checked = False
                txtRis.Text = ""

                Sql = "Select A.* From Pronostici A " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "A.Giornata=" & gg & " And " & _
                    "A.Partita=" & numRiga & " And " & _
                    "A.CodGiocatore=" & codGiocatore
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    txtRis.Text = "" & Rec("Risultato").Value

                    Dim Segni As String = "" & Rec("Segni").Value

                    Segni = Segni.ToUpper.Trim

                    If Segni.IndexOf("1") > -1 Then
                        chk1.Checked = True
                    End If
                    If Segni.IndexOf("X") > -1 Then
                        chkX.Checked = True
                    End If
                    If Segni.IndexOf("2") > -1 Then
                        chk2.Checked = True
                    End If
                End If
                Rec.Close()

                Path = "App_Themes/Standard/Images/Icone/Jolly.png"

                If numRiga = DatiGioco.PartitaJolly Then
                    ImgJolly.ImageUrl = Path
                    ImgJolly.Visible = True
                Else
                    ImgJolly.ImageUrl = ""
                    ImgJolly.Visible = False
                End If


                divvettoC.InnerHtml = CreaPrecedenti(ConnSQL, Db, Rec, TxtCasa.Text)
                divvettoC.Attributes.Add("Style", "display:none")

                divvettoF.InnerHtml = CreaPrecedenti(ConnSQL, Db, Rec, TxtFuori.Text)
                divvettoF.Attributes.Add("Style", "display:none")

                ConnSQL.Close()

                lblStat.Text = ""

                ' e.Row.Cells(0).Visible = False
            End If

            Db = Nothing
        End If
    End Sub

    Private Function CreaPrecedenti(COnnSQL As Object, DB As GestioneDB, Rec As Object, Squadra As String) As String
        Dim Sql As String = ""
        Dim Colore As String = ""

        Sql = "Select Top 5 * From PartiteGiornata "
        Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And "
        Sql &= "Giornata<=" & DatiGioco.Giornata & " And "
        Sql &= "(Upper(Ltrim(Rtrim(Casa))) = '" & SistemaTestoPerDB(Squadra) & "' Or Upper(Ltrim(Rtrim(Fuori))) = '" & SistemaTestoPerDB(Squadra) & "') "
        Sql &= "Order By Giornata Desc"
        Rec = DB.LeggeQuery(COnnSQL, Sql)
        Dim Tabella As New StringBuilder
        Tabella.Append("<table border=""0"" width=""99%"">")
        Tabella.Append("<tr>")
        Tabella.Append("<td style=""text-align: center; font-weight:bold;"">")
        Tabella.Append("G.")
        Tabella.Append("</td>")
        Tabella.Append("<td style=""text-align: center; font-weight:bold;"">")
        Tabella.Append("Casa")
        Tabella.Append("</td>")
        Tabella.Append("<td style=""text-align: center; font-weight:bold;"">")
        Tabella.Append("Fuori")
        Tabella.Append("</td>")
        Tabella.Append("<td style=""text-align: center; font-weight:bold;"">")
        Tabella.Append("Ris.")
        Tabella.Append("</td>")
        Tabella.Append("</tr>")
        Do Until Rec.Eof
            If Rec("Casa").Value.ToString.Trim.ToUpper = Squadra Then
                If Rec("Ris1").Value > Rec("Ris2").Value Then
                    Colore = "#0a0"
                Else
                    If Rec("Ris1").Value < Rec("Ris2").Value Then
                        Colore = "#a00"
                    Else
                        Colore = "#000"
                    End If
                End If
            Else
                If Rec("Ris1").Value > Rec("Ris2").Value Then
                    Colore = "#a00"
                Else
                    If Rec("Ris1").Value < Rec("Ris2").Value Then
                        Colore = "#0a0"
                    Else
                        Colore = "#000"
                    End If
                End If
            End If

            Tabella.Append("<tr>")
            Tabella.Append("<td style=""color: " & Colore & """> ")
            Tabella.Append(Rec("Giornata").Value)
            Tabella.Append("</td>")
            Tabella.Append("<td style=""color:" & Colore & """>")
            Tabella.Append(Rec("Casa").Value)
            Tabella.Append("</td>")
            Tabella.Append("<td style=""color:" & Colore & """>")
            Tabella.Append(Rec("Fuori").Value)
            Tabella.Append("</td>")
            Tabella.Append("<td style=""color: " & Colore & """>")
            Tabella.Append(Rec("Ris1").Value & "-" & Rec("Ris2").Value)
            Tabella.Append("</td>")
            Tabella.Append("</tr>")

            Rec.MoveNext
        Loop
        Tabella.Append("</table>")
        Rec.Close

        Return Tabella.ToString
    End Function

    Public Function UsaTappo(grdP As GridView) As String
        Dim grdPartite As GridView = grdP
        Dim Db As New GestioneDB
        Dim Ritorno As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Colonna As String = ""

            Sql = "Select Colonna From Tappi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codGiocatore
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Ritorno = "Colonna tappo non inserita"
            Else
                Colonna = Rec("Colonna").Value
            End If
            Rec.Close()

            If Colonna <> "" Then
                Dim chk1 As CheckBox
                Dim chkX As CheckBox
                Dim chk2 As CheckBox
                Dim txtRis As TextBox
                Dim gMail As New GestioneMail
                Dim Segno As String

                For i As Integer = 0 To grdPartite.Rows.Count - 1
                    Segno = ""

                    chk1 = DirectCast(grdPartite.Rows(i).FindControl("chk1"), CheckBox)
                    chkX = DirectCast(grdPartite.Rows(i).FindControl("chkX"), CheckBox)
                    chk2 = DirectCast(grdPartite.Rows(i).FindControl("chk2"), CheckBox)

                    If chk1.Checked = True Then Segno += "1"
                    If chkX.Checked = True Then Segno += "X"
                    If chk2.Checked = True Then Segno += "2"

                    txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)

                    txtRis.Text = TornaRisultatoRandom(Segno)

                    Select Case Mid(Colonna, i + 1, 1)
                        Case "1"
                            chk1.Checked = True
                            chkX.Checked = False
                            chk2.Checked = False
                        Case "X"
                            chk1.Checked = False
                            chkX.Checked = True
                            chk2.Checked = False
                        Case "2"
                            chk1.Checked = False
                            chkX.Checked = False
                            chk2.Checked = True
                    End Select
                Next

                Ritorno = "Colonna tappo impostata. Il contatore dei tappi non si incrementerà"
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Public Function UsaRandom(grdP As GridView) As String
        Dim grdPartite As GridView = grdP
        Dim chk1 As CheckBox
        Dim chkX As CheckBox
        Dim chk2 As CheckBox
        Dim gMail As New GestioneMail
        Dim Colonna As String
        Dim Segni As String = "1X2"
        Dim X As Integer
        Dim txtRis As TextBox

        For i As Integer = 0 To grdPartite.Rows.Count - 1
            chk1 = DirectCast(grdPartite.Rows(i).FindControl("chk1"), CheckBox)
            chkX = DirectCast(grdPartite.Rows(i).FindControl("chkX"), CheckBox)
            chk2 = DirectCast(grdPartite.Rows(i).FindControl("chk2"), CheckBox)

            Randomize()
            X = Int(Rnd(1) * 3) + 1
            Colonna = Mid(Segni, X, 1)

            Select Case Colonna
                Case "1"
                    chk1.Checked = True
                    chkX.Checked = False
                    chk2.Checked = False
                Case "X"
                    chk1.Checked = False
                    chkX.Checked = True
                    chk2.Checked = False
                Case "2"
                    chk1.Checked = False
                    chkX.Checked = False
                    chk2.Checked = True
            End Select

            txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)

            txtRis.Text = TornaRisultatoRandom(Colonna)
        Next

        Return "Segni random impostati"
    End Function

    Public Function RandomRis(grdP As GridView) As String
        Dim grdPartite As GridView = grdP
        Dim chkSegno As CheckBox
        Dim txtRis As TextBox
        Dim Segno As String
        Dim gMail As New GestioneMail

        For i As Integer = 0 To grdPartite.Rows.Count - 1
            Segno = ""

            ' Sondaggio N° 1 - I risultati devono essere congruenti con il segno
            chkSegno = DirectCast(grdPartite.Rows(i).FindControl("chk1"), CheckBox)
            If chkSegno.Checked = True Then Segno += "1"
            chkSegno = DirectCast(grdPartite.Rows(i).FindControl("chkX"), CheckBox)
            If chkSegno.Checked = True Then Segno += "X"
            chkSegno = DirectCast(grdPartite.Rows(i).FindControl("chk2"), CheckBox)
            If chkSegno.Checked = True Then Segno += "2"
            ' Sondaggio N° 1 - I risultati devono essere congruenti con il segno

            txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)

            txtRis.Text = TornaRisultatoRandom(Segno)
        Next

        Return "Risultati random impostati"
    End Function

    Public Function SelezionaPartita(sender As Object) As String
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim txtCasa As Label = DirectCast(row.FindControl("txtCasa"), Label)
        Dim txtFuori As Label = DirectCast(row.FindControl("txtFuori"), Label)
        Dim txtRis As TextBox = DirectCast(row.FindControl("txtRisultato"), TextBox)
        Dim chk1 As CheckBox = DirectCast(row.FindControl("chk1"), CheckBox)
        Dim chkX As CheckBox = DirectCast(row.FindControl("chkX"), CheckBox)
        Dim chk2 As CheckBox = DirectCast(row.FindControl("chk2"), CheckBox)
        Dim Partita As Integer = row.Cells(0).Text

        Dim Testo As String = ""

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Punti1 As Integer = 0
            Dim Giocate1 As Integer = 0
            Dim Vinte1 As Integer = 0
            Dim Pareggiate1 As Integer = 0
            Dim Perse1 As Integer = 0
            Dim gFatti1 As Integer = 0
            Dim gSubiti1 As Integer = 0
            Dim Posizione1 As Integer = 0

            Dim gg As Integer

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Sql = "Select A.*, B.Descrizione From Classifiche A " & _
                "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(txtCasa.Text.ToUpper.Trim) & "'"
            Rec = DB.LeggeQuery(COnnSQL, Sql)
            If Rec.Eof = False Then
                Punti1 = Rec("Punti").Value
                Giocate1 = Rec("Giocate").Value
                Vinte1 = Rec("vCasa").Value
                Pareggiate1 = Rec("nCasa").Value
                Perse1 = Rec("pCasa").Value
                gFatti1 = Rec("gFatti").Value
                gSubiti1 = Rec("gSubiti").Value
                Posizione1 = Rec("progressivo").Value
            End If
            Rec.Close()

            Dim Punti2 As Integer = 0
            Dim Giocate2 As Integer = 0
            Dim Vinte2 As Integer = 0
            Dim Pareggiate2 As Integer = 0
            Dim Perse2 As Integer = 0
            Dim gFatti2 As Integer = 0
            Dim gSubiti2 As Integer = 0
            Dim Posizione2 As Integer = 0
            Dim idSerie As Integer = 0

            Sql = "Select A.*, B.Descrizione From Classifiche A " & _
                "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(txtFuori.Text.ToUpper.Trim) & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Punti2 = Rec("Punti").Value
                Giocate2 = Rec("Giocate").Value
                Vinte2 = Rec("vFuori").Value
                Pareggiate2 = Rec("nFuori").Value
                Perse2 = Rec("pFuori").Value
                gFatti2 = Rec("gFatti").Value
                gSubiti2 = Rec("gSubiti").Value
                Posizione2 = Rec("progressivo").Value
                idSerie = Rec("idSerie").Value
            End If
            Rec.Close()

            Dim q1 As Single
            Dim qX As Single
            Dim q2 As Single

            Dim q1x As Single
            Dim q12 As Single
            Dim qx2 As Single

            Dim sRis() As String = {"0-0", "0-1", "0-2", "0-3", "0-4", "1-0", "1-1", "1-2", "1-3", "1-4", "2-0", "2-1", "2-2", "2-3", "2-4", "3-0", "3-1", "3-2", "3-3", "3-4", "4-0", "4-1", "4-2", "4-3", "4-4", "Altro"}
            Dim Quote(25) As Single

            Sql = "Select * From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And Partita=" & Partita
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                For i As Integer = 3 To 28
                    Quote(i - 3) = Rec(i).Value
                Next

                q1 = Rec("r1").Value
                qX = Rec("rX").Value
                q2 = Rec("r2").Value

                q1x = Rec("r1x").Value
                q12 = Rec("r12").Value
                qx2 = Rec("rx2").Value
            End If
            Rec.Close()

            Dim posSquadra() As Integer
            Dim ClassSquadra() As String = {}
            Dim ClassPunti() As Integer
            Dim ClassQuante As Integer = 0
            Dim VecchiPunti As Integer = 0
            Dim Conta As Integer = 0

            Sql = "Select * From Classifiche Where idSerie=" & idSerie & " Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                ClassQuante += 1
                ReDim Preserve posSquadra(ClassQuante)
                ReDim Preserve ClassSquadra(ClassQuante)
                ReDim Preserve ClassPunti(ClassQuante)

                If VecchiPunti <> Rec("Punti").Value Then
                    Conta = Rec("Progressivo").Value
                    VecchiPunti = Rec("Punti").Value
                End If

                posSquadra(ClassQuante) = Conta
                ClassSquadra(ClassQuante) = Rec("Squadra").Value
                ClassPunti(ClassQuante) = Rec("Punti").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From ClassificheSerie Where idSerie=" & idSerie
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim NomeSerie As String
            If Rec.Eof = False Then
                NomeSerie = Rec("Descrizione").Value.ToString.Trim.ToUpper
            End If
            Rec.Close()

            Dim Path1 As String
            Dim Path2 As String

            Dim gM As New GestioneMail

            Path1 = gM.RitornaImmagineSquadra(txtCasa.Text, True)
            If File.Exists(Server.MapPath(Path1)) = True Then
            Else
                Path1 = "App_Themes/Standard/Images/Stemmi/Niente.png"
            End If

            Path2 = gM.RitornaImmagineSquadra(txtFuori.Text, True)
            If File.Exists(Server.MapPath(Path2)) = True Then
            Else
                Path2 = "App_Themes/Standard/Images/Stemmi/Niente.png"
            End If

            gM = Nothing

            Dim SpanTestoSquadra As String = "font-family: Verdana; font-size: 12px; font-weight: bold; color: #AA0000;"
            Dim SpanTestoTitolo As String = "font-family: Verdana; font-size: 10px; font-weight: bold; color: #0000AA;"
            Dim SpanTesto As String = "font-family: Verdana; font-size: 10px; color: #000000;"

            Testo += "<table width=99%>"

            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<table>"

            Testo += "<tr style=""vertical-align:top;"">"
            Testo += "<td>"
            Testo += "<img src=""" & Path1 & """ width=35px height=35px />"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoSquadra & """>" & txtCasa.Text & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Pos.</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Posizione1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Pts.</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Punti1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>V. in casa</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Vinte1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Par. in casa</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Pareggiate1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Perse in casa</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Perse1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>G. fatti</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & gFatti1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>G. subiti</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & gSubiti1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            Testo += "<tr>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "</tr>"

            Testo += "<tr style=""vertical-align:top;"">"
            Testo += "<td>"
            Testo += "<img src=""" & Path2 & """ width=35px height=35px />"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoSquadra & """>" & txtFuori.Text & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Pos.</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Posizione2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Pts.</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Punti2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>V. fuori.</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Vinte2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>Par. fuori</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Pareggiate2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>P. fuori</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & Perse2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>G. fatti</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & gFatti2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoTitolo & """>G. subiti</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & gSubiti2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            Testo += "<tr>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "</tr>"

            Dim Colore As String

            If chk1.Checked = True And chkX.Checked = False And chk2.Checked = False Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr style=""vertical-align:top;"">"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>1</span>"
            Testo += "</td>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTesto & """>" & q1 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            If chk1.Checked = False And chkX.Checked = True And chk2.Checked = False Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>X</span>"
            Testo += "</td>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTesto & """>" & qX & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            If chk1.Checked = False And chkX.Checked = False And chk2.Checked = True Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>2</span>"
            Testo += "</td>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTesto & """>" & q2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            Testo += "<tr>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "<td height=15px>"
            Testo += "</td>"
            Testo += "</tr>"

            If chk1.Checked = True And chkX.Checked = True And chk2.Checked = False Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>1X</span>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTesto & """>" & q1x & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            If chk1.Checked = True And chkX.Checked = False And chk2.Checked = True Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>12</span>"
            Testo += "</td>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTesto & """>" & q12 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            If chk1.Checked = False And chkX.Checked = True And chk2.Checked = True Then
                Colore = "#00FFFF"
            Else
                Colore = "#FFFFFF"
            End If
            Testo += "<tr>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTestoTitolo & """>X2</span>"
            Testo += "</td>"
            Testo += "<td bgcolor=""" & Colore & """>"
            Testo += "<span style=""" & SpanTesto & """>" & qx2 & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            Testo += "</table>"
            Testo += "</td>"

            Testo += "<td width=25px>"
            Testo += "</td>"

            Testo += "<td>"
            Testo += "<table>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoSquadra & """>QUOTE</span>"
            Testo += "</td>"
            Testo += "</tr>"
            For i As Integer = 0 To 25
                If sRis(i).Trim = txtRis.Text.Trim Then
                    Colore = "#00FFFF"
                Else
                    Colore = "#FFFFFF"
                End If

                Testo += "<tr>"
                Testo += "<td bgcolor=""" & Colore & """>"
                Testo += "<span style=""" & SpanTestoTitolo & """>" & sRis(i) & "</span>"
                Testo += "</td>"
                Testo += "<td bgcolor=""" & Colore & """>"
                Testo += "<span style=""" & SpanTesto & """>" & Quote(i) & "</span>"
                Testo += "</td>"
                Testo += "</tr>"
            Next
            Testo += "</table>"
            Testo += "</td>"

            Testo += "<td width=25px>"
            Testo += "</td>"

            Testo += "<td>"
            Testo += "<table>"
            Testo += "<tr>"
            Testo += "<td>"
            Testo += "</td>"
            Testo += "<td>"
            Testo += "<span style=""" & SpanTestoSquadra & """>CLASSIFICA<br />" & NomeSerie & "</span>"
            Testo += "</td>"
            Testo += "</tr>"

            For i As Integer = 1 To ClassQuante
                If ClassSquadra(i) = txtCasa.Text Then
                    Colore = "#FF0000"
                Else
                    If ClassSquadra(i) = txtFuori.Text Then
                        Colore = "#00FF00"
                    Else
                        Colore = "#FFFFFF"
                    End If
                End If


                Testo += "<tr>"
                Testo += "<td bgcolor=""" & Colore & """>"
                Testo += "<span style=""" & SpanTesto & """>" & posSquadra(i) & "</span>"
                Testo += "</td>"
                Testo += "<td bgcolor=""" & Colore & """>"
                Testo += "<span style=""" & SpanTesto & """>" & ClassSquadra(i) & "</span>"
                Testo += "</td>"
                Testo += "<td bgcolor=""" & Colore & """>"
                Testo += "<span style=""" & SpanTesto & """>" & ClassPunti(i) & "</span>"
                Testo += "</td>"
                Testo += "</tr>"
            Next
            Testo += "</table>"
            Testo += "</td>"

            Testo += "<tr>"

            Testo += "</table>"

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Testo
    End Function
End Class
