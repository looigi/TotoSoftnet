Imports System.Globalization

Public Class Cassa
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                clsCash(Session("CodGiocatore")) = New clsCassa
            Catch ex As Exception
                redim Preserve clsCash(Session("CodGiocatore"))
                clsCash(Session("CodGiocatore")) = New clsCassa
            End Try

            clsCash(Session("CodGiocatore")).CaricaDati(grdBilancio, grdVittorie, Session("Permesso"), Session("CodGiocatore"))

            divTesto.Visible = False
            divDettaglio.Visible = False
            divPagamento.Visible = False

            Menu1.Items(0).Selected = True

            If Session("Permesso") <> 1 Then
                Menu1.Items.RemoveAt(1)
            End If
        End If
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Private Sub grdVittorie_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdVittorie.RowDataBound
        clsCash(Session("CodGiocatore")).CaricamentoRigaVittorie(sender, e, Server.MapPath("."), grdBilancio, True)
    End Sub

    Private Sub grdBilancio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdBilancio.PageIndexChanging
        grdBilancio.PageIndex = e.NewPageIndex
        grdBilancio.DataBind()

        clsCash(Session("CodGiocatore")).CaricaDati(grdBilancio, grdVittorie, Session("Permesso"), Session("CodGiocatore"))
    End Sub

    Private Sub grdBilancio_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBilancio.RowDataBound
        clsCash(Session("CodGiocatore")).CaricamentoRigaBilancio(sender, e, Server.MapPath("."), Session("Permesso"))
    End Sub

    Protected Sub EffettuaPagamento(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(1).Text

        hdnTipoCampo.Value = "PAGAMENTO;" & Utente
        hdnModalita.Value = "PAGAMENTOGIOCATORE"
        hdnUtente.Value = Utente
        lblCampo.Text = "Inserire l'importo del pagamento"
        txtCampo.Text = ""
        txtCampo.Focus()
        divTesto.Visible = True
    End Sub

    Protected Sub EffettuaIncasso(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(1).Text
        Dim Cifra As Single = row.Cells(6).Text.Replace(".", ",")
        If Cifra < 0 Then Cifra = 0

        hdnTipoCampo.Value = "INCASSO;" & Utente & ";" & Cifra.ToString
        hdnModalita.Value = "INCASSOGIOCATORE"
        hdnUtente.Value = Utente
        lblCampo.Text = "Inserire l'importo dell'incasso"
        txtCampo.Text = Cifra
        txtCampo.Focus()
        divTesto.Visible = True
    End Sub

    Private Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divTesto.Visible = False
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If txtCampo.Text = "" Then
            Dim Messaggi() As String = {"Inserire un importo"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            If IsNumeric(txtCampo.Text) = False Then
                Dim Messaggi() As String = {"Inserire un importo valido"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Sql As String = ""
            Dim G As New Giocatori
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Valore As Single = txtCampo.Text.Replace(".", ",")
            Dim sValore As String = Valore.ToString.Replace(",", ".")

            If hdnModalita.Value = "VISUALIZZADETTAGLIO" Then
                'Dim Campi() As String = hdnTipoCampo.Value.Split(";")

                'If Campi(0) = "INCASSO" Then
                '    Dim sInserito As Single = txtCampo.Text.Replace(".", ".")
                '    Dim sMassimo As Single = Campi(2).Replace(".", ".")

                '    If sInserito > sMassimo Then
                '        VisualizzaMessaggioInPopup("Importo troppo alto", Master)
                '        Exit Sub
                '    End If
                'End If

                'Dim idGioc As Integer = G.TornaIdGiocatore(Campi(1))
                'Dim Tipologia As String

                'If Campi(0) = "PAGAMENTO" Then
                '    Sql = "Update Bilancio Set Reali=Reali+" & sValore & " " & _
                '        "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                '    Tipologia = "D"
                'Else
                '    Sql = "Update Bilancio Set Presi=Presi+" & sValore & " " & _
                '        "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                '    Tipologia = "A"
                'End If
                'Db.EsegueSql(ConnSQL, Sql)

                'ScriveMovimentoBilancio(Db, ConnSQL, idGioc, sValore, Tipologia, "")

                'Dim gm As New GestioneMail

                'Dim Bilancio As String = "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & ">"
                'Dim Bilancione As Single
                'Dim Riga As String = "Tipologia;Importo;"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga, True)
                'Dim Rec As Object = Server.CreateObject("ADODB.Recordset")

                'Sql = "Select * From Bilancio Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                'Rec = Db.LeggeQuery(ConnSQL, Sql)

                'Riga = "Totale versamento;" & Rec("TotVersamento").Value & ";"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga)

                'Riga = "Totale soldi versati;" & Rec("Reali").Value & ";"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga)

                'Riga = "Totale soldi vinti;" & Rec("Vinti").Value & ";"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga)

                'Riga = "Totale soldi prelevati;" & Rec("Presi").Value & ";"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga)

                'Bilancione = (Rec("Reali").Value - Rec("Vinti").Value) - Rec("Presi").Value

                'Riga = "Bilancio;" & ScriveNumeroFormattato(Bilancione) & ";"
                'Bilancio += gm.ConverteTestoInRigaTabella(Riga)

                'Rec.Close()

                'Bilancio += "</table>"

                'If Campi(0) = "PAGAMENTO" Then
                '    gm.InviaMailPerPagamentoGiocatore(sValore, idGioc, Session("CodGiocatore"), Bilancio)
                'Else
                '    gm.InviaMailPerIncassoGiocatore(sValore, idGioc, Session("CodGiocatore"), Bilancio)
                'End If

                'gm = Nothing
            Else
                Dim idGioc As Integer = G.TornaIdGiocatore(hdnUtente.Value)
                Dim Tipologia As String = ""
                Dim Bilancio As String = ""
                Dim gm As New GestioneMail

                If hdnModalita.Value = "MODIFICATOTALE" Then
                    Sql = "Update Bilancio Set TotVersamento=" & sValore & " Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                End If

                If hdnModalita.Value = "PAGAMENTOGIOCATORE" Then
                    Sql = "Update Bilancio Set Reali=Reali+" & sValore & " " & _
                        "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                    Tipologia = "D"

                    Bilancio = RitornaBilancio(gm, Db, ConnSQL, idGioc, Val(sValore))

                    gm.InviaMailPerPagamentoGiocatore(sValore, idGioc, Session("CodGiocatore"), Bilancio)
                End If

                If hdnModalita.Value = "INCASSOGIOCATORE" Then
                    Dim Campi() As String = hdnTipoCampo.Value.Split(";")
                    Dim sInserito As Single = txtCampo.Text.Replace(".", ".")
                    Dim sMassimo As Single = Campi(2).Replace(".", ".")

                    If sInserito > sMassimo Then
                        Dim Messaggi() As String = {"Importo troppo alto"}
                        VisualizzaMessaggioInPopup(Messaggi, Me)
                        Exit Sub
                    End If

                    Sql = "Update Bilancio Set Presi=Presi+" & sValore & " Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
                    Tipologia = "A"

                    Bilancio = RitornaBilancio(gm, Db, ConnSQL, idGioc, -Val(sValore))

                    gm.InviaMailPerIncassoGiocatore(sValore, idGioc, Session("CodGiocatore"), Bilancio)
                End If

                If Sql <> "" Then
                    Db.EsegueSql(ConnSQL, Sql)

                    If Tipologia <> "" Then
                        ScriveMovimentoBilancio(Db, ConnSQL, idGioc, sValore, Tipologia, "")
                    End If
                End If
            End If

            G = Nothing
        End If

        Db = Nothing

        clsCash(Session("CodGiocatore")).CaricaDati(grdBilancio, grdVittorie, Session("Permesso"), Session("CodGiocatore"))

        divTesto.Visible = False
    End Sub

    Private Function RitornaBilancio(gm As GestioneMail, Db As GestioneDB, ConnSql As Object, idGioc As Integer, Importo As Single) As String
        Dim Bilancio As String = "<table border-left=" & Chr(34) & "1" & Chr(34) & " cellpadding=" & Chr(34) & "3" & Chr(34) & " cellspacing=" & Chr(34) & "3" & Chr(34) & ">"
        Dim Bilancione As Single
        Dim Riga As String = "Tipologia;Importo;"
        Bilancio += gm.ConverteTestoInRigaTabella(Riga, True)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select * From Bilancio Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc
        Rec = Db.LeggeQuery(ConnSql, Sql)

        Riga = "Totale versamento;" & Rec("TotVersamento").Value & ";"
        Bilancio += gm.ConverteTestoInRigaTabella(Riga)

        If Importo > 0 Then
            Riga = "Totale soldi versati;" & Val(Rec("Reali").Value) + Importo & ";"
            Bilancio += gm.ConverteTestoInRigaTabella(Riga)
        End If

        Riga = "Totale soldi vinti;" & Rec("Vinti").Value & ";"
        Bilancio += gm.ConverteTestoInRigaTabella(Riga)

        If Importo < 0 Then
            Riga = "Totale soldi prelevati;" & Val(Rec("Presi").Value) & ";"
            Bilancio += gm.ConverteTestoInRigaTabella(Riga)
        End If

        Dim v As Single = ((Rec("Reali").Value + Rec("Vinti").Value) + Importo) - Rec("Presi").Value
        Bilancione = Rec("TotVersamento").Value - v

        Riga = "Bilancio;" & ScriveNumeroFormattato(Bilancione) & ";"
        Bilancio += gm.ConverteTestoInRigaTabella(Riga)

        Rec.Close()

        Bilancio += "</table>"

        Return Bilancio
    End Function

    Protected Sub VisualizzaDettaglio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(1).Text

        hdnUtente.Value = Utente
        hdnModalita.Value = "VISUALIZZADETTAGLIO"
        LeggeDettaglio(Utente)

        divDettaglio.Visible = True
    End Sub

    Private Sub LeggeDettaglio(Utente As String)
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.RecordSet")
            Dim Sql As String
            Dim g As New Giocatori
            Dim idGioc As String = g.TornaIdGiocatore(Utente)
            g = Nothing

            Dim DataOra As New DataColumn("DataOra")
            Dim Importo As New DataColumn("Importo")
            Dim Tipologia As New DataColumn("Tipologia")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(DataOra)
            dttTabella.Columns.Add(Importo)
            dttTabella.Columns.Add(Tipologia)

            Sql = "Select * From BilancioDettaglio " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & idGioc & " " & _
                "Order By Progressivo"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                riga = dttTabella.NewRow()
                riga(0) = Rec("DataOra").Value
                riga(1) = Rec("Importo").Value
                If Rec("Tipologia").Value = "A" Then
                    riga(2) = "Incasso"
                Else
                    riga(2) = "Pagamento"
                End If
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdDettaglio.DataSource = dttTabella
            grdDettaglio.DataBind()
        End If

        Db = Nothing
    End Sub

    Private Sub grdDettaglio_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDettaglio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Tipologia As String = e.Row.Cells(2).Text
            Dim img As Image = DirectCast(e.Row.FindControl("imgTipologia"), Image)
            Dim Path As String = ""

            If Tipologia = "Pagamento" Then
                Path = "App_Themes/Standard/Images/Icone/Pagamento.png"
            Else
                Path = "App_Themes/Standard/Images/Icone/Incasso.png"
            End If

            img.ImageUrl = Path
        End If
    End Sub

    Private Sub cmdOkD_Click(sender As Object, e As EventArgs) Handles cmdChiudeDettaglio.Click
        divDettaglio.Visible = False
    End Sub

    Private Sub grdDettaglio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDettaglio.PageIndexChanging
        grdDettaglio.PageIndex = e.NewPageIndex
        grdDettaglio.DataBind()

        LeggeDettaglio(hdnUtente.Value)
    End Sub

    Protected Sub cmdPaga_Click(sender As Object, e As EventArgs) Handles cmdPaga.Click
        If IsNumeric(txtImporto.Text) = False Then
            Dim Messaggi() As String = {"Inserire un importo valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            If txtImporto.Text.IndexOf(",") > -1 Or txtImporto.Text.IndexOf(".") > -1 Then
                Dim Messaggi() As String = {"Inserire importi senza decimali"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            Else
                If Val(txtImporto.Text) = 0 Then
                    Dim Messaggi() As String = {"Inserire un importo superiore a 0"}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                    Exit Sub
                End If
            End If
        End If

        Session("Amount") = txtImporto.Text
        Session("request_id") = "1"

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     var myWindow = window.open('PagaPayPal.aspx', 'Pagamenti TotoMIO', 'width=980, height=800');")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)

        'Response.Redirect("PagaPayPal.aspx")
    End Sub

    Protected Sub imgMostraNascondePag_Click(sender As Object, e As ImageClickEventArgs) Handles imgMostraNascondePag.Click
        If divPagamento.Visible = True Then
            divPagamento.Visible = False
        Else
            divPagamento.Visible = True
        End If
    End Sub

    Protected Sub ModificaTotale(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(1).Text
        Dim Importo As String = row.Cells(2).Text

        hdnUtente.Value = Utente
        hdnModalita.Value = "MODIFICATOTALE"
        lblCampo.Text = "Selezionare il nuovo importo"
        txtCampo.Text = Importo

        divTesto.Visible = True
    End Sub
End Class