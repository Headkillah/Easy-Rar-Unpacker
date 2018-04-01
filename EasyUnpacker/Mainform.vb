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
    Private z As Integer = 0
    Private fI As FileInfo

    'Thread BEGIN
    Private MainThread As Thread
    Private _cancelThread As Boolean = False
    Private _wait As AutoResetEvent = New AutoResetEvent(False)
    'Thead END

    Dim Percentage As Integer = 0

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
    Private TotalBytesRead As Int32 = 0
    Private Shared SelectedLanguage As String = "" ' ausgewählte Sprache
    Public language As Language = New Language
    Public Event ResultOK(sender As System.Object, e As DialogResult)
#End Region

#Region "Form"
    Private Sub Mainform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        logger = New CompositeLogger(New ListBoxLogger(Me.lbLog))
        ErrLog = New ErrorLogger
        lbScanFolders.Items.Clear()
        myControls = New List(Of Control)

        AddEventsToForm()
        LoadSettings()
        LoadFormState()

        language.GetSupportedLanguages()
        InitLanguage()

        'Get all supported languages within the folder "Langs" and add them to the Language selection menu
        For Each langFile In language.GetSupportedLanguages()
            Dim langName As String = langFile.key.Substring(langFile.key.IndexOf("-") + 1)
            mnuChangeLang.DropDownItems.Add(langName)
        Next

        'Count all items in the language selection menu and add an ClickEventHandler, needed for selecting / changing language (sender)
        For i As Integer = 0 To mnuChangeLang.DropDownItems.Count - 1
            Dim Item As ToolStripItem = mnuChangeLang.DropDownItems.Item(i)
            AddHandler Item.Click, New EventHandler(AddressOf Me.LanguageClickEventHandler)
        Next

        'Call Helperfunction and set all values stored in preferences (Checkboxes)
        SetCB()

        Dim sMsg As String = language.GetString(47, Nothing)

        logger.LogMessage(language.GetString(38, "Einstellungen: Erfolgreich geladen"))
        logger.LogMessage(sMsg.Replace("%ACTVER%", language.GetString(0, Nothing)))
        Me.Text = Me.ProductName & " | " & language.LangEditor
    End Sub
    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            SaveFormState()
            'Store Path, Checkboxes etc. in Preferences and save
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
                logger.LogMessage(language.GetString(24, "Suchpfad hinzugefügt: ") & " " & FBD.SelectedPath)
            Else
                logger.LogMessage(language.GetString(25, "Pfad existiert bereits!"))
                Exit Sub
            End If
        Else
            logger.LogMessage(language.GetString(26, "Operation abgebrochen"))
            Exit Sub
        End If
    End Sub

    Private Sub btnRmvSrcDir_Click(sender As Object, e As EventArgs) Handles btnRmvPath.Click
        Dim knt As Integer = lbScanFolders.SelectedIndices.Count
        Dim i As Integer
        For i = 0 To knt - 1
            Preferences.LastSourcePath.Remove(lbScanFolders.SelectedItem)
            logger.LogMessage(language.GetString(39, "Suchpfad entfernt: ") & " " & lbScanFolders.SelectedItem)
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
            logger.LogMessage(language.GetString(8, "Zielordner: ") & ": " & DestDirBrowser.SelectedPath)
        Else
            logger.LogMessage(language.GetString(26, "Operation abgebrochen"))
            Exit Sub
        End If
    End Sub

    Private Sub btnScanFiles_Click(sender As Object, e As EventArgs) Handles btnScanFiles.Click

        ClearControls()

        Dim FileList As New List(Of FileInfo)

        For Each path In lbScanFolders.Items
            If CheckPath(path, "Dir") = False Then
                logger.LogMessage(language.GetString(27, "Pfad ") & path & language.GetString(28, " nicht verfügbar"))
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
        logger.LogMessage(language.GetString(29, "Archiv(e) gefunden: ") & Files2XtractCounter)
    End Sub

    Private Sub btnXtract_Click(sender As Object, e As EventArgs) Handles btnXtract.Click
        Try
            MainThread = New Thread(AddressOf ExtractVolumes)
            MainThread.Start()
            MainThread.IsBackground = True
            MainThread.Name = "ExtractThread"
            myControls.Add(btnCancelThread)
            ChangeControlStatus("Enable")
        Catch ex As Exception
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException)
            MsgBox(ex.InnerException.ToString)
        End Try
    End Sub

    Private Sub btnCancelThread_Click(sender As Object, e As EventArgs) Handles btnCancelThread.Click

        _wait.Set()
        'logger.LogMessage(language.GetString(40, "Abbruch nach laufendem Entpackvorgang!"))

    End Sub

    Private Sub cbArchivePath_CheckedChanged(sender As Object, e As EventArgs) Handles cbArchivePath.CheckedChanged
        If cbArchivePath.Checked Then
            If tbDestDir.Text IsNot "" Then
                tbDestDir.Text = ""
                Preferences.ExtractSame = cbArchivePath.Checked
            End If
            logger.LogMessage(language.GetString(32, "Dateien im Archivordner") & language.GetString(30, " aktiviert"))
        Else
            Preferences.ExtractSame = cbArchivePath.Checked
            tbDestDir.Text = Preferences.LastDestPath
            logger.LogMessage(language.GetString(32, "Dateien im Archivordner") & language.GetString(31, " deaktiviert"))
        End If
    End Sub

    Private Sub cbFullPath_CheckedChanged(sender As Object, e As EventArgs) Handles cbFullPath.CheckedChanged
        Preferences.ExtractFullPath = cbFullPath.Checked
        If cbFullPath.Checked Then
            logger.LogMessage(language.GetString(13, "Entpacke ganzen Pfad") & language.GetString(30, " aktiviert"))
        Else
            logger.LogMessage(language.GetString(13, "Entpacke ganzen Pfad") & language.GetString(31, " deaktiviert"))
        End If
    End Sub

    Private Sub cbOverwrite_CheckedChanged(sender As Object, e As EventArgs) Handles cbOverwrite.CheckedChanged
        Preferences.Overwrite = cbOverwrite.Checked
        If cbOverwrite.Checked Then
            logger.LogMessage(language.GetString(14, "Dateien überschreiben") & language.GetString(30, " aktiviert"))
        Else
            logger.LogMessage(language.GetString(14, "Dateien überschreiben") & language.GetString(31, " deaktiviert"))
        End If
    End Sub
