Module mdlInviaMessaggi
    Public Iconizzato As Boolean
    Public ModalitaTest As Boolean
    Public StringaConnessione As String
    Public Anno As Integer
    Public Giornata As Integer
    Public DataChiusura As Date
    Public StatoConcorso As String
    Public Differenza As TimeSpan
    Public DifferenzaTS As DateTime
    Public PercorsoImmaginiRoot As String = "http://looigi.no-ip.biz:12345/TotoMioII/"
    Public Logo As String = PercorsoImmaginiRoot & "App_Themes/Standard/Images/Icone/logo.png"
    Public ImmagineDiTestaMail As String = "<img src='" & Logo & "' width='400px' height='150px' /><hr />"
    Public EmailDiInvio As String
    Public CasellaDiPosta As String
    Public PasswordCasellaDiPosta As String
    Public IndirizzoSito As String = "http://looigi.no-ip.biz:12345/TotoMioII"
    Public PercorsoApplicazione As String

    Public Function ApreTesto() As String
        Return "<span style=""font-family:Tahoma; font-size: 13px; color: #000000; font-weight:bold;"">"
    End Function

    Public Function ApreTestoGrande() As String
        Return "<span style=""font-family:Tahoma; font-size: 16px; color: #AA0000; font-weight:bold;"">"
    End Function

    Public Function ChiudeTesto() As String
        Return "</span>"
    End Function

    Public Function PrendeStilePerMail() As String
        Dim Stile As String = ""

        Stile += "opacity:0.82; "
        Stile += "background-color: #FFFFFF; "
        Stile += "padding: 4px; "
        Stile += "margin: 0 auto; "
        Stile += "padding-bottom: 7px; "

        Stile += "-webkit-box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "
        Stile += "-moz-box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "
        Stile += "box-shadow: 0px 0px 4px 1px rgba(0, 0, 0, .3); "

        Stile += "-webkit-border-radius: 0.45em 0.45em 0.45em 0.45em; "
        Stile += "-moz-border-radius: 0.45em 0.45em 0.45em 0.45em; "
        Stile += "border-radius: 0.35em 0.45em 0.45em 0.45em; "

        Stile += "display: inline-block;"

        Return Stile
    End Function
End Module
