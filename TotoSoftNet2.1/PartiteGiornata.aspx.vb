Imports System.IO

Public Class PartiteGiornata
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divRisultato.Visible = False
            hdnPartita.Value = ""

            Dim g As Integer

            Select Case DatiGioco.StatoConcorso
                Case ValoriStatoConcorso.Nessuno
                    g = 1
                Case ValoriStatoConcorso.Aperto
                    g = DatiGioco.Giornata - 1
                Case ValoriStatoConcorso.DaControllare
                    g = DatiGioco.Giornata - 1
                Case ValoriStatoConcorso.Chiuso
                    g = DatiGioco.Giornata
                Case ValoriStatoConcorso.AnnoChiuso
                    g = 38
            End Select

            'CreaTabella()
            CaricaPartite(g)
        End If
    End Sub

    'Private Sub CreaTabella()
    '    Dim Sql As String = ""

    '    Dim Db As New GestioneDB

    '    If Db.LeggeImpostazioniDiBase() = True Then
    '        Dim ConnSQL As Object = Db.ApreDB()
    '        Sql = "Create Table PartiteGiornata (Anno Int NOT NULL, Giornata Int NOT NULL, Partita Int NOT NULL, Casa Varchar(30), Fuori Varchar(30), Ris1 Int, Ris2 Int"
    '        Sql &= " Constraint [PK_PartiteGiornata] PRIMARY KEY CLUSTERED "
    '        Sql &= "( "
    '        Sql &= "	[Anno] Asc, "
    '        Sql &= "	[Giornata] Asc, "
    '        Sql &= "	[Partita] Asc "
    '        Sql &= ") WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] "
    '        Sql &= ") ON [PRIMARY]"
    '        Try
    '            Db.EsegueSqlSenzaTRY(ConnSQL, Sql)
    '        Catch ex As Exception
    '        End Try
    '    End If

    '    Db = Nothing
    'End Sub

    Private Sub CaricaPartite(Giornata As Integer)
        Dim Sql As String = ""
        Sql &= "Select Partita, Casa, Fuori, LTrim(Rtrim(Str(Ris1)))+'-'+Ltrim(Rtrim(Str(Ris2))) As Ris From PartiteGiornata "
        Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & Giornata & " Order By Partita"

        Dim Gr As New Griglie

        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing

        divRisultato.Visible = False
        hdnPartita.Value = ""

        lblGiornata.Text = "Giornata " & Giornata
        hdnGiornata.Value = Giornata
    End Sub

    Private Sub grdCalendario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim gM As New GestioneMail

            Dim Path As String

            Path = gM.RitornaImmagineSquadra(e.Row.Cells(1).Text, True)
            Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
            'Path = "App_Themes/Standard/Images/Stemmi/" & TxtCasa.Text & ".jpg"
            If File.Exists(Server.MapPath(Path)) = True Then
                ImgCasa.ImageUrl = Path
            Else
                ImgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
            End If

            Path = gM.RitornaImmagineSquadra(e.Row.Cells(3).Text, True)
            Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
            'Path = "App_Themes/Standard/Images/Stemmi/" & TxtFuori.Text & ".jpg"
            If File.Exists(Server.MapPath(Path)) = True Then
                ImgFuori.ImageUrl = Path
            Else
                ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
            End If
        End If
    End Sub

    Private Sub grdPartite_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPartite.PageIndexChanging
        grdPartite.PageIndex = e.NewPageIndex
        grdPartite.DataBind()

        CaricaPartite(Val(hdnGiornata.Value))
    End Sub

    Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        If Val(hdnGiornata.Value) > 1 Then
            CaricaPartite(Val(hdnGiornata.Value - 1))
        End If
    End Sub

    Protected Sub cmdAvanti_Click(sender As Object, e As EventArgs) Handles cmdAvanti.Click
        If Val(hdnGiornata.Value) < 38 Then
            CaricaPartite(Val(hdnGiornata.Value + 1))
        End If
    End Sub

    Protected Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        PrendePartiteDellaGiornata(Server.MapPath(".") & "\Appoggio", "Partite.txt", Val(hdnGiornata.Value))
        CaricaPartite(Val(hdnGiornata.Value))
    End Sub

    Protected Sub SelezionaPartita(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Partita As String = row.Cells(0).Text.ToUpper.Trim

        hdnPartita.Value = Partita

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String = ""

            Sql &= "Select Partita, Casa, Fuori, LTrim(Rtrim(Str(Ris1)))+'-'+Ltrim(Rtrim(Str(Ris2))) As Ris From PartiteGiornata "
            Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value & " And Partita=" & Partita & " "
            Sql &= "Order By Partita"

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Dim Path As String
                Dim gM As New GestioneMail

                Path = gM.RitornaImmagineSquadra(Rec("Casa").Value, True)
                Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
                If File.Exists(Server.MapPath(Path)) = True Then
                    imgCasa.ImageUrl = Path
                Else
                    imgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = gM.RitornaImmagineSquadra(Rec("Fuori").Value, True)
                Path = Path.Replace(PercorsoApplicazione & "\", "").Replace(PercorsoApplicazione & "/", "")
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgFuori.ImageUrl = Path
                Else
                    ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                gM = Nothing

                lblCasa.Text = Rec("Casa").Value
                lblFuori.Text = Rec("Fuori").Value
                txtRisultato.Text = Rec("Ris").Value

                txtCasa.Visible = False
                txtFuori.Visible = False
                lblCasa.Visible = True
                lblFuori.Visible = True
                cmdElimina.Visible = True

                divRisultato.Visible = True
            Else
                hdnPartita.Value = ""
                divRisultato.Visible = False
            End If
            Rec.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalvaRis.Click
        Dim Ris1 As Integer = -1
        Dim Ris2 As Integer = -1

        If txtRisultato.Text = "" Or txtRisultato.Text.IndexOf("-") = -1 Then
            Dim Messaggi() As String = {"Risultato non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        Else
            Dim a As Integer = txtRisultato.Text.IndexOf("-")
            Ris1 = Val(Mid(txtRisultato.Text, 1, a))
            Ris2 = Val(Mid(txtRisultato.Text, a + 2, txtRisultato.Text.Length))

            If IsNumeric(Ris1) = False Or IsNumeric(Ris2) = False Then
                Dim Messaggi() As String = {"Risultato non valido"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
        End If

        If hdnPartita.Value = "-1" Then
            If txtCasa.Text = "" Then
                Dim Messaggi() As String = {"Inserire la squadra di casa"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
            If txtFuori.Text = "" Then
                Dim Messaggi() As String = {"Inserire la squadra fuori casa"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Exit Sub
            End If
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String = ""

            If hdnPartita.Value = "-1" Then
                Dim Part As Integer = -1

                Sql = "Select Max(Partita)+1 From PartiteGiornata Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec(0).Value Is DBNull.Value Then
                    Part = 1
                Else
                    Part = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert Into PartiteGiornata Values ("
                Sql &= " " & DatiGioco.AnnoAttuale & ", "
                Sql &= " " & hdnGiornata.Value & ", "
                Sql &= " " & Part & ", "
                Sql &= "'" & SistemaTestoPerDB(MetteMaiuscole(txtCasa.Text.Trim)) & "', "
                Sql &= "'" & SistemaTestoPerDB(MetteMaiuscole(txtFuori.Text.Trim)) & "', "
                Sql &= " " & Ris1.ToString.Trim & ", "
                Sql &= " " & Ris2.ToString.Trim & " "
                Sql &= ")"
            Else
                Sql = "Update PartiteGiornata "
                Sql &= "Set Ris1=" & Ris1.ToString.Trim & ", "
                Sql &= "Ris2=" & Ris2.ToString.Trim & " "
                Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value & " And Partita=" & hdnPartita.Value
            End If

            Db.EsegueSql(ConnSQL, Sql)
        End If

        Db = Nothing

        CaricaPartite(Val(hdnGiornata.Value))
    End Sub

    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        imgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
        ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
        hdnPartita.Value = "-1"
        txtCasa.Visible = True
        txtFuori.Visible = True
        lblCasa.Visible = False
        lblFuori.Visible = False
        txtCasa.Text = ""
        txtFuori.Text = ""
        txtRisultato.Text = ""
        cmdElimina.Visible = False
        divRisultato.Visible = True
    End Sub

    Protected Sub cmdElimina_Click(sender As Object, e As EventArgs) Handles cmdElimina.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String = ""

            Sql &= "Delete From PartiteGiornata "
            Sql &= "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & hdnGiornata.Value & " And Partita=" & hdnPartita.Value

            Db.EsegueSql(ConnSQL, Sql)
        End If

        Db = Nothing

        CaricaPartite(Val(hdnGiornata.Value))
    End Sub
End Class