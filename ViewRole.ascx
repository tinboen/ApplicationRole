<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ViewRole.ascx.cs" Inherits="DBH.ApplicationRole.ViewRole" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<link href='<%=ResolveUrl("~/bootstrap/datatables/css/jquery.dataTables.min.css") %>' rel="stylesheet" type="text/css" />
<script src='<%=ResolveUrl("~/bootstrap/datatables/js/jquery.dataTables.min.js") %>' type="text/javascript"></script>

<div class="container-fluid">

	<ul id="feedbackTab" runat="server"></ul>

	<div class="dnnFormMessage dnnFormInfo">
		<asp:Label ID="lblIntro" runat="server" resourceKey="lblIntro" />
	</div>

</div>


<div class="container">

	<div class="dnnForm dnnSecurityRoles">
		<div runat="server" id="divGroups">
			<div class="dnnFormItem">
				<dnn:label id="plRoleGroups" runat="server" suffix="" controlname="cboRoleGroups" />
				<asp:DropDownList ID="cboRoleGroups" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboRoleGroups_SelectedIndexChanged" />
			</div>
		</div>

		<div class="dnnFormItem">
			<ul class="dnnActions dnnClear">
				<li>
					<asp:LinkButton runat="server" CssClass="dnnPrimaryAction" ID="cmdAddNewRecord" Resourcekey="cmdAddNewRecord" /></li>
			</ul>
		</div>
	</div>

</div>


<div class="container-fluid">

	<div class="dnnForm AccessRole dnnClear" id="AccessRole">
		<ul class="dnnAdminTabNav dnnClear">
			<li><a href="#Role_Info">Role</a></li>
			<li><a href="#User_Info">User</a></li>
		</ul>
		<div class="dnnClear" id="Role_Info">
			<asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="false"
				OnPreRender="gvRoles_PreRender"
				CssClass="table table-striped">
				<EmptyDataTemplate>
					<asp:Image ID="Image0" ImageUrl="~/Images/yellow-warning.gif" AlternateText="No Image" runat="server" />No Data Found.
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField HeaderText="Edit">
						<ItemTemplate>
							<a href="javascript:dnnModal.show('<%#DotNetNuke.Common.Globals.NavigateURL("EditRole","RoleID=" + Eval("RoleID").ToString(),"mid=" + ModuleId.ToString()) + "?popUp=true" %>',false,570,950,true)" class="">
								<dnn:dnnImage ID="imgEdit" IconKey="Edit" AlternateText="Edit" runat="server" resourcekey="Edit" />
							</a>
						</ItemTemplate>
					</asp:TemplateField>

					<asp:TemplateField HeaderText="Users">
						<ItemTemplate>
							<a href="javascript:dnnModal.show('<%#DotNetNuke.Common.Globals.NavigateURL("UserRoles","RoleID=" + Eval("RoleID").ToString(),"mid=" + ModuleId.ToString()) + "?popUp=true" %>',false,400,950,true)" class="">
								<asp:Image ID="Image2" runat="server" ImageUrl="~/images/icon_users_16px.gif" AlternateText="View" resourcekey="view" HeaderText="View" />
							</a>
						</ItemTemplate>
					</asp:TemplateField>

					<asp:BoundField DataField="RoleName" SortExpression="RoleName" HeaderText="Role Name" />
					<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" />
					<asp:TemplateField HeaderText="Public">
						<ItemTemplate>
							<dnn:DnnImage Runat="server" ID="imgApproved" IconKey="Checked" Visible='<%# DataBinder.Eval(Container.DataItem,"IsPublic") %>' />
							<dnn:DnnImage Runat="server" ID="imgNotApproved" IconKey="Unchecked" Visible='<%# !(bool)DataBinder.Eval(Container.DataItem,"IsPublic")%>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Updated Date">
						<ItemTemplate>
							<dnn:Dnnimage Runat="server" ID="Image1" IconKey="Checked" Visible='<%# DataBinder.Eval(Container.DataItem,"AutoAssignment") %>' />
							<dnn:Dnnimage Runat="server" ID="Image2" IconKey="Unchecked" Visible='<%# !(bool)DataBinder.Eval(Container.DataItem,"AutoAssignment") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="UserCount" HeaderText="UserCount" ItemStyle-HorizontalAlign="Center" />
				</Columns>
			</asp:GridView>
		</div>
		<div class="dnnClear" id="User_Info">
			<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false"
				OnPreRender="gvRoles_PreRender"
				CssClass="table table-striped">
				<EmptyDataTemplate>
					<asp:Image ID="Image0" ImageUrl="~/Images/yellow-warning.gif" AlternateText="No Image" runat="server" />No Data Found.
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField HeaderText="Users">
						<ItemTemplate>
							<a href="javascript:dnnModal.show('<%#DotNetNuke.Common.Globals.NavigateURL("SecurityRoles","UserID=" + Eval("UserID").ToString(),"mid=" + ModuleId.ToString()) + "?popUp=true" %>',false,400,950,true)" class="">
								<asp:Image ID="Image2" runat="server" ImageUrl="~/images/icon_users_16px.gif" AlternateText="View" resourcekey="view" HeaderText="View" />
							</a>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="UserName" HeaderText="Username" />
					<asp:BoundField DataField="FirstName" HeaderText="FirstName" />
					<asp:BoundField DataField="LastName" HeaderText="LastName" />
					<asp:BoundField DataField="DisplayName" HeaderText="DisplayName" />
					<asp:BoundField DataField="RoleName" HeaderText="RoleName" />
				</Columns>
			</asp:GridView>
		</div>

	</div>
</div>

<script language="javascript" type="text/javascript">
	/*globals jQuery, window, Sys */
	(function ($, Sys) {
		function setUpViewRequestResult() {
			$('#AccessRole').dnnTabs();
		}
		$(document).ready(function () {
			setUpViewRequestResult();
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
				setUpViewRequestResult();
			});
		});

		$(document).ready(function () {
			$('#<%= gvRoles.ClientID %>').dataTable({
					"order": [[3, "desc"]]
				});
				$('#<%= gvUsers.ClientID %>').dataTable({
					"order": [[2, "desc"]]
				});
			});
		}(jQuery, window.Sys));
</script>
