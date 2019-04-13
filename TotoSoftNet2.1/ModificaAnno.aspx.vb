Imports System.IO

Public Class ModificaAnno
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            txtDoppie.Text = MaxDoppie
            txtGiornate.Text = NumeroGiornateTotali
            txtPuntoVirgola.Text = PuntoVirgola
            txtCostoGiocata.Text = QuotaGiocoSettimanale
            txtGiornataSemifinale.Text = GiornataSemifinale
            txtGiornataFinale.Text = GiornataFinale
            txtSpeciali.Text = QuotaPerSpeciali

            CaricaDatiAnno()

            Dim Percorso As String = "App_Themes/Standard/Images/Anni/" & DatiGioco.AnnoAttuale & ".jpg"
            Dim Immagine As String

            If File.Exists(Server.MapPath(Percorso)) = True Then
                Immagine = Percorso
            Else
                Immagine = "App_Themes/Standard/Images/Main.jpg"
            End If

            imgSfondo.ImageUrl = Immagine
        End If
    End Sub

    Private Sub CaricaDatiAnno()
        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = ""

            Sql = "Select * From Anni Where Anno=" & DatiGioco.AnnoAttuale
            Rec = Db.LeggeQuery(ConnSQL, Sql)
            txtDescAnno.Text = Rec("Descrizione").Value
            txtInizioAnno.Text = Rec("AnnoInizio").Value
            Rec.Close
        End If

        Db = Nothing
    End Sub

    Private Function ControllaBuonoNumerico(Cosa As String, Minimo As Single, Massimo As Single) As Boolean
        Dim Ok As Boolean = True
        Dim Valore As Single = Cosa.Replace(".", ",")

        If Cosa = "" Or IsNumeric(Cosa) = False Or Val(Valore) < Minimo Or Val(Valore) > Massimo Then
            Ok = False
        End If

        Return Ok
    End Function

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If ControllaBuonoNumerico(txtGiornate.Text, 1, 40) = False Then
            Dim Messaggi() As String = {"Giornata non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If ControllaBuonoNumerico(txtDoppie.Text, 1, 10) = False Then
            Dim Messaggi() As String = {"Doppie non valide"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If ControllaBuonoNumerico(txtCostoGiocata.Text, 0, 3) = False Then
            Dim Messaggi() As String = {"Costo giocata settimanale non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If ControllaBuonoNumerico(txtSpeciali.Text, 0, 3) = False Then
            Dim Messaggi() As String = {"Costo quota speciali non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtPuntoVirgola.Text = "" Or (txtPuntoVirgola.Text <> "," And txtPuntoVirgola.Text <> ".") Then
            Dim Messaggi() As String = {"Punto/Virgola non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If ControllaBuonoNumerico(txtGiornataSemifinale.Text, 1, txtGiornate.Text) = False Then
            Dim Messaggi() As String = {"Giornata della semifinale non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If ControllaBuonoNumerico(txtGiornataFinale.Text, 1, txtGiornate.Text) = False Then
            Dim Messaggi() As String = {"Giornata della finale non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtInizioAnno.Text = "" Or IsNumeric(txtInizioAnno.Text) = False Then
            Dim Messaggi() As String = {"Inizio anno non valido"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If
        If txtDescAnno.Text = "" Then
            Dim Messaggi() As String = {"Descrizione anno non valida"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
            Exit Sub
        End If

        Dim Db As New GestioneDB

        If Db.LeggeImpostazioniDiBase() = True Then
            Dim ConnSQL As Object = Db.ApreDB()
            Dim Sql As String

            Sql = "Update AnnoConfig Set " &
                "Giornate=" & txtGiornate.Text & ", " &
                "Doppie=" & txtDoppie.Text & ", " &
                "CostoGiocata=" & txtCostoGiocata.Text.Replace(",", ".") & ", " &
                "GiornataSemifinale=" & txtGiornataSemifinale.Text.Replace(",", ".") & ", " &
                "GiornataFinale=" & txtGiornataFinale.Text.Replace(",", ".") & ", " &
                "QuotaPerSpeciali=" & txtSpeciali.Text.Replace(",", ".") & ", " &
                "PuntoVirgola='" & txtPuntoVirgola.Text & "' " &
                "Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            Sql = "Update Anni Set Descrizione='" & SistemaTestoPerDB(txtDescAnno.Text) & "', AnnoInizio=" & txtInizioAnno.Text & " Where Anno=" & DatiGioco.AnnoAttuale
            Db.EsegueSql(ConnSQL, Sql)

            MaxDoppie = txtDoppie.Text
            NumeroGiornateTotali = txtGiornate.Text
            PuntoVirgola = txtPuntoVirgola.Text
            QuotaGiocoSettimanale = txtCostoGiocata.Text
            GiornataSemifinale = txtGiornataSemifinale.Text
            GiornataFinale = txtGiornataFinale.Text
            QuotaPerSpeciali = txtSpeciali.Text

            ConnSQL.Close()

            SalvaImmagine()

            Dim Messaggi() As String = {"Dati anno modificati"}
            VisualizzaMessaggioInPopup(Messaggi, Me)
        End If

        Db = Nothing
    End Sub

    Protected Sub SalvaImmagine()
        If FileUpload1.HasFile = False Then
            'VisualizzaMessaggioInPopup("Selezionare un'immagine", Master)
            Exit Sub
        End If

        Dim Percorso As String
        Dim Nome As String = DatiGioco.AnnoAttuale

        Percorso = Server.MapPath(".") & "\App_Themes\Standard\Images\Anni\"

        Dim f As New GestioneFilesDirectory

        f.CreaDirectoryDaPercorso(Percorso)

        f = Nothing

        Percorso += Nome & ".jpg"

        FileUpload1.SaveAs(Percorso)

        imgSfondo.ImageUrl = "App_Themes/Standard/Images/Anni/" & Nome & ".jpg"

        Dim Messaggi() As String = {"Immagine di sfondo caricata"}
        VisualizzaMessaggioInPopup(Messaggi, Me)
    End Sub
End Class