#End Region

#Region "Main Functions"

    'Es darf nur der GUI-Thread auf Controls zugreifen. Andere Threads dürfen das überhaupt nicht und niemals.
    Private Sub ExtractVolumes()
        Try
            'Deactivate the ContextMenuStrip for the Archive ListBox, because we wan´t that the user can delete items if Mainthread is active ;)
            ContextMenuStrip1.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), ContextMenuStrip1, Nothing, "Disable")

            'Assign a Listbox to global Variable
            myListBox = lbArchiveList

            'Count the Archives and set the Value
            Files2XtractCounter = myListBox.Items.Count

            For i = 0 To myListBox.Items.Count - 1

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
                                    Dim entryFolder As String = Path.GetDirectoryName(entry.Key)

                                    'Get the entry without Path
                                    Dim entryFile As String = Path.GetFileName(entry.Key)

                                    'If Extract FullPath is enabled and Overwrite (File) is disabled, automatically generate a new File Name (It´s easier to handle :))
                                    If cbFullPath.Checked And Not cbOverwrite.Checked Then
                                        If CheckPath(FullPath, "File") Then
                                            z += 1
                                            FullPath = Path.Combine(DestDir, entryFolder, "(" & z & ")-" & entryFile)
                                            logger.LogMessage(language.GetString(33, "Datei ") & " " & entry.Key & " " & language.GetString(34, " existiert.") & " " & language.GetString(35, "Neuer Name: ") & FullPath)
                                            entry.WriteToFile(FullPath)
                                        End If
                                        entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = False})

                                        'If Extract Full Path AND Overwrite enabled, do the normal Thing :)
                                    ElseIf cbFullPath.Checked AndAlso cbOverwrite.Checked Then
                                        archive.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = True})

                                        'If Overwrite is enabled, overwrite the file, but DON`T extract the full path ;)
                                    ElseIf cbOverwrite.Checked Then
                                        entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = False, .Overwrite = True})

                                    Else
                                        If CheckPath(Path.Combine(DestDir & "\" & entryFile), "File") Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(DestDir, "(" & z & ")-" & entryFile)
                                            entry.WriteToFile(FullPath)
                                        End If
                                        entry.WriteToDirectory(DestDir)

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
                                    Dim entryFile As String = Path.GetFileName(entry.Key)

                                    If cbFullPath.Checked And Not cbOverwrite.Checked Then
                                        If CheckPath(FullPath, "File") Then
                                            z += 1
                                            FullPath = Path.Combine(DestDir, entryFolder, "(" & z & ")-" & entryFile)
                                            logger.LogMessage("File " & entry.Key & " exists, new Filename: " & FullPath)
                                            entry.WriteToFile(FullPath)
                                        End If
                                        entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = False})

                                        'If Extract Full Path AND Overwrite enabled, do the normal Thing :)
                                    ElseIf cbFullPath.Checked AndAlso cbOverwrite.Checked Then
                                        archive.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = True, .Overwrite = True})

                                        'If Overwrite is enabled, overwrite the file, but DON`T extract the full path ;)
                                    ElseIf cbOverwrite.Checked Then
                                        entry.WriteToDirectory(DestDir, New ExtractionOptions() With {.ExtractFullPath = False, .Overwrite = True})

                                    Else
                                        If CheckPath(FullPath, "File") Then
                                            z += 1
                                            FullPath = System.IO.Path.Combine(tbDestDir.Text, "(" & z & ")-" & entry.Key)
                                        End If
                                        logger.LogMessage(language.GetString(33, "Datei ") & " " & entry.Key & " " & language.GetString(34, " existiert.") & " " & language.GetString(35, "Neuer Name: ") & FullPath)
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
                logger.LogMessage(language.GetString(14, "Überschreiben") & " " & language.GetString(31, " deaktiviert. ") & " " & fI.Name & " " & language.GetString(36, " kann nicht entpackt werden"))
            End If
            ErrLog.WriteErrorLogs(LogsCategory.Exception, fex)
        Catch ex As Exception
            logger.LogMessage(ex.Message)
                ErrLog.WriteErrorLogs(LogsCategory.Exception, ex)
            End Try
        ClearControls()
        FileCounter = 0
        XtractedParts = 0
        ContextMenuStrip1.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), ContextMenuStrip1, Nothing, "Enable")

    End Sub

