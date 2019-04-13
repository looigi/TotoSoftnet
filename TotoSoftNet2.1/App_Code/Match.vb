Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class Match
    Public Class Match
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
        Public Property TournamentRoundMatches() As SortedList(Of Integer, SortedList(Of Integer, Match))
            Get
                Return m_TournamentRoundMatches
            End Get
            Private Set(value As SortedList(Of Integer, SortedList(Of Integer, Match)))
                m_TournamentRoundMatches = Value
            End Set
        End Property
        Private m_TournamentRoundMatches As SortedList(Of Integer, SortedList(Of Integer, Match))
        Public Property ThirdPlaceMatch() As Match
            Get
                Return m_ThirdPlaceMatch
            End Get
            Private Set(value As Match)
                m_ThirdPlaceMatch = Value
            End Set
        End Property
        Private m_ThirdPlaceMatch As Match

        Public Sub New(rounds As Integer)
            Me.TournamentRoundMatches = New SortedList(Of Integer, SortedList(Of Integer, Match))()
            Me.GenerateTournamentResults(rounds)
            If rounds > 1 Then
                Me.GenerateThirdPlaceResult(rounds)
            End If
        End Sub

        Public Sub AddMatch(m As Match)
            If Me.TournamentRoundMatches.ContainsKey(m.roundnumber) Then
                If Not Me.TournamentRoundMatches(m.roundnumber).ContainsKey(m.id) Then
                    Me.TournamentRoundMatches(m.roundnumber).Add(m.id, m)
                End If
            Else
                Me.TournamentRoundMatches.Add(m.roundnumber, New SortedList(Of Integer, Match)())
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
                    Me.AddMatch(New Match(match_id, team1_id, team2_id, round, winner))
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
            Dim semifinal_1 As Match = Me.TournamentRoundMatches(rounds - 1)(semifinal_matchid1)
            Dim semifinal_2 As Match = Me.TournamentRoundMatches(rounds - 1)(semifinal_matchid2)
            Dim semifinal_loser1 As Integer = If((semifinal_1.winner = semifinal_1.teamid1), semifinal_1.teamid2, semifinal_1.teamid1)
            Dim semifinal_loser2 As Integer = If((semifinal_2.winner = semifinal_2.teamid1), semifinal_2.teamid2, semifinal_2.teamid1)
            Dim third_place_winner As Integer = If((WinnerRandomizer.[Next](1, 3) = 1), semifinal_loser1, semifinal_loser2)
            Me.ThirdPlaceMatch = New Match((1 << rounds) + 1, semifinal_loser1, semifinal_loser2, 1, third_place_winner)
        End Sub
    End Class
End Class
