Public Class ColonnaTappo
    Inherits System.Web.UI.Page

    Private Colonna As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PrendeColonna()

            If DatiGioco.StatoConcorso = ValoriStatoConcorso.Aperto Then
                Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

                If DataArrivo < Now Then
                    cmdUsa.Visible = False
                Else
                    cmdUsa.Visible = True
                End If
            Else
                cmdUsa.Visible = False
            End If
        End If
    End Sub

    Private Sub PrendeColonna()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.* From Tappi A " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "A.CodGiocatore=" & Session("CodGiocatore")
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Colonna = ""
            Else
                Colonna = Rec("Colonna").Value
            End If
            Rec.Close()

            ConnSQL.Close()

            CreaTabella()
        End If

        Db = Nothing
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

        cmdUsa.Visible = False
    End Sub

    Private Sub grdPartite_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim TxtSegno As TextBox = DirectCast(e.Row.FindControl("TxtSegno"), TextBox)
            Dim Partita As Integer = e.Row.Cells(0).Text

            TxtSegno.Text = Mid(Colonna, Partita, 1)
            TxtSegno.DataBind()
        End If
    End Sub

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Dim TxtSegno As TextBox
        Dim I As Integer
        Dim Ok As Boolean = True
        Dim Appoggio As String = ""

        For I = 0 To grdPartite.Rows.Count - 1
            TxtSegno = DirectCast(grdPartite.Rows(I).FindControl("TxtSegno"), TextBox)
            TxtSegno.Text = TxtSegno.Text.Trim.ToUpper
            If TxtSegno.Text = "" And TxtSegno.Text <> "1" And TxtSegno.Text <> "X" And TxtSegno.Text <> "2" Then
                Dim Messaggi() As String = {"Segno " & I + 1 & " non valido"}
                VisualizzaMessaggioInPopup(Messaggi, Me)

                TxtSegno.Focus()
                TxtSegno.DataBind()
                Ok = False
                Exit For
            Else
                Appoggio = Appoggio & TxtSegno.Text.Trim.ToUpper
            End If
        Next
        If Ok = True Then
            Dim Db As New GestioneDB

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Sql = "Delete From Tappi Where " & _
                    "Anno=" & DatiGioco.AnnoAttuale & " And " & _
                    "CodGiocatore=" & Session("CodGiocatore")
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Insert Into Tappi Values (" & _
                        " " & DatiGioco.AnnoAttuale & ", " & _
                        " " & Session("CodGiocatore") & ", " & _
                        "'" & Appoggio & "' " & _
                        ")"
                Dim Messaggi() As String = {"Colonna tappo inserita"}
                VisualizzaMessaggioInPopup(Messaggi, Me)

                Db.EsegueSql(ConnSQL, Sql)

                ConnSQL.Close()

                PrendeColonna()
                CreaTabella()
            End If
        End If
    End Sub

    Protected Sub cmdRandom_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRandom.Click
        Dim I As Integer
        Dim Segni As String = "1X2"
        Dim X As Integer
        Dim TxtSegno As TextBox

        For I = 0 To grdPartite.Rows.Count - 1
            TxtSegno = DirectCast(grdPartite.Rows(I).FindControl("TxtSegno"), TextBox)

            Randomize()
            X = Int(Rnd(1) * 3) + 1

            TxtSegno.Text = Mid(Segni, X, 1)
            TxtSegno.DataBind()
        Next I
    End Sub

    Protected Sub cmdUsa_Click(sender As Object, e As EventArgs) Handles cmdUsa.Click
        Dim DataArrivo As Date = DatiGioco.ChiusuraConcorso

        If DataArrivo < Now Then
            Dim Messaggi() As String = {"Il concorso è chiuso"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim TxtSegno As TextBox
            Dim Risultato As String

            Sql = "Delete From Pronostici Where " &
                "Anno=" & DatiGioco.AnnoAttuale & " And " &
                "Giornata=" & DatiGioco.Giornata & " And " &
                "Codgiocatore=" & Session("CodGiocatore")
            Db.EsegueSql(ConnSQL, Sql)

            For i As Integer = 0 To grdPartite.Rows.Count - 1
                TxtSegno = DirectCast(grdPartite.Rows(i).FindControl("TxtSegno"), TextBox)

                Risultato = TornaRisultatoRandom(TxtSegno.Text)

                Sql = "Insert Into Pronostici Values (" &
                    " " & DatiGioco.AnnoAttuale & ", " &
                    " " & DatiGioco.Giornata & ", " &
                    " " & Session("CodGiocatore") & ", " &
                    " " & i + 1 & ", " &
                    "'" & TxtSegno.Text & "', " &
                    "'" & Risultato & "' " &
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            Next

            Dim Messaggi() As String = {"Colonna tappo impostata<br />Il contatore dei tappi non sarà increementato"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub
End Class