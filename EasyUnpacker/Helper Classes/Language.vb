Imports System.IO

Public Class Language
    Public Shared _AllLang As Hashtable
    Private Shared htStrings As New Hashtable
    Private Const msREQUIREDLANGUAGEVERSION As String = "1.0.0"
    Public Startup As String = Application.StartupPath
    Public LangPath As String = Startup + "\Langs"
    Public LangEditor As String = ""

    Protected Friend Function GetSupportedLanguages() As Hashtable
        Dim di As New DirectoryInfo(Application.StartupPath & "\Langs")
        Dim aryFi As IO.FileInfo() = di.GetFiles("*.xml", SearchOption.TopDirectoryOnly)
        _AllLang = New Hashtable
        For Each item In aryFi
            Dim name As String = Path.GetFileNameWithoutExtension(item.FullName)
            _AllLang.Add(name, item)
        Next
        Return _AllLang
    End Function

    Protected Friend Function GetString(ByVal ID As Integer, ByVal strDefault As String) As String
        If IsNothing(htStrings) Then
            Return strDefault
        End If
        If htStrings.ContainsKey(ID) Then
            Return htStrings.Item(ID).ToString
        Else
            Return strDefault
        End If
    End Function

    Public Function GetFullLangName(ByVal language As String)
        For Each item In _AllLang.Keys
            If item.Contains(language) Then
                Return item
            End If
        Next
        Return Nothing
    End Function

    Private Function ReadXMLComment(ByVal node As XDocument)
        Dim _node = node.Nodes().OfType(Of XComment)()
        Return _node(0).Value.ToString
    End Function

    Public Sub LoadLanguage(ByVal Language As String)
        Try
            Dim lPath As String = String.Empty

            If Not String.IsNullOrEmpty(Language) Then
                lPath = String.Concat(Application.StartupPath, "\Langs", Path.DirectorySeparatorChar, GetFullLangName(Language), ".xml")
            End If

            htStrings = New Hashtable
            htStrings.Clear()

            If File.Exists(lPath) Then
                Dim tReader As New StreamReader(lPath, System.Text.Encoding.Default)

                Dim LangXML As XDocument = XDocument.Load(tReader)
                Dim xLanguage = From xLang In LangXML...<strings>...<string> Select xLang.@id, xLang.Value

                If xLanguage.Count > 0 Then
                    For i As Integer = 0 To xLanguage.Count - 1
                        htStrings.Add(Convert.ToInt32(xLanguage(i).id), xLanguage(i).Value)
                    Next
                End If
                LangEditor = ReadXMLComment(LangXML)
            End If

            CheckLangfileVersion()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckLangfileVersion()
        Dim sVersion As String
        Dim sMsg As String

        sVersion = GetString(0, Nothing)
        sMsg = GetString(47, Nothing) & GetString(48, Nothing) & GetString(49, Nothing)
        sMsg = Replace(sMsg, "%ACTVER%", sVersion)
        sMsg = Replace(sMsg, "%REQVER%", msREQUIREDLANGUAGEVERSION)
        sMsg = Replace(sMsg, "\n", vbCrLf)

        If VersionValue(msREQUIREDLANGUAGEVERSION) > VersionValue(sVersion) Then
            If vbNo = MsgBox(sMsg, vbYesNo Or vbQuestion) Then
                End 'MD-Marker
            End If

        End If
    End Sub

    Protected Friend Function VersionValue(ByVal sVersion As String) As Long
        On Error Resume Next
        Dim lMajor As Integer
        Dim lMinor As Integer
        Dim lRevision As Integer
        Dim sBeta As String
        Dim lPos1 As Integer
        Dim lPos2 As Integer

        lPos1 = InStr(1, sVersion, ".")
        If lPos1 > 0 Then
            lMajor = Val(Mid(sVersion, 1, lPos1 - 1))

            lPos2 = InStr(lPos1 + 1, sVersion, ".")
            If lPos2 > 0 Then
                lMinor = Val(Mid(sVersion, lPos1 + 1, lPos2 - lPos1))
                lRevision = Val(Mid(sVersion, lPos2 + 1))
                sBeta = Mid(sVersion, lPos2 + Len(CStr(lRevision)) + 1)
            Else
                lMinor = Val(Mid(sVersion, lPos1 + 1))
            End If
        Else
            lMajor = Val(sVersion)
        End If

        VersionValue = ((lMajor * 1000000) + (lMinor * 1000)) + lRevision
    End Function
End Class
