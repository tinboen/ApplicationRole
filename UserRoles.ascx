<%@ Control Language="C#" AutoEventWireup="false" Inherits="DBH.ApplicationRole.UserRoles" CodeBehind="UserRoles.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnnui" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="dnnForm dnnManageSecurityRoles">

    <asp:Panel ID="pnlRoles" runat="server" Visible="True">
        <h2 class="dnnFormSectionHead">
            <asp:Label ID="lblTitle" runat="server" /></h2>
        <div class="container">
            <fieldset>
                <div class="row">
                    <table>
                        <tr>
                            <td valign="bottom">
                                <div class="dnnFormItem">
                                    <dnn:Label ID="plRoles" runat="server" />
                                    <dnn:Label ID="plUsers" runat="server" />
                                    <asp:TextBox ID="txtUsers" runat="server" Width="150" />
                                    <asp:LinkButton ID="cmdValidate" runat="server" CssClass="dnnSecondaryAction" resourceKey="cmdValidate" />
                                    <asp:DropDownList ID="cboUsers" runat="server" AutoPostBack="True" />
                                    <asp:DropDownList ID="cboRoles" runat="server" AutoPostBack="True" DataValueField="RoleID" DataTextField="RoleName" />
                                </div>
                                <asp:PlaceHolder runat="server" ID="placeIsOwnerHeader" Visible="false">
                                    <dnn:Label ID="lblIsOwner" runat="server" />
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" ID="placeIsOwner" Visible="false">
                                    <asp:CheckBox runat="server" ID="chkIsOwner" />
                                </asp:PlaceHolder>
                            </td>
                            <td valign="bottom">
                                <div class="dnnFormItem">
                                    <dnn:Label ID="plEffectiveDate" runat="server" />
                                    <dnnui:RadDatePicker ID="effectiveDatePicker" runat="server" />
                                </div>
                            </td>
                            <td valign="bottom">
                                <div class="dnnFormItem">
                                    <dnn:Label ID="plExpiryDate" runat="server" />
                                    <dnnui:RadDatePicker ID="expiryDatePicker" runat="server" />
                                </div>
                            </td>
                            <td nowrap="true" valign="bottom">
                                <div class="dnnFormItem">
                                    <asp:LinkButton ID="cmdAdd" CssClass="dnnPrimaryAction" runat="server" CausesValidation="true" ValidationGroup="SecurityRole" />
                                    <asp:CheckBox ID="chkNotify" resourcekey="SendNotification" runat="server" Checked="True" />
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="row">
                    <div class="dnnFormItem">
                        <asp:CompareValidator ID="valEffectiveDate" CssClass="dnnFormError" runat="server" resourcekey="valEffectiveDate" Display="Dynamic" Type="Date" Operator="DataTypeCheck" ControlToValidate="effectiveDatePicker" ValidationGroup="SecurityRole" />
                        <asp:CompareValidator ID="valExpiryDate" CssClass="dnnFormError" runat="server" resourcekey="valExpiryDate" Display="Dynamic" Type="Date" Operator="DataTypeCheck" ControlToValidate="expiryDatePicker" ValidationGroup="SecurityRole" />
                        <asp:CompareValidator ID="valDates" CssClass="dnnFormError" runat="server" resourcekey="valDates" Display="Dynamic" Type="Date" Operator="GreaterThan" ControlToValidate="expiryDatePicker" ControlToCompare="effectiveDatePicker" ValidationGroup="SecurityRole" />
                    </div>
                </div>
            </fieldset>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlUserRoles" runat="server" CssClass="WorkPanel" Visible="True">
        <asp:DataGrid ID="grdUserRoles" runat="server" Width="100%" GridLines="None" DataKeyField="UserRoleID" EnableViewState="false" AutoGenerateColumns="false" CellSpacing="0" CellPadding="0" CssClass="dnnGrid">
            <HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
            <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
            <AlternatingItemStyle CssClass="dnnGridAltItem" />
            <EditItemStyle CssClass="dnnFormInput" />
            <SelectedItemStyle CssClass="dnnFormError" />
            <FooterStyle CssClass="dnnGridFooter" />
            <PagerStyle CssClass="dnnGridPager" />
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <!-- [DNN-4285] Hide the button if the user cannot be removed from the role -->
                        <dnn:DnnImageButton ID="cmdDeleteUserRole" runat="server" AlternateText="Delete" CausesValidation="False" CommandName="Delete" IconKey="Delete" resourcekey="cmdDelete" Visible='<%# DeleteButtonVisible(Convert.ToInt32(Eval("UserID")), Convert.ToInt32(Eval("RoleID")))  %>' OnClick="cmdDeleteUserRole_click">
                        </dnn:DnnImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="UserName">
                    <ItemTemplate>
                        <a href='<%# DotNetNuke.Common.Globals.LinkClick("userid=" + Eval("UserID").ToString(), TabId, ModuleId) %>' class=""><%# Eval("FullName").ToString()%> </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="RoleName" HeaderText="SecurityRole" />
                <asp:TemplateColumn HeaderText="EffectiveDate">
                    <ItemTemplate>
                        <%#FormatDate(Convert.ToDateTime(Eval("EffectiveDate"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="ExpiryDate">
                    <ItemTemplate>
                        <%#FormatDate(Convert.ToDateTime(Eval("ExpiryDate"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="IsOwner">
                    <ItemTemplate>
                        <dnn:DnnImage Runat="server" ID="imgApproved" IconKey="Checked" Visible='<%# (bool)DataBinder.Eval(Container.DataItem,"IsOwner") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <dnn:pagingcontrol id="ctlPagingControl" runat="server"></dnn:pagingcontrol>

    </asp:Panel>
    <ul id="actionsRow" runat="server" class="dnnActions dnnClear">
        <li>
            <asp:HyperLink ID="cmdCancel" runat="server" CssClass="dnnPrimaryAction" resourcekey="Close" /></li>
    </ul>
</div>
