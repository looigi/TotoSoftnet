Public Class Regolamento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If "" & Session("Nick") <> "" Then
        '    cmdIndietro.Visible = False
        'Else
        '    cmdIndietro.Visible = True
        'End If
    End Sub

    'Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
    '    Response.Redirect("Default.aspx")
    'End Sub
End Class