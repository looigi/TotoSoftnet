Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO

Public Class GeneraTabellone
    Private Shared Turni

    Public Sub ImpostaQuantiTurni(Numero As Integer)
        Turni = Numero
    End Sub

    Private Shared Function GenerateHTMLResultsTable(tournament As Tournament) As String
        Dim match_white_span As Integer
        Dim match_span As Integer
        Dim position_in_match_span As Integer
        Dim column_stagger_offset As Integer
        Dim effective_row As Integer
        Dim col_match_num As Integer
        Dim cumulative_matches As Integer
        Dim effective_match_id As Integer
        Dim rounds As Integer = tournament.TournamentRoundMatches.Count
        Dim teams As Integer = 1 << rounds
        Dim max_rows As Integer = teams << 1
        Dim HTMLTable As New StringBuilder()
        Dim NomeTurno() As String = {"", "1,2", "3,4", "5,6", "7,8", "9,10"}
        Dim ValoreSpanPerBootStrap As Integer = 3
        Dim Partita(rounds + 1) As Integer

        Select Case Turni
            Case 1
                ValoreSpanPerBootStrap = 6
            Case 2
                ValoreSpanPerBootStrap = 6
            Case 3
                ValoreSpanPerBootStrap = 4
            Case 4
                ValoreSpanPerBootStrap = 2
        End Select

        HTMLTable.AppendLine("<style type=""text/css"">")
        HTMLTable.AppendLine("    .thd {background: rgb(120,120,120); font: bold 10pt Arial; color: #FFFFFF;}")
        HTMLTable.AppendLine("    .team {color: white; font: bold 10pt Arial; min-width: 130px;}")
        HTMLTable.AppendLine("    .winner {color: white; background: #A2D0A1; font: bold 10pt Arial; border: 1px solid #aaaaaa; min-width: 130px; max-width: 250px;}")
        HTMLTable.AppendLine("    .vs {font: bold 7pt Arial; }")
        HTMLTable.AppendLine("    td, th {padding: 1px 1px; }")
        HTMLTable.AppendLine("    h1 {font: bold 14pt Arial; margin-top: 7pt;}")
        HTMLTable.AppendLine("    .ScrittaTurni { font-family:Verdana; font-size: 10px; color: #000000; }")
        HTMLTable.AppendLine("    .white_span {border: 0;}")
        HTMLTable.AppendLine("</style>")

        HTMLTable.AppendLine("<h1>****TITOLO****</h1>")
        HTMLTable.AppendLine("<table border=""0"" cellspacing=""0"" style=""width: 99%;"">")

        For row As Integer = 0 To max_rows
            cumulative_matches = 0
            HTMLTable.AppendLine("    <tr>")

            For col As Integer = 1 To rounds + 1
                match_span = 1 << (col + 1)
                match_white_span = (1 << col) - 1
                column_stagger_offset = match_white_span >> 1
                If row = 0 Then
                    HTMLTable.AppendLine("        <th style=""width: 10px;""></th>")
                    If col <= rounds Then
                        HTMLTable.AppendLine("        <th class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " thd"">Turno " & col.ToString & "</th>")
                    Else
                        HTMLTable.AppendLine("        <th class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " thd"">Finale</th>")
                    End If
                ElseIf row = 1 Then
                    HTMLTable.AppendLine("        <td style=""width: 10px;""></td>")
                    HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " white_span"" rowspan=""" & (match_white_span - column_stagger_offset).ToString & """>&nbsp;</td>")
                Else
                    HTMLTable.AppendLine("        <td style=""width: 10px;""></td>")
                    effective_row = row + column_stagger_offset
                    If col <= rounds Then
                        position_in_match_span = effective_row Mod match_span
                        position_in_match_span = If((position_in_match_span = 0), match_span, position_in_match_span)
                        col_match_num = (effective_row \ match_span) + (If((position_in_match_span < match_span), 1, 0))
                        effective_match_id = cumulative_matches + col_match_num
                        If (position_in_match_span = 1) AndAlso (effective_row Mod match_span = position_in_match_span) Then
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " white_span"" rowspan=""" & match_white_span & """>&nbsp;</td>")
                        ElseIf (position_in_match_span = (match_span >> 1)) AndAlso (effective_row Mod match_span = position_in_match_span) Then
                            Partita(col) += 1
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " team"">****" & NomeTurno(col) & "-" & Partita(col).ToString & "****</td>")
                        ElseIf (position_in_match_span = ((match_span >> 1) + 1)) AndAlso (effective_row Mod match_span = position_in_match_span) Then
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " vs"" rowspan=""" & match_white_span & """>Vs.</td>")
                        ElseIf (position_in_match_span = match_span) AndAlso (effective_row Mod match_span = 0) Then
                            Partita(col) += 1
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " team"">****" & NomeTurno(col) & "-" & Partita(col).ToString & "****</td>")
                        End If
                    Else
                        If row = column_stagger_offset + 2 Then
                            Partita(col) += 1
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " winner"">****IMMCOPPA****<br />****" & NomeTurno(col) & "-" & Partita(col).ToString & "****</td>")
                        ElseIf row = column_stagger_offset + 3 Then
                            HTMLTable.AppendLine("        <td class=""span" & ValoreSpanPerBootStrap.ToString.Trim & " white_span"" rowspan=""" & (match_white_span - column_stagger_offset) & """>&nbsp;</td>")
                        End If
                    End If
                End If
                If col <= rounds Then
                    cumulative_matches += tournament.TournamentRoundMatches(col).Count
                End If
            Next
            HTMLTable.AppendLine("    </tr>")
        Next
        HTMLTable.AppendLine("</table>")

        Return HTMLTable.ToString()
    End Function

    Public Function Esegui() As String
        Dim Test3RoundTournament As New Tournament(Turni)

        Return GenerateHTMLResultsTable(Test3RoundTournament)
    End Function
End Class

Public Class Partite
    Public Property id() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = Value
        End Set
    End Property
    Private m_id As Integer
    Public Property teamid1() As Integer
        Get
            Return m_teamid1
        End Get
        Set(value As Integer)
            m_teamid1 = Value
        End Set
    End Property
    Private m_teamid1 As Integer
    Public Property teamid2() As Integer
        Get
            Return m_teamid2
        End Get
        Set(value As Integer)
            m_teamid2 = Value
        End Set
    End Property
    Private m_teamid2 As Integer
    Public Property roundnumber() As Integer
        Get
            Return m_roundnumber
        End Get
        Set(value As Integer)
            m_roundnumber = Value
        End Set
    End Property
    Private m_roundnumber As Integer
    Public Property winner() As Integer
        Get
            Return m_winner
        End Get
        Set(value As Integer)
            m_winner = Value
        End Set
    End Property
    Private m_winner As Integer

    Public Sub New(id As Integer, teamid1 As Integer, teamid2 As Integer, roundnumber As Integer, winner As Integer)
        Me.id = id
        Me.teamid1 = teamid1
        Me.teamid2 = teamid2
        Me.roundnumber = roundnumber
        Me.winner = winner
    End Sub
End Class

Public Class Tournament
    Public Property TournamentRoundMatches() As SortedList(Of Integer, SortedList(Of Integer, Partite))
        Get
            Return m_TournamentRoundMatches
        End Get
        Private Set(value As SortedList(Of Integer, SortedList(Of Integer, Partite)))
            m_TournamentRoundMatches = Value
        End Set
    End Property
    Private m_TournamentRoundMatches As SortedList(Of Integer, SortedList(Of Integer, Partite))
    Public Property ThirdPlaceMatch() As Partite
        Get
            Return m_ThirdPlaceMatch
        End Get
        Private Set(value As Partite)
            m_ThirdPlaceMatch = Value
        End Set
    End Property
    Private m_ThirdPlaceMatch As Partite

    Public Sub New(rounds As Integer)
        Me.TournamentRoundMatches = New SortedList(Of Integer, SortedList(Of Integer, Partite))()
        Me.GenerateTournamentResults(rounds)
        If rounds > 1 Then
            Me.GenerateThirdPlaceResult(rounds)
        End If
    End Sub

    Public Sub AddMatch(m As Partite)
        If Me.TournamentRoundMatches.ContainsKey(m.roundnumber) Then
            If Not Me.TournamentRoundMatches(m.roundnumber).ContainsKey(m.id) Then
                Me.TournamentRoundMatches(m.roundnumber).Add(m.id, m)
            End If
        Else
            Me.TournamentRoundMatches.Add(m.roundnumber, New SortedList(Of Integer, Partite)())
            Me.TournamentRoundMatches(m.roundnumber).Add(m.id, m)
        End If
    End Sub

    Private Sub GenerateTournamentResults(rounds As Integer)
        Dim WinnerRandomizer As New Random()

        Dim round As Integer = 1, match_id As Integer = 1
        While round <= rounds
            Dim matches_in_round As Integer = 1 << (rounds - round)
            Dim round_match As Integer = 1
            While round_match <= matches_in_round
                Dim team1_id As Integer
                Dim team2_id As Integer
                Dim winner As Integer
                If round = 1 Then
                    team1_id = (match_id * 2) - 1
                    team2_id = (match_id * 2)
                Else
                    Dim match1 As Integer = (match_id - (matches_in_round * 2) + (round_match - 1))
                    Dim match2 As Integer = match1 + 1
                    team1_id = Me.TournamentRoundMatches(round - 1)(match1).winner
                    team2_id = Me.TournamentRoundMatches(round - 1)(match2).winner
                End If
                winner = If((WinnerRandomizer.[Next](1, 3) = 1), team1_id, team2_id)
                Me.AddMatch(New Partite(match_id, team1_id, team2_id, round, winner))
                round_match += 1
                match_id += 1
            End While
            round += 1
        End While
    End Sub

    Private Sub GenerateThirdPlaceResult(rounds As Integer)
        Dim WinnerRandomizer As New Random()
        Dim semifinal_matchid1 As Integer = Me.TournamentRoundMatches(rounds - 1).Keys.ElementAt(0)
        Dim semifinal_matchid2 As Integer = Me.TournamentRoundMatches(rounds - 1).Keys.ElementAt(1)
        Dim semifinal_1 As Partite = Me.TournamentRoundMatches(rounds - 1)(semifinal_matchid1)
        Dim semifinal_2 As Partite = Me.TournamentRoundMatches(rounds - 1)(semifinal_matchid2)
        Dim semifinal_loser1 As Integer = If((semifinal_1.winner = semifinal_1.teamid1), semifinal_1.teamid2, semifinal_1.teamid1)
        Dim semifinal_loser2 As Integer = If((semifinal_2.winner = semifinal_2.teamid1), semifinal_2.teamid2, semifinal_2.teamid1)
        Dim third_place_winner As Integer = If((WinnerRandomizer.[Next](1, 3) = 1), semifinal_loser1, semifinal_loser2)
        Me.ThirdPlaceMatch = New Partite((1 << rounds) + 1, semifinal_loser1, semifinal_loser2, 1, third_place_winner)
    End Sub
End Class