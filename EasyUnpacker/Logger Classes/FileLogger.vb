Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO

Namespace Logger
    Class FileLogger
        Implements ILogger
        Private m_strFileName As String
        Private m_sync As New Object()
        Public Sub New(strFileName As String)
            m_strFileName = strFileName
        End Sub
        Public Sub LogMessage(strMessage As String) Implements ILogger.LogMessage
            SyncLock m_sync
                Using writer As New StreamWriter(m_strFileName)
                    writer.WriteLine(strMessage)
                End Using
            End SyncLock
        End Sub
    End Class
End Namespace
