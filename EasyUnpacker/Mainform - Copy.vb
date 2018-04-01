Imports System.IO
Imports SCA = SharpCompress
Imports SharpCompress.Archives
Imports SharpCompress.Archives.Rar
Imports SharpCompress.Common
Imports SharpCompress.Readers
Imports System.Threading
Imports EasyUnpacker.Logger
Imports EasyUnpacker.ErrorLogger

Public Class Mainform

#Region "Declarations"
    Public Property Preferences() As Preferences
    Public PrefsSaved As Boolean = False
    Private logger As Logger.ILogger ' create a composite logger with all the loggers.
    Private ErrLog As ErrorLogger
    Private i As Integer = 0
    Private z As Integer = 0
    Private file As String
    Private fI As FileInfo
    Private isCancelled As Boolean = False
    Private MainThread As Thread
    Private ArchiveName As String = ""
    Private maxParts As Integer = 0
    Private XtractedParts As Integer = 0
    Private Files2XtractCounter As Integer = 0
    Private FileCounter As Integer = 0
    Private myListBox As ListBox
    Private myControls As List(Of Control)
    Friend WithEvents ArchiveProgressBar As CustomProgressBar
    Private FileProgressBAr As ProgressBar
    Private CurrentArchive As String = ""
    Private entryName As String = ""
    Private entryTotal As Long = 0
    Private entryCompressed As Long = 0
    Private TotalBytesRead As Long = 0
#End Region

