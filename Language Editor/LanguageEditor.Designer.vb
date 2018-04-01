<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LanguageEditor
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lvw = New System.Windows.Forms.ListView()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.editbox = New System.Windows.Forms.TextBox()
        Me.tbValue = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsLngFile = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsLngMaker = New System.Windows.Forms.ToolStripLabel()
        Me.lvwContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctmEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctmDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1.SuspendLayout()
        Me.lvwContext.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(12, 11)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(109, 25)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Load Language File"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lvw
        '
        Me.lvw.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lvw.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvw.BackColor = System.Drawing.Color.LavenderBlush
        Me.lvw.FullRowSelect = True
        Me.lvw.GridLines = True
        Me.lvw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvw.LabelEdit = True
        Me.lvw.Location = New System.Drawing.Point(-1, 45)
        Me.lvw.MultiSelect = False
        Me.lvw.Name = "lvw"
        Me.lvw.Size = New System.Drawing.Size(582, 347)
        Me.lvw.TabIndex = 2
        Me.lvw.UseCompatibleStateImageBehavior = False
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Footlight MT Light", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(469, 13)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(112, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Add Item"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'editbox
        '
        Me.editbox.Location = New System.Drawing.Point(738, 15)
        Me.editbox.Name = "editbox"
        Me.editbox.Size = New System.Drawing.Size(100, 20)
        Me.editbox.TabIndex = 5
        Me.editbox.Visible = False
        '
        'tbValue
        '
        Me.tbValue.Location = New System.Drawing.Point(207, 17)
        Me.tbValue.Name = "tbValue"
        Me.tbValue.Size = New System.Drawing.Size(256, 20)
        Me.tbValue.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(136, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "New Value:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.ToolStripLabel2, Me.ToolStripSeparator1, Me.tsLngFile, Me.ToolStripSeparator2, Me.tsLngMaker})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 395)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(587, 25)
        Me.ToolStrip1.TabIndex = 10
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(75, 22)
        Me.ToolStripLabel1.Text = "Items Count:"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(88, 22)
        Me.ToolStripLabel2.Text = "ToolStripLabel2"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsLngFile
        '
        Me.tsLngFile.Name = "tsLngFile"
        Me.tsLngFile.Size = New System.Drawing.Size(0, 22)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsLngMaker
        '
        Me.tsLngMaker.Name = "tsLngMaker"
        Me.tsLngMaker.Size = New System.Drawing.Size(88, 22)
        Me.tsLngMaker.Text = "ToolStripLabel3"
        '
        'lvwContext
        '
        Me.lvwContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctmEdit, Me.ctmDelete})
        Me.lvwContext.Name = "lvwContext"
        Me.lvwContext.Size = New System.Drawing.Size(153, 70)
        Me.lvwContext.Text = "Eintrag bearbeiten"
        '
        'ctmEdit
        '
        Me.ctmEdit.Name = "ctmEdit"
        Me.ctmEdit.Size = New System.Drawing.Size(152, 22)
        Me.ctmEdit.Text = "Change value"
        '
        'ctmDelete
        '
        Me.ctmDelete.Name = "ctmDelete"
        Me.ctmDelete.Size = New System.Drawing.Size(152, 22)
        Me.ctmDelete.Text = "Delete Value"
        '
        'LanguageEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(587, 420)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbValue)
        Me.Controls.Add(Me.editbox)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.lvw)
        Me.Controls.Add(Me.Button1)
        Me.Name = "LanguageEditor"
        Me.Text = "LanguageEditor"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.lvwContext.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lvw As System.Windows.Forms.ListView
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents editbox As System.Windows.Forms.TextBox
    Friend WithEvents tbValue As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lvwContext As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctmEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctmDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsLngFile As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsLngMaker As System.Windows.Forms.ToolStripLabel
End Class
