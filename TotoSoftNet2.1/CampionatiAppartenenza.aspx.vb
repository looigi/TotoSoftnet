Imports System.IO

Public Class CampionatiAppartenenza
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaCombo()
            RileggeSquadre()
            RileggeSerie()
        End If
    End Sub

    Private Sub CaricaCombo()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & PrefissoTabelle & "Campionati Order By Campionato"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            cmbSerie.Items.Clear()
            cmbSerie.Items.Add("")
            Do Until Rec.Eof
                cmbSerie.Items.Add(Rec("Campionato").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()
            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub RileggeSquadre()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select A.Squadra, C.Campionato From (" & _
            "Select Distinct SquadraCasa As Squadra From " & PrefissoTabelle & "Schedine " & _
            "Where Anno=" & DatiGioco.AnnoAttuale & " " & _
            "Union All " & _
            "Select Distinct SquadraFuori As Squadra From " & PrefissoTabelle & "Schedine " & _
            "Where Anno=" & DatiGioco.AnnoAttuale & " " & _
            ") A Left Join " & PrefissoTabelle & "SquadreCampionati B On A.Squadra = B.Squadra " & _
            "Left Join " & PrefissoTabelle & "Campionati C On B.idCampionato = C.idCampionato " & _
            "Order By A.Squadra"
        Gr.ImpostaCampi(Sql, grdSquadre)
        Gr = Nothing

        tabella.Visible = False
        hdnSquadra.Value = ""
        txtSquadra.Text = ""
        cmbSerie.Text = ""
    End Sub

    Private Sub RileggeSerie()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Campionato From " & PrefissoTabelle & "Campionati Order By Campionato"
        Gr.ImpostaCampi(Sql, grdSerie)
        Gr = Nothing

        tabella2.Visible = False
        hdnSerie.Value = ""
        txtSerie.Text = ""
    End Sub

    Private Sub grdSquadre_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSquadre.PageIndexChanging
        grdSquadre.PageIndex = e.NewPageIndex
        grdSquadre.DataBind()

        RileggeSquadre()
    End Sub

    Private Sub grdSerie_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSerie.PageIndexChanging
        grdSerie.PageIndex = e.NewPageIndex
        grdSerie.DataBind()

        RileggeSerie()
    End Sub

    Protected Sub ModificaNomeSquadra(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdSquadre.Rows(Riga)
        Dim Squadra As String = Di.Cells(0).Text
        Dim Serie As String = Di.Cells(1).Text

        If Serie = "&nbsp;" Then Serie = ""

        tabella.Visible = True
        hdnSquadra.Value = Squadra
        txtSquadra.Text = Squadra
        cmbSerie.Text = Serie
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim idC As Integer

            Sql = "Select * From " & PrefissoTabelle & "Campionati Where Campionato='" & cmbSerie.Text.Replace("'", "''") & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            idC = Rec(0).Value
            Rec.Close()

            Sql = "Select * From " & PrefissoTabelle & "SquadreCampionati Where Squadra='" & hdnSquadra.Value.Replace("'", "''") & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Sql = "Insert Into " & PrefissoTabelle & "SquadreCampionati Values (" & _
                    " '" & txtSquadra.Text.Replace("'", "''").Trim.ToUpper & "', " & _
                    " " & idC & ")"
            Else
                Sql = "Update " & PrefissoTabelle & "SquadreCampionati Set " & _
                    "Squadra='" & txtSquadra.Text.Replace("'", "''").Trim.ToUpper & "', " & _
                    "idCampionato=" & idC & " " & _
                    "Where Squadra='" & hdnSquadra.Value.Replace("'", "''") & "'"
            End If
            Rec.Close()

            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            RileggeSquadre()
        End If

        Db = Nothing
    End Sub

    Protected Sub ModificaNomeSerie(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdSerie.Rows(Riga)
        Dim Serie As String = Di.Cells(0).Text

        If Serie = "&nbsp;" Then Serie = ""

        tabella2.Visible = True
        hdnSerie.Value = Serie
        txtSerie.Text = Serie
    End Sub

    Protected Sub cmdSalvaS_Click(sender As Object, e As EventArgs) Handles cmdSalvaS.Click
        If txtSerie.Text <> "" Then
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Dim idC As String

                Sql = "Select Max(idCampionato)+1 From " & PrefissoTabelle & "Campionati"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec(0).Value Is DBNull.Value = True Then
                    idC = 1
                Else
                    idC = Rec(0).Value
                End If
                Rec.Close()

                If hdnSerie.Value = "" Then
                    Sql = "Insert Into " & PrefissoTabelle & "Campionati Values (" & idC & ", '" & txtSerie.Text.Replace("'", "''").ToUpper.Trim & "')"
                Else
                    Sql = "Select * From " & PrefissoTabelle & "Campionati Where Campionato='" & hdnSerie.Value.Replace("'", "''") & "'"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = True Then
                        Sql = "Insert Into " & PrefissoTabelle & "Campionati Values (" & idC & ", '" & txtSerie.Text.Replace("'", "''").ToUpper.Trim & "')"
                    Else
                        Sql = "Update " & PrefissoTabelle & "Campionati Set Campionato='" & txtSerie.Text.Replace("'", "''").ToUpper.Trim & "' Where Campionato='" & hdnSerie.Value.Replace("'", "''") & "'"
                    End If
                    Rec.Close()
                End If

                Db.EsegueSql(ConnSQL, Sql)

                ConnSQL.Close()

                CaricaCombo()
                RileggeSerie()
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub cmdNuovaSerie_Click(sender As Object, e As EventArgs) Handles cmdNuovaSerie.Click
        tabella2.Visible = True
        hdnSerie.Value = ""
        txtSerie.Text = ""
    End Sub

    'Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
    '    Response.Redirect("Amministrazione.aspx")
    'End Sub
End Class