#Region "Form"
    Private Sub Mainform_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.CenterToScreen()
        logger = New CompositeLogger(New ListBoxLogger(Me.lbLog))
        ErrLog = New ErrorLogger
        lbScanFolders.Items.Clear()
        myControls = New List(Of Control)
        LoadSettings()
    End Sub
    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            'Programmeinstellungen etc. in der Preferences.xml speichern
            If Preferences.SavePreferences(Preferences) AndAlso PrefsSaved = True Then
                Me.Dispose()
                Me.Close()
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Controls"

    Private Sub btnAddSrcDir_Click(sender As Object, e As EventArgs) Handles btnAddPath.Click
        Dim FBD As New FolderBrowserDialog
        If (FBD.ShowDialog() = DialogResult.OK) Then

            If Not lbScanFolders.Items.Contains(FBD.SelectedPath) Then
                lbScanFolders.Items.Add(FBD.SelectedPath)
                Preferences.LastSourcePath.Add(FBD.SelectedPath)

                myControls.AddRange({btnDestDir, btnRmvPath, tbDestDir, btnScanFiles, cbArchivePath, cbFullPath, cbOverwrite})
                ChangeControlStatus("Enable")
                logger.LogMessage("Added Search path: " & FBD.SelectedPath)
            Else
                logger.LogMessage("Path exists!")
                Exit Sub
            End If
        Else
            logger.LogMessage("Operation cancelled")
            Exit Sub
        End If
    End Sub

    Private Sub btnRmvSrcDir_Click(sender As Object, e As EventArgs) Handles btnRmvPath.Click
        Dim knt As Integer = lbScanFolders.SelectedIndices.Count
        Dim i As Integer
        For i = 0 To knt - 1
            Preferences.LastSourcePath.Remove(lbScanFolders.SelectedItem)
            logger.LogMessage("Removed Search path: " & lbScanFolders.SelectedItem)
            lbScanFolders.Items.RemoveAt(lbScanFolders.SelectedIndex)
        Next
        For i = 0 To lbScanFolders.Items.Count
            If i = 0 Then
                myControls.AddRange({btnRmvPath, btnDestDir, btnXtract, btnScanFiles, tbDestDir})
                ChangeControlStatus("Disable")
            End If

        Next
    End Sub

    Private Sub btnDestDir_Click(sender As Object, e As EventArgs) Handles btnDestDir.Click
        Dim DestDirBrowser As New FolderBrowserDialog

        DestDirBrowser.SelectedPath = Preferences.LastDestPath

        If DestDirBrowser.ShowDialog = DialogResult.OK Then
            tbDestDir.Text = DestDirBrowser.SelectedPath
            Preferences.LastDestPath = DestDirBrowser.SelectedPath

            myControls.AddRange({btnScanFiles, tbDestDir, lbArchiveList})
            ChangeControlStatus("Enable")
            logger.LogMessage("Destination: " & DestDirBrowser.SelectedPath)
        Else
            logger.LogMessage("Operation cancelled")
            Exit Sub
        End If

    End Sub

    Private Sub btnScanFiles_Click(sender As Object, e As EventArgs) Handles btnScanFiles.Click

        ClearControls()

        Dim FileList As New List(Of FileInfo)

        For Each path In lbScanFolders.Items
            If CheckPath(path, "Dir") = False Then
                logger.LogMessage("Path " & path & " not available")
                Exit Sub
            Else
                For Each item In Get_Files(path, System.IO.SearchOption.AllDirectories, "rar", "")
                    FileList.Add(item)
                Next
            End If
        Next

        ' Bindingsource-Quelle zuweisen
        BindingSourceArchives.DataSource = FileList

        ' Listbox-Datenquelle festlegen
        lbArchiveList.DisplayMember = "Name"
        lbArchiveList.DataSource = BindingSourceArchives

        Files2XtractCounter = lbArchiveList.Items.Count
        myControls.Add(btnXtract)
        ChangeControlStatus("Enable")
        logger.LogMessage("Found " & Files2XtractCounter & " Archive(s)")
    End Sub

    Private Sub btnXtract_Click(sender As Object, e As EventArgs) Handles btnXtract.Click
        Try
            MainThread = New Thread(AddressOf ExtractVolumes)
            MainThread.Start()
            myControls.Add(btnCancelThread)
            ChangeControlStatus("Enable")
        Catch ex As Exception
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException)
            MsgBox(ex.InnerException.ToString)
        End Try
    End Sub

    Private Sub btnCancelThread_Click(sender As Object, e As EventArgs) Handles btnCancelThread.Click
        If MainThread IsNot Nothing AndAlso MainThread.IsAlive Then
            isCancelled = True
            logger.LogMessage("Extraction will be cancelled after current operation!")
        End If
    End Sub

    Private Sub cbArchivePath_CheckedChanged(sender As Object, e As EventArgs) Handles cbArchivePath.CheckedChanged
        If cbArchivePath.Checked Then
            If tbDestDir.Text IsNot "" Then
                tbDestDir.Text = ""
                Preferences.ExtractSame = cbArchivePath.Checked
            End If
            logger.LogMessage("Extract Files to same as Archive enabled")
        Else
            Preferences.ExtractSame = cbArchivePath.Checked
            tbDestDir.Text = Preferences.LastDestPath
            logger.LogMessage("Extract Files to same as Archive disabled")
        End If
    End Sub

    Private Sub cbFullPath_CheckedChanged(sender As Object, e As EventArgs) Handles cbFullPath.CheckedChanged
        Preferences.ExtractFullPath = cbFullPath.Checked
        If cbFullPath.Checked Then
            logger.LogMessage("Extract Full Path enabled")
        Else
            logger.LogMessage("Extract Full Path disabled")
        End If
    End Sub

    Private Sub cbOverwrite_CheckedChanged(sender As Object, e As EventArgs) Handles cbOverwrite.CheckedChanged
        Preferences.Overwrite = cbOverwrite.Checked
        If cbOverwrite.Checked Then
            logger.LogMessage("Overwrite Files enabled")
        Else
            logger.LogMessage("Overwrite Files disabled")
        End If
    End Sub
#End Region

