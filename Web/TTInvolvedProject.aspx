<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTInvolvedProject.aspx.cs"
    Inherits="TTInvolvedProject" %>

<%@ Register Assembly="ZedGraph.Web" Namespace="ZedGraph.Web" TagPrefix="cc1" %>
<%@ Register Assembly="ZedGraph" Namespace="ZedGraph" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我参与的项目</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #AboveDiv {
            min-width: 1280px;
            width: expression (document.body.clientWidth <= 1280? "1280px" : "auto" ));
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
                        <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                            <tr>
                                <td style="text-align: left;">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 5px;">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left" style="padding-left: 20px; font-weight: bold; height: 24px; color: #394f66; background-image: url('ImagesSkin/titleBG.jpg')">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td style="text-align: left; width: 40%;">
                                                                        <span style="font-size: 9pt">
                                                                            <asp:Label ID="LB_MyQueryScope" runat="server" Text="<%$ Resources:lang,MyQueryScope%>"></asp:Label>:
                                                                    <asp:Label ID="LB_QueryScope" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: left; width: 30%;"></td>
                                                                    <td style="text-align: right; width: 30%;">
                                                                        <asp:Label ID="LB_Operator" runat="server" Text="<%$ Resources:lang,Operator%>" />:
                                                                        <asp:Label ID="LB_UserCode" runat="server"></asp:Label>

                                                                        <asp:Label ID="LB_UserName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
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
                                                                                        <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,DGProjectID%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="12%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ProjectCode%>"></asp:Label>
                                                                                    </strong>
                                                                                </td>
                                                                                <td width="20%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGProjectName" runat="server" Text="<%$ Resources:lang,ProjectName%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGMyActor" runat="server" Text="<%$ Resources:lang,MyActor%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGJoinTime" runat="server" Text="<%$ Resources:lang,JoinTime%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="11%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGPM" runat="server" Text="<%$ Resources:lang,PM%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="90px" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGProgress" runat="server" Text="<%$ Resources:lang,Progress%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="70px" align="center">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GongShi2%>"></asp:Label></strong>
                                                                                </td>
                                                                          
                                                                                <td width="9%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DGStatus" runat="server" Text="<%$ Resources:lang,Status%>"></asp:Label></strong>
                                                                                </td>

                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JiHua%>"></asp:Label>
                                                                                    </strong>
                                                                                </td>
                                                                              
                                                                                <td align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,WenDang%>"></asp:Label>
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
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="false" PageSize="10"
                                                                ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                Width="100%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="ProjectID" HeaderText="Number">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ProjectCode" HeaderText="Code">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="ProjectID" DataNavigateUrlFormatString="TTInvolvedProDetail.aspx?ProjectID={0}"
                                                                        DataTextField="ProjectName" HeaderText="项目名称" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:BoundColumn DataField="Actor" HeaderText="我的角色">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="JoinDate" HeaderText="加入时间" DataFormatString="{0:yyyy/MM/dd}">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:HyperLinkColumn DataNavigateUrlField="PMCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                        DataTextField="PMName" HeaderText="ProjectManager" Target="_blank">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                    </asp:HyperLinkColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <a href="TTWorkPlanGanttForProjectStandardActivityCompareMain.aspx?ProjectID=<%#DataBinder .Eval (Container .DataItem ,"ProjectID") %>"
                                                                                target="_blank">
                                                                                <div class="green" style="text-align: left;">
                                                                                    <asp:Label ID="LB_FinishPercent" runat="server" Height="20px" Font-Size="XX-Small"
                                                                                        BackColor="YellowGreen" Text='<%#DataBinder .Eval (Container .DataItem ,"FinishPercent") %>'></asp:Label>
                                                                                </div>
                                                                                <div class="yellow" style="text-align: right;">
                                                                                    <asp:Label ID="LB_DefaultPercent" runat="server" Height="20px" Width="15px" Font-Size="XX-Small"
                                                                                        BackColor="Yellow"></asp:Label>
                                                                                </div>
                                                                            </a>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="100px" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <a href="TTInvolvedDailyWorkReport.aspx?ProjectID=<%#DataBinder .Eval (Container .DataItem ,"ProjectID") %>"
                                                                                target="_blank">
                                                                                <asp:Label ID="LB_WorkhourNumber" runat="server" Text='<%# GetProjectMemberTotalConfirmWorkHour(Eval("ProjectID").ToString()) %>' Width="50px" Height="20px" Font-Size="XX-Small"
                                                                                    BackColor="YellowGreen"></asp:Label>
                                                                            </a>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" VerticalAlign="Middle" Width="50px" />
                                                                    </asp:TemplateColumn>
                                                                    <%-- 2013-11-28 LiuJianping--%>
                                                                    <asp:TemplateColumn Visible="false">
                                                                        <ItemTemplate>
                                                                            <a href="TTProjectCostOperationView.aspx?ProjectID=<%#DataBinder .Eval (Container.DataItem ,"ProjectID") %>" target="_blank">
                                                                                <asp:Label ID="LB_PercentRea" runat="server" Height="15px" Font-Size="XX-Small" BackColor="YellowGreen" Text='<%#DataBinder.Eval (Container.DataItem ,"PercentRea") %>'></asp:Label></a>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="100px" />
                                                                    </asp:TemplateColumn>
                                                                    <%-- 2013-11-28 LiuJianping--%>
                                                                    <asp:BoundColumn DataField="Priority" HeaderText="优先级" Visible="false">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                    </asp:BoundColumn>

                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass. GetStatusHomeNameByProjectStatus(Eval("Status").ToString(),Eval("ProjectType").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="FinishPercent" Visible="False">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:BoundColumn>

                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <a href='TTWorkPlanMain.aspx?ProjectID=<%#DataBinder.Eval (Container .DataItem ,"ProjectID") %>'>
                                                                                <img src="ImagesSkin/plan.png" alt="ProjectPlan" width="32px" height="32px" style="border: none;" />
                                                                            </a>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ProjectID", "TTDocumentTreeView.aspx?RelatedType=Project&RelatedID={0}") %>'
                                                                                Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder" alt="" /></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <ItemStyle CssClass="itemStyle" />
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            </asp:DataGrid>
                                                            <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; padding: 5px 5px 5px 5px; text-align: left;" valign="top">
                                                            <cc2:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" Width="100%" runat="server" ActiveTabIndex="0">
                                                                <cc2:TabPanel ID="TabPanel3" runat="server" HeaderText="项目状态">

                                                                    <HeaderTemplate>

                                                                        <asp:Label ID="LB_ProjectStatusChart" runat="server" Text="<%$ Resources:lang,ProjectStatusChart%>"></asp:Label>
                                                                    </HeaderTemplate>

                                                                    <ContentTemplate>

                                                                        <table width="100%">

                                                                            <tr>

                                                                                <td>

                                                                                    <div class="renyList">

                                                                                        <asp:Repeater ID="RP_ChartList" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="LB_ChartName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ChartName") %>' Visible="false"></asp:Label>
                                                                                                <asp:Label ID="LB_ChartType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ChartType") %>' Visible="false"></asp:Label>
                                                                                                <asp:Label ID="LB_SqlCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SqlCode") %>' Visible="false"></asp:Label>

                                                                                                <iframe src="TTTakeTopAnalystChartSet.aspx?FormType=<%# DataBinder.Eval(Container.DataItem,"FormType") %>&ChartType=<%# DataBinder.Eval(Container.DataItem,"ChartType") %>&ChartName=<%# DataBinder.Eval(Container.DataItem,"ChartName") %>" style="width: 300px; height: 295px; border: 1px solid white; text-align: center; overflow: hidden;"></iframe>
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                        <br />
                                                                                        <br />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>

                                                                                <td align="center" style="vertical-align: bottom;">

                                                                                    <asp:HyperLink ID="HL_SystemAnalystChartRelatedUserSet" runat="server" Text="<%$ Resources:lang,FenXiTuSheZhi%>"></asp:HyperLink>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </cc2:TabPanel>
                                                                <cc2:TabPanel ID="TabPanel2" runat="server" HeaderText="项目费用">

                                                                    <HeaderTemplate>

                                                                        <asp:Label ID="LB_ProjectExpense" runat="server" Text="<%$ Resources:lang,ProjectExpense%>"></asp:Label>
                                                                    </HeaderTemplate>

                                                                    <ContentTemplate>

                                                                        <table>

                                                                            <tr>

                                                                                <td style="width: 30px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer1" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost1" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer1" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg1"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg1" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer2" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost2" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer2" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg2"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg2" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer3" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost3" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer3" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg3"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg3" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer4" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost4" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer4" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg4"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg4" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer5" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost5" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer5" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg5"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg5" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer6" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost6" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer6" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg6"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg6" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer7" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost7" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer7" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg7"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg7" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer8" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost8" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer8" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg8"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg8" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer9" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost9" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer9" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton ID="IMB_ProBdg9"
                                                                                                    runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg" Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg9" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td style="width: 60px; text-align: center; vertical-align: bottom; height: 164px;">

                                                                                    <table>

                                                                                        <tr>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_CostPer10" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProCost10" runat="server" ImageUrl="~/Images/FinishPercentCost.jpg" Width="30px" />
                                                                                            </td>

                                                                                            <td style="width: 30px; text-align: center; vertical-align: bottom;">

                                                                                                <asp:Label ID="LB_BdgPer10" runat="server" Text="1"></asp:Label><br />

                                                                                                <asp:ImageButton
                                                                                                    ID="IMB_ProBdg10" runat="server" ImageUrl="~/Images/FinishPercentBudget.jpg"
                                                                                                    Width="30px" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td colspan="2">

                                                                                                <asp:Label ID="LB_ProBdg10" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </cc2:TabPanel>
                                                                <cc2:TabPanel ID="TabPanel4" runat="server" HeaderText="综合查询">

                                                                    <HeaderTemplate>

                                                                        <asp:Label ID="LB_IntegratedQuery" runat="server" Text="<%$ Resources:lang,IntegratedQuery%>"></asp:Label>
                                                                    </HeaderTemplate>

                                                                    <ContentTemplate>

                                                                        <table style="width: 80%;" cellpadding="3" cellspacing="0" class="formBgStyle">

                                                                            <tr>

                                                                                <td style="width: 15%; " class="formItemBgStyleForAlignLeft">

                                                                                    <span>

                                                                                        <asp:Label ID="LB_ProjectName" runat="server" Text="<%$ Resources:lang,ProjectName %>"></asp:Label>:</span>
                                                                                </td>

                                                                                <td style="width: 35%; "  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:TextBox ID="TB_ProjectName" runat="server" Width="95%" Font-Size="10pt"></asp:TextBox>
                                                                                </td>

                                                                                <td style="width: 15%;"  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Button ID="BT_HazyFind" runat="server" OnClick="BT_HazyFind_Click" Text="<%$ Resources:lang,FuzzySearch %>"
                                                                                        Font-Size="10pt" CssClass="inpuLong" />
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <span>

                                                                                        <asp:Label ID="LB_ProjectID" runat="server" Text="<%$ Resources:lang,ProjectID %>"></asp:Label>:</span>
                                                                                </td>

                                                                                <td  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:TextBox ID="TB_ProjectID" runat="server"></asp:TextBox>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Button ID="BT_ProjectIDFind" runat="server" Text="<%$ Resources:lang,Find %>"
                                                                                        OnClick="BT_ProjectIDFind_Click" CssClass="inpuLong" />
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <span>

                                                                                        <asp:Label ID="LB_ProjectCreator" runat="server" Text="<%$ Resources:lang,ProjectCreator %>"></asp:Label>:</span>
                                                                                </td>

                                                                                <td  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:TextBox ID="TB_MakeUser" runat="server" Width="95%"></asp:TextBox>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Button ID="BT_MakeUserFind" runat="server" OnClick="BT_MakeUserFind_Click" Text="<%$ Resources:lang,Find %>"
                                                                                        CssClass="inpuLong" />
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Label ID="LB_StartTime" runat="server" Text="<%$ Resources:lang,StartTime %>"></asp:Label>:<br />
                                                                                </td>

                                                                                <td  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:TextBox ID="DLC_BeginDate" runat="server"></asp:TextBox>

                                                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BeginDate" Enabled="True">
                                                                                    </ajaxToolkit:CalendarExtender>
                                                                                </td>

                                                                                <td rowspan="2" style="vertical-align: middle; " class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Button ID="BT_DateFind" runat="server" OnClick="BT_DateFind_Click" Text="<%$ Resources:lang,Find %>"
                                                                                        CssClass="inpuLong" />
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Label ID="LB_EndTime" runat="server" Text="<%$ Resources:lang,EndTime %>"></asp:Label>:
                                                                                </td>

                                                                                <td  class="formItemBgStyleForAlignLeft">

                                                                                    <asp:TextBox ID="DLC_EndDate" runat="server"></asp:TextBox>

                                                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                                                                        runat="server" TargetControlID="DLC_EndDate" Enabled="True">
                                                                                    </ajaxToolkit:CalendarExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                        <br />
                                                                    </ContentTemplate>
                                                                </cc2:TabPanel>
                                                            </cc2:TabContainer>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="border-left: solid 1px #D8D8D8; padding: 5px 0px 0px 5px; width: 165px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 165px; height: 14px; padding-top: 5px; text-align: center">
                                                <asp:Button ID="BT_AllRelatedProject" runat="server" OnClick="BT_AllRelatedProject_Click"
                                                    Text="<%$ Resources:lang,MyInvolvedProject%>" CssClass="inpuLong" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 165; height: 1px; text-align: center; padding-top: 5px;">
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
                                                                            <asp:Label ID="LB_FindByStatus" runat="server" Text="<%$ Resources:lang,FindByStatus%>"></asp:Label></strong>
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
                                                    ShowHeader="false" Width="165" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="按项目状态分类：">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_Status" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'
                                                                    CssClass="inpu" Visible="false" />
                                                                <asp:Button ID="BT_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'
                                                                    CssClass="inpu" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%-- <td width="250" valign="top" style="padding: 5px 5px 5px 5px; border-left: solid 1px #D8D8D8; border-right: solid 1px #D8D8D8;"
                                                align="left">
                                                <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                    ShowLines="True" Width="250">
                                                    <RootNodeStyle CssClass="rootNode" />
                                                    <NodeStyle CssClass="treeNode" />
                                                    <LeafNodeStyle CssClass="leafNode" />
                                                    <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                </asp:TreeView>
                                            </td>--%>
                            </tr>
                        </table>
                        <br />
                        </td>
                            </tr>
                        </table>
                    </div>
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
