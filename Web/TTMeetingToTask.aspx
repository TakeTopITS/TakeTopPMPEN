<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMeetingToTask.aspx.cs" Inherits="TTMeetingToTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 980px;
            width: expression (document.body.clientWidth <= 980? "980px" : "auto" ));
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

        //设置IFRMAE的高度
        function setBusinessFormIFrameHeight() {

            var winHeight = 0;

            winHeight = document.getElementById("popwindow").style.height;

            document.getElementById("TabContainer1_TabPanel2_IFrame_RelatedInformation").height = (winHeight.toString().replace("px", "") - 160);
        }
        window.onmousemove = setBusinessFormIFrameHeight;

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
                                <td>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td height="31" class="page_topbj">
                                                <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left">
                                                            <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="29">
                                                                        <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                                    </td>
                                                                    <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,HuiYiZhuanRenWu%>"></asp:Label>
                                                                    </td>
                                                                    <td width="5">
                                                                        <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; padding: 5px 5px 5px 5px;" valign="top">
                                                <table cellpadding="3" cellspacing="0" style="width: 100%">

                                                    <tr>
                                                        <td style="height: 11px; text-align: left">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,FenPai%>" /></strong>
                                                                    </td>
                                                                                <td align="left" width="5%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="8%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="20%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,RenWu%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="8%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YouXianJi%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="8%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="8%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="5%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="7%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="7%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,FeiYong%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left" width="5%">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <strong></strong>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <strong>&nbsp;
                                                                                    </strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="20"
                                                                ShowHeader="False" OnItemCommand="DataGrid1_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                <Columns>
                                                                    <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:ButtonColumn>
                                                                    <asp:TemplateColumn HeaderText="Delete">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:ButtonColumn ButtonType="LinkButton" CommandName="Assign" Text="&lt;div&gt;&lt;img src=ImagesSkin/Assign.png border=0 alt='Deleted' /&gt;&lt;/div&gt;">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:ButtonColumn>
                                                                    <asp:BoundColumn DataField="TaskID" HeaderText="ID">
                                                                        <ItemStyle CssClass="itemBorder" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                        <ItemStyle CssClass="itemBorder" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="TaskID" DataTextField="Task" DataNavigateUrlFormatString="TTTaskAssignRecord.aspx?TaskID={0}"
                                                                        HeaderText="分派记录" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:BoundColumn DataField="Priority" HeaderText="优先级">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Budget" HeaderText="Budget">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FinishPercent" HeaderText="完成程度">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>

                                                                            <a href='TTProExpenseView.aspx?TaskID=<%#DataBinder .Eval (Container .DataItem ,"TaskID") %>'><%#DataBinder .Eval (Container .DataItem ,"Expense") %>  </a>

                                                                        </ItemTemplate>

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="TaskID" DataNavigateUrlFormatString="TTMakeTaskTestCase.aspx?TaskID={0}"
                                                                        Text="&lt;div&gt;&lt;img src=ImagesSkin/TestCase.jpg border=0 alt='用例' /&gt;&lt;/div&gt;" HeaderText="测试用例" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>

                                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.TaskID", "TTProTaskRelatedDoc.aspx?TaskID={0}") %>'
                                                                                Target="_blank"><img src ="ImagesSkin/Doc.gif" class="noBorder" /></asp:HyperLink>

                                                                        </ItemTemplate>

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:TemplateColumn>
                                                                    <%--    <asp:ButtonColumn ButtonType="LinkButton" CommandName="Other" Text="- - -">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:ButtonColumn>--%>
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; display: none;">
                                    <asp:Label ID="LB_MeetingID" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 800px; height: 550px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label9" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="text-align: left; overflow: auto; padding: 0px 5px 0px 5px;">

                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" Width="100%" runat="server" ActiveTabIndex="0">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="基本信息" TabIndex="0">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,JiBenXinXi%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <table style="width: 100%;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                   <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>
                                                    ：
                                                </td>
                                                <td style="width: 20%; " class="formItemBgStyleForAlignLeft">
                                                      <asp:DropDownList ID="DL_Type" runat="server" DataTextField="Type" DataValueField="Type">
                                                    </asp:DropDownList><asp:Label ID="LB_TaskNO" runat="server" Visible ="false"></asp:Label>
                                                </td>
                                                <td style="width: 20%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,YouXianJi %>"></asp:Label>：
                                                </td>
                                                <td style="width: 20%; "  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Priority" runat="server" DataTextField="Priority" DataValueField="Priority">

                                                        <asp:ListItem Value="2-H" />
                                                        <asp:ListItem Value="1-H" />
                                                        <asp:ListItem Value="2-M" />
                                                        <asp:ListItem Value="1-M" />
                                                        <asp:ListItem Value="COMMON" />
                                                        <asp:ListItem Value="1-L" />
                                                        <asp:ListItem Value="2-L" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,YuSuan %>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_Budget" runat="server" Width="67px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,RenWu %>"></asp:Label>：
                                                </td>
                                                <td colspan="3" rowspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Task" runat="server" Height="45px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ShiJiFeiYong %>"></asp:Label>：
                                                </td>
                                                <td style="width: 93px; " class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_Expense" runat="server" Enabled="False" Width="65px" OnBlur=""
                                                        OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GongShi2 %>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ManHour" runat="server" Width="67px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_BeginDate" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label>：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_EndDate" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                        runat="server" TargetControlID="DLC_EndDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ShiJiGongShi %>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_RealManHour" runat="server" Enabled="False" Width="53px"
                                                        OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label>：
                                                </td>
                                                <td style="width: 150px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" DataTextField="HomeName" DataValueField="Status"
                                                        Width="103px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,JianLiShiJian %>"></asp:Label>：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_MakeDate" runat="server"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,WanChengChengDu %>"></asp:Label>：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_FinishPercent" Enabled="false" runat="server" Width="47px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                    %
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                <td colspan="5"  class="formItemBgStyleForAlignLeft">
                                                    <asp:Button ID="BT_Close" runat="server" CssClass="inpu" Enabled="False" Visible="false"
                                                        OnClick="BT_Close_Click" Text="<%$ Resources:lang,GuanBi %>" />
                                                    &nbsp;<asp:Button ID="BT_Active" runat="server" CssClass="inpu" Enabled="False" Visible="false" OnClick="BT_Active_Click"
                                                        Text="<%$ Resources:lang,JiHuo %>" />

                                                    <asp:Label runat="server" ID="LB_Status" Visible="false"></asp:Label>
                                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_TaskID" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="关联信息" TabIndex="2">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <asp:Panel ID="Panel_RelatedBusiness" runat="server" Visible="false">
                                            <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                <%--  <tr>
                                                    <td colspan="8" align="left" style="padding-right: 7%;">
                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan %>"></asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label3333" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>: </td>
                                                    <td style="width: 20%; "  class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_WLType" runat="server" DataTextField="HomeName" DataValueField="Type"
                                                            AutoPostBack="true" OnSelectedIndexChanged="DL_WLType_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%; " class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label7333" runat="server" Text="<%$ Resources:lang,BiaoDanGuanLianDeLiuChengMoBan%>"></asp:Label>：
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_WFTemplate" runat="server" DataTextField="TemName" DataValueField="TemName">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%; " class="formItemBgStyleForAlignLeft">
                                                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,YunXuHouXuXiuGe%>"></asp:Label>：
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">
                                                        <asp:DropDownList ID="DL_AllowUpdate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_AllowUpdate_SelectedIndexChanged">
                                                            <asp:ListItem Value="YES" Text="YES"></asp:ListItem>
                                                            <asp:ListItem Value="NO" Text="NO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft">

                                                        <asp:Button ID="BT_StartupBusinessForm" runat="server" CssClass="inpu" Text="<%$ Resources:lang,DaKai %>" OnClick="BT_StartupBusinessForm_Click" />
                                                    </td>
                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <iframe id="IFrame_RelatedInformation" name="IFrame_RelatedInformation" src="TTRelatedDIYBusinessForm.aspx" runat="server" style="width: 100%; overflow: auto;"></iframe>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClientClick="window.frames['IFrame_RelatedInformation'].document.getElementById('BT_SaveXMLFile').click()" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popAssignWindow" name="noConfirm"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label11" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table cellpadding="3" cellspacing="0" style="width: 100%" class="formBgStyle">

                                <tr>
                                    <td style="width: 12%; " class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 30%;;">
                                        <asp:DropDownList ID="DL_RecordType" runat="server" DataTextField="Type" DataValueField="Type">
                                        </asp:DropDownList>
                                        <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 15%; " class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ShouLiRen%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_OperatorCode" runat="server" DataTextField="UserName" DataValueField="UserCode" AutoPostBack="True" OnSelectedIndexChanged="DL_OperatorCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:HyperLink ID="HL_MemberWorkload" runat="server" Target="_blank">
                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ChaKanGongZuoFuHe%>"></asp:Label>
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,GongZuoYaoQiu%>"></asp:Label>：
                                    </td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                        <CKEditor:CKEditorControl ID="HE_Operation" runat="server" Height="100px" Width="99%" Visible="False" />
                                        <CKEditor:CKEditorControl runat="server" ID="HT_Operation" Width="99%" Height="100px" Visible="False" />

                                        <asp:DropDownList ID="DL_WorkRequest" runat="server" AutoPostBack="True" DataTextField="Operation"
                                            DataValueField="Operation" OnSelectedIndexChanged="DL_WorkRequest_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>：
                                    </td>
                                    <td  class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_TaskBegin" runat="server" AutoPostBack="true" OnTextChanged="DLC_TaskBegin_TextChanged"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_TaskBegin" Enabled="True">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_TaskEnd" runat="server" AutoPostBack="true" OnTextChanged="DLC_TaskEnd_TextChanged"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4" runat="server" TargetControlID="DLC_TaskEnd" Enabled="True">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                        <asp:Button ID="BT_UpdateAssign" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_UpdateAssign_Click"
                                            Text="<%$ Resources:lang,BaoCun%>" />
                                        &nbsp;<asp:Button ID="BT_DeleteAssign" runat="server" CssClass="inpu" Enabled="False" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                            OnClick="BT_DeleteAssign_Click" Text="<%$ Resources:lang,ShanChu%>" />
                                        &nbsp;&nbsp;&nbsp;
                                                                                            <asp:Button ID="BT_Assign" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Assign_Click"
                                                                                                Text="<%$ Resources:lang,FenPai%>" />
                                        <td class="formItemBgStyleForAlignLeft" style="height: 21px; ">&nbsp;&nbsp;
                                        </td>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 21px; "></td>
                                            <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 21px; ">
                                                <asp:CheckBox ID="CB_SendMsg" runat="server" Font-Bold="False" Font-Size="10pt" Text="<%$ Resources:lang,FaXinXi%>" />
                                                <asp:CheckBox ID="CB_SendMail" runat="server" Font-Bold="False" Font-Size="10pt"
                                                    Text="<%$ Resources:lang,FaYouJian%>" />
                                                <span style="font-size: 10pt">
                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,TongZhiShouLiRen%>"></asp:Label></span>
                                                <asp:TextBox ID="TB_Message" runat="server" Width="383px"></asp:TextBox>
                                                <asp:Button ID="BT_Send" runat="server" CssClass="inpu" OnClick="BT_Send_Click" Text="<%$ Resources:lang,FaSong%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 9px;">
                                                <span><strong>
                                                    <asp:Label ID="LB_TaskName" runat="server" Visible="False"></asp:Label>
                                                </strong></span>
                                            </td>
                                        </tr>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="text-align: left">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                            <tr>
                                                <td width="7">
                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="9%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="8%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="6%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,RenWuHao%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="8%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,FenPaiRen%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="8%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,ShouLiRen%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="12%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,ShouLiRenDeGongZuo%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="7%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,FeiYong%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,LuXian%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="7%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="6" align="right">
                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            OnItemCommand="DataGrid2_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                            ShowHeader="False" Width="100%" Height="1px" CellPadding="4" ForeColor="#333333"
                                            GridLines="None">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Number">
                                                    <ItemTemplate>
                                                        <asp:Button ID="BT_ID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                                            CssClass="inpu" />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TaskID" HeaderText="任务号">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AssignManName" HeaderText="分派人">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="OperatorName" HeaderText="受理人">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Operation" HeaderText="受理人的工作">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Expense" HeaderText="Expense">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="RouteNumber" HeaderText="路线">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Status">
                                                    <ItemTemplate>
                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="7%" />
                                                </asp:TemplateColumn>

                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                            <ItemStyle CssClass="itemStyle" />
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer1" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popOtherWindow"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title111">
                            <asp:Label ID="Label36" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content112" class="layui-layer-content" style="overflow: auto; text-align: left; padding: 0px 5px 0px 5px;">

                            <table cellpadding="3" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td align="left">
                                        <table style="width: 100%;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td colspan="5" style="height: 4px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:HyperLink ID="HL_TestCase" runat="server" Enabled="False" NavigateUrl="TTMakeTaskTestCase.aspx"
                                                        Target="_blank">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,CeShiYongLi%>"></asp:Label>
                                                    </asp:HyperLink>
                                                    &nbsp;&nbsp;<asp:HyperLink ID="HL_AssignRecord" runat="server" Enabled="False" NavigateUrl="TTTaskAssignRecord.aspx"
                                                        Target="_blank">
                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,FenPaiJiLu%>"></asp:Label>
                                                    </asp:HyperLink>
                                                    &nbsp; &nbsp;
                                                     <asp:HyperLink ID="HL_ActorGroup" runat="server" Enabled="False"
                                                         NavigateUrl="~/TTProjectRelatedActorGroup.aspx" Target="_blank">
                                                         <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JueSeZuSheZhi%>"></asp:Label>
                                                     </asp:HyperLink>
                                                    &nbsp;&nbsp;
                                                       <asp:HyperLink ID="HL_RunTaskByWF" runat="server" Enabled="False" Target="_blank" Visible="false">---&gt;<asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,RenWuChuLiLiuChengMoShi%>"></asp:Label></asp:HyperLink>
                                                    &nbsp;&nbsp;
                                                    <asp:HyperLink ID="HL_TaskReview" runat="server" Enabled="False" Visible="false" Target="_blank">---&gt;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,RenWuPingShen%>"></asp:Label></asp:HyperLink>
                                                    &nbsp;&nbsp;<asp:HyperLink ID="HL_RelatedWorkFlowTemplate" runat="server" Enabled="False"
                                                        NavigateUrl="TTProRelatedWFTemplate.aspx" Target="_blank" Visible="false">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GuanLianGongZuoLiuMuBan%>"></asp:Label>
                                                    </asp:HyperLink>
                                                    &nbsp; &nbsp;
                                                   
                                                    <asp:HyperLink ID="HL_WLTem" runat="server" Enabled="False" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                                        Target="_blank" Visible="false">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                                    </asp:HyperLink>
                                                    &nbsp;&nbsp;

                                                    <asp:HyperLink ID="HL_TaskRelatedDoc" runat="server" Enabled="False" Visible="false"
                                                        NavigateUrl="TTProTaskRelatedDoc.aspx" Target="_blank">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XiangGuanWenDang%>"></asp:Label>
                                                    </asp:HyperLink>


                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer11" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>

                    <asp:DataList ID="DataList1" runat="server" CssClass="bian" CellPadding="0" ForeColor="#333333" Width="100%" Visible="false">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <ItemTemplate>
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td style="width: 10%; text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>：
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"ID") %>
                                    </td>
                                    <td style="width: 10%; text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"Type") %>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,HuiYiMingCheng%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                    </td>

                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhuChiRen%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"Host") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"BeginTime") %>
                                    </td>

                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem,"EndTime") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhaoJiRen%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;" colspan="3">
                                        <%# DataBinder.Eval(Container.DataItem,"Organizer") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,RenWuNeiRong%>"></asp:Label>：
                                    </td>
                                    <td style="text-align: left;" colspan="3">
                                        <%# DataBinder.Eval(Container.DataItem,"Content") %>
                                    </td>
                                </tr>

                            </table>
                        </ItemTemplate>

                        <ItemStyle CssClass="itemStyle" />
                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    </asp:DataList>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
