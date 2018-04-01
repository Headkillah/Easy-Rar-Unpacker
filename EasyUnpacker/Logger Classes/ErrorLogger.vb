Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml

Public Class ErrorLogger
    Private Shared m_instance As ErrorLogger
    Public Shared ReadOnly Property Instance() As ErrorLogger
        Get
            If m_instance Is Nothing Then
                m_instance = New ErrorLogger()
            End If
            Return m_instance
        End Get
    End Property

    Public Enum LogsCategory
        Debug = 1
        Exception = 2
        Message = 3
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LogsCat"></param>
    ''' <param name="ex"></param>
    Public Sub WriteErrorLogs(ByVal LogsCat As LogsCategory, ByVal ex As Exception)
        Dim st As New StackTrace()
        Dim frame As StackFrame = st.GetFrame(2)

        Dim category As String = GetLogsCategory(LogsCat)
        Dim methodName As String = [String].Format("[{0}.{1}]", frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name)
        Dim logMessage As String = [String].Format("{0}", ex.Message)
        Dim stackTrace As String = [String].Format("{0}", ex.StackTrace)
        Dim [date] As String = [String].Format("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToShortTimeString())

        ' Adds the message to the Form Logs for display
        'LogViewer.AddMessage(category, [date], methodName, logMessage, stackTrace)

        ' Writes the Logs to the XML file
        Write2XML(category, methodName, logMessage, stackTrace, [date])
    End Sub

    Public Sub WriteGlobalErrorLogs(ByVal LogsCat As ErrorLogger.LogsCategory, ByVal ex As Exception)
        ErrorLogger.Instance.WriteErrorLogs(LogsCat, ex)
    End Sub

    Private Function GetLogsCategory(ByVal LogsCat As LogsCategory) As String
        Select Case Convert.ToInt32(LogsCat)
            Case 1
                Return "DEBUG"
            Case 2
                Return "EXCEPTION"
            Case 3
                Return "MESSAGE"
            Case Else
                Return "UNKNOWN"
        End Select
    End Function


    Private Sub Write2XML(ByVal category As String, ByVal method As String, ByVal message As String, ByVal stackTrace As String, ByVal [date] As String)

        Dim xmldoc As XmlDocument
        Dim appPath As String = Application.StartupPath & "\"
        Dim fullPath As String = "ErrorLogs.xml"

        Try
            Dim fs As New FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            xmldoc = New XmlDocument()
            xmldoc.Load(fs)

            Dim xmlnode As XmlNode = xmldoc.LastChild.LastChild
            Dim xmlattr As XmlAttributeCollection = xmlnode.Attributes
            Dim lastErrorId As Integer = Convert.ToInt32(xmlattr(0).Value)
            Dim currentErrorId As Integer = System.Threading.Interlocked.Increment(lastErrorId)


            Dim newcatalogentry As XmlElement = xmldoc.CreateElement("Error")
            Dim newcatalogattr As XmlAttribute = xmldoc.CreateAttribute("ID")
            newcatalogattr.Value = currentErrorId.ToString()
            newcatalogentry.SetAttributeNode(newcatalogattr)

            Dim xmlelem2 As XmlElement = xmldoc.CreateElement("", "Category", "")
            xmlelem2.InnerText = category
            newcatalogentry.AppendChild(xmlelem2)

            Dim xmlelem3 As XmlElement = xmldoc.CreateElement("", "Date", "")
            xmlelem3.InnerText = [date]
            newcatalogentry.AppendChild(xmlelem3)

            Dim xmlelem4 As XmlElement = xmldoc.CreateElement("", "Method", "")
            xmlelem4.InnerText = method
            newcatalogentry.AppendChild(xmlelem4)

            Dim xmlelem5 As XmlElement = xmldoc.CreateElement("", "Message", "")
            xmlelem5.InnerText = message
            newcatalogentry.AppendChild(xmlelem5)

            Dim xmlelem6 As XmlElement = xmldoc.CreateElement("", "StackTrace", "")
            xmlelem6.InnerText = stackTrace
            newcatalogentry.AppendChild(xmlelem6)

            xmldoc.DocumentElement.InsertAfter(newcatalogentry, xmldoc.DocumentElement.LastChild)

            Dim fsxml As New FileStream(fullPath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite)
            xmldoc.Save(fsxml)
        Catch generatedExceptionName As FileNotFoundException
            CreateNewLogFile(category, [date], method, message, stackTrace, fullPath)
        End Try
    End Sub

    Private Sub CreateNewLogFile(ByVal category As String, ByVal [date] As String, ByVal method As String, ByVal message As String, ByVal stackTrace As String, ByVal fileName As String)
        Dim newLogFile As New XmlDocument()
        Dim xmlnode As XmlNode = newLogFile.CreateNode(XmlNodeType.XmlDeclaration, "", "")
        newLogFile.AppendChild(xmlnode)

        Dim xmlRootElem As XmlElement = newLogFile.CreateElement("", "Errors", "")
        newLogFile.AppendChild(xmlRootElem)

        Dim xmlelem As XmlElement = newLogFile.CreateElement("", "Error", "")
        Dim xmlattr As XmlAttribute = newLogFile.CreateAttribute("ID")
        xmlattr.Value = "1"
        xmlelem.SetAttributeNode(xmlattr)
        newLogFile.ChildNodes.Item(1).AppendChild(xmlelem)

        Dim xmlelem2 As XmlElement = newLogFile.CreateElement("", "Category", "")
        xmlelem2.InnerText = category
        xmlelem.AppendChild(xmlelem2)

        Dim xmlelem3 As XmlElement = newLogFile.CreateElement("", "Date", "")
        xmlelem2.InnerText = [date]
        xmlelem.AppendChild(xmlelem3)

        Dim xmlelem4 As XmlElement = newLogFile.CreateElement("", "Method", "")
        xmlelem2.InnerText = method
        xmlelem.AppendChild(xmlelem4)

        Dim xmlelem5 As XmlElement = newLogFile.CreateElement("", "Message", "")
        xmlelem2.InnerText = message
        xmlelem.AppendChild(xmlelem5)

        Dim xmlelem6 As XmlElement = newLogFile.CreateElement("", "StackTrace", "")
        xmlelem2.InnerText = stackTrace
        xmlelem.AppendChild(xmlelem6)

        newLogFile.Save(fileName)

    End Sub
End Class