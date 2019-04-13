Public Class GestioneCoppe
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaPercCoppe()

            divModifica.Visible = False
        End If
    End Sub

    Private Sub CaricaPercCoppe()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select NumGiocatori,CL,PassCL,EL,PassEL,IT,PassIT,Der From PercCoppe Order By NumGiocatori"
        Gr.ImpostaCampi(Sql, grdPercentuali)
        Gr = Nothing
    End Sub

    Protected Sub AggiornaPercentuale(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Numero As String = row.Cells(0).Text
        Dim CL As String = row.Cells(1).Text
        Dim PassCL As String = row.Cells(2).Text
        Dim EL As String = row.Cells(3).Text
        Dim PassEL As String = row.Cells(4).Text
        Dim IT As String = row.Cells(5).Text
        Dim PassIT As String = row.Cells(6).Text
        Dim Der As String = row.Cells(7).Text

        txtNumeroUtenti.Text = Numero
        txtCL.Text = CL
        txtPassCL.Text = PassCL
        txtEL.Text = EL
        txtPassEL.Text = PassEL
        txtIT.Text = IT
        txtPassIT.Text = PassIT
        txtDer.Text = Der

        divModifica.Visible = True
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divModifica.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtCL.Text = "" Or IsNumeric(txtCL.Text) = False Or Val(txtCL.Text) < 0 Or Val(txtCL.Text) > 15 Then
            Dim Messaggi() As String = {"Qualificate in Coppa Campioni non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtEL.Text = "" Or IsNumeric(txtEL.Text) = False Or Val(txtEL.Text) < 0 Or Val(txtEL.Text) > 15 Then
            Dim Messaggi() As String = {"Qualificate in Europa League non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtIT.Text = "" Or IsNumeric(txtIT.Text) = False Or Val(txtIT.Text) < 0 Or Val(txtIT.Text) > 15 Then
            Dim Messaggi() As String = {"Qualificate in Intertoto non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtDer.Text = "" Or IsNumeric(txtDer.Text) = False Or Val(txtDer.Text) < 0 Or Val(txtDer.Text) > 15 Then
            Dim Messaggi() As String = {"Qualificate al torneo Pippettero non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        If txtPassCL.Text = "" Or IsNumeric(txtPassCL.Text) = False Or Val(txtPassCL.Text) < 0 Or Val(txtPassCL.Text) > 15 Then
            Dim Messaggi() As String = {"Passaggio del turno di Coppa Campioni non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtPassEL.Text = "" Or IsNumeric(txtPassEL.Text) = False Or Val(txtPassEL.Text) < 0 Or Val(txtPassEL.Text) > 15 Then
            Dim Messaggi() As String = {"Passaggio del turno di Europa League non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtPassIT.Text = "" Or IsNumeric(txtPassIT.Text) = False Or Val(txtPassIT.Text) < 0 Or Val(txtPassIT.Text) > 15 Then
            Dim Messaggi() As String = {"Passaggio del turno di Intertoto non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update PercCoppe Set " & _
                    "CL=" & txtCL.Text & ", " & _
                    "PassCL=" & txtPassCL.Text & ", " & _
                    "EL=" & txtEL.Text & ", " & _
                    "PassEL=" & txtPassEL.Text & ", " & _
                    "IT=" & txtIT.Text & ", " & _
                    "PassIT=" & txtPassIT.Text & ", " & _
                    "DER=" & txtDer.Text & " " & _
                    "Where Numgiocatori=" & txtNumeroUtenti.Text
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            divModifica.Visible = False
            CaricaPercCoppe()
        End If

        Db = Nothing
    End Sub
End Class