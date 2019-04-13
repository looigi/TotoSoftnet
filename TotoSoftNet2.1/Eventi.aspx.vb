Public Class Eventi1
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

        Sql = "Select Giornata, Progressivo, Descrizione From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " Order By Giornata, Progressivo"
        Gr.ImpostaCampi(Sql, grdEventi)
        Gr = Nothing
    End Sub

    Protected Sub AggiornaEvento(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Giornata As String = row.Cells(0).Text
        Dim Progressivo As String = row.Cells(1).Text
        Dim Descrizione As String = row.Cells(2).Text

        txtGiornata.Text = Giornata
        txtDescrizione.Text = Descrizione

        divModifica.Visible = True
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divModifica.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtGiornata.Text = "" Or IsNumeric(txtGiornata.Text) = False Or Val(txtGiornata.Text) < 1 Or Val(txtGiornata.Text) > 38 Then
            Dim Messaggi() As String = {"Giornata non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Massimo As Integer

            Sql = "Select Max(Progressivo)+1 From Eventi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & txtGiornata.Text
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Massimo = 1
            Else
                Massimo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Update Eventi Set Giornata=" & txtGiornata.Text & ", Progressivo=" & Massimo & " Where Anno=" & DatiGioco.AnnoAttuale & " And Descrizione='" & txtDescrizione.Text & "'"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            divModifica.Visible = False
            CaricaEventi()
        End If

        Db = Nothing
    End Sub
End Class