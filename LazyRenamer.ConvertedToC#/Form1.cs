//    Copyright © Alister Hood, 2011, 2012
//
//    This file is part of LazyRenamer.
//    https://github.com/AlisterH/lazyrenamer
//
//    LazyRenamer is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your option) any later version.
//
//    LazyRenamer is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with LazyRenamer.  If not, see <http://www.gnu.org/licenses/>.

// Note 1: Windows programs/filesystems are not truly case-sensitive: they allow paths/names to be displayed in a case sensitive manner, 
// but otherwise paths are treated case-insensitively i.e. there cannot be both c:\Test.shp and c:\test.shp.
// We partly cater to this lowest-common-denominator behaviour:
// e.g. we won't allow creating "test.*" if "Test.*" exists
// Note 2: But since version 1.2 we only rename or copy the files with basenames which match the selected file _case sensitively_.
// This is because otherwise we fail if the set of files we are renaming includes two files that only differ by the case of some characters
// e.g. we would try to give the same new name to both "test.shp" and "Test.shp"
// On Windows this situation is unlikely but possible e.g. accessing files on a Samba server which were named from a Linux system.

//using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
namespace LazyRenamer
{
	public partial class Form1
	{
		string[] fila;
		string FileDrop;
		string FilePath;
		string FileName;
		string FileExtension;
/*		string MwsrFileIn;
		string MwsrFileOut;
		string MwsrLine;
		string MwsrLabelName;
		string MwsrLabelNameNew;
*/		string FilesInDir;
		string FileNameTxtbox;
		string foundFileName;
		string foundFileExtension;
/*		FileInfo foundFileReadOnly;
		string line;
		int startposX;
		int startposY;
		//Hack: we don't need to use Path.DirectorySeparatorChar for cross-platform support because even though Windows has \ as directory 
		//separator, it also accepts /
		string IniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/LazyRenamer.ini";
*/
		private void Buttons_Disable()
		{
			btnRename.Enabled = false;
			btnCopy.Enabled = false;
		}
		private void Form1_Load(System.Object sender, System.EventArgs e)
		{
			//Restores the program position of the last LazyRenamer session
			//Looks like this doesn't work with some window managers
/*			if (File.Exists(IniFile) == true) {
				FileSystem.FileOpen(1, IniFile, OpenMode.Input);
				//On Error GoTo 10  //(VB)
				FileSystem.LineInput(1);
				//The X coordinate
				line = FileSystem.LineInput(1);
				
				//startposX = Convert.ToInt16(Strings.Right(line, Strings.Len(line) - Strings.InStrRev(line, " ") + 1));
				//this.Left = startposX;
				//The Y coordinate
				//line = FileSystem.LineInput(1);
				//startposY = Convert.ToInt16(Strings.Right(line, Strings.Len(line) - Strings.InStrRev(line, " ") + 1));
				
				//startposX = Convert.ToInt16(line.Substring(Convert.ToInt16(Strings.InStrRev(line, " ")),line.Length-Convert.ToInt16(Strings.InStrRev(line, " "))));
				//this.Left = startposX;
				//The Y coordinate
				//line = FileSystem.LineInput(1);
				//startposY = Convert.ToInt16(line.Substring(Convert.ToInt16(Strings.InStrRev(line, " ")),line.Length-Convert.ToInt16(Strings.InStrRev(line, " "))));
				
				startposX = Convert.ToInt16(line.Substring(line.LastIndexOf(" ")));
				this.Left = startposX;
				//The Y coordinate
				line = FileSystem.LineInput(1);
				startposY = Convert.ToInt16(line.Substring(line.LastIndexOf(" ")));
				this.Top = startposY;
				//10:  //(VB)
				FileSystem.FileClose(1);

			}
			if (Environment.GetCommandLineArgs().Length > 1) {
				//Get label text from file passed as a command line argument
				//How to do this with C# version?
				//lblLoad_File(LazyRenamer.My.MyProject.Application.CommandLineArgs());
			}
*/		}
		private void lblFile_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}
		private void lblLoad_File(string[] fila)
		{
			FileDrop = fila[0];
			//Determines the path, name and extension of the dropped file
			//Shouldn't something like this work, too (VB)?
			//FilePath = Path.Combine(Path.GetDirectoryName(FileDrop),Path.DirectorySeparatorChar)
			FilePath = FileDrop.Substring(0, FileDrop.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			FileName = Path.GetFileName(FileDrop); //Do we really need this variable, or could we work directly with txtNewName.Text?
			//Note we can't use Path.GetExtension() (VB) because it handles files with multiple extensions incorrectly
			//Files with multiple extensions are probably more common in fields where we would use lazyrename than are random dots in the middle of a name.
			if (FileName.Contains(".")) {
				FileExtension = FileName.Substring(FileName.IndexOf("."));
				FileName = FileName.Substring(0, FileName.IndexOf("."));	//Do we really need this variable, or could we work directly with txtNewName.Text?
			}
			//Updates gui
			lblFile.BackColor = Color.WhiteSmoke;
			lblFile.Text = FileDrop;
			txtNewName.Enabled = true;
			txtNewName.Text = FileName;
			lblFile.Focus(); //hack: if a file is dropped onto lazyrename twice we need to unfocus txtNewName before focusing it, so that the filename is selected
			txtNewName.Focus(); //maybe we should also select the filename after renaming or copying
			Buttons_Disable();
		}
		private void lblFile_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//Get label text from file drag-and-dropped onto window
			fila = (string[])e.Data.GetData(DataFormats.FileDrop);
			lblLoad_File(fila);
			this.Activate();
		}
		private void txtNewName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//Prevents user from entering illegal file name characters in textbox
			//Does not process pasted text!
			//N.B. Depending on the filesystem these may not all be illegal, but we can't check against Path.GetInvalidPathChars(), as this does 
			//not actually check what characters are illegal on the particular filesystem we are writing to.
			//so we restrict characters to the lowest-common-denominator: Windows
			//Characters below are (in order): " * / \ : < > ? |
			if (e.KeyChar == '\"' | e.KeyChar == '*' | e.KeyChar == '/' | e.KeyChar == '\\' | e.KeyChar == ':' | e.KeyChar == '<' | e.KeyChar == '>' | e.KeyChar == '?' | e.KeyChar == '|')
				e.Handled = true;
		}
		private void txtNewName_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Buttons_Disable();
			//disallow file base name that is blank or has trailing spaces, so we can rename a file with no extension reliably
			//does not detect if the user "cuts" the whole file name!
			if (txtNewName.Text.Length == 0)
				return;
			if (txtNewName.Text.Substring(txtNewName.Text.Length - 1) == " ")
				return;
			foreach (string foundFile in Directory.GetFiles(FilePath)) {
				//disallow file base name if a file with the same base name (and any extension) already exists
				if (check_NewName(foundFile) == false)
					return;
			}
			foreach (string foundFile in Directory.GetDirectories(FilePath)) {
				//disallow file base name if a folder with the same base name (and any extension) already exists
				if (check_NewName(foundFile) == false)
					return;
			}
			btnRename.Enabled = true;
			btnCopy.Enabled = true;
		}
		private bool check_NewName(string foundFile)
		{
			FilesInDir = Path.GetFileName(foundFile);
			if (FilesInDir.Contains("."))
				FilesInDir = FilesInDir.Substring(0, FilesInDir.IndexOf("."));
			if (string.Equals(txtNewName.Text, FilesInDir, StringComparison.CurrentCultureIgnoreCase))
				return false;
			return true;
		}
		private void btnRename_Click(System.Object sender, System.EventArgs e)
		{
			btn_Click(false);
		}
		private void btnCopy_Click(System.Object sender, System.EventArgs e)
		{
			btn_Click(true);
		}
		private void btn_Click(bool Copy_TrueFalse)
		{
			FileNameTxtbox = Path.GetFileName(txtNewName.Text); // Why do we need this variable?
			//On Error GoTo 100   //  (VB) 'If file we try to rename is in use by another program
			//Renames or Copies all associated files
			foreach (string foundFile in Directory.GetFiles(FilePath)) {
				foundFileName = Path.GetFileName(foundFile);
				CopyOrRename(foundFileName, Copy_TrueFalse, false);
			}
			//Renames or Copies all associated directories
			foreach (string foundFile in Directory.GetDirectories(FilePath)) {
				foundFileName = Path.GetFileName(foundFile);
				CopyOrRename(foundFileName, Copy_TrueFalse, true);
			}
			//Updates the Layer Name value in MapWindow layer properties files
/*			MwsrFileIn = FilePath + txtNewName.Text + ".mwsr";
			if (File.Exists(MwsrFileIn)) {
				foundFileReadOnly = LazyRenamer.My.MyProject.Computer.FileSystem.GetFileInfo(MwsrFileIn);
				if (foundFileReadOnly.IsReadOnly == false) {
					MwsrFileOut = FilePath + FileNameTxtbox + ".mwsr2";
					FileSystem.FileOpen(1, MwsrFileIn, OpenMode.Input);
					FileSystem.FileOpen(2, MwsrFileOut, OpenMode.Output);
					MwsrLabelName = "Layer Name=" + Strings.ChrW(34) + FileName + Strings.ChrW(34);
					MwsrLabelNameNew = "Layer Name=" + Strings.ChrW(34) + txtNewName.Text + Strings.ChrW(34);
					do {
						MwsrLine = FileSystem.LineInput(1);
						if (MwsrLine.Contains(MwsrLabelName))
							MwsrLine = Strings.Replace(MwsrLine, MwsrLabelName, MwsrLabelNameNew);
						FileSystem.PrintLine(2, MwsrLine);
					} while (!(FileSystem.EOF(1) == true));
					FileSystem.FileClose(1, 2);
					File.Delete(MwsrFileIn);
					FileSystem.Rename(MwsrFileOut, MwsrFileOut.Substring(0, MwsrFileOut.Length - 1));
				}
				//Deletes old file as we seem to have been successful
				if (File.Exists(MwsrFileOut) == true)
					File.Delete(MwsrFileIn);
			}
*/			//Removes #tmp as we seem to have been successful
			foreach (string foundFile in Directory.GetFiles(FilePath)) {
				if (foundFile.Substring(foundFile.Length - 4) == "#tmp")
					File.Move(foundFile, foundFile.Substring(0, foundFile.Length - 4));
			}
			foreach (string foundFile in Directory.GetDirectories(FilePath)) {
				if (foundFile.Substring(foundFile.Length - 4) == "#tmp")
					Directory.Move(foundFile, foundFile.Substring(0, foundFile.Length - 4));
			}
			//Updates the FileName variable
			fila[0] = FilePath + txtNewName.Text + FileExtension;
			//Updates Gui
			lblLoad_File(fila);
			return;
			//100:  //(VB)
			//Jumps here when "On Error" occurs
			lblFile.Text = "One or more of the associated files appears to be in use by another program. Close the file and try again.";
			lblFile.BackColor = Color.LavenderBlush;
			//Should we restore any files with a #tmp to their original filenames if there has been an error?
			//At the moment the user can close any files in use and click rename again to finish the failed attempt.  This is better in a way.
		}
		private void CopyOrRename(string foundFile, bool Copy_TrueFalse, bool IsDirectory)
		{
			if (! foundFile.Contains(".")) {
				FilesInDir = foundFile;
				foundFileExtension = "";
			} else {
				FilesInDir = foundFile.Substring(0, foundFile.IndexOf("."));
				foundFileExtension = foundFile.Substring(foundFile.IndexOf("."));
			}
			//ToDo: Should move this test to before we start trying to CopyOrRename files for improved performance
			//Q - is this test really needed? A - it won't be once we process _pasted_ text (instead of only typed text).
			if (FileNameTxtbox != FileName) {
				//need to be case sensitive here as it might not be a windows filesystem (see note 2 at top)
				if (FilesInDir == FileName) {
						
/*					if (Copy_TrueFalse == false) {
						FileSystem.Rename(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?
					} else {
						if ((IsDirectory == false)) {
							LazyRenamer.My.MyProject.Computer.FileSystem.CopyFile(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?  Is it needed for copying as well as renaming?
						} else {
							LazyRenamer.My.MyProject.Computer.FileSystem.CopyDirectory(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?  Is it needed for copying as well as renaming?
						}
					}
*/

					if (Copy_TrueFalse == false) {
						if ((IsDirectory == false)) {
							File.Move(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?
						} else {
							Directory.Move(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?
						}
					} else {
						if ((IsDirectory == false)) {
							File.Copy(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?  Is it needed for copying as well as renaming?
						} else {
							//C# can't copy directories.  We could stick with the VB code below.
							//Directory.Copy(FilePath + foundFile, FilePath + FileNameTxtbox + foundFileExtension + "#tmp"); //Is the #tmp really wise?  Is it needed for copying as well as renaming?
							string SourcePath = FilePath + foundFile;
							string DestinationPath = FilePath + FileNameTxtbox + foundFileExtension + "#tmp";
							foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", 
							    SearchOption.AllDirectories))
							    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

							//Copy all the files
							foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", 
							    SearchOption.AllDirectories))
							    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath));
						}
					}
				}
			}
		}
		private void Form1_Disposed(object sender, System.EventArgs e)
		{
/*			//Saves the current program position for the next LazyRenamer session
			FileSystem.FileOpen(1, IniFile, OpenMode.Output);
			FileSystem.PrintLine(1, "[StartupPosition]");
			FileSystem.PrintLine(1, "startposX = " + this.Left);
			FileSystem.PrintLine(1, "startposY = " + this.Top);
			FileSystem.FileClose(1);
*/		}
	}
}

