/*
' Copyright (c) 2017 Department of Beaches and Harbors
' All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using DBH.ApplicationRole.Components;
using System.Collections.Generic;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common;

namespace DBH.ApplicationRole
{
	/// -----------------------------------------------------------------------------
	/// <summary>
	/// The Settings class manages Module Settings
	/// 
	/// Typically your settings control would be used to manage settings for your module.
	/// There are two types of settings, ModuleSettings, and TabModuleSettings.
	/// 
	/// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
	/// 
	/// TabModuleSettings apply only to the current module on the current page, if you copy that module to
	/// another page the settings are not transferred.
	/// 
	/// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
	/// 
	/// Below we have some examples of how to access these settings but you will need to uncomment to use.
	/// 
	/// Because the control inherits from ApplicationRoleSettingsBase you have access to any custom properties
	/// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
	/// </summary>
	/// -----------------------------------------------------------------------------
	public partial class Settings : ModuleSettingsBase
	{
		#region Base Method Implementations

		/// -----------------------------------------------------------------------------
		/// <summary>
		/// LoadSettings loads the settings from the Database and displays them
		/// </summary>
		/// -----------------------------------------------------------------------------
		public override void LoadSettings()
		{
			try
			{
				if (Page.IsPostBack == false)
				{
					//Check for existing settings and use those on this page
					//Settings["SettingName"]

					if(Settings.Contains("Setting1"))
						txtSetting1.Text = Settings["Setting1"].ToString();
			
					if (Settings.Contains("Setting2"))
						txtSetting2.Text = Settings["Setting2"].ToString();

					//// Check if lookup table does not data, then display the initialize button command
					//LookupController objCtrl = new LookupController();
					//List<LookupInfo> objInfos;
					//objInfos = objCtrl.GetAllLookup(ModuleId, Null.NullInteger, Null.NullString, Null.NullString);
					//// if the lookup table is populated, then hide the button
					//if (objInfos.Count <= 0)
					//    cmdModuleInitialize.Visible = true;
					//else
					//    cmdModuleInitialize.Visible = false;

				}
			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		/// -----------------------------------------------------------------------------
		/// <summary>
		/// UpdateSettings saves the modified settings to the Database
		/// </summary>
		/// -----------------------------------------------------------------------------
		public override void UpdateSettings()
		{
			try
			{
				var modules = new ModuleController();

				//the following are two sample Module Settings, using the text boxes that are commented out in the ASCX file.
				//module settings
				modules.UpdateModuleSetting(ModuleId, "Setting1", txtSetting1.Text);
				modules.UpdateModuleSetting(ModuleId, "Setting2", txtSetting2.Text);

				//tab module settings
				modules.UpdateTabModuleSetting(TabModuleId, "Setting1", txtSetting1.Text);
				modules.UpdateTabModuleSetting(TabModuleId, "Setting2", txtSetting2.Text);
			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

	}
}