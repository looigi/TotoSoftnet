Imports System.IO

Public Class ModificaUtente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            LeggeRuoli()
            LeggePagante()

            CreaTabella()

            divModifica.Visible = False
        End If
    End Sub

    Private Sub LeggePagante()
        cmbPagante.Items.Clear()
        cmbPagante.Items.Add("Sì")
        cmbPagante.Items.Add("No")
    End Sub

    Private Sub CreaTabella()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Numero As New DataColumn("Utente")
            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(Numero)

            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                riga = dttTabella.NewRow()
                riga(0) = Rec("Giocatore").Value.ToString.Trim.ToUpper
                dttTabella.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            grdUtenti.DataSource = dttTabella
            grdUtenti.DataBind()
        End If

        Db = Nothing
    End Sub

    Private Sub grdUtenti_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUtenti.PageIndexChanging
        grdUtenti.PageIndex = e.NewPageIndex
        grdUtenti.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdUtenti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUtenti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim ImgTasto As ImageButton = DirectCast(e.Row.FindControl("imgVai"), ImageButton)
            Dim Nome As String = e.Row.Cells(0).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub LeggeRuoli()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Tipologie Order By Descrizione"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            cmdPermessi.Items.Clear()
            Do Until Rec.Eof
                cmdPermessi.Items.Add(Rec("Descrizione").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub SelezionaUtente(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(0).Text.ToUpper.Trim

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.*, B.Descrizione From Giocatori A " & _
                "Left Join Tipologie B On A.idTipologia=B.idTipologia " & _
                "Where Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(Utente.Trim.ToUpper) & "' And Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            txtNick.Text = "" & Rec("Giocatore").Value
            txtPassword.Text = "" & DecriptaPassword(Rec("Password").Value)
            txtCognome.Text = "" & Rec("Cognome").Value
            txtNome.Text = "" & Rec("Nome").Value
            txtMotto.Text = "" & Rec("Testo").Value
            txtEMail.Text = "" & Rec("Email").Value
            If "" & Rec("Pagante").Value = "S" Then
                cmbPagante.Text = "Sì"
            Else
                cmbPagante.Text = "No"
            End If
            If Rec("InvioMailCompleta").Value = "S" Then
                chkControlloCompleto.Checked = True
            Else
                chkControlloCompleto.Checked = False
            End If
            cmdPermessi.Text = "" & Rec("Descrizione").Value
            Rec.Close()

            imgAvatar.ImageUrl = RitornaImmagine(Server.MapPath("."), Utente)
            'If File.Exists(Server.MapPath("App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/") & Utente & ".jpg") = True Then
            '    imgSfondo.ImageUrl = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & Utente & ".jpg"
            'Else
            '    imgSfondo.ImageUrl = "App_Themes/Standard/Images/Main.jpg"
            'End If

            lblUtente.Text = Utente

            divModifica.Visible = True

            hdnUtenteScelto.Value = Utente

            cmdEliminaColonna.Visible = False
            If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
                Sql = "Select COUNT(*) From Pronostici A " & _
                    "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore = B.CodGiocatore " & _
                    "Where Upper(Ltrim(Rtrim(B.Giocatore)))='" & SistemaTestoPerDB(Utente.ToUpper.Trim) & "' And A.Giornata=" & DatiGioco.Giornata & " And A.Anno=" & DatiGioco.AnnoAttuale
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec(0).Value Is DBNull.Value = False Then
                    cmdEliminaColonna.Visible = True
                End If
                Rec.Close()
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub SalvaImmagineGiocatore()
        If FileUpload1.HasFile = False Then
            'VisualizzaMessaggioInPopup("Selezionare un'immagine", Master)
            Exit Sub
        End If

        Dim Percorso As String
        Dim Nome As String = lblUtente.Text

        Percorso = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\"

        Dim f As New GestioneFilesDirectory

        f.CreaDirectoryDaPercorso(Percorso)

        f = Nothing

        Percorso += Nome & ".jpg"

        FileUpload1.SaveAs(Percorso)

        Dim gi As New GestioneImmagini
        gi.RidimensionaEArrotondaIcona(Percorso)
        gi = Nothing

        imgAvatar.ImageUrl = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/" & Nome & ".Jpg"

        CreaTabella()

        Dim Messaggi() As String = {"Avatar caricato"}
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub

    Protected Sub SalvaImmagineSfondo()
        'If FileUpload2.HasFile = False Then
        '    'VisualizzaMessaggioInPopup("Selezionare un'immagine", Master)
        '    Exit Sub
        'End If

        'Dim Percorso As String
        'Dim Nome As String = lblUtente.Text

        'Percorso = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\Sfondi\"

        'Dim f As New GestioneFilesDirectory

        'f.CreaDirectoryDaPercorso(Percorso)

        'f = Nothing

        'Dim PercorsoAppoggio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\Sfondi\" & Nome & ".Jpg"

        'FileUpload2.SaveAs(PercorsoAppoggio)

        'imgSfondo.ImageUrl = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & Nome & ".Jpg"

        'VisualizzaMessaggioInPopup("Immagine di sfondo caricata", Master)
    End Sub

    Private Function EffettuaControlli() As Boolean
        Dim Ok As Boolean = True
        Dim nErrori As Integer = 0
        Dim Errore() As String = {}

        If txtNick.Text = "" Then
            ReDim Preserve Errore(nErrori)
            Errore(nErrori) += "Nick non inserito"
            nErrori += 1
            Ok = False
        Else
            txtNick.Text = txtNick.Text.Replace("à", "a").Replace("è", "e").Replace("é", "e").Replace("ì", "i").Replace("ò", "o").Replace("ù", "u").Replace(" ", "_")
            For i As Integer = 1 To txtNick.Text.Length
                Dim c As String = Mid(txtNick.Text, i, 1)
                If LettereValide.IndexOf(c) = -1 Then
                    Dim Messaggi() As String = {"Caratteri non validi nel nick: " & c}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                    Ok = False
                    Exit For
                End If
            Next
            txtNick.Text = txtNick.Text.ToUpper
        End If
        If txtNome.Text = "" Then
            ReDim Preserve Errore(nErrori)
            Errore(nErrori) += "Nome non inserito"
            nErrori += 1
            Ok = False
        End If
        If txtCognome.Text = "" Then
            ReDim Preserve Errore(nErrori)
            Errore(nErrori) += "Cognome non inserito"
            nErrori += 1
            Ok = False
        End If
        If txtEMail.Text = "" Then
            ReDim Preserve Errore(nErrori)
            Errore(nErrori) += "E-Mail non inserita"
            nErrori += 1
            Ok = False
        Else
            If txtEMail.Text.IndexOf("@") = -1 Or txtEMail.Text.IndexOf(".") = -1 Or txtEMail.Text.Length < 5 Then
                ReDim Preserve Errore(nErrori)
                Errore(nErrori) += "E-Mail non valida"
                nErrori += 1
                Ok = False
            End If
        End If

        If Errore.Length > 0 Then
            VisualizzaMessaggioInPopup(Errore, Master)
        End If

        Return Ok
    End Function

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If EffettuaControlli() = True Then
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Sql As String
                Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
                Dim Pagante As String = IIf(cmbPagante.Text = "Sì", "S", "N")
                Dim Ruolo As Integer

                Sql = "Select * From Tipologie Where Descrizione='" & cmdPermessi.Text & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Ruolo = Rec("idTipologia").Value
                Rec.Close()

                Dim VecchioNome As String = lblUtente.Text.Trim.ToUpper
                Dim InviaMail As String

                If chkControlloCompleto.Checked Then
                    InviaMail = "S"
                Else
                    InviaMail = "N"
                End If

                Sql = "Update Giocatori Set " &
                    "Giocatore='" & SistemaTestoPerDB(txtNick.Text.ToUpper.Trim) & "', " &
                    "Nome='" & SistemaTestoPerDB(txtNome.Text) & "', " &
                    "Cognome='" & SistemaTestoPerDB(txtCognome.Text) & "', " &
                    "Testo='" & SistemaTestoPerDB(txtMotto.Text) & "', " &
                    "EMail='" & SistemaTestoPerDB(txtEMail.Text) & "', " &
                    "Password='" & SistemaTestoPerDB(CriptaPassword(txtPassword.Text)) & "', " &
                    "Pagante='" & Pagante & "', " &
                    "idTipologia=" & Ruolo & ", " &
                    "InvioMailCompleta='" & InviaMail & "' " &
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And Upper(Ltrim(Rtrim(Giocatore)))='" & lblUtente.Text.Trim.ToUpper & "'"
                Db.EsegueSql(ConnSQL, Sql)

                ConnSQL.Close()

                SalvaImmagineGiocatore()
                SalvaImmagineSfondo()

                If VecchioNome.ToUpper.Trim <> txtNick.Text.ToUpper.Trim Then
                    ' Sostituisco l'immagine per il nuovo nome
                    Dim Perc As String

                    Perc = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & DatiGioco.AnnoAttuale & "\"

                    Try
                        Rename(Perc & VecchioNome.ToUpper.Trim & ".jpg", Perc & txtNick.Text.ToUpper.Trim & ".jpg")
                    Catch ex As Exception

                    End Try

                    Try
                        Rename(Perc & "Sfondi\" & VecchioNome.ToUpper.Trim & ".jpg", Perc & "Sfondi\" & txtNick.Text.ToUpper.Trim & ".jpg")
                    Catch ex As Exception

                    End Try

                    imgAvatar.ImageUrl = RitornaImmagine(Server.MapPath("."), txtNick.Text.ToUpper.Trim)

                    'If File.Exists(Server.MapPath("App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/") & txtNick.Text.ToUpper.Trim & ".jpg") = True Then
                    '    imgSfondo.ImageUrl = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & txtNick.Text.ToUpper.Trim & ".jpg"
                    'Else
                    '    imgSfondo.ImageUrl = "App_Themes/Standard/Images/Main.jpg"
                    'End If

                    lblUtente.Text = txtNick.Text.ToUpper.Trim

                    CreaTabella()
                End If

                Dim Messaggi() As String = {"Dati giocatore modificati"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdElimina_Click(sender As Object, e As EventArgs) Handles cmdElimina.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Update Giocatori Set Cancellato='S' " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(lblUtente.Text.Trim.ToUpper) & "'"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabella()

            divModifica.Visible = False

            Dim Messaggi() As String = {"Giocatore eliminato"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdEliminaColonna_Click(sender As Object, e As EventArgs) Handles cmdEliminaColonna.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select CodGiocatore From Giocatori " &
                "Where  Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(hdnUtenteScelto.Value) & "' And Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim CodGiocatore As Integer = Rec("CodGiocatore").Value
            Rec.Close()

            Sql = "Delete Pronostici " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " And CodGiocatore=" & CodGiocatore
            Db.EsegueSql(ConnSQL, Sql)

            cmdEliminaColonna.Visible = False

            ConnSQL.Close()

            Dim Messaggi() As String = {"Colonna per il giocatore eliminata"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub
End Class