#Region "Main Functions"
    'Es darf nur der GUI-Thread auf Controls zugreifen. Andere Threads dürfen das überhaupt nicht und niemals.
    Private Sub ExtractVolumes()
        Try
            'Assign a Listbox to global Variable
            myListBox = lbArchiveList

            'Count the Archives and set the Value
            Files2XtractCounter = myListBox.Items.Count

            For i = 0 To myListBox.Items.Count - 1

                If isCancelled Then
                    logger.LogMessage("Extraction cancelled")
                    MainThread = Nothing
                    isCancelled = False
                    ClearControls()
                    Exit Sub
                End If

                Using archive As IArchive = getArchive(lbArchiveList.Items.Item(i).ToString, Nothing)

                    'Add Handler
                    AddHandler archive.FilePartExtractionBegin, AddressOf Archive_FilePartExtractionBegin
                    AddHandler archive.EntryExtractionBegin, AddressOf Archive_EntryExtractionBegin
                    AddHandler archive.EntryExtractionEnd, AddressOf Archive_EntryExtractionEnd
                    AddHandler archive.CompressedBytesRead, AddressOf Archive_CompressedBytesRead

                    'Is the Archive complete?? e.g.: RAR-Archive with 20 Parts (.rar - .r19)
                    'If some part is missing, the extraction progress won´t work
                    If archive.IsComplete Then

                        If TypeOf archive Is SharpCompress.Archives.Rar.RarArchive AndAlso
                              CType(archive, SharpCompress.Archives.Rar.RarArchive).IsMultipartVolume() AndAlso
                              Not CType(archive, SharpCompress.Archives.Rar.RarArchive).IsFirstVolume() Then
                            Exit Sub
                        End If

                        'Count the Volumes of Archive an store into "maxParts"
                        maxParts = archive.Volumes.Count

                        'Set the MAX-Value of the ProgressBar based on "maxParts"
                        ArchiveProgressBar.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), ArchiveProgressBar, maxParts + 1, "Max")

                        For Each entry In archive.Entries

                            'If the entry of the Archive is NOT a Directory begin extracting...
                            If Not entry.IsDirectory Then

                                'If Checkbox "Same As Archive" enabled, use this routine
                                If cbArchivePath.Checked Then

                                    'Store the current item of the Listbox into FileInfo
                                    fI = DirectCast(lbArchiveList.Items(i), FileInfo)
                                    'Set the Current ArchiveName
                                    CurrentArchive = fI.Name
                                    'Set the Extraction(Destination) Dir. Because Checkbox "Same As Archive" is enabled, all files will be extracted in the archive directory
                                    Dim DestDir As String = fI.Directory.ToString
                                    'Combine the Destination Path & the Filename (entry of archive)
                                    Dim FullPath As String = System.IO.Path.Combine(DestDir, entry.Key)
                                    'If the entry is within a folder, get the folder name
                                    Dim entryFolder As String = path.GetDirectoryName(entry.Key)

                                    'If Extract FullPath is enabled and Overwrite (File) is disabled, automatically generate a new File Name (It´s easier to handle :))
                                    If cbFullPath.Checked And Not cbOverwrite.Checked Then
                                        If CheckPath(FullPath, "File") Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(DestDir, "(" & z & ")-" & entry.Key)
                                            logger.LogMessage("File " & entry.Key & " exists, new Filename: " & FullPath)
                                            entry.WriteToFile(FullPath)
                                        End If
                                        entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = False})

                                        'If Overwrite is enabled, overwrite the file, but DON`T extract the full path ;)
                                    ElseIf cbOverwrite.Checked Then
                                        entry.WriteToDirectory(FullPath, New ExtractionOptions() With {.ExtractFullPath = False, .Overwrite = True})

                                        'If Extract Full Path AND Overwrite enabled, do the normal Thing :)
                                    ElseIf cbFullPath.Checked AndAlso cbOverwrite.Checked Then
                                        archive.WriteToDirectory(FullPath, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = True})

                                    Else
                                        If CheckPath(entryFolder, "File") Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(DestDir, "(" & z & ")-" & entry.Key)
                                            entry.WriteToFile(FullPath)
                                            'ElseIf System.IO.File.Exists(path) Then
                                            '   z += 1
                                            '  path = System.IO.Path.Combine(DestDir, "(" & z & ")-" & entry.Key)
                                            logger.LogMessage("File " & entry.Key & " exists, new Filename: " & FullPath)
                                            entry.WriteToFile(FullPath)
                                        Else
                                            entry.WriteToDirectory(DestDir)
                                        End If
                                    End If

                                    FileCounter += 1
                                    XtractedParts += 1

                                    'If Checkbox "Same As Archive" disabled, use this routine
                                Else
                                    fI = DirectCast(lbArchiveList.Items(i), FileInfo)
                                    CurrentArchive = fI.Name
                                    Dim DestDir As String = Preferences.LastDestPath
                                    Dim FullPath As String = System.IO.Path.Combine(DestDir, entry.Key)
                                    Dim entryFolder As String = Path.GetDirectoryName(entry.Key)

                                    If cbFullPath.Checked And Not cbOverwrite.Checked Then
                                        If CheckPath(FullPath, "File") Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(DestDir, "(" & z & ")-" & entry.Key)
                                            logger.LogMessage("File " & entry.Key & " exists, new Filename: " & FullPath)
                                            entry.WriteToFile(FullPath)
                                        Else
                                            entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = False})
                                        End If

                                    ElseIf Not cbFullPath.Checked AndAlso cbOverwrite.Checked Then
                                        entry.WriteToDirectory(tbDestDir.Text, New ExtractionOptions() With {.ExtractFullPath = False, .Overwrite = True})

                                    ElseIf cbFullPath.Checked AndAlso cbOverwrite.Checked Then
                                        entry.WriteToFile(FullPath, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = True})

                                    Else
                                        '  If CheckPath(path, "File") Then
                                        ' z += 1
                                        'path = System.IO.Path.Combine(tbDestDir.Text, "(" & z & ")-" & entry.Key)
                                        If System.IO.File.Exists(FullPath) Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(tbDestDir.Text, "(" & z & ")-" & entry.Key)
                                        End If
                                        logger.LogMessage("File " & entry.Key & " exists, new Filename: " & FullPath)
                                        entry.WriteToFile(FullPath)
                                    End If
                                    'End If
                                    XtractedParts += 1
                                End If
                            End If
                        Next
                        XtractedParts = 0
                    End If
                End Using
            Next
        Catch fex As System.IO.IOException
            If cbOverwrite.Checked = False Then
                logger.LogMessage("Overwrite disabled: " & fI.Name & " can´t be extracted")
            End If
            ErrLog.WriteErrorLogs(LogsCategory.Exception, fex)
        Catch ex As Exception
            logger.LogMessage(ex.Message)
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex)
        End Try
        ClearControls()
    End Sub


