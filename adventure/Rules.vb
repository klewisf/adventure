Imports System.ComponentModel
Imports System.Data
Imports System.Formats.Asn1
Imports System.Globalization
Imports System.Linq.Expressions
Imports System.Net.Http.Headers
Imports System.Reflection.Metadata
Imports System.Reflection.Metadata.Ecma335
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.JavaScript
Imports System.Runtime.Loader
Imports System.Runtime.Serialization
Imports System.Security
Imports System.Security.AccessControl
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports System.Threading.Tasks.Dataflow

Public Class Rules

    Public Class Rule

        Public trigger As Func(Of String, Boolean)
        Public conditions As Func(Of String, Boolean)
        Public effects As Action(Of String)
        Public Sub SetTrigger(t As Func(Of String, Boolean))
            trigger = t
        End Sub

        Public Sub SetConditions(c As Func(Of String, Boolean))
            conditions = c
        End Sub

        Public Sub SetEffects(e As Action(Of String))
            effects = e
        End Sub

    End Class

    Private Shared m_instance As Rules = Nothing
    Private ruleList As List(Of Rule) = Nothing

    Public Shared Function Instance() As Rules
        If m_instance Is Nothing Then
            m_instance = New Rules
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Sub SetUp()
        ruleList = New List(Of Rule)

        Dim r = New Rule
        r.SetTrigger(Function(input)
                         If input = "look" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         GameIO.DescribeLoc(Vars.Instance().alpha.GetValue("CURRENTLOC"))
                     End Sub)

        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "north" And Vars.Instance().alpha.GetValue("CURRENTLOC") = "WAYHOME" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim score As Integer = 0
                            If ObjectManager.Instance.WhereIs("FIGURINE") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("BRACELET") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("RING") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("RUBY") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("BELL") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("CRYSTAL") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance.WhereIs("TROPHY") = "HELD" Then
                                score = score + 1
                            End If
                            If ObjectManager.Instance().WhereIs("WATCH") = "HELD" Then
                                score = score + 1
                            End If
                            If score = 8 Then
                                Console.WriteLine("The gargoyle gestures that you can pass. You have won the game!")
                                Return True
                            End If
                            If score = 1 Then
                                Console.WriteLine("The gargoyle says, 'You have found 1 treasure out of 8, you cannot pass'")
                                Return False
                            End If
                            Console.WriteLine("The gargoyle says, 'You have found " & score & " treasures out of 8, you cannot pass'")
                            Return False
                        End Function)
        r.SetEffects(Sub(input)
                         Vars.Instance().bool.SetValue("GAMEOVER", True)
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "examine shelves" And
                            ObjectManager.Instance().GetStateNode("SHELVES").Value = "UNEXAMINED" And
                            Vars.Instance().alpha.GetValue("CURRENTLOC") = "PANTRY" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine(ObjectManager.Instance().GetDescription("SHELVES"))
                         Console.WriteLine("You find a white ball")
                         ObjectManager.Instance().MoveObj("BALL", "PANTRY")
                         ObjectManager.Instance().GetStateNode("SHELVES").Value = "EXAMINED"
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "examine shelves" And
                             ObjectManager.Instance().GetStateNode("LIBRARYSHELVES").Value = "UNEXAMINED" And
                             Vars.Instance().alpha.GetValue("CURRENTLOC") = "LIBRARY" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine(ObjectManager.Instance().GetDescription("LIBRARYSHELVES"))
                         Console.WriteLine("You find a trophy")
                         ObjectManager.Instance().MoveObj("TROPHY", "LIBRARY")
                         ObjectManager.Instance().GetStateNode("LIBRARYSHELVES").Value = "EXAMINED"
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "in" And Vars.Instance().alpha.GetValue("CURRENTLOC") = "HOUSE" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Console.WriteLine("The doors to the house are closed, and cannot be opened")
                            Return False
                        End Function)
        r.SetEffects(Sub(input)

                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "examine rosebush" Then
                             If ObjectManager.Instance().GetStateNode("ROSEBUSH").Value = "UNEXAMINED" Then
                                 If Vars.Instance().alpha.GetValue("CURRENTLOC") = "SUNKENGARDEN" Then
                                     Return True
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine(ObjectManager.Instance().GetDescription("ROSEBUSH"))
                         Console.WriteLine("You find a ring")
                         ObjectManager.Instance().GetStateNode("ROSEBUSH").Value = "EXAMINED"
                         ObjectManager.Instance().MoveObj("RING", "SUNKENGARDEN")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "examine basket" Then
                             If ObjectManager.Instance().GetStateNode("BASKET").Value = "UNEXAMINED" And
                                 ObjectManager.Instance().WhereIs("BASKET") = "POND" Then
                                 Return True
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine(ObjectManager.Instance().GetDescription("BASKET"))
                         Console.WriteLine("You find a ruby")
                         ObjectManager.Instance().GetStateNode("BASKET").Value = "EXAMINED"
                         ObjectManager.Instance().MoveObj("RUBY", "POND")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "examine wardrobe" Then
                             If ObjectManager.Instance().GetStateNode("WARDROBE").Value = "UNEXAMINED" And
                             Vars.Instance().alpha.GetValue("CURRENTLOC") = "GUESTROOM" Then
                                 Return True
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine(ObjectManager.Instance().GetDescription("WARDROBE"))
                         Console.WriteLine("You find secret door leading west")
                         ObjectManager.Instance().GetStateNode("WARDROBE").Value = "EXAMINED"
                         LocationManager.Instance().UnHideLink("GUESTROOM", "west")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "north" And Vars.Instance().alpha.GetValue("CURRENTLOC") = "APPARTMENTS" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("The door leading north does not open")
                     End Sub)
        ruleList.Add(r)


        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "i" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)

        r.SetConditions(Function(input)
                            Return True
                        End Function)

        r.SetEffects(Sub(input)
                         Dim objects As List(Of String) = ObjectManager.Instance().ObjectsInPlace("HELD")
                         Console.WriteLine("You are holding:")
                         If objects.Count = 0 Then
                             Console.WriteLine("    Nothing")
                         Else For Each o In objects
                                 Console.WriteLine("    " & ObjectManager.Instance().CodeToTitle(o))
                             Next
                         End If
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "quit" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)

        r.SetConditions(Function(input)
                            Return True
                        End Function)

        r.SetEffects(Sub(input)
                         Vars.Instance().bool.SetValue("GAMEOVER", True)
                     End Sub)

        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Length = 1 And Vocab.Instance().GetWordType(input_split.First) = "direction" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim links = From link In LocationManager.Instance().GetLocation(Vars.Instance().alpha.GetValue("CURRENTLOC")).Descendants("Link")
                                        Where link.Attribute("command").Value = input
                                        Select link.Attribute("linkto").Value
                            If links.Count = 1 Then
                                If Not LocationManager.Instance().LinkIsHidden(Vars.Instance().alpha.GetValue("CURRENTLOC"), input_split.First) Then
                                    Return True
                                Else
                                    Console.WriteLine("You can't go that way")
                                    Return False
                                End If
                            Else
                                Console.WriteLine("You can't go that way")
                                Return False
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim links = From link In LocationManager.Instance().GetLocation(Vars.Instance().alpha.GetValue("CURRENTLOC")).Descendants("Link")
                                     Where link.Attribute("command").Value = input
                                     Select link.Attribute("linkto").Value
                         Vars.Instance().alpha.SetValue("CURRENTLOC", links.First)
                         GameIO.DescribeLoc(Vars.Instance().alpha.GetValue("CURRENTLOC"))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 2 Then
                             If input_split.First = "examine" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "object" Then
                                     Return True
                                 End If
                             End If
                         End If
                         Return False
                     End Function)

        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = "HELD" Or loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There is more then one " & input_split(1) & " here")
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You don't see a " & input_split(1) & " here")
                                Return False
                            Else
                                Return True
                            End If

                        End Function)

        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = "HELD" Or loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                     Select c
                         Dim code = query.First
                         Console.WriteLine(ObjectManager.Instance().GetDescription(code))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 3 Then
                             If input_split.First = "examine" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "adjective" Then
                                     If Vocab.Instance.GetWordType(input_split(2)) = "object" Then
                                         Return True
                                     End If
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = "HELD" Or loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There is more then one " & input_split(1) & " here")
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You don't see a " & input_split(1) & " " & input_split(2) & " here")
                                Return False
                            Else
                                Return True
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = "HELD" Or loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                     Select c
                         Dim code = query.First
                         Console.WriteLine(ObjectManager.Instance().GetDescription(code))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 2 Then
                             If input_split.First = "take" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "object" Then
                                     Return True
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There is more then one " & input_split(1) & " here")
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You don't see a " & input_split(1) & " here")
                                Return False
                            ElseIf ObjectManager.Instance().IsFixture(query.First) Then
                                Console.WriteLine("You can't take that")
                                Return False
                            Else
                                Return True
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                     Select c
                         ObjectManager.Instance().MoveObj(query.First, "HELD")
                         Console.WriteLine("You take the " & input_split(1))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 3 Then
                             If input_split.First = "take" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "adjective" Then
                                     If Vocab.Instance.GetWordType(input_split(2)) = "object" Then
                                         Return True
                                     End If
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There is more then one " & input_split(1) & input_split(2) & " here")
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You don't see a " & input_split(1) & " " & input_split(2) & " here")
                                Return False
                            Else
                                Return True
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = Vars.Instance().alpha.GetValue("CURRENTLOC")
                                     Select c
                         Dim code = query.First
                         ObjectManager.Instance().MoveObj(code, "HELD")
                         Console.WriteLine("You take the " & input_split(1) & " " & input_split(2))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 2 Then
                             If input_split.First = "drop" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "object" Then
                                     Return True
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = "HELD"
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There are holding more then one " & input_split(1))
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You are not holding a " & input_split(1))
                                Return False
                            Else
                                Return True
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode("", input_split(1))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = "HELD"
                                     Select c
                         Dim code = query.First

                         ObjectManager.Instance().MoveObj(code, Vars.Instance().alpha.GetValue("CURRENTLOC"))
                         Console.WriteLine("You drop the " & input_split(1))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         Dim input_split = input.Split()
                         If input_split.Count = 3 Then
                             If input_split.First = "drop" Then
                                 If Vocab.Instance.GetWordType(input_split(1)) = "adjective" Then
                                     If Vocab.Instance.GetWordType(input_split(2)) = "object" Then
                                         Return True
                                     End If
                                 End If
                             End If
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            Dim input_split = input.Split()
                            Dim codes As IEnumerable(Of String) = Nothing
                            codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                            Dim query = From c In codes
                                        Let loc As String = ObjectManager.Instance().WhereIs(c)
                                        Where loc = "HELD"
                                        Select c

                            If query.Count > 1 Then
                                Console.WriteLine("There are holding more then one " & input_split(1) & " " & input_split(2))
                                Return False
                            ElseIf query.Count = 0 Then
                                Console.WriteLine("You are not holding a " & input_split(1) & " " & input_split(2))
                                Return False
                            Else
                                Return True
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Dim input_split = input.Split()
                         Dim codes = ObjectManager.Instance().WordsToCode(input_split(1), input_split(2))
                         Dim query = From c In codes
                                     Let loc As String = ObjectManager.Instance().WhereIs(c)
                                     Where loc = "HELD"
                                     Select c
                         Dim code = query.First
                         ObjectManager.Instance().MoveObj(code, Vars.Instance().alpha.GetValue("CURRENTLOC"))
                         Console.WriteLine("You drop the " & input_split(1) & " " & input_split(2))
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "pull rope" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Vars.Instance().alpha.GetValue("CURRENTLOC") = "POND" Then
                                If ObjectManager.Instance().GetStateNode("PONDROPE").Value = "UNPULLED" Then
                                    Return True
                                Else
                                    Console.WriteLine("Nothing happens")
                                    Return False
                                End If
                            ElseIf ObjectManager.Instance().WhereIs("ROPE") = Vars.Instance.alpha.GetValue("CURRENTLOC") Then
                                Console.WriteLine("You can't pull that")
                                Return False
                            ElseIf ObjectManager.Instance().WhereIs("ROPE") = "HELD" Then
                                Console.WriteLine("You can't pull that")
                                Return False
                            ElseIf Vars.Instance().alpha.GetValue("CURRENTLOC") = "FORESTPATH" And ObjectManager.Instance().GetStateNode("BRANCH").Value = "ROPETIED" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            Else
                                Console.WriteLine("There is no rope here")
                                Return False
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("You pull the rope out of the pond. There is a basket attached to it.")
                         ObjectManager.Instance().GetStateNode("PONDROPE").Value = "PULLED"
                         ObjectManager.Instance().MoveObj("BASKET", "POND")
                         ObjectManager.Instance().SetDescription("PONDROPE", "A length of rope. One end is fixed to a rock, the other to a basket")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "unlock door with key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                Console.WriteLine("There is no door here")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("DOOR").Value = "UNLOCKED" Then
                                Console.WriteLine("The door is already unlocked")
                                Return False
                            End If

                            Dim codes = ObjectManager.Instance().WordsToCode("", "key")
                            Dim query = From c In codes
                                        Where ObjectManager.Instance().WhereIs(c) = "HELD"
                                        Select c
                            If query.Count > 1 Then
                                Console.WriteLine("You are holding more than one key")
                                Return False
                            End If
                            If query.Count = 0 Then
                                Console.WriteLine("You are not holding a key")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("KEY") = "HELD" Then
                                Console.WriteLine("You are not holding the right key")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().GetStateNode("DOOR").Value = "UNLOCKED"
                         ObjectManager.Instance().SetDescription("DOOR", "An ordinary looking wooden door. It is unlocked")
                         LocationManager.Instance().UnHideLink("INSIDECOTTAGE", "east")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "unlock door with brass key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                Console.WriteLine("There is no door here")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("DOOR").Value = "UNLOCKED" Then
                                Console.WriteLine("The door is already unlocked")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("KEY") = "HELD" Then
                                Console.WriteLine("You are not holding a brass key")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().GetStateNode("DOOR").Value = "UNLOCKED"
                         ObjectManager.Instance().SetDescription("DOOR", "An ordinary looking wooden door. It is unlocked")
                         LocationManager.Instance().UnHideLink("INSIDECOTTAGE", "east")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "lock door with brass key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                Console.WriteLine("There is no door here")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("DOOR").Value = "LOCKED" Then
                                Console.WriteLine("The door is already locked")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("KEY") = "HELD" Then
                                Console.WriteLine("You are not holding a brass key")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().GetStateNode("DOOR").Value = "LOCKED"
                         ObjectManager.Instance().SetDescription("DOOR", "An ordinary looking wooden door. It is locked")
                         LocationManager.Instance().HideLink("INSIDECOTTAGE", "east")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "lock door with key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                Console.WriteLine("There is no door here")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("DOOR").Value = "LOCKED" Then
                                Console.WriteLine("The door is already locked")
                                Return False
                            End If

                            Dim codes = ObjectManager.Instance().WordsToCode("", "key")
                            Dim query = From c In codes
                                        Where ObjectManager.Instance().WhereIs(c) = "HELD"
                                        Select c
                            If query.Count > 1 Then
                                Console.WriteLine("You are holding more than one key")
                                Return False
                            End If
                            If query.Count = 0 Then
                                Console.WriteLine("You are not holding a key")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("KEY") = "HELD" Then
                                Console.WriteLine("You are not holding the right key")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().GetStateNode("DOOR").Value = "LOCKED"
                         ObjectManager.Instance().SetDescription("DOOR", "An ordinary looking wooden door. It is locked")
                         LocationManager.Instance().HideLink("INSIDECOTTAGE", "east")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "unlock door with red key" Or
                            input = "lock door with red key" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not ObjectManager.Instance().WhereIs("REDKEY") = "HELD" Then
                                Console.WriteLine("You are not holding a red key")
                                Return False
                            End If
                            If Vars.Instance().alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                Console.WriteLine("That is not the right key")
                                Return False
                            Else
                                Console.WriteLine("There is no suitable door here")
                                Return False
                            End If
                        End Function)
        r.SetEffects(Sub(input)
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "put stone on plinth" Or input = "put stone onto plinth" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "ROCK" Then
                                Console.WriteLine("There is no plinth here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("STONE") = "HELD" Then
                                Console.WriteLine("You are not holding a stone")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("A voice says 'Thank you'. A figurine appears.")
                         ObjectManager.Instance().MoveObj("STONE", "NOWHERE")
                         ObjectManager.Instance().SetDescription("PLINTH", "A stone plinth, about two feet high.")
                         ObjectManager.Instance().MoveObj("FIGURINE", "ROCK")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "put feather next to sculpture" Or
                            input = "put feather on sculpture" Or
                            input = "give feather to sculpture" Or
                            input = "give feather to owl" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "CLEARING" Then
                                Console.WriteLine("There are no sculptures here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("FEATHER") = "HELD" Then
                                Console.WriteLine("You are not holding a feather")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("The sculture becomes a real owl and flies away. A piece of rope appears")
                         ObjectManager.Instance().MoveObj("FEATHER", "NOWHERE")
                         ObjectManager.Instance().MoveObj("SCULPTURE", "NOWHERE")
                         ObjectManager.Instance().MoveObj("ROPE", "CLEARING")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "swing across river" Or
                             input = "swing over river" Or
                             input = "swing on rope" Or
                             input = "swing over" Or
                             input = "swing across" Or
                             input = "swing west" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "FORESTPATH" Then
                                Console.WriteLine("You can't do that here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().GetStateNode("BRANCH").Value = "ROPETIED" Then
                                Console.WriteLine("There is nothing to swing over on")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("You swing on the rope across the river to the other side")
                         Vars.Instance().alpha.SetValue("CURRENTLOC", "NARROWPATH")
                         GameIO.DescribeLoc("NARROWPATH")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "tie rope to branch" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "FORESTPATH" Then
                                Console.WriteLine("There is no branch here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("ROPE") = "HELD" Then
                                Console.WriteLine("You are not holding a rope")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("You tie the rope to the branch")
                         ObjectManager.Instance().MoveObj("ROPE", "NOWHERE")
                         ObjectManager.Instance().GetStateNode("BRANCH").Value = "ROPETIED"
                         ObjectManager.Instance().SetDescription("BRANCH", "A branch reaches out from a tree across the river to the west. A rope is tied to it.")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "unlock box with key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "NARROWPATH" Then
                                Console.WriteLine("There is no box here")
                                Return False
                            End If

                            Dim codes = ObjectManager.Instance().WordsToCode("", "key")
                            Dim query = From c In codes
                                        Where ObjectManager.Instance().WhereIs(c) = "HELD"
                                        Select c
                            If query.Count > 1 Then
                                Console.WriteLine("You are holding more than one key")
                                Return False
                            End If
                            If query.Count = 0 Then
                                Console.WriteLine("You are not holding a key")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("REDKEY") = "HELD" Then
                                Console.WriteLine("You are not holding the right key")
                                Return False
                            End If

                            If ObjectManager.Instance().GetStateNode("BOX").Value = "OPEN" Then
                                Console.WriteLine("The box is already unlocked")
                                Return False
                            End If

                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().SetDescription("BOX", "A small wooden box. it is open")
                         ObjectManager.Instance().MoveObj("COIN", "NARROWPATH")
                         ObjectManager.Instance().GetStateNode("BOX").Value = "OPEN"

                         Console.WriteLine("You unlock and open the box. It contains a small coin")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "unlock box with red key" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "NARROWPATH" Then
                                Console.WriteLine("There is no box here")
                                Return False
                            End If

                            If Not ObjectManager.Instance().WhereIs("REDKEY") = "HELD" Then
                                Console.WriteLine("You are not holding a red key")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("BOX").Value = "OPEN" Then
                                Console.WriteLine("The box is already unlocked")
                                Return False
                            End If

                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().SetDescription("BOX", "A small wooden box. it is open")
                         ObjectManager.Instance().MoveObj("COIN", "NARROWPATH")
                         ObjectManager.Instance().GetStateNode("BOX").Value = "OPEN"

                         Console.WriteLine("You unlock and open the box. It contains a small coin")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "throw coin in fountain" Or
                            input = "throw coin into fountain" Or
                            input = "put coin into fountain" Or
                            input = "put coin in fountain" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "FOUNTAIN" Then
                                Console.WriteLine("There is no fountain here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("COIN") = "HELD" Then
                                Console.WriteLine("You are not holding a coin")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().MoveObj("COIN", "NOWHERE")
                         ObjectManager.Instance().MoveObj("BRACELET", "FOUNTAIN")
                         Console.WriteLine("You throw the coin into a the fountain. It vanishes when it enters the water, and a bracelet appears.")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "open gates" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Vars.Instance().alpha.GetValue("CURRENTLOC") = "GATE" Then
                                If ObjectManager.Instance().GetStateNode("GATES") = "OPEN" Then
                                    Console.WriteLine("The gates are already open")
                                    Return False
                                Else
                                    Console.WriteLine("You can't")
                                End If
                            Else
                                Console.WriteLine("There are no gates here")
                            End If
                            Return False
                        End Function)
        r.SetEffects(Sub(input)
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "say open please" Then
                             Return True
                         Else
                             Return False
                         End If
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "GATE" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            If ObjectManager.Instance().GetStateNode("GATES") = "OPEN" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().GetStateNode("GATES").Value = "OPEN"
                         LocationManager.Instance().UnHideLink("GATE", "north")
                         ObjectManager.Instance().SetDescription("GATES", "The two iron gates are open. A sign attached reads 'Thank you for being polite'")
                         Console.WriteLine("The gates open")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "nail horseshoe to door" Or
                             input = "nail horseshoe above door" Or
                             input = "fix horseshoe to door with hammer" Or
                             input = "fix horseshoe to door" Or
                             input = "fix horseshoe above door with hammer" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not ObjectManager.Instance().WhereIs("HORSESHOE") = "HELD" Then
                                Console.WriteLine("You are not holding a horseshoe")
                                Return False
                            End If
                            If Not Vars.Instance.alpha.GetValue("CURRENTLOC") = "KITCHENGARDEN" Then
                                Console.WriteLine("You can't do that here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("HAMMER") = "HELD" Then
                                Console.WriteLine("You need a hammer")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("NAILS") = "HELD" Then
                                Console.WriteLine("You need some nails")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         ObjectManager.Instance().MoveObj("HORSESHOE", "NOWHERE")
                         ObjectManager.Instance().GetStateNode("KITCHENDOOR").Value = "UNLOCKED"
                         LocationManager.Instance().UnHideLink("KITCHENGARDEN", "in")
                         ObjectManager.Instance().SetDescription("KITCHENDOOR", "This is the door leading to the kitchen. A sign reads 'You can get in if you are lucky'. There is a horseshoe nailed to the door.")
                         Console.WriteLine("The door opens!")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "shake stick" Or
                             input = "shake stick at painting" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not ObjectManager.Instance().WhereIs("STICK") = "HELD" Then
                                Console.WriteLine("You are not holding a stick")
                                Return False
                            End If
                            If input = "shake stick" And Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "GALLERY" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            If Vars.Instance().alpha.GetValue("CURRENTLOC") = "LIBRARY" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "GALLERY" Then
                                Console.WriteLine("There is no painting here")
                                Return False
                            End If
                            If Not ObjectManager.Instance().GetStateNode("PAINTING").Value = "CLOSED" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("The painting opens revealing a corridor")
                         ObjectManager.Instance().GetStateNode("PAINTING").Value = "OPEN"
                         ObjectManager.Instance().SetDescription("PAINTING", "A well executed painting of a rosebush. Next to it, a sign reads 'Here is a rosebush with more flowers than you can shake a stick at'. The picture has swung back revealing a corridor")
                         LocationManager.Instance().UnHideLink("GALLERY", "north")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "sing" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "GROTTO" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            If Not ObjectManager.Instance().WhereIs("CRYSTAL") = "NOWHERE" Then
                                Console.WriteLine("Nothing happens")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("A crystal appears")
                         ObjectManager.Instance().MoveObj("CRYSTAL", "GROTTO")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "throw ball at painting" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not ObjectManager.Instance.WhereIs("BALL") = "HELD" Then
                                Console.WriteLine("You are not holding a ball")
                                Return False
                            End If
                            If Vars.Instance().alpha.GetValue("CURRENTLOC") = "GALLERY" Then
                                Console.WriteLine("Nothing happens")
                                ObjectManager.Instance().MoveObj("BALL", "GALLERY")
                                Return False
                            End If
                            If Not Vars.Instance().alpha.GetValue("CURRENTLOC") = "LIBRARY" Then
                                Console.WriteLine("There is no painting here")
                                Return False
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(input)
                         Console.WriteLine("The ball disappears into the painting, where it becomes a painted white ball, and strikes and pockets the black ball. The painting slides back revealing shelves")
                         ObjectManager.Instance().MoveObj("BALL", "NOWHERE")
                         ObjectManager.Instance().MoveObj("SNOOKERPAINTING", "NOWHERE")
                         ObjectManager.Instance().MoveObj("LIBRARYSHELVES", "LIBRARY")
                     End Sub)
        ruleList.Add(r)

        r = New Rule
        r.SetTrigger(Function(input)
                         If input = "open door" Then
                             Return True
                         End If
                         Return False
                     End Function)
        r.SetConditions(Function(input)
                            If Not (Vars.Instance().alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Or
                                Vars.Instance().alpha.GetValue("CURRENTLOC") = "KITCHENGARDEN") Then
                                Console.WriteLine("There is no door here")
                                Return False
                            End If
                            If Vars.Instance.alpha.GetValue("CURRENTLOC") = "INSIDECOTTAGE" Then
                                If ObjectManager.Instance().GetStateNode("DOOR").Value = "LOCKED" Then
                                    Console.WriteLine("It is locked")
                                    Return False
                                End If
                            End If
                            If Vars.Instance.alpha.GetValue("CURRENTLOC") = "KITCHENGARDEN" Then
                                If ObjectManager.Instance().GetStateNode("KITCHENDOOR").Value = "LOCKED" Then
                                    Console.WriteLine("It is locked")
                                    Return False
                                End If
                            End If
                            Return True
                        End Function)
        r.SetEffects(Sub(Input)
                         Console.WriteLine("It is already open")
                     End Sub)
        ruleList.Add(r)

    End Sub

    Public Function ApplyRules(input As String) As Boolean
        For Each rule In ruleList
            If rule.trigger(input) Then
                If rule.conditions(input) Then
                    rule.effects(input)
                    Console.WriteLine("OK")
                End If
                Return True
            End If
        Next
        Return False
    End Function

End Class
