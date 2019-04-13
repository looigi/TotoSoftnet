Imports System.IO

Public Class Colonne
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaNomi()
            hdnGiocatore.Value = Session("CodGiocatore")
            CaricaPartite()
            ScriveNome()
        End If
    End Sub

    Private Sub CaricaNomi()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N' Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            cmbGiocatore.Items.Clear()
            Do Until Rec.Eof
                cmbGiocatore.Items.Add(Rec("Giocatore").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub ScriveNome()
        Dim g As New Giocatori

        Dim Nome As String = g.TornaNickGiocatore(hdnGiocatore.Value)

        cmbGiocatore.Text = Nome
        imgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), cmbGiocatore.Text)

        g = Nothing
    End Sub

    Private Sub CaricaPartite()
        Dim Db As New GestioneDB
        Dim Speciale As Boolean
        Dim g As Integer

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                Speciale = True
                g = DatiGioco.GiornataSpeciale + 100
            Else
                Speciale = False
                g = DatiGioco.Giornata
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select A.Partita From Schedine A " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
            "A.Giornata=" & g & " Order By A.Partita"
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As TextBox = DirectCast(e.Row.FindControl("TxtCasa"), TextBox)
            Dim TxtFuori As TextBox = DirectCast(e.Row.FindControl("TxtFuori"), TextBox)
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim ImgJolly As Image = DirectCast(e.Row.FindControl("imgJolly"), Image)
            Dim ImgFR As Image = DirectCast(e.Row.FindControl("imgFR"), Image)
            Dim chk1 As CheckBox = DirectCast(e.Row.FindControl("chk1"), CheckBox)
            Dim chkX As CheckBox = DirectCast(e.Row.FindControl("chkX"), CheckBox)
            Dim chk2 As CheckBox = DirectCast(e.Row.FindControl("chk2"), CheckBox)
            Dim txtRis As TextBox = DirectCast(e.Row.FindControl("txtRisultato"), TextBox)
            Dim numRiga As Integer = e.Row.Cells(0).Text
            Dim lblSerieC As Label = DirectCast(e.Row.FindControl("lblSerieC"), Label)
            Dim lblSerieF As Label = DirectCast(e.Row.FindControl("lblSerieF"), Label)
            Dim Db As New GestioneDB
            Dim gm As New GestioneMail
            Dim filRouge As Integer

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String
                Dim g As Integer

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    g = DatiGioco.GiornataSpeciale + 100
                Else
                    g = DatiGioco.Giornata
                End If

                Sql = "Select A.* From Schedine A " &
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
                    "A.Giornata=" & g & " And " &
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

                ' Path = "App_Themes/Standard/Images/Stemmi/" & TxtCasa.Text & ".jpg"
                Path = gm.RitornaImmagineSquadra(TxtCasa.Text, True)
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgCasa.ImageUrl = Path
                    ImgCasa.Visible = True
                Else
                    ImgCasa.ImageUrl = ""
                    ImgCasa.Visible = False
                End If

                ' Path = "App_Themes/Standard/Images/Stemmi/" & TxtFuori.Text & ".jpg"
                Path = gm.RitornaImmagineSquadra(TxtFuori.Text, True)
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgFuori.ImageUrl = Path
                    ImgFuori.Visible = True
                Else
                    ImgFuori.ImageUrl = ""
                    ImgFuori.Visible = False
                End If

                chk1.Checked = False
                chkX.Checked = False
                chk2.Checked = False
                txtRis.Text = ""

                Sql = "Select A.* From Pronostici A " &
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
                    "A.Giornata=" & g & " And " &
                    "A.Partita=" & numRiga & " And " &
                    "A.CodGiocatore=" & hdnGiocatore.Value
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    txtRis.Text = "" & Rec("Risultato").Value

                    Dim Segni As String = "" & Rec("Segni").Value

                    Segni = Segni.ToUpper.Trim

                    If Segni.IndexOf("1") > -1 Then
                        chk1.Checked = True
                    End If
                    If Segni.IndexOf("X") > -1 Then
                        chkX.Checked = True
                    End If
                    If Segni.IndexOf("2") > -1 Then
                        chk2.Checked = True
                    End If
                End If
                Rec.Close()

                Path = "App_Themes/Standard/Images/Icone/Jolly.png"

                If numRiga = DatiGioco.PartitaJolly Then
                    ImgJolly.ImageUrl = Path
                    ImgJolly.Visible = True
                Else
                    ImgJolly.ImageUrl = ""
                    ImgJolly.Visible = False
                End If

                Sql = "Select * From FilsRouge " &
                    "Where " &
                    "Anno=" & DatiGioco.AnnoAttuale & " " &
                    "And Giornata=" & g & " " &
                    "And CodGiocatore=" & hdnGiocatore.Value
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                filRouge = -1
                If Rec.Eof = False Then
                    filRouge = Rec("FilRouge").Value
                End If
                Rec.Close()

                If filRouge = e.Row.Cells(0).Text Then
                    ImgFR.ImageUrl = "App_Themes/Standard/Images/Icone/Fortuna.png"
                    ImgFR.Visible = True
                Else
                    ImgFR.Visible = False
                End If

                ConnSQL.Close()
            End If

            Db = Nothing

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub cmbGiocatore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbGiocatore.SelectedIndexChanged
        Dim g As New Giocatori
        Dim Numerello As Integer = g.TornaIdGiocatore(cmbGiocatore.Text)

        hdnGiocatore.Value = Numerello
        CaricaPartite()
        ScriveNome()

        g = Nothing
    End Sub
End Class