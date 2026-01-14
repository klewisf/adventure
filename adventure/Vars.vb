Imports System.Reflection.Metadata
Imports System.Threading

Public Class Vars

    Private Shared m_instance As Vars = Nothing
    Private doc As XDocument = Nothing

    Public alpha As AlphaValue = New AlphaValue
    Public bool As BoolValue = New BoolValue
    Public number As NumberValue = New NumberValue
    Public Class AlphaValue
        Public Function GetValue(name As String) As String

            Dim v = Vars.Instance().doc.Descendants("Alpha").First.Descendants("Var")
            Dim q = From e In v
                    Where e.Attribute("name").Value = name
                    Select e

            Return q.First.Attribute("value")

        End Function

        Public Sub SetValue(name As String, value As String)
            Dim query = From e In Vars.Instance.doc.Descendants("Alpha").First.Descendants("Var")
                        Where e.Attribute("name").Value = name
                        Select e

            query.First.Attribute("value").Value = value
        End Sub

    End Class

    Public Class BoolValue

        Public Function GetValue(name As String) As Boolean

            Dim query = From e In Vars.Instance().doc.Descendants("Boolean").First.Descendants("Var")
                        Where e.Attribute("name").Value = name
                        Select e
            'Console.WriteLine(query.First.Attribute("value").Value)

            If query.First.Attribute("value").Value = "true" Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub SetValue(name As String, value As Boolean)

            Dim query = From e In Vars.Instance().doc.Descendants("Boolean").First.Descendants("Var")
                        Where e.Attribute("name").Value = name
                        Select e
            If value Then
                'Console.WriteLine("set to true")
                query.First.Attribute("value").Value = "true"
            Else
                'Console.WriteLine("set to false")
                query.First.Attribute("value").Value = "false"
            End If

        End Sub

    End Class

    Public Class NumberValue
        Public Function GetValue(name As String) As Integer

            Dim query = From e In Vars.Instance().doc.Descendants("Number").First.Descendants("Var")
                        Where e.Attribute("name").Value = name
                        Select e

            Dim result = CInt(query.First.Attribute("value").Value)
            Return result

        End Function

        Public Sub SetValue(name As String, value As Integer)

            Dim query = From e In Vars.Instance().doc.Descendants("Number").First.Descendants("Var")
                        Where e.Attribute("name").Value = name
                        Select e

            query.First.Attribute("value").Value = CStr(value)

        End Sub

    End Class

    Public Shared Function Instance() As Vars
        If m_instance Is Nothing Then
            m_instance = New Vars
            Return m_instance
        Else
            Return m_instance
        End If
    End Function

    Public Sub LoadXML(filename As String)
        doc = XDocument.Load(filename)
    End Sub

End Class
