Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms

Namespace Logger
    Class ListViewLogger
        Implements ILogger
        Dim logDelegate As MethodInvoker
        Private m_ListView As ListView
        Public Sub New(listBox As ListView)
            m_ListView = listBox
        End Sub
        Public Sub LogMessage(strMessage As String) Implements ILogger.LogMessage
            If m_ListView.InvokeRequired Then
                m_ListView.BeginInvoke(New MethodInvoker(Sub()
                                                             Dim item As New ListViewItem
                                                             item.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                                             item.SubItems.Add(strMessage)
                                                             item.ToolTipText = strMessage
                                                             m_ListView.Items.Add(item)
                                                         End Sub))
            Else
                m_ListView.Items.Add(strMessage)
            End If
        End Sub
    End Class
End Namespace
