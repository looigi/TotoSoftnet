Public Class Calendario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaCalendario()
        End If
    End Sub

    Private Sub CaricaCalendario()
        Dim Sql As String = "Select Giornata, Descrizione From (" &
            "Select Giornata, Descrizione From [dbo].[Eventi] Where Anno=" & DatiGioco.AnnoAttuale & "  " &
            "Union All " &
            "Select Giornata, upper('Champion''s League girone A') As Descrizione From [dbo].[EventiChampionsA] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Champion''s League girone B') As Descrizione From [dbo].[EventiChampionsB] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Champion League turni') As Descrizione From [dbo].[EventiChampionsTurni] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Giornata Coppa Italia') As Descrizione From [dbo].[EventiCoppaItaliaTurni] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Torneo Er Pippettero') As Descrizione From [dbo].[EventiDerelitti] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Europa League') As Descrizione From [dbo].[EventiEuropaLeague] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Europa League turni') As Descrizione From [dbo].[EventiEuropaLeagueTurni] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Supercoppa Europea') As Descrizione From [dbo].[EventiSuperCoppaEuropeaTurni] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Supercoppa Italiana') As Descrizione From [dbo].[EventiSuperCoppaItalianaTurni] Where Anno=" & DatiGioco.AnnoAttuale & " " &
            "Union All " &
            "Select Giornata, upper('Torneo Intertoto') As Descrizione From [dbo].[EventiInterToto]) A Where Giornata>=" & DatiGioco.Giornata & " Order By Giornata"
        Dim Gr As New Griglie

        Gr.ImpostaCampi(Sql, grdCalendario)
        Gr = Nothing
    End Sub

    Private Sub grdCalendario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCalendario.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("img"), Image)
            Dim Tipo As String = e.Row.Cells(1).Text.ToUpper.Trim
            Dim Prefisso As String = "App_Themes/Standard/Images/Icone/"
            Dim Icona As String

            If Tipo.IndexOf("CHAMPION") > -1 Then
                Icona = Prefisso & "Champions.png"
            Else
                If Tipo.IndexOf("ITALIA") > -1 Then
                    Icona = Prefisso & "CoppaItalia.png"
                Else
                    If Tipo.IndexOf("PIPPETTERO") > -1 Then
                        Icona = Prefisso & "Pippettero.png"
                    Else
                        If Tipo.IndexOf("EUROPA") > -1 Then
                            Icona = Prefisso & "EuropaLeague.png"
                        Else
                            Icona = Prefisso & "Elabora.png"
                        End If
                    End If
                End If
            End If

            img.ImageUrl = Icona
        End If
    End Sub
End Class