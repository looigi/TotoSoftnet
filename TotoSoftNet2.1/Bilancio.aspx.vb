Public Class Bilancio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divTesto.Visible = False

            CaricaCombo()
            CreaTabella()
        End If
    End Sub

    Private Sub CaricaCombo()
        cmbModalita.Items.Clear()
        cmbModalita.Items.Add("")
        cmbModalita.Items.Add("Contanti")
        cmbModalita.Items.Add("Paypal")
        cmbModalita.Items.Add("Vincita")

        cmbCaricati.Items.Clear()
        cmbCaricati.Items.Add("")
        cmbCaricati.Items.Add("Sì")
        cmbCaricati.Items.Add("No")
    End Sub

    Private Sub CreaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select B.Giocatore, DataOra, Importo, Tipologia, Modalita, Caricati, SUBSTRING(Note,1,10), Progressivo From BilancioDettaglio A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And Tipologia='D' And (Modalita Is Null Or Modalita<>'Vincita') " & _
            "And B.Pagante='S' " & _
            "Order By DataOra Desc"
        Gr.ImpostaCampi(Sql, grdBilancio)
        Gr = Nothing

        ScriveTotali()
    End Sub

    Private Sub ScriveTotali()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Sum(Importo) As Importo From BilancioDettaglio " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Caricati='N' And Tipologia='D' And Modalita='Contanti'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                lblBuzzico.Text = "&euro; 0"
            Else
                lblBuzzico.Text = ScriveNumeroFormattato(Rec(0).Value)
            End If
            Rec.Close()

            Sql = "Select Sum(Importo) As Importo From BilancioDettaglio " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Caricati='S' And Tipologia='D' And Modalita='Paypal'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                lblContoPayPal.Text = "&euro; 0"
            Else
                lblContoPayPal.Text = ScriveNumeroFormattato(Rec(0).Value)
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub grdBilancio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdBilancio.PageIndexChanging
        grdBilancio.PageIndex = e.NewPageIndex
        grdBilancio.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdUtenti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdBilancio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Protected Sub SelezionaRiga(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Giocatore As String = row.Cells(1).Text
        Dim g As New Giocatori
        Dim CodGiocatore As String = g.TornaIdGiocatore(Giocatore)
        g = Nothing
        Dim Progressivo As String = row.Cells(8).Text
        hdnProgressivo.Value = Progressivo

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From BilancioDettaglio " & _
            "Where Anno = " & DatiGioco.AnnoAttuale & " And Progressivo = " & Progressivo & " " & _
            "And CodGiocatore = " & CodGiocatore

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            lblGiocatore.Text = Giocatore
            lblImporto.Text = ScriveNumeroFormattato("" & Rec("Importo").Value)
            lblTipologia.Text = IIf("" & Rec("Tipologia").Value = "D", "Dare", "Avere")
            lblData.Text = "" & Rec("DataOra").Value
            cmbModalita.Text = "" & Rec("Modalita").Value
            cmbCaricati.Text = IIf("" & Rec("Caricati").Value = "S", "Sì", "No")
            txtNote.Text = "" & Rec("Note").Value
            Rec.Close()

            ConnSQL.Close()

            divTesto.Visible = True
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divTesto.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim g As New Giocatori
            Dim CodGiocatore As String = g.TornaIdGiocatore(lblGiocatore.Text)
            g = Nothing
            Dim Caricati As String = cmbCaricati.Text
            If Caricati <> "" Then
                Caricati = Mid(Caricati, 1, 1)
            End If

            Sql = "Update BilancioDettaglio Set " & _
                "Modalita='" & cmbModalita.Text & "', " & _
                "Caricati='" & Caricati & "', " & _
                "[Note]='" & SistemaTestoPerDB(txtNote.Text) & "' " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & CodGiocatore & " And Progressivo=" & hdnProgressivo.Value
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        Db = Nothing

        divTesto.Visible = False
        CreaTabella()
    End Sub
End Class