/*
' Copyright (c) 2018  Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

#region Usings

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using DotNetNuke.Security.Roles.Internal;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using DBH.ApplicationRole.Components;
using DotNetNuke.Entities.Users;
using System.Web;
using System.Data;

#endregion

namespace DBH.ApplicationRole
{

    /// <summary>
    /// The Roles PortalModuleBase is used to manage the Security Roles for the
    /// portal.
    /// </summary>
    public partial class ViewRole : PortalModuleBase
    {

        #region Private Members

        private string AddNewRecord = "EditRole";
        private int _roleGroupId = -1;
        private string GroupRole = "DBH";

        #endregion

        #region Private Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// BindGroups gets the role Groups from the Database and binds them to the DropDown
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        ///     [cnurse]    01/05/2006  Created
        /// </history>
        /// -----------------------------------------------------------------------------
        private void BindGroups()
        {
            ArrayList arrGroups = RoleController.GetRoleGroups(PortalId);
            bool bFound = false;

            foreach (RoleGroupInfo roleGroup in arrGroups)
            {
                if (roleGroup.RoleGroupName.StartsWith(GroupRole))
                {
                    cboRoleGroups.Items.Add(new ListItem(roleGroup.RoleGroupName, roleGroup.RoleGroupID.ToString()));
                    bFound = true;
                }
            }

            // If the Group "DBH" is not found, then we will add
            if(!bFound)
            {
                var objRoleGroupInfo = new RoleGroupInfo();
                objRoleGroupInfo.PortalID = PortalId;
                //objRoleGroupInfo.RoleGroupID = RoleGroupID;
                objRoleGroupInfo.RoleGroupName = GroupRole;
                objRoleGroupInfo.Description = "Department of Beaches and Harbors Security Group Role";
                try
                {
                    RoleController.AddRoleGroup(objRoleGroupInfo);
                }
                catch
                {
                    Skin.AddModuleMessage(this, Localization.GetString("DuplicateRoleGroup", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
                }

                arrGroups = RoleController.GetRoleGroups(PortalId);

                foreach (RoleGroupInfo roleGroup in arrGroups)
                {
                    if (roleGroup.RoleGroupName.StartsWith(GroupRole))
                    {
                        cboRoleGroups.Items.Add(new ListItem(roleGroup.RoleGroupName, roleGroup.RoleGroupID.ToString()));
                    }
                }
            }

            //Determine there is parameter of Request
            if (this.Request.QueryString["RoleGroupID"] != null)
            {
                cboRoleGroups.SelectedValue = this.Request.QueryString["RoleGroupID"].ToString();
            }
        }

        protected void BindData()
        {
            try
            {
                _roleGroupId = Convert.ToInt32(cboRoleGroups.SelectedValue);

                var roles = _roleGroupId < -1
                                    ? RoleController.Instance.GetRoles(PortalId)
                                    : RoleController.Instance.GetRoles(PortalId, r => r.RoleGroupID == _roleGroupId);

                gvRoles.DataSource = roles;
                gvRoles.DataBind();

                List<AccessRoleInfo> myIList = new List<AccessRoleInfo>();

                foreach (RoleInfo objRoleInfo in roles)
                {
                    IList<UserInfo> users = RoleController.Instance.GetUsersByRole(PortalId, objRoleInfo.RoleName);
                    foreach (UserInfo objUserInfo in users)
                    {
                        AccessRoleInfo objAccessRoleInfo = new AccessRoleInfo();
                        objAccessRoleInfo.UserID = objUserInfo.UserID;
                        objAccessRoleInfo.Username = objUserInfo.Username;
                        objAccessRoleInfo.FirstName = objUserInfo.FirstName;
                        objAccessRoleInfo.LastName = objUserInfo.LastName;
                        objAccessRoleInfo.DisplayName = objUserInfo.DisplayName;

                        objAccessRoleInfo.RoleName = objRoleInfo.RoleName;

                        myIList.Add(objAccessRoleInfo);
                    }
                }

                // Group by userID and combine RoleNme in one line
                var result = from o in myIList
                group o by new { o.UserID, o.Username, o.FirstName, o.LastName, o.DisplayName } into g
                select new
                {
                    UserID = g.Key.UserID,
                    Username = g.Key.Username,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    DisplayName = g.Key.DisplayName,
                    RoleName = string.Join(",", g.Select(x => x.RoleName))
                };

                gvUsers.DataSource = result;
                gvUsers.DataBind();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Get text description of Frequency Value
        /// </summary>
        protected string FormatFrequency(string frequency)
        {
            if (frequency == "N") return string.Empty;

            var ctlEntry = new ListController();
            ListEntryInfo entry = ctlEntry.GetListEntryInfo("Frequency", frequency);
            return entry != null ? entry.Text : frequency;
        }

        #endregion

        #region BoundFields with Paging and Sorting

        protected void gvRoles_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }


        #endregion

        #region Command

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdAddNewRecord_Click redirect to add new record dialog when the add new record button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void cmdAddNewRecord_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                // return to main page
                Response.Redirect(Globals.NavigateURL(AddNewRecord, "mid=" + ModuleId.ToString()));
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (!Page.IsPostBack)
                {
                    string strURL = Null.NullString;
                    strURL = DotNetNuke.Common.Globals.NavigateURL(AddNewRecord, "mid=" + ModuleId.ToString()) + "?popUp=true";
                    cmdAddNewRecord.Attributes.Add("href", "javascript: dnnModal.show('" + strURL + "', false, 550, 950, true)");

                    BindGroups();
                    BindData();

                    gvRoles.Columns[0].Visible = false;
                    gvRoles.Columns[1].Visible = false;
                    cmdAddNewRecord.Visible = false;

                    if (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "Edit", ModuleConfiguration))
                    {
                        gvRoles.Columns[0].Visible = true;
                        gvRoles.Columns[1].Visible = true;
                        cmdAddNewRecord.Visible = true;
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void cboRoleGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        protected void OnRolesGridItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item;
            switch (item.ItemType)
            {
                case GridItemType.SelectedItem:
                case GridItemType.AlternatingItem:
                case GridItemType.Item:
                    {
                        var gridDataItem = (GridDataItem)item;

                        var editLink = gridDataItem["EditButton"].Controls[0] as HyperLink;
                        if (editLink != null)
                        {
                            var role = (RoleInfo)item.DataItem;
                            editLink.Visible = role.RoleName != PortalSettings.AdministratorRoleName || (PortalSecurity.IsInRole(PortalSettings.AdministratorRoleName));
                        }

                        var rolesLink = gridDataItem["RolesButton"].Controls[0] as HyperLink;
                        if (rolesLink != null)
                        {
                            var role = (RoleInfo)item.DataItem;
                            rolesLink.Visible = (role.Status == RoleStatus.Approved) && (role.RoleName != PortalSettings.AdministratorRoleName || (PortalSecurity.IsInRole(PortalSettings.AdministratorRoleName)));
                        }
                    }
                    break;
            }
        }

        #endregion

    }
}
