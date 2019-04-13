Imports System.IO
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Drawing.Image

Public Class GestioneImmagini
    Public Sub ConverteImmaginInBN(Path As String, Path2 As String)
        Dim img As Bitmap
        Dim ImmaginePiccola As Image
        Dim ImmaginePiccola2 As Image
        Dim jgpEncoder As Imaging.ImageCodecInfo
        Dim myEncoder As System.Drawing.Imaging.Encoder
        Dim myEncoderParameters As New Imaging.EncoderParameters(1)

        img = New Bitmap(Path)

        ImmaginePiccola = New Bitmap(img)

        img = Nothing

        ImmaginePiccola = Converte(ImmaginePiccola)

        jgpEncoder = GetEncoder(Imaging.ImageFormat.Jpeg)
        myEncoder = System.Drawing.Imaging.Encoder.Quality
        Dim myEncoderParameter As New Imaging.EncoderParameter(myEncoder, 75)
        myEncoderParameters.Param(0) = myEncoderParameter

        ImmaginePiccola.Save(Path2, jgpEncoder, myEncoderParameters)

        ImmaginePiccola = Nothing
        ImmaginePiccola2 = Nothing
        jgpEncoder = Nothing
        myEncoderParameter = Nothing
    End Sub

    Public Sub Ridimensiona(Path As String, Path2 As String, Larghezza As Integer, Altezza As Integer)
        Dim myEncoder As System.Drawing.Imaging.Encoder
        Dim myEncoderParameters As New Imaging.EncoderParameters(1)
        Dim img2 As Bitmap
        Dim ImmaginePiccola22 As Image
        Dim jgpEncoder2 As Imaging.ImageCodecInfo
        Dim myEncoder2 As System.Drawing.Imaging.Encoder
        Dim myEncoderParameters2 As New Imaging.EncoderParameters(1)

        img2 = New Bitmap(Path)
        ImmaginePiccola22 = New Bitmap(img2, Val(Larghezza), Val(Altezza))
        img2.Dispose()
        img2 = Nothing

        myEncoder = System.Drawing.Imaging.Encoder.Quality
        jgpEncoder2 = GetEncoder(Imaging.ImageFormat.Jpeg)
        myEncoder2 = System.Drawing.Imaging.Encoder.Quality
        Dim myEncoderParameter2 As New Imaging.EncoderParameter(myEncoder, 75)
        myEncoderParameters2.Param(0) = myEncoderParameter2
        ImmaginePiccola22.Save(Path2, jgpEncoder2, myEncoderParameters2)

        ImmaginePiccola22.Dispose()
        ImmaginePiccola22 = Nothing
        jgpEncoder2 = Nothing
        myEncoderParameter2 = Nothing
        myEncoder = Nothing
    End Sub

    Private Function Converte(ByVal inputImage As Image) As Image
        Dim outputBitmap As Bitmap = New Bitmap(inputImage.Width, inputImage.Height)
        Dim X As Long
        Dim Y As Long
        Dim currentBWColor As Color

        For X = 0 To outputBitmap.Width - 1
            For Y = 0 To outputBitmap.Height - 1
                currentBWColor = ConverteColore(DirectCast(inputImage, Bitmap).GetPixel(X, Y))
                outputBitmap.SetPixel(X, Y, currentBWColor)
            Next
        Next

        inputImage = Nothing
        Return outputBitmap
    End Function

    Private Function ConverteColore(ByVal InputColor As Color)
        'Dim eyeGrayScale As Integer = (InputColor.R * 0.3 + InputColor.G * 0.59 + InputColor.B * 0.11)
        Dim Rosso As Single = InputColor.R * 0.3
        Dim Verde As Single = InputColor.G * 0.59
        Dim Blu As Single = InputColor.B * 0.41
        Dim eyeGrayScale As Integer = (Rosso + Verde + Blu) ' * 1.7
        If eyeGrayScale > 255 Then eyeGrayScale = 255
        Dim outputColor As Color = Color.FromArgb(eyeGrayScale, eyeGrayScale, eyeGrayScale)

        Return outputColor
    End Function

    Private Function ConverteChiara(ByVal inputImage As Image) As Image
        Dim outputBitmap As Bitmap = New Bitmap(inputImage.Width, inputImage.Height)
        Dim X As Long
        Dim Y As Long
        Dim currentBWColor As Color

        For X = 0 To outputBitmap.Width - 1
            For Y = 0 To outputBitmap.Height - 1
                currentBWColor = ConverteColoreChiaro(DirectCast(inputImage, Bitmap).GetPixel(X, Y))
                outputBitmap.SetPixel(X, Y, currentBWColor)
            Next
        Next

        inputImage = Nothing
        Return outputBitmap
    End Function

    Private Function ConverteColoreChiaro(ByVal InputColor As Color)
        'Dim eyeGrayScale As Integer = (InputColor.R * 0.3 + InputColor.G * 0.59 + InputColor.B * 0.11)
        Dim Rosso As Single = InputColor.R * 0.49999999999999994
        Dim Verde As Single = InputColor.G * 0.49000000000000005
        Dim Blu As Single = InputColor.B * 0.49999999999999595
        Dim eyeGrayScale As Integer = (Rosso + Verde + Blu) '* 4.1000000000000005
        If eyeGrayScale > 250 Then eyeGrayScale = 250
        If eyeGrayScale < 185 Then eyeGrayScale = 185
        Dim outputColor As Color = Color.FromArgb(eyeGrayScale, eyeGrayScale, eyeGrayScale)

        Return outputColor
    End Function

    Private Function GetEncoder(ByVal format As Imaging.ImageFormat) As Imaging.ImageCodecInfo

        Dim codecs As Imaging.ImageCodecInfo() = Imaging.ImageCodecInfo.GetImageDecoders()

        Dim codec As Imaging.ImageCodecInfo
        For Each codec In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next codec
        Return Nothing

    End Function

    Public Sub DisegnaStatisticheSondaggi(Giocatori As Integer, Votanti As Integer, vRisposte() As Integer, sRisposte() As String, Percorso As String)
        Dim originalX As Integer = 300
        Dim originalY As Integer = 300
        Dim Risposte() As Integer = vRisposte
        Dim RisposteStringhe() As String = sRisposte

        Dim Massimo As Integer
        Dim Appoggio As Integer
        Dim AppoggioStringa As String

        For i As Integer = 0 To UBound(Risposte)
            For k As Integer = i + 1 To UBound(Risposte)
                If Risposte(i) < Risposte(k) Then
                    Appoggio = Risposte(i)
                    Risposte(i) = Risposte(k)
                    Risposte(k) = Appoggio

                    AppoggioStringa = RisposteStringhe(i)
                    RisposteStringhe(i) = RisposteStringhe(k)
                    RisposteStringhe(k) = AppoggioStringa
                End If
            Next
        Next
        Massimo = Risposte(0)

        Dim origRisposte() As Integer = Risposte
        Dim altRisposte(2) As Integer

        For i As Integer = 0 To UBound(Risposte)
            If Massimo > 0 Then
                altRisposte(i) = (originalY - 100) * (Risposte(i) / Massimo)
            Else
                altRisposte(i) = 0
            End If
            If altRisposte(i) = 0 Then altRisposte(i) = 3
        Next

        Dim thumb As New Bitmap(originalX, originalY)
        Dim g As Graphics = Graphics.FromImage(thumb)

        Dim font As Font = New Font("Arial", 13, FontStyle.Regular)

        Dim coloreNero As Pen = New Pen(Color.Black)
        Dim Dove As Point

        g.DrawLine(coloreNero, 25, 0, 25, originalY - 95)
        g.DrawLine(coloreNero, 20, originalY - 100, originalX - 5, originalY - 100)

        Dove.X = 0
        Dove.Y = 0
        g.DrawString(Massimo, font, Brushes.Blue, Dove)

        Dove.X = 0
        Dove.Y = originalY - 110
        g.DrawString("0", font, Brushes.Blue, Dove)

        Dim rettangolo As Rectangle

        rettangolo.X = 50
        rettangolo.Y = ((originalY - 100) - altRisposte(0))
        rettangolo.Width = 60
        rettangolo.Height = altRisposte(0)

        g.FillRectangle(Brushes.Red, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        rettangolo.X = 140
        rettangolo.Y = ((originalY - 100) - altRisposte(1))
        rettangolo.Width = 60
        rettangolo.Height = altRisposte(1)

        g.FillRectangle(Brushes.Green, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        rettangolo.X = 230
        rettangolo.Y = ((originalY - 100) - altRisposte(2))
        rettangolo.Width = 60
        rettangolo.Height = altRisposte(2)

        g.FillRectangle(Brushes.Yellow, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        ' Legenda 
        rettangolo.X = 25
        rettangolo.Y = originalY - 90
        rettangolo.Width = 30
        rettangolo.Height = 15

        g.FillRectangle(Brushes.Red, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        Dove.X = 60
        Dove.Y = originalY - 93
        g.DrawString(RisposteStringhe(0) & ": voti " & origRisposte(0), font, Brushes.Black, Dove)

        rettangolo.X = 25
        rettangolo.Y = originalY - 70
        rettangolo.Width = 30
        rettangolo.Height = 15

        g.FillRectangle(Brushes.Green, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        Dove.X = 60
        Dove.Y = originalY - 73
        g.DrawString(RisposteStringhe(1) & ": voti " & origRisposte(1), font, Brushes.Black, Dove)

        rettangolo.X = 25
        rettangolo.Y = originalY - 50
        rettangolo.Width = 30
        rettangolo.Height = 15

        g.FillRectangle(Brushes.Yellow, rettangolo)
        g.DrawRectangle(coloreNero, rettangolo)

        Dove.X = 60
        Dove.Y = originalY - 53
        g.DrawString(RisposteStringhe(2) & ": voti " & origRisposte(2), font, Brushes.Black, Dove)
        ' Legenda 

        Dim Perc As Integer

        If Giocatori > 0 Then
            Perc = (Votanti / Giocatori) * 100
        Else
            Perc = 0
        End If

        Dove.X = 55
        Dove.Y = originalY - 20
        g.DrawString("Giocatori: " & Giocatori & " - Votanti: " & Votanti & " (" & Perc & "%)", font, Brushes.Black, Dove)

        thumb.Save(Percorso, System.Drawing.Imaging.ImageFormat.Png)
        thumb.Dispose()
        g.Dispose()
    End Sub

    Public Sub RidimensionaEArrotondaIcona(ByVal PercorsoImmagine As String)
        Dim bm As Bitmap
        Dim originalX As Integer
        Dim originalY As Integer

        'carica immagine originale
        bm = New Bitmap(PercorsoImmagine)

        originalX = bm.Width
        originalY = bm.Height

        Dim thumb As New Bitmap(originalX, originalY)
        Dim g As Graphics = Graphics.FromImage(thumb)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.DrawImage(bm, New Rectangle(0, 0, originalX, originalY), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)

        Dim r As System.Drawing.Rectangle
        Dim s As System.Drawing.Size
        Dim coloreRosso As Pen = New Pen(Color.Red)
        coloreRosso.Width = 3

        For dimeX = originalX - 15 To originalX * 2
            r.X = (originalX / 2) - (dimeX / 2)
            r.Y = (originalY / 2) - (dimeX / 2)
            s.Width = dimeX
            s.Height = dimeX
            r.Size = s
            g.DrawEllipse(coloreRosso, r)
        Next

        Dim InizioY As Integer = -1
        Dim InizioX As Integer = -1
        Dim FineY As Integer = -1
        Dim FineX As Integer = -1
        Dim pixelColor As Color

        For i As Integer = 1 To originalX - 1
            For k As Integer = 1 To originalY - 1
                pixelColor = thumb.GetPixel(i, k)
                If pixelColor.ToArgb <> Color.Red.ToArgb Then
                    InizioX = i
                    'g.DrawLine(Pens.Black, i, 0, i, originalY)
                    Exit For
                End If
            Next
            If InizioX <> -1 Then
                Exit For
            End If
        Next

        For i As Integer = originalX - 1 To 1 Step -1
            For k As Integer = originalY - 1 To 1 Step -1
                pixelColor = thumb.GetPixel(i, k)
                If pixelColor.ToArgb <> Color.Red.ToArgb Then
                    FineX = i
                    'g.DrawLine(Pens.Black, i, 0, i, originalY)
                    Exit For
                End If
            Next
            If FineX <> -1 Then
                Exit For
            End If
        Next

        For i As Integer = 1 To originalY - 1
            For k As Integer = 1 To originalX - 1
                pixelColor = thumb.GetPixel(k, i)
                If pixelColor.ToArgb <> Color.Red.ToArgb Then
                    InizioY = i
                    'g.DrawLine(Pens.Black, 0, i, originalX, i)
                    Exit For
                End If
            Next
            If InizioY <> -1 Then
                Exit For
            End If
        Next

        For i As Integer = originalY - 1 To 1 Step -1
            For k As Integer = originalX - 1 To 1 Step -1
                pixelColor = thumb.GetPixel(k, i)
                If pixelColor.ToArgb <> Color.Red.ToArgb Then
                    FineY = i
                    'g.DrawLine(Pens.Black, 0, i, originalX, i)
                    Exit For
                End If
            Next
            If FineY <> -1 Then
                Exit For
            End If
        Next

        Dim nDimeX As Integer = FineX - InizioX
        Dim nDimeY As Integer = FineY - InizioY

        r.X = InizioX - 1
        r.Y = InizioY - 1
        r.Width = nDimeX + 1
        r.Height = nDimeY + 1

        Dim bmpAppoggio As Bitmap = New Bitmap(nDimeX, nDimeY)
        Dim g2 As Graphics = Graphics.FromImage(bmpAppoggio)

        g2.DrawImage(thumb, 0, 0, r, GraphicsUnit.Pixel)

        thumb = bmpAppoggio
        g2.Dispose()

        g.Dispose()

        thumb.MakeTransparent(Color.Red)

        thumb.Save(percorsoImmagine & ".tsz", System.Drawing.Imaging.ImageFormat.Png)
        bm.Dispose()
        bmpAppoggio.Dispose()
        thumb.Dispose()

        Try
            Kill(percorsoImmagine)
        Catch ex As Exception

        End Try

        thumb = Nothing
        g = Nothing
        g2 = Nothing
        bm = Nothing
        bmpAppoggio = Nothing

        Rename(percorsoImmagine & ".tsz", percorsoImmagine)
    End Sub

    Public Sub RuotaFoto(Nome As String)
        Dim bm As Bitmap
        Dim originalX As Integer
        Dim originalY As Integer

        bm = New Bitmap(Nome)

        originalX = 100
        originalY = 100

        Dim thumb As New Bitmap(originalX + 30, originalY + 25)
        Dim g As Graphics = Graphics.FromImage(thumb)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.RotateTransform(-12.0F)

        Dim fillRect As New Rectangle(0, 0, originalX + 30, originalY + 25)
        Dim fillRegion As New [Region](fillRect)

        g.FillRegion(Brushes.Red, fillRegion)

        g.DrawImage(bm, New Rectangle(5, 23, originalX, originalY), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
        g.DrawRectangle(Pens.DarkGray, New Rectangle(5, 23, originalX, originalY))
        g.DrawRectangle(Pens.DarkGray, New Rectangle(4, 22, originalX + 1, originalY + 1))
        thumb.MakeTransparent(Color.Red)

        thumb.Save(Nome & ".ruo", System.Drawing.Imaging.ImageFormat.Png)

        bm.Dispose()
        thumb.Dispose()

        Kill(Nome)
        Rename(Nome & ".ruo", Nome)
    End Sub
End Class
