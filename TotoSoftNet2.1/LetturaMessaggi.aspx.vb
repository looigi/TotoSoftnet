Public Class LetturaMessaggi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Request.QueryString("idmessaggio") = "" Then
                CreaTabella()
            End If

            divTesto.Visible = False
            divRispondi.Visible = False

            If Request.QueryString("idmessaggio") <> "" Then
                ScriveMessaggio(Request.QueryString("idMessaggio"))
                imgRispondi.Visible = True
            End If
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select A.Progressivo, B.Giocatore, A.DataInvio, A.Testo, Letto From Messaggi A " & _
            "Left Join Giocatori B On A.Anno=B.Anno And A.idOrigine=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.idDestinatario=" & Session("CodGiocatore") & " " & _
            "Order By A.DataInvio, A.Letto"
        Gr.ImpostaCampi(Sql, grdMessaggi)
        Gr = Nothing
    End Sub

    Private Sub grdMessaggi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMessaggi.PageIndexChanging
        grdMessaggi.PageIndex = e.NewPageIndex
        grdMessaggi.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdUtenti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMessaggi.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatarM"), Image)
            Dim Nome As String = e.Row.Cells(2).Text
            Dim imgLeggi As ImageButton = DirectCast(e.Row.FindControl("imgLeggi"), ImageButton)

            If e.Row.Cells(5).Text = "S" Then
                imgLeggi.Visible = False
            Else
                imgLeggi.Visible = True
            End If

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Sub ScriveMessaggio(Progressivo As Integer)
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            hdnProgressivo.Value = Progressivo

            Sql = "Select A.*, B.Giocatore From Messaggi A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.idOrigine=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "idDestinatario=" & Session("CodGiocatore") & " And " & _
                "Progressivo=" & Progressivo
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            imgRispondi.Visible = False
            If Rec.Eof = False Then
                txtMessaggio.Text = Rec("Testo").Value
                lblUtente.Text = Rec("Giocatore").Value
                imgAvatarM.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
                divTesto.Visible = True
            Else
                txtMessaggio.Text = ""
                lblUtente.Text = ""
                divTesto.Visible = False
            End If
            Rec.Close()

            Sql = "Update Messaggi Set " & _
                "Letto='S' " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "idDestinatario=" & Session("CodGiocatore") & " And " & _
                "Progressivo=" & Progressivo
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabella()
        End If

        Db = Nothing
    End Sub

    Protected Sub VisualizzaMessaggio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Progressivo As Integer = row.Cells(0).Text

        ScriveMessaggio(Progressivo)
    End Sub

    Protected Sub RispondiMessaggio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Progressivo As Integer = row.Cells(0).Text

        If Progressivo = 0 And Val(hdnProgressivo.Value) > 0 Then
            Progressivo = hdnProgressivo.Value
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.*, B.Giocatore From Messaggi A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.idOrigine=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "idDestinatario=" & Session("CodGiocatore") & " And " & _
                "Progressivo=" & Progressivo
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            imgRispondi.Visible = True
            If Rec.Eof = False Then
                txtMessaggio.Text = Rec("Testo").Value
                lblUtente.Text = Rec("Giocatore").Value
                imgAvatarM.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
                divTesto.Visible = True
            Else
                txtMessaggio.Text = ""
                lblUtente.Text = ""
                divTesto.Visible = False
            End If
            Rec.Close()

            hdnProgressivo.Value = Progressivo

            Sql = "Update Messaggi Set " & _
                "Letto='S' " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "idDestinatario=" & Session("CodGiocatore") & " And " & _
                "Progressivo=" & Progressivo
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabella()
        End If

        Db = Nothing
    End Sub

    Protected Sub EliminaMessaggio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Progressivo As Integer = row.Cells(0).Text

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Delete From Messaggi " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And " &
                "idDestinatario=" & Session("CodGiocatore") & " And " &
                "Progressivo=" & Progressivo
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            hdnProgressivo.Value = ""
            divTesto.Visible = False
            divRispondi.Visible = False

            CreaTabella()

            Dim Messaggi() As String = {"Messaggio eliminato"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divRispondi.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtCampo.Text = "" Then
            Dim Messaggi() As String = {"Inserire un testo"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Massimo As Integer
            Dim idDestinatario As Integer
            Dim DataInvio As Date = Now

            Sql = "Select A.idOrigine From Messaggi A " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "idDestinatario=" & Session("CodGiocatore") & " And " & _
                "Progressivo=" & hdnProgressivo.Value
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            idDestinatario = Rec(0).Value
            Rec.Close()

            Sql = "Select Max(Progressivo)+1 From Messaggi " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And idDestinatario=" & idDestinatario
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Massimo = 1
            Else
                Massimo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into Messaggi Values (" & _
                " " & DatiGioco.AnnoAttuale & ", " & _
                " " & idDestinatario & ", " & _
                " " & Massimo & ", " & _
                " " & Session("CodGiocatore") & ", " & _
                "'" & SistemaTestoPerDB(txtCampo.Text) & "', " & _
                "'" & ConverteData(DataInvio) & "', " & _
                "'N' " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            Dim gm As New GestioneMail
            gm.InviaMailPerNotificaMessaggio(idDestinatario, Session("CodGiocatore"), txtCampo.Text)
            gm = Nothing

            Dim Messaggi() As String = {"Risposta inviata"}
            VisualizzaMessaggioInPopup(Messaggi, Me)

            hdnProgressivo.Value = ""
            divTesto.Visible = False
            divRispondi.Visible = False

            CreaTabella()
        End If

        Db = Nothing
    End Sub

    Protected Sub imgRispondi_Click(sender As Object, e As ImageClickEventArgs) Handles imgRispondi.Click
        txtCampo.Text = ""
        divRispondi.Visible = True
    End Sub
End Class