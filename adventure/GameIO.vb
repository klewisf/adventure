Public Class GameIO

    Private Shared m_instance As GameIO = Nothing

    Public Shared Function Instance() As GameIO
        If m_instance Is Nothing Then
            m_instance = New GameIO
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Shared Sub DescribeLoc(code As String)
        Dim location As XElement = LocationManager.Instance().GetLocation(code)
        Console.WriteLine(location.Attribute("title").Value)
        Console.WriteLine(location.Attribute("description").Value)
        Console.WriteLine("Exits are:")
        For Each ex In location.Descendants("Link")
            If Not LocationManager.Instance().LinkIsHidden(code, ex.Attribute("command").Value) Then
                Console.Write("    " & ex.Attribute("command").Value)
                Console.Write(" to ")
                Console.WriteLine(LocationManager.Instance().GetLocation(ex.Attribute("linkto")).Attribute("title").Value)
                'Else
                'Console.WriteLine("Hidden Exit")
            End If
        Next

        Dim objects = ObjectManager.Instance().ObjectsInPlace(code)
        If objects.Count > 0 Then
            Console.WriteLine("Here is:")
            For Each obj In objects
                Console.WriteLine("    " & ObjectManager.Instance().CodeToTitle(obj))
            Next
        End If


    End Sub

    Public Shared Function GetCommand() As String
        Dim wordsknown As Boolean = False
        Dim wordarray As String() = Nothing
        Dim input As String = Nothing

        Do Until wordsknown

            Console.Write("> ")
            input = Console.ReadLine()
            wordarray = input.Split()
            wordsknown = True

            For Each word In wordarray
                If Not Vocab.Instance.IsWord(word) Then
                    Console.WriteLine("Word " & word & " is not known")
                    wordsknown = False
                End If
            Next

        Loop

        Return input

    End Function

End Class
