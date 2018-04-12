Imports System.Xml.Serialization
Imports System.IO
Imports EasyUnpacker.Logger
Imports EasyUnpacker.ErrorLogger

''' <summary>
''' Class for reading and writing XML style application settings.
''' </summary>
Public Class Preferences

    Private Shared ReadOnly m_instance As New Preferences()

    Public Shared ReadOnly Property Instance() As Preferences
        Get
            Return m_instance
        End Get
    End Property

#Region "Local Variables"
    'Standartlanguage of the Program
    Public Property Language() As String
        Get
            Return m_Language
        End Get
        Set(ByVal value As String)
            m_Language = value
        End Set
    End Property
    Private m_Language As String

    'Form X-Position
    Public Property FormLocationX As String
        Get
            Return m_FormLocationX
        End Get
        Set(value As String)
            m_FormLocationX = value
        End Set
    End Property
    Private m_FormLocationX As String

    'Form Y-Position
    Public Property FormLocationY As String
        Get
            Return m_FormLocationY
        End Get
        Set(value As String)
            m_FormLocationY = value
        End Set
    End Property
    Private m_FormLocationY As String

    'Form Width
    'Public Property FormWidth As String
    'Get
    'Return m_FormWidth
    'End Get
    'Set(value As String)
    '       m_FormWidth = value
    'End Set
    'End Property
    'Private m_FormWidth As String

    'Form Height
    'Public Property FormHeight As String
    'Get
    'Return m_FormHeight
    'End Get
    'Set(value As String)
    '       m_FormHeight = value
    'End Set
    'End Property
    'Private m_FormHeight As String

    'Formstate -> Maximized, Normal, etc.
    'Public Property WindowState As String
    'Get
    'Return m_WindowState
    'End Get
    'Set(value As String)
    '        m_WindowState = value
    'End Set
    'End Property
    'Private m_WindowState As String

    Public Property ExtractSame As Boolean
        Get
            Return m_ExtractSame
        End Get
        Set(value As Boolean)
            m_ExtractSame = value
        End Set
    End Property
    Private m_ExtractSame As Boolean

    Public Property ExtractFullPath As Boolean
        Get
            Return m_ExtractFullPath
        End Get
        Set(value As Boolean)
            m_ExtractFullPath = value
        End Set
    End Property
    Private m_ExtractFullPath As Boolean

    Public Property Overwrite As Boolean
        Get
            Return m_Overwrite
        End Get
        Set(value As Boolean)
            m_Overwrite = value
        End Set
    End Property
    Private m_Overwrite As Boolean

    'Last Source Archive Path(s)
    Property LastSourcePath() As List(Of String)
        Get
            Return m_LastSourcePath
        End Get
        Set(value As List(Of String))
            m_LastSourcePath = value
        End Set
    End Property
    Private m_LastSourcePath As List(Of String)

    'Last Extraction Path
    Public Property LastDestPath As String
        Get
            Return m_LastDestPath
        End Get
        Set(value As String)
            m_LastDestPath = value
        End Set
    End Property
    Private m_LastDestPath As String

    Private Property Preferences() As Preferences
#End Region

#Region "Private methods"

    Private Shared IsLoggerLoaded As Boolean = False

    'Standart Filename for Preferences, can be changed :)
    Private Shared ReadOnly DefaultFilename As String = Application.StartupPath & "\Preferences.xml"

    Public Shared Logger As Logger.ILogger
    Public Shared ErrLog As ErrorLogger

    <XmlIgnore()> _
    Public DeserializedWithErrors As Boolean = False

    'Initiate all Settings
    Public Sub New()
        Language = "de-German"
        FormLocationX = 275
        FormLocationY = 180
        'FormWidth = 1275
        'FormHeight = 715
        'WindowState = "Normal"
        LastSourcePath = New List(Of String)
        LastDestPath = Application.StartupPath
        ExtractSame = False
        ExtractFullPath = False
        Overwrite = False
    End Sub

    Public Function GetItem(ByVal name As String) As String
        Dim doc As New System.Xml.XmlDocument
        doc.Load(DefaultFilename)
        Dim list = doc.GetElementsByTagName(name)

        For Each item As System.Xml.XmlElement In list
            Return item.InnerText
        Next
        Return Nothing
    End Function

    'XML speichern (serializieren)
    Public Function SavePreferences(options As Preferences)
        Try
            Dim serializer As New XmlSerializer(GetType(Preferences))
            Dim writer As TextWriter = New StreamWriter(DefaultFilename, False, System.Text.Encoding.UTF8)
            serializer.Serialize(writer, options)
            writer.Close()
        Catch ex As Exception
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException.InnerException)
            Return False
        End Try
        Mainform.PrefsSaved = True
        Return True
    End Function

    'XML laden (deserializieren)
    Public Shared Function LoadPreferences(Optional _logger As ILogger = Nothing, Optional _errlogger As ErrorLogger = Nothing) As Preferences
        If IsLoggerLoaded Then
            Dim serializer As New XmlSerializer(GetType(Preferences))
            AddHandler serializer.UnknownAttribute, New XmlAttributeEventHandler(AddressOf serializer_UnknownAttribute)
            AddHandler serializer.UnknownElement, New XmlElementEventHandler(AddressOf serializer_UnknownElement)
            AddHandler serializer.UnknownNode, New XmlNodeEventHandler(AddressOf serializer_UnknownNode)

            Dim preferences As Preferences = Nothing
            Try
                If File.Exists(DefaultFilename) Then
                    Dim reader As TextReader = New StreamReader(DefaultFilename)
                    preferences = DirectCast(serializer.Deserialize(reader), Preferences)
                    reader.Close()
                    Return preferences
                End If
                Logger.LogMessage("Preferences.xml not found, using standart settings")
            Catch ex As FileNotFoundException
                Mainform.WriteErrorLogs(ErrorLogger.LogsCategory.Exception, ex)
            End Try
            Return Nothing
        Else
            Logger = _logger
            ErrLog = _errlogger
            IsLoggerLoaded = True

            Dim serializer As New XmlSerializer(GetType(Preferences))
            AddHandler serializer.UnknownAttribute, New XmlAttributeEventHandler(AddressOf Serializer_UnknownAttribute)
            AddHandler serializer.UnknownElement, New XmlElementEventHandler(AddressOf Serializer_UnknownElement)
            AddHandler serializer.UnknownNode, New XmlNodeEventHandler(AddressOf Serializer_UnknownNode)

            Dim preferences As Preferences = Nothing
            Try
                If File.Exists(DefaultFilename) Then
                    Try
                        Dim reader As TextReader = New StreamReader(DefaultFilename)
                        preferences = DirectCast(serializer.Deserialize(reader), Preferences)
                        reader.Close()
                        Return preferences
                    Catch ex As Exception
                        ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException)
                    End Try

                End If
                Logger.LogMessage("Preferences.xml not found, using standart settings")
            Catch ex As FileNotFoundException
                ErrLog.WriteErrorLogs(ErrorLogger.LogsCategory.Exception, ex)
            End Try
            Return Nothing
        End If
    End Function

    Private Shared Sub Serializer_UnknownNode(sender As Object, e As XmlNodeEventArgs)
        DirectCast(e.ObjectBeingDeserialized, Preferences).DeserializedWithErrors = True
    End Sub

    Private Shared Sub Serializer_UnknownElement(sender As Object, e As XmlElementEventArgs)
        DirectCast(e.ObjectBeingDeserialized, Preferences).DeserializedWithErrors = True
    End Sub

    Private Shared Sub Serializer_UnknownAttribute(sender As Object, e As XmlAttributeEventArgs)
        DirectCast(e.ObjectBeingDeserialized, Preferences).DeserializedWithErrors = True
    End Sub
#End Region
End Class
