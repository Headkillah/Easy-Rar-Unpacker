Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics

Namespace Logger
    Class TraceWindowLogger
        Implements ILogger
        Public Sub New()
        End Sub
        Public Sub LogMessage(strMessage As String) Implements ILogger.LogMessage
            Trace.WriteLine(strMessage)
        End Sub
    End Class
End Namespace
