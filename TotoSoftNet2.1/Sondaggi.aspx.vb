Public Class Sondaggi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divModifica.Visible = False
            CaricaSondaggi()
        End If
    End Sub

    Private Sub CaricaSondaggi()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select idSondaggio,TestoSondaggio,Risposta1,Risposta2,Risposta3,DataInizio,DataFine From Sondaggi Order By idSondaggio Desc"
        Gr.ImpostaCampi(Sql, grdSondaggi)
        Gr = Nothing
    End Sub

    Private Sub grdSondaggi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSondaggi.PageIndexChanging
        grdSondaggi.PageIndex = e.NewPageIndex
        grdSondaggi.DataBind()

        CaricaSondaggi()
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If txtSondaggio.Text = "" Then
            Dim Messaggi() As String = {"Inserire il testo del sondaggio"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtRisposta1.Text = "" Then
            Dim Messaggi() As String = {"Inserire il testo per la risposta 1"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtRisposta2.Text = "" Then
            Dim Messaggi() As String = {"Inserire il testo per la risposta 2"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtRisposta3.Text = "" Then
            Dim Messaggi() As String = {"Inserire il testo per la risposta 3"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idSondaggio As Integer

            Sql = "Select Max(idSondaggio)+1 From Sondaggi"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                idSondaggio = 1
            Else
                idSondaggio = Rec(0).Value
            End If
            Rec.Close()

            Dim DataInizio As String = ConverteData(Now)
            Dim DataFine As String = ConverteData(Now.AddDays(7))

            Sql = "Insert Into Sondaggi Values (" & _
                " " & idSondaggio & ", " & _
                " " & DatiGioco.AnnoAttuale & ", " & _
                "'" & SistemaTestoPerDB(txtSondaggio.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtRisposta1.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtRisposta2.Text) & "', " & _
                "'" & SistemaTestoPerDB(txtRisposta3.Text) & "', " & _
                "'" & DataInizio & "', " & _
                "'" & DataFine & "', " & _
                "'N' " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim gm As New GestioneMail
            gm.InviaMailNuovoSondaggio(idSondaggio, txtSondaggio.Text, DataFine, Session("Nick"))
            gm = Nothing

            ConnSQL.Close()

            Dim Messaggi() As String = {"Nuovo sondaggio creato"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            CaricaSondaggi()

            divModifica.Visible = False
            cmdNuovo.Visible = True
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        txtSondaggio.Text = ""
        txtRisposta1.Text = "Sì"
        txtRisposta2.Text = "No"
        txtRisposta3.Text = "Mi astengo"

        divModifica.Visible = True
        imgStatSondaggio.Visible = False
        ulModifica.Visible = True

        cmdNuovo.Visible = False
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divModifica.Visible = False
        cmdNuovo.Visible = True
    End Sub

    Protected Sub MostraStatisticheSondaggio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim idSondaggio As Integer = row.Cells(0).Text
        Dim Testo As String = row.Cells(1).Text
        Dim Risposta1 As String = row.Cells(2).Text
        Dim Risposta2 As String = row.Cells(3).Text
        Dim Risposta3 As String = row.Cells(4).Text

        cmdNuovo.Visible = True
        divModifica.Visible = True
        imgStatSondaggio.Visible = True
        ulModifica.Visible = False

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            
            Dim Giocatori As Integer
            Dim Votanti As Integer
            Dim Risposte(2) As Integer
            Dim RispStringa() As String = {Risposta1, Risposta2, Risposta3}
            Dim Anno As Integer

            Sql = "Select Count(*) From SondaggiRisposte Where idSondaggio=" & idSondaggio
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Votanti = Rec(0).Value
            Rec.Close()

            Sql = "Select Anno From Sondaggi Where idSondaggio=" & idSondaggio
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Anno = Rec(0).Value
            Rec.Close()

            Sql = "Select Count(*) From Giocatori Where Anno=" & Anno
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Giocatori = Rec(0).Value
            Rec.Close()

            Sql = "Select Risposta, Count(*) From SondaggiRisposte Where idSondaggio=" & idSondaggio & " Group By Risposta"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Risposte(Rec("Risposta").Value - 1) = Rec(1).Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim NomeImmagineStat As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale.ToString.Trim & "\" & Session("Nick") & ".stat.png"
            Dim NomeImmagineUrl As String = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale.ToString.Trim & "/" & Session("Nick") & ".stat.png"

            Try
                Kill(NomeImmagineStat)
            Catch ex As Exception

            End Try

            Dim gi As New GestioneImmagini
            gi.DisegnaStatisticheSondaggi(Giocatori, Votanti, Risposte, RispStringa, NomeImmagineStat)
            gi = Nothing

            imgStatSondaggio.ImageUrl = NomeImmagineUrl

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub ChiudeSondaggio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim idSondaggio As Integer = row.Cells(0).Text
        Dim Testo As String = row.Cells(1).Text
        Dim RisposteTesto() As String = {row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text}
        Dim Risposte(2) As Integer

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giocatori As Integer
            Dim Anno As Integer
            Dim Votanti As Integer

            Sql = "Select Count(*) From SondaggiRisposte Where idSondaggio=" & idSondaggio
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Votanti = Rec(0).Value
            Rec.Close()

            Sql = "Select Anno From Sondaggi Where idSondaggio=" & idSondaggio
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Anno = Rec(0).Value
            Rec.Close()

            Sql = "Select Count(*) From Giocatori Where Anno=" & Anno
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Giocatori = Rec(0).Value
            Rec.Close()

            Sql = "Select Risposta, Count(*) From SondaggiRisposte Where idSondaggio=" & idSondaggio & " Group By Risposta"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Risposte(Rec("Risposta").Value - 1) = Rec(1).Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim gm As New GestioneMail
            gm.InviaMailChiusuraSondaggio(idSondaggio, Testo, Session("Nick"), RisposteTesto, Risposte, Giocatori, Votanti)
            gm = Nothing

            Sql = "Update Sondaggi Set Chiuso='S' Where idSondaggio=" & idSondaggio
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            Dim Messaggi() As String = {"Sondaggio " & idSondaggio & " chiuso"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing

        CaricaSondaggi()
    End Sub

    Private Sub grdSondaggi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdSondaggi.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imgChiude As ImageButton = DirectCast(e.Row.FindControl("imgChiude"), ImageButton)

            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Select Chiuso From Sondaggi Where idSondaggio=" & e.Row.Cells(0).Text
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec("Chiuso").Value = "N" Then
                    imgChiude.Visible = True
                Else
                    imgChiude.Visible = False
                End If
                Rec.Close()

                ConnSQL.Close()
            End If

            Db = Nothing

            e.Row.Cells(0).Visible = False
        End If
    End Sub
End Class