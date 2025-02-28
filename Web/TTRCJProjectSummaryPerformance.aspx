<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectSummaryPerformance.aspx.cs" Inherits="TTRCJProjectSummaryPerformance" %>
     

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>��Ŀ��֧��Ч������ϸ��Ϣ</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>
    

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

            aHandler();

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
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">��Ŀ��֧��Ч�����б���Ϣ
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">����</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="31">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" class="titlezi">
                                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                <Columns>
                                                                    <asp:ButtonField CommandName="RefreshButton" Text="ˢ��" />
                                                                    <asp:BoundField DataField="ProjectID" HeaderText="��Ŀ��" />
                                                                    <asp:BoundField DataField="ProjectYear" HeaderText="���" />
                                                                    <asp:BoundField DataField="ProjectMonth" HeaderText="�·�" />
                                                                    <asp:BoundField DataField="ProjectSTCV" HeaderText="��ͬ��(STCV)" />
                                                                    <asp:BoundField DataField="ProjectBCWS" HeaderText="�ƻ��ܶ�(BCWS)" />
                                                                    <asp:BoundField DataField="ProjectBCWP" HeaderText="��ֵ�ܶ�(BCWP)" />
                                                                    <asp:BoundField DataField="ProjectBCRWP" HeaderText="Ӧ���ܶ�(BCRWP)" />
                                                                    <asp:BoundField DataField="ProjectPBCWP" HeaderText="ʵ���ܶ�(PBCWP)" />
                                                                    <asp:BoundField DataField="ProjectEAV" HeaderText="�������(EAV)" />
                                                                    <asp:BoundField DataField="ProjectRV" HeaderText="�ؿ�ƫ��(RV)" />
                                                                    <asp:BoundField DataField="ProjectACWP" HeaderText="ʵ�ʳɱ�(ACWP)" />
                                                                    <asp:BoundField DataField="ProjectAI" HeaderText="���㼨Ч(AI)" DataFormatString="{0:p}" />
                                                                    <asp:BoundField DataField="ProjectBVI" HeaderText="�˵���Ч(BVI)" DataFormatString="{0:p}" />
                                                                    <asp:BoundField DataField="ProjectRVI" HeaderText="�ؿЧ(RVI)" DataFormatString="{0:p}" />
                                                                    <asp:BoundField DataField="ProjectPL" HeaderText="��������(P&amp;L)" />
                                                                    <asp:BoundField DataField="ProjectRP" HeaderText="�ɱ���Ч��RP��" DataFormatString="{0:p}" />
                                                                    <asp:BoundField DataField="ProjectTotalSpending" HeaderText="��֧��" />
                                                                    <asp:BoundField DataField="ProjectTotalIncome" HeaderText="������" />
                                                                    <asp:BoundField DataField="ProjectIncomeDifference" HeaderText="��֧�ܲ�" />
                                                                    <asp:BoundField DataField="ProjectContractReceived" HeaderText="��ͬ����" />
                                                                    <asp:BoundField DataField="ProjectCPB" HeaderText="��ͬƫ��(CPB)" />
                                                                    <asp:BoundField DataField="ThisMonthFinished" HeaderText="���������" />
                                                                    <asp:BoundField DataField="TotalMonthFinished" HeaderText="���������" />
                                                                </Columns>
                                                                <EditRowStyle BackColor="#999999" />
                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" class="titlezi">
                                                            <asp:Label ID="lb_showMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="Table1" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi"><u><span style="font-size:10.5pt;mso-bidi-font-size:
11.0pt;font-family:����;mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;
mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;mso-hansi-theme-font:
minor-latin;mso-bidi-font-family:&quot;Times New Roman&quot;;mso-bidi-theme-font:minor-bidi;
mso-ansi-language:EN-US;mso-fareast-language:ZH-CN;mso-bidi-language:AR-SA">��ֵ����ͼ</span></u>&nbsp;</td>
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
                                <td height="31">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" class="titlezi">      
                                                            
                                                               <iframe runat="server" id="IFrame_Chart1" src="TTTakeTopAnalystChartSet.aspx" style="width: 800px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>


                                                            <%--<asp:Chart ID="Chart1" runat="server" Height="500px" Width="800px">
                                                                <Series>
                                                                    <asp:Series Name="Series1" Legend="Legend1" ShadowOffset="1" XValueType="Double"></asp:Series>
                                                                    <asp:Series ChartArea="ChartArea1" Legend="Legend1" Name="Series2">
                                                                    </asp:Series>
                                                                </Series>
                                                                <ChartAreas>
                                                                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                                                </ChartAreas>
                                                                <Legends>
                                                                    <asp:Legend DockedToChartArea="ChartArea1" IsDockedInsideChartArea="False" Name="Legend1">
                                                                    </asp:Legend>
                                                                </Legends>
                                                                <Titles>
                                                                    <asp:Title Name="Title1" Text="��ֵ����ͼ">
                                                                    </asp:Title>
                                                                </Titles>
                                                            </asp:Chart>--%>
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 40%; top: 40%;">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="Images/Processing.gif" alt="���Ժ򣬴�����..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
