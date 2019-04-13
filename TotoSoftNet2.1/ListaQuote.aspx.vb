Imports System.IO

Public Class ListaQuote
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            divVelo.Visible = False
            divModifica.Visible = False
            hdnPartita.Value = ""

            PrendeColonna()
        End If
    End Sub

    Protected Sub SelezionaQuota(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Partita As Integer = row.Cells(1).Text

        hdnPartita.Value = Partita

        divVelo.Visible = True
        divModifica.Visible = True

        Dim gM As New GestioneMail
        Dim Path As String

        Path = gM.RitornaImmagineSquadra(row.Cells(2).Text, True)

        If File.Exists(Server.MapPath(Path)) = True Then
            imgCasa.ImageUrl = Path
        Else
            imgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
        End If

        Path = gM.RitornaImmagineSquadra(row.Cells(4).Text, True)
        If File.Exists(Server.MapPath(Path)) = True Then
            imgFuori.ImageUrl = Path
        Else
            imgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
        End If

        lblPartita.Text = row.Cells(2).Text & " - " & row.Cells(4).Text

        Dim Db As New GestioneDB
        Dim gg As Integer

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")

            If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                gg = DatiGioco.GiornataSpeciale + 100
            Else
                gg = DatiGioco.Giornata
            End If

            Dim Sql As String = "Select A.Partita, B.SquadraCasa, B.SquadraFuori, " & _
                 "r1, rX, r2, r1X, r12, rX2, " & _
                 "r0_0, r0_1, r0_2, r0_3, r0_4, " & _
                 "r1_0, r1_1, r1_2, r1_3, r1_4, " & _
                 "r2_0, r2_1, r2_2, r2_3, r2_4, " & _
                 "r3_0, r3_1, r3_2, r3_3, r3_4, " & _
                 "r4_0, r4_1, r4_2, r4_3, r4_4, rAltro " & _
                 "From Quote A Left Join Schedine B On A.Giornata = B.Giornata And A.Anno = B.Anno And A.Partita = B.Partita " & _
                 "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & gg & " And A.Partita=" & Partita

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                txt1.Text = Rec("r1").Value
                txtX.Text = Rec("rX").Value
                txt2.Text = Rec("r2").Value

                txtS1X.Text = Rec("r1x").Value
                txtS12.Text = Rec("r12").Value
                txtSX2.Text = Rec("rx2").Value

                txt00.Text = Rec("r0_0").Value
                txt01.Text = Rec("r0_1").Value
                txt02.Text = Rec("r0_2").Value
                txt03.Text = Rec("r0_3").Value
                txt04.Text = Rec("r0_4").Value

                txt10.Text = Rec("r1_0").Value
                txt11.Text = Rec("r1_1").Value
                txt12.Text = Rec("r1_2").Value
                txt13.Text = Rec("r1_3").Value
                txt14.Text = Rec("r1_4").Value

                txt20.Text = Rec("r2_0").Value
                txt21.Text = Rec("r2_1").Value
                txt22.Text = Rec("r2_2").Value
                txt23.Text = Rec("r2_3").Value
                txt24.Text = Rec("r2_4").Value

                txt30.Text = Rec("r3_0").Value
                txt31.Text = Rec("r3_1").Value
                txt32.Text = Rec("r3_2").Value
                txt33.Text = Rec("r3_3").Value
                txt34.Text = Rec("r3_4").Value

                txt40.Text = Rec("r4_0").Value
                txt41.Text = Rec("r4_1").Value
                txt42.Text = Rec("r4_2").Value
                txt43.Text = Rec("r4_3").Value
                txt44.Text = Rec("r4_4").Value

                txtAltro.Text = Rec("rAltro").Value
            Else
                txt1.Text = ""
                txtX.Text = ""
                txt2.Text = ""

                txtS1X.Text = ""
                txtS12.Text = ""
                txtSX2.Text = ""

                txt00.Text = ""
                txt01.Text = ""
                txt02.Text = ""
                txt03.Text = ""
                txt04.Text = ""

                txt10.Text = ""
                txt11.Text = ""
                txt12.Text = ""
                txt13.Text = ""
                txt14.Text = ""

                txt20.Text = ""
                txt21.Text = ""
                txt22.Text = ""
                txt23.Text = ""
                txt24.Text = ""

                txt30.Text = ""
                txt31.Text = ""
                txt32.Text = ""
                txt33.Text = ""
                txt34.Text = ""

                txt40.Text = ""
                txt41.Text = ""
                txt42.Text = ""
                txt43.Text = ""
                txt44.Text = ""

                txtAltro.Text = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub

    Private Function ControllaValore(Valore As String, Campo As String) As Boolean
        Dim Ok As Boolean = True

        If Valore.Trim = "" Then
            Ok = False
        Else
            Valore = Valore.Replace(",", ".")
            If IsNumeric(Valore) = False Then
                Ok = False
            End If
        End If

        If Not Ok Then
            Dim sMessaggi() As String = {"Valore non valido nel campo: " & campo}
            VisualizzaMessaggioInPopup(sMessaggi, Me)
        End If

        Return Ok
    End Function

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim Ok As Boolean = True

        If Not ControllaValore(txt1.Text, "1") Then
            Ok = False
        End If
        If Ok Then
            If Not ControllaValore(txtX.Text, "X") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt2.Text, "2") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txtS1X.Text, "1X") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txtS12.Text, "12") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txtSX2.Text, "X2") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txt00.Text, "0-0") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt01.Text, "0-1") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt02.Text, "0-2") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt03.Text, "0-3") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt04.Text, "0-4") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txt10.Text, "1-0") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt11.Text, "1-1") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt12.Text, "1-2") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt13.Text, "1-3") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt14.Text, "1-4") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txt20.Text, "2-0") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt21.Text, "2-1") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt22.Text, "2-2") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt23.Text, "2-3") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt24.Text, "2-4") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txt30.Text, "3-0") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt31.Text, "3-1") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt32.Text, "3-2") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt33.Text, "3-3") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt34.Text, "3-4") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txt40.Text, "4-0") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt41.Text, "4-1") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt42.Text, "4-2") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt43.Text, "4-3") Then
                Ok = False
            End If
        End If
        If Ok Then
            If Not ControllaValore(txt44.Text, "4-4") Then
                Ok = False
            End If
        End If

        If Ok Then
            If Not ControllaValore(txtAltro.Text, "Altro") Then
                Ok = False
            End If
        End If

        If Ok Then
            Dim Db As New GestioneDB
            Dim gg As Integer

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String = ""

                If ControllaConcorsoSpeciale(Db, ConnSQL) = True Then
                    gg = DatiGioco.GiornataSpeciale + 100
                Else
                    gg = DatiGioco.Giornata
                End If

                Sql = "Update Quote Set " & _
                     "r1 = " & txt1.Text.Trim.Replace(",", ".") & ", rX = " & txtX.Text.Trim.Replace(",", ".") & ", r2 = " & txt2.Text.Trim.Replace(",", ".") & ", " & _
                     "r1X = " & txtS1X.Text.Trim.Replace(",", ".") & ", r12 = " & txtS12.Text.Trim.Replace(",", ".") & ", rX2 = " & txtSX2.Text.Trim.Replace(",", ".") & ", " & _
                     "r0_0 = " & txt00.Text.Trim.Replace(",", ".") & ", r0_1 = " & txt01.Text.Trim.Replace(",", ".") & ", r0_2 = " & txt02.Text.Trim.Replace(",", ".") & ", r0_3 = " & txt03.Text.Trim.Replace(",", ".") & ", r0_4 = " & txt04.Text.Trim.Replace(",", ".") & ", " & _
                     "r1_0 = " & txt10.Text.Trim.Replace(",", ".") & ", r1_1 = " & txt11.Text.Trim.Replace(",", ".") & ", r1_2 = " & txt12.Text.Trim.Replace(",", ".") & ", r1_3 = " & txt13.Text.Trim.Replace(",", ".") & ", r1_4 = " & txt14.Text.Trim.Replace(",", ".") & ", " & _
                     "r2_0 = " & txt20.Text.Trim.Replace(",", ".") & ", r2_1 = " & txt21.Text.Trim.Replace(",", ".") & ", r2_2 = " & txt22.Text.Trim.Replace(",", ".") & ", r2_3 = " & txt23.Text.Trim.Replace(",", ".") & ", r2_4 = " & txt24.Text.Trim.Replace(",", ".") & ", " & _
                     "r3_0 = " & txt30.Text.Trim.Replace(",", ".") & ", r3_1 = " & txt31.Text.Trim.Replace(",", ".") & ", r3_2 = " & txt32.Text.Trim.Replace(",", ".") & ", r3_3 = " & txt33.Text.Trim.Replace(",", ".") & ", r3_4 = " & txt34.Text.Trim.Replace(",", ".") & ", " & _
                     "r4_0 = " & txt40.Text.Trim.Replace(",", ".") & ", r4_1 = " & txt41.Text.Trim.Replace(",", ".") & ", r4_2 = " & txt42.Text.Trim.Replace(",", ".") & ", r4_3 = " & txt43.Text.Trim.Replace(",", ".") & ", r4_4 = " & txt44.Text.Trim.Replace(",", ".") & ", " & _
                     "rAltro = " & txtAltro.Text.Trim.Replace(",", ".") & " " & _
                     "From Quote " & _
                     "Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=" & gg & " And Partita=" & hdnPartita.Value
                Db.EsegueSql(ConnSQL, Sql)

                ConnSQL.Close()
            End If

            Db = Nothing

            Dim sMessaggi() As String = {"Quote salvate"}
            VisualizzaMessaggioInPopup(sMessaggi, Me)

            PrendeColonna()

            hdnPartita.Value = ""
            divVelo.Visible = False
            divModifica.Visible = False
        End If
    End Sub

    Protected Sub imgChiudi_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudi.Click
        hdnPartita.Value = ""
        divVelo.Visible = False
        divModifica.Visible = False
    End Sub

    Private Sub PrendeColonna()
        Dim GrP As New Griglie
        Dim GrLQ As New Griglie
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

        Db = Nothing

        Sql = "Select A.Partita, B.SquadraCasa, B.SquadraFuori, " & _
            "r1, '  ', rX, '  ', r2, '  ', r1X, '  ', r12, '  ', rX2, '  ', " & _
            "r0_0, '  ', r0_1, '  ', r0_2, '  ', r0_3, '  ', r0_4, '  ', " & _
            "r1_0, '  ', r1_1, '  ', r1_2, '  ', r1_3, '  ', r1_4, '  ', " & _
            "r2_0, '  ', r2_1, '  ', r2_2, '  ', r2_3, '  ', r2_4, '  ', " & _
            "r3_0, '  ', r3_1, '  ', r3_2, '  ', r3_3, '  ', r3_4, '  ', " & _
            "r4_0, '  ', r4_1, '  ', r4_2, '  ', r4_3, '  ', r4_4, '  ', rAltro " & _
            "From Quote A Left Join Schedine B On A.Giornata = B.Giornata And A.Anno = B.Anno And A.Partita = B.Partita " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & gg & " Order By Partita"
        GrP.ImpostaCampi(Sql, grdPartite)

        'Sql = "Select " & _
        '    "r1, '  ', rX, '  ', r2, '  ', r1X, '  ', r12, '  ', rX2, '  ', " & _
        '    "r0_0, '  ', r0_1, '  ', r0_2, '  ', r0_3, '  ', r0_4, '  ', " & _
        '    "r1_0, '  ', r1_1, '  ', r1_2, '  ', r1_3, '  ', r1_4, '  ', " & _
        '    "r2_0, '  ', r2_1, '  ', r2_2, '  ', r2_3, '  ', r2_4, '  ', " & _
        '    "r3_0, '  ', r3_1, '  ', r3_2, '  ', r3_3, '  ', r3_4, '  ', " & _
        '    "r4_0, '  ', r4_1, '  ', r4_2, '  ', r4_3, '  ', r4_4, '  ', rAltro " & _
        '    "From Quote A Left Join Schedine B On A.Giornata = B.Giornata And A.Anno = B.Anno And A.Partita = B.Partita " & _
        '    "Where A.Anno=" & DatiGioco.AnnoAttuale & " And A.Giornata=" & gg & " Order By A.Partita"
        'GrLQ.ImpostaCampi(Sql, grdListaQuote)

        GrLQ = Nothing
        GrP = Nothing
    End Sub

    Private Sub grdPartite_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPartite.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ImgCasa As ImageButton = DirectCast(e.Row.FindControl("imgCasa"), ImageButton)
            Dim ImgFuori As ImageButton = DirectCast(e.Row.FindControl("imgFuori"), ImageButton)
            Dim Db As New GestioneDB
            Dim gM As New GestioneMail

            If Db.LeggeImpostazioniDiBase() = True Then
                Dim ConnSQL As Object = Db.ApreDB()
                Dim Rec As Object = CreateObject("ADODB.Recordset")
                Dim Sql As String

                Dim Path As String

                Path = gM.RitornaImmagineSquadra(e.Row.Cells(2).Text, True)

                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgCasa.ImageUrl = Path
                Else
                    ImgCasa.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                Path = gM.RitornaImmagineSquadra(e.Row.Cells(4).Text, True)
                If File.Exists(Server.MapPath(Path)) = True Then
                    ImgFuori.ImageUrl = Path
                Else
                    ImgFuori.ImageUrl = "App_Themes/Standard/Images/Stemmi/Niente.png"
                End If

                ConnSQL.Close()
            End If

            Db = Nothing
        End If
    End Sub
End Class