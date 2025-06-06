<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTContactListSAAS.aspx.cs" Inherits="TTContactListSAAS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1040px;
            width: expression (document.body.clientWidth <= 1040? "1040px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
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
                        <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <a href="TakeTopAPPMain.aspx">
                                                    <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29">
                                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>
                                                            </td>
                                                            <td width="5">
                                                                <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                                        </tr>
                                                    </table>
                                                </a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="4" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <span style="font-size: 10pt">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZongHeChaXunLeiXing %>"></asp:Label>：</span><asp:DropDownList ID="DL_ContactType"
                                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_ContactType_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:lang,QingXuanZe %>" />
                                                        <asp:ListItem Value="Colleague" />
                                                        <asp:ListItem Value="Friend" />
                                                        <asp:ListItem Value="Classmate" />
                                                        <asp:ListItem Value="Customer" />
                                                        <asp:ListItem Value="Supplier" />
                                                        <asp:ListItem Value="Relative" />
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />

                                                    </asp:DropDownList>
                                                <span style="font-size: 10pt">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>：</span><asp:TextBox ID="TB_ContactName" runat="server"
                                                        Width="100px"></asp:TextBox>
                                                <span style="font-size: 9pt">&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GongSi %>"></asp:Label>：<asp:TextBox ID="TB_CompanyName" runat="server" Width="150px"></asp:TextBox>
                                                    &nbsp;<asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click"
                                                        Text="<%$ Resources:lang,MoHuChaXun %>" />
                                                </span>
                                            </td>

                                            <td align="right" style="padding-right: 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />

                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                        </td>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                        </td>

                                                        <td width="6%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XuHao %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="8%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XingMing %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShouJi %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="6%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,XingBie %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="6%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,NianLing %>"></asp:Label></strong>
                                                        </td>
                                                        <td width="16%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,GongSi %>"></asp:Label></strong>
                                                        </td>

                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BanGongDianHua %>"></asp:Label></strong>
                                                        </td>

                                                        <td width="12%" align="left">
                                                            <strong>Email</strong>
                                                        </td>
                                                        <td width="8%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,faxinxi %>"></asp:Label></strong>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                        OnPageIndexChanged="DataGrid1_PageIndexChanged" ShowHeader="False" Width="100%">
                                        <Columns>
                                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                            </asp:ButtonColumn>
                                            <asp:TemplateColumn HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                            </asp:TemplateColumn>

                                            <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="FirstName" HeaderText="Name">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="MobilePhone" HeaderText="手机">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Age" HeaderText="Age">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Company" HeaderText="Company">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                            </asp:BoundColumn>

                                            <asp:BoundColumn DataField="OfficePhone" HeaderText="OfficePhone">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                            </asp:BoundColumn>

                                            <asp:BoundColumn DataField="Email1" HeaderText="Email1">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="发信息">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CB_Select" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder"
                                                    Width="8%"
                                                    HorizontalAlign="left" />
                                            </asp:TemplateColumn>

                                        </Columns>
                                        <ItemStyle CssClass="itemStyle " />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                    </asp:DataGrid>
                                    <br />
                                    <table cellpadding="5" cellspacing="0" class="formBgStyle" width="95%">
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 5%; height: 24px">
                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,NeiRong %>"></asp:Label>：
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_Message" runat="server" Height="96px" TextMode="MultiLine" Width="90%"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" style="height: 21px;"></td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="BT_Send" runat="server" CssClass="inpu" OnClick="BT_Send_Click" Text="<%$ Resources:lang,FaSong %>" />
                                                        </td>

                                                    </tr>
                                                </table>


                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table cellpadding="0" cellspacing="0" width="95%">
                                        <tr>
                                            <td align="left" style="height: 29px;">
                                                <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,DuanXinLieBiao %>"></asp:Label>
                                                ：&nbsp;</td>
                                            <tr>
                                                <td align="left" style="height: 29px;">
                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="left" width="10%"><strong>
                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="55%"><strong>
                                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,XinXi %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="20%"><strong>
                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,FaSongShiJian %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td align="left" width="15%"><strong>
                                                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label>
                                                                        </strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="right" width="6">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid4_ItemCommand" OnPageIndexChanged="DataGrid4_PageIndexChanged" ShowHeader="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Number">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Message" HeaderText="信息内容">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="55%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SendTime" HeaderText="发送时间">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="20%" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <ItemStyle CssClass="itemStyle " />
                                                        <PagerStyle CssClass="notTab" HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:DataGrid>
                                                    <asp:Label ID="LB_Sql4" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px; text-align: left;">
                                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label31" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table style="width: 80%" cellpadding="2" cellspacing="0" class="formBgStyle">
                                <tr>
                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,XingMing %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_FirstName" runat="server" Width="220px"></asp:TextBox>
                                        <span style="color: #ff0000">*</span>
                                        <asp:Label ID="LB_ID" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ShouJi %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_MobilePhone" runat="server"></asp:TextBox>
                                        <span style="color: #ff0000">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">EMail：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_EMail1" runat="server" Width="90%"></asp:TextBox><span style="color: #ff0000">*</span>
                                    </td>

                                    <td style="height: 3px;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>：
                                    </td>
                                    <td style="height: 3px;" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_Type" runat="server">
                                            <asp:ListItem Value="Colleague" />
                                            <asp:ListItem Value="Friend" />
                                            <asp:ListItem Value="Classmate" />
                                            <asp:ListItem Value="Customer" />
                                            <asp:ListItem Value="Supplier" />
                                            <asp:ListItem Value="Relative" />
                                            <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="height: 3px;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XingBie %>"></asp:Label>：
                                    </td>
                                    <td style="height: 3px;" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_Gender" runat="server">
                                            <asp:ListItem Value="Male" Text="<%$ Resources:lang,Nan %>"/>
                                            <asp:ListItem Value="Female" Text="<%$ Resources:lang,Nv %>"/>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,NianLing %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <NickLee:NumberBox ID="NB_Age" runat="server" MaxAmount="100" MinAmount="0" OnBlur=""
                                            OnFocus="" OnKeyPress="" PositiveColor="" Width="50px" Precision="0">0</NickLee:NumberBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,BanGongDianHua %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_OfficePhone" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,JiaTingDianHua %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_HomePhone" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,IM %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_QQ" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,WangZhan %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_WebSite" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,GongSi %>"></asp:Label>：
                                    </td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Company" runat="server" Width="70%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,BuMen %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Department" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhiWu %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Duty" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,GongSiDiZhi %>"></asp:Label>：
                                    </td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_CompanyAddress" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,YouZhengBianMa %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_PostCode" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,GuoJia %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Country" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ShengFen %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_State" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ChengShi %>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_City" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,JiaTingDiZhi %>"></asp:Label>：
                                    </td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_HomeAddress" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>


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

