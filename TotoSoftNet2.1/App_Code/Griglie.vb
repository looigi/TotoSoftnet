Imports System.Data

Public Class Griglie
    Private Colonne() As DataColumn
    Private riga As DataRow
    Private dttTabella As New DataTable()
    Private Db As New GestioneDB
    Private ConnSQL As Object
    Private Rec As Object = CreateObject("ADODB.Recordset")
    Private QuantiCampi As Integer
    Private AggiunteRighe As Boolean = False

    Public Sub ImpostaCampi(Sql As String, Griglia As GridView, Optional NonEseguireSubito As Boolean = False)
        Dim q As Integer = 0
        Dim nCampi() As String = {}
        Dim Appo As String

        For i As Integer = 0 To Griglia.Columns.Count - 1
            Try
                Appo = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
            Catch ex As Exception
                Appo = ""
            End Try

            If Appo <> "" Then
                redim Preserve nCampi(q)
                nCampi(q) = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
                q += 1
            End If
        Next
        QuantiCampi = q - 1

        redim Preserve Colonne(UBound(nCampi))
        QuantiCampi = UBound(nCampi)

        For i As Integer = 0 To QuantiCampi
            Colonne(i) = New DataColumn(nCampi(i))

            dttTabella.Columns.Add(Colonne(i))
        Next

        ApreDB()
        ImpostaValori(Sql)
        If NonEseguireSubito = False Then
            VisualizzaValori(Griglia)
            ChiudeDB()
        Else
            AggiunteRighe = True
        End If
    End Sub

    Private Sub ApreDB()
        If Db.LeggeImpostazioniDiBase() = True Then
            ConnSQL = Db.ApreDB()
        End If
    End Sub

    Private Sub ChiudeDB()
        riga = Nothing
        dttTabella = Nothing

        ConnSQL.Close()
        Db = Nothing
    End Sub

    Public Sub AggiungeValori(Sql As String)
        Dim Campo As String

        Rec = Db.LeggeQuery(ConnSQL, Sql)

        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi
                Campo = "" & Rec(i).Value

                If IsDate(Campo) = True And Campo.Length > 6 Then
                    Dim dCampo As Date = Campo

                    Campo = ConverteData(dCampo)
                End If

                riga(i) = Campo
            Next
            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop

        Rec.Close()
    End Sub

    Private Sub ImpostaValori(Sql As String)
        Dim Campo As String

        Rec = Db.LeggeQuery(ConnSQL, Sql)

        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi ' - 1
                Campo = "" & Rec(i).Value

                If IsDate(Campo) = True And Campo.Length > 6 Then
                    Dim dCampo As Date = Campo

                    Campo = ConverteData(dCampo)
                End If

                riga(i) = Campo
            Next
            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop

        Rec.Close()
    End Sub

    Public Sub VisualizzaValori(grdView As GridView)
        grdView.DataSource = dttTabella
        grdView.DataBind()

        If AggiunteRighe = True Then
            ChiudeDB()
        End If
    End Sub
End Class
