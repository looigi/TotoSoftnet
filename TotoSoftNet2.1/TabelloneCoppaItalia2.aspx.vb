Imports System.IO

Public Class TabelloneCoppaItalia2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            DisegnaTabellone(3, "CoppaItalia", "Coppa Italia", "PartiteCoppaItaliaTurni", divContenuto)
            ControllaVincitore()
        End If
    End Sub

    Private Sub ControllaVincitore()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select A.GiocCasa, B.Giocatore From PartiteCoppaItaliaTurni A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                divVincente.Visible = False
            Else
                divVincente.Visible = True
                lblVincitore.Text = Rec("Giocatore").Value
                imgVincente.ImageUrl = RitornaImmagine(Server.MapPath("."), Rec("Giocatore").Value)
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing
    End Sub
End Class