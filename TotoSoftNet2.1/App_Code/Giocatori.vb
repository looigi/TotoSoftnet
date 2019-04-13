Public Class Giocatori
    Public Function TornaIdGiocatore(Chi As String) As Integer
        Dim Db As New GestioneDB
        Dim id As Integer = -1

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Upper(Ltrim(Rtrim(Giocatore)))='" & SistemaTestoPerDB(Chi.ToUpper.Trim) & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                id = Rec("CodGiocatore").value
            Else
                id = -1
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return id
    End Function

    Public Function TornaIdGiocatoreDaMail(Chi As String) As Integer
        Dim Db As New GestioneDB
        Dim id As Integer = -1

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Upper(Ltrim(Rtrim(EMail)))='" & SistemaTestoPerDB(Chi.ToUpper.Trim) & "'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                id = Rec("CodGiocatore").value
            Else
                id = -1
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return id
    End Function

    Public Function TornaNickGiocatore(id As Integer) As String
        Dim Db As New GestioneDB
        Dim Nome As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Nome = Rec("Giocatore").value
            Else
                Nome = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Nome
    End Function

    Public Function TornaNomeGiocatore(id As Integer) As String
        Dim Db As New GestioneDB
        Dim Nome As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Nome = Rec("Nome").value & " " & Rec("Cognome").Value
            Else
                Nome = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Nome
    End Function

    Public Function TornaDettaglioGiocatore(id As String) As String
        Dim Db As New GestioneDB
        Dim Ritorno As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From DettaglioGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Ritorno = Rec("Vittorie").Value & ";" & Rec("SecondiPosti").Value & ";" & Rec("UltimiPosti").Value & ";" & Rec("NumeroTappi").Value & ";"
            Else
                Ritorno = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Public Function PrendeNumeroGiocatori() As Integer
        Dim Db As New GestioneDB
        Dim Ritorno As Integer = -1

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Count(*) From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Cancellato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Ritorno = 0
            Else
                Ritorno = Rec(0).Value
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Public Function PrendePercentualiCoppe(NumGiocatori As Integer) As Object
        Dim Db As New GestioneDB
        Dim Ritorno As Perc

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Max(Numgiocatori) From PercCoppe"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim Massimo As Integer = Rec(0).Value
            Rec.Close()

            Sql = "Select Min(Numgiocatori) From PercCoppe"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Dim Minimo As Integer = Rec(0).Value
            Rec.Close()

            If NumGiocatori > Massimo Then
                NumGiocatori = Massimo
            End If

            Dim Ok As Boolean = True

            If NumGiocatori >= Minimo Then
                Sql = "Select * From PercCoppe Where NumGiocatori=" & NumGiocatori
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                If Rec(0).Value Is DBNull.Value = True Then
                    Ok = False
                Else
                    Ritorno.PassaggioCL = Rec("PassCL").Value
                    Ritorno.QuantiChampions = Rec("CL").Value
                    Ritorno.PassaggioEL = Rec("PassEL").Value
                    Ritorno.QuantiEuropaLeague = Rec("EL").Value
                    Ritorno.PassaggioIT = Rec("IT").Value
                    Ritorno.QuantiIntertoto = Rec("PassIT").Value
                    Ritorno.QuantiDerelitti = Rec("Der").Value
                End If
                Rec.Close()
            Else
                Ok = False
            End If

            If Ok = False Then
                Ritorno.PassaggioCL = -1
                Ritorno.PassaggioEL = -1
                Ritorno.PassaggioIT = -1
                Ritorno.QuantiChampions = -1
                Ritorno.QuantiEuropaLeague = -1
                Ritorno.QuantiIntertoto = -1
                Ritorno.QuantiDerelitti = -1
            End If

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Ritorno
    End Function

    Public Function TornaMailGiocatore(id As Integer) As String
        Dim Db As New GestioneDB
        Dim EMail As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                EMail = "" & Rec("EMail").value
            Else
                EMail = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return EMail
    End Function

    Public Function TornaPasswordGiocatore(id As Integer) As String
        Dim Db As New GestioneDB
        Dim Password As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Password = DecriptaPassword("" & Rec("Password").value)
            Else
                Password = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Password
    End Function

    Public Function TornaMottoGiocatore(id As Integer) As String
        Dim Db As New GestioneDB
        Dim Motto As String = ""

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & id
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                Motto = "" & Rec("Testo").value
            Else
                Motto = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Db = Nothing

        Return Motto
    End Function
End Class
