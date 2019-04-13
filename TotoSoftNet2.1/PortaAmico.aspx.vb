Public Class PortaAmico
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            txtEMail.Text = ""

            CaricaDati()
        End If
    End Sub

    Private Sub CaricaDati()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Sql = "Select EMail, Registrato From Amici Where " _
            & "Anno=" & DatiGioco.AnnoAttuale & " And " _
            & "CodGiocatore=" & Session("CodGiocatore") & " " _
            & "Order By Progressivo"
        Gr.ImpostaCampi(Sql, grdUtenti)
        Gr = Nothing
    End Sub

    Protected Sub cmdInvia_Click(sender As Object, e As EventArgs) Handles cmdInvia.Click
        If txtEMail.Text = "" Then
            Dim Messaggi() As String = {"Selezionare una E-Mail"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            If txtEMail.Text.Length < 4 Or txtEMail.Text.IndexOf(".") = -1 Or txtEMail.Text.IndexOf("@") = -1 Then
                Dim Messaggi() As String = {"Selezionare una E-Mail valida"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Amici " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore") & " " &
                "And LTRim(Rtrim(Upper(Email)))='" & SistemaTestoPerDB(txtEMail.Text.Trim.ToUpper) & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Dim Messaggi() As String = {"Casella di posta già presentata"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Rec.Close()
                ConnSQL.Close()
                Db = Nothing
                Exit Sub
            End If
            Rec.Close()

            Dim Progressivo As Integer

            Sql = "Select Max(Progressivo)+1 From Amici Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo = 1
            Else
                Progressivo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into Amici Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & Session("CodGiocatore") & ", " &
                " " & Progressivo & ", " &
                "'" & SistemaTestoPerDB(txtEMail.Text) & "', " &
                "'N'" &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim gm As New GestioneMail
            gm.InviaMailPresentaAmico(Session("CodGiocatore"), txtEMail.Text)
            gm = Nothing

            ConnSQL.Close()

            Dim rd As New RiconoscimentiDisonori
            rd.ControllaRegistrazioneAmici(Session("CodGiocatore"))
            rd = Nothing

            txtEMail.Text = ""

            Dim sMessaggi() As String = {"E-Mail inviata"}
            VisualizzaMessaggioInPopup(sMessaggi, Me)
        End If

        Db = Nothing
    End Sub
End Class