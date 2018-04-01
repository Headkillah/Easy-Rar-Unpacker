Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms

Namespace Logger
    Class ListBoxLogger
        Implements ILogger
        Dim logDelegate As MethodInvoker
        Private m_listBox As ListBox
        Public Sub New(listBox As ListBox)
            m_listBox = listBox
        End Sub
        Public Sub LogMessage(strMessage As String) Implements ILogger.LogMessage
            If m_listBox.InvokeRequired Then
                m_listBox.BeginInvoke(New MethodInvoker(Sub()
                                                            m_listBox.Items.Add(strMessage)
                                                            m_listBox.TopIndex = m_listBox.Items.Count - 1
                                                        End Sub))
            Else
                m_listBox.Items.Add(strMessage)
                m_listBox.TopIndex = m_listBox.Items.Count - 1
            End If
        End Sub
    End Class
End Namespace
