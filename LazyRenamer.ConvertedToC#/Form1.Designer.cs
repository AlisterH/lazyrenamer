//    Copyright © Alister Hood, 2011
//
//    This file is part of LazyRenamer.
//    http://code.google.com/p/lazyrenamer
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

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace LazyRenamer
{

	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class Form1 : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.btnRename = new System.Windows.Forms.Button();
			this.lblFile = new System.Windows.Forms.Label();
			this.txtNewName = new System.Windows.Forms.TextBox();
			this.btnCopy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			//btnRename
			//
			this.btnRename.Enabled = false;
			this.btnRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnRename.Location = new System.Drawing.Point(115, 83);
			this.btnRename.Name = "btnRename";
			this.btnRename.Size = new System.Drawing.Size(82, 24);
			this.btnRename.TabIndex = 17;
			this.btnRename.Text = "Rename";
			this.btnRename.UseVisualStyleBackColor = true;
			//
			//lblFile
			//
			this.lblFile.AllowDrop = true;
			this.lblFile.BackColor = System.Drawing.Color.WhiteSmoke;
			this.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblFile.Location = new System.Drawing.Point(0, 1);
			this.lblFile.MinimumSize = new System.Drawing.Size(196, 59);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(196, 59);
			this.lblFile.TabIndex = 0;
			this.lblFile.Text = "<Drag and drop associated file here>";
			this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//txtNewName
			//
			this.txtNewName.Enabled = false;
			this.txtNewName.Location = new System.Drawing.Point(0, 62);
			this.txtNewName.Name = "txtNewName";
			this.txtNewName.Size = new System.Drawing.Size(196, 20);
			this.txtNewName.TabIndex = 15;
			this.txtNewName.Text = "Enter new name";
			//
			//btnCopy
			//
			this.btnCopy.Enabled = false;
			this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.btnCopy.Location = new System.Drawing.Point(0, 83);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(82, 24);
			this.btnCopy.TabIndex = 16;
			this.btnCopy.Text = "Copy";
			this.btnCopy.UseVisualStyleBackColor = true;
			//
			//Form1
			//
			this.AcceptButton = this.btnRename;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(196, 107);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.lblFile);
			this.Controls.Add(this.txtNewName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(202, 109);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LazyRenamer";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button withEventsField_btnRename;
		internal System.Windows.Forms.Button btnRename {
			get { return withEventsField_btnRename; }
			set {
				if (withEventsField_btnRename != null) {
					withEventsField_btnRename.Click -= btnRename_Click;
				}
				withEventsField_btnRename = value;
				if (withEventsField_btnRename != null) {
					withEventsField_btnRename.Click += btnRename_Click;
				}
			}
		}
		private System.Windows.Forms.Label withEventsField_lblFile;
		internal System.Windows.Forms.Label lblFile {
			get { return withEventsField_lblFile; }
			set {
				if (withEventsField_lblFile != null) {
					withEventsField_lblFile.DragEnter -= lblFile_DragEnter;
					withEventsField_lblFile.DragDrop -= lblFile_DragDrop;
				}
				withEventsField_lblFile = value;
				if (withEventsField_lblFile != null) {
					withEventsField_lblFile.DragEnter += lblFile_DragEnter;
					withEventsField_lblFile.DragDrop += lblFile_DragDrop;
				}
			}
		}
		private System.Windows.Forms.TextBox withEventsField_txtNewName;
		internal System.Windows.Forms.TextBox txtNewName {
			get { return withEventsField_txtNewName; }
			set {
				if (withEventsField_txtNewName != null) {
					withEventsField_txtNewName.KeyPress -= txtNewName_KeyPress;
					withEventsField_txtNewName.KeyUp -= txtNewName_KeyUp;
				}
				withEventsField_txtNewName = value;
				if (withEventsField_txtNewName != null) {
					withEventsField_txtNewName.KeyPress += txtNewName_KeyPress;
					withEventsField_txtNewName.KeyUp += txtNewName_KeyUp;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCopy;
		internal System.Windows.Forms.Button btnCopy {
			get { return withEventsField_btnCopy; }
			set {
				if (withEventsField_btnCopy != null) {
					withEventsField_btnCopy.Click -= btnCopy_Click;
				}
				withEventsField_btnCopy = value;
				if (withEventsField_btnCopy != null) {
					withEventsField_btnCopy.Click += btnCopy_Click;
				}
			}

		}
		public Form1()
		{
			Disposed += Form1_Disposed;
			Load += Form1_Load;
			InitializeComponent();
		}
	}
}
