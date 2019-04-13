Public Class Percentuali
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaEventi()

            divModifica.Visible = False
        End If
    End Sub

    Private Sub CaricaEventi()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select descPremio, Perc From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio<>'SPECIALI' Order By descPremio"
        Gr.ImpostaCampi(Sql, grdPercentuali)

        Gr = Nothing

        Dim Gr2 As New Griglie

        Sql = "Select descPremio, Perc From PercentualiPremi Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='SPECIALI' Order By descPremio"
        Gr2.ImpostaCampi(Sql, grdSpeciali)

        Gr2 = Nothing
    End Sub

    Protected Sub AggiornaPercentuale(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Descrizione As String = row.Cells(0).Text
        Dim Percentuale As String = row.Cells(1).Text

        txtDescrizione.Text = Descrizione
        txtPercentuale.Text = Percentuale

        hdnModalita.Value = "PERC"
        divModifica.Visible = True
    End Sub

    Protected Sub AggiornaSpeciali(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Descrizione As String = row.Cells(0).Text
        Dim Percentuale As String = row.Cells(1).Text

        txtDescrizione.Text = Descrizione
        txtPercentuale.Text = Percentuale

        hdnModalita.Value = "SPEC"
        divModifica.Visible = True
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divModifica.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtPercentuale.Text = "" Or IsNumeric(txtPercentuale.Text) = False Or Val(txtPercentuale.Text) < 1 Or Val(txtPercentuale.Text) > 100 Then
            Dim Messaggi() As String = {"Percentuale non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update PercentualiPremi Set Perc=" & txtPercentuale.Text.Replace(",", ".") & " Where Anno=" & DatiGioco.AnnoAttuale & " And descPremio='" & txtDescrizione.Text & "'"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            divModifica.Visible = False
            CaricaEventi()
        End If

        Db = Nothing
    End Sub
End Class