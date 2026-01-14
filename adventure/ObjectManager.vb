Public Class ObjectManager

    Private Shared m_instance As ObjectManager = Nothing
    Private doc As XDocument = Nothing

    Public Shared Function Instance() As ObjectManager
        If m_instance Is Nothing Then
            m_instance = New ObjectManager
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Sub LoadXML(filename As String)
        doc = XDocument.Load(filename)
    End Sub

    Public Function ObjectsInPlace(where_code As String) As List(Of String)
        Dim query = From element In doc.Descendants("Object")
                    Where element.Attribute("where").Value = where_code
                    Select element.Attribute("code").Value

        Dim result As New List(Of String)

        For Each e In query
            result.Add(e)
        Next

        Return result

    End Function

    Public Function CodeToTitle(code As String) As String
        Dim query = From element In doc.Descendants("Object")
                    Where element.Attribute("code").Value = code
                    Select element.Attribute("title").Value
        Return query.First
    End Function

    'Public Function WordToCode(word As String) As String
    'Dim query =    From e In doc.Descendants("Object")
    '               Where e.Attribute("word").Value = word
    '               Select e.Attribute("code").Value
    'Return query.First
    'End Function

    Public Function WordsToCode(adjective As String, word As String) As IEnumerable(Of String)
        If adjective = "" Then
            Dim query = From e In doc.Descendants("Object")
                        Where e.Attribute("word").Value = word
                        Select e.Attribute("code").Value
            Return query
        Else
            Dim query1 = From e In doc.Descendants("Object")
                         Where e.Attribute("word").Value = word
                         Select e
            'Dim query2 = From e In query1
            'Where e.Attribute("adjective").Value <> ""
            'Where e.Attribute("adjective") = ""
            'Select Case e
            Dim query2 = From e In query1
                         Where Not e.Attribute("adjective") Is Nothing
                         Select e
            Dim query3 = From e In query2
                         Where e.Attribute("adjective").Value = adjective
                         Select e.Attribute("code").Value
            Return query3
        End If
    End Function

    Public Function WhereIs(code As String) As String
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e.Attribute("where").Value
        Return query.First
    End Function

    Public Function GetDescription(code As String) As String
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e.Attribute("description").Value
        Return query.First
    End Function

    Public Sub SetDescription(code As String, newDescription As String)
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e.Attribute("description")
        query.First.Value = newDescription
    End Sub

    Public Sub MoveObj(code As String, where As String)
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e
        query.First.Attribute("where").Value = where
    End Sub

    Public Function IsFixture(code As String) As Boolean
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e
        Dim f = query.First.Attributes("fixture")
        If f.Count > 0 Then
            If f.First.Value = "TRUE" Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetStateNode(code As String) As XAttribute
        Dim query = From e In doc.Descendants("Object")
                    Where e.Attribute("code").Value = code
                    Select e
        Return query.First.Attribute("state")
    End Function


End Class
