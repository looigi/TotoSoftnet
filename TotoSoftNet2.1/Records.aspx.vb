Public Class Records
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaRecords()
        End If
    End Sub

    Private Sub CaricaRecords()
        CaricaMaxPunti()
        CaricaMinPunti()
        CaricaMaxPuntiCampionato()
        CaricaMinPuntiCampionato()
        CaricaMaxPuntiStorico()
        CaricaMinPuntiStorico()

        CaricaVoltePrimoC()
        CaricaVolteSecondoC()
        CaricaVolteUltimoC()
        CaricaVoltePrimoS()
        CaricaVolteSecondoS()
        CaricaVolteUltimoS()

        CaricaGiocate()
        CaricaTappi()
    End Sub

    Private Sub CaricaTappi()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " QuandoTappi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where B.Giocatore Is Not Null " & _
            "Group By Giocatore " & _
            "Order By 2 Desc, Giocatore"
        Gr.ImpostaCampi(Sql, grdTappi)

        Gr = Nothing
    End Sub

    Private Sub CaricaGiocate()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quante From " & _
            " Pronostici A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where Partita = 1 And B.Giocatore Is Not Null And A.Giornata <100 " & _
            "Group By Giocatore " & _
            "Order By 2 Desc, Giocatore"
        Gr.ImpostaCampi(Sql, grdGiocate)

        Gr = Nothing
    End Sub

    Private Sub CaricaVoltePrimoC()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Primi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdPrimiCampionato)

        Gr = Nothing
    End Sub

    Private Sub CaricaVolteSecondoC()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Secondi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdSecondiCampionato)

        Gr = Nothing
    End Sub

    Private Sub CaricaVolteUltimoC()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Ultimi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Where A.Anno=" & DatiGioco.AnnoAttuale & " " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdUltimoCampionato)

        Gr = Nothing
    End Sub

    Private Sub CaricaVoltePrimoS()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Primi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdPrimiStorico)

        Gr = Nothing
    End Sub

    Private Sub CaricaVolteSecondoS()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Secondi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdSecondiStorico)

        Gr = Nothing
    End Sub

    Private Sub CaricaVolteUltimoS()
        Dim Gr As New Griglie
        Dim Sql As String = ""

        Sql = "Select Top 5 Giocatore, Count(*) As Quanti From " & _
            " Ultimi A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            "Group By B.Giocatore " & _
            "Order By Quanti Desc"
        Gr.ImpostaCampi(Sql, grdUltimoStorico)

        Gr = Nothing
    End Sub

    Private Function PrendeCosa() As String
        Dim Ritorno As String = ""

        If chkJolly.Checked = True Then
            Ritorno += "PuntiJolly+"
        End If
        If chkSegni.Checked = True Then
            Ritorno += "PuntiSegni+"
        End If
        If chkRisultati.Checked = True Then
            Ritorno += "PuntiRisultati+"
        End If
        If chkQuote.Checked = True Then
            Ritorno += "PuntiQuote+"
        End If
        If chkFR.Checked = True Then
            Ritorno += "PuntiFR+"
        End If
        If Right(Ritorno, 1) = "+" Then
            Ritorno = Mid(Ritorno, 1, Ritorno.Length - 1)
        End If

        Return Ritorno
    End Function

    Private Function PrendeValore(Sql As String) As String
        Dim Ritorno As String
        Dim Db As New GestioneDB
        Dim ConnSql As Object
        Dim Rec As Object = Server.CreateObject("ADODB.Recordset")

        If Db.LeggeImpostazioniDiBase() = True Then
            ConnSql = Db.ApreDB()
            Rec = Db.LeggeQuery(ConnSql, Sql)
            If Rec(0).Value Is DBNull.Value = True Then
                Ritorno = -1
            Else
                Ritorno = Rec(0).Value
            End If
            Rec.Close()
            ConnSql.Close()
        End If
        Db = Nothing

        Ritorno = Ritorno.Replace(",", ".")

        Return Ritorno
    End Function

    Private Sub CaricaMaxPunti()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            Sql = "Select Max(" & Cosa & ") As Massimo From Risultati"
            Dim Quanto As String = PrendeValore(Sql)

            Sql = "Select B.Giocatore, " & Cosa & " As Punti, A.Anno, A.Concorso As Giornata From Risultati A Left Join " & _
                    " Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " & _
                    "Where " & Cosa & "=" & Quanto & " " & _
                    "Order By A.Anno Desc, Concorso Desc, Giocatore"
            Gr.ImpostaCampi(Sql, grdMaxPunti)
            grdMaxPunti.Visible = True
        Else
            grdMaxPunti.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub CaricaMinPunti()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            Sql = "Select Min(" & Cosa & ") As Massimo From Risultati"
            Dim Quanto As String = PrendeValore(Sql)

            Sql = "Select B.Giocatore, " & Cosa & " As Punti, A.Anno, A.Concorso As Giornata From Risultati A Left Join " & _
                " Giocatori B On A.CodGiocatore=B.CodGiocatore And A.Anno=B.Anno " & _
                "Where " & Cosa & "=" & Quanto & " " & _
                "Order By A.Anno Desc, Concorso Desc, Giocatore"
            Gr.ImpostaCampi(Sql, grdMinPunti)
            grdMinPunti.Visible = True
        Else
            grdMinPunti.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub CaricaMaxPuntiCampionato()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            'Sql = "Select Top 5 B.Giocatore, Sum(" & Cosa & ")+(" &
            '    "Select SUM(D.Punti) From TSN2_RiconDisonGioc C " &
            '    "Left OUTER JOIN TSN2_RiconDison D On C.idPremio=D.idPremio " &
            '    "Where idAnno=A.Anno And C.CodGioc=B.CodGiocatore)) As Punti, A.Anno, Count(*) As Partite From " & _
            '    " Risultati A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            '    "Where A.Concorso<100 " & _
            '    "Group By A.Anno, B.Giocatore, B.CodGiocatore " & _
            '    "Order By Punti Desc, Partite Desc"

            Sql = "Select Top 5 Giocatore, SUM(PuntiTot)+SUM(RiconDison) As Punti, Anno, Partite From (" &
                "Select B.Giocatore, Sum(" & Cosa & ") As PuntiTot,(" &
                "Select SUM(D.Punti) From RiconDisonGioc C " &
                "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio Where idAnno=A.Anno And C.CodGioc=B.CodGiocatore " &
                ") As RiconDison, A.Anno, Count(*) As Partite From" &
                " Risultati A " &
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                "Where A.Concorso<100 Group By A.Anno, B.Giocatore, B.CodGiocatore " &
                ") F Group By Giocatore, Anno, Partite " &
                "Order By Punti Desc, Partite Desc"

            Gr.ImpostaCampi(Sql, grdMaxPuntiCampionato)
            grdMaxPuntiCampionato.Visible = True
        Else
            grdMaxPuntiCampionato.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub CaricaMinPuntiCampionato()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            'Sql = "Select Top 5 B.Giocatore, Sum(" & Cosa & ") As Punti, A.Anno, Count(*) As Partite From " & _
            '    " Risultati A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            '    "Group By A.Anno, B.Giocatore " & _
            '    "Having Count(*)>35 " & _
            '    "Order By Punti, Partite "

            Sql = "Select Top 5 Giocatore, SUM(PuntiTot)+SUM(RiconDison) As Punti, Anno, Partite From (" &
                "Select B.Giocatore, Sum(" & Cosa & ") As PuntiTot,(" &
                "Select SUM(D.Punti) From RiconDisonGioc C " &
                "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio Where idAnno=A.Anno And C.CodGioc=B.CodGiocatore " &
                ") As RiconDison, A.Anno, Count(*) As Partite From " &
                " Risultati A " &
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                "Where A.Concorso<100 Group By A.Anno, B.Giocatore, B.CodGiocatore " &
                ") F " &
                "Where Partite > 35 " &
                "Group By Giocatore, Anno, Partite " &
                "Order By Punti, Partite Desc"

            Gr.ImpostaCampi(Sql, grdMinPuntiCampionato)
            grdMinPuntiCampionato.Visible = True
        Else
            grdMinPuntiCampionato.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub CaricaMaxPuntiStorico()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            'Sql = "Select Top 5 Giocatore, Sum(" & Cosa & ") As Punti, Count(*) As Partite From " & _
            '    " Risultati A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            '    "Group By Giocatore " & _
            '    "Order By Punti Desc"

            Sql = "Select Top 5 Giocatore, SUM(PuntiTot)+SUM(RiconDison) As Punti, Sum(Partite) From (" &
                "Select B.Giocatore, Sum(" & Cosa & ") As PuntiTot,( " &
                "Select SUM(D.Punti) From RiconDisonGioc C " &
                "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio Where idAnno=A.Anno And C.CodGioc=B.CodGiocatore " &
                ") As RiconDison, Count(*) As Partite From " &
                " Risultati A " &
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                "Where A.Concorso<100 Group By A.Anno, B.Giocatore, B.CodGiocatore " &
                ") F Group By Giocatore, Partite " &
                "Order By Punti Desc, Partite Desc"

            Gr.ImpostaCampi(Sql, grdMaxPuntiStorico)
            grdMaxPuntiStorico.Visible = True
        Else
            grdMaxPuntiStorico.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub CaricaMinPuntiStorico()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim Cosa As String = PrendeCosa()

        If Cosa <> "" Then
            'Sql = "Select Top 5 Giocatore, Sum(" & Cosa & ") As Punti, Count(*) As Partite From " & _
            '    " Risultati A Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " & _
            '    "Group By Giocatore " & _
            '    "Having Count(*)>35 " & _
            '    "Order By Punti"

            Sql = "Select Top 5 Giocatore, SUM(PuntiTot)+SUM(RiconDison) As Punti, Sum(Partite) From (" &
                "Select B.Giocatore, Sum(PuntiJolly+PuntiSegni+PuntiRisultati+PuntiQuote+PuntiFR) As PuntiTot,(" &
                "Select SUM(D.Punti) From RiconDisonGioc C " &
                "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio Where idAnno=A.Anno And C.CodGioc=B.CodGiocatore " &
                ") As RiconDison, Count(*) As Partite From  " &
                " Risultati A " &
                "Left Join Giocatori B On A.Anno=B.Anno And A.CodGiocatore=B.CodGiocatore " &
                "Where A.Concorso<100 Group By A.Anno, B.Giocatore, B.CodGiocatore " &
                ") F " &
                "Where Partite > 35 " &
                "Group By Giocatore, Partite " &
                "Order By Punti , Partite Desc"
            Gr.ImpostaCampi(Sql, grdMinPuntiStorico)
            grdMinPuntiStorico.Visible = True
        Else
            grdMinPuntiStorico.Visible = False
        End If

        Gr = Nothing
    End Sub

    Private Sub grdMaxPunti_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMaxPunti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            Dim Anno As String = e.Row.Cells(3).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome, Anno)
        End If
    End Sub

    Private Sub grdMaxPuntiCampionato_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMaxPuntiCampionato.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdMinPuntiCampionato_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMinPuntiCampionato.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            Dim Anno As String = e.Row.Cells(3).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome, Anno)
        End If
    End Sub

    Private Sub grdMinPunti_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMinPunti.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text
            Dim Anno As String = e.Row.Cells(3).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome, Anno)
        End If
    End Sub

    Private Sub grdMaxPuntiStorico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMaxPuntiStorico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdMinPuntiStorico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMinPuntiStorico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdPrimi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPrimiCampionato.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdSecondi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdSecondiCampionato.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdUltimi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUltimoCampionato.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdPrimiStorico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPrimiStorico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdSecondiStorico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdSecondiStorico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdUltimoStorico_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUltimoStorico.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdTappi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTappi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Private Sub grdGiocate_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdGiocate.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim img As Image = DirectCast(e.Row.FindControl("imgAvatar"), Image)
            Dim Nome As String = e.Row.Cells(1).Text

            img.ImageUrl = RitornaImmagine(Server.MapPath("."), Nome)
        End If
    End Sub

    Protected Sub Ricarica()
        CaricaMaxPunti()
        CaricaMinPunti()
        CaricaMaxPuntiCampionato()
        CaricaMinPuntiCampionato()
        CaricaMaxPuntiStorico()
        CaricaMinPuntiStorico()
    End Sub
End Class