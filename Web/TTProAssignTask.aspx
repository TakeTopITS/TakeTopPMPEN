<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProAssignTask.aspx.cs"
    Inherits="TTProAssignTask" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                                    <table cellpadding="0" cellspacing="0" width="100%">
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
                                                                        <asp:Label ID="LB_AssignTask" runat="server" Text="<%$ Resources:lang,assignTask%>"></asp:Label>
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
                                            <td>
                                                <table style="width: 100%; height: 1px;">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello"  OnClick="BT_Create_Click" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,FenPai%>" /></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdTaskID" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label>
                                                                                    </strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdTaskType" runat="server" Text="<%$ Resources:lang,Type%>"></asp:Label>
                                                                                    </strong>
                                                                                </td>
                                                                                <td width="17%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdTask" runat="server" Text="<%$ Resources:lang,Task%>"></asp:Label>
                                                                                    </strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdPirority" runat="server" Text="<%$ Resources:lang,Pirority%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="8%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdStartTime" runat="server" Text="<%$ Resources:lang,StartTime%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="8%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdEndTime" runat="server" Text="<%$ Resources:lang,EndTime%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdExpenseBudget" runat="server" Text="<%$ Resources:lang,ExpenseBudget%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdManpowerBudget" runat="server" Text="<%$ Resources:lang,ManpowerBudget%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="106px" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdProgress" runat="server" Text="<%$ Resources:lang,Progress%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdExpense" runat="server" Text="<%$ Resources:lang,Expense%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdManHour" runat="server" Text="<%$ Resources:lang,ManHour%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_tdStatus" runat="server" Text="<%$ Resources:lang,Status%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <strong></strong>
                                                                                </td>

                                                                                <td align="left">
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
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="25"
                                                                OnItemCommand="DataGrid1_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                ShowHeader="false" Width="100%" Height="1px" CellPadding="4" ForeColor="#333333"
                                                                GridLines="None">
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
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="TaskID" DataTextField="Task" DataNavigateUrlFormatString="TTTaskAssignRecord.aspx?TaskID={0}"
                                                                        HeaderText="分派记录" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="17%" />
                                                                    </asp:HyperLinkColumn>

                                                                    <%--                                                                    <asp:BoundColumn DataField="Task" HeaderText="Task">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                                    </asp:BoundColumn>--%>
                                                                    <asp:BoundColumn DataField="Priority" HeaderText="优先级">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Budget" HeaderText="费用预算">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ManHour" HeaderText="工时预算">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <div class="MYgreen"></div>
                                                                            <asp:Label ID="LB_FinishPercent" runat="server" BackColor="YellowGreen" Width="30px" Text='<%#DataBinder .Eval (Container .DataItem ,"FinishPercent") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="98px" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>

                                                                            <a href='TTProExpenseView.aspx?TaskID=<%#DataBinder .Eval (Container .DataItem ,"TaskID") %>'><%#DataBinder .Eval (Container .DataItem ,"Expense") %>  </a>

                                                                        </ItemTemplate>

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn DataField="RealManHour" HeaderText="LaborHours">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
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
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                            </asp:DataGrid>
                                                            <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
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
                            <asp:Label ID="Label388" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="text-align: left; overflow: auto; padding: 0px 5px 0px 5px;">

                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" Width="100%" runat="server" ActiveTabIndex="0">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="基本信息" TabIndex="0">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,JiBenXinXi%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbType" runat="server" Text="<%$ Resources:lang,Type %>"></asp:Label>
                                                    :
                                                </td>
                                                <td style="width: 20%; "  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Type" runat="server" DataTextField="Type" DataValueField="Type">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 20%; " class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbPirority" runat="server" Text="<%$ Resources:lang,Pirority %>"></asp:Label>:
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Priority" runat="server" DataTextField="Priority" DataValueField="Priority">
                                                        <asp:ListItem Value="Normal" Text="<%$ Resources:lang,PuTong %>" ></asp:ListItem>
                                                          <asp:ListItem Value="High" Text="<%$ Resources:lang,Gao %>" ></asp:ListItem>
                                                         <asp:ListItem Value="Low" Text="<%$ Resources:lang,Di2 %>" ></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbExpenseBudget" runat="server" Text="<%$ Resources:lang,ExpenseBudget %>"></asp:Label></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_Budget" runat="server" Width="67px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>

                                                    <asp:Label ID="LB_TaskNO" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_TaskID" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbTask" runat="server" Text="<%$ Resources:lang,Task %>"></asp:Label>: </td>
                                                <td colspan="3" rowspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Task" runat="server" Width="95%" Height="49px" TextMode="MultiLine"></asp:TextBox></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbExpense" runat="server" Text="<%$ Resources:lang,Expense %>"></asp:Label>: </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_Expense" runat="server" Enabled="False" Width="67px" OnBlur=""
                                                        OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbManhourBudget" runat="server" Text="<%$ Resources:lang,ManhourBudget %>"></asp:Label>: </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ManHour" runat="server" Width="53px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbStartTime" runat="server" Text="<%$ Resources:lang,StartTime %>"></asp:Label>: </td>
                                                <td style="width: 220px; " class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_BeginDate" runat="server"></asp:TextBox><ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate" Enabled="True"></ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbEndTime" runat="server" Text="<%$ Resources:lang,EndTime %>"></asp:Label>: </td>
                                                <td style="width: 220px; "  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_EndDate" runat="server"></asp:TextBox><ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                        runat="server" TargetControlID="DLC_EndDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbManHour" runat="server" Text="<%$ Resources:lang,ManHour %>"></asp:Label>: </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_RealManHour" runat="server" Width="53px" Enabled="False"
                                                        OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox></td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YaoQiuShuLiang %>" />&nbsp;</td>
                                                <td class="formItemBgStyleForAlignLeft">

                                                    <NickLee:NumberBox MaxAmount="1000000000000" ID="NB_RequireNumber" runat="server" Width="79px" MinAmount="0" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YiWanChenLiang %>" />&nbsp;</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="NB_FinishedNumber" runat="server" MaxAmount="1000000000000" MinAmount="0" Width="79px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,DanJia %>" />
                                                    <NickLee:NumberBox ID="NB_Price" runat="server" MaxAmount="1000000000000" MinAmount="0" Width="79px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>：
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:DropDownList ID="DL_UnitName" runat="server" DataTextField="UnitName" DataValueField="UnitName" CssClass="DDList">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbStatus" runat="server" Text="<%$ Resources:lang,Status %>"></asp:Label>: </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" DataTextField="HomeName" DataValueField="Status" />
                                                
                                                    <asp:Label ID="LB_Status" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbCreateTime" runat="server" Text="<%$ Resources:lang,CreateTime %>"></asp:Label>: </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_MakeDate" runat="server"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="LB_tbProgress" runat="server" Text="<%$ Resources:lang,Progress %>"></asp:Label>: </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_FinishPercent" Enabled="False" runat="server" Width="43px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>% </td>
                                            </tr>
                                            <tr style="display: none;">

                                                <td class="formItemBgStyleForAlignLeft" style="height: 2px;"></td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="5" style="height: 2px;">
                                                    <asp:Button ID="BT_Close" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Close_Click" Text="<%$ Resources:lang,Close %>" Visible="False" />
                                                    &nbsp;<asp:Button ID="BT_Active" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Active_Click" Text="<%$ Resources:lang,Activate %>" Visible="False" />
                                                    <asp:HyperLink ID="HL_WLTem" runat="server" Enabled="False" NavigateUrl="~/TTWorkFlowTemplate.aspx" Target="_blank" Text="<%$ Resources:lang,WFTemplate %>" Visible="False"></asp:HyperLink>
                                                    &nbsp;<asp:HyperLink ID="HL_RelatedWorkFlowTemplate" runat="server" Enabled="False" NavigateUrl="TTProRelatedWFTemplate.aspx" Target="_blank" Text="<%$ Resources:lang,RelatedWFTemplate %>" Visible="False"></asp:HyperLink>
                                                    &nbsp;<asp:HyperLink ID="HL_ActorGroup" runat="server" Enabled="False" NavigateUrl="~/TTProjectRelatedActorGroup.aspx" Target="_blank" Text="<%$ Resources:lang,ActorGroup %>" Visible="False"></asp:HyperLink>
                                                </td>
                                        </table>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="关联信息" TabIndex="2">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <asp:Panel ID="Panel_RelatedBusiness" runat="server" Visible="false">
                                            <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                <%--   <tr>
                                        <td colspan="8" align="left" style="padding-right: 7%;">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan %>"></asp:Label>
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
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YunXuHouXuXiuGe%>"></asp:Label>：
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
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClientClick="window.frames['IFrame_RelatedInformation'].document.getElementById('BT_SaveXMLFile').click()" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton>
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popAssignWindow" name="noConfirm"
                        style="z-index: 9999; width: 800px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label4" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">

                                <tr>
                                    <td style="width: 10%; " class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbAssignType" runat="server" Text="<%$ Resources:lang,Type %>"></asp:Label>
                                        :   </td>
                                    <td style="width: 50%; "  class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_RecordType" runat="server" DataTextField="Type" DataValueField="Type">
                                        </asp:DropDownList><asp:Label ID="LB_ID" runat="server"></asp:Label></td>
                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbOperator" runat="server" Text="<%$ Resources:lang,Executor %>"></asp:Label>
                                        : </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_OperatorCode" runat="server" DataTextField="UserName" DataValueField="UserCode" AutoPostBack="True" OnSelectedIndexChanged="DL_OperatorCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:HyperLink ID="HL_MemberWorkload" runat="server" Target="_blank">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChaKanGongZuoFuHe%>"></asp:Label>
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GongZuoYaoQiu%>"></asp:Label>：</td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                        <CKEditor:CKEditorControl ID="HE_Operation" runat="server" Height="100px" Width="99%" Visible="False" />
                                        <CKEditor:CKEditorControl runat="server" ID="HT_Operation" Width="99%" Height="150px" Visible="False" />

                                        <asp:DropDownList ID="DL_WorkRequest" runat="server" AutoPostBack="True" DataTextField="Operation"
                                            DataValueField="Operation" OnSelectedIndexChanged="DL_WorkRequest_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbAssignStartTime" runat="server"
                                            Text="<%$ Resources:lang,StartTime %>"></asp:Label>: </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_TaskBegin" runat="server" AutoPostBack="true" OnTextChanged="DLC_TaskBegin_TextChanged"></asp:TextBox><ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_TaskBegin" Enabled="True"></ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_tbAssignEndTime" runat="server" Text="<%$ Resources:lang,EndTime %>"></asp:Label>: </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_TaskEnd" runat="server" AutoPostBack="true" OnTextChanged="DLC_TaskEnd_TextChanged"></asp:TextBox><ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4" runat="server" TargetControlID="DLC_TaskEnd" Enabled="True"></ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                        <asp:Button ID="BT_UpdateAssign" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_UpdateAssign_Click"
                                            Text="<%$ Resources:lang,BaoCun %>" />&#160;<asp:Button ID="BT_DeleteAssign" runat="server" CssClass="inpu" Enabled="False"
                                                OnClick="BT_DeleteAssign_Click" Text="<%$ Resources:lang,Delete %>" OnClientClick="return confirm(getDeleteMsgByLangCode())" />&#160;<asp:Button ID="BT_Assign" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Assign_Click"
                                                    Text="<%$ Resources:lang,Assign %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td style="height: 19px; " colspan="3" class="formItemBgStyleForAlignLeft">

                                        <asp:CheckBox ID="CB_SendMsg" runat="server" Font-Bold="False" Font-Size="10pt" Text="<%$ Resources:lang,SendSMS %>" /><asp:CheckBox ID="CB_SendMail" runat="server" Font-Bold="False" Font-Size="10pt"
                                            Text="<%$ Resources:lang,SendEMail %>" /><span style="font-size: 10pt"><asp:Label ID="LB_tbNoticeOperator" runat="server" Text="<%$ Resources:lang,NoticeOperator %>"></asp:Label><asp:TextBox ID="TB_Message" runat="server" Width="45%"></asp:TextBox><asp:Button ID="BT_Send" runat="server" CssClass="inpu" OnClick="BT_Send_Click" Text="<%$ Resources:lang,Send %>" /></span></td>
                                </tr>
                            </table>
                            <br />
                            <strong>
                                <asp:Label ID="LB_TaskName" runat="server" Visible="False"></asp:Label></strong>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="10%" align="left"><strong>
                                                    <asp:Label ID="LB_dgAssignID" runat="server" Text="<%$ Resources:lang,AssignID %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgType" runat="server" Text="<%$ Resources:lang,Type %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgTaskID" runat="server" Text="<%$ Resources:lang,TaskID %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgAssignPeople" runat="server" Text="<%$ Resources:lang,AssignPeople %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgOperator" runat="server" Text="<%$ Resources:lang,Operator %>"></asp:Label></strong></td>
                                                <td width="10%" align="left"><strong>
                                                    <asp:Label ID="LB_dgWorkOperation" runat="server" Text="<%$ Resources:lang,WorkOperation %>"></asp:Label></strong></td>
                                                <td width="12%" align="left"><strong>
                                                    <asp:Label ID="LB_dgStartTime" runat="server" Text="<%$ Resources:lang,StartTime %>"></asp:Label></strong></td>
                                                <td width="12%" align="left"><strong>
                                                    <asp:Label ID="LB_dgEndTime" runat="server" Text="<%$ Resources:lang,EndTime %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgExpense" runat="server" Text="<%$ Resources:lang,Expense %>"></asp:Label></strong></td>
                                                <td width="8%" align="left"><strong>
                                                    <asp:Label ID="LB_dgAssignStatus" runat="server" Text="<%$ Resources:lang,Status %>"></asp:Label></strong></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ShowHeader="False" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid2_ItemCommand"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Number">
                                        <ItemTemplate>
                                            <asp:Button ID="BT_ID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                                CssClass="inpu" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="TaskID" HeaderText="任务号">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AssignManName" HeaderText="分派人">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="OperatorName" HeaderText="受理人">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Operation" HeaderText="受理人的工作">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Expense" HeaderText="Expense">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:TemplateColumn>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditItemStyle BackColor="#2461BF" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                <ItemStyle CssClass="itemStyle" />
                            </asp:DataGrid>
                        </div>

                        <div id="popwindow_footer1" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
