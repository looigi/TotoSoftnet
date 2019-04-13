Imports System.IO

Public Class AggiornaConcorso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaPartite()
        End If
    End Sub

    Private Sub CaricaPartite()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Db As New GestioneDB
        Dim gg As Integer

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            ConnSQL.Close()
        End If

        Sql = "Select A.Partita From Schedine A " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
            "A.Giornata=" & gg
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing
    End Sub

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ' e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As TextBox = DirectCast(e.Row.FindControl("TxtCasa"), TextBox)
            Dim TxtFuori As TextBox = DirectCast(e.Row.FindControl("TxtFuori"), TextBox)
            Dim TxtRis As TextBox = DirectCast(e.Row.FindControl("TxtRisultato"), TextBox)
            Dim TxtSegno As TextBox = DirectCast(e.Row.FindControl("TxtSegno"), TextBox)
            Dim Checketto As CheckBox = DirectCast(e.Row.FindControl("chkSospesa"), CheckBox)
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

                Dim gg As Integer

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    gg = DatiGioco.GiornataSpeciale + 100
                Else
                    gg = DatiGioco.Giornata
                End If

                Sql = "Select A.* From Schedine A " & _
                    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "A.Giornata=" & gg & " And " & _
                    "A.Partita=" & numRiga
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    TxtCasa.Text = Rec("SquadraCasa").Value
                    TxtFuori.Text = Rec("SquadraFuori").Value
                    TxtRis.Text = "" & Rec("Risultato").Value
                    TxtSegno.Text = "" & Rec("Segno").Value
                Else
                    TxtCasa.Text = ""
                    TxtFuori.Text = ""
                    TxtRis.Text = ""
                    TxtSegno.Text = ""
                End If
                Rec.Close()

                If TxtSegno.Text = "S" Then
                    Checketto.Checked = True
                Else
                    Checketto.Checked = False
                End If

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

            ' e.Row.Cells(0).Visible = False
        End If
    End Sub

    Private Function EffettuaControlli() As Boolean
        Dim Ok As Boolean = True
        Dim txtRis As TextBox
        Dim txtSegno As TextBox
        Dim chk As CheckBox
        Dim Goal1 As String
        Dim Goal2 As String

        For i As Integer = 0 To grdPartite.Rows.Count - 1
            txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)
            txtSegno = DirectCast(grdPartite.Rows(i).FindControl("txtSegno"), TextBox)
            chk = DirectCast(grdPartite.Rows(i).FindControl("chkSospesa"), CheckBox)

            If txtRis.Text.Trim <> "" Or chk.Checked Then
                If txtRis.Text.IndexOf("-") = -1 And Not chk.Checked Then
                    Dim Messaggi() As String = {"Risultato della partita " & i + 1 & " non valido<br />Inserirlo nel formato Goal1-Goal2 (es. 2-1)"}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                    Ok = False
                    Exit For
                Else
                    If Not chk.Checked Then
                        Goal1 = Mid(txtRis.Text, 1, txtRis.Text.IndexOf("-")).Trim
                        Goal2 = Mid(txtRis.Text, txtRis.Text.IndexOf("-") + 2, txtRis.Text.Length).Trim

                        If IsNumeric(Goal1) = False Then
                            Dim Messaggi() As String = {"Goal in casa nel risultato della partita " & i + 1 & " non valido"}
                            VisualizzaMessaggioInPopup(Messaggi, Me)
                            Ok = False
                            Exit For
                        End If

                        If IsNumeric(Goal2) = False Then
                            Dim Messaggi() As String = {"Goal fuori casa nel risultato della partita " & i + 1 & " non valido"}
                            VisualizzaMessaggioInPopup(Messaggi, Me)
                            Ok = False
                            Exit For
                        End If
                    End If
                End If
            Else
                Dim Messaggi() As String = {"Risultato della partita " & i + 1 & " non presente"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Ok = False
                Exit For
            End If
        Next

        Return Ok
    End Function

    Private Sub AggiornaSegni()
        For i As Integer = 0 To 13
            Dim row As GridViewRow = grdPartite.Rows(i)
            Dim txtRis As TextBox = DirectCast(row.FindControl("txtRisultato"), TextBox)
            Dim txtSegno As TextBox = DirectCast(row.FindControl("txtSegno"), TextBox)
            Dim Goal1 As String
            Dim Goal2 As String
            Dim Segno As String = ""

            If txtRis.Text <> "" Then
                If txtRis.Text.IndexOf("-") > -1 Then
                    Goal1 = Mid(txtRis.Text, 1, txtRis.Text.IndexOf("-")).Trim
                    Goal2 = Mid(txtRis.Text, txtRis.Text.IndexOf("-") + 2, txtRis.Text.Length).Trim

                    If IsNumeric(Goal1) = True And IsNumeric(Goal2) = True Then
                        If Goal1 > Goal2 Then
                            Segno = "1"
                        End If
                        If Goal1 = Goal2 Then
                            Segno = "X"
                        End If
                        If Goal1 < Goal2 Then
                            Segno = "2"
                        End If
                    End If
                End If
            End If

            txtSegno.Text = Segno

            Dim Riga As Integer = row.RowIndex + 1

            If Riga < 14 Then
                txtRis = DirectCast(grdPartite.Rows(Riga).FindControl("txtRisultato"), TextBox)

                txtRis.Focus()
            End If
        Next
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        AggiornaSegni()

        If EffettuaControlli() = True Then
            Dim txtRis As TextBox
            Dim txtSegno As TextBox
            Dim chk As CheckBox
            Dim Goal1 As Integer
            Dim Goal2 As Integer
            Dim Segno As String = ""
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Sql As String

                Dim gg As Integer
                Dim Altro As String

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    gg = DatiGioco.GiornataSpeciale + 100
                    Altro = "speciale "
                Else
                    gg = DatiGioco.Giornata
                    Altro = ""
                End If

                For i As Integer = 0 To grdPartite.Rows.Count - 1
                    txtRis = DirectCast(grdPartite.Rows(i).FindControl("txtRisultato"), TextBox)
                    txtSegno = DirectCast(grdPartite.Rows(i).FindControl("txtSegno"), TextBox)
                    chk = DirectCast(grdPartite.Rows(i).FindControl("chkSospesa"), CheckBox)

                    If Not chk.Checked Then
                        Goal1 = Val(Mid(txtRis.Text, 1, txtRis.Text.IndexOf("-")).Trim)
                        Goal2 = Val(Mid(txtRis.Text, txtRis.Text.IndexOf("-") + 2, txtRis.Text.Length).Trim)

                        If Goal1 > Goal2 Then
                            Segno = "1"
                        End If
                        If Goal1 = Goal2 Then
                            Segno = "X"
                        End If
                        If Goal1 < Goal2 Then
                            Segno = "2"
                        End If
                    Else
                        txtRis.Text = ""
                        Segno = "S"
                    End If

                    Sql = "Update Schedine Set " &
                        "Risultato='" & txtRis.Text.Trim & "', " &
                        "Segno='" & Segno & "' " &
                        "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And Partita=" & i + 1
                    Db.EsegueSql(ConnSQL, Sql)
                Next

                ConnSQL.Close()

                Dim Messaggi() As String = {"Concorso " & Altro & "aggiornato"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
            End If

            Db = Nothing
        End If
    End Sub

    Protected Sub SpegneTesto(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, CheckBox).NamingContainer, GridViewRow)
        Dim checketto As CheckBox = DirectCast(row.FindControl("chkSospesa"), CheckBox)
        Dim txtRis As TextBox = DirectCast(row.FindControl("txtRisultato"), TextBox)
        Dim txtSegno As TextBox = DirectCast(row.FindControl("txtSegno"), TextBox)

        If checketto.Checked = True Then
            txtRis.Enabled = False
            txtSegno.Enabled = False
            txtRis.Text = ""
            txtSegno.Text = "S"
        Else
            txtRis.Enabled = True
            txtSegno.Enabled = True
            txtRis.Text = ""
            txtSegno.Text = ""
        End If
    End Sub

    Protected Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        AggiornaSegni()
    End Sub

    'Protected Sub AggiornaSegno(sender As Object, e As EventArgs)
    '    Dim row As GridViewRow = DirectCast(DirectCast(sender, TextBox).NamingContainer, GridViewRow)
    '    Dim txtRis As TextBox = DirectCast(row.FindControl("txtRisultato"), TextBox)
    '    Dim txtSegno As TextBox = DirectCast(row.FindControl("txtSegno"), TextBox)
    '    Dim Goal1 As String
    '    Dim Goal2 As String
    '    Dim Segno As String = ""

    '    If txtRis.Text <> "" Then
    '        If txtRis.Text.IndexOf("-") > -1 Then
    '            Goal1 = Mid(txtRis.Text, 1, txtRis.Text.IndexOf("-")).Trim
    '            Goal2 = Mid(txtRis.Text, txtRis.Text.IndexOf("-") + 2, txtRis.Text.Length).Trim

    '            If IsNumeric(Goal1) = True And IsNumeric(Goal2) = True Then
    '                If Goal1 > Goal2 Then
    '                    Segno = "1"
    '                End If
    '                If Goal1 = Goal2 Then
    '                    Segno = "X"
    '                End If
    '                If Goal1 < Goal2 Then
    '                    Segno = "2"
    '                End If
    '            End If
    '        End If
    '    End If

    '    txtSegno.Text = Segno

    '    Dim Riga As Integer = row.RowIndex + 1

    '    If Riga < 14 Then
    '        txtRis = DirectCast(grdPartite.Rows(Riga).FindControl("txtRisultato"), TextBox)

    '        txtRis.Focus()
    '    End If
    'End Sub

End Class