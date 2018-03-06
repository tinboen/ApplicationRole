<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DBH.ApplicationRole.Settings" %>


<!-- uncomment the code below to start using the DNN Form pattern to create and update settings -->


<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="lblSetting1" runat="server" />
        <asp:TextBox ID="txtSetting1" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblSetting2" runat="server" />
        <asp:TextBox ID="txtSetting2" runat="server" />
    </div>
</fieldset>

<%--<div class="dnnForm">
    <div class="dnnFormItem">
        <ul class="dnnActions dnnClear">
            <li>
                <asp:LinkButton runat="server" CssClass="dnnPrimaryAction" ID="cmdModuleInitialize" Resourcekey="cmdModuleInitialize" OnClick="cmdModuleInitialize_Click" /></li>
        </ul>

    </div>
</div>
--%>
