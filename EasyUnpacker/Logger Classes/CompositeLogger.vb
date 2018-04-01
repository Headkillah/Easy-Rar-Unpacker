Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Logger
    ' Logger of composite of loggers
    Class CompositeLogger
        Implements ILogger
        Private m_loggerArray As ILogger()
        ' pass a ILoggers that are part of this composite logger
        Public Sub New(ParamArray loggers As ILogger())
            m_loggerArray = loggers
        End Sub
        Public Sub LogMessage(strMessage As String) Implements ILogger.LogMessage
            ' loop around all the loggers, and log the message.
            For Each logger As ILogger In m_loggerArray
                logger.LogMessage(strMessage)
            Next
        End Sub
    End Class
End Namespace
