Imports System.IO
Imports System.Text
Imports System.Management

Public Structure ModalitaDiScan
    Dim TipologiaScan As Integer
    Const SoloStruttura = 0
    Const Elimina = 1
End Structure

Public Class GestioneFilesDirectory
    Private barra As String = "\"

    Private DirectoryRilevate() As String
    Private FilesRilevati() As String
    Private QuantiFilesRilevati As Long
    Private QuanteDirRilevate As Long
    Private RootDir As String
    Private Eliminati As Boolean
    Private Percorso As String

    Public Const NonEliminareRoot As Boolean = False
    Public Const EliminaRoot As Boolean = True
    Public Const NonEliminareFiles As Boolean = False
    Public Const EliminaFiles As Boolean = True

    Private DimensioniArrayAttualeDir As Long
    Private DimensioniArrayAttualeFiles As Long

    Public Sub PrendeRoot(R As String)
        RootDir = R
    End Sub

    Public Function RitornaFilesRilevati() As String()
        Return FilesRilevati
    End Function

    Public Function RitornaDirectoryRilevate() As String()
        Return DirectoryRilevate
    End Function

    Public Function RitornaQuantiFilesRilevati() As Long
        Return QuantiFilesRilevati
    End Function

    Public Function RitornaQuanteDirectoryRilevate() As Long
        Return QuanteDirRilevate
    End Function

    Public Sub ImpostaPercorsoAttuale(sPercorso As String)
        Percorso = sPercorso
    End Sub

    Public Function TornaDimensioneFile(NomeFile As String) As Long
        Dim infoReader As System.IO.FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(NomeFile)
        Dim Dime As Long = infoReader.Length
        infoReader = Nothing

        Return Dime
    End Function

    Public Function NomeFileEsistente(NomeFile As String) As String
        Dim NomeFileDestinazione As String = NomeFile
        Dim gf As New GestioneFilesDirectory
        Dim Estensione As String = gf.TornaEstensioneFileDaPath(NomeFileDestinazione)
        NomeFileDestinazione = NomeFileDestinazione.Replace(Estensione, "")
        Dim Contatore As Integer = 1

        Do While File.Exists(NomeFileDestinazione & "_" & Format(Contatore, "0000") & Estensione) = True
            Contatore += 1
        Loop

        NomeFileDestinazione = NomeFileDestinazione & "_" & Format(Contatore, "0000") & Estensione
        gf = Nothing

        Return NomeFileDestinazione
    End Function

    Public Function EliminaFileFisico(NomeFileOrigine As String) As String
        Dim Ritorno As String = ""

        If NomeFileOrigine.Trim <> "" Then
            Try
                File.Delete(NomeFileOrigine)

                Do While (System.IO.File.Exists(NomeFileOrigine) = True)
                    Threading.Thread.Sleep(1)
                Loop
            Catch ex As Exception
                Ritorno = "ERRORE: " & ex.Message
            End Try
        End If

        Return Ritorno
    End Function

    Public Function CopiaFileFisico(NomeFileOrigine As String, NomeFileDestinazione As String, SovraScrittura As Boolean) As String
        Dim Ritorno As String = ""

        If NomeFileOrigine.Trim <> "" And NomeFileDestinazione.Trim <> "" And NomeFileOrigine.Trim.ToUpper <> NomeFileDestinazione.Trim.ToUpper Then
            If File.Exists(NomeFileDestinazione) = True Then
                If SovraScrittura = False Then
                    NomeFileDestinazione = NomeFileEsistente(NomeFileDestinazione)
                End If
            End If

            Try
                File.Copy(NomeFileOrigine, NomeFileDestinazione, True)

                Do Until (System.IO.File.Exists(NomeFileDestinazione))
                    Threading.Thread.Sleep(1)
                Loop

                Ritorno = TornaNomeFileDaPath(NomeFileDestinazione)
            Catch ex As Exception
                Ritorno = "ERRORE: " & ex.Message
            End Try
        End If

        Return Ritorno
    End Function

    Public Function TornaNomeFileDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "/" Or Mid(Percorso, i, 1) = barra Then
                Ritorno = Mid(Percorso, i + 1, Percorso.Length)
                Exit For
            End If
        Next

        Return Ritorno
    End Function

    Public Function TornaEstensioneFileDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "." Then
                Ritorno = Mid(Percorso, i, Percorso.Length)
                Exit For
            End If
        Next
        If Ritorno.Length > 5 Then
            Ritorno = ""
        End If

        Return Ritorno
    End Function

    Public Function TornaNomeDirectoryDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "/" Or Mid(Percorso, i, 1) = barra Then
                Ritorno = Mid(Percorso, 1, i - 1)
                Exit For
            End If
        Next

        Return Ritorno
    End Function

    Public Sub CreaAggiornaFile(NomeFile As String, Cosa As String)
        Try
            Dim path As String

            If Percorso <> "" Then
                path = Percorso & barra & NomeFile
            Else
                path = NomeFile
            End If

            path = path.Replace(barra & barra, barra)

            ' Create or overwrite the file.
            Dim fs As FileStream = File.Create(path)

            ' Add text to the file.
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(Cosa)
            fs.Write(info, 0, info.Length)
            fs.Close()
        Catch ex As Exception
            'Dim StringaPassaggio As String
            'Dim H As HttpApplication = HttpContext.Current.ApplicationInstance

            'StringaPassaggio = "?Errore=Errore CreaAggiornaFileVisMese: " & Err.Description.Replace(" ", "%20").Replace(vbCrLf, "")
            'StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("Nick")
            'StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            'H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try
    End Sub

    Private objReader As StreamReader

    Public Sub ApreFilePerLettura(NomeFile As String)
        objReader = New StreamReader(NomeFile)
    End Sub

    Public Function RitornaRiga() As String
        Return objReader.ReadLine()
    End Function

    Public Sub ChiudeFile()
        objReader.Close()
    End Sub

    Public Function LeggeFileIntero(NomeFile As String) As String
        Dim objReader As StreamReader = New StreamReader(NomeFile)
        Dim sLine As String = ""
        Dim Ritorno As String = ""

        Do
            sLine = objReader.ReadLine()
            Ritorno += sLine
        Loop Until sLine Is Nothing
        objReader.Close()

        Return Ritorno
    End Function

    Public Sub ScansionaDirectorySingola(Percorso As String, Optional Filtro As String = "", Optional lblAggiornamento As Label = Nothing, Optional SoloRoot As Boolean = False)
        Eliminati = False

        PulisceInfo()

        QuanteDirRilevate += 1
        DirectoryRilevate(QuanteDirRilevate) = Percorso

        lblAggiornamento.Text = "Lettura " & Percorso & " - Files rilevati: " & QuantiFilesRilevati
        Application.DoEvents()

        LeggeFilesDaDirectory(Percorso, Filtro)

        If SoloRoot = False Then
            LeggeTutto(Percorso, Filtro, lblAggiornamento)
        End If
    End Sub

    Dim Conta As Integer

    Private Sub LeggeTutto(Percorso As String, Filtro As String, lblAggiornamento As Label)
        Try
            Dim di As New IO.DirectoryInfo(Percorso)
            Dim diar1 As IO.DirectoryInfo() = di.GetDirectories
            Dim dra As IO.DirectoryInfo

            For Each dra In diar1
                If lblAggiornamento Is Nothing = False Then
                    Conta += 1
                    If Conta = 2 Then
                        Conta = 0
                        lblAggiornamento.Text = "Lettura " & Percorso & " - Files rilevati: " & QuantiFilesRilevati
                        Application.DoEvents()
                    End If
                End If

                QuanteDirRilevate += 1
                If QuanteDirRilevate > DimensioniArrayAttualeDir Then
                    DimensioniArrayAttualeDir += 10000
                    ReDim Preserve DirectoryRilevate(DimensioniArrayAttualeDir)
                End If
                DirectoryRilevate(QuanteDirRilevate) = dra.FullName

                LeggeFilesDaDirectory(dra.FullName, Filtro)

                LeggeTutto(dra.FullName, Filtro, lblAggiornamento)
            Next
        Catch ex As Exception
            Stop
        End Try
    End Sub

    Public Sub PulisceInfo()
        Erase FilesRilevati
        QuantiFilesRilevati = 0
        Erase DirectoryRilevate
        QuanteDirRilevate = 0

        DimensioniArrayAttualeDir = 10000
        DimensioniArrayAttualeFiles = 10000

        ReDim DirectoryRilevate(DimensioniArrayAttualeDir)
        ReDim FilesRilevati(DimensioniArrayAttualeFiles)
    End Sub

    Public Function RitornaEliminati() As Boolean
        Return Eliminati
    End Function

    Public Sub LeggeFilesDaDirectory(Percorso As String, Optional Filtro As String = "")
        Dim di As New IO.DirectoryInfo(Percorso)

        Dim fi As New IO.DirectoryInfo(Percorso)
        Dim fiar1 As IO.FileInfo() = di.GetFiles
        Dim fra As IO.FileInfo
        Dim Ok As Boolean = True
        Dim Filtri() As String = Filtro.Split(";")

        For Each fra In fiar1
            QuantiFilesRilevati += 1
            If QuantiFilesRilevati > DimensioniArrayAttualeFiles Then
                DimensioniArrayAttualeFiles += 10000
                ReDim Preserve FilesRilevati(DimensioniArrayAttualeFiles)
            End If
            FilesRilevati(QuantiFilesRilevati) = fra.FullName
        Next
    End Sub

    Public Sub CreaDirectoryDaPercorso(Percorso As String)
        Dim Ritorno As String = Percorso

        For i As Integer = 1 To Ritorno.Length
            If Mid(Ritorno, i, 1) = barra Then
                On Error Resume Next
                MkDir(Mid(Ritorno, 1, i))
                On Error GoTo 0
            End If
        Next
    End Sub

    Public Function Ordina(Filetti() As String) As String()
        If Filetti Is Nothing Then
            Return {}
            Exit Function
        End If

        Dim Appoggio() As String = Filetti
        Dim Appo As String

        For i As Integer = 1 To Appoggio.Count - 1
            For k As Integer = i + 1 To Appoggio.Count - 1
                If Appoggio(i).ToUpper.Trim > Appoggio(k).ToUpper.Trim Then
                    Appo = Appoggio(i)
                    Appoggio(i) = Appoggio(k)
                    Appoggio(k) = Appo
                End If
            Next
        Next

        Return Appoggio
    End Function

    Public Sub EliminaAlberoDirectory(Percorso As String, EliminaRoot As Boolean, EliminaFiles As Boolean)
        ScansionaDirectorySingola(Percorso, 0)

        DirectoryRilevate = Ordina(DirectoryRilevate)

        If EliminaFiles = True Then
            FilesRilevati = Ordina(FilesRilevati)

            For i As Integer = FilesRilevati.Length - 1 To 1 Step -1
                Try
                    EliminaFileFisico(FilesRilevati(i))
                Catch ex As Exception

                End Try
            Next
        End If

        For i As Integer = DirectoryRilevate.Length - 1 To 1 Step -1
            Try
                RmDir(RitornaDirectoryRilevate(i))
            Catch ex As Exception

            End Try
        Next

        If EliminaRoot = True Then
            Try
                RmDir(Percorso)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Function TornaDataDiCreazione(NomeFile As String) As Date
        Dim info As New FileInfo(NomeFile)
        Return info.CreationTime
    End Function

    Public Function TornaDataDiUltimaModifica(NomeFile As String) As Date
        Dim info As New FileInfo(NomeFile)
        Return info.LastWriteTime
    End Function

    Public Function TornaDataUltimoAccesso(NomeFile As String) As Date
        Dim info As New FileInfo(NomeFile)
        Return info.LastAccessTime
    End Function
End Class
