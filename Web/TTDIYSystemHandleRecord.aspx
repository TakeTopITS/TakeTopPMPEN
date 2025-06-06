<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTDIYSystemHandleRecord.aspx.cs" Inherits="TTDIYSystemHandleRecord" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1100px;
            width: expression (document.body.clientWidth <= 1100? "1100px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            if (top.location != self.location) { } else { CloseWebPage(); }

        });

    </script>

</head>
<body>

    <center>
        <form id="form1" runat="server">
            <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>

                                                                    <td width="80px" align="right">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_WFID" runat="server" Precision="0" Width="53px">0</NickLee:NumberBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BT_FindWFID" runat="server" CssClass="inpu" OnClick="BT_FindWFID_Click"
                                                                            Text="<%$ Resources:lang,ChaXun%>" />
                                                                    </td>


                                                                    <%--                                                                    <td style="text-align: left;">&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                                    </td>--%>
                                                                    <%--<td>
                                                                        <asp:DropDownList ID="DL_WLType" runat="server" DataTextField="Type" DataValueField="Type"
                                                                            Height="20px" Width="120px" OnSelectedIndexChanged="DL_WLType_SelectedIndexChanged" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>&nbsp;<asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,MuBan%>"></asp:Label>：</td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_WFTemName" runat="server"></asp:TextBox>
                                                                        <asp:DropDownList ID="DL_WLTem" runat="server" DataTextField="TemName" DataValueField="TemName" Height="20px" OnSelectedIndexChanged="DL_WLTem_SelectedIndexChanged" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                    </td>--%>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_WFName" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click"
                                                                            Text="<%$ Resources:lang,ChaXun%>" />
                                                                    </td>
                                                                    <td></td>
                                                                    <td>&nbsp;&nbsp;<asp:Button ID="BT_FindByTime" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXunYiShiJian%>" />
                                                                        <cc1:ModalPopupExtender ID="BT_FindByTime_ModalPopupExtender" runat="server" Enabled="True"
                                                                            TargetControlID="BT_FindByTime" PopupControlID="Panel1" CancelControlID="IMBT_Close"
                                                                            BackgroundCssClass="modalBackground" Y="150">
                                                                        </cc1:ModalPopupExtender>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <table style="width: 100%; margin-top: 5px" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" style="padding-left: 20px; font-weight: bold; height: 24px; color: #394f66; background-image: url('ImagesSkin/titleBG.jpg')">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td colspan="2" style="text-align: left; padding-left: 10px;">
                                                                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2" style="text-align: right; padding-right: 30px;">
                                                                        <asp:Button ID="BT_CreateUserDIYModule" runat="server" CssClass="inpuYello" Text="<%$ Resources:lang,New%>" />

                                                                        <asp:Label ID="LB_UserCode" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="LB_UserName" Visible="false" runat="server"></asp:Label>
                                                                        <asp:Label ID="LB_QueryScope" runat="server" Visible="false"></asp:Label>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px;">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td style="width: 100%; height: 1px;" align="left">
                                                                        <cc2:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                                            Width="100%">
                                                                            <cc2:TabPanel ID="TabPanel1" runat="server" HeaderText="">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,WoYaoChuLiDeGongZuo%>"></asp:Label>：
                                                                                </HeaderTemplate>

                                                                                <ContentTemplate>
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                                                                            <td>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left" width="5%"><strong>
                                                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td align="left" width="25%"><strong>
                                                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GongZuoLiuMingCheng %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td align="left" width="10%"><strong>
                                                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,QianBuShenQingRen %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td align="left" width="15%"><strong>
                                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YaoShenHeDegongZuoLiu %>"></asp:Label>
                                                                                                        </strong></td>
                                                                                                        <td align="left" width="6%"><strong>
                                                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label>
                                                                                                        </strong></td>
                                                                                                        <td align="left" width="8%"><strong>
                                                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,DongZuo %>"></asp:Label>
                                                                                                        </strong></td>
                                                                                                        <td align="left" width="10%"><strong>
                                                                                                            <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,JianLiShiJian %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td align="left" width="10%"><strong>
                                                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,FaQiRen %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td align="left" width="8%"><strong>
                                                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label>
                                                                                                        </strong></td>
                                                                                                        <td align="left" width="4%"><strong></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="6" align="right">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                                        OnPageIndexChanged="DataGrid4_PageIndexChanged" PageSize="5" Width="100%">

                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                            </asp:BoundColumn>

                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTWorkFlowDetailMain.aspx?ID={0}"
                                                                                                DataTextField="WLName" HeaderText="要审核的工作" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                            </asp:HyperLinkColumn>

                                                                                            <asp:TemplateColumn HeaderText="前步审批人">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label58" runat="server" ToolTip='<%# ShareClass. GetPriorStepLastestOperator(Eval("WLID").ToString(),Eval("StepID").ToString(),Eval("ID").ToString()) %>'>
                                                                                                        <%# ShareClass.StringCutByRequire(ShareClass. GetPriorStepLastestOperator(Eval("WLID").ToString(),Eval("StepID").ToString(),Eval("ID").ToString()),18) %>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTWorkFlowDetailMain.aspx?ID={0}"
                                                                                                DataTextField="WorkDetail" HeaderText="要审核的工作" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:BoundColumn DataField="Requisite" HeaderText="必需">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Operation" HeaderText="动作">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CheckingTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HeaderText="审核时间">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:BoundColumn>

                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                DataTextField="CreatorName" HeaderText="Applicant" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:HyperLinkColumn>

                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <ItemTemplate>
                                                                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTDocumentTreeView.aspx?RelatedType=WorkFlow&RelatedID={0}") %>'
                                                                                                        Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder" /></asp:HyperLink>
                                                                                                </ItemTemplate>

                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>

                                                                                        <EditItemStyle BackColor="#2461BF" />

                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                                        <ItemStyle CssClass="itemStyle" />

                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                    </asp:DataGrid>
                                                                                    <asp:Label ID="LB_Sql4" runat="server" Visible="False"></asp:Label>


                                                                                </ContentTemplate>

                                                                            </cc2:TabPanel>
                                                                            <cc2:TabPanel ID="TabPanel2" runat="server" HeaderText="要处理的代理工作">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,YaoPiHeDeDaiLiGongZuoLiu%>"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ContentTemplate>
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td width="5%" align="left"><strong>
                                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>


                                                                                                        <td width="25%" align="left"><strong>

                                                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GongZuoLiuMingCheng %>"></asp:Label></strong></td>
                                                                                                        <td align="left" width="10%"><strong>
                                                                                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,QianBuShenQingRen %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td width="15%" align="left"><strong>
                                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YaoShenHeDeGongZuo %>"></asp:Label></strong></td>
                                                                                                        <td width="6%" align="left"><strong>
                                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,DongZuo %>"></asp:Label></strong></td>
                                                                                                        <td align="left" width="10%"><strong>
                                                                                                            <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,ShiJian %>"></asp:Label>
                                                                                                        </strong></td>

                                                                                                        <td width="10%" align="left"><strong>
                                                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,FaQiRen %>"></asp:Label></strong></td>

                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong></td>
                                                                                                        <td width="4%" align="left"><strong></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="6" align="right">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid5" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                                        OnPageIndexChanged="DataGrid5_PageIndexChanged" PageSize="5" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTWorkFlowDetailMain.aspx?ID={0}"
                                                                                                DataTextField="WLName" HeaderText="工作流名称" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:TemplateColumn HeaderText="前步审批人">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label21" runat="server" ToolTip='<%# ShareClass. GetPriorStepLastestOperator(Eval("WLID").ToString(),Eval("StepID").ToString(),Eval("ID").ToString()) %>'>
                                                                                                        <%# ShareClass.StringCutByRequire(ShareClass. GetPriorStepLastestOperator(Eval("WLID").ToString(),Eval("StepID").ToString(),Eval("ID").ToString()),18) %>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTWorkFlowDetailMain.aspx?ID={0}"
                                                                                                DataTextField="WorkDetail" HeaderText="要审核的工作" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:BoundColumn DataField="Requisite" HeaderText="必需">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Operation" HeaderText="动作">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CheckingTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HeaderText="审核时间">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:BoundColumn>

                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                DataTextField="CreatorName" HeaderText="Applicant" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:HyperLinkColumn>

                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <ItemTemplate>
                                                                                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTDocumentTreeView.aspx?RelatedType=WorkFlow&RelatedID={0}") %>'
                                                                                                        Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder" /></asp:HyperLink>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>

                                                                                        <ItemStyle CssClass="itemStyle" />
                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                    </asp:DataGrid>
                                                                                    <asp:Label ID="LB_Sql5" runat="server" Visible="False"></asp:Label>

                                                                                </ContentTemplate>

                                                                            </cc2:TabPanel>
                                                                        </cc2:TabContainer>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 15px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <cc2:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="0"
                                                                            Width="100%">
                                                                            <cc2:TabPanel ID="TabPanel3" runat="server" HeaderText="">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,WoYiChuLiDeGongZuo%>"></asp:Label>：
                                                                                </HeaderTemplate>
                                                                                <ContentTemplate>
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td width="5%" align="left"><strong>
                                                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left"><strong>
                                                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YaoShenHeDeGongZuo %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DongZuo %>"></asp:Label></strong></td>
                                                                                                        <td width="11%" align="left"><strong>
                                                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShenHeShiJian %>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left"><strong>
                                                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,GongZuoLiuMingChen %>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left"><strong>
                                                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,FaQiRen %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong></td>
                                                                                                        <td width="4%" align="left"><strong></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="6" align="right">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        ShowHeader="False" Height="1px" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                                        PageSize="5" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTDIYSystemDetailMain.aspx?ID={0}&ModuleType=DIYSYS"
                                                                                                DataTextField="WorkDetail" HeaderText="已审核的工作" Target="right">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:BoundColumn DataField="Requisite" HeaderText="必需">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Operation" HeaderText="动作">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CheckingTime" HeaderText="审核时间">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="11%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="WLName" HeaderText="工作流名称">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                DataTextField="CreatorName" HeaderText="Applicant" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <ItemTemplate>
                                                                                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?RelatedType=WorkFlow&WLID={0}") %>'
                                                                                                        Target="_blank"><img src="ImagesSkin/Doc.gif" alt ="Fold Logo"  class="noBorder"/></asp:HyperLink>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>
                                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                        <ItemStyle CssClass="itemStyle" />
                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                    </asp:DataGrid><asp:Label ID="LB_Sql1" runat="server" Visible="False"></asp:Label>
                                                                                </ContentTemplate>
                                                                            </cc2:TabPanel>
                                                                            <cc2:TabPanel ID="TabPanel4" runat="server" HeaderText="已处理的代理工作">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,YiPiHeDeDaiLiGongZuoLiu%>"></asp:Label>
                                                                                </HeaderTemplate>

                                                                                <ContentTemplate>
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td width="5%" align="left"><strong>
                                                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left"><strong>
                                                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,YaoShenHeDeGongZuo %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,BiXu %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DongZuo %>"></asp:Label></strong></td>
                                                                                                        <td width="11%" align="left"><strong>
                                                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,FaQiShiJian %>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left"><strong>
                                                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,GongZuoLiuMingChen %>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left"><strong>
                                                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,FaQiRen %>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left"><strong>
                                                                                                            <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong></td>
                                                                                                        <td width="4%" align="left"><strong></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="6" align="right">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid6" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        Height="1px" OnPageIndexChanged="DataGrid6_PageIndexChanged" PageSize="5" Width="100%"
                                                                                        ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTDIYSystemDetailMain.aspx?ID={0}&ModuleType=DIYSYS"
                                                                                                DataTextField="WorkDetail" HeaderText="已审核的工作" Target="right">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:BoundColumn DataField="Requisite" HeaderText="必需">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Operation" HeaderText="动作">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CheckingTime" HeaderText="审核时间">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="11%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="WLName" HeaderText="工作流名称">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                DataTextField="CreatorName" HeaderText="Applicant" Target="_blank">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:HyperLinkColumn>
                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <ItemTemplate>
                                                                                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?RelatedType=WorkFlow&WLID={0}") %>'
                                                                                                        Target="_blank"><img src="ImagesSkin/Doc.gif" alt ="Fold Logo"  class="noBorder"/></asp:HyperLink>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>

                                                                                        <ItemStyle CssClass="itemStyle" />
                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                    </asp:DataGrid><asp:Label ID="LB_Sql6" runat="server" Visible="False"></asp:Label>
                                                                                </ContentTemplate>
                                                                            </cc2:TabPanel>
                                                                        </cc2:TabContainer>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 15px">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;" align="left">

                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,WoDeGongZuoLiu%>"></asp:Label>：
                                                                                </td>
                                                                                <td>(</td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="IMB_Yellow" ImageUrl="~/Images/lamp_yellow.png" runat="server" /></td>
                                                                                <td>
                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,ShenPiZhong%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="IMB_refuse" ImageUrl="~/Images/lamp_refuse.png" runat="server" /></td>
                                                                                <td>
                                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,BoHui%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="IMB_Green" ImageUrl="~/Images/lamp_green.png" runat="server" /></td>
                                                                                <td>
                                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,TongGuo%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="IMB_Red" ImageUrl="~/Images/lamp_ok.png" runat="server" /></td>
                                                                                <td>
                                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,JieAn%>"></asp:Label></td>
                                                                                <td>)</td>

                                                                            </tr>
                                                                        </table>


                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <strong></strong>
                                                                                            </td>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="35%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,GongZuo%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>&nbsp;
                                                                                                </strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="16%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,MuBan%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="20%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,FaQiShiJian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="8%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid3" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid3_PageIndexChanged"
                                                                            PageSize="5" Width="98%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="IMB_Lamp" ImageUrl="~/Images/lamp_yellow.png" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="WLID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTDIYSystemDataHandleDetailForMeMain.aspx?WLID={0}"
                                                                                    DataTextField="WLName" HeaderText="Workflow" Target="right">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="35%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTWFChartViewJS.aspx?WLID={0}"
                                                                                    HeaderText="Workflow" Target="_blank" Text="<%$ Resources:lang,JinDu%>">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:TemplateColumn HeaderText="HomeName">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetWorkflowTypeHomeName(Eval("WLType").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="TemName" HeaderText="模板">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CreateTime" HeaderText="申请时间">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn>
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?RelatedType=WorkFlow&WLID={0}") %>'
                                                                                            Target="_blank"><img src="ImagesSkin/Doc.gif" alt ="Folder Icon"  class="noBorder"/></asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="170px" style="padding: 5px 5px 0px 5px; border-left: solid 1px #D8D8D8"
                                                valign="top">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 170px; text-align: left">
                                                            <asp:Button ID="BT_AllWL" runat="server" CssClass="inpuLong" OnClick="BT_AllWL_Click"
                                                                Text="<%$ Resources:lang,WoSuoYouDeGongZuo%>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 170px; text-align: left">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,AnZhuangTaiFenLei%>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid2_ItemCommand"
                                                                ShowHeader="false" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="按状态分类：">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 650px; height: 35px; overflow: auto;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,Cong%>"></asp:Label>：
                                    </td>
                                    <td style="width: 220px;" align="left">

                                        <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_StartTime">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,Dao%>"></asp:Label>：
                                    </td>
                                    <td style="width: 220px;" align="left">


                                        <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_EndTime">
                                        </ajaxToolkit:CalendarExtender>

                                    </td>
                                    <td>
                                        <asp:Button ID="BT_FindTime" runat="server" CssClass="inpu" OnClick="BT_FindTime_Click"
                                            Text="<%$ Resources:lang,ChaXun%>" />
                                    </td>
                                    <td style="width: 60px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 50%; top: 50%;">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="Images/Processing.gif" alt="Loading,please wait..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
