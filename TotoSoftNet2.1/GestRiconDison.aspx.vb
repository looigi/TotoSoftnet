Public Class GestRiconDison
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaRD()

            divModifica.Visible = False
        End If
    End Sub

    Private Sub CaricaRD()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select idPremio, Descrizione, Immagine, Punti From RiconDison Order By idPremio"
        Gr.ImpostaCampi(Sql, grdRiconDison)
        Gr = Nothing
    End Sub

    Private Sub grdRiconDison_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRiconDison.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim Img As Image = DirectCast(e.Row.FindControl("img"), Image)
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

    Protected Sub AggiornaEvento(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Descrizione As String = row.Cells(2).Text
        Dim Immagine As String = row.Cells(3).Text
        Dim Premio As String = row.Cells(4).Text

        hdnId.Value = row.Cells(1).Text

        txtDescrizione.Text = Descrizione
        txtImmagine.Text = Immagine
        txtPremio.Text = Premio

        divModifica.Visible = True
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divModifica.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtDescrizione.Text = "" Then
            Dim Messaggi() As String = {"Inserire una descrizione"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtImmagine.Text = "" Then
            Dim Messaggi() As String = {"Inserire una immagine"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtPremio.Text = "" Then
            Dim Messaggi() As String = {"Inserire il premio"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            If IsNumeric(txtPremio.Text) = False Then
                Dim Messaggi() As String = {"Inserire un premio valido"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update RiconDison Set " & _
                "Descrizione='" & SistemaTestoPerDB(txtDescrizione.Text) & "', " & _
                "Immagine='" & SistemaTestoPerDB(txtImmagine.Text) & "', " & _
                "Punti=" & txtPremio.Text.Replace(",", ".") & " " & _
                "Where idPremio=" & hdnId.Value
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            divModifica.Visible = False

            CaricaRD()
        End If

        Db = Nothing
    End Sub
End Class