Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms.ListView
Imports System.Text

Public Class LanguageEditor

#Region "Constructors"
    Dim xmlPath As String = ""
    Private ofd As New OpenFileDialog
    Dim XMLDoc As XElement
    Dim t As New Thread(AddressOf LoadXML) With {.IsBackground = True}
    Dim Daten As XDocument = Nothing
#End Region

#Region "Forms"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        CreateListview()
        DisableAndClearControls()
    End Sub
#End Region

#Region "Little Helper"
    Private Sub DisableAndClearControls()
        lvw.Items.Clear()
        Me.Text = "Language Editor"
        ToolStripLabel2.Text = ""
        tbValue.ReadOnly = True
        Button2.Enabled = False
    End Sub

    Private Sub EnableControls()
        tbValue.ReadOnly = False
        Button2.Enabled = True
    End Sub

    Private Sub CreateListview()
        With lvw
            .View = System.Windows.Forms.View.Details
            .Columns.Add("ID", 50, HorizontalAlignment.Left)
            .Columns.Add("Value", 450, HorizontalAlignment.Left)
        End With
    End Sub

    Private Delegate Sub UpdateStatusDelegate(ByVal lbl As ToolStripLabel, ByVal status As String)
    Private Sub UpdateStatus(ByVal lbl As ToolStripLabel, ByVal status As String)
        'If lbl.InvokeRequired Then
        Me.Invoke(New UpdateStatusDelegate(AddressOf Me.UpdateStatus), New Object() {status})
        Return
        'End If
        lbl.Text = lbl.Text & " / " & status
    End Sub
#End Region

