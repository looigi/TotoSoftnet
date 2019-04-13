Imports System.IO

Public Class ClassificaSpeciale
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            DisegnaClassifica()
        End If
    End Sub

    Private Sub DisegnaClassifica()
        Dim Db As New GestioneDB

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
            Dim totGiornate As Integer = 0

            Dim TotPunti As Single = 0
            Dim VecchiPunti As Single = 0

            Sql = "Select Count(*) From ClassificaSpeciale Where idAnno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=1"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                totGiornate = Rec(0).Value
            End If
            Rec.Close()

            lblGiornata.Text = "Giornate: " & totGiornate

            dttTabella.Columns.Add(Posiz)
            dttTabella.Columns.Add(Giocatore)
            dttTabella.Columns.Add(PuntiRis)

            Sql = "Select Sum(PuntiTot) As PuntiRisultati, B.CodGiocatore, B.Giocatore From ClassificaSpeciale A " & _
                "Left Join Giocatori B On A.idAnno=B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                "Where A.idAnno = " & DatiGioco.AnnoAttuale & " And B.Cancellato='N' " & _
                "Group By B.CodGiocatore, B.Giocatore " & _
                "Order By 1 Desc"

            Dim StringaIn As String = ""

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                StringaIn += Rec("CodGiocatore").Value & ", "

                TotPunti = Rec("PuntiRisultati").Value

                If TotPunti <> VecchiPunti Then
                    Posizione += 1
                    VecchiPunti = TotPunti
                End If

                riga = dttTabella.NewRow()
                riga(0) = Posizione
                riga(1) = Rec("Giocatore").Value
                riga(2) = FormattaNumeroConVirgola(Rec("PuntiRisultati").Value)
                dttTabella.Rows.Add(riga)

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

            g = Nothing

            e.Row.Cells(3).Visible = False
        End If
    End Sub
End Class