#End Region

#Region "Helper Functions"

#Region "Language"

    Public Sub InitLanguage()
        ' Load the selected language, the standart language is "de-German" !
        If SelectedLanguage Is "" Then
            If Preferences Is Nothing Or Preferences.Language = "" Then
                SelectedLanguage = "German"
                language.LoadLanguage(SelectedLanguage)
            Else
                language.LoadLanguage(Preferences.Language)
            End If
        Else
            language.LoadLanguage(SelectedLanguage)
        End If

        'Change all controls (names, text, etc) to the selected language
        ChangeLanguage()
    End Sub

    Private Sub LanguageClickEventHandler(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Get the selected language from sender (mnuChangeLang)
        setLanguage(sender.ToString)

        'Log the language change
        logger.LogMessage(language.GetString(2, "Ausgewählte Sprache:") & " " & SelectedLanguage)
    End Sub

    Friend Sub setLanguage(langName As String)
        SelectedLanguage = langName
        Preferences.Language = SelectedLanguage
        InitLanguage()
    End Sub

    Public Sub ChangeLanguage()
        'Menu
        mnuChangeLang.Text = language.GetString(1, "Sprache wechseln")

        'Checkbox
        cbArchivePath.Text = language.GetString(12, "Wie Archiveordner")
        cbFullPath.Text = language.GetString(13, "Entpacke ganzen Pfad")
        cbOverwrite.Text = language.GetString(14, "Überschreiben")

        'Buttons
        btnAddPath.Text = language.GetString(6, "Pfad hinzufügen")
        btnRmvPath.Text = language.GetString(7, "Pfad entfernen")
        btnDestDir.Text = language.GetString(8, "Zielordner")
        btnScanFiles.Text = language.GetString(9, "Archive suchen")
        btnXtract.Text = language.GetString(10, "Entpacken")
        btnCancelThread.Text = language.GetString(43, "Pause")

        'Groupboxes
        grpScanFolders.Text = language.GetString(4, "Suchpfade")
        grpXtractOpt.Text = language.GetString(5, "Extrahier Einstellungen")
        grpArcInf.Text = language.GetString(16, "Extrahier Informationen")
        grpFilesList.Text = language.GetString(15, "Archiv-Liste")

        'Labels
        Label6.Text = language.GetString(17, "Aktuelles Archiv:")
        Label7.Text = language.GetString(18, "Entpacke Volume:")
        Label1.Text = language.GetString(19, "Aktueller Eintrag:")
        Label3.Text = language.GetString(20, "Größe komprimiert:")
        Label2.Text = language.GetString(21, "Größe gesamt:")
        Label5.Text = language.GetString(22, "Entpackte Dateien:")
        Label4.Text = language.GetString(23, "Fortschritt:")
        lblDest.Text = language.GetString(8, "Zielordner")
    End Sub
#End Region

#Region "Settings"
    Private Sub LoadSettings()
        ' Load Preferences
        Preferences = If(Preferences.LoadPreferences(logger, ErrLog), Nothing)

        If Preferences Is Nothing Then
            Preferences = New Preferences()
        End If

        'If something goes wrong with the preferences, e.g. new options after an update,
        'the "DeserializedWithErrors" Event will be thrown and the standart options from preferences will be loaded
        If Preferences.DeserializedWithErrors Then
            logger.LogMessage(language.GetString(37, "Einstellungen: Deserialize Fehler!"))
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

        'logger.LogMessage(language.GetString(38, "Einstellungen: Erfolgreich geladen"))
    End Sub

    Private Sub AddEventsToForm()
        If Not DesignMode AndAlso Me IsNot Nothing Then
            AddHandler Me.FormClosing, AddressOf Mainform_FormClosing
        End If
    End Sub

    Private Sub LoadFormState()
        Try
            Left = Convert.ToInt32(Preferences.FormLocationX)
            Top = Convert.ToInt32(Preferences.FormLocationY)
            'Width = Convert.ToInt32(Preferences.FormWidth)
            'Height = Convert.ToInt32(Preferences.FormHeight)
            'WindowState = CType([Enum].Parse(GetType(FormWindowState), Preferences.WindowState), FormWindowState)
        Catch ex As Exception
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException)
        End Try
    End Sub

    Private Sub SaveFormState()
        Try
            'Dim bounds As Rectangle = Me.Bounds
            'If Me.WindowState <> FormWindowState.Normal Then
            'Bounds = Me.RestoreBounds
            'End If
            Preferences.FormLocationX = bounds.X.ToString()
            Preferences.FormLocationY = bounds.Y.ToString()
            'Preferences.FormWidth = bounds.Width.ToString()
            'Preferences.FormHeight = bounds.Height.ToString()
            'Preferences.WindowState = Me.WindowState.ToString()
        Catch ex As Exception
            ErrLog.WriteErrorLogs(LogsCategory.Exception, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "Controls"
    Private Delegate Function UpdateControlDelegate(ByVal ctrl As Control, ByVal msg As Object, ByVal action As Object)
    Private Function UpdateControl(ByVal ctrl As Control, ByVal msg As Object, ByVal action As Object)
        Try
            Dim ctrlFullName As String = ctrl.GetType().FullName
            Dim ctrlShortName As String = ctrlFullName.Substring(21)

            Select Case ctrl.GetType()
                Case GetType(Button)
                    If action = "Add" Then
                        ctrl.Text = msg
                    End If
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
                        ArchiveProgressBar.Value = msg
                        Application.DoEvents()
                    ElseIf action = "Clear" Then
                        If ArchiveProgressBar.Value > 0 Then
                            ArchiveProgressBar.Value = 0
                        End If
                    End If
                Case GetType(ContextMenuStrip)
                    If action = "Enable" Then
                        ctrl.Enabled = True
                    Else
                        ctrl.Enabled = False
                    End If
            End Select

        Catch ex As Exception
            WriteErrorLogs(LogsCategory.Exception, ex)
        End Try
    End Function

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

    Private Sub RmvArchive_Click(sender As Object, e As EventArgs) Handles RmvArchive.Click
        If BindingSourceArchives.Count > 0 Then
            BindingSourceArchives.RemoveCurrent()
        End If
    End Sub
#End Region

#Region "Check Functions"
    Private Sub CheckLB()
        If lbScanFolders.Items.Count > 0 Then
            lbScanFolders.SetSelected(0, True)
        End If
        If tbDestDir.Text IsNot "" Then
            tbDestDir.Enabled = True
        End If
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
#End Region

#Region "Other Helper"
    ' Get Files {directory} {recursive} {ext} {word in filename}
    Private Function Get_Files(ByVal directory As String,
                               ByVal recursive As System.IO.SearchOption,
                               ByVal ext As String,
                               ByVal with_word_in_filename As String) As List(Of System.IO.FileInfo)

        Return System.IO.Directory.GetFiles(directory, "*" & If(ext.StartsWith("*"), ext.Substring(1), ext), recursive) _
                           .Where(Function(o) o.ToLower.Contains(with_word_in_filename.ToLower)) _
                           .Select(Function(p) New System.IO.FileInfo(p)).ToList
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
#End Region

#Region "SharpCompressHandler"
    Private Sub Archive_FilePartExtractionBegin(ByVal sender As Object, ByVal e As FilePartExtractionBeginEventArgs)
        If _wait.WaitOne(0) Then 'pause requested?
            logger.LogMessage(language.GetString(10, "Entpacken:") & " " & language.GetString(41, "Angehalten"))
            btnCancelThread.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), btnCancelThread, language.GetString(44, "Weiter"), "Add")
            _wait.WaitOne() 'wait here for continuation
            logger.LogMessage(language.GetString(10, "Entpacken:") & " " & language.GetString(42, "Fortgesetzt"))
            btnCancelThread.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), btnCancelThread, language.GetString(43, "Weiter"), "Add")
        End If
        lblCurArchive.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurArchive, CurrentArchive, "Add")
        lblVolume.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblVolume, e.Name, "Add")
        XtractedParts += 1
    End Sub

    Private Sub Archive_EntryExtractionBegin(ByVal sender As Object, ByVal e As ArchiveExtractionEventArgs(Of IArchiveEntry))
        entryName = e.Item.Key
        entryTotal = e.Item.Size
        entryCompressed = e.Item.CompressedSize
        lblFileFullSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileFullSize, bytesize(entryTotal), "Add")
        lblFileCompSize.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblFileCompSize, bytesize(entryCompressed), "Add")
        lblCurFile.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblCurFile, entryName, "Add")
        logger.LogMessage(language.GetString(46, "Entpacke Volume: ") & " " & entryName)
    End Sub

    Private Sub Archive_EntryExtractionEnd(ByVal sender As Object, ByVal e As ArchiveExtractionEventArgs(Of IArchiveEntry))
        FileCounter += 1
        lblPercentExtracted.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), lblPercentExtracted, FileCounter & " / " & Files2XtractCounter, "Add")
        logger.LogMessage(language.GetString(45, "Entpacken beendet: ") & " " & e.Item.Key)

        If BindingSourceArchives.Contains(e.Item.Key) Then
            Dim i As Integer = BindingSourceArchives.IndexOf(e.Item.Key)
            BindingSourceArchives.RemoveAt(i)
            'End If
        End If
    End Sub

    Private Sub Archive_CompressedBytesRead(ByVal sender As Object, ByVal e As CompressedBytesReadEventArgs)
        TotalBytesRead = e.CompressedBytesRead
        'Set the MAX-Value of the ProgressBar based on "maxParts"
        ArchiveProgressBar.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), ArchiveProgressBar, entryTotal, "Max")
        ArchiveProgressBar.Invoke(New UpdateControlDelegate(AddressOf UpdateControl), ArchiveProgressBar, TotalBytesRead, "DoStep")
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
#End Region
#End Region

End Class

