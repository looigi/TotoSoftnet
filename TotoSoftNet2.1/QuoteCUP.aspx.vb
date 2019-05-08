Public Class QuoteCUP
    Inherits System.Web.UI.Page

    Private Contatore As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaNomi()
            CaricaDati()
            DisegnaTabelloneQuoteCUP(divScontriDiretti)

            Menu1.Items(0).Selected = True
        End If
    End Sub

    Private Sub ControllaVincitore(Db As GestioneDB, ConnSql As Object)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        divVincente.Visible = False

        Sql = "Select B.Giocatore From QuoteCUP_Vincenti A "
        Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & "   "
        Sql &= "Left Join Giocatori B On A.Anno=B.Anno And C.Progressivo=B.CodGiocatore "
        Sql &=     "Where A.Anno=" & DatiGioco.AnnoAttuale
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = False Then
            divVincente.Visible = True
            lblVincitore.Text = Rec("Giocatore").Value
            imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
        End If
        Rec.Close()
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Private Sub CaricaNomi()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori " & _
                "Where Anno=" & DatiGioco.AnnoAttuale & " And " & _
                "Cancellato='N' And " & _
                "CodGiocatore In (Select CodGiocatore From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale & ") " & _
                "Order By Giocatore"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            cmbGiocatori.Items.Clear()
            Do Until Rec.Eof
                cmbGiocatori.Items.Add(Rec("Giocatore").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            cmbGiocatori.Text = Session("Nick")

            ControllaVincitore(Db, ConnSQL)

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub CaricaDati()
        Dim Sql As String = ""

        For i As Integer = 0 To 3
            Dim Gr As New Griglie

            Sql = "Select 0, B.Giocatore, SUM(Punti) As PuntiTot "
            Sql &= "From QuoteCUP_Risultati A "
            Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & "   "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
            Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And A.Concorso<=" & DatiGioco.Giornata & " And C.Progressivo In (" & NumeriGruppiQuoteCUP(i) & ") "
            Sql &= "And Giocatore Is Not Null "
            Sql &= "Group By B.Giocatore "
            Sql &= "Order By 3 Desc"
            Select Case i
                Case 0
                    Contatore = 0
                    Gr.ImpostaCampi(Sql, grd1A)
                Case 1
                    Contatore = 0
                    Gr.ImpostaCampi(Sql, grd1B)
                Case 2
                    Contatore = 0
                    Gr.ImpostaCampi(Sql, grd2C)
                Case 3
                    Contatore = 0
                    Gr.ImpostaCampi(Sql, grd2D)
            End Select
            Gr = Nothing
        Next

        CaricaDatiPartite(DatiGioco.Giornata - 5)
        CaricaProprie()
        CaricaClassifiche()
    End Sub

    Private Sub CaricaProprie(Optional AltroGiocatore As String = "")
        Dim Sql As String = ""
        Dim Gr As New Griglie
        Dim codGioc As Integer = Session("CodGiocatore")

        If AltroGiocatore <> "" Then
            Dim g As New Giocatori
            codGioc = g.TornaIdGiocatore(AltroGiocatore)
            g = Nothing
        End If

        Sql = "Select A.Giornata, B.Giocatore, E.Giocatore, ISNULL(F.PuntiTot,-1) As Ris1, IsNULL(G.PuntiTot,-1) As Ris2, -1 As Segno, -1 As Punti "
        Sql &= "From QuoteCUP_Abbinamenti A "
        Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & "   "
        Sql &= "Left Join QuoteCUP_Squadre D On A.CodAvversario=D.Progressivo And D.Anno=" & DatiGioco.AnnoAttuale & "   "
        Sql &= "Left Join Giocatori B On C.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
        Sql &= "Left Join Giocatori E On C.Anno=E.Anno And D.CodGiocatore=E.CodGiocatore "
        Sql &= "Left Join QuoteCUP_Risultati F On C.Anno=F.Anno And C.Progressivo=F.CodGiocatore And A.Giornata=F.Concorso "
        Sql &= "Left Join QuoteCUP_Risultati G On C.Anno=G.Anno And D.Progressivo=G.CodGiocatore And A.Giornata=G.Concorso "
        Sql &= "Where C.Anno = " & DatiGioco.AnnoAttuale & " And (C.CodGiocatore = " & codGioc & " Or D.CodGiocatore = " & codGioc & ") "
        Sql &= "Union All "
        Sql &= "Select Distinct Giornata, 'Riposo' As Giocatore, '' , -1 As Ris1, -1 As Ris2, -1 As Segno, -1 As Punti "
        Sql &= "From QuoteCUP_Abbinamenti "
        Sql &= "Where Giornata Not In ("
        Sql &= "Select A.Giornata "
        Sql &= "From QuoteCUP_Abbinamenti A "
        Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & "   "
        Sql &= "Left Join QuoteCUP_Squadre D On A.CodAvversario=D.Progressivo And D.Anno=" & DatiGioco.AnnoAttuale & "   "
        Sql &= "Left Join Giocatori B On C.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
        Sql &= "Left Join Giocatori E On C.Anno=E.Anno And D.CodGiocatore=E.CodGiocatore "
        Sql &= "Left Join QuoteCUP_Risultati F On C.Anno=F.Anno And C.Progressivo=F.CodGiocatore And A.Giornata=F.Concorso "
        Sql &= "Left Join QuoteCUP_Risultati G On C.Anno=G.Anno And D.Progressivo=G.CodGiocatore And A.Giornata=G.Concorso "
        Sql &= "Where C.Anno = " & DatiGioco.AnnoAttuale & " And (C.CodGiocatore = " & codGioc & " Or D.CodGiocatore = " & codGioc & ") "
        Sql &= ")            "
        Sql &= "Order By A.Giornata"
        Gr.ImpostaCampi(Sql, grdProprie)
        Gr = Nothing

        ScriveGirone()
    End Sub

    Private Sub CaricaClassifiche()
        Dim Sql As String = ""
        Dim Gr1 As New Griglie
        Dim Gr2 As New Griglie

        ' Gruppo 1
        Contatore = 0
        Sql = "Select C.CodGiocatore, C.Giocatore, SUM(A.Punti) As Punti, SUM(PuntiTot) As PuntiTot From QuoteCUP_Risultati A "
        Sql &= "Left Join QuoteCUP_Squadre B On A.Anno = B.Anno And A.CodGiocatore = B.Progressivo "
        Sql &= "Left Join Giocatori C On C.Anno = A.Anno And C.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Giocatore Is Not Null And Progressivo <= 6 "
        Sql &= "Group By C.CodGiocatore, C.Giocatore "
        Sql &=     "Order By 3 Desc, 4 Desc"
        Gr1.ImpostaCampi(Sql, grdClassGruppo1)
        ' Gruppo 1

        ' Gruppo 2
        Contatore = 0
        Sql = "Select C.CodGiocatore, C.Giocatore, SUM(A.Punti) As Punti, SUM(PuntiTot) As PuntiTot From QuoteCUP_Risultati A "
        Sql &= "Left Join QuoteCUP_Squadre B On A.Anno = B.Anno And A.CodGiocatore = B.Progressivo "
        Sql &= "Left Join Giocatori C On C.Anno = A.Anno And C.CodGiocatore = B.CodGiocatore "
        Sql &= "Where A.Anno = " & DatiGioco.AnnoAttuale & " And Giocatore Is Not Null And Progressivo > 6 "
        Sql &= "Group By C.CodGiocatore, C.Giocatore "
        Sql &=    "Order By 3 Desc, 4 Desc"
        Gr2.ImpostaCampi(Sql, grdClassGruppo2)
        ' Gruppo 2

        Gr2 = Nothing
        Gr1 = Nothing
    End Sub

    Private Sub ScriveGirone()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim g As New Giocatori
            Dim cod As Integer = g.TornaIdGiocatore(cmbGiocatori.Text)
            Dim Quale As Integer = -1

            Sql = "Select * From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & cod
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Quale = Rec("Progressivo").Value
            End If
            Rec.Close()

            lblGirone.Text = ""
            If Quale < 4 Then
                lblGirone.Text = "Gruppo 1 - Girone A"
            Else
                If Quale > 3 And Quale < 7 Then
                    lblGirone.Text = "Gruppo 1 - Girone B"
                Else
                    If Quale > 6 And Quale < 10 Then
                        lblGirone.Text = "Gruppo 2 - Girone C"
                    Else
                        If Quale > 9 Then
                            lblGirone.Text = "Gruppo 2 - Girone D"
                        End If
                    End If
                End If
            End If

            g = Nothing
            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Sub CaricaDatiPartite(g As Integer, Optional SoloQuesta As Integer = -1)
        Dim Sql As String = ""
        Dim i As Integer = 0
        Dim Arrivo As Integer = 3

        If g >= QuantePartiteQuoteCUP Then
            g = QuantePartiteQuoteCUP - 1
        End If

        If SoloQuesta <> -1 Then
            i = SoloQuesta
            Arrivo = SoloQuesta
        End If

        Do While i <= Arrivo
            Dim GrP As New Griglie

            Sql = "Select B.Giocatore, E.Giocatore, ISNULL(F.PuntiTot,-1) As Ris1, IsNULL(G.PuntiTot,-1) As Ris2, -1 As Punti "
            Sql &= "From QuoteCUP_Abbinamenti A "
            Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & " "
            Sql &= "Left Join QuoteCUP_Squadre D On A.CodAvversario=D.Progressivo And D.Anno=" & DatiGioco.AnnoAttuale & " "
            Sql &= "Left Join Giocatori B On C.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
            Sql &= "Left Join Giocatori E On C.Anno=E.Anno And D.CodGiocatore=E.CodGiocatore "
            Sql &= "Left Join QuoteCUP_Risultati F On C.Anno=F.Anno And C.Progressivo=F.CodGiocatore And A.Giornata=F.Concorso "
            Sql &= "Left Join QuoteCUP_Risultati G On C.Anno=G.Anno And D.Progressivo=G.CodGiocatore And A.Giornata=G.Concorso "
            Sql &= "Where C.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata=" & g & " And (C.Progressivo In (" & NumeriGruppiQuoteCUP(i) & ") Or D.Progressivo In (" & NumeriGruppiQuoteCUP(i) & ")) "
            Sql &= "Union All "
            Sql &= "Select B.Giocatore, 'Riposo', -1 As Ris1, -1 As Ris2, -1 As Punti From  "
            Sql &= "QuoteCUP_Squadre A "
            Sql &= "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore "
            Sql &= "Where A.Anno=" & DatiGioco.AnnoAttuale & " And Progressivo In (" & NumeriGruppiQuoteCUP(i) & ") And Progressivo Not In ( "
            Sql &= "Select C.Progressivo  "
            Sql &= "From QuoteCUP_Abbinamenti A "
            Sql &= "Left Join QuoteCUP_Squadre C On A.CodGiocatore=C.Progressivo And C.Anno=" & DatiGioco.AnnoAttuale & "  "
            Sql &= "Left Join Giocatori B On C.Anno=B.Anno And C.CodGiocatore=B.CodGiocatore "
            Sql &= "Left Join QuoteCUP_Risultati F On C.Anno=F.Anno And C.Progressivo=F.CodGiocatore And A.Giornata=F.Concorso "
            Sql &= "Where C.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata=" & g & " And C.Progressivo In (" & NumeriGruppiQuoteCUP(i) & ")) "
            Sql &= "And Progressivo Not In ( "
            Sql &= "Select D.Progressivo  "
            Sql &= "From QuoteCUP_Abbinamenti A "
            Sql &= "Left Join QuoteCUP_Squadre D On A.CodAvversario=D.Progressivo And D.Anno=" & DatiGioco.AnnoAttuale & "  "
            Sql &= "Left Join Giocatori E On D.Anno=E.Anno And D.CodGiocatore=E.CodGiocatore "
            Sql &= "Left Join QuoteCUP_Risultati G On D.Anno=G.Anno And D.Progressivo=G.CodGiocatore And A.Giornata=G.Concorso "
            Sql &= "Where D.Anno = " & DatiGioco.AnnoAttuale & " And A.Giornata=" & g & "And D.Progressivo In (" & NumeriGruppiQuoteCUP(i) & "))"
            Select Case i
                Case 0
                    Contatore = 0
                    GrP.ImpostaCampi(Sql, grdP1A)
                    hdnGiornata1A.Value = g
                    lblGiornata1A.Text = "Giornata " & g
                Case 1
                    Contatore = 0
                    GrP.ImpostaCampi(Sql, grdP1B)
                    hdnGiornata1B.Value = g
                    lblGiornata1B.Text = "Giornata " & g
                Case 2
                    Contatore = 0
                    GrP.ImpostaCampi(Sql, grdP2C)
                    hdnGiornata2C.Value = g
                    lblGiornata2C.Text = "Giornata " & g
                Case 3
                    Contatore = 0
                    GrP.ImpostaCampi(Sql, grdP2D)
                    hdnGiornata2D.Value = g
                    lblGiornata2D.Text = "Giornata " & g
            End Select
            GrP = Nothing

            i += 1
        Loop
    End Sub

    Private Sub grdClassGruppo1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdClassGruppo1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore
            If Contatore < 5 Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#b9efbd")
                Next
            End If

            Dim g As New Giocatori
            Dim idGiocatore As Integer = g.TornaIdGiocatore(Nome)
            If idGiocatore = Session("CodGiocatore") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If
            g = Nothing

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grdClassGruppo2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdClassGruppo2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore
            If Contatore < 5 Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", "#b9efbd")
                Next
            End If

            Dim g As New Giocatori
            Dim idGiocatore As Integer = g.TornaIdGiocatore(Nome)
            If idGiocatore = Session("CodGiocatore") Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", ColoreSfondoRigaPropria)
                    e.Row.Cells(i).Style.Add("color", ColoreTestoRigaPropria)
                Next
            End If
            g = Nothing

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grd1A_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd1A.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grd1B_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd1B.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grd2C_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd2C.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    Private Sub grd2D_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd2D.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtente As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(2).Text

            Contatore += 1
            e.Row.Cells(0).Text = Contatore

            ImgUtente.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtente.DataBind()
        End If
    End Sub

    ' ----------------------
    Private Function ControllaGruppoGiocatore(Giocatore As String) As String
        Dim Ritorno As String = ""
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Quale As Integer = -1
            Dim g As New Giocatori
            Dim codGioc As Integer = g.TornaIdGiocatore(Giocatore)
            g = Nothing

            Sql = "Select Progressivo From QuoteCUP_Squadre Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codGioc
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Quale = Rec("Progressivo").Value
            End If
            Rec.Close()

            If Quale > 0 And Quale < 4 Then
                Ritorno = "Gruppo 1<br />Girone A"
            Else
                If Quale > 3 And Quale < 7 Then
                    Ritorno = "Gruppo 1<br />Girone B"
                Else
                    If Quale > 6 And Quale < 10 Then
                        Ritorno = "Gruppo 2<br />Girone C"
                    Else
                        If Quale > 9 Then
                            Ritorno = "Gruppo 2<br />Girone D"
                        End If
                    End If
                End If
            End If

            If Ritorno <> "" Then
                Ritorno = "<span style=""color: #aa0000; font-size: 10px;"">" & Ritorno & "</span>"
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Private Sub grdp1A_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdP1A.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtenteC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)
            Dim ImgUtenteF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            e.Row.Cells(1).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteC.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteC.DataBind()

            Nome = e.Row.Cells(3).Text
            e.Row.Cells(3).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteF.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteF.DataBind()

            If e.Row.Cells(4).Text = "-1,00" Or e.Row.Cells(5).Text = "-1,00" Then
                e.Row.Cells(4).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
            Else
                Dim r1 As Single = e.Row.Cells(4).Text
                Dim r2 As Single = e.Row.Cells(5).Text

                If r1 > r2 Then
                    e.Row.Cells(6).Text = "1"
                Else
                    If r1 < r2 Then
                        e.Row.Cells(6).Text = "2"
                    Else
                        e.Row.Cells(6).Text = "X"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub grdp1B_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdP1B.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtenteC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)
            Dim ImgUtenteF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            e.Row.Cells(1).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteC.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteC.DataBind()

            Nome = e.Row.Cells(3).Text
            e.Row.Cells(3).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteF.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteF.DataBind()

            If e.Row.Cells(4).Text = "-1,00" Or e.Row.Cells(5).Text = "-1,00" Then
                e.Row.Cells(4).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
            Else
                Dim r1 As Single
                Dim r2 As Single

                Try
                    r1 = e.Row.Cells(4).Text
                    r2 = e.Row.Cells(5).Text
                Catch ex As Exception
                    r1 = 0
                    r2 = 0
                End Try

                If r1 > r2 Then
                    e.Row.Cells(6).Text = "1"
                Else
                    If r1 < r2 Then
                        e.Row.Cells(6).Text = "2"
                    Else
                        e.Row.Cells(6).Text = "X"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub grdp2C_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdP2C.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtenteC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)
            Dim ImgUtenteF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            e.Row.Cells(1).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteC.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteC.DataBind()

            Nome = e.Row.Cells(3).Text
            e.Row.Cells(3).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteF.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteF.DataBind()

            If e.Row.Cells(4).Text = "-1,00" Or e.Row.Cells(5).Text = "-1,00" Then
                e.Row.Cells(4).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
            Else
                Dim r1 As Single = e.Row.Cells(4).Text
                Dim r2 As Single = e.Row.Cells(5).Text

                If r1 > r2 Then
                    e.Row.Cells(6).Text = "1"
                Else
                    If r1 < r2 Then
                        e.Row.Cells(6).Text = "2"
                    Else
                        e.Row.Cells(6).Text = "X"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub grdp2D_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdP2D.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtenteC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)
            Dim ImgUtenteF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            e.Row.Cells(1).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteC.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteC.DataBind()

            Nome = e.Row.Cells(3).Text
            e.Row.Cells(3).Text &= "<br />" & ControllaGruppoGiocatore(Nome)

            ImgUtenteF.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteF.DataBind()

            If e.Row.Cells(4).Text = "-1,00" Or e.Row.Cells(5).Text = "-1,00" Then
                e.Row.Cells(4).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
            Else
                Dim r1 As Single = e.Row.Cells(4).Text
                Dim r2 As Single = e.Row.Cells(5).Text

                If r1 > r2 Then
                    e.Row.Cells(6).Text = "1"
                Else
                    If r1 < r2 Then
                        e.Row.Cells(6).Text = "2"
                    Else
                        e.Row.Cells(6).Text = "X"
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub cmdIndietro1A_Click(sender As Object, e As EventArgs) Handles cmdIndietro1A.Click
        If hdnGiornata1A.Value > 1 Then
            hdnGiornata1A.Value -= 1

            CaricaDatiPartite(hdnGiornata1A.Value, 0)
        End If
    End Sub

    Protected Sub cmdIndietro1B_Click(sender As Object, e As EventArgs) Handles cmdIndietro1B.Click
        If hdnGiornata1B.Value > 1 Then
            hdnGiornata1B.Value -= 1

            CaricaDatiPartite(hdnGiornata1B.Value, 1)
        End If
    End Sub

    Protected Sub cmdIndietro2C_Click(sender As Object, e As EventArgs) Handles cmdIndietro2C.Click
        If hdnGiornata2C.Value > 1 Then
            hdnGiornata2C.Value -= 1

            CaricaDatiPartite(hdnGiornata2C.Value, 2)
        End If
    End Sub

    Protected Sub cmdIndietro2D_Click(sender As Object, e As EventArgs) Handles cmdIndietro2D.Click
        If hdnGiornata2D.Value > 1 Then
            hdnGiornata2D.Value -= 1

            CaricaDatiPartite(hdnGiornata2D.Value, 3)
        End If
    End Sub

    ' --------

    Protected Sub cmdAvanti1A_Click(sender As Object, e As EventArgs) Handles cmdAvanti1A.Click
        If hdnGiornata1A.Value < QuantePartiteQuoteCUP - 1 Then
            hdnGiornata1A.Value += 1

            CaricaDatiPartite(hdnGiornata1A.Value, 0)
        End If
    End Sub

    Protected Sub cmdAvanti1B_Click(sender As Object, e As EventArgs) Handles cmdAvanti1B.Click
        If hdnGiornata1B.Value < QuantePartiteQuoteCUP - 1 Then
            hdnGiornata1B.Value += 1

            CaricaDatiPartite(hdnGiornata1B.Value, 1)
        End If
    End Sub

    Protected Sub cmdAvanti2C_Click(sender As Object, e As EventArgs) Handles cmdAvanti2C.Click
        If hdnGiornata2C.Value < QuantePartiteQuoteCUP - 1 Then
            hdnGiornata2C.Value += 1

            CaricaDatiPartite(hdnGiornata2C.Value, 2)
        End If
    End Sub

    Protected Sub cmdAvanti2D_Click(sender As Object, e As EventArgs) Handles cmdAvanti2D.Click
        If hdnGiornata2D.Value < QuantePartiteQuoteCUP - 1 Then
            hdnGiornata2D.Value += 1

            CaricaDatiPartite(hdnGiornata2D.Value, 3)
        End If
    End Sub

    ' ---------------

    Protected Sub cmbGiocatori_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbGiocatori.SelectedIndexChanged
        CaricaProprie(cmbGiocatori.Text)
    End Sub

    Private Sub grdProprie_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdProprie.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Campone As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim ImgUtenteC As Image = DirectCast(e.Row.FindControl("imgAvatarC"), Image)
            Dim ImgUtenteF As Image = DirectCast(e.Row.FindControl("imgAvatarF"), Image)
            Dim Nome As String = e.Row.Cells(2).Text
            If e.Row.Cells(2).Text <> cmbGiocatori.Text And e.Row.Cells(2).Text.Trim.ToUpper <> "RIPOSO" Then
                e.Row.Cells(2).Text &= "<br />" & ControllaGruppoGiocatore(Nome)
            End If

            ImgUtenteC.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteC.DataBind()

            Nome = e.Row.Cells(4).Text
            If e.Row.Cells(4).Text <> cmbGiocatori.Text And e.Row.Cells(4).Text.Trim.ToUpper <> "RIPOSO" Then
                e.Row.Cells(4).Text &= "<br />" & ControllaGruppoGiocatore(Nome)
            End If

            ImgUtenteF.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
            ImgUtenteF.DataBind()

            If e.Row.Cells(5).Text = "-1,00" Or e.Row.Cells(6).Text = "-1,00" Then
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
                e.Row.Cells(7).Text = ""
                e.Row.Cells(8).Text = ""
            Else
                Dim r1 As Single = e.Row.Cells(5).Text
                Dim r2 As Single = e.Row.Cells(6).Text

                Dim Diffe As Single = r1 - r2

                Select Case Diffe
                    Case Is > 0.5
                        e.Row.Cells(7).Text = "1"
                        If e.Row.Cells(2).Text = cmbGiocatori.Text Then
                            e.Row.Cells(8).Text = "3"
                        Else
                            e.Row.Cells(8).Text = "0"
                        End If
                    Case Is < -0.5
                        e.Row.Cells(7).Text = "2"
                        If e.Row.Cells(2).Text = cmbGiocatori.Text Then
                            e.Row.Cells(8).Text = "0"
                        Else
                            e.Row.Cells(8).Text = "3"
                        End If
                    Case Else
                        e.Row.Cells(7).Text = "X"
                        e.Row.Cells(8).Text = "1"
                End Select
            End If
        End If
    End Sub
End Class