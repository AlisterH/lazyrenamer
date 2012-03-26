
'    Copyright © Alister Hood, 2011, 2012
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

' Note 1: Windows programs/filesystems are not truly case-sensitive: they allow paths/names to be displayed in a case sensitive manner, 
' but otherwise paths are treated case-insensitively i.e. there cannot be both c:\Test.shp and c:\test.shp.
' We partly cater to this lowest-common-denominator behaviour:
' e.g. we won't allow creating "test.*" if "Test.*" exists
' Note 2: But since version 1.2 we only rename or copy the files with basenames which match the selected file _case sensitively_.
' This is because otherwise we fail if the set of files we are renaming includes two files that only differ by the case of some characters
' e.g. we would try to give the same new name to both "test.shp" and "Test.shp"
' On Windows this situation is unlikely but possible e.g. accessing files on a Samba server which were named from a Linux system.

Imports VB = Microsoft.VisualBasic
Imports System.IO
Public Class Form1
    Dim fila As String(), FileDrop As String, FilePath As String, FileName As String, FileExtension As String
    Dim MwsrFileIn As String, MwsrFileOut As String, MwsrLine As String, MwsrLabelName As String, MwsrLabelNameNew As String
    Dim FilesInDir As String, FileNameTxtbox As String
    Dim foundFileExtension As String, foundFileReadOnly As System.IO.FileInfo
    Dim line As String, startposX As Integer, startposY As Integer
    'Hack: we don't need to use Path.DirectorySeparatorChar for cross-platform support because even though Windows has \ as directory 
    'separator, it also accepts /
    Dim IniFile As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "/LazyRenamer.ini"

    Private Sub Buttons_Disable()
        btnRename.Enabled = False
        btnCopy.Enabled = False
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Restores the program position of the last LazyRenamer session
        'Looks like this doesn't work with some window managers
        If File.Exists(IniFile) = True Then
            FileOpen(1, IniFile, OpenMode.Input)
            On Error GoTo 10
            LineInput(1)
            'The X coordinate
            line = LineInput(1)
            startposX = VB.Right(line, VB.Len(line) - InStrRev(line, " ") + 1)
            Me.Left = startposX
            'The Y coordinate
            line = LineInput(1)
            startposY = VB.Right(line, VB.Len(line) - InStrRev(line, " ") + 1)
            Me.Top = startposY
