Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Security.AccessControl
Imports System.Xml.Linq

Module Program

    Sub Main(args As String())

        Vocab.Instance().LoadXML("words.xml")
        LocationManager.Instance().LoadXML("locations.xml")
        ObjectManager.Instance().LoadXML("objects.xml")
        Vars.Instance().LoadXML("vars.xml")
        Rules.Instance().SetUp()

        'Vars.Instance().alpha.SetValue("CURRENTLOC", "APPARTMENTS")
        'ObjectManager.Instance().MoveObj("BRACELET", "HELD")
        'ObjectManager.Instance().MoveObj("RING", "HELD")
        'ObjectManager.Instance().MoveObj("RUBY", "HELD")
        'ObjectManager.Instance().MoveObj("BELL", "HELD")
        'ObjectManager.Instance().MoveObj("CRYSTAL", "HELD")
        'ObjectManager.Instance().MoveObj("TROPHY", "HELD")
        'ObjectManager.Instance().MoveObj("FIGURINE", "HELD")

        Vars.Instance().bool.SetValue("GAMEOVER", False)
        GameIO.DescribeLoc(Vars.Instance().alpha.GetValue("CURRENTLOC"))
        Dim input As String

        Do Until Vars.Instance().bool.GetValue("GAMEOVER")
            input = GameIO.GetCommand()
            If Not Rules.Instance().ApplyRules(input) Then
                Console.WriteLine("I don't understand")
            End If
        Loop

    End Sub
End Module
