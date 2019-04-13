Public Class Prese
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divDettaglio.Visible = False

            CreaTabella()
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Partite() As Integer
            Dim Prese() As Integer
            Dim nSquadra() As String
            Dim Quante As Integer

            Dim Squadra As New DataColumn("Squadra")
            Dim Volte As New DataColumn("Volte")
            Dim Presa As New DataColumn("Presa")

            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Squadra)
            dttTabella.Columns.Add(Volte)
            dttTabella.Columns.Add(Presa)

            Sql = "Select Distinct Squadra From ( " _
                & "Select Distinct SquadraCasa As Squadra From Schedine " _
                & "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata<" & DatiGioco.Giornata + 1 & " " _
                & "Union All " _
                & "Select Distinct SquadraFuori As Squadra From Schedine " _
                & "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata<" & DatiGioco.Giornata + 1 & " " _
                & ") A Order By 1"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Sql = "Select * From Pronostici A " _
                    & "Left Join Schedine B On " _
                    & "A.Anno=B.Anno And A.Giornata = B.Giornata And A.Partita = B.Partita " _
                    & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.CodGiocatore = " & Session("CodGiocatore") & " And A.Giornata<" & DatiGioco.Giornata + 1 & " And " _
                    & "(B.SquadraCasa = '" & SistemaTestoPerDB(Rec("Squadra").Value) & "' Or B.SquadraFuori = '" & SistemaTestoPerDB(Rec("Squadra").Value) & "')"
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                redim Preserve Partite(Quante)
                redim Preserve nSquadra(Quante)
                redim Preserve Prese(Quante)

                nSquadra(Quante) = Rec("Squadra").Value
                Do Until Rec2.Eof
                    Partite(Quante) += 1
                    If Rec2("Segni").Value.ToString.Length = 1 Then
                        If Rec2("Segni").Value = Rec2("Segno").Value And Rec2("Segno").Value.ToString.Trim <> "" Then
                            Prese(Quante) += 1
                        End If
                    Else
                        If Rec2("Segni").Value.ToString.IndexOf(Rec2("Segno").Value) > -1 And Rec2("Segno").Value.ToString.Trim <> "" Then
                            Prese(Quante) += 1
                        End If
                    End If

                    Rec2.MoveNext
                Loop
                Rec2.Close

                Quante += 1

                Rec.MoveNext
            Loop
            Rec.Close

            Dim Appo As Integer
            Dim sAppo As String

            For i As Integer = 0 To Quante - 1
                For k As Integer = i + 1 To Quante - 1
                    If Prese(k) > Prese(i) Or (Prese(k) = Prese(i) And Partite(i) > Partite(k)) Then
                        Appo = Prese(i)
                        Prese(i) = Prese(k)
                        Prese(k) = Appo

                        Appo = Partite(i)
                        Partite(i) = Partite(k)
                        Partite(k) = Appo

                        sAppo = nSquadra(i)
                        nSquadra(i) = nSquadra(k)
                        nSquadra(k) = sAppo
                    End If
                Next
            Next

            For i As Integer = 0 To Quante - 1
                riga = dttTabella.NewRow()
                riga(0) = nSquadra(i)
                riga(1) = Partite(i)
                riga(2) = Prese(i)
                dttTabella.Rows.Add(riga)
            Next

            grdPrese.DataSource = dttTabella
            grdPrese.DataBind()

            ConnSQL.Close
        End If

        Db = Nothing
    End Sub

    Private Sub grdPrese_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPrese.PageIndexChanging
        grdPrese.PageIndex = e.NewPageIndex
        grdPrese.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdPrese_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPrese.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgSquadra As Image = DirectCast(e.Row.FindControl("imgSquadra"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            Dim gm As New GestioneMail

            ImgSquadra.ImageUrl = gm.RitornaImmagineSquadra(Nome, True)
            ImgSquadra.DataBind()

            gm = Nothing
        End If
    End Sub

    Protected Sub cmdChiudeDettaglio_Click(sender As Object, e As EventArgs) Handles cmdChiudeDettaglio.Click
        divDettaglio.Visible = False
    End Sub

    Protected Sub VisualizzaDettaglio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnSquadra.Value = row.Cells(1).Text

        CreaDettaglio()

        divDettaglio.Visible = True
    End Sub

    Private Sub CreaDettaglio()
        Dim Sql As String
        Dim gr As New Griglie
        Dim Squadra As String = hdnSquadra.Value

        Sql = "Select A.Giornata, A.SquadraCasa+'-'+A.SquadraFuori As Partita, A.Segno, A.Risultato , B.Segni As Pronostico " _
            & "From Schedine A Left Join " _
            & " Pronostici B On A.Anno = B.Anno And A.Giornata = B.Giornata And A.Partita = B.Partita " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And (A.SquadraCasa = '" & Squadra & "' Or A.SquadraFuori ='" & Squadra & "') And B.CodGiocatore=" & Session("CodGiocatore") & " " _
            & "And A.Giornata<=" & DatiGioco.Giornata & " Order By A.Giornata"
        gr.ImpostaCampi(Sql, grdDettaglio)
        gr = Nothing
    End Sub

    Private Sub grdDettaglio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDettaglio.PageIndexChanging
        grdDettaglio.PageIndex = e.NewPageIndex
        grdDettaglio.DataBind()

        CreaDettaglio()
    End Sub

End Class