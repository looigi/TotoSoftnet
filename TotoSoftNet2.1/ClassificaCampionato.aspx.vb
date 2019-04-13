Imports System.IO

Public Class ClassificaCampionato
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Dim QualeGiorn As Integer = DatiGioco.Giornata

            If GiornataEventoCreazione = -1 Then
                Dim Db As New GestioneDB

                If Db.LeggeImpostazioniDiBase() = True Then
                    Dim ConnSQL As Object = Db.ApreDB()
                    Dim Rec As Object = CreateObject("ADODB.Recordset")
                    Dim Sql As String = "Select * From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " And Descrizione='CREAZIONE SCONTRI DIRETTI'"

                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    GiornataEventoCreazione = Rec("Giornata").Value
                    Rec.Close()
                End If

                Db = Nothing
            End If

            If DatiGioco.StatoConcorso <> ValoriStatoConcorso.Chiuso Then
                hdnGiorn.Value = (QualeGiorn - 1) - GiornataEventoCreazione
            Else
                hdnGiorn.Value = QualeGiorn - GiornataEventoCreazione
            End If

            CreaTabella()

            Dim sd As New ScontriDiretti

            hdnGiornata.Value = DatiGioco.Giornata
            hdnMaxGiornate.Value = sd.TornaMaxGiornateScontriDiretti
            hdnMinGiornate.Value = sd.TornaMinGiornateScontriDiretti
            If Val(hdnGiorn.Value) < 0 Then hdnGiorn.Value = 0
            If Val(hdnGiornata.Value) < 0 Then hdnGiornata.Value = 0

            sd = Nothing

            If QualeGiorn > hdnMaxGiornate.Value Then
                QualeGiorn = hdnMaxGiornate.Value
            Else
                If QualeGiorn < 0 Then
                    QualeGiorn = 0
                End If
            End If
            hdnGiornata.Value = QualeGiorn

            VisualizzaGiornata(QualeGiorn)
        End If
    End Sub

    Private Sub VisualizzaGiornata(Giornata As Integer)
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select B.Giocatore As Casa, C.Giocatore As Fuori, A.RisultatoUfficiale, A.RisultatoReale From ScontriDiretti A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocatoreCasa=B.CodGiocatore " & _
            "Left Join Giocatori C On A.Anno=C.Anno And A.GiocatoreFuori=C.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & Giornata & " " & _
            "Order By A.Partita"
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing

        Dim g As Integer = Giornata - GiornataEventoCreazione
        If g < 0 Then g = 0

        lblGiornataVis.Text = "Giornata " & g

        'If grdPartite.Rows.Count > 0 Then
        '    divGiornata.Visible = True
        'Else
        '    divGiornata.Visible = False
        'End If
    End Sub

    Private Sub CreaTabella()
        Dim Db As New GestioneDB

        If Val(hdnGiorn.Value) < 0 Then hdnGiorn.Value = "0"

        lblGiornata.Text = "Giornata: " & hdnGiorn.Value

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim StringaIn As String = ""

            Db.EsegueSql(ConnSQL, "Delete From PosizioniCampionato Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiorn.Value)

            Dim sDiretti As New ScontriDiretti

            sDiretti.CreaTabellaClassificaScontriDiretti((hdnGiorn.Value) + GiornataEventoCreazione, Session("CodGiocatore"))

            sDiretti = Nothing

            Dim Gr As New Griglie
            Dim Sql As String = ""

            Sql = "Select A.CodGiocatore From AppoScontriDiretti A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' " & _
                "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                StringaIn += Rec("CodGiocatore").Value & ", "

                Rec.MoveNext()
            Loop
            Rec.Close()

            Session("VecchiPunti") = 0
            Session("Posizione") = 0

            Sql = "Select 0, A.Giocatore, Punti, Giocate,Media,GFatti,GSubiti,Differenza From AppoScontriDiretti A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' And A.CodGiocVisua = 1 " & _
                "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
            Gr.ImpostaCampi(Sql, grdClassifica, True)

            ' Prende i giocatori che non hanno punti in classifica
            Sql = ""
            If StringaIn.Length > 0 Then
                StringaIn = Mid(StringaIn, 1, StringaIn.Length - 2)
                Sql = "Select 0, Giocatore, 0,0,0,0,0,0 From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In (" & StringaIn & ") And Cancellato='N' Order By Giocatore"
            Else
                If StringaIn = "" Then
                    Sql = "Select 0, Giocatore, 0,0,0,0,0,0 From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
                End If
            End If
            If Sql <> "" Then
                Gr.AggiungeValori(Sql)
            End If
            ' Prende i giocatori che non hanno punti in classifica
            Gr.VisualizzaValori(grdClassifica)
            Gr = Nothing

            Session("Posizione") = ""
            Session("VecchiPunti") = ""

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub grdClassifica_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdClassifica.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatore As String = e.Row.Cells(2).Text
            Dim ImgGioc As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)

            ImgGioc.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatore)
            ImgGioc.DataBind()

            Dim Punti As String = Val(e.Row.Cells(4).Text)

            If Session("VecchiPunti") <> Punti Then
                Session("Posizione") += 1
                Session("VecchiPunti") = Punti
            End If

            e.Row.Cells(0).Text = Session("Posizione")

            ' Dim lblMotto As Label = DirectCast(e.Row.FindControl("lblmotto"), Label)
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

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim Posizione As Integer = e.Row.Cells(0).Text

                ' Scrive la posizione del giocatore
                Sql = "Insert Into PosizioniCampionato Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    " " & hdnGiorn.Value & ", " & _
                    " " & idGiocatore & ", " & _
                    " " & Posizione & " " & _
                    ")"
                Try
                    Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
                Catch ex As Exception

                End Try
                ' Scrive la posizione del giocatore

                ' Visualizza la differenza di posizione
                Dim ImgDiff As Image = DirectCast(e.Row.FindControl("imgDifferenza"), Image)
                ' Dim lblDiff As Label = DirectCast(e.Row.FindControl("lblDiff"), Label)
                Dim lblDiff As String
                Dim iconcina As String

                Sql = "Select * From PosizioniCampionato Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "Giornata=" & hdnGiorn.Value - 1 & " And " & _
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

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NomeGiocatoreC As String = e.Row.Cells(1).Text
            Dim ImgGiocC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)

            Dim NomeGiocatoreF As String = e.Row.Cells(3).Text
            Dim ImgGiocF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)

            If NomeGiocatoreC = Session("Nick") Or NomeGiocatoreF = Session("Nick") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreC & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    If Rec("Cancellato").Value = "S" Then
                        e.Row.Cells(1).Text = "Riposo"
                        NomeGiocatoreC = "Riposo"
                    End If
                End If
                Rec.Close()

                Sql = "Select Cancellato From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & NomeGiocatoreF & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    If Rec("Cancellato").Value = "S" Then
                        e.Row.Cells(3).Text = "Riposo"
                        NomeGiocatoreF = "Riposo"
                    End If
                End If
                Rec.Close()

                ConnSQL.Close()
            End If

            Db = Nothing

            ImgGiocC.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatoreC)
            ImgGiocC.DataBind()

            ImgGiocF.ImageUrl = RitornaImmagine(Server.MapPath("."), NomeGiocatoreF)
            ImgGiocF.DataBind()

            If NomeGiocatoreC = "" Or NomeGiocatoreC = "&nbsp;" Then
                e.Row.Cells(1).Text = "Riposo"
                NomeGiocatoreC = "Riposo"
            End If

            If NomeGiocatoreF = "" Or NomeGiocatoreF = "&nbsp;" Then
                e.Row.Cells(3).Text = "Riposo"
                NomeGiocatoreF = "Riposo"
            End If
        End If
    End Sub

    Protected Sub cmdGIndietro_Click(sender As Object, e As EventArgs) Handles cmdGIndietro.Click
        Dim Giornata As Integer = hdnGiornata.Value - 1

        If Giornata >= Val(hdnMinGiornate.Value) Then
            VisualizzaGiornata(Giornata)
            hdnGiornata.Value = Giornata
        End If
    End Sub

    Protected Sub cmdGAvanti_Click(sender As Object, e As EventArgs) Handles cmdGAvanti.Click
        Dim Giornata As Integer = hdnGiornata.Value + 1

        If Giornata <= Val(hdnMaxGiornate.Value) Then
            VisualizzaGiornata(Giornata)
            hdnGiornata.Value = Giornata
        End If
    End Sub

    Protected Sub cmdGIndietroC_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        Dim Giornata As Integer = hdnGiorn.Value - 1

        If Giornata >= Val(hdnMinGiornate.Value) - GiornataEventoCreazione Then
            hdnGiorn.Value = Giornata
            CreaTabella()
        End If
    End Sub

    Protected Sub cmdGAvantiC_Click(sender As Object, e As EventArgs) Handles cmdAvanti.Click
        Dim Giornata As Integer = hdnGiorn.Value + 1

        If Giornata <= Val(hdnMaxGiornate.Value) - GiornataEventoCreazione Then
            hdnGiorn.Value = Giornata
            CreaTabella()
        End If
    End Sub
End Class