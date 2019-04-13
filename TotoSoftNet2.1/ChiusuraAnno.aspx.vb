Public Class ChiusuraAnno
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
        End If
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
        clsCash(Session("CodGiocatore")).CaricamentoRigaBilancio(sender, e, Server.MapPath("."), Session("Permesso"), False)
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdChiudeAnno.Click
        Dim TestoMail As String = ""
        Dim gMail As New GestioneMail
        Dim Riga As String
        Dim Nome As String
        Dim totPag As String
        Dim Pagati As String
        Dim Vinti As String
        Dim Incassati As String
        Dim Amici As String
        Dim Bilancio As String
        Dim Vincitore As String
        Dim Path1 As String = ""

        TestoMail += gMail.ApreTestoTitolo() & "Dettaglio Vincitori - Fine Anno" & gMail.ChiudeTesto & "<hr />"
        TestoMail += gMail.ApreTabella
        Riga = ";Competizione;Importo;Vincitore;;"
        TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
        For i As Integer = 2 To grdVittorie.Rows.Count - 1
            Nome = grdVittorie.Rows(i).Cells(1).Text
            Vinti = grdVittorie.Rows(i).Cells(2).Text
            Vincitore = grdVittorie.Rows(i).Cells(3).Text

            Select Case Nome.ToUpper.Trim
                Case "SALDO ATTUALE"
                    Path1 = "App_themes/Standard/Images/Icone/Incasso.png"
                Case "CAMPIONATO"
                    Path1 = "App_themes/Standard/Images/Icone/Campionato.png"
                Case "CLASS. RISULTATI"
                    Path1 = "App_themes/Standard/Images/Icone/Risultati.png"
                Case "COPPA ITALIA"
                    Path1 = "App_themes/Standard/Images/Icone/CoppaItalia.png"
                Case "CHAMPION'S LEAGUE"
                    Path1 = "App_themes/Standard/Images/Icone/Champions.png"
                Case "EUROPA LEAGUE"
                    Path1 = "App_themes/Standard/Images/Icone/EuropaLeague.png"
                Case "PIPPETTERO"
                    Path1 = "App_themes/Standard/Images/Icone/Pippettero.png"
                Case "SUPERCOPPA ITALIANA"
                    Path1 = "App_themes/Standard/Images/Icone/SuperCoppaItaliana.png"
                Case "SUPERCOPPA EUROPEA"
                    Path1 = "App_themes/Standard/Images/Icone/SuperCoppaEuropea.png"
                Case "SUDDEN DEATH"
                    Path1 = "App_themes/Standard/Images/Icone/SuddenDeath.png"
                Case "PRIMO", "SECONDO", "TERZO"
                    Path1 = "App_themes/Standard/Images/Icone/Generale.png"
            End Select

            Path1 = "<img src='" & Path1 & "' width='45px' height='45px' />"

            Riga = Path1 & ";" & Nome & ";" & Vinti & ";" & Vincitore & ";" & gMail.RitornaImmagineGiocatore(Vincitore) & ";"
            TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
        Next
        TestoMail += "</table> "

        'TestoMail += "<hr />"

        'TestoMail += gMail.ApreTestoTitolo() & "Bilancio Giocatori - Fine Anno" & gMail.ChiudeTesto & "<hr />"
        'TestoMail += gMail.ApreTabella
        'Riga = ";Giocatore;Tot.Pagamenti;Pagati;Vinti;Incassati;Amici;Bilancio;"
        'TestoMail += gMail.ConverteTestoInRigaTabella(Riga, True)
        'For i As Integer = 0 To grdBilancio.Rows.Count - 1
        '    Nome = grdBilancio.Rows(i).Cells(1).Text
        '    totPag = grdBilancio.Rows(i).Cells(2).Text
        '    Pagati = grdBilancio.Rows(i).Cells(3).Text
        '    Vinti = grdBilancio.Rows(i).Cells(4).Text
        '    Incassati = grdBilancio.Rows(i).Cells(5).Text
        '    Amici = grdBilancio.Rows(i).Cells(7).Text
        '    Bilancio = grdBilancio.Rows(i).Cells(6).Text

        '    Riga = gMail.RitornaImmagineGiocatore(Nome) & ";" & Nome & ";" & totPag & ";" & Pagati & ";" & Vinti & ";" & Incassati & ";" & Amici & ";" & Bilancio & ";"
        '    TestoMail += gMail.ConverteTestoInRigaTabella(Riga)
        'Next
        'TestoMail += "</table> "

        gMail.InviaMailChiusuraAnno(TestoMail)

        gMail = Nothing

        DatiGioco.StatoConcorso = ValoriStatoConcorso.AnnoChiuso
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            AggiornaDatiDiGioco(Db)

            ConnSQL.Close()
        End If
        Db = Nothing

        ' VisualizzaMessaggioInPopup("Anno chiuso", Master)
        Response.Redirect("Principale.aspx")
    End Sub
End Class