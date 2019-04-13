Imports System.IO

Public Class TsnII
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Nome As String = "" & Session("Nick")

        If Nome <> "" Then
            VisualizzaContatore()
        Else
            divContatore.Visible = False
        End If

        'utilizzato per visualizzare eventi
        'lblScroll.Text = ""
        'ControllaCalendario()
        'utilizzato per visualizzare eventi

        If Page.IsPostBack = False Then
            Dim NomePagina As String = Request.CurrentExecutionFilePath

            For i As Integer = Len(NomePagina) To 1 Step -1
                If Mid(NomePagina, i, 1) = "/" Then
                    NomePagina = Mid(NomePagina, i + 1, Len(NomePagina))
                    Exit For
                End If
            Next

            If "" & Session("Nick") = "" Or DatiGioco.AnnoAttuale = 0 Then
                If NomePagina.ToUpper.Trim <> "DEFAULT.ASPX" And NomePagina.ToUpper.Trim <> "NUOVOUTENTE.ASPX" And NomePagina.ToUpper.Trim <> "REGOLAMENTO.ASPX" Then
                    Response.Redirect("Default.aspx")
                    Exit Sub
                End If
            End If
            'ricordasi sfondo bianco.
            Dim Percorso As String = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/Sfondi/" & Nome '& "_BN.Jpg"
            Dim IconaMaschera As String = ""

            Dim Immagine As String
            Dim ImmagineStretchata As Boolean = False

            If File.Exists(Server.MapPath(Percorso & ".jpg")) = True Then
                Immagine = Percorso & ".jpg"
            Else
                'creare immagine bianca 
                Immagine = "App_Themes/Standard/Images/Main.jpg"
            End If

            Select Case NomePagina.ToUpper.Trim
                Case "DEFAULT.ASPX", "NUOVOUTENTE.ASPX"
                    idmenu.Visible = False
                    idnavigazione.Visible = False
                    idboxprofilo.Visible = False

                    Immagine = "App_Themes/Standard/Images/Anni/" & DatiGioco.AnnoAttuale & ".jpg"
                    If File.Exists(Server.MapPath(Immagine)) = False Then
                        Immagine = "App_Themes/Standard/Images/Main.jpg"
                    End If

                    idSfondo.Style.Add("background-size", "contain;")
                    idSfondo.Style.Add("background-repeat", "no-repeat;")
                    idSfondo.Style.Add("background-position", "top center;")
                    idSfondo.Style.Add("background", "transparent url('" & Immagine & "')")
                Case "REGOLAMENTO.ASPX"
                Case Else
                    idmenu.Visible = True
                    idnavigazione.Visible = True
                    idboxprofilo.Visible = True
                    ImpostaTastiMenu()
                    CaricaInfoGioc()
                    ControllaMessaggi()
            End Select

            If DatiGioco.Giornata <= 5 Then
                liQuoteCup.Visible = False
            Else
                liQuoteCup.Visible = True
            End If

            'imgMaschera.ImageUrl = IconaMaschera


            'lblInfoConc.Text = DatiGioco.Giornata
            'lblDescAnno.Text = "Anno " & DatiGioco.AnnoAttuale & "-" & DatiGioco.NomeCampionato
            'lblStato.Text = TornaStatoStringa()
            'If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
            '    lblChiusuraConc.Text = DatiGioco.ChiusuraConcorso
            'Else
            '    lblDescChiusura.Visible = False
            '    lblChiusuraConc.Visible = False
            'End If
        End If

    End Sub

    Private Sub MostraScrittaScorrevole()
        'If ScrittaScorrevole <> "" Then
        '    Dim sbs As System.Text.StringBuilder = New System.Text.StringBuilder()

        '    sbs.Append("<script type='text/javascript' language='javascript'>")
        '    sbs.Append("     scrolling('" & ScrittaScorrevole.Replace("'", " ") & "');")
        '    sbs.Append("</script>")

        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR2", sbs.ToString(), False)
        'Else
        '    lblScroll.Text = ""
        'End If
    End Sub

    Private Function SistemaNomeMaschera(Nome As String) As String
        Dim Ritorno As String = ""

        For i As Integer = 1 To Nome.Length
            If Asc(Mid(Nome, i, 1)) >= Asc("A") And Asc(Mid(Nome, i, 1)) <= Asc("Z") Then
                Ritorno += " " & Mid(Nome, i, 1)
            Else
                Ritorno += Mid(Nome, i, 1)
            End If
        Next

        Return Ritorno
    End Function

    Private Sub ImpostaTastiMenu()
        If "" & Session("Permesso") = "" Then
            liAdmin.Visible = False
            liConcorsi.Visible = False
        Else
            Select Case Session("Permesso")
                Case Permessi.Amministratore
                    liAdmin.Visible = True
                    liConcorsi.Visible = True
                    'lblTipologia.Text = "- Amministratore -"
                Case Permessi.Giocatore
                    liAdmin.Visible = False
                    liConcorsi.Visible = False
                    'lblTipologia.Text = ""
                Case Permessi.Cassiere
                    liAdmin.Visible = False
                    liConcorsi.Visible = False
                    'lblTipologia.Text = "- Cassiere -"
                Case Permessi.Aggiornatore
                    liAdmin.Visible = False
                    liConcorsi.Visible = False
                    'lblTipologia.Text = "- Aggiornatore -"
            End Select
        End If

        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Nessuno
                liPropria.Visible = False
            Case ValoriStatoConcorso.Aperto
                liPropria.Visible = True
            Case ValoriStatoConcorso.DaControllare
                liPropria.Visible = False
            Case ValoriStatoConcorso.Chiuso
                liPropria.Visible = False
            Case ValoriStatoConcorso.AnnoChiuso
                liPropria.Visible = False
        End Select

        liNewConc.Visible = False
        liCloseConc.Visible = False
        liUpdateConc.Visible = False
        liControlCon.Visible = False
        liControlloGiocatori.Visible = False
        liReopenConc.Visible = False
        liModConc.Visible = False
        liSegniUsciti.Visible = False
        liColonne.Visible = False
        liChiudeTutto.Visible = False
        liResoconto.Visible = False
        Select Case DatiGioco.StatoConcorso
            Case ValoriStatoConcorso.Aperto
                liResoconto.Visible = False
                liCloseConc.Visible = True
                liUpdateConc.Visible = True
                liSegniUsciti.Visible = True
            Case ValoriStatoConcorso.Chiuso
                If Session("Permesso") <> Permessi.Amministratore Then
                    If Not LeggeSeControllato() Then
                        Dim ControlloMailCompleto As String = LeggeSeControlloMailCompleto()

                        If ControlloMailCompleto = "N" Then
                            liControlloGiocatori.Visible = True
                        Else
                            liResoconto.Visible = True
                        End If
                    Else
                        liResoconto.Visible = True
                    End If
                Else
                    liResoconto.Visible = True
                End If

                liNewConc.Visible = True
                If DatiGioco.Giornata = 38 Then
                    liChiudeTutto.Visible = True
                End If
            Case ValoriStatoConcorso.DaControllare
                liResoconto.Visible = False
                liModConc.Visible = True
                liControlCon.Visible = True
                liReopenConc.Visible = True
                liColonne.Visible = True
            Case ValoriStatoConcorso.Nessuno
                liResoconto.Visible = False
                liNewConc.Visible = True
            Case ValoriStatoConcorso.AnnoChiuso
        End Select

        Dim p As String = "" & Session("Permesso")

        If p <> "" Then
            If DatiGioco.Giornata > 4 Or p = Permessi.Amministratore Then
                liAmico.Visible = False
            End If
        End If
    End Sub

    Private Function LeggeSeControlloMailCompleto() As String
        Dim Db As New GestioneDB
        Dim Ritorno As String = "S"

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Ritorno = Rec("InvioMailCompleta").Value
            End If
            Rec.Close()
        End If

        Return Ritorno
    End Function

    Private Function LeggeSeControllato() As Boolean
        Dim Db As New GestioneDB
        Dim Ritorno As Boolean = False

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From ControlliConcorsoGiocatore Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiocatore=" & Session("CodGiocatore") & " And idGiornata=" & DatiGioco.Giornata
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Ritorno = True
            End If
            Rec.Close()
        End If

        Return Ritorno
    End Function

    Private Sub CaricaInfoGioc()
        Dim Nome As String = Session("Nick")
        Dim Percorso As String = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/" & Nome & ".Jpg"

        lblNome.Text = Nome.ToUpper.Trim
        If File.Exists(Server.MapPath(Percorso)) = False Then
            Percorso = "App_Themes/Standard/Images/Giocatori/Sconosciuto.png"
        End If
        imgGioc.ImageUrl = Percorso

        'Dim Gioc As New Giocatori
        'Dim Dett As String = Gioc.TornaDettaglioGiocatore(Session("CodGiocatore"))
        'Gioc = Nothing
        'If Dett <> "" Then
        '    Dim Campi() As String = Dett.Split(";")

        '    lblVittorie.Text = Campi(0)
        '    lblSecondiPosti.Text = Campi(1)
        '    lblUltimiPosti.Text = Campi(2)
        '    lblTappi.Text = Campi(3)
        'Else
        '    lblVittorie.Text = ""
        '    lblSecondiPosti.Text = ""
        '    lblUltimiPosti.Text = ""
        '    lblTappi.Text = ""
        'End If

        'PrendeDatiCoppe()
    End Sub

    Private Sub PrendeDatiCoppe()
        'Dim Db As New GestioneDB

        'If Db.LeggeImpostazioniDiBase() = True Then
        '    Dim ConnSQL As Object = Db.ApreDB()
        '    Dim Sql As String
        '    Dim Rec As Object = Server.CreateObject("ADODB.RecordSet")

        '    liCL.Visible = True
        '    If DatiGioco.GiornataChampionsLeague > 6 Then
        '        lblCL.Text = RitornaTurnoCoppe(Db, ConnSQL, "PartiteChampionsTurni", DatiGioco.GiornataChampionsLeague)

        '        Sql = "Select B.Giocatore From PartiteChampionsTurni A " & _
        '            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
        '            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        '        Rec = Db.LeggeQuery(ConnSQL, Sql)
        '        If Rec.Eof = False Then
        '            lblCL.Text = "V: " & Rec("Giocatore").Value.ToString.ToUpper
        '        End If
        '        Rec.Close()

        '        If lblCL.Text = "" Then
        '            lblCL.Text = "Fase elimin."
        '        End If
        '    Else
        '        If DatiGioco.GiornataChampionsLeague > 0 Then
        '            lblCL.Text = "Giornata " & DatiGioco.GiornataChampionsLeague
        '        Else
        '            lblCL.Text = ""
        '            liCL.Visible = False
        '        End If
        '    End If

        '    liEL.Visible = True
        '    If DatiGioco.GiornataEuropaLeague > 5 Then
        '        lblEL.Text = RitornaTurnoCoppe(Db, ConnSQL, "PartiteEuropaLeagueTurni", DatiGioco.GiornataEuropaLeague)

        '        Sql = "Select B.Giocatore From PartiteEuropaLeagueTurni A " & _
        '            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
        '            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        '        Rec = Db.LeggeQuery(ConnSQL, Sql)
        '        If Rec.Eof = False Then
        '            lblEL.Text = "V: " & Rec("Giocatore").Value.ToString.ToUpper
        '        End If
        '        Rec.Close()

        '        If lblEL.Text = "" Then
        '            lblEL.Text = "Fase elimin."
        '        End If
        '    Else
        '        If DatiGioco.GiornataEuropaLeague > 0 Then
        '            lblEL.Text = "Giornata " & DatiGioco.GiornataEuropaLeague
        '        Else
        '            lblEL.Text = ""
        '            liEL.Visible = False
        '        End If
        '    End If

        '    liCI.Visible = True
        '    If DatiGioco.GiornataCoppaItalia > 0 Then
        '        lblCI.Text = RitornaTurnoCoppe(Db, ConnSQL, "PartiteCoppaItaliaTurni", DatiGioco.GiornataCoppaItalia)

        '        Sql = "Select B.Giocatore From PartiteCoppaItaliaTurni A " & _
        '            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
        '            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        '        Rec = Db.LeggeQuery(ConnSQL, Sql)
        '        If Rec.Eof = False Then
        '            lblCI.Text = "V: " & Rec("Giocatore").Value.ToString.ToUpper
        '        End If
        '        Rec.Close()

        '        If lblCI.Text = "" Then
        '            lblCI.Text = "Fase elimin."
        '        End If
        '    Else
        '        lblCI.Text = ""
        '        liCI.Visible = False
        '    End If

        '    liIT.Visible = True
        '    If DatiGioco.GiornataInterToto > 0 Then
        '        lblIT.Text = "Giornata " & DatiGioco.GiornataInterToto

        '        Sql = "Select B.Giocatore From PartiteIntertoto A " & _
        '            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
        '            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        '        Rec = Db.LeggeQuery(ConnSQL, Sql)
        '        If Rec.Eof = False Then
        '            lblIT.Text = "V: " & Rec("Giocatore").Value.ToString.ToUpper
        '        End If
        '        Rec.Close()

        '        If lblIT.Text = "" Then
        '            lblIT.Text = "Fase elimin."
        '        End If
        '    Else
        '        lblIT.Text = ""
        '        liIT.Visible = False
        '    End If

        '    liDER.Visible = True
        '    If DatiGioco.GiornataDerelitti > 0 Then
        '        lblDer.Text = "Giornata " & DatiGioco.GiornataDerelitti

        '        Sql = "Select B.Giocatore From PartiteDerelitti A " & _
        '            "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
        '            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        '        Rec = Db.LeggeQuery(ConnSQL, Sql)
        '        If Rec.Eof = False Then
        '            lblDer.Text = "V: " & Rec("Giocatore").Value.ToString.ToUpper
        '        End If
        '        Rec.Close()

        '        If lblDer.Text = "" Then
        '            lblDer.Text = "Fase elimin."
        '        End If
        '    Else
        '        lblDer.Text = ""
        '        liDER.Visible = False
        '    End If
        'End If

        'Db = Nothing
    End Sub

    'Protected Sub cmdOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOK.Click
    '    divBloccaFinestra.Visible = False
    '    divPopup.Visible = False
    'End Sub

    Private Sub ControllaCalendario()
        Dim Testo As String

        Dim ev As New Eventi
        Dim g As Integer

        If DatiGioco.StatoConcorso = ValoriStatoConcorso.Chiuso Then
            ScrittaScorrevole = "Prossimi eventi in calendario: "
            g = DatiGioco.Giornata + 1
        Else
            ScrittaScorrevole = "Eventi della giornata in calendario: "
            g = DatiGioco.Giornata
        End If
        Testo = ev.ControllaPresenzaEventi(True, g)

        ev = Nothing

        Dim Spazi As New String(".", 20)

        ScrittaScorrevole += " " & Spazi & " " & Testo & " " & Spazi & " "

        MostraScrittaScorrevole()
    End Sub

    Private Sub VisualizzaContatore()
        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select Sum(Accessi) From AccessiDett Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Session("CodGiocatore")
            Dim Quanti As String = "00000"
            Dim Accessi As Long

            Rec = DB.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                Accessi = Rec(0).Value
                Do While Accessi > 99999
                    Accessi -= 99999
                Loop
                Quanti = Format(Accessi, "00000")
            End If
            Rec.Close()

            Dim Perc As String = "App_Themes/Standard/Images/Icone/"

            img1.ImageUrl = Perc & Mid(Quanti, 1, 1) & ".png"
            img2.ImageUrl = Perc & Mid(Quanti, 2, 1) & ".png"
            img3.ImageUrl = Perc & Mid(Quanti, 3, 1) & ".png"
            img4.ImageUrl = Perc & Mid(Quanti, 4, 1) & ".png"
            img5.ImageUrl = Perc & Mid(Quanti, 5, 1) & ".png"

            ConnSQL.Close()
        End If

        DB = Nothing
    End Sub

    Private Sub ControllaMessaggi()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Count(*) From Messaggi " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And idDestinatario=" & Session("CodGiocatore") & " And Letto='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                If Rec(0).Value > 0 Then
                    lblContMessDaLeggere.Text = Rec(0).Value
                    span_dalegg.Visible = True
                Else
                    lblContMessDaLeggere.Text = ""
                    span_dalegg.Visible = False
                End If
            End If
            Rec.Close()

            Sql = "Select Count(*) From Messaggi " &
                "Where Anno=" & DatiGioco.AnnoAttuale & " And idDestinatario=" & Session("CodGiocatore") & " And Letto='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                AnteprimaMessaggi(Db, ConnSQL, "Hai " & Rec(0).Value & " nuovi messaggi")
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub AnteprimaMessaggi(db As GestioneDB, ConnSQL As Object, Quanti As String)
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim AntMessa As StringBuilder = New StringBuilder()
        Dim Testo As String

        AntMessa.Append("<ul class=""dropdown-menu messages"">")
        AntMessa.Append("   <li class=""dropdown-menu-title"" style=""margin-top: -36px; padding-bottom: 20px;"">")
        AntMessa.Append("	    <span>" & Quanti & "</span>")
        AntMessa.Append("	    <a href=""#refresh""></a>") ' <i class=""icon-repeat""></i>
        AntMessa.Append("   </li>")

        Sql = "SELECT TOP 5 b.Giocatore " &
                ",a.Testo " &
                ",a.DataInvio " &
                ",a.progressivo " &
                "FROM Messaggi a " &
                "left join Giocatori  b on a.idOrigine=b.CodGiocatore and a.Anno=b.Anno " &
                "where idDestinatario = " & Session("CodGiocatore") & " And a.anno =" & DatiGioco.AnnoAttuale & " " &
                "order by DataInvio desc"
        Rec = db.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof()
            Testo = Rec("Testo").Value
            If Testo.Length > 30 Then
                Testo = Mid(Testo, 1, 30) & "..."
            End If

            Dim Percorso As String = "App_Themes/Standard/Images/Giocatori/" & DatiGioco.AnnoAttuale & "/" & Rec("Giocatore").Value & ".Jpg"
            If File.Exists(Server.MapPath(Percorso)) = False Then
                Percorso = "App_Themes/Standard/Images/Giocatori/Sconosciuto.png"
            End If

            AntMessa.Append("<li><a href='LetturaMessaggi.aspx?idMessaggio=" & Rec("Progressivo").Value & "' style=""height: 40px;"">")
            AntMessa.Append(" <span class=""avatar""><img src=""" & Percorso & """ alt=""Avatar""></span>")
            AntMessa.Append(" <span class=""header""> </span>")
            AntMessa.Append("  <span class=""from""> " & Rec("Giocatore").Value & "</span>")
            AntMessa.Append("  <span class=""time"">" & Rec("DataInvio").Value & "</span>")
            AntMessa.Append("")
            AntMessa.Append(" <span class=""message"">" & Testo & "</span> ")
            AntMessa.Append("  </a>")
            AntMessa.Append("</li>  ")
            Rec.MoveNext()
        Loop
        Rec.Close()

        AntMessa.Append("<li>")
        AntMessa.Append("<a class=""dropdown-menu-sub-footer"" href=""LetturaMessaggi.aspx"">Tutti i messaggi</a>")
        AntMessa.Append("</li>")
        AntMessa.Append("</ul>")
        div_messaggi.InnerHtml = AntMessa.ToString
    End Sub
End Class