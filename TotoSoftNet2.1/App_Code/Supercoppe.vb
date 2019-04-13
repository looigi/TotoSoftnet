Public Class Supercoppe
    Public Function PrendeSquadreSuperCoppaItaliana() As String
        Dim Ritorno As String = ""
        Dim Db As New GestioneDB
        Dim gm As New GestioneMail

        Ritorno = "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineSupercoppaItaliana & " Qualificate Supercoppa Italiana " & gm.RitornaImmagineSupercoppaItaliana & gm.ChiudeTesto() & "<br /><br />"

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim sc As New StringheClassifiche
            Dim Sql As String

            Sql = "Delete From PartiteSuperCoppaItalianaTurni Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            ' Prende vincitore coppa Italia
            Dim codVincenteCoppaItalia As String = ControllaVincitoreCoppaItalia(Db, ConnSQL)

            ' Prende primo in classifica se non è uguale al vincente della Coppa Italia
            Dim codVincenteScontriDiretti As String = ""

            Sql = "Select A.CodGiocatore, Punti From AppoScontriDiretti A " & _
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
                "Where A.Anno=" & DatiGioco.AnnoAttuale & " And B.Cancellato='N' " & _
                "Order By Punti Desc, GFatti Desc, GSubiti, Differenza Desc, Media Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            Do Until Rec.Eof
                If Rec("CodGiocatore").Value <> codVincenteCoppaItalia Then
                    codVincenteScontriDiretti = Rec("CodGiocatore").Value
                    Exit Do
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            If codVincenteScontriDiretti <> "" And codVincenteCoppaItalia <> "" Then
                Sql = "Insert Into PartiteSuperCoppaItalianaTurni Values (" _
                    & " " & DatiGioco.AnnoAttuale & ", " _
                    & "38, " _
                    & "1, " _
                    & " " & codVincenteScontriDiretti & ", " _
                    & " " & codVincenteCoppaItalia & ", " _
                    & "'', " _
                    & "'', " _
                    & "null " _
                    & ")"
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codVincenteScontriDiretti
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Dim NomeVCampionato As String = Rec("Giocatore").Value
                Rec.Close()

                Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codVincenteCoppaItalia
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Dim NomeVCItalia As String = Rec("Giocatore").Value
                Rec.Close()

                Ritorno += gm.ApreTesto & "Campionato: " & NomeVCampionato & "<br />"
                Ritorno += gm.ApreTesto & "Coppa Italia: " & NomeVCItalia & "<br />"

                Sql = "Delete From EventiSuperCoppaItalianaTurni Where Anno=" & DatiGioco.AnnoAttuale
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Insert Into EventiSuperCoppaItalianaTurni Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    "38, " & _
                    "1 " & _
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            End If

            ConnSQL.Close()
        End If

        Db = Nothing
        gm = Nothing

        Return Ritorno
    End Function

    Private Function ControllaVincitoreCoppaItalia(Db As GestioneDB, ConnSql As Object) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Vincente As String = ""
        Dim Sql As String

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteCoppaItaliaTurni A " _
            & "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = True Then
            Vincente = ""
        Else
            Vincente = Rec("GiocCasa").Value
        End If
        Rec.Close()

        Return Vincente
    End Function

    Private Function ControllaVincitoreEuropaLeague(Db As GestioneDB, ConnSql As Object) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Vincente As String = ""

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteEuropaLeagueTurni A " _
            & "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = True Then
            Vincente = ""
        Else
            Vincente = Rec("GiocCasa").Value
        End If
        Rec.Close()

        Return Vincente
    End Function

    Private Function ControllaVincitoreChampions(Db As GestioneDB, ConnSql As Object) As String
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim Vincente As String = ""

        Sql = "Select A.GiocCasa, B.Giocatore From PartiteChampionsTurni A " _
            & "Left Join Giocatori B On A.Anno=B.Anno And A.GiocCasa=B.CodGiocatore " _
            & "Where A.Anno=" & DatiGioco.AnnoAttuale & " And GiocFuori=-1"
        Rec = Db.LeggeQuery(ConnSql, Sql)
        If Rec.Eof = True Then
            Vincente = ""
        Else
            Vincente = Rec("GiocCasa").Value
        End If
        Rec.Close()

        Return Vincente
    End Function

    Public Function PrendeSquadreSuperCoppaEuropea() As String
        Dim Ritorno As String = ""
        Dim Db As New GestioneDB
        Dim gm As New GestioneMail

        Ritorno = "<hr />" & gm.ApreTestoTitolo() & gm.RitornaImmagineSupercoppaEuropea & " Qualificate Supercoppa Europea " & gm.RitornaImmagineSupercoppaEuropea & gm.ChiudeTesto() & "<br /><br />"

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim sc As New StringheClassifiche
            Dim Sql As String

            Sql = "Delete From PartiteSuperCoppaEuropeaTurni Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            ' Prende vincitore coppa Campioni
            Dim codVincenteCoppaCampioni As String = ControllaVincitoreChampions(Db, ConnSQL)

            ' Prende vinctiore Europa League
            Dim codVincenteEuropaLeague As String = ControllaVincitoreEuropaLeague(Db, ConnSQL)


            If codVincenteEuropaLeague <> "" And codVincenteCoppaCampioni <> "" Then
                Sql = "Insert Into PartiteSuperCoppaEuropeaTurni Values (" _
                    & " " & DatiGioco.AnnoAttuale & ", " _
                    & "38, " _
                    & "1, " _
                    & " " & codVincenteEuropaLeague & ", " _
                    & " " & codVincenteCoppaCampioni & ", " _
                    & "'', " _
                    & "'', " _
                    & "null " _
                    & ")"
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codVincenteEuropaLeague
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Dim NomeVChampions As String = Rec("Giocatore").Value
                Rec.Close

                Sql = "Select * From Giocatori Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & codVincenteCoppaCampioni
                Rec = Db.LeggeQuery(ConnSQL, Sql)
                Dim NomeVEuropaLeague As String = Rec("Giocatore").Value
                Rec.Close

                Ritorno += gm.ApreTesto & "Champion's League: " & NomeVChampions & "<br />"
                Ritorno += gm.ApreTesto & "Europa League: " & NomeVEuropaLeague & "<br />"

                Sql = "Delete From EventiSuperCoppaEuropeaTurni Where Anno=" & DatiGioco.AnnoAttuale & " And Giornata=38"
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Insert Into EventiSuperCoppaEuropeaTurni Values (" & _
                    " " & DatiGioco.AnnoAttuale & ", " & _
                    "38, " & _
                    "1 " & _
                    ")"
                Db.EsegueSql(ConnSQL, Sql)
            End If

            ConnSQL.Close
        End If

        Db = Nothing
        gm = Nothing

        Return Ritorno
    End Function
End Class
