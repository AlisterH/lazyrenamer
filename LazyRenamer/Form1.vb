Imports VB = Microsoft.VisualBasic
Imports System.IO
Public Class Form1
    Dim FileDrop As String, FilePath As String, FileName As String, FileExtension As String
    Dim MwsrFileIn As String, MwsrFileOut As String, MwsrLine As String, MwsrLabelName As String, MwsrLabelNameNew As String
    Dim TestIfExist As String, FileNameDot As String, BatchFile As String, FilesInDir As String, FileNameTxtbox As String
    Dim foundFileExtension As String, foundFileReadOnly As System.IO.FileInfo
    Dim line As String, startposX As Integer, startposY As Integer
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Restores the program position of the last LazyRenamer session
        If File.Exists(CurDir() & "\LazyRenamer.ini") = True Then
            FileOpen(1, CurDir() & "\LazyRenamer.ini", OpenMode.Input)
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
    End Sub
    Private Sub lblFile_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lblFile.DragEnter
        e.Effect = DragDropEffects.Link
    End Sub
    Private Sub lblFile_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lblFile.DragDrop
        lblFile.BackColor = Color.WhiteSmoke
        'Assign string to file drop label
        Dim fila As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        FileDrop = fila(0)
        lblFile.Text = FileDrop
        txtNewName.Enabled = True
        'Determines the path, name and extension of the dropped file
        FilePath = VB.Left(FileDrop, InStrRev(FileDrop, "\"))
        FileName = VB.Right(FileDrop, VB.Len(FileDrop) - InStrRev(FileDrop, "\"))
        If InStr(FileName, ".") <> 0 Then
            FileNameDot = FileName
            FileName = VB.Left(FileName, InStr(FileName, ".") - 1)
            FileExtension = VB.Right(FileDrop, VB.Len(FileDrop) - InStrRev(FileDrop, ".") + 1)
        End If
        lblFile.Text = FileDrop
        txtNewName.Text = FileName
        txtNewName.Focus()
        btnRename.Enabled = False
        btnCopy.Enabled = False
    End Sub
    Private Sub txtNewName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewName.KeyPress
        'Prevents user from entering illegal file name characters in textbox
        If e.KeyChar = ChrW(44) Or e.KeyChar = ChrW(46) Or e.KeyChar = ChrW(34) Or e.KeyChar = ChrW(42) Or e.KeyChar = ChrW(47) Or e.KeyChar = ChrW(92) Or e.KeyChar = ChrW(58) Or e.KeyChar = ChrW(60) Or e.KeyChar = ChrW(62) Or e.KeyChar = ChrW(63) Then e.Handled = True
    End Sub
    Private Sub txtNewName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewName.KeyUp
        btnRename.Enabled = True
        btnCopy.Enabled = True
        If Trim(txtNewName.Text) = "" Then btnRename.Enabled = False
        'Disables the Rename and Copy buttons if any file with that name and any extension already exists
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            FilesInDir = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, "\"))
            If InStr(FilesInDir, ".") <> 0 Then FilesInDir = VB.Left(FilesInDir, InStr(FilesInDir, ".") - 1)
            If LCase(txtNewName.Text) = LCase(FilesInDir) Then
                btnRename.Enabled = False
                btnCopy.Enabled = False
            End If
        Next
        For Each foundFile As String In My.Computer.FileSystem.GetDirectories(FilePath)
            FilesInDir = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, "\"))
            If InStr(FilesInDir, ".") <> 0 Then FilesInDir = VB.Left(FilesInDir, InStr(FilesInDir, ".") - 1)
            If LCase(txtNewName.Text) = LCase(FilesInDir) Then
                btnRename.Enabled = False
                btnCopy.Enabled = False
            End If
        Next
    End Sub
    Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
        'Rename associated files
        FileName = VB.Right(FileDrop, VB.Len(FileDrop) - InStrRev(FileDrop, "\"))
        If InStr(FileName, ".") <> 0 Then FileName = VB.Left(FileName, InStr(FileName, ".") - 1)
        FileNameTxtbox = VB.Right(txtNewName.Text, VB.Len(txtNewName.Text) - InStrRev(txtNewName.Text, "\"))
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            foundFile = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, "\"))
            If InStr(foundFile, ".") = 0 Then
                If LCase(foundFile) = LCase(FileName) Then
                    'Renames file that doesn't have an extension e.g. "Land" in a Land.*-series
                    Rename(FilePath & foundFile, FilePath & FileNameTxtbox & "#tmp") 'Do we really need the #tmp?  What situations does it save us in?
                End If
            Else
                FilesInDir = VB.Left(foundFile, InStr(foundFile, ".") - 1)
                If FileNameTxtbox <> FileName Then
                    If LCase(FilesInDir) = LCase(FileName) Then
                        foundFileExtension = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, ".") + 1)
                        On Error GoTo 100   'If file is in use by another program
                        If InStr(foundFile, ".") = InStrRev(foundFile, ".") Then
                            'Renames files with a single dot, e.g. "Land.shp" in a Land.*-series
                            Rename(FilePath & foundFile, FilePath & FileNameTxtbox & foundFileExtension & "#tmp") 'Do we really need the #tmp?  What situations does it save us in?
                        Else
                            'Renames files with more-than-one dot, e.g. "Land.shp.xml" in a Land.*-series
                            Rename(FilePath & foundFile, FilePath & FileNameTxtbox & VB.Mid(foundFile, InStr(foundFile, "."), (InStrRev(foundFile, ".") - InStr(foundFile, "."))) & foundFileExtension & "#tmp") 'Do we really need the #tmp?  What situations does it save us in?
                        End If
                    End If
                End If
            End If
        Next
        lblFile.BackColor = Color.WhiteSmoke
        'Assigns the new name to the FileDrop variable
        FileDrop = FilePath & txtNewName.Text & FileExtension
        btnRename.Enabled = False
        btnCopy.Enabled = False
        GoTo 110
100:    'Jumps here when "On Error" occurs
        lblFile.Text = "One or more of the associated files appears to be in use by another program. Aborting."
        lblFile.BackColor = Color.LavenderBlush
110:    'Assigns the new names (removes #tmp suffix) or restores names when error occurs
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            If VB.Right(foundFile, 4) = "#tmp" Then Rename(foundFile, VB.Left(foundFile, VB.Len(foundFile) - 4))
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
        End If
        FileName = lblFile.Text
        If File.Exists(MwsrFileOut) = True Then File.Delete(MwsrFileIn)
        If lblFile.BackColor <> Color.LavenderBlush Then txtNewName.Focus()
        'Updates the file label with the new file name
        If lblFile.Text <> "One or more of the associated files appears to be in use by another program. Aborting." Then
            lblFile.BackColor = Color.WhiteSmoke
            'Updates lblfile.text with the new name ...
            If InStr(FileName, ".") = 0 Then
                lblFile.Text = FilePath & txtNewName.Text
            ElseIf FileNameDot <> FileName Then
                lblFile.Text = FilePath & txtNewName.Text & VB.Mid(FileNameDot, InStr(FileNameDot, "."), VB.Len(FileNameDot))
            Else
                lblFile.Text = FilePath & txtNewName.Text & FileExtension
            End If
        End If
    End Sub
    'We really should share code with btnRename_Click() instead of just copying the code ;)
    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        'Copy associated files
        FileName = VB.Right(FileDrop, VB.Len(FileDrop) - InStrRev(FileDrop, "\"))
        If InStr(FileName, ".") <> 0 Then FileName = VB.Left(FileName, InStr(FileName, ".") - 1)
        FileNameTxtbox = VB.Right(txtNewName.Text, VB.Len(txtNewName.Text) - InStrRev(txtNewName.Text, "\"))
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(FilePath)
            foundFile = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, "\"))
            If InStr(foundFile, ".") = 0 Then
                If LCase(foundFile) = LCase(FileName) Then
                    'Copies file that doesn't have an extension e.g. "Land" in a Land.*-series
                    My.Computer.FileSystem.CopyFile(FilePath & foundFile, FilePath & FileNameTxtbox)
                End If
            Else
                FilesInDir = VB.Left(foundFile, InStr(foundFile, ".") - 1)
                If InStr(foundFile, ".") <> 0 Then FilesInDir = VB.Left(foundFile, InStr(foundFile, ".") - 1)
                If FileNameTxtbox <> FileName Then
                    If LCase(FilesInDir) = LCase(FileName) Then
                        foundFileExtension = VB.Right(foundFile, VB.Len(foundFile) - InStrRev(foundFile, ".") + 1)
                        On Error GoTo 100   'Shouldn't get an error copying, should we?
                        If InStr(foundFile, ".") = InStrRev(foundFile, ".") Then
                            'Copies files with a single dot, e.g. "Land.shp" in a Land.*-series
                            My.Computer.FileSystem.CopyFile(FilePath & foundFile, FilePath & FileNameTxtbox & foundFileExtension)
                        Else
                            'Copies files with more-than-one dot, e.g. "Land.shp.xml" in a Land.*-series
                            My.Computer.FileSystem.CopyFile(FilePath & foundFile, FilePath & FileNameTxtbox & VB.Mid(foundFile, InStr(foundFile, "."), (InStrRev(foundFile, ".") - InStr(foundFile, "."))) & foundFileExtension)
                        End If
                    End If
                End If
            End If
        Next
        lblFile.BackColor = Color.WhiteSmoke
        'Assigns the new name to the FileDrop variable
        FileDrop = FilePath & txtNewName.Text & FileExtension
        btnRename.Enabled = False
        btnCopy.Enabled = False
        GoTo 110
100:    'Jumps here when "On Error" occurs
        lblFile.Text = "Aborting. This seems to be a bug!"
        lblFile.BackColor = Color.LavenderBlush
110:    'Hmmm.  What happens if an error occurs while copying?  Is it possible?
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
        End If
        FileName = lblFile.Text
        If File.Exists(MwsrFileOut) = True Then File.Delete(MwsrFileIn)
        If lblFile.BackColor <> Color.LavenderBlush Then txtNewName.Focus()
        'Updates the file label with the new file name
        If lblFile.Text <> "One or more of the associated files appears to be in use by another program. Aborting." Then
            lblFile.BackColor = Color.WhiteSmoke
            'Updates lblfile.text with the new name ...
            If InStr(FileName, ".") = 0 Then
                lblFile.Text = FilePath & txtNewName.Text
            ElseIf FileNameDot <> FileName Then
                lblFile.Text = FilePath & txtNewName.Text & VB.Mid(FileNameDot, InStr(FileNameDot, "."), VB.Len(FileNameDot))
            Else
                lblFile.Text = FilePath & txtNewName.Text & FileExtension
            End If
        End If
    End Sub
    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Saves the current program position for the next LazyRenamer session
        FileOpen(1, CurDir() & "\LazyRenamer.ini", OpenMode.Output)
        PrintLine(1, "[StartupPosition]")
        PrintLine(1, "startposX = " & Me.Left)
        PrintLine(1, "startposY = " & Me.Top)
        FileClose(1)
    End Sub
End Class

