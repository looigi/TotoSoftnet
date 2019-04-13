Public Class StringheClassifiche
    Public Function RitornaStringaClassificaGenerale(Optional Giornata As Integer = -1)
        Dim Ritorno As String

        If Giornata = -1 Then Giornata = DatiGioco.Giornata

        Ritorno = "Select CodGiocatore, Giocatore, Testo, Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) As TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti, IsNull(Sum(Punti),0) As RiconDison From ( " & _
            "SELECT A_1.CodGiocatore, A_1.Giocatore, A_1.Testo, A_1.TotPunti, A_1.PSegni, A_1.PRisultati, " & _
            "A_1.PJolly, A_1.PQuote, A_1.PFR, B.NumeroTappi, B.Vittorie, B.UltimiPosti, B.SecondiPosti, D.Punti FROM " & _
            "(SELECT A.Anno, A.CodGiocatore, A.Giocatore, A.Testo, SUM(B.PuntiSegni) + SUM(B.PuntiRisultati) + SUM(B.PuntiJolly) + SUM(B.PuntiQuote) + SUM(B.PuntiFR) AS TotPunti, SUM(B.PuntiSegni) AS PSegni,  SUM(B.PuntiRisultati) AS PRisultati, SUM(B.PuntiJolly) AS PJolly, SUM(B.PuntiQuote) AS PQuote, SUM(B.PuntiFR) AS PFR " & _
            "FROM Giocatori AS A LEFT OUTER JOIN Risultati AS B ON A.Anno = B.Anno AND A.CodGiocatore = B.CodGiocatore  " & _
            "WHERE(A.Anno=" & DatiGioco.AnnoAttuale & " And B.Concorso <= " & Giornata & " And Cancellato='N')  GROUP BY A.Anno, A.Giocatore, A.CodGiocatore, A.Testo) AS A_1 " & _
            "LEFT OUTER JOIN DettaglioGiocatori AS B ON A_1.Anno = B.Anno And A_1.CodGiocatore = B.CodGiocatore " & _
            "LEFT OUTER JOIN RiconDisonGioc C On A_1.Anno =C.idAnno And A_1.CodGiocatore=C.CodGioc " & _
            "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio " & _
            ") A " & _
            "Group By CodGiocatore, Giocatore, Testo, TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti " & _
            "ORDER BY Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) DESC, PSegni DESC, PRisultati DESC, NumeroTappi, Vittorie DESC, UltimiPosti, SecondiPosti DESC, " & _
            "PJolly DESC"

        Return Ritorno
    End Function

    Public Function RitornaStringaClassificaGeneralePerCassa(Optional Giornata As Integer = -1)
        Dim Ritorno As String

        If Giornata = -1 Then Giornata = DatiGioco.Giornata

        Ritorno = "Select CodGiocatore, Giocatore, Testo, Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) As TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti, IsNull(Sum(Punti),0) As RiconDison From ( " & _
            "SELECT A_1.CodGiocatore, A_1.Giocatore, A_1.Testo, A_1.TotPunti, A_1.PSegni, A_1.PRisultati, " & _
            "A_1.PJolly, A_1.PQuote, A_1.PFR, B.NumeroTappi, B.Vittorie, B.UltimiPosti, B.SecondiPosti, D.Punti FROM " & _
            "(SELECT A.Anno, A.CodGiocatore, A.Giocatore, A.Testo, SUM(B.PuntiSegni) + SUM(B.PuntiRisultati) + SUM(B.PuntiJolly) + SUM(B.PuntiQuote) + SUM(B.PuntiFR) AS TotPunti, SUM(B.PuntiSegni) AS PSegni,  SUM(B.PuntiRisultati) AS PRisultati, SUM(B.PuntiJolly) AS PJolly, SUM(B.PuntiQuote) AS PQuote, SUM(B.PuntiFR) AS PFR  " & _
            "FROM Giocatori AS A LEFT OUTER JOIN Risultati AS B ON A.Anno = B.Anno AND A.CodGiocatore = B.CodGiocatore  " & _
            "WHERE(A.Anno=" & DatiGioco.AnnoAttuale & " And B.Concorso <= " & Giornata & " And Cancellato='N' And Pagante='S')  GROUP BY A.Anno, A.Giocatore, A.CodGiocatore, A.Testo) AS A_1 " & _
            "LEFT OUTER JOIN DettaglioGiocatori AS B ON A_1.Anno = B.Anno And A_1.CodGiocatore = B.CodGiocatore " & _
            "LEFT OUTER JOIN RiconDisonGioc C On A_1.Anno =C.idAnno And A_1.CodGiocatore=C.CodGioc " & _
            "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio " & _
            ") A " & _
            "Group By CodGiocatore, Giocatore, Testo, TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti " & _
            "ORDER BY Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) DESC, PSegni DESC, PRisultati DESC, NumeroTappi, Vittorie DESC, UltimiPosti, SecondiPosti DESC, " & _
            "PJolly DESC"

        Return Ritorno
    End Function

    Public Function RitornaStringaClassificaGeneralePerGiocatore(Giocatore As Integer)
        Dim Ritorno As String

        Ritorno = "Select CodGiocatore, Giocatore, Testo, Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) As TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti, IsNull(Sum(Punti),0) As RiconDison From ( " & _
            "SELECT A_1.CodGiocatore, A_1.Giocatore, A_1.Testo, A_1.TotPunti, A_1.PSegni, A_1.PRisultati, " & _
            "A_1.PJolly, A_1.PQuote, A_1.PFR, B.NumeroTappi, B.Vittorie, B.UltimiPosti, B.SecondiPosti, D.Punti FROM " & _
            "(SELECT A.Anno, A.CodGiocatore, A.Giocatore, A.Testo, SUM(B.PuntiSegni) + SUM(B.PuntiRisultati) + SUM(B.PuntiJolly) + SUM(B.PuntiQuote) + SUM(B.PuntiFR) AS TotPunti, SUM(B.PuntiSegni) AS PSegni,  SUM(B.PuntiRisultati) AS PRisultati, SUM(B.PuntiJolly) AS PJolly, SUM(B.PuntiQuote) AS PQuote, SUM(B.PuntiFR) AS PFR  " & _
            "FROM Giocatori AS A LEFT OUTER JOIN Risultati AS B ON A.Anno = B.Anno AND A.CodGiocatore = B.CodGiocatore  " & _
            "WHERE(A.Anno=" & DatiGioco.AnnoAttuale & " And B.Concorso <= " & DatiGioco.Giornata & " And Cancellato='N')  GROUP BY A.Anno, A.Giocatore, A.CodGiocatore, A.Testo) AS A_1 " & _
            "LEFT OUTER JOIN DettaglioGiocatori AS B ON A_1.Anno = B.Anno And A_1.CodGiocatore = B.CodGiocatore " & _
            "LEFT OUTER JOIN RiconDisonGioc C On A_1.Anno =C.idAnno And A_1.CodGiocatore=C.CodGioc " & _
            "Left OUTER JOIN RiconDison D On C.idPremio=D.idPremio " & _
            ") A " & _
            "Where CodGiocatore=" & Giocatore & " " & _
            "Group By CodGiocatore, Giocatore, Testo, TotPunti, PSegni, PRisultati, " & _
            "PJolly, PQuote, PFR, NumeroTappi, Vittorie, UltimiPosti, SecondiPosti " & _
            "ORDER BY Convert(Numeric(7,2),TotPunti)+IsNull(Sum(Punti),0) DESC, PSegni DESC, PRisultati DESC, NumeroTappi, Vittorie DESC, UltimiPosti, SecondiPosti DESC, " & _
            "PJolly DESC"

        Return Ritorno
    End Function

End Class
