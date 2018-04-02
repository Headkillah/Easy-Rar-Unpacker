<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Mainform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.OFD_Zip = New System.Windows.Forms.OpenFileDialog()
        Me.lblCurArchive = New System.Windows.Forms.Label()
        Me.btnAddPath = New System.Windows.Forms.Button()
        Me.FBD = New System.Windows.Forms.FolderBrowserDialog()
        Me.lbArchiveList = New System.Windows.Forms.ListBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RmvArchive = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnXtract = New System.Windows.Forms.Button()
        Me.lblDest = New System.Windows.Forms.Label()
        Me.tbDestDir = New System.Windows.Forms.TextBox()
        Me.btnDestDir = New System.Windows.Forms.Button()
        Me.DestFoldBrow = New System.Windows.Forms.FolderBrowserDialog()
        Me.cbArchivePath = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblCurFile = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblFileCompSize = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblPercentExtracted = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblFileFullSize = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbLog = New System.Windows.Forms.ListBox()
        Me.grpLog = New System.Windows.Forms.GroupBox()
        Me.grpArcInf = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pbArchiveProgress = New EasyUnpacker.CustomProgressBar()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grpScanFolders = New System.Windows.Forms.GroupBox()
        Me.btnRmvPath = New System.Windows.Forms.Button()
        Me.lbScanFolders = New System.Windows.Forms.ListBox()
        Me.btnScanFiles = New System.Windows.Forms.Button()
        Me.cbFullPath = New System.Windows.Forms.CheckBox()
        Me.cbOverwrite = New System.Windows.Forms.CheckBox()
        Me.grpXtractOpt = New System.Windows.Forms.GroupBox()
        Me.btnCancelThread = New System.Windows.Forms.Button()
        Me.grpFilesList = New System.Windows.Forms.GroupBox()
        Me.BindingSourceArchives = New System.Windows.Forms.BindingSource(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuChangeLang = New System.Windows.Forms.ToolStripMenuItem()
        Me.Process1 = New System.Diagnostics.Process()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpLog.SuspendLayout()
        Me.grpArcInf.SuspendLayout()
        Me.grpScanFolders.SuspendLayout()
        Me.grpXtractOpt.SuspendLayout()
        Me.grpFilesList.SuspendLayout()
        CType(Me.BindingSourceArchives, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OFD_Zip
        '
        Me.OFD_Zip.Filter = "Zip Files|*zip"
        Me.OFD_Zip.Multiselect = True
        '
        'lblCurArchive
        '
        Me.lblCurArchive.AutoSize = True
        Me.lblCurArchive.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurArchive.Location = New System.Drawing.Point(152, 29)
        Me.lblCurArchive.Name = "lblCurArchive"
        Me.lblCurArchive.Size = New System.Drawing.Size(35, 14)
        Me.lblCurArchive.TabIndex = 1
        Me.lblCurArchive.Text = "...."
        '
        'btnAddPath
        '
        Me.btnAddPath.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnAddPath.Location = New System.Drawing.Point(39, 19)
        Me.btnAddPath.Name = "btnAddPath"
        Me.btnAddPath.Size = New System.Drawing.Size(127, 23)
        Me.btnAddPath.TabIndex = 16
        Me.btnAddPath.Text = "Add Path"
        Me.btnAddPath.UseVisualStyleBackColor = True
        '
        'FBD
        '
        Me.FBD.ShowNewFolderButton = False
        '
        'lbArchiveList
        '
        Me.lbArchiveList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbArchiveList.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lbArchiveList.FormattingEnabled = True
        Me.lbArchiveList.HorizontalScrollbar = True
        Me.lbArchiveList.ItemHeight = 14
        Me.lbArchiveList.Location = New System.Drawing.Point(6, 14)
        Me.lbArchiveList.Name = "lbArchiveList"
        Me.lbArchiveList.Size = New System.Drawing.Size(384, 116)
        Me.lbArchiveList.TabIndex = 18
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RmvArchive})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(161, 26)
        '
        'RmvArchive
        '
        Me.RmvArchive.Name = "RmvArchive"
        Me.RmvArchive.Size = New System.Drawing.Size(160, 22)
        Me.RmvArchive.Text = "Remove Archive"
        '
        'btnXtract
        '
        Me.btnXtract.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnXtract.Location = New System.Drawing.Point(252, 19)
        Me.btnXtract.Name = "btnXtract"
        Me.btnXtract.Size = New System.Drawing.Size(83, 23)
        Me.btnXtract.TabIndex = 20
        Me.btnXtract.Text = "Extract"
        Me.btnXtract.UseVisualStyleBackColor = True
        '
        'lblDest
        '
        Me.lblDest.AutoSize = True
        Me.lblDest.Location = New System.Drawing.Point(3, 65)
        Me.lblDest.Name = "lblDest"
        Me.lblDest.Size = New System.Drawing.Size(91, 14)
        Me.lblDest.TabIndex = 22
        Me.lblDest.Text = "Destination:"
        '
        'tbDestDir
        '
        Me.tbDestDir.Location = New System.Drawing.Point(100, 62)
        Me.tbDestDir.Name = "tbDestDir"
        Me.tbDestDir.Size = New System.Drawing.Size(345, 20)
        Me.tbDestDir.TabIndex = 23
        '
        'btnDestDir
        '
        Me.btnDestDir.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnDestDir.Location = New System.Drawing.Point(6, 19)
        Me.btnDestDir.Name = "btnDestDir"
        Me.btnDestDir.Size = New System.Drawing.Size(104, 23)
        Me.btnDestDir.TabIndex = 24
        Me.btnDestDir.Text = "Destination"
        Me.btnDestDir.UseVisualStyleBackColor = True
        '
        'cbArchivePath
        '
        Me.cbArchivePath.AutoSize = True
        Me.cbArchivePath.Location = New System.Drawing.Point(6, 94)
        Me.cbArchivePath.Name = "cbArchivePath"
        Me.cbArchivePath.Size = New System.Drawing.Size(131, 18)
        Me.cbArchivePath.TabIndex = 25
        Me.cbArchivePath.Text = "Same as Archive"
        Me.cbArchivePath.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 14)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Processing File:"
        '
        'lblCurFile
        '
        Me.lblCurFile.AutoSize = True
        Me.lblCurFile.Location = New System.Drawing.Point(152, 87)
        Me.lblCurFile.Name = "lblCurFile"
        Me.lblCurFile.Size = New System.Drawing.Size(35, 14)
        Me.lblCurFile.TabIndex = 28
        Me.lblCurFile.Text = "...."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 14)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Compressed Size:"
        '
        'lblFileCompSize
        '
        Me.lblFileCompSize.AutoSize = True
        Me.lblFileCompSize.Location = New System.Drawing.Point(152, 116)
        Me.lblFileCompSize.Name = "lblFileCompSize"
        Me.lblFileCompSize.Size = New System.Drawing.Size(35, 14)
        Me.lblFileCompSize.TabIndex = 30
        Me.lblFileCompSize.Text = "...."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 168)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 14)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Extracted Files:"
        '
        'lblPercentExtracted
        '
        Me.lblPercentExtracted.AutoSize = True
        Me.lblPercentExtracted.Location = New System.Drawing.Point(152, 168)
        Me.lblPercentExtracted.Name = "lblPercentExtracted"
        Me.lblPercentExtracted.Size = New System.Drawing.Size(35, 14)
        Me.lblPercentExtracted.TabIndex = 32
        Me.lblPercentExtracted.Text = "...."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(140, 14)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "Processing Archive:"
        '
        'lblFileFullSize
        '
        Me.lblFileFullSize.AutoSize = True
        Me.lblFileFullSize.Location = New System.Drawing.Point(151, 142)
        Me.lblFileFullSize.Name = "lblFileFullSize"
        Me.lblFileFullSize.Size = New System.Drawing.Size(35, 14)
        Me.lblFileFullSize.TabIndex = 37
        Me.lblFileFullSize.Text = "...."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 14)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "Full Size:"
        '
        'lbLog
        '
        Me.lbLog.BackColor = System.Drawing.SystemColors.ControlLight
        Me.lbLog.FormattingEnabled = True
        Me.lbLog.HorizontalScrollbar = True
        Me.lbLog.ItemHeight = 14
        Me.lbLog.Location = New System.Drawing.Point(6, 18)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.Size = New System.Drawing.Size(384, 102)
        Me.lbLog.TabIndex = 35
        '
        'grpLog
        '
        Me.grpLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpLog.Controls.Add(Me.lbLog)
        Me.grpLog.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpLog.Location = New System.Drawing.Point(436, 294)
        Me.grpLog.Name = "grpLog"
        Me.grpLog.Size = New System.Drawing.Size(396, 129)
        Me.grpLog.TabIndex = 36
        Me.grpLog.TabStop = False
        Me.grpLog.Text = "Log"
        '
        'grpArcInf
        '
        Me.grpArcInf.Controls.Add(Me.Label4)
        Me.grpArcInf.Controls.Add(Me.pbArchiveProgress)
        Me.grpArcInf.Controls.Add(Me.lblVolume)
        Me.grpArcInf.Controls.Add(Me.Label6)
        Me.grpArcInf.Controls.Add(Me.lblCurArchive)
        Me.grpArcInf.Controls.Add(Me.Label7)
        Me.grpArcInf.Controls.Add(Me.lblFileCompSize)
        Me.grpArcInf.Controls.Add(Me.Label5)
        Me.grpArcInf.Controls.Add(Me.Label3)
        Me.grpArcInf.Controls.Add(Me.lblFileFullSize)
        Me.grpArcInf.Controls.Add(Me.lblPercentExtracted)
        Me.grpArcInf.Controls.Add(Me.lblCurFile)
        Me.grpArcInf.Controls.Add(Me.Label2)
        Me.grpArcInf.Controls.Add(Me.Label1)
        Me.grpArcInf.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpArcInf.Location = New System.Drawing.Point(12, 149)
        Me.grpArcInf.Name = "grpArcInf"
        Me.grpArcInf.Size = New System.Drawing.Size(418, 274)
        Me.grpArcInf.TabIndex = 37
        Me.grpArcInf.TabStop = False
        Me.grpArcInf.Text = "Extract Informations"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 197)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(133, 14)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Extraction Status:"
        '
        'pbArchiveProgress
        '
        Me.pbArchiveProgress.ForeColor = System.Drawing.Color.AliceBlue
        Me.pbArchiveProgress.Location = New System.Drawing.Point(151, 197)
        Me.pbArchiveProgress.Name = "pbArchiveProgress"
        Me.pbArchiveProgress.Size = New System.Drawing.Size(242, 17)
        Me.pbArchiveProgress.TabIndex = 41
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(151, 57)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(35, 14)
        Me.lblVolume.TabIndex = 40
        Me.lblVolume.Text = "...."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 57)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(133, 14)
        Me.Label7.TabIndex = 39
        Me.Label7.Text = "Extracting Volume:"
        '
        'grpScanFolders
        '
        Me.grpScanFolders.Controls.Add(Me.btnRmvPath)
        Me.grpScanFolders.Controls.Add(Me.lbScanFolders)
        Me.grpScanFolders.Controls.Add(Me.btnAddPath)
        Me.grpScanFolders.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpScanFolders.Location = New System.Drawing.Point(6, 27)
        Me.grpScanFolders.Name = "grpScanFolders"
        Me.grpScanFolders.Size = New System.Drawing.Size(369, 116)
        Me.grpScanFolders.TabIndex = 35
        Me.grpScanFolders.TabStop = False
        Me.grpScanFolders.Text = "Folders to Scan"
        '
        'btnRmvPath
        '
        Me.btnRmvPath.BackColor = System.Drawing.SystemColors.Control
        Me.btnRmvPath.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRmvPath.Location = New System.Drawing.Point(202, 19)
        Me.btnRmvPath.Name = "btnRmvPath"
        Me.btnRmvPath.Size = New System.Drawing.Size(117, 23)
        Me.btnRmvPath.TabIndex = 17
        Me.btnRmvPath.Text = "Remove Path"
        Me.btnRmvPath.UseVisualStyleBackColor = False
        '
        'lbScanFolders
        '
        Me.lbScanFolders.FormattingEnabled = True
        Me.lbScanFolders.ItemHeight = 14
        Me.lbScanFolders.Location = New System.Drawing.Point(6, 48)
        Me.lbScanFolders.Name = "lbScanFolders"
        Me.lbScanFolders.Size = New System.Drawing.Size(356, 60)
        Me.lbScanFolders.TabIndex = 0
        '
        'btnScanFiles
        '
        Me.btnScanFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnScanFiles.Location = New System.Drawing.Point(127, 19)
        Me.btnScanFiles.Name = "btnScanFiles"
        Me.btnScanFiles.Size = New System.Drawing.Size(110, 23)
        Me.btnScanFiles.TabIndex = 1
        Me.btnScanFiles.Text = "Scan Files"
        Me.btnScanFiles.UseVisualStyleBackColor = True
        '
        'cbFullPath
        '
        Me.cbFullPath.AutoSize = True
        Me.cbFullPath.Location = New System.Drawing.Point(153, 94)
        Me.cbFullPath.Name = "cbFullPath"
        Me.cbFullPath.Size = New System.Drawing.Size(145, 18)
        Me.cbFullPath.TabIndex = 39
        Me.cbFullPath.Text = "Extract Full Path"
        Me.cbFullPath.UseVisualStyleBackColor = True
        '
        'cbOverwrite
        '
        Me.cbOverwrite.AutoSize = True
        Me.cbOverwrite.Location = New System.Drawing.Point(328, 94)
        Me.cbOverwrite.Name = "cbOverwrite"
        Me.cbOverwrite.Size = New System.Drawing.Size(89, 18)
        Me.cbOverwrite.TabIndex = 40
        Me.cbOverwrite.Text = "Overwrite"
        Me.cbOverwrite.UseVisualStyleBackColor = True
        '
        'grpXtractOpt
        '
        Me.grpXtractOpt.Controls.Add(Me.btnCancelThread)
        Me.grpXtractOpt.Controls.Add(Me.btnDestDir)
        Me.grpXtractOpt.Controls.Add(Me.btnScanFiles)
        Me.grpXtractOpt.Controls.Add(Me.cbOverwrite)
        Me.grpXtractOpt.Controls.Add(Me.lblDest)
        Me.grpXtractOpt.Controls.Add(Me.btnXtract)
        Me.grpXtractOpt.Controls.Add(Me.cbFullPath)
        Me.grpXtractOpt.Controls.Add(Me.tbDestDir)
        Me.grpXtractOpt.Controls.Add(Me.cbArchivePath)
        Me.grpXtractOpt.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpXtractOpt.Location = New System.Drawing.Point(381, 27)
        Me.grpXtractOpt.Name = "grpXtractOpt"
        Me.grpXtractOpt.Size = New System.Drawing.Size(451, 116)
        Me.grpXtractOpt.TabIndex = 41
        Me.grpXtractOpt.TabStop = False
        Me.grpXtractOpt.Text = "Extract Options"
        '
        'btnCancelThread
        '
        Me.btnCancelThread.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCancelThread.Location = New System.Drawing.Point(350, 19)
        Me.btnCancelThread.Name = "btnCancelThread"
        Me.btnCancelThread.Size = New System.Drawing.Size(84, 23)
        Me.btnCancelThread.TabIndex = 41
        Me.btnCancelThread.Text = "Pause"
        Me.btnCancelThread.UseVisualStyleBackColor = True
        '
        'grpFilesList
        '
        Me.grpFilesList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpFilesList.Controls.Add(Me.lbArchiveList)
        Me.grpFilesList.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpFilesList.Location = New System.Drawing.Point(436, 147)
        Me.grpFilesList.Name = "grpFilesList"
        Me.grpFilesList.Size = New System.Drawing.Size(396, 141)
        Me.grpFilesList.TabIndex = 42
        Me.grpFilesList.TabStop = False
        Me.grpFilesList.Text = "Archives Found"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuChangeLang})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(839, 24)
        Me.MenuStrip1.TabIndex = 43
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuChangeLang
        '
        Me.mnuChangeLang.Name = "mnuChangeLang"
        Me.mnuChangeLang.Size = New System.Drawing.Size(115, 20)
        Me.mnuChangeLang.Text = "Change Language"
        '
        'Process1
        '
        Me.Process1.StartInfo.Domain = ""
        Me.Process1.StartInfo.LoadUserProfile = False
        Me.Process1.StartInfo.Password = Nothing
        Me.Process1.StartInfo.StandardErrorEncoding = Nothing
        Me.Process1.StartInfo.StandardOutputEncoding = Nothing
        Me.Process1.StartInfo.UserName = ""
        Me.Process1.SynchronizingObject = Me
        '
        'Mainform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(839, 427)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.grpFilesList)
        Me.Controls.Add(Me.grpXtractOpt)
        Me.Controls.Add(Me.grpScanFolders)
        Me.Controls.Add(Me.grpArcInf)
        Me.Controls.Add(Me.grpLog)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(765, 466)
        Me.Name = "Mainform"
        Me.Text = "Easy RAR Unpacker"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpLog.ResumeLayout(False)
        Me.grpArcInf.ResumeLayout(False)
        Me.grpArcInf.PerformLayout()
        Me.grpScanFolders.ResumeLayout(False)
        Me.grpXtractOpt.ResumeLayout(False)
        Me.grpXtractOpt.PerformLayout()
        Me.grpFilesList.ResumeLayout(False)
        CType(Me.BindingSourceArchives, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OFD_Zip As OpenFileDialog
    Friend WithEvents lblCurArchive As Label
    Friend WithEvents btnAddPath As Button
    Friend WithEvents FBD As FolderBrowserDialog
    Friend WithEvents lbArchiveList As ListBox
    Friend WithEvents btnXtract As Button
    Friend WithEvents lblDest As Label
    Friend WithEvents tbDestDir As TextBox
    Friend WithEvents btnDestDir As Button
    Friend WithEvents DestFoldBrow As FolderBrowserDialog
    Friend WithEvents cbArchivePath As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblCurFile As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblFileCompSize As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblPercentExtracted As Label
    Friend WithEvents lbLog As ListBox
    Friend WithEvents lblFileFullSize As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents grpLog As GroupBox
    Friend WithEvents grpArcInf As GroupBox
    Friend WithEvents grpScanFolders As GroupBox
    Friend WithEvents lbScanFolders As ListBox
    Friend WithEvents btnScanFiles As Button
    Friend WithEvents btnRmvPath As Button
    Friend WithEvents cbFullPath As CheckBox
    Friend WithEvents cbOverwrite As CheckBox
    Friend WithEvents lblVolume As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents pbArchiveProgress As CustomProgressBar
    Friend WithEvents grpXtractOpt As GroupBox
    Friend WithEvents grpFilesList As GroupBox
    Friend WithEvents btnCancelThread As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents BindingSourceArchives As BindingSource
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents RmvArchive As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnuChangeLang As ToolStripMenuItem
    Friend WithEvents Process1 As Process
End Class
