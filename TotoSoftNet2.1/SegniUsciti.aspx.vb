Imports System.IO

Public Class SegniUsciti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CreaPartite()
            CreaTabella()
            divDettaglio.Visible = False
        End If
    End Sub

    Private Sub CreaPartite()
        Dim Sql As String
        Dim gg As New Griglie

        Sql = "Select Partita, SquadraCasa As Casa, SquadraFuori As Fuori From Schedine Where " _
            & "Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & " Order By Partita"
        gg.ImpostaCampi(Sql, grdPartite)
        gg = Nothing
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As Label = DirectCast(e.Row.FindControl("TxtCasa"), Label)
            Dim TxtFuori As Label = DirectCast(e.Row.FindControl("TxtFuori"), Label)
            'Dim TxtRis As TextBox = DirectCast(e.Row.FindControl("TxtRisultato"), TextBox)
            'Dim TxtSegno As TextBox = DirectCast(e.Row.FindControl("TxtSegno"), TextBox)
            'Dim Checketto As CheckBox = DirectCast(e.Row.FindControl("chkSospesa"), CheckBox)
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim ImgJolly As Image = DirectCast(e.Row.FindControl("imgJolly"), Image)
            Dim numRiga As Integer = e.Row.Cells(0).Text
            Dim lblSerieC As Label = DirectCast(e.Row.FindControl("lblSerieC"), Label)
            Dim lblSerieF As Label = DirectCast(e.Row.FindControl("lblSerieF"), Label)
            'Dim Goal1 As Integer
            'Dim Goal2 As Integer
            Dim Segno As String = ""
            Dim Db As New GestioneDB
            Dim gM As New GestioneMail

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String


                Sql = "Select A.* From Schedine A " &
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
                    "A.Giornata=" & DatiGioco.Giornata & " And " &
                    "A.Partita=" & numRiga
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    TxtCasa.Text = Rec("SquadraCasa").Value
                    TxtFuori.Text = Rec("SquadraFuori").Value
                Else
                    TxtCasa.Text = ""
                    TxtFuori.Text = ""
                End If
                Rec.Close()

                'Sql = "Select B.Campionato From SquadreCampionati A Left Join Campionati B On A.idCampionato=B.idCampionato Where A.Squadra='" & TxtCasa.Text.Replace("'", "''") & "'"
                'Rec = Db.LeggeQuery(ConnSQL, Sql)
                'If Rec.Eof = False Then
                '    lblSerieC.Text = Rec("Campionato").Value
                'Else
                '    lblSerieC.Text = ""
                'End If
                'Rec.Close()

                'Sql = "Select B.Campionato From SquadreCampionati A Left Join Campionati B On A.idCampionato=B.idCampionato Where A.Squadra='" & TxtFuori.Text.Replace("'", "''") & "'"
                'Rec = Db.LeggeQuery(ConnSQL, Sql)
                'If Rec.Eof = False Then
                '    lblSerieF.Text = Rec("Campionato").Value
                'Else
                '    lblSerieF.Text = ""
                'End If
                'Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " & _
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtCasa.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieC.Text = Rec("Descrizione").Value
                Else
                    lblSerieC.Text = ""
                End If
                Rec.Close()

                Sql = "Select B.Descrizione From Classifiche A " & _
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " & _
                    "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtFuori.Text.ToUpper.Trim) & "'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    lblSerieF.Text = Rec("Descrizione").Value
                Else
                    lblSerieF.Text = ""
                End If
                Rec.Close()

                Dim Path As String

                Path = gM.RitornaImmagineSquadra(TxtCasa.Text, True)
                ' Path = "App_Themes/Standard/Images/Stemmi/" & TxtCasa.Text & ".jpg"

                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgCasa.ImageUrl = Path
                Else
                    ImgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = gM.RitornaImmagineSquadra(TxtFuori.Text, True)
                ' Path = "App_Themes/Standard/Images/Stemmi/" & TxtFuori.Text & ".jpg"
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgFuori.ImageUrl = Path
                Else
                    ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = "App_Themes/Standard/Images/Icone/Jolly.png"

                If numRiga = DatiGioco.PartitaJolly Then
                    ImgJolly.ImageUrl = Path
                    ImgJolly.Visible = True
                Else
                    ImgJolly.ImageUrl = ""
                    ImgJolly.Visible = False
                End If

                ConnSQL.Close()
            End If

            Db = Nothing
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Sql As String = ""

        Sql = "Select Squadra, Segno, Sum(Quante) As QuanteVolte From ( " _
            & "Select SquadraCasa + ' in casa' As Squadra, Segno, Count(*) As Quante From Schedine " _
            & "Where Anno=" & DatiGioco.AnnoAttuale & " And  " _
            & "SquadraCasa In  " _
            & "(Select SquadraCasa From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & ")  " _
            & "And Giornata<" & DatiGioco.Giornata & " And Segno='1' " _
            & "Group By SquadraCasa,Segno " _
            & "Union All " _
            & "Select SquadraCasa + ' in casa' As Squadra, Segno, Count(*) As Quante From Schedine  " _
            & "Where Anno=" & DatiGioco.AnnoAttuale & " And  " _
            & "SquadraCasa In  " _
            & "(Select SquadraCasa From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & ")  " _
            & "And Giornata<" & DatiGioco.Giornata & " And Segno='X' " _
            & "Group By SquadraCasa,Segno " _
            & "Union All " _
            & "Select SquadraFuori + ' fuori' As Squadra, Segno, Count(*) As Quante From Schedine  " _
            & "Where Anno=" & DatiGioco.AnnoAttuale & " And  " _
            & "SquadraFuori In  " _
            & "(Select SquadraFuori From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & ")  " _
            & "And Giornata<" & DatiGioco.Giornata & " And Segno='X' " _
            & "Group By SquadraFuori,Segno " _
            & "Union All " _
            & "Select SquadraFuori + ' fuori' As Squadra, Segno, Count(*) As Quante From Schedine  " _
            & "Where Anno=" & DatiGioco.AnnoAttuale & " And  " _
            & "SquadraFuori In  " _
            & "(Select SquadraFuori From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & DatiGioco.Giornata & ")  " _
            & "And Giornata<" & DatiGioco.Giornata & " And Segno='2' " _
            & "Group By SquadraFuori,Segno " _
            & ") A " _
            & "Group By Squadra, Segno " _
            & "Order By Squadra, Segno"

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Squadra() As String
            Dim Segno1() As Integer
            Dim SegnoX() As Integer
            Dim Segno2() As Integer
            Dim Quante As Integer = 0
            Dim Vecchia As String = ""

            Dim nSquadra As New DataColumn("Squadra")
            Dim nSegno1 As New DataColumn("Segno1")
            Dim nSegnoX As New DataColumn("SegnoX")
            Dim nSegno2 As New DataColumn("Segno2")

            Dim riga As DataRow
            Dim dttTabella As New DataTable()

            dttTabella.Columns.Add(nSquadra)
            dttTabella.Columns.Add(nSegno1)
            dttTabella.Columns.Add(nSegnoX)
            dttTabella.Columns.Add(nSegno2)

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            'If Rec.Eof = False Then
            '    Quante += 1
            '    redim Preserve Squadra(Quante)
            '    redim Preserve Segno1(Quante)
            '    redim Preserve SegnoX(Quante)
            '    redim Preserve Segno2(Quante)

            '    Squadra(Quante) = Rec("Squadra").Value
            '    Select Case Rec("Segno").value
            '        Case "1"
            '            Segno1(Quante) += Rec("QuanteVolte").Value
            '        Case "X"
            '            SegnoX(Quante) += Rec("QuanteVolte").Value
            '        Case "2"
            '            Segno2(Quante) += Rec("QuanteVolte").Value
            '    End Select

            '    Vecchia = Rec("Squadra").Value
            'End If
            Vecchia = "*"
            Do Until Rec.Eof
                If Rec("Squadra").Value <> Vecchia Then
                    Quante += 1
                    redim Preserve Squadra(Quante)
                    redim Preserve Segno1(Quante)
                    redim Preserve SegnoX(Quante)
                    redim Preserve Segno2(Quante)

                    Squadra(Quante) = Rec("Squadra").Value

                    Vecchia = Rec("Squadra").Value
                End If
                Select Case Rec("Segno").value
                    Case "1"
                        Segno1(Quante) += Rec("QuanteVolte").Value
                    Case "X"
                        SegnoX(Quante) += Rec("QuanteVolte").Value
                    Case "2"
                        Segno2(Quante) += Rec("QuanteVolte").Value
                End Select

                Rec.MoveNext
            Loop
            Rec.Close

            For i As Integer = 1 To Quante
                riga = dttTabella.NewRow()
                riga(0) = Squadra(i)
                riga(1) = Segno1(i)
                riga(2) = SegnoX(i)
                riga(3) = Segno2(i)
                dttTabella.Rows.Add(riga)
            Next

            grdSquadre.DataSource = dttTabella
            grdSquadre.DataBind()

            ConnSQL.Close
        End If

        Db = Nothing
    End Sub

    Private Sub grdSquadre_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSquadre.PageIndexChanging
        grdSquadre.PageIndex = e.NewPageIndex
        grdSquadre.DataBind()

        CreaTabella()
    End Sub

    Private Sub grdSquadre_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdSquadre.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgSquadra"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            Nome = Nome.Replace("in casa", "").Trim
            Nome = Nome.Replace("fuori", "").Trim
            Dim gm As New GestioneMail

            ImgUtente.ImageUrl = gm.RitornaImmagineSquadra(Nome, True)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub CreaDettaglio()
        Dim Sql As String
        Dim gr As New Griglie
        Dim Squadra As String = hdnSquadra.Value
        Dim Cosa As String

        If hdnDove.Value = "in casa" Then
            Cosa = "A.SquadraCasa = '" & Squadra & "' And Segno <>'2' "
        Else
            Cosa = "A.SquadraFuori = '" & Squadra & "' And Segno <>'1' "
        End If

        Sql = "Select A.Giornata, A.SquadraCasa+'-'+A.SquadraFuori As Partita, A.Segno, A.Risultato " _
            & "From Schedine A " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And  " & Cosa & " And Segno<>'S' " _
            & "And A.Giornata<" & DatiGioco.Giornata & " Order By A.Giornata"
        gr.ImpostaCampi(Sql, grdDettaglio)
        gr = Nothing
    End Sub

    Private Sub grdDettaglio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDettaglio.PageIndexChanging
        grdDettaglio.PageIndex = e.NewPageIndex
        grdDettaglio.DataBind()

        CreaDettaglio()
    End Sub

    Protected Sub VisualizzaDettaglio(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnSquadra.Value = row.Cells(1).Text
        hdnSquadra.Value = hdnSquadra.Value.Replace("in casa", "").Trim
        hdnSquadra.Value = hdnSquadra.Value.Replace("fuori", "").Trim
        hdnDove.Value = row.Cells(1).Text.Replace(hdnSquadra.Value, "").Trim

        CreaDettaglio()

        divDettaglio.Visible = True
    End Sub

    Protected Sub cmdChiudeDettaglio_Click(sender As Object, e As EventArgs) Handles cmdChiudeDettaglio.Click
        divDettaglio.Visible = False
    End Sub
End Class