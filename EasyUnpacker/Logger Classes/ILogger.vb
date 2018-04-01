Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Logger
    Public Interface ILogger
        Sub LogMessage(strMessage As String)
    End Interface
End Namespace
