Imports System.IO
Imports System.Net

Public Class NuovoConcorso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Permesso") <> Permessi.Amministratore Then
            Response.Redirect("Principale.aspx")
        End If

        If Page.IsPostBack = False Then
            If Request.QueryString("Modifica") = "True" Then
                CaricaTabella()
                chkSpeciale.Enabled = False
                cmdAggiornaQuote.Visible = True
            Else
                CreaTabella()
                cmdAggiornaQuote.Visible = False
            End If

            divCambiaStemma.Visible = False
            divSceltaSquadra.Visible = False

            CaricaCategorieSquadre()
        End If
    End Sub

    Protected Sub CaricaCategorieSquadre()
        Dim Sql As String = ""
        Dim Db As New GestioneDB
        Dim Rec As Object

        cmbCategoria.Items.Clear()
        cmbSquadra.Items.Clear()
        cmbCategoria.Items.Add("")

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Sql = "Select * From ClassificheSerie Order By idSerie"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                cmbCategoria.Items.Add(Rec("Descrizione").Value)

                Rec.MoveNext
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub CaricaSquadreCategoria()
        If cmbCategoria.Text = "" Then
            Exit Sub
        End If

        Dim Sql As String = ""
        Dim Db As New GestioneDB
        Dim Rec As Object

        cmbSquadra.Items.Clear()
        cmbSquadra.Items.Add("")
        imgScelta.Visible = False

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            Sql = "Select * From ClassificheSerie Where Descrizione='" & cmbCategoria.Text & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim idCategoria As String = Rec("idSerie").Value
            Rec.Close

            Sql = "Select * From Classifiche Where idSerie=" & idCategoria & " Order By Squadra"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                cmbSquadra.Items.Add(Rec("Squadra").Value)

                Rec.MoveNext
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub SceltaSquadreCategoria()
        If cmbSquadra.Text = "" Then
            Exit Sub
        End If

        Dim Path1 As String = "App_Themes/Standard/Images/Stemmi/" & cmbSquadra.Text & "_Tonda.jpg"
        Dim Path2 As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Stemmi\" & cmbSquadra.Text & "_Tonda.jpg"

        If File.Exists(Path2) Then
            imgScelta.ImageUrl = Path1
        Else
            imgScelta.ImageUrl = "App_Themes/Standard/Images/Stemmi/Vuota.jpg"
        End If
        imgScelta.Visible = True
        lblSquadraScelta.Text = cmbSquadra.Text
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If EffettuaControlli() = False Then
            Exit Sub
        Else
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Sql As String
                Dim Casa As TextBox
                Dim Fuori As TextBox
                Dim GiornataReale As Integer

                'If Request.QueryString("Modifica") = "True" Then
                'Else
                If chkSpeciale.Checked = True Then
                    If Request.QueryString("Modifica") <> "True" Then
                        DatiGioco.GiornataSpeciale += 1
                    End If

                    GiornataReale = 100 + DatiGioco.GiornataSpeciale
                Else
                    If Request.QueryString("Modifica") <> "True" Then
                        DatiGioco.Giornata += 1
                    End If

                    GiornataReale = DatiGioco.Giornata
                End If
                'End If

                Sql = "Delete From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & GiornataReale
                Db.EsegueSql(ConnSQL, Sql)

                For i As Integer = 0 To grdPartite.Rows.Count - 1
                    Casa = DirectCast(grdPartite.Rows(i).FindControl("txtCasa"), TextBox)
                    Fuori = DirectCast(grdPartite.Rows(i).FindControl("txtFuori"), TextBox)

                    Sql = "Insert Into Schedine Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & GiornataReale & ", " & _
                        " " & i + 1 & ", " & _
                        "'" & SistemaTestoPerDB(Casa.Text.ToUpper.Trim) & "', " & _
                        "'" & SistemaTestoPerDB(Fuori.Text.ToUpper.Trim) & "', " & _
                        "'', " & _
                        "'' " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)
                Next

                If Request.QueryString("Modifica") = "True" Then
                Else
                    ' Pulizia tabella partita speciale
                    Sql = "Delete From PartiteSpeciali Where idAnno=" & DatiGioco.AnnoAttuale
                    Db.EsegueSql(ConnSQL, Sql)
                    ' Pulizia tabella partita speciale

                    Randomize()
                    Dim x As Integer = Int(Rnd(1) * 14) + 1

                    If x > 14 Then x = 14
                    If x < 1 Then x = 1

                    DatiGioco.PartitaJolly = x
                End If

                If txtChiusura.Text.IndexOf(" ") = -1 Then
                    txtChiusura.Text = txtChiusura.Text.Trim & " " & txtOrario.Text.Trim
                End If

                DatiGioco.ChiusuraConcorso = txtChiusura.Text
                DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto
                AggiornaDatiDiGioco(Db)

                Dim Altro As String = ""

                If chkSpeciale.Checked = True Then
                    Sql = "Delete From PartiteSpeciali Where idAnno=" & DatiGioco.AnnoAttuale & " And idGiornata=" & DatiGioco.GiornataSpeciale
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Insert Into PartiteSpeciali Values (" & DatiGioco.AnnoAttuale & ", " & DatiGioco.GiornataSpeciale & ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Altro = " speciale"
                End If

                If Request.QueryString("Modifica") = "True" Then
                    Dim Messaggi() As String = {"Concorso" & Altro & " " & DatiGioco.GiornataSpeciale & " modificato"}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                Else
                    Dim gMail As New GestioneMail
                    Dim g As Integer

                    If chkSpeciale.Checked = True Then
                        g = DatiGioco.GiornataSpeciale
                    Else
                        g = DatiGioco.Giornata
                    End If

                    gMail.InviaMailAperturaNuovoConcorso(g, txtChiusura.Text, Session("Nick"), chkSpeciale.Checked)

                    gMail = Nothing

                    Dim Messaggi() As String = {"Concorso" & Altro & " " & g & " salvato"}
                    VisualizzaMessaggioInPopup(Messaggi, Me)
                End If

                Response.Redirect("Principale.aspx")
        End If

            Db = Nothing
        End If
    End Sub

    Private Sub CaricaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Db As New GestioneDB
        Dim g As Integer

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            If Speciale = True Then
                g = DatiGioco.GiornataSpeciale + 100
                chkSpeciale.Checked = True
            Else
                g = DatiGioco.Giornata
                chkSpeciale.Checked = False
            End If

            ConnSQL.Close()
        End If

        DB = Nothing

        Sql = "Select A.Partita From Schedine A " &
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " &
            "A.Giornata=" & g & " Order By A.Partita"
        Gr.ImpostaCampi(Sql, grdPartite)
        Gr = Nothing

        If DatiGioco.ChiusuraConcorso.IndexOf(" ") > -1 Then
            txtChiusura.Text = Mid(DatiGioco.ChiusuraConcorso, 1, DatiGioco.ChiusuraConcorso.IndexOf(" "))
            txtOrario.Text = Mid(DatiGioco.ChiusuraConcorso, DatiGioco.ChiusuraConcorso.IndexOf(" ") + 1, 15)
        Else
            txtChiusura.Text = ""
            txtOrario.Text = ""
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Numero As New DataColumn("Partita")
        Dim riga As DataRow
        Dim dttTabella As New DataTable()
        Dim I As Integer

        dttTabella.Columns.Add(Numero)

        For I = 1 To 14
            riga = dttTabella.NewRow()
            riga(0) = I
            dttTabella.Rows.Add(riga)
        Next I

        grdPartite.DataSource = dttTabella
        grdPartite.DataBind()
    End Sub

    Private Function EffettuaControlli() As Boolean
        Dim Ok As Boolean = True
        Dim nErrori As Integer = 0
        Dim Messaggi() As String = {}

        If txtChiusura.Text.Trim = "" Then
            ReDim Preserve Messaggi(nErrori)
            Messaggi(nErrori) = "Inserire la data di chiusura concorso"
            nErrori += 1
            Ok = False
        Else
            If IsDate(txtChiusura.Text) = False Then
                txtChiusura.Text = Mid(txtChiusura.Text, 4, 2) & "/" & Mid(txtChiusura.Text, 1, 2) & "/" & Mid(txtChiusura.Text, 7, 4)
                If IsDate(txtChiusura.Text) = False Then
                    ReDim Preserve Messaggi(nErrori)
                    Messaggi(nErrori) = "Data di chiusura concorso non valida"
                    nErrori += 1
                    Ok = False
                End If
            End If
        End If
        If txtOrario.Text.Trim = "" Then
            ReDim Preserve Messaggi(nErrori)
            Messaggi(nErrori) = "Inserire l'orario di chiusura concorso"
            nErrori += 1
            Ok = False
        Else
            If IsDate("26/02/1972 " & txtOrario.Text) = False Then
                ReDim Preserve Messaggi(nErrori)
                Messaggi(nErrori) = "Orario di chiusura concorso non valida"
                nErrori += 1
                Ok = False
            Else
                Dim d As Date = "26/02/1972 " & txtOrario.Text

                txtOrario.Text = Format(d.Hour, "00") & ":" & Format(d.Minute, "00") & ":" & Format(d.Second, "00")
            End If
        End If

        If Ok = True Then
            Dim Casa As TextBox
            Dim Fuori As TextBox
            Dim lblSerieC As Label
            Dim lblSerieF As Label

            For i As Integer = 0 To grdPartite.Rows.Count - 1
                Casa = DirectCast(grdPartite.Rows(i).FindControl("txtCasa"), TextBox)
                Fuori = DirectCast(grdPartite.Rows(i).FindControl("txtFuori"), TextBox)
                lblSerieC = DirectCast(grdPartite.Rows(i).FindControl("lblSerieC"), Label)
                lblSerieF = DirectCast(grdPartite.Rows(i).FindControl("lblSerieF"), Label)

                If Casa.Text.Trim = "" Or Casa.Text.Length < 4 Then
                    ReDim Preserve Messaggi(nErrori)
                    Messaggi(nErrori) = "Squadra in casa della partita " & i + 1 & " non inserita"
                    nErrori += 1
                    Ok = False
                Else
                    Dim Errori() As String = ControllaSquadra(Casa, i, lblSerieC, "in casa")
                    If Errori.Length > 0 Then
                        For ii As Integer = 0 To Errori.Length - 1
                            ReDim Preserve Messaggi(nErrori)
                            Messaggi(nErrori) = Errori(ii)
                            nErrori += 1
                        Next

                        Ok = False
                    End If
                End If

                If Ok = True Then
                    If Fuori.Text.Trim = "" Or Fuori.Text.Length < 4 Then
                        ReDim Preserve Messaggi(nErrori)
                        Messaggi(nErrori) = "Squadra fuori casa della partita " & i + 1 & " non inserita"
                        nErrori += 1
                        Ok = False
                    Else
                        Dim Errori() As String = ControllaSquadra(Fuori, i, lblSerieF, "fuori casa")
                        If Errori.Length > 0 Then
                            For ii As Integer = 0 To Errori.Length - 1
                                ReDim Preserve Messaggi(nErrori)
                                Messaggi(nErrori) = Errori(ii)
                                nErrori += 1
                            Next

                            Ok = False
                        End If
                    End If
                End If

                If Ok = False Then
                    Exit For
                Else
                    PrendeQuote(Casa.Text, Fuori.Text, i + 1)
                End If
            Next
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()

            SistemaQuoteA0(Db, ConnSQL)

            ConnSQL.Close()
            ' Controlla se ci sono quote a 0 ed eventualmente le imposta
        End If

        Db = Nothing

        If Ok = False Then
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Return Ok
    End Function

    Private Sub PrendeQuote(Casa As String, Fuori As String, Partita As Integer)
        Dim Db As New GestioneDB
        Dim ValoreQuotePerMancanzaDati As Single = 0.87
        Dim ValoreQuotePerMancanzaDati2 As Single = ValoreQuotePerMancanzaDati * 3
        Dim ValoreFittizio As Boolean = False

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim g As Integer

            If chkSpeciale.Checked = True Then
                g = DatiGioco.GiornataSpeciale + 100
            Else
                g = DatiGioco.Giornata
            End If

            If Request.QueryString("Modifica") = "True" Then
            Else
                g += 1
            End If

            Sql = "Delete From Quote " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And Partita=" & Partita
            Db.EsegueSql(ConnSQL, Sql)

            Dim mgf1 As Single = ValoreQuotePerMancanzaDati
            Dim mgs1 As Single = ValoreQuotePerMancanzaDati

            Sql = "Select " &
                "(gFatti)/(Cast(Giocate As Numeric(4,2))) As MediaGoalFatti, " &
                "(gSubiti)/(Cast(Giocate As Numeric(4,2))) As MediaGoalSubiti " &
                "From Classifiche " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(Casa.Trim.ToUpper) & "' And Giocate>0"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                mgf1 = Rec("MediaGoalFatti").Value
                mgs1 = Rec("MediaGoalSubiti").Value
            Else
                mgf1 = ValoreQuotePerMancanzaDati
                mgs1 = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End If
            Rec.Close()

            Dim mgf2 As Single = ValoreQuotePerMancanzaDati * 0.5
            Dim mgs2 As Single = ValoreQuotePerMancanzaDati * 0.5

            Sql = "Select " &
                "(gFatti)/(Cast(Giocate As Numeric(4,2))) As MediaGoalFatti, " &
                "(gSubiti)/(Cast(Giocate As Numeric(4,2))) As MediaGoalSubiti " &
                "From Classifiche " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(Fuori.Trim.ToUpper) & "' And Giocate>0"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                mgf2 = Rec("MediaGoalFatti").Value
                mgs2 = Rec("MediaGoalSubiti").Value
            Else
                mgf2 = ValoreQuotePerMancanzaDati * 0.5
                mgs2 = ValoreQuotePerMancanzaDati * 0.5
                ValoreFittizio = True
            End If
            Rec.Close()

            Dim vCasa As Integer = ValoreQuotePerMancanzaDati
            Dim nCasa As Integer = ValoreQuotePerMancanzaDati
            Dim pCasa As Integer = ValoreQuotePerMancanzaDati
            Dim gCasa As Integer = ValoreQuotePerMancanzaDati2

            Sql = "Select Giocate, vCasa, nCasa, pCasa " &
                "From Classifiche " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(Casa.Trim.ToUpper) & "' And Giocate>0"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                gCasa = Rec("Giocate").Value / 2
                vCasa = Rec("vCasa").Value
                nCasa = Rec("nCasa").Value
                pCasa = Rec("pCasa").Value
            Else
                gCasa = ValoreQuotePerMancanzaDati2
                vCasa = ValoreQuotePerMancanzaDati
                nCasa = ValoreQuotePerMancanzaDati
                pCasa = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End If
            Rec.Close()

            Dim vFuori As Integer = ValoreQuotePerMancanzaDati
            Dim nFuori As Integer = ValoreQuotePerMancanzaDati
            Dim pFuori As Integer = ValoreQuotePerMancanzaDati
            Dim gFuori As Integer = ValoreQuotePerMancanzaDati2

            Sql = "Select Giocate, vFuori, nFuori, pFuori " &
                "From Classifiche " & _
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(Fuori.Trim.ToUpper) & "' And Giocate>0"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                gFuori = Rec("Giocate").Value / 2
                vFuori = Rec("vFuori").Value
                nFuori = Rec("nFuori").Value
                pFuori = Rec("pFuori").Value
            Else
                gFuori = ValoreQuotePerMancanzaDati2
                vFuori = ValoreQuotePerMancanzaDati
                nFuori = ValoreQuotePerMancanzaDati
                pFuori = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End If
            Rec.Close()

            Dim q1 As Integer = ValoreQuotePerMancanzaDati
            Dim qX As Integer = ValoreQuotePerMancanzaDati * 0.25
            Dim q2 As Integer = ValoreQuotePerMancanzaDati * 0.5

            Try
                q1 = (((vCasa + pFuori) / (gCasa + gFuori)) * 100)
            Catch ex As Exception
                q1 = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End Try

            Try
                qX = (((nCasa + nFuori) / (gCasa + gFuori)) * 100)
            Catch ex As Exception
                qX = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End Try

            Try
                q2 = (((vFuori + pCasa) / (gCasa + gFuori)) * 100)
            Catch ex As Exception
                q2 = ValoreQuotePerMancanzaDati
                ValoreFittizio = True
            End Try

            Dim somma As Integer = q1 + qX + q2
            Dim qq1 As Single = ValoreQuotePerMancanzaDati
            Dim qqX As Single = ValoreQuotePerMancanzaDati * 0.25
            Dim qq2 As Single = ValoreQuotePerMancanzaDati * 0.5

            If somma > 0 Then
                Try
                    qq1 = q1 / somma
                    qq1 *= 100
                Catch ex As Exception
                    qq1 = ValoreQuotePerMancanzaDati
                    ValoreFittizio = True
                End Try

                Try
                    qqX = qX / somma
                    qqX *= 100
                Catch ex As Exception
                    qqX = ValoreQuotePerMancanzaDati
                    ValoreFittizio = True
                End Try

                Try
                    qq2 = q2 / somma
                    qq2 *= 100
                Catch ex As Exception
                    qq2 = ValoreQuotePerMancanzaDati
                    ValoreFittizio = True
                End Try
            End If

            q1 = qq1
            qX = qqX
            q2 = qq2

            Dim quota As Single = ValoreQuotePerMancanzaDati
            Dim sommagoal As Single = ValoreQuotePerMancanzaDati
            Dim max As Single = ValoreQuotePerMancanzaDati

            Sql = ""

            For i As Integer = 0 To 4
                For k As Integer = 0 To 4
                    quota = 0
                    sommagoal = 0
                    Try
                        sommagoal = 1 + (((i + 0.3) + (k + 0.3)) / 3)
                        quota = (((mgs2 / mgf1) * (i + 0.3)) + (((mgs1 / mgf2) * (k + 0.3)))) * sommagoal

                        If i > k Then
                            quota *= (100 / q1)
                        Else
                            If i < k Then
                                quota *= (100 / q2)
                            Else
                                quota *= (100 / qX)
                            End If
                        End If
                    Catch ex As Exception
                        quota = ValoreQuotePerMancanzaDati
                    End Try
                    If ValoreFittizio = True Then
                        quota /= 4.27
                    End If
                    If quota > max Then
                        max = quota
                    End If
                    Sql += quota.ToString.Replace(",", ".") & ", "
                Next
            Next
            ' Altro

            quota = max * 1.27
            If Val(quota.ToString) < 2 Then
                quota = 227
            End If
            Sql += quota.ToString.Replace(",", ".") & ", "

            Dim q11 As Single = ValoreQuotePerMancanzaDati
            Dim qxx As Single = ValoreQuotePerMancanzaDati
            Dim q22 As Single = ValoreQuotePerMancanzaDati

            Try
                q11 = 100 / q1
                If ValoreFittizio = True Then
                    q11 /= 2.27
                End If
            Catch ex As Exception
                q11 = ValoreQuotePerMancanzaDati
                q11 /= 2.27
            End Try

            Try
                qxx = 100 / qX
                If ValoreFittizio = True Then
                    qxx /= 2.27
                End If
            Catch ex As Exception
                qxx = ValoreQuotePerMancanzaDati
                qxx /= 4.27
            End Try

            Try
                q22 = 100 / q2
                If ValoreFittizio = True Then
                    q22 /= 2.27
                End If
            Catch ex As Exception
                q22 = ValoreQuotePerMancanzaDati
                q22 /= 2.27
            End Try

            Sql += q11.ToString.Replace(",", ".") & ", "
            Sql += qxx.ToString.Replace(",", ".") & ", "
            Sql += q22.ToString.Replace(",", ".") & ", "

            Dim q1x As Single = ValoreQuotePerMancanzaDati
            Dim q12 As Single = ValoreQuotePerMancanzaDati
            Dim qx2 As Single = ValoreQuotePerMancanzaDati

            Try
                q1x = 1 / ((1 / q11) + (1 / qxx))
                If ValoreFittizio = True Then
                    q1x /= 2.27
                End If
            Catch ex As Exception
                q1x = ValoreQuotePerMancanzaDati
                q1x /= 2.27
            End Try

            Try
                q12 = 1 / ((1 / q11) + (1 / q22))
                If ValoreFittizio = True Then
                    q12 /= 2.27
                End If
            Catch ex As Exception
                q12 = ValoreQuotePerMancanzaDati
                q12 /= 2.27
            End Try

            Try
                qx2 = 1 / ((1 / qxx) + (1 / q22))
                If ValoreFittizio = True Then
                    qx2 /= 2.27
                End If
            Catch ex As Exception
                qx2 = ValoreQuotePerMancanzaDati
                qx2 /= 2.27
            End Try

            Sql += q1x.ToString.Replace(",", ".") & ", "
            Sql += q12.ToString.Replace(",", ".") & ", "
            Sql += qx2.ToString.Replace(",", ".") & ", "

            Sql = Mid(Sql, 1, Sql.Length - 2)

            Sql = Sql.ToUpper.Replace("+INFINITO", "0").Replace("-INFINITO", "0").Replace("NON UN NUMERO REALE", "0").Replace("NON UN NUMERO", "0")

            Sql = "Insert Into Quote Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & g & ", " &
                " " & Partita & ", " &
                " " & Sql & " " &
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Function ControllaSquadra(Squadra As TextBox, i As Integer, Categoria As Label, Dove As String) As String()
        Dim Db As New GestioneDB
        Dim Errori() As String = {}
        Dim nErrori As Integer = 0

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.Squadra, B.Descrizione From Classifiche A " &
                "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(Squadra.Text.ToUpper.Trim) & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Rec.Close()

                Sql = "Select A.Squadra, B.Descrizione From Classifiche A " &
                    "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                    "Where Upper(Ltrim(Rtrim(Squadra))) Like '%" & SistemaTestoPerDB(Squadra.Text.ToUpper.Trim) & "%'"
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec.Eof = False Then
                    Dim q As Integer = 0

                    Do Until Rec.Eof
                        q += 1

                        Rec.MoveNext()
                    Loop

                    If q = 1 Then
                        Rec.MoveFirst()

                        If chkSkipNomiSquadre.Checked = False Then
                            Squadra.Text = Rec("Squadra").Value.ToString.ToUpper.Trim
                            Categoria.Text = Rec("Descrizione").Value.ToString.ToUpper.Trim
                        End If

                        Rec.Close()
                    Else
                        Rec.Close()

                        Dim Trovate As String = ""

                        For k As Integer = Squadra.Text.Length To 4 Step -1
                            Sql = "Select A.Squadra, B.Descrizione From Classifiche A " &
                                "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                                "Where Upper(Ltrim(Rtrim(Squadra))) Like '%" & SistemaTestoPerDB(Mid(Squadra.Text.ToUpper.Trim, 1, k)) & "%'"
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                Do Until Rec.Eof
                                    Trovate += Rec("Squadra").Value & " (" & Rec("Descrizione").Value & ")<br />"

                                    Rec.MoveNext()
                                Loop
                                Exit For
                            End If
                            Rec.Close()
                        Next

                        For k As Integer = 1 To Squadra.Text.Length - 4
                            Sql = "Select A.Squadra, B.Descrizione From Classifiche A " &
                                "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                                "Where Upper(Ltrim(Rtrim(Squadra))) Like '%" & SistemaTestoPerDB(Mid(Squadra.Text.ToUpper.Trim, k, Squadra.Text.Length)) & "%'"
                            Rec = Db.LeggeQuery(ConnSQL, Sql)
                            If Rec.Eof = False Then
                                Do Until Rec.Eof
                                    Trovate += Rec("Squadra").Value & " (" & Rec("Descrizione").Value & ")<br />"

                                    Rec.MoveNext()
                                Loop
                                Exit For
                            End If
                            Rec.Close()
                        Next

                        If chkSkipNomiSquadre.Checked = False Then
                            If Trovate <> "" Then
                                Trovate = Mid(Trovate, 1, Trovate.Length - 6)

                                ReDim Preserve Errori(nErrori)
                                Errori(nErrori) = "Rilevate più squadre per la partita " & i + 1 & " " & Dove & ":<hr />" & Trovate & "<hr />"
                                nErrori += 1
                            Else
                                ReDim Preserve Errori(nErrori)
                                Errori(nErrori) = "Squadra " & Dove & " della partita " & i + 1 & " non rilevata sul db delle classifiche"
                                nErrori += 1
                            End If
                        End If
                    End If
                Else
                    If chkSkipNomiSquadre.Checked = False Then
                        ReDim Preserve Errori(nErrori)
                        Errori(nErrori) = "Squadra " & Dove & " della partita " & i + 1 & " non rilevata sul db delle classifiche"
                        nErrori += 1
                    End If
                End If
            Else
                If chkSkipNomiSquadre.Checked = False Then
                    Squadra.Text = Rec("Squadra").Value.ToString.ToUpper.Trim
                    Categoria.Text = Rec("Descrizione").Value.ToString.ToUpper.Trim
                End If
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Errori
    End Function

    Private Sub grdPartite_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ' e.Row.Cells(0).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TxtCasa As TextBox = DirectCast(e.Row.FindControl("TxtCasa"), TextBox)
            Dim TxtFuori As TextBox = DirectCast(e.Row.FindControl("TxtFuori"), TextBox)
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim ImgJolly As Image = DirectCast(e.Row.FindControl("imgJolly"), Image)

            If Request.QueryString("Modifica") = "True" Then
                Dim r As Integer
                If IsNumeric(e.Row.Cells(0).Text) Then
                    r = Val(e.Row.Cells(0).Text)
                Else
                    r = -1
                End If
                Dim numRiga As Integer = r
                Dim lblSerieC As Label = DirectCast(e.Row.FindControl("lblSerieC"), Label)
                Dim lblSerieF As Label = DirectCast(e.Row.FindControl("lblSerieF"), Label)
                Dim Db As New GestioneDB
                Dim gm As New GestioneMail

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

                    Sql = "Select B.Descrizione From Classifiche A " &
                        "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                        "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtCasa.Text.ToUpper.Trim) & "'"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        lblSerieC.Text = Rec("Descrizione").Value
                    Else
                        lblSerieC.Text = ""
                    End If
                    Rec.Close()

                    Sql = "Select B.Descrizione From Classifiche A " &
                        "Left Join ClassificheSerie B On A.idSerie=B.idSerie " &
                        "Where Upper(Ltrim(Rtrim(Squadra)))='" & SistemaTestoPerDB(TxtFuori.Text.ToUpper.Trim) & "'"
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec.Eof = False Then
                        lblSerieF.Text = Rec("Descrizione").Value
                    Else
                        lblSerieF.Text = ""
                    End If
                    Rec.Close()

                    Dim Path As String

                    Path = gm.RitornaImmagineSquadra(TxtCasa.Text, True)
                    If File.Exists(Server.MapPath(Path)) = True Then
                        ImgCasa.ImageUrl = Path
                        ImgCasa.Visible = True
                    Else
                        ImgCasa.ImageUrl = ""
                        ImgCasa.Visible = False
                    End If

                    Path = gm.RitornaImmagineSquadra(TxtFuori.Text, True)
                    If File.Exists(Server.MapPath(Path)) = True Then
                        ImgFuori.ImageUrl = Path
                        ImgFuori.Visible = True
                    Else
                        ImgFuori.ImageUrl = ""
                        ImgFuori.Visible = False
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
            Else
                TxtCasa.Text = ""
                TxtFuori.Text = ""
                ImgCasa.Visible = False
                ImgFuori.Visible = False
                ImgJolly.ImageUrl = ""
                ImgJolly.Visible = False
            End If

            ' e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub VisuaImmagineCasa(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, TextBox).NamingContainer, GridViewRow)
        Dim TxtFuori As TextBox = DirectCast(DirectCast(sender, TextBox).NamingContainer.FindControl("TxtFuori"), TextBox)

        TxtFuori.Focus()
    End Sub

    Protected Sub VisuaImmagineFuori(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, TextBox).NamingContainer, GridViewRow)

        Dim Riga As Integer = row.RowIndex + 1
        If Riga < 14 Then
            Dim TxtCasa As TextBox = DirectCast(grdPartite.Rows(Riga).FindControl("TxtCasa"), TextBox)

            TxtCasa.Focus()
        Else
            txtChiusura.Focus()
        End If
    End Sub

    Protected Sub CambiaImmagineSquadraCasa(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Squadra As TextBox = DirectCast(row.FindControl("txtCasa"), TextBox)
        Dim Immagine As ImageButton = DirectCast(row.FindControl("imgCasa"), ImageButton)
        Dim Path As String = "App_Themes/Standard/Images/Stemmi/" & Squadra.Text & "_Tonda.jpg"

        If File.Exists(Server.MapPath(Path)) = False Then
            Path = "App_Themes/Standard/Images/Stemmi/Niente.png"
        End If

        hdnSquadra.Value = Squadra.Text
        hdnDove.Value = "imgCasa"
        AppoRow = row

        imgStemma.ImageUrl = Path
        divCambiaStemma.Visible = True
        Squadra.Focus()
    End Sub

    Protected Sub CambiaImmagineSquadraFuori(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Squadra As TextBox = DirectCast(row.FindControl("txtFuori"), TextBox)
        Dim Immagine As ImageButton = DirectCast(row.FindControl("imgFuori"), ImageButton)
        Dim Path As String = "App_Themes/Standard/Images/Stemmi/" & Squadra.Text & "_Tonda.jpg"

        If File.Exists(Server.MapPath(Path)) = False Then
            Path = "App_Themes/Standard/Images/Stemmi/Niente.png"
        End If

        hdnSquadra.Value = Squadra.Text
        hdnDove.Value = "imgFuori"
        AppoRow = row

        imgStemma.ImageUrl = Path
        divCambiaStemma.Visible = True
        Squadra.Focus()
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) ' Handles cmdOK.Click
        divCambiaStemma.Visible = False

        If FileUpload2.HasFile = False Then
            Dim Messaggi() As String = {"Selezionare una immagine"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Path As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Stemmi\" & hdnSquadra.Value & ".jpg"
        Dim Path2 As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Stemmi\" & hdnSquadra.Value & "_Tonda.jpg"
        Dim Path3 As String = "App_Themes/Standard/Images/Stemmi/" & hdnSquadra.Value & "_Tonda.jpg"

        FileUpload2.SaveAs(Path)

        Dim gi As New GestioneImmagini
        gi.Ridimensiona(Path, Path, 100, 100)
        gi.RidimensionaEArrotondaIcona(Path)
        gi = Nothing

        Try
            Kill(Path2)
        Catch ex As Exception

        End Try

        Rename(Path, Path2)

        Dim Immagine As ImageButton = DirectCast(AppoRow.FindControl(hdnDove.Value), ImageButton)

        Immagine.ImageUrl = Path3

        AppoRow = Nothing
        hdnSquadra.Value = ""
        hdnDove.Value = ""

        If Request.QueryString("Modifica") = "True" Then
            CaricaTabella()
        End If
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) ' Handles cmdAnnulla.Click
        divCambiaStemma.Visible = False
    End Sub

    Protected Sub PrendeClassifiche()
        Dim NomeFileClassifica As String = Server.MapPath(".") & "\Appoggio\Classifica.pdf"
        Dim NomeFileQuote As String = Server.MapPath(".") & "\Appoggio\Quote.pdf"

        Try
            My.Computer.FileSystem.CreateDirectory(Server.MapPath(".") & "\Appoggio")
        Catch ex As Exception

        End Try

        Try
            Dim myWebClient As New WebClient()

            myWebClient.DownloadFile("http://landing.sisal.it/volantini/Scommesse_Sport/Classifiche/Classifiche%202%20colonne%20CALCIO.pdf", NomeFileClassifica)
        Catch ex As Exception

        End Try

        'Try
        '    Dim myWebClient As New WebClient()

        '    myWebClient.DownloadFile("http://landing.sisal.it/volantini/Scommesse_Sport/Quote/calcio%20ris.esatto%20per%20manifestazione.pdf", NomeFileQuote)
        'Catch ex As Exception

        'End Try

        SpippaClassifica(NomeFileClassifica)
        'SpippaQuote(NomeFileQuote)

        Dim Messaggi() As String = {"Classifica e quote caricate"}
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub

    Private Sub SpippaClassifica(NomeFileClassifica As String)
        Dim gp As New GestionePDF
        gp.ConvertePDF(NomeFileClassifica, Server.MapPath(".") & "\Appoggio\Butta.txt")
        gp = Nothing

        Dim gf As New GestioneFilesDirectory
        Dim Filetto As String = gf.LeggeFileIntero(Server.MapPath(".") & "\Appoggio\Butta.txt")
        Dim RigheFile() As String = Filetto.Split("§§§")
        Dim PrendeNomeSerie As Boolean = False
        Dim NomeSerie As String = ""
        Dim Girone As String = ""
        Dim Ritorno As String = ""
        RigheFile(0) = "***"
        For i As Long = 1 To RigheFile.Length - 1
            If RigheFile(i).Trim = "" Or RigheFile(i).Length < 4 Then
                RigheFile(i) = "***"
            Else
                If PrendeNomeSerie = True Then
                    PrendeNomeSerie = False
                    RigheFile(i) = "SERIE: " & RigheFile(i)
                    Girone = ""
                End If
                If RigheFile(i) = "RETI TRASFERTA CASA" Or _
                    RigheFile(i) = "CALCIO" Or _
                    RigheFile(i) = "CALCIO COMPLETO" Then

                    RigheFile(i) = "***"
                Else
                    If RigheFile(i).IndexOf("Girone") > -1 Then
                        Girone = "GIRONE: " & RigheFile(i)
                    Else
                        If RigheFile(i) = "S F S P V S P V G P" Then
                            RigheFile(i) = "***"
                            PrendeNomeSerie = True
                        Else
                            If RigheFile(i).IndexOf("giornate") > -1 Then
                                Dim ancora As Integer = 0

                                For k As Integer = RigheFile(i).Length - 1 To 1 Step -1
                                    If Mid(RigheFile(i), k, 1) = " " Then
                                        ancora += 1
                                        If ancora = 2 Then
                                            RigheFile(i) = Mid(RigheFile(i), k + 1, RigheFile(i).Length)
                                            Exit For
                                        End If
                                    End If
                                Next
                            Else
                                If RigheFile(i).IndexOf("Dati aggiornati al ") > -1 Then
                                    RigheFile(i) = "***"
                                Else
                                    If RigheFile(i).IndexOf("CAMP. ARGENTINO") > -1 Then
                                        For k As Long = i To RigheFile.Length - 1
                                            RigheFile(k) = "***"
                                        Next
                                        Exit For
                                    Else
                                        If RigheFile(i).IndexOf("(") > -1 Then
                                            RigheFile(i) = Mid(RigheFile(i), 1, RigheFile(i).IndexOf("("))
                                        Else
                                            RigheFile(i) = RigheFile(i).Replace("POff", "")
                                            RigheFile(i) = RigheFile(i).Replace("PrChL", "")
                                            RigheFile(i) = RigheFile(i).Replace("ChL", "")
                                            RigheFile(i) = RigheFile(i).Replace("POut", "")
                                            RigheFile(i) = RigheFile(i).Replace("UEFA", "")
                                            RigheFile(i) = RigheFile(i).Replace("Retr", "")
                                            RigheFile(i) = RigheFile(i).Replace("Prom", "")
                                            RigheFile(i) = RigheFile(i).Replace("AFCCp", "")
                                            RigheFile(i) = RigheFile(i).Replace("AFCC", "")
                                            RigheFile(i) = RigheFile(i).Replace("POUE", "")

                                            If RigheFile(i).Trim = "" Or RigheFile(i).Length < 4 Then
                                                RigheFile(i) = "***"
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next

        Dim DB As New GestioneDB

        If DB.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = DB.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = ""
            Dim Serie As String = ""
            Dim idSerie As Integer
            Dim Progressivo As Integer = 0

            Sql = "Delete From Classifiche"
            DB.EsegueSql(ConnSQL, Sql)

            Sql = "Delete From ClassificheSerie"
            DB.EsegueSql(ConnSQL, Sql)

            For i As Long = 0 To RigheFile.Length - 1
                If RigheFile(i) <> "***" Then
                    If Mid(RigheFile(i), 1, 1) <> " " And Mid(RigheFile(i).ToUpper.Trim, 1, 4) <> "DAL " Then
                        If RigheFile(i).IndexOf("=") = -1 Then
                            Ritorno = RigheFile(i)

                            If Ritorno.IndexOf("SERIE:") > -1 Then
                                Ritorno = Mid(Ritorno, 7, Ritorno.Length).Trim
                                Serie = Ritorno
                                Progressivo = 0

                                idSerie = RitornaSerie(Ritorno, DB, ConnSQL)
                            Else
                                If Ritorno.ToUpper.IndexOf("GIRONE") > -1 Then
                                    Ritorno = Serie & " - " & Ritorno
                                    Progressivo = 0

                                    idSerie = RitornaSerie(Ritorno, DB, ConnSQL)
                                Else
                                    If Ritorno.IndexOf("giornate") > -1 Then
                                    Else
                                        Dim TrovataLettera As Boolean = False

                                        Ritorno = Ritorno.Replace("1.", "")
                                        Ritorno = Ritorno.Replace("HANNOVER 96 II", "HANNOVER II 96")
                                        Ritorno = Ritorno.Replace("1899 HOFFENHEIM", "HOFFENHEIM 1899")
                                        Ritorno = Ritorno.Replace("1899 HOFFENHEIM II", "HOFFENHEIM II 1899")
                                        Ritorno = Ritorno.Replace("1461 TRABZON", "TRABZON 1461")
                                        Ritorno = Ritorno.Replace("1860 MONACO", "MONACO 1860")
                                        Ritorno = Ritorno.Replace("1896 RAIN AM LECH", "RAIN AM LECH 1896")

                                        For k As Integer = Ritorno.Length - 1 To 1 Step -1
                                            If IsNumeric(Mid(Ritorno, k, 1)) = True And TrovataLettera = True Then
                                                Dim Ritorno2 As String = Mid(Ritorno, k + 2, Ritorno.Length).Replace(" ", "_")

                                                Ritorno = Mid(Ritorno, 1, k) & " " & Ritorno2
                                                Exit For
                                            Else
                                                If IsNumeric(Mid(Ritorno, k, 1)) = False Then
                                                    TrovataLettera = True
                                                End If
                                            End If
                                        Next

                                        Dim Campi() As String = Ritorno.Split(" ")

                                        Progressivo += 1
                                        Sql = idSerie & ", " & Progressivo & ", "
                                        For k As Integer = Campi.Length - 1 To 0 Step -1
                                            If Campi(k).Trim <> "" Then
                                                If IsNumeric(Campi(k)) = True Then
                                                    Sql += Campi(k) & ", "
                                                Else
                                                    Sql += "'" & SistemaTestoPerDB(Campi(k).Replace("_", " ")).ToUpper.Trim & "', "
                                                End If
                                            End If
                                        Next
                                        Sql = Mid(Sql, 1, Sql.Length - 2)
                                        Sql = "Insert Into Classifiche Values (" & Sql & ")"
                                        Try
                                            DB.EsegueSqlSenzaTRY(ConnSQL, Sql)
                                        Catch ex As Exception

                                        End Try
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        DB = Nothing

        gf = Nothing
    End Sub

    Private Function RitornaSerie(Ritorno As String, Db As GestioneDB, ConnSql As Object) As Integer
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim idSerie As Integer

        Sql = "Select * From ClassificheSerie Where Descrizione='" & SistemaTestoPerDB(Ritorno.ToUpper.Trim) & "'"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            idSerie = Rec(0).Value
        Else
            idSerie = -1
        End If
        Rec.Close()

        If idSerie = -1 Then
            Sql = "Select Max(idSerie) From ClassificheSerie"
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                idSerie = 1
            Else
                idSerie = Rec(0).Value + 1
            End If
            Rec.Close()

            Sql = "Insert Into ClassificheSerie Values (" & idSerie & ", '" & SistemaTestoPerDB(Ritorno.ToUpper.Trim) & "')"
            Db.EsegueSql(ConnSql, Sql)
        End If

        Return idSerie
    End Function

    Protected Sub cmdCaricaClassifiche_Click(sender As Object, e As EventArgs) Handles cmdCaricaClassifiche.Click
        PrendeClassifiche()
    End Sub

    Protected Sub cmdAggiornaQuote_Click(sender As Object, e As EventArgs) Handles cmdAggiornaQuote.Click
        Dim Db As New GestioneDB

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

            Sql = "Select SquadraCasa, SquadraFuori, Partita From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                PrendeQuote(Rec("SquadraCasa").Value, Rec("SquadraFuori").Value, Rec("Partita").value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            SistemaQuoteA0(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing

        Dim Messaggi() As String = {"Quote caricate"}

        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub

    Private Sub SistemaQuoteA0(DB As GestioneDB, ConnSQL As Object)
        Dim Sql As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Rec2 As Object = CreateObject("ADODB.Recordset")

        ' Controlla se ci sono quote a 0 ed eventualmente le imposta
        'Dim AZero() As Integer = {}
        'Dim qAZero As Integer = 0

        Dim g As Integer

        If chkSpeciale.Checked = True Then
            g = DatiGioco.GiornataSpeciale + 100
        Else
            g = DatiGioco.Giornata
        End If

        If Request.QueryString("Modifica") = "True" Then
        Else
            g += 1
        End If

        Dim NomeCampi() As String = {"r0_0", "r0_1", "r0_2", "r0_3", "r0_4", "r1_0", "r1_1", "r1_2", "r1_3", "r1_4", "r2_0", "r2_1", "r2_2", "r2_3", "r2_4", "r3_0", "r3_1", "r3_2", "r3_3", "r3_4", "r4_0", "r4_1", "r4_2", "r4_3", "r4_4", "rAltro", "r1", "rX", "r2", "r1X", "r12", "rX2"}

        Sql = "Select * From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g
        Rec = DB.LeggeQuery(ConnSQL, Sql)
        Do Until Rec.Eof
            For i As Integer = 0 To NomeCampi.Length - 1
                If Rec(NomeCampi(i)).Value = 0 Then
                    Sql = "Select Min(" & NomeCampi(i) & ") From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And " & NomeCampi(i) & ">0"
                    Rec2 = DB.LeggeQuery(ConnSQL, Sql)
                    If Rec2(0).Value Is DBNull.Value = False Then
                        Dim v As Single = Val(Rec2(0).Value.ToString)

                        If v < 1 Then
                            v = 0.7
                        End If
                        Sql = "Update Quote Set " & NomeCampi(i) & "=" & v.ToString.Replace(",", ".") & " Where  Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And Partita=" & Rec("Partita").Value
                        DB.EsegueSql(ConnSQL, Sql)
                    End If
                    Rec2.Close()
                End If
            Next
            'ReDim Preserve AZero(qAZero)
            'AZero(qAZero) = Rec("Partita").Value
            'qAZero += 1

            Rec.MoveNext()
        Loop
        Rec.Close()

        'If qAZero > 0 Then
        '    Dim PartitaConQuote As Integer = -1

        '    Sql = "Select * From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And r0_0<>0 And r3_0<>0"
        '    Rec = DB.LeggeQuery(ConnSQL, Sql)
        '    If Rec.Eof = False Then
        '        PartitaConQuote = Rec("Partita").Value
        '    End If
        '    Rec.Close()

        '    If PartitaConQuote > 0 Then
        '        For i As Integer = 0 To qAZero - 1
        '            Sql = "Delete From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And Partita=" & AZero(i)
        '            DB.EsegueSql(ConnSQL, Sql)

        '            Sql = "Insert Into Quote " &
        '                "SELECT [Anno],[Giornata]," & AZero(i) & ",[r0_0],[r0_1],[r0_2],[r0_3],[r0_4],[r1_0],[r1_1],[r1_2],[r1_3],[r1_4],[r2_0] " &
        '                "      ,[r2_1],[r2_2],[r2_3],[r2_4],[r3_0],[r3_1],[r3_2],[r3_3],[r3_4],[r4_0],[r4_1],[r4_2],[r4_3],[r4_4] " &
        '                "      ,[rAltro],[r1],[rX],[r2],[r1X],[r12],[rX2] " &
        '                "From Quote Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & g & " And Partita=" & PartitaConQuote
        '            DB.EsegueSql(ConnSQL, Sql)
        '        Next
        '    End If
        'End If
        ' Controlla se ci sono quote a 0 ed eventualmente le imposta
    End Sub

    Protected Sub SceltaSquadraCasa(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnNumRiga.Value = -row.RowIndex

        cmbCategoria.Text = ""
        cmbSquadra.Text = ""
        lblSquadraScelta.Text = ""
        imgScelta.Visible = False

        divSceltaSquadra.Visible = True
    End Sub

    Protected Sub SceltaSquadraFuori(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnNumRiga.Value = row.RowIndex

        cmbCategoria.Text = ""
        cmbSquadra.Text = ""
        lblSquadraScelta.Text = ""
        imgScelta.Visible = False

        divSceltaSquadra.Visible = True
    End Sub

    Protected Sub cmdAnnullaSS_Click(sender As Object, e As EventArgs) Handles cmdAnnullaSS.Click
        divSceltaSquadra.Visible = False
    End Sub

    Protected Sub cmdOkSS_Click(sender As Object, e As EventArgs) Handles cmdOkSS.Click
        Dim row As GridViewRow = grdPartite.Rows(Math.Abs(Val(hdnNumRiga.Value)))
        Dim TxtSceltaSquadra As TextBox

        If Val(hdnNumRiga.Value) < 0 Then
            TxtSceltaSquadra = DirectCast(row.FindControl("TxtCasa"), TextBox)
        Else
            TxtSceltaSquadra = DirectCast(row.FindControl("TxtFuori"), TextBox)
        End If

        TxtSceltaSquadra.Text = lblSquadraScelta.Text

        divSceltaSquadra.Visible = False
    End Sub
End Class