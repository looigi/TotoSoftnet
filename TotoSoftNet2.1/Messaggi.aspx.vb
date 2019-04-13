Public Class Messaggi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PulisceTabellaDestinatari()

            CreaTabellaUtentiDaScegliere()
            CreaTabellaUtentiScelti()
        End If
    End Sub

    Private Sub PulisceTabellaDestinatari()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Try
                Sql = "Drop Table AppoMessaggi" & Session("CodGiocatore")
                Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
            Catch ex As Exception

            End Try

            Sql = "Create Table AppoMessaggi" & Session("CodGiocatore") & " (Anno smallint, Destinatario smallint)"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub CreaTabellaUtentiDaScegliere()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Numero As New DataColumn("Utente")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Numero)

            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Messi As Boolean = False

            Sql = "Select * From Giocatori " & _
                "Where CodGiocatore Not In (Select Destinatario From AppoMessaggi" & Session("CodGiocatore") & " Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N') " & _
                "And Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Messi = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value.ToString.Trim.ToUpper
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            grdUtentiTutti.DataSource = dttTabella
            grdUtentiTutti.DataBind()

            If Messi = True Then
                divDaScegliere.Visible = True
            Else
                divDaScegliere.Visible = False
            End If
        End If

        Db = Nothing
    End Sub

    Private Sub grdUtentiTutti_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUtentiTutti.PageIndexChanging
        grdUtentiTutti.PageIndex = e.NewPageIndex
        grdUtentiTutti.DataBind()

        CreaTabellaUtentidaScegliere()
    End Sub

    Private Sub grdUtentiTutti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUtentiTutti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim ImgTasto As ImageButton = DirectCast(e.Row.FindControl("imgVai"), ImageButton)
            Dim Nome As String = e.Row.Cells(0).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Protected Sub SelezionaUtente(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(0).Text.ToUpper.Trim

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim g As New Giocatori
            Dim idGiocatore As Integer = g.TornaIdGiocatore(Utente)
            g = Nothing

            Sql = "Insert Into AppoMessaggi" & Session("CodGiocatore") & " Values (" & DatiGioco.AnnoAttuale & ", " & idGiocatore & ")"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabellaUtentidaScegliere()
            CreaTabellaUtentiScelti()
        End If

        Db = Nothing
    End Sub

    Private Sub CreaTabellaUtentiScelti()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Numero As New DataColumn("Utente")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Numero)

            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Messi As Boolean = False

            Sql = "Select A.*, B.Giocatore From AppoMessaggi" & Session("CodGiocatore") & " A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.Destinatario=B.CodGiocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                Messi = True

                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value.ToString.Trim.ToUpper
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            grdUtentiScelti.DataSource = dttTabella
            grdUtentiScelti.DataBind()

            If Messi = True Then
                divScelti.Visible = True
                divMessaggio.Visible = True
            Else
                divScelti.Visible = False
                divMessaggio.Visible = False
            End If
        End If

        Db = Nothing
    End Sub

    Private Sub grdUtentiScelti_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUtentiScelti.PageIndexChanging
        grdUtentiScelti.PageIndex = e.NewPageIndex
        grdUtentiScelti.DataBind()

        CreaTabellaUtentiScelti()
    End Sub

    Private Sub grdUtentiScelti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUtentiScelti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim ImgTasto As ImageButton = DirectCast(e.Row.FindControl("imgVai"), ImageButton)
            Dim Nome As String = e.Row.Cells(1).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Protected Sub DeSelezionaUtente(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(1).Text.ToUpper.Trim

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim g As New Giocatori
            Dim idGiocatore As Integer = g.TornaIdGiocatore(Utente)
            g = Nothing

            Sql = "Delete From AppoMessaggi" & Session("CodGiocatore") & " Where Destinatario=" & idGiocatore
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabellaUtentiDaScegliere()
            CreaTabellaUtentiScelti()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdTutti_Click(sender As Object, e As EventArgs) Handles cmdTutti.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Insert Into AppoMessaggi1 " & _
                "Select Anno, CodGiocatore From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In " & _
                "(Select Destinatario From AppoMessaggi1 Where Anno=" & DatiGioco.AnnoAttuale & ")"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabellaUtentiDaScegliere()
            CreaTabellaUtentiScelti()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdNessuno_Click(sender As Object, e As EventArgs) Handles cmdNessuno.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Delete From AppoMessaggi" & Session("CodGiocatore") & " Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabellaUtentiDaScegliere()
            CreaTabellaUtentiScelti()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdInvia_Click(sender As Object, e As EventArgs) Handles cmdInvia.Click
        If grdUtentiScelti.Rows.Count = 0 Then
            Dim Messaggi() As String = {"Selezionare almeno un destinatario"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String
            Dim Rec As Object = Server.CreateObject("ADODB.RecordSet")
            Dim Rec2 As Object = Server.CreateObject("ADODB.RecordSet")
            Dim id As String = ""
            Dim id2 As String = ""
            Dim Massimo As Integer
            Dim DataInvio As Date = Now

            Sql = "Select * From AppoMessaggi" & Session("CodGiocatore") & " " &
                "Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                id += Rec("Destinatario").Value & ";"
                id2 = Rec("Destinatario").Value

                Sql = "Select Max(Progressivo)+1 From Messaggi " &
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And idDestinatario=" & id2
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                If Rec2(0).Value Is DBNull.Value = True Then
                    Massimo = 1
                Else
                    Massimo = Rec2(0).Value
                End If
                Rec2.Close()

                Sql = "Insert Into Messaggi Values (" &
                    " " & DatiGioco.AnnoAttuale & ", " &
                    " " & id2 & ", " &
                    " " & Massimo & ", " &
                    " " & Session("CodGiocatore") & ", " &
                    "'" & SistemaTestoPerDB(txtMessaggio.Text) & "', " &
                    "'" & ConverteData(DataInvio) & "', " &
                    "'N' " &
                    ")"
                Db.EsegueSql(ConnSQL, Sql)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            Dim gm As New GestioneMail
            gm.InviaMailPerNotificaMessaggio(id, Session("CodGiocatore"), txtMessaggio.Text)
            gm = Nothing

            PulisceTabellaDestinatari()

            CreaTabellaUtentiDaScegliere()
            CreaTabellaUtentiScelti()

            Dim Messaggi() As String = {"Messaggio inviato"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub
End Class