#End Region

#Region "Helper Functions"
    Private Sub LoadSettings()
        ' Load Preferences
        Preferences = If(Preferences.LoadPreferences(logger, ErrLog), Nothing)

        If Preferences Is Nothing Then
            Preferences = New Preferences()
        End If

        ' Wenn z.B. durch ein Update eine neue Einstellung in der settings.xml steht, wird das Event "DeserializedWithErrors" ausgelöst und die Standartwerte
        ' aus Preferences geladen
        If Preferences.DeserializedWithErrors Then
            logger.LogMessage("Preferences: Deserialize Error!")
        End If

        'Set Progressbar
        ArchiveProgressBar = pbArchiveProgress

        'Set last Archivepath, stored in Preferences
        If Preferences.LastSourcePath.Count > 0 Then
            For Each path In Preferences.LastSourcePath
                lbScanFolders.Items.Add(path)
            Next
        End If

        'Add controls to disable to a List(Of Control)
        myControls.AddRange({btnRmvPath, btnDestDir, btnScanFiles, btnXtract, btnCancelThread, cbArchivePath, cbFullPath, cbOverwrite, tbDestDir})

        'Disable Controls
        ChangeControlStatus("Disable")

        'Call Helperfunction and set all values stored in preferences (Checkboxes)
        SetCB()

        logger.LogMessage("Settings successfully loaded!")
    End Sub

    Private Sub CheckLB()
        If lbScanFolders.Items.Count > 0 Then
            lbScanFolders.SetSelected(0, True)
        End If
        If tbDestDir.Text IsNot "" Then
            tbDestDir.Enabled = True
        End If
    End Sub

    Public Sub ChangeControlStatus(Optional Action As String = Nothing)
        For Each item As Control In myControls
            If Action = "Disable" Then
                item.Enabled = False
            Else
                item.Enabled = True
            End If
        Next
        myControls.Clear()
    End Sub

    Private Sub ClearControls()
        Try
            lblCurArchive.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurArchive, Nothing, "Clear")
            lblCurFile.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurFile, Nothing, "Clear")
            lblVolume.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblVolume, Nothing, "Clear")
            lblFileCompSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileCompSize, Nothing, "Clear")
            lblFileFullSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileFullSize, Nothing, "Clear")
            lblPercentExtracted.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblPercentExtracted, Nothing, "Clear")
            ArchiveProgressBar.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), pbArchiveProgress, Nothing, "Clear")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetCB()
        If Preferences.ExtractSame Then
            cbArchivePath.Checked = True
            myControls.Add(btnScanFiles)
            ChangeControlStatus("Enable")
        Else
            CheckLB()
            tbDestDir.Text = Preferences.LastDestPath

        End If

        cbFullPath.Checked = Preferences.ExtractFullPath
        cbOverwrite.Checked = Preferences.Overwrite
    End Sub

    ' Get Files {directory} {recursive} {ext} {word in filename}
    Private Function Get_Files(ByVal directory As String,
                               ByVal recursive As System.IO.SearchOption,
                               ByVal ext As String,
                               ByVal with_word_in_filename As String) As List(Of System.IO.FileInfo)

        Return System.IO.Directory.GetFiles(directory, "*" & If(ext.StartsWith("*"), ext.Substring(1), ext), recursive) _
                           .Where(Function(o) o.ToLower.Contains(with_word_in_filename.ToLower)) _
                           .Select(Function(p) New System.IO.FileInfo(p)).ToList
    End Function

    Private Function CheckPath(ByVal PathOrFile As String, ByVal Type As String)

        Select Case Type
            Case "Dir"
                If Directory.Exists(PathOrFile) Then
                    Return True
                End If
            Case "File"
                If System.IO.File.Exists(PathOrFile) Then
                    Return True
                End If
        End Select
        Return False
    End Function

    Private Delegate Function UpdateControlDelegate(ByVal ctrl As Control, ByVal msg As Object, ByVal action As Object)
    Private Function UpdateControl(ByVal ctrl As Control, ByVal msg As Object, ByVal action As Object)
        Try

            Dim ctrlFullName As String = ctrl.GetType().FullName
            Dim ctrlShortName As String = ctrlFullName.Substring(21)

            Select Case ctrl.GetType()
                Case GetType(Label)
                    If action = "Add" Then
                        ctrl.Text = msg
                    ElseIf action = "Clear" Then
                        ctrl.ResetText()
                    End If
                Case GetType(TextBox)
                    If action = "Add" Then
                        ctrl.Text = msg
                    ElseIf action = "Clear" Then
                        ctrl.ResetText()
                    End If
                Case GetType(CheckBox)
                    Dim cb As CheckBox = ctrl
                    If action = "Checked" Then
                        cb.CheckState = 1
                    Else
                        cb.CheckState = 0
                    End If
                Case GetType(ListBox)
                    If action = "Add" Then
                        myListBox.Items.Add(msg)
                    ElseIf action = "Count" Then
                        Return myListBox.Items.Count
                    ElseIf action = "DataSource" Then
                        myListBox.DataSource = msg
                    End If
                Case GetType(CustomProgressBar)
                    If action = "Enable" Then
                        ArchiveProgressBar.Show()
                    ElseIf action = "Width" Then
                        ArchiveProgressBar.Width = msg
                    ElseIf action = "Max" Then
                        ArchiveProgressBar.Maximum = msg
                    ElseIf action = "DoStep" Then
                        ArchiveProgressBar.Value = XtractedParts
                        ArchiveProgressBar.Increment(1)
                    ElseIf action = "Clear" Then
                        If ArchiveProgressBar.Value > 0 Then
                            ArchiveProgressBar.Value = 0
                        End If
                    End If
            End Select

        Catch ex As Exception

        End Try
    End Function

    Protected Friend Function bytesize(ByVal bytes As Long) As String
        Select Case bytes
            Case Is > (1024.0 * 1024.0 * 1024.0 * 1024.0)
                Return CStr(Math.Round((bytes / 1024 / 1024 / 1024 / 1024), 2)) + " TB"
            Case Is > (1024.0 * 1024.0 * 1024.0)
                Return CStr(Math.Round((bytes / 1024 / 1024 / 1024), 2)) + " GB"
            Case Is > (1024.0 * 1024.0)
                Return CStr(Math.Round((bytes / 1024 / 1024), 2)) + " MB"
            Case Is > 1024.0
                Return CStr(Math.Round((bytes / 1024), 2)) + " KB"
            Case Is < 1024.0
                Return CStr(Math.Round(bytes, 2)) + " B"
            Case Else
                Return CStr(Math.Round(bytes, 2)) + " B"
        End Select
    End Function

    Function GetFilesByExtensions(ByVal dir As DirectoryInfo, ParamArray extensions As String()) As IEnumerable(Of FileInfo)
        If extensions Is Nothing Then Throw New ArgumentNullException("extensions")
        Dim files As IEnumerable(Of FileInfo) = dir.EnumerateFiles()
        Return files.Where(Function(f) extensions.Contains(f.Extension))
    End Function

    Public Sub WriteErrorLogs(ByVal LogsCat As ErrorLogger.LogsCategory, ByVal ex As Exception)
        ErrorLogger.Instance.WriteErrorLogs(LogsCat, ex)
    End Sub

    Private Sub lbScanFolders_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbScanFolders.SelectedIndexChanged
        If lbScanFolders.Items.Count > 0 Then
            myControls.AddRange({btnRmvPath, btnDestDir, tbDestDir, btnScanFiles, cbArchivePath, cbFullPath, cbOverwrite})
            ChangeControlStatus("Enable")
        End If
    End Sub

    Private Sub tbDestDir_TextChanged(sender As Object, e As EventArgs) Handles tbDestDir.TextChanged
        If lbScanFolders.Items.Count > 0 Then
            myControls.AddRange({btnScanFiles, tbDestDir})
            ChangeControlStatus("Enable")
        Else
            myControls.Add(tbDestDir)
            ChangeControlStatus("Disable")
        End If
    End Sub

    Private Sub Archive_FilePartExtractionBegin(ByVal sender As Object, ByVal e As FilePartExtractionBeginEventArgs)
        lblCurArchive.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurArchive, CurrentArchive, "Add")
        lblVolume.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblVolume, e.Name, "Add")
        XtractedParts += 1
        ArchiveProgressBar.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), pbArchiveProgress, Nothing, "DoStep")
    End Sub

    Private Sub Archive_EntryExtractionBegin(ByVal sender As Object, ByVal e As ArchiveExtractionEventArgs(Of IArchiveEntry))
        entryName = e.Item.Key
        entryTotal = e.Item.Size
        entryCompressed = e.Item.CompressedSize
        lblFileFullSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileFullSize, bytesize(entryTotal), "Add")
        lblFileCompSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileCompSize, bytesize(entryCompressed), "Add")
        lblCurFile.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurFile, entryName, "Add")
        logger.LogMessage("Start Extracting " & entryName)
    End Sub

    Private Sub Archive_EntryExtractionEnd(ByVal sender As Object, ByVal e As ArchiveExtractionEventArgs(Of IArchiveEntry))
        FileCounter += 1
        lblPercentExtracted.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblPercentExtracted, "Extracted " & FileCounter & " out of " & Files2XtractCounter, "Add")
        logger.LogMessage("Extracting Done :)")
    End Sub

    Private Sub Archive_CompressedBytesRead(ByVal sender As Object, ByVal e As CompressedBytesReadEventArgs)
        TotalBytesRead = e.CompressedBytesRead
        Dim XtractedBytes As String = "Bytes extracted: " & bytesize(TotalBytesRead) & " from " & bytesize(entryTotal)
        Label8.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), Label8, XtractedBytes, "Add")
    End Sub

    Private Shared Function getArchive(ArchivePath As String, ReadOptions As ReaderOptions) As IArchive
        If ArchivePath.ToUpper.EndsWith(".RAR") Then
            Return SharpCompress.Archives.Rar.RarArchive.Open(ArchivePath, ReadOptions)
        ElseIf ArchivePath.ToUpper.EndsWith(".ZIP") Then
            Return SharpCompress.Archives.Zip.ZipArchive.Open(ArchivePath, ReadOptions)
        Else
            Return ArchiveFactory.Open(ArchivePath, ReadOptions)
        End If
    End Function

    Private Sub RmvArchive_Click(sender As Object, e As EventArgs) Handles RmvArchive.Click
        If BindingSourceArchives.Count > 0 Then
            BindingSourceArchives.RemoveCurrent()
        End If
    End Sub

#End Region

End Class

