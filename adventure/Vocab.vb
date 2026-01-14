Public Class Vocab

    Private Shared m_instance As Vocab = Nothing
    Private doc As XDocument = Nothing
    Private words As List(Of String)
    Public Shared Function Instance() As Vocab
        If m_instance Is Nothing Then
            m_instance = New Vocab
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Sub LoadXML(filename As String)
        doc = XDocument.Load(filename)
        words = New List(Of String)
        For Each element As XElement In doc.Descendants("Word")
            words.Add(element.Value)
        Next
    End Sub

    Public Function IsWord(word As String) As Boolean
        '        Return words.Contains(word)
        Dim query = From element In doc.Descendants("Word")
                    Where element.Attribute("word").Value = word
                    Select element

        If query.Count = 1 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function IsDirection(word As String) As Boolean
        Dim query = From element In doc.Descendants("Word")
                    Where element.Attribute("word").Value = word
                    Select element

        If query.First.Attribute("type").Value = "direction" Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetWordType(word As String) As String
        Dim query = From element In doc.Descendants("Word")
                    Where element.Attribute("word").Value = word
                    Select element

        If query.Count = 0 Then
            Return Nothing
        Else
            Return query.First.Attribute("type").Value
        End If

    End Function

End Class
