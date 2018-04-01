Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms

Namespace Logger
    Class TextBoxLogger
        Implements ILogger
        Dim logDelegate As MethodInvoker
        Private m_multiline As Boolean
        Private m_textBox As TextBox
        Public Sub New(txtBox As TextBox, multiline As Boolean)
            m_textBox = txtBox
            m_multiline = multiline
        End Sub
        Public Sub LogMessage(strLogMessage As String) Implements ILogger.LogMessage
            If m_textBox.InvokeRequired Then
                If m_multiline Then
                    m_textBox.BeginInvoke(New MethodInvoker(Sub()
                                                                m_textBox.AppendText(strLogMessage & vbCrLf)
                                                            End Sub))
                Else
                    m_textBox.BeginInvoke(New MethodInvoker(Sub()
                                                                m_textBox.Text = strLogMessage
                                                            End Sub))
                End If

            Else
                If m_multiline Then
                    m_textBox.AppendText(strLogMessage)
                Else
                    m_textBox.Text = strLogMessage
                End If

            End If
        End Sub
    End Class
End Namespace
