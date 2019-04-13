Public Class ChiudeConcorso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaMancanti()
        End If
    End Sub

    Private Sub CaricaMancanti()
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

        Sql = "Select Giocatore From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore Not In (" & _
                "Select Distinct(CodGiocatore) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & ") And Cancellato='N' Order By Giocatore"
        Gr.ImpostaCampi(Sql, grdMancanti)
        Gr = Nothing

        If grdMancanti.Rows.Count > 0 Then
            divMancanti.Visible = True
            cmdAvvisa1.Visible = True
            cmdAvvisa2.Visible = True
            cmdAvvisa3.Visible = True
        Else
            divMancanti.Visible = False
            cmdAvvisa1.Visible = False
            cmdAvvisa2.Visible = False
            cmdAvvisa3.Visible = False
        End If
    End Sub

    Private Sub grdMancanti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMancanti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Protected Sub cmdChiude_Click(sender As Object, e As EventArgs) Handles cmdChiude.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Rec2 As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Ok As Boolean
            Dim Tappati() As String = {}
            Dim Squadre() As String = {}
            Dim Prese() As Boolean = {}
            Dim qSquadre As Integer = 0
            Dim qTappati As Integer = 0
            Dim x As Integer

            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim gg As Integer

            If Speciale = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            ' Gestione Sudden Death
            If Speciale = False Then
                If gg > 3 And gg < 100 Then
                    Sql = "Delete From SuddenDeathDett Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Select * From Schedine Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
                    Rec = Db.LeggeQuery(ConnSQL, Sql)
                    Do Until Rec.Eof
                        qSquadre += 1
                        redim Preserve Squadre(qSquadre)
                        redim Preserve Prese(qSquadre)
                        Prese(qSquadre) = False
                        Squadre(qSquadre) = Rec("SquadraCasa").Value

                        qSquadre += 1
                        redim Preserve Squadre(qSquadre)
                        redim Preserve Prese(qSquadre)
                        Prese(qSquadre) = False
                        Squadre(qSquadre) = Rec("SquadraFuori").Value

                        Rec.MoveNext()
                    Loop
                    Rec.Close()
                End If
            End If
            ' Gestione Sudden Death

            ' Mettere la gestione dei tappati

            Dim QuantiGiocatori As Integer = 0
            Dim QuantiEsclusi As Integer = 0

            Sql = "Select Count(*) From SuddenDeathEsclusi Where Anno = " & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Not Rec(0).Value Is DBNull.Value Then
                QuantiEsclusi = Rec(0).Value
            End If
            Rec.Close

            Sql = "Select Count(*) From Giocatori Where Anno = " & DatiGioco.AnnoAttuale & " And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Not Rec(0).Value Is DBNull.Value Then
                QuantiGiocatori = Rec(0).Value
            End If
            Rec.Close

            Sql = "Delete From SuddenDeathDett Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Select Distinct(CodGiocatore) From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If DatiGioco.Giornata > 3 And Speciale = False And (QuantiGiocatori - QuantiEsclusi) > 1 Then
                    Sql = "Select * From SuddenDeathEsclusi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec("CodGiocatore").Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        ' Gestione Sudden Death
                        Randomize()
                        x = Int(Rnd(1) * qSquadre) + 1
                        If x = 0 Then x = 1
                        If x > qSquadre Then x = qSquadre
                        Do While Prese(x) = True
                            Randomize()
                            x = Int(Rnd(1) * qSquadre) + 1
                            If x = 0 Then x = 1
                            If x > qSquadre Then x = qSquadre
                            Ok = False
                            For i As Integer = 1 To qSquadre
                                If Prese(i) = False Then
                                    Ok = True
                                    Exit For
                                End If
                            Next
                            If Ok = False Then
                                For i As Integer = 1 To qSquadre
                                    Prese(i) = False
                                Next
                            End If
                        Loop
                        Prese(x) = True
                        Sql = "Insert Into SuddenDeathDett Values (" &
                                " " & DatiGioco.AnnoAttuale & ", " &
                                " " & gg & ", " &
                                " " & Rec(0).Value & ", " &
                                "'" & SistemaTestoPerDB(Squadre(x)) & "' " &
                                ")"
                        Db.EsegueSql(ConnSQL, Sql)
                        ' Gestione Sudden Death
                    End If
                    Rec2.Close()
                End If

                Sql = "Select * From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And CodGiocatore=" & Rec(0).Value
                Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                Ok = False
                If Rec2.Eof = True Then
                    ' Il giocatore non ha giocato. Tappare
                    Ok = True
                End If
                Rec2.Close()
                If Ok = True Then
                    Dim Colonna As String = ""
                    Dim Risultato As String

                    ' Prende il tappo del giocatore
                    Sql = "Select Colonna From Tappi Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)
                    If Rec2.Eof = True Then
                        ' Il giocatore non ha tappi...
                        ' Creo colonna ad minchiam
                        Colonna = TornaColonnaRandom()
                    Else
                        Colonna = Rec2("Colonna").Value
                    End If

                    Dim Segno As String

                    For i As Integer = 1 To 14

                        Segno = Mid(Colonna, i, 1)

                        Risultato = TornaRisultatoRandom(Segno)

                        Sql = "Insert Into Pronostici Values (" &
                            " " & DatiGioco.AnnoAttuale & ", " &
                            " " & gg & ", " &
                            " " & Rec(0).Value & ", " &
                            " " & i.ToString & ", " &
                            "'" & Segno & "', " &
                            "'" & Risultato & "' " &
                            ")"
                        Db.EsegueSql(ConnSQL, Sql)
                    Next
                    Rec2.Close()

                    ' Aggiorna il numero di tappi
                    Sql = "Update DettaglioGiocatori Set NumeroTappi=NumeroTappi+1 " &
                        "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
                    Db.EsegueSql(ConnSQL, Sql)

                    ' Inserisce la riga per il quando tappi
                    Sql = "Delete From QuandoTappi Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And CodGiocatore=" & Rec(0).Value
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Insert Into QuandoTappi Values (" &
                        " " & DatiGioco.AnnoAttuale & ", " &
                        " " & Rec(0).Value & ", " &
                        " " & gg & " " &
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Randomize()
                    Dim fr As Integer = Int(Rnd(1) * 14) + 1
                    If fr < 1 Then fr = 1
                    If fr > 14 Then fr = 14

                    Sql = "Insert Into FilsRouge Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & gg & ", " & _
                        " " & Rec(0).Value & ", " & _
                        " " & fr & " " & _
                        ")"
                    Db.EsegueSql(ConnSQL, Sql)

                    Sql = "Select Giocatore From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & Rec(0).Value
                    Rec2 = Db.LeggeQuery(ConnSQL, Sql)

                    qTappati += 1
                    redim Preserve Tappati(qTappati)
                    Tappati(qTappati) = Rec("CodGiocatore").Value & ";" & Rec2("Giocatore").Value & ";"

                    Rec2.Close()
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            DatiGioco.StatoConcorso = ValoriStatoConcorso.DaControllare
            AggiornaDatiDiGioco(Db)

            ConnSQL.Close()

            Dim gMail As New GestioneMail

            If gg > 100 Then gg -= 100
            gMail.InviaMailChiusuraConcorso(gg, Tappati, Session("Nick"), Speciale)

            gMail = Nothing
        End If

        Db = Nothing

        Response.Redirect("Principale.aspx")
    End Sub

    Protected Sub cmdAvvisa1_Click(sender As Object, e As EventArgs) Handles cmdAvvisa1.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql
            Dim Giocatori() As Integer
            Dim qGioc As Integer

            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim gg As Integer

            If Speciale = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Sql = "Select CodGiocatore, Giocatore From Giocatori Where CodGiocatore Not In (" & _
                "Select Distinct(CodGiocatore) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & ") And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                qGioc += 1
                redim Preserve Giocatori(qGioc)
                Giocatori(qGioc) = Rec("CodGiocatore").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim gMail As New GestioneMail

            If gg > 100 Then gg -= 100
            gMail.InviaMailAvvisoChiusuraConcorsoSoft(gg, Giocatori, Session("Nick"), Speciale)

            gMail = Nothing

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdAvvisa2_Click(sender As Object, e As EventArgs) Handles cmdAvvisa2.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql
            Dim Giocatori() As Integer
            Dim qGioc As Integer

            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim gg As Integer

            If Speciale = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Sql = "Select CodGiocatore, Giocatore From Giocatori Where CodGiocatore Not In (" & _
                "Select Distinct(CodGiocatore) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & ") And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                qGioc += 1
                redim Preserve Giocatori(qGioc)
                Giocatori(qGioc) = Rec("CodGiocatore").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim gMail As New GestioneMail

            If gg > 100 Then gg -= 100
            gMail.InviaMailAvvisoChiusuraConcorsoHard(gg, Giocatori, Session("Nick"), Speciale)

            gMail = Nothing

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Protected Sub cmdAvvisa3_Click(sender As Object, e As EventArgs) Handles cmdAvvisa3.Click
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql
            Dim Giocatori() As Integer
            Dim qGioc As Integer

            Dim Speciale As Boolean = ControllaConcorsoSpeciale(Db, ConnSQL)
            Dim gg As Integer

            If Speciale = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Sql = "Select CodGiocatore, Giocatore From Giocatori Where CodGiocatore Not In (" &
                "Select Distinct(CodGiocatore) From Pronostici Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & ") And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                qGioc += 1
                redim Preserve Giocatori(qGioc)
                Giocatori(qGioc) = Rec("CodGiocatore").Value

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim gMail As New GestioneMail

            If gg > 100 Then gg -= 100
            gMail.InviaMailAvvisoChiusuraConcorsoLast(gg, Giocatori, Session("Nick"), Speciale)

            gMail = Nothing

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub
End Class