10:         FileClose(1)
        End If
        If Environment.GetCommandLineArgs().Length > 1 Then
            'Get label text from file passed as a command line argument
            lblLoad_File(My.Application.CommandLineArgs())
        End If
    End Sub
    Private Sub lblFile_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lblFile.DragEnter
        e.Effect = DragDropEffects.Link
    End Sub
    Private Sub lblLoad_File(ByVal fila As Object)
        FileDrop = fila(0)
        'Determines the path, name and extension of the dropped file
        'Shouldn't something like this work, too?
        'FilePath = Path.Combine(Path.GetDirectoryName(FileDrop),Path.DirectorySeparatorChar)
        FilePath = VB.Left(FileDrop, InStrRev(FileDrop, Path.DirectorySeparatorChar))
        FileName = Path.GetFileName(FileDrop) 'Do we really need this variable, or could we work directly with txtNewName.Text?
        'Note we can't use Path.GetExtension() because it handles files with multiple extensions incorrectly
        'Files with multiple extensions are probably more common in fields where we would use lazyrename than are random dots in the middle of a name.
        If InStr(FileName, ".") <> 0 Then
            FileExtension = VB.Right(FileName, VB.Len(FileName) - InStr(FileName, ".") + 1)
            FileName = VB.Left(FileName, InStr(FileName, ".") - 1) 'Do we really need this variable, or could we work directly with txtNewName.Text?
        End If
        'Updates gui
        lblFile.BackColor = Color.WhiteSmoke
        lblFile.Text = FileDrop
        txtNewName.Enabled = True
        txtNewName.Text = FileName
        lblFile.Focus() 'hack: if a file is dropped onto lazyrename twice we need to unfocus txtNewName before focusing it, so that the filename is selected
        txtNewName.Focus() 'maybe we should also select the filename after renaming or copying
        Buttons_Disable()
    End Sub
    Private Sub lblFile_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lblFile.DragDrop
        'Get label text from file drag-and-dropped onto window
        fila = CType(e.Data.GetData(DataFormats.FileDrop), String())
        lblLoad_File(fila)
        Me.Activate()
    End Sub
    Private Sub txtNewName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewName.KeyPress
        'Prevents user from entering illegal file name characters in textbox
        'Does not process pasted text!
        'N.B. Depending on the filesystem these may not all be illegal, but we can't check against Path.GetInvalidPathChars(), as this does 
        'not actually check what characters are illegal on the particular filesystem we are writing to.
        'so we restrict characters to the lowest-common-denominator: Windows
        'Characters below are (in order): " * / \ : < > ? |
        If e.KeyChar = ChrW(34) Or e.KeyChar = ChrW(42) Or e.KeyChar = ChrW(47) Or e.KeyChar = ChrW(92) Or e.KeyChar = ChrW(58) Or e.KeyChar = ChrW(60) Or e.KeyChar = ChrW(62) Or e.KeyChar = ChrW(63) Or e.KeyChar = ChrW(124) Then e.Handled = True
    End Sub
    Private Sub txtNewName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewName.KeyUp
        Buttons_Disable()
        'disallow file base name that is blank or has trailing spaces, so we can rename a file with no extension reliably
        'does not detect if the user "cuts" the whole file name!
        If Trim(VB.Right(txtNewName.Text, 1)) = "" Then Exit Sub
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            'disallow file base name if a file with the same base name (and any extension) already exists
            If check_NewName(foundFile) = False Then Exit Sub
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetDirectories(FilePath)
            'disallow file base name if a folder with the same base name (and any extension) already exists
            If check_NewName(foundFile) = False Then Exit Sub
        Next
        btnRename.Enabled = True
        btnCopy.Enabled = True
    End Sub
    Private Function check_NewName(ByVal foundFile As String) As Boolean
        FilesInDir = Path.GetFileName(foundFile)
        If InStr(FilesInDir, ".") <> 0 Then FilesInDir = VB.Left(FilesInDir, InStr(FilesInDir, ".") - 1)
        If LCase(txtNewName.Text) = LCase(FilesInDir) Then Exit Function 'Returns False
        Return True
    End Function
    Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
        btn_Click(False)
    End Sub
    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        btn_Click(True)
    End Sub
    Private Sub btn_Click(ByVal Copy_TrueFalse As Boolean)
        FileNameTxtbox = Path.GetFileName(txtNewName.Text) ' Why do we need this variable?
        On Error GoTo 100   'If file we try to rename is in use by another program
        'Renames or Copies all associated files
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            foundFile = Path.GetFileName(foundFile)
            CopyOrRename(foundFile, Copy_TrueFalse, False)
        Next
        'Renames or Copies all associated directories
        For Each foundFile As String In My.Computer.FileSystem.GetDirectories(FilePath)
            foundFile = Path.GetFileName(foundFile)
            CopyOrRename(foundFile, Copy_TrueFalse, True)
        Next
        'Updates the Layer Name value in MapWindow layer properties files
        MwsrFileIn = FilePath & txtNewName.Text & ".mwsr"
        If File.Exists(MwsrFileIn) Then
            foundFileReadOnly = My.Computer.FileSystem.GetFileInfo(MwsrFileIn)
            If foundFileReadOnly.IsReadOnly = False Then
                MwsrFileOut = FilePath & FileNameTxtbox & ".mwsr2"
                FileOpen(1, MwsrFileIn, OpenMode.Input)
                FileOpen(2, MwsrFileOut, OpenMode.Output)
                MwsrLabelName = "Layer Name=" & ChrW(34) & FileName & ChrW(34)
                MwsrLabelNameNew = "Layer Name=" & ChrW(34) & txtNewName.Text & ChrW(34)
                Do
                    MwsrLine = LineInput(1)
                    If InStr(MwsrLine, MwsrLabelName) <> 0 Then MwsrLine = Replace(MwsrLine, MwsrLabelName, MwsrLabelNameNew)
                    PrintLine(2, MwsrLine)
                Loop Until VB.EOF(1) = True
                FileClose(1, 2)
                File.Delete(MwsrFileIn)
                Rename(MwsrFileOut, VB.Left(MwsrFileOut, VB.Len(MwsrFileOut) - 1))
            End If
            'Deletes old file as we seem to have been successful
            If File.Exists(MwsrFileOut) = True Then File.Delete(MwsrFileIn)
        End If
        'Removes #tmp as we seem to have been successful
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            If VB.Right(foundFile, 4) = "#tmp" Then Rename(foundFile, VB.Left(foundFile, VB.Len(foundFile) - 4))
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetDirectories(FilePath)
            If VB.Right(foundFile, 4) = "#tmp" Then Rename(foundFile, VB.Left(foundFile, VB.Len(foundFile) - 4))
        Next
        'Updates the FileName variable
        fila(0) = FilePath & txtNewName.Text & FileExtension
        'Updates Gui
        lblLoad_File(fila)
        Exit Sub
100:    'Jumps here when "On Error" occurs
        lblFile.Text = "One or more of the associated files appears to be in use by another program. Close the file and try again."
        lblFile.BackColor = Color.LavenderBlush
        'Should we restore any files with a #tmp to their original filenames if there has been an error?
        'At the moment the user can close any files in use and click rename again to finish the failed attempt.  This is better in a way.
    End Sub
    Private Sub CopyOrRename(ByVal foundFile As String, ByVal Copy_TrueFalse As Boolean, ByVal IsDirectory As Boolean)
        If InStr(foundFile, ".") = 0 Then
            FilesInDir = foundFile
            foundFileExtension = ""
        Else
            FilesInDir = VB.Left(foundFile, InStr(foundFile, ".") - 1)
            foundFileExtension = VB.Right(foundFile, VB.Len(foundFile) - InStr(foundFile, ".") + 1)
        End If
        'ToDo: Should move this test to before we start trying to CopyOrRename files for improved performance
        If FileNameTxtbox <> FileName Then 'Q - is this test really needed? A - it won't be once we process _pasted_ text (instead of only typed text).
            If FilesInDir = FileName Then 'need to be case sensitive here as it might not be a windows filesystem (see note 2 at top)
                If Copy_TrueFalse = False Then
                    Rename(FilePath & foundFile, FilePath & FileNameTxtbox & foundFileExtension & "#tmp") 'Is the #tmp really wise?
                Else
                    If (IsDirectory = False) Then
                        My.Computer.FileSystem.CopyFile(FilePath & foundFile, FilePath & FileNameTxtbox & foundFileExtension & "#tmp") 'Is the #tmp really wise?  Is it needed for copying as well as renaming?
                    Else
                        My.Computer.FileSystem.CopyDirectory(FilePath & foundFile, FilePath & FileNameTxtbox & foundFileExtension & "#tmp") 'Is the #tmp really wise?  Is it needed for copying as well as renaming?
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Saves the current program position for the next LazyRenamer session
        FileOpen(1, IniFile, OpenMode.Output)
        PrintLine(1, "[StartupPosition]")
        PrintLine(1, "startposX = " & Me.Left)
        PrintLine(1, "startposY = " & Me.Top)
        FileClose(1)
    End Sub
End Class

