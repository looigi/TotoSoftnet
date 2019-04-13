Imports System.IO

Public Class Registranti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CreaTabella()
        End If
    End Sub

    Private Sub CreaTabella()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Giocatore,Nome,Cognome,EMail From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " Order By Giocatore"
        Gr.ImpostaCampi(Sql, grdUtenti)
        Gr = Nothing
    End Sub

    Protected Sub AggiungeGiocatore(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(0).Text

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = Server.CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idGiocatore As Integer

            Sql = "Select Max(CodGiocatore)+1 From Giocatori Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                idGiocatore = 1
            Else
                idGiocatore = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select * From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & SistemaTestoPerDB(Utente) & "'"

            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = True Then
                Dim Messaggi() As String = {"Problemi nella registrazione dell'account"}
                VisualizzaMessaggioInPopup(Messaggi, Me)
                Rec.Close()
                ConnSQL.Close()
                Db = Nothing
                Exit Sub
            End If

            Dim Nome As String = MetteMaiuscole(Rec("Nome").Value)
            Dim Cognome As String = MetteMaiuscole(Rec("Cognome").Value)
            Dim Mail As String = Rec("EMail").Value

            Sql = "Insert Into Giocatori Values (" &
                " " & DatiGioco.AnnoAttuale & ", " &
                " " & idGiocatore & ", " &
                "'" & SistemaTestoPerDB(Utente.ToUpper.Trim) & "', " &
                "'" & CriptaPassword(Rec("Password").Value) & "', " &
                " " & DatiGioco.Giornata & ", " &
                "'" & SistemaTestoPerDB(Cognome) & "', " &
                "'" & SistemaTestoPerDB(Nome) & "', " &
                " " & Permessi.Giocatore & ", " &
                "'" & SistemaTestoPerDB(Rec("EMail").Value) & "', " &
                "'N', " &
                "'', " &
                "'S', " &
                "'S' " &
                ")"
            Rec.Close()
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Delete From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & SistemaTestoPerDB(Utente) & "'"
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into DettaglioGiocatori Values (" & _
                " " & DatiGioco.AnnoAttuale & ", " & _
                " " & idGiocatore & ", " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0 " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim Quante As Integer

            Quante = DatiGioco.Giornata
            Select Case DatiGioco.StatoConcorso
                Case ValoriStatoConcorso.Aperto
                Case ValoriStatoConcorso.Chiuso
                    Quante -= 1
                Case ValoriStatoConcorso.DaControllare
                    Quante -= 1
                Case ValoriStatoConcorso.Nessuno
            End Select

            If Quante < 0 Then
                Quante = 0
            End If

            Dim Totale As Single = ((NumeroGiornateTotali - Quante) * QuotaGiocoSettimanale) + QuotaPerSpeciali

            Sql = "Insert Into Bilancio Values (" & _
                " " & DatiGioco.AnnoAttuale & ", " & _
                " " & idGiocatore & ", " & _
                " " & Totale.ToString.Replace(",", ".") & ", " & _
                "0, " & _
                "0, " & _
                "0, " & _
                "0 " & _
                ")"
            Db.EsegueSql(ConnSQL, Sql)

            Dim g As New GestioneMail
            Dim gf As New GestioneFilesDirectory

            ' Effettua controlli sulla presentazione dell'amico
            Dim TrovatoAmico As Integer = -1

            Sql = "Select * From Amici Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(Mail).Trim.ToUpper & "' And Registrato='N'"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                TrovatoAmico = Rec("CodGiocatore").Value
            End If
            Rec.Close()

            If TrovatoAmico <> -1 Then
                Sql = "Update Amici " & _
                    "Set Registrato='S' " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And LTrim(RTrim(Upper(EMail)))='" & SistemaTestoPerDB(Mail).Trim.ToUpper & "' And Registrato='N'"
                Db.EsegueSql(ConnSQL, Sql)

                Sql = "Update Bilancio " & _
                    "Set Amici=Amici+3, TotVersamento=TotVersamento-3 " & _
                    "Where Anno=" & DatiGioco.AnnoAttuale & " And CodGiocatore=" & TrovatoAmico
                Db.EsegueSql(ConnSQL, Sql)

                g.InviaMailPerAmicoRegistrato(TrovatoAmico, Utente & " (" & Nome & " " & Cognome & ")")
            End If
            ' Effettua controlli sulla presentazione dell'amico

            ' Prende eventuale immagine dell'anno precedente
            Dim vecchioID As String = ""

            Sql = "Select Giocatore From Giocatori Where (" &
                "(Cognome='" & SistemaTestoPerDB(Cognome) & "' And Nome='" & SistemaTestoPerDB(Nome) & "') Or Giocatore='" & SistemaTestoPerDB(Utente.ToUpper.Trim) & "') And Anno<" & DatiGioco.AnnoAttuale & " Order By anno Desc"
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            If Rec.Eof = False Then
                vecchioID = Rec("Giocatore").Value
            End If
            Rec.Close()

            If vecchioID <> "" Then
                Dim PathAnnoPrec As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & (DatiGioco.AnnoAttuale - 1).ToString.Trim & "\"
                Dim PathAnnoAtt As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Giocatori\" & (DatiGioco.AnnoAttuale).ToString.Trim & "\"
                gf.CreaDirectoryDaPercorso(PathAnnoPrec)
                gf.CreaDirectoryDaPercorso(PathAnnoAtt)
                PathAnnoPrec += vecchioID.ToString.Trim & ".jpg"
                PathAnnoAtt += Utente.ToUpper.Trim & ".jpg"
                If File.Exists(PathAnnoPrec) = True Then
                    FileCopy(PathAnnoPrec, PathAnnoAtt)
                End If
            End If
            ' Prende eventuale immagine dell'anno precedente

            ConnSQL.Close()

            Db = Nothing

            g.InviaMailGiocatoreRegistrato(idGiocatore)

            gf = Nothing
            g = Nothing

            Dim sMessaggi() As String = {"Giocatore registrato correttamente"}
            VisualizzaMessaggioInPopup(sMessaggi, Me)

            CreaTabella()
        End If

        Db = Nothing
    End Sub

    Protected Sub EliminaGiocatore(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        Dim Utente As String = row.Cells(0).Text

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Delete From AppoGiocatori Where Anno=" & DatiGioco.AnnoAttuale & " And Giocatore='" & SistemaTestoPerDB(Utente) & "'"
            Db.EsegueSql(ConnSQL, Sql)

            ConnSQL.Close()

            CreaTabella()
        End If

        Db = Nothing
    End Sub
End Class