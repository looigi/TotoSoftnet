Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser

Public Class GestionePDF
    Public Sub ConvertePDF(fileName As String, fileDest As String)
        Dim Ritorno As String = LeggePDF(fileName)
        Dim gf As New GestioneFilesDirectory
        gf.CreaAggiornaFile(fileDest, Ritorno)
        gf = Nothing
    End Sub

    Public Function LeggePDF(fileName As String) As String
        If Not File.Exists(fileName) Then
            Throw New FileNotFoundException("fileName")
        End If
        Using reader As New PdfReader(fileName)
            Dim sb As New StringBuilder()

            Dim strategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()
            For page As Integer = 0 To reader.NumberOfPages - 1
                Dim text As String = PdfTextExtractor.GetTextFromPage(reader, page + 1, strategy)

                text = text.Replace(Chr(10), "§§§")
                text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(text))) & Chr(13) & Chr(10)
                If page > 0 Then
                    text = Mid(text, sb.ToString.Length, text.Length)
                End If

                sb.Append(text)
            Next

            Return sb.ToString()
        End Using
    End Function

End Class
