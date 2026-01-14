Public Class LocationManager
    Private Shared m_instance As LocationManager = Nothing
    Private doc As XDocument = Nothing

    Public Shared Function Instance() As LocationManager
        If m_instance Is Nothing Then
            m_instance = New LocationManager
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Sub LoadXML(filename As String)
        doc = XDocument.Load(filename)
    End Sub

    Public Function GetLocation(code As String) As XElement
        Dim query = From element In doc.Descendants("Location")
                    Where element.Attribute("code").Value = code
                    Select element
        Return query.First
    End Function

    Public Function LinkIsHidden(location As String, command As String) As Boolean
        Dim links = GetLocation(location).Descendants("Link")
        Dim query = From e In links
                    Where e.Attribute("command").Value = command
                    Select e
        Dim hidden = query.First.Attributes("hidden")
        If hidden.Count > 0 Then
            If hidden.First.Value = "TRUE" Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Sub HideLink(location As String, command As String)
        Dim links = GetLocation(location).Descendants("Link")
        Dim query = From e In links
                    Where e.Attribute("command").Value = command
                    Select e
        Dim hidden = query.First.Attributes("hidden")
        hidden.First.Value = "TRUE"
    End Sub
    Public Sub UnHideLink(location As String, command As String)
        Dim links = GetLocation(location).Descendants("Link")
        Dim query = From e In links
                    Where e.Attribute("command").Value = command
                    Select e
        Dim hidden = query.First.Attributes("hidden")
        hidden.First.Value = "FALSE"
    End Sub

End Class
