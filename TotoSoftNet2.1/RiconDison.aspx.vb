Public Class RiconDison
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PrendePuntiTot()
            CreaTabella()
        End If
    End Sub

    Private Sub PrendePuntiTot()
        'Dim Db As New GestioneDB

        'If Db.LeggeImpostazioniDiBase() = True Then
        '    Dim ConnSQL As Object = Db.ApreDB()
        '    Dim Rec As Object = CreateObject("ADODB.Recordset")
        '    Dim Sql As String
        '    Dim Punti As Single

        '    Sql = "Select Sum(Punti) As Punti From RiconDisonGioc A " & _
        '        "Left Join RiconDison B On A.idPremio=B.idPremio " & _
        '        "Where idAnno = " & DatiGioco.AnnoAttuale & " And CodGioc = " & Session("CodGiocatore")
        '    Rec = Db.LeggeQuery(ConnSQL, Sql)
        '    If Rec(0).Value Is DBNull.Value = True Then
        '        Punti = 0
        '    Else
        '        Punti = Rec("Punti").Value
        '    End If
        '    Rec.Close()

        '    ConnSQL.Close()

        '    lblBilancio.Text = "Bilancio punti: " & Punti
        'End If

        'Db = Nothing
    End Sub

    Private Sub CreaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Giornata, B.Descrizione, Punti From RiconDisonGioc A " & _
            "Left Join RiconDison B On A.idPremio=B.idPremio " & _
            "Where idAnno = " & DatiGioco.AnnoAttuale & " And CodGioc = " & Session("CodGiocatore")
        Gr.ImpostaCampi(Sql, grdRiconDison)
        Gr = Nothing
    End Sub

    Private Sub grdRiconDison_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdRiconDison.PageIndexChanging
        grdRiconDison.PageIndex = e.NewPageIndex
        grdRiconDison.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdRiconDison_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRiconDison.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim Img As Image = DirectCast(e.Row.FindControl("imgImmagine"), Image)
            Dim Testo As String = e.Row.Cells(2).Text

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Select Immagine From RiconDison Where Descrizione='" & SistemaTestoPerDB(RimetteCaratteriStraniNelNome(Testo)) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Dim Immagine As String = "App_Themes/Standard/Images/Icone/Riconoscimenti/" & Rec("Immagine").Value
                Rec.Close()

                Img.ImageUrl = Immagine

                ConnSQL.Close()
            End If

            Db = Nothing
        End If
    End Sub
End Class