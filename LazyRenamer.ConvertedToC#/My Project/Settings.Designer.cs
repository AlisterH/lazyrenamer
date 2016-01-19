using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace LazyRenamer.My
{

	[System.Runtime.CompilerServices.CompilerGeneratedAttribute(), System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	internal sealed partial class MySettings : global::System.Configuration.ApplicationSettingsBase
	{

		private static MySettings defaultInstance = (MySettings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new MySettings());

		#region "My.Settings Auto-Save Functionality"

		private static bool addedHandler;

		private static object addedHandlerLockObject = new object();
		[System.Diagnostics.DebuggerNonUserCodeAttribute(), System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		private static void AutoSaveSettings(global::System.Object sender, global::System.EventArgs e)
		{
			if (MyProject.Application.SaveMySettingsOnExit) {
				//Oops.  When did I break this?
				//LazyRenamer.My.Settings.Save();
			}
		}
		#endregion

		public static MySettings Default {
			get {

				if (!addedHandler) {
					lock (addedHandlerLockObject) {
						if (!addedHandler) {
							MyProject.Application.Shutdown += AutoSaveSettings;
							addedHandler = true;
						}
					}
				}
				return defaultInstance;
			}
		}
	}
}

namespace LazyRenamer.My
{

	[Microsoft.VisualBasic.HideModuleNameAttribute(), System.Diagnostics.DebuggerNonUserCodeAttribute(), System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	static internal class MySettingsProperty
	{

		[System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")]
		static internal global::LazyRenamer.My.MySettings Settings {
			get { return global::LazyRenamer.My.MySettings.Default; }
		}
	}
}