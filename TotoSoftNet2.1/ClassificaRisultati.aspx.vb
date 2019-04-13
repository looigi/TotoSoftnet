Imports System.IO

Public Class ClassificaRisultati
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If DatiGioco.StatoConcorso <> ValoriStatoConcorso.Chiuso Then
                hdnGiornata.Value = DatiGioco.Giornata - 1
            Else
                hdnGiornata.Value = DatiGioco.Giornata
            End If
            If hdnGiornata.Value < 0 Then hdnGiornata.Value = 0

            DisegnaClassifica()
        End If
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
            Dim PuntiRis As New DataColumn("PuntiRis")
            Dim UltRis As New DataColumn("UltRis")
            Dim Preced As New DataColumn("Precedente")
            Dim Gioc As New DataColumn("Giocate")
            Dim Media As New DataColumn("Media")
            Dim Posizione As Integer = 0
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            Dim UltimoRisultato As Single
            Dim Precedente As Single
            Dim vMedia As Single
            Dim Giocate As Single
            Dim TotPunti As Single
            Dim VecchiPunti As Single

            dttTabella.Columns.Add(Posiz)
            dttTabella.Columns.Add(Giocatore)
            dttTabella.Columns.Add(PuntiRis)
            dttTabella.Columns.Add(UltRis)
            dttTabella.Columns.Add(Preced)
            dttTabella.Columns.Add(Gioc)
            dttTabella.Columns.Add(Media)

            'Dim sc As New StringheClassifiche

            'Sql = sc.RitornaStringaClassificaGenerale

            Sql = "Select Sum(PuntiRisultati) As PuntiRisultati, B.CodGiocatore, B.Giocatore From Risultati A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Concorso <= " & hdnGiornata.Value & " And B.Cancellato='N' " & _
                "Group By B.CodGiocatore, B.Giocatore " & _
                "Order By PuntiRisultati Desc"

            Dim StringaIn As String = ""

            Db.EsegueSql(ConnSQL, "Delete From PosizioniRisultati Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value)

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                StringaIn += Rec("CodGiocatore").Value & ", "

                Sql = "Select Count(Giornata) From Pronostici " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value & " " & _
                    "And Partita=1 And Giornata<=" & hdnGiornata.Value
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

                Sql = "Select PuntiRisultati As TotPunti From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & hdnGiornata.Value & " And CodGiocatore=" & Rec("CodGiocatore").Value
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

                Sql = "Select PuntiRisultati As TotPunti From Risultati Where Anno=" & DatiGioco.AnnoAttuale & " And Concorso=" & hdnGiornata.Value - 1 & " And CodGiocatore=" & Rec("CodGiocatore").Value
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

                riga = dttTabella.NewRow()
                riga(0) = Posizione
                riga(1) = Rec("Giocatore").Value
                riga(2) = FormattaNumeroConVirgola(Rec("PuntiRisultati").Value)
                riga(3) = FormattaNumeroConVirgola(UltimoRisultato)
                riga(4) = FormattaNumeroConVirgola(Precedente)
                riga(5) = Giocate
                riga(6) = FormattaNumeroConVirgola(vMedia)
                dttTabella.Rows.Add(riga)

                ' Scrive la posizione del giocatore
                Sql = "Insert Into PosizioniRisultati Values (" & _
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
                Posizione += 1

                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Do Until Rec.Eof
                    riga = dttTabella.NewRow()
                    riga(0) = Posizione
                    riga(1) = Rec("Giocatore").Value
                    riga(2) = 0
                    riga(3) = 0
                    riga(4) = 0
                    riga(5) = 0
                    riga(6) = 0
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
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(3).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(2).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            Dim lblMotto As Label = DirectCast(e.Row.FindControl("lblmotto"), Label)
            Dim g As New Giocatori
            lblMotto.CssClass = "etichettamotto"
            Dim idGiocatore As Integer = g.TornaIdGiocatore(NomeGiocatore)
            lblMotto.Text = MetteaCapo(g.TornaMottoGiocatore(idGiocatore), 25)
            If lblMotto.Text = "" Then
                lblMotto.Visible = False
            End If

            If idGiocatore = Session("CodGiocatore") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            Dim Immagine As String = RitornaImmagineSfondo(Server.MapPath("."), NomeGiocatore)

            If Immagine <> "" Then
                e.Row.Cells(3).Style.Add("background", "transparent url('" & Immagine & "')")
                e.Row.Cells(3).Style.Add("background-repeat", "no-repeat")
                e.Row.Cells(3).Style.Add("background-position", "center center")
                e.Row.Cells(3).Style.Add("background-size", "90% 90%")
                e.Row.Cells(3).Style.Add("opacity", "0.7")

                ' Mette il colore di sfondo simile a quello dell'immagine visualizzata
                Dim bm As System.Drawing.Bitmap = New System.Drawing.Bitmap(Server.MapPath(".") & "\" & Immagine)
                Dim colore As System.Drawing.Color = bm.GetPixel(1, bm.Height / 2)
                Dim colorello As String = "#" & Hex(colore.R) & Hex(colore.G) & Hex(colore.B)
                e.Row.Cells(3).Style.Add("background-color", colorello)
                bm.Dispose()
                ' Mette il colore di sfondo simile a quello dell'immagine visualizzata
            End If

            ' Visualizza la differenza di posizione
            Dim Sql As String
            Dim ImgDiff As Image = DirectCast(e.Row.FindControl("imgDifferenza"), Image)
            Dim lblDiff As Label = DirectCast(e.Row.FindControl("lblDiff"), Label)
            Dim Db As New GestioneDB
            Dim iconcina As String

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")

                Sql = "Select * From PosizioniRisultati Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & hdnGiornata.Value - 1 & " And " & _
                    "CodGiocatore=" & idGiocatore
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = True Then
                    iconcina = "App_Themes/Standard/Images/Icone/PosizioneUguale.png"
                    lblDiff.Text = ""
                Else
                    Dim PosAttuale As Integer = e.Row.Cells(0).Text
                    Dim PosPrima As Integer = Rec("Posizione").value

                    If PosAttuale > PosPrima Then
                        iconcina = "App_Themes/Standard/Images/Icone/PosizionePeggiore.png"
                        lblDiff.Text = "-" & PosAttuale - PosPrima
                    Else
                        If PosAttuale < PosPrima Then
                            iconcina = "App_Themes/Standard/Images/Icone/PosizioneMigliore.png"
                            lblDiff.Text = "+" & PosPrima - PosAttuale
                        Else
                            iconcina = "App_Themes/Standard/Images/Icone/PosizioneUguale.png"
                            lblDiff.Text = ""
                        End If
                    End If
                End If
                Rec.Close()

                ImgDiff.ImageUrl = iconcina
                ImgDiff.DataBind()

                ConnSQL.Close()
            End If

            Db = Nothing
            ' Visualizza la differenza di posizione

            g = Nothing

            e.Row.Cells(3).Visible = False
        End If
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