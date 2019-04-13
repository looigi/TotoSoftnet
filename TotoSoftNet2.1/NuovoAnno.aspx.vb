Public Class NuovoAnno
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaAnno()
        End If
    End Sub

    Private Sub CaricaAnno()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim Rec As Object = Server.CreateObject("ADODB.RecordSet")

            Sql = "Select Max(Anno)+1 From Anni"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            txtNumeroAnno.Text = Rec(0).Value
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If txtDescrizione.Text = "" Then
            Dim Messaggi() As String = {"Inserire la descrizione del nuovo anno"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtAnnoInizio.Text = "" Or IsNumeric(txtAnnoInizio.Text) = False Then
            Dim Messaggi() As String = {"Anno inizio campionato non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into Anni Values (" & txtNumeroAnno.Text & ", '" & SistemaTestoPerDB(MetteMaiuscole(txtDescrizione.Text.Trim)) & "', " & txtAnnoInizio.Text & ")"
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into Giocatori Values (" &
                " " & txtNumeroAnno.Text & ", " &
                "1, " &
                "'LOOIGI', " &
                "'6¢¥¥ŸŸ', " &
                "1, " &
                "'Pecce', " &
                "'Luigi', " &
                "1, " &
                "'looigi@gmail.com', " &
                "'N', " &
                "'Anche un orologio fermo ha ragione due volte al giorno', " &
                "'S', " &
                "'S' " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into DettaglioGiocatori Values (" & _
                " " & txtNumeroAnno.Text & ", " & _
                "1, " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0 " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Totale As Single = NumeroGiornateTotali * QuotaGiocoSettimanale

            Sql = "Insert Into Bilancio Values (" & _
                " " & txtNumeroAnno.Text & ", " & _
                "1, " & _
                " " & Totale.ToString.Replace(",", ".") & ", " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0 " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into DatiDiGioco Values (" & _
                " " & txtNumeroAnno.Text & ", " & _
                "0, " & _
                "0, " & _
                "'', " & _
                "-1, " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0 " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into AnnoConfig Values (" &
                " " & txtNumeroAnno.Text & ", " &
                "38, " &
                "6, " &
                "1.5, " &
                "',', " &
                "33, " &
                "36, " &
                "3 " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            ' Creazione eventi
            Sql = "Insert Into Eventi Select " & txtNumeroAnno.Text & " As Anno, Giornata, Progressivo, Descrizione From Eventi Where Anno=" & Val(txtNumeroAnno.Text) - 1
            Db.EsegueSql(ConnSQL, Sql)
            ' Creazione eventi

            ' Creazione percentuali vincita
            Sql = "Insert Into PercentualiPremi Select " & txtNumeroAnno.Text & " As Anno, descPremio, Perc From PercentualiPremi Where Anno=" & Val(txtNumeroAnno.Text) - 1
            Db.EsegueSql(ConnSQL, Sql)
            ' Creazione percentuali vincita

            Response.Redirect("Default.aspx?NuovoAnno=1")
        End If

        Db = Nothing
    End Sub
End Class