#Region "Controls"
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        DisableAndClearControls()
        With ofd
            .InitialDirectory = Application.StartupPath
            .Multiselect = False
            .Filter = "Language Files (*.xml)|*.xml"

            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                xmlPath = .FileName

                tsLngFile.Text = Path.GetFileName(xmlPath)

                EnableControls()

                If t.IsAlive = False Then
                    t = New Thread(AddressOf LoadXML) With {.IsBackground = True}
                    t.Start()
                Else
                    t.Start()
                End If
            End If
        End With
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Anzahl der Einträge im Listview ermitteln
        Dim i As Integer = lvw.Items.Count + 1

        If tbValue.Text = "" Then
            MessageBox.Show("Add a new value first!")
            Exit Sub
        Else
            'Den neue Eintrag hinzufügen und den Zähler i um 1 erhöhen (wegen der ID)
            lvwAddItem(lvw, i, tbValue.Text)
        End If

        ' Call FindItemWithText with the contents of the textbox.
        Dim foundItem As ListViewItem = _
            lvw.FindItemWithText(tbValue.Text, True, 0, False)

        If (foundItem IsNot Nothing) Then
            lvw.TopItem = foundItem
        End If

        EditEntry(i.ToString, foundItem.SubItems(1).Text, True)
    End Sub

    Private Sub ctmEdit_Click(sender As Object, e As EventArgs) Handles ctmEdit.Click

        ' check where clicked
        CurrentItem = lvw.SelectedItems(0)  ' which listviewitem was clicked

        If CurrentItem Is Nothing Then Exit Sub

        CurrentSB = CurrentItem.SubItems(1)  ' which subitem was clicked

        ' See which column has been clicked

        ' NOTE: This portion is important. Here you can define your own
        '       rules as to which column can be edited and which cannot.
        Dim iSubIndex As Integer = CurrentItem.SubItems.IndexOf(CurrentSB)
        Select Case iSubIndex
            Case 0, 1
                ' These two columns are allowed to be edited. So continue the code
            Case Else
                ' In my example I have defined that only "Runs" and "Wickets" columns can be edited by user
                Exit Sub
        End Select

        ' Check if the first subitem is being edited
        If iSubIndex = 0 Then
            ' There's a slight coding difficulty here. If the first item is to be edited
            ' then when you get the Bounds of the first subitem, it returns the Bounds of
            ' the entire ListViewItem. Hence the Textbox looks very wierd. In order to allow
            ' editing on the first column, we use the built-in editing method.

            CurrentItem.BeginEdit()     ' make sure the LabelEdit is set to True
            Exit Sub
        End If

        Dim lLeft = CurrentSB.Bounds.Left + 2
        Dim lWidth As Integer = CurrentSB.Bounds.Width
        With editbox
            .SetBounds(lLeft + lvw.Left, CurrentSB.Bounds.Top + lvw.Top, lWidth, CurrentSB.Bounds.Height)
            .Text = CurrentSB.Text
            .Show()
            .Focus()
        End With
    End Sub

    Private Sub ctmDelete_Click(sender As Object, e As EventArgs) Handles ctmDelete.Click
        CurrentItem = lvw.SelectedItems(0)  ' which listviewitem was clicked
        CurrentSB = CurrentItem.SubItems(1)
        If MessageBox.Show("Eintrag mit ID: " & CurrentItem.Text.ToString & " und dem Inhalt: " & CurrentSB.Text.ToString & " wirklich löschen?", "Eintrag löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            lvw.Items.Remove(CurrentItem)

            removeEntry(CurrentItem.Text, CurrentSB.Text)
        End If
    End Sub
#End Region

#Region "XMLEdit"
    Private Function ReadXMLComment(ByVal node As XDocument)
        Dim _node = node.Nodes().OfType(Of XComment)()
        Return _node(0).Value.ToString
    End Function

    Private Sub LoadXML()
        Try
            Daten = XDocument.Load(xmlPath)

            tsLngMaker.Text = ReadXMLComment(Daten)

            For Each Eintrag In Daten.<strings>.<string>
                lvwAddItem(lvw, Eintrag.@id, Eintrag.Value)
            Next

            Dim i As Integer = lvw.Items.Count()
            ToolStripLabel2.Text = i

            Application.ExitThread()
        Catch ex As Exception
            MessageBox.Show("Not a valid language file!")
        End Try
    End Sub

    Private Sub EditEntry(ByVal iSubindex As String, ByVal szValue As String, Optional _new As Boolean = False)
        Try

            'Dim InfoListe = From Infos In Daten.<id> Where Infos.Element("id").Value = CurrentItem.Text And Infos.Element("name").Value = szPropertyName Select Infos                           ' Alle passenden Knoten lesen

            If _new Then
                ' Daten.Add(New XElement("string", szValue, New XAttribute("id", iSubindex)))
                'Daten.Save(xmlPath)

                Daten.Element("strings").Add(New XElement("string", szValue, New XAttribute("id", iSubindex)))

                Daten.Save(xmlPath)
            Else

                Dim lNode = (From e In Daten.Descendants Where e.@id = iSubindex).Single()
                'MessageBox.Show(lNode.Value)

                lNode.Value = szValue
                Daten.Save(xmlPath)

            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub removeEntry(ByVal szPropertyName As String, ByVal szValue As String)
        Try
            Dim toRemove As New List(Of XElement)

            For Each xel In Daten.<strings>.<string>
                If xel.Value = szValue Then
                    'MessageBox.Show(xel.Attribute("id").Value)
                    toRemove.Add(xel)
                End If
            Next
            For Each xel In toRemove.Take(1)
                xel.Remove()
            Next
            For Each xel In Daten.<strings>.<string>
                toRemove.Add(xel)
            Next

            Daten.Save(xmlPath)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
#End Region

#Region "ListView Coding"

    Dim bCancelEdit As Boolean
    Dim CurrentSB As ListViewItem.ListViewSubItem
    Dim CurrentItem As ListViewItem

    Private Sub LV_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvw.KeyDown
        ' This subroutine is for starting editing when keyboard shortcut is pressed (e.g. F2 key)
        If lvw.SelectedItems.Count = 0 Then Exit Sub

        Select Case e.KeyCode
            Case Keys.F2    ' F2 key is pressed. Initiate editing
                e.Handled = True
                BeginEditListItem(lvw.SelectedItems(0), 1)
            Case Keys.Delete
                e.Handled = True
                CurrentItem = lvw.SelectedItems(0)  ' which listviewitem was clicked
                CurrentSB = CurrentItem.SubItems(1)
                If MessageBox.Show("Eintrag mit ID: " & CurrentItem.Text.ToString & " und dem Inhalt: " & CurrentSB.Text.ToString & " wirklich löschen?", "Eintrag löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                    lvw.Items.Remove(CurrentItem)

                    removeEntry(CurrentItem.Text, CurrentSB.Text)
                End If
        End Select
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvw.MouseDoubleClick
        ' This subroutine checks where the double-clicking was performed and
        ' initiates in-line editing if user double-clicked on the right subitem

        ' check where clicked
        CurrentItem = lvw.GetItemAt(e.X, e.Y)     ' which listviewitem was clicked
        If CurrentItem Is Nothing Then Exit Sub
        CurrentSB = CurrentItem.GetSubItemAt(e.X, e.Y)  ' which subitem was clicked

        ' See which column has been clicked

        ' NOTE: This portion is important. Here you can define your own
        '       rules as to which column can be edited and which cannot.
        Dim iSubIndex As Integer = CurrentItem.SubItems.IndexOf(CurrentSB)
        Select Case iSubIndex
            Case 0, 1
                ' These two columns are allowed to be edited. So continue the code
            Case Else
                ' In my example I have defined that only "Runs" and "Wickets" columns can be edited by user
                Exit Sub
        End Select

        ' Check if the first subitem is being edited
        If iSubIndex = 0 Then
            ' There's a slight coding difficulty here. If the first item is to be edited
            ' then when you get the Bounds of the first subitem, it returns the Bounds of
            ' the entire ListViewItem. Hence the Textbox looks very wierd. In order to allow
            ' editing on the first column, we use the built-in editing method.

            CurrentItem.BeginEdit()     ' make sure the LabelEdit is set to True
            Exit Sub
        End If

        Dim lLeft = CurrentSB.Bounds.Left + 2
        Dim lWidth As Integer = CurrentSB.Bounds.Width
        With editbox
            .SetBounds(lLeft + lvw.Left, CurrentSB.Bounds.Top + lvw.Top, lWidth, CurrentSB.Bounds.Height)
            .Text = CurrentSB.Text
            .Show()
            .Focus()
        End With
    End Sub

    Private Sub editbox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles editbox.KeyPress
        ' This subroutine closes the text box
        Select Case e.KeyChar
            Case Microsoft.VisualBasic.ChrW(Keys.Return)    ' Enter key is pressed
                bCancelEdit = False ' editing completed
                e.Handled = True
                editbox.Hide()

            Case Microsoft.VisualBasic.ChrW(Keys.Escape)    ' Escape key is pressed
                bCancelEdit = True  ' editing was cancelled
                e.Handled = True
                editbox.Hide()
        End Select
    End Sub

    Private Sub TextBox1_LostFocus(sender As Object, e As System.EventArgs) Handles editbox.LostFocus
        Try

            editbox.Hide()

            If bCancelEdit = False Then
                If editbox.Text.Trim <> "" Then

                    ' NOTE: You can define your validation rules here. In my example I've
                    '       set that only numbers can be entered in "Runs" and "Wickets" column

                    ' update listviewitem
                    CurrentSB.Text = editbox.Text

                    ' save changes so that next time you load this XML document the changes are there.
                    Dim iSubIndex As Integer = CurrentItem.SubItems.IndexOf(CurrentSB)
                    Dim szPropertyName As String = ""
                    If iSubIndex = 1 Then
                        szPropertyName = "string id"
                    End If

                    'EditEntry(CurrentItem.Text, szPropertyName, CurrentSB.Text)
                    EditEntry(CurrentItem.Text, CurrentSB.Text, False)

                End If
            Else
                ' Editing was cancelled by user
                bCancelEdit = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.InnerException.ToString)
        End Try
        lvw.Focus()
    End Sub

    Private Sub BeginEditListItem(iTm As ListViewItem, SubItemIndex As Integer)
        ' This subroutine is for manually initiating editing instead of mouse double-clicks

        Dim pt As Point = iTm.SubItems(SubItemIndex).Bounds.Location
        Dim ee As New System.Windows.Forms.MouseEventArgs(Windows.Forms.MouseButtons.Left, 2, pt.X, pt.Y, 0)
        Call ListView1_MouseDoubleClick(lvw, ee)
    End Sub

    Private Sub lvw_MouseClick(sender As Object, e As MouseEventArgs) Handles lvw.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If lvw.GetItemAt(e.X, e.Y) IsNot Nothing Then
                lvw.GetItemAt(e.X, e.Y).Selected = True
                lvwContext.Show(lvw, New Point(e.X, e.Y))
            End If
        End If
    End Sub

    ''' Fügt dem ListView eine komplette Datenzeile hinzu
    ''' </summary>
    ''' <param name="lvw">ListView-Control</param>
    ''' <param name="Text">Parameterliste der einzelnen Zellenwerte</param>
    Private Sub lvwAddItem(ByVal _lvw As ListView, ByVal ParamArray Text() As String)
        With _lvw.Items
            Me.Invoke(Function() .Add(New ListViewItem(Text)))
        End With
    End Sub
#End Region

End Class

