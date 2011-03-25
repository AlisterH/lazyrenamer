
'    Copyright © Alister Hood, 2011
'
'    This file is part of LazyRenamer.
'    http://code.google.com/p/lazyrenamer
'
'    LazyRenamer is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 2 of the License, or
'    (at your option) any later version.
'
'    LazyRenamer is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with LazyRenamer.  If not, see <http://www.gnu.org/licenses/>.


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btnRename = New System.Windows.Forms.Button
        Me.lblFile = New System.Windows.Forms.Label
        Me.txtNewName = New System.Windows.Forms.TextBox
        Me.btnCopy = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnRename
        '
        Me.btnRename.Enabled = False
        Me.btnRename.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRename.Location = New System.Drawing.Point(115, 83)
        Me.btnRename.Name = "btnRename"
        Me.btnRename.Size = New System.Drawing.Size(82, 24)
        Me.btnRename.TabIndex = 17
        Me.btnRename.Text = "Rename"
        Me.btnRename.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.AllowDrop = True
        Me.lblFile.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFile.Location = New System.Drawing.Point(0, 1)
        Me.lblFile.MinimumSize = New System.Drawing.Size(196, 59)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(196, 59)
        Me.lblFile.TabIndex = 0
        Me.lblFile.Text = "<Drag and drop associated file here>"
        Me.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNewName
        '
        Me.txtNewName.Enabled = False
        Me.txtNewName.Location = New System.Drawing.Point(0, 62)
        Me.txtNewName.Name = "txtNewName"
        Me.txtNewName.Size = New System.Drawing.Size(196, 20)
        Me.txtNewName.TabIndex = 15
        Me.txtNewName.Text = "Enter new name"
        '
        'btnCopy
        '
        Me.btnCopy.Enabled = False
        Me.btnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopy.Location = New System.Drawing.Point(0, 83)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(82, 24)
        Me.btnCopy.TabIndex = 16
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AcceptButton = Me.btnRename
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(196, 107)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnRename)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.txtNewName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(202, 109)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LazyRenamer"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRename As System.Windows.Forms.Button
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents txtNewName As System.Windows.Forms.TextBox
    Friend WithEvents btnCopy As System.Windows.Forms.Button

End Class
