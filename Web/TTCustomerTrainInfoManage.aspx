<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCustomerTrainInfoManage.aspx.cs" Inherits="TTCustomerTrainInfoManage" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>��ѵ����</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }
           
        });
    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="1500px" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,PeiXunGuanLi%>"></asp:Label></td>
                                                        <td width="5">
                                                            <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td></td>
                                            <td width="10%">
                                                <asp:HyperLink ID="HL_CustomerTrainInfoEdit" NavigateUrl ="TTCustomerTrainInfoEdit.aspx" runat="server" Text="<%$ Resources:lang,ChengYuanPeiXunXinXi%>" Target="_blank"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td style="padding-left: 5px;">
                                    <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GeRenPeiXunXinXi%>"></asp:Label><asp:Label ID="lbl_sql1" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="3%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JiNengDengJi%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhiYeJiNengZhengShuBianHao%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,JianDingGongZhong%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,FaZhengRiQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,AnKongYouXiaoQi%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,AnKongZhengShuBianHao%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,SheWaiYingYuKaoHe%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PeiXunXiangGuanXinXi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProfessionalSkillLevel" HeaderText="���ܵȼ�">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProfessionSkillNumber" HeaderText="ְҵ����֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ValidityType" HeaderText="ǩ������">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ReleaseTime" HeaderText="CertificateIssuanceDate" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AnnValidTime" HeaderText="AntiTerrorismValidityPeriod">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AnnCertificateNo" HeaderText="����֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EnglishRiew" HeaderText="����Ӣ�￼��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingInfo" HeaderText="��ѵ�����Ϣ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GeRenPeiXunJiLu%>"></asp:Label><asp:Label ID="lbl_sql6" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,PeiXunXiangMu%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,PeiXunYiJu%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,JuBanDanWei%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,PeiXunDiDian%>"></asp:Label></strong></td>
                                                                    <td width="20%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,PeiXunNeiRong%>"></asp:Label></strong></td>
                                                                    <td width="15%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,PeiXunRiQi%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid6" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid6_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingProject" HeaderText="TrainingProgram">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingAccord" HeaderText="��ѵ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingUnit" HeaderText="�ٰ쵥λ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingAddress" HeaderText="��ѵ�ص�">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingContent" HeaderText="��ѵ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TrainingTime" HeaderText="TrainingDate" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="15%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,GRTZZYPXXX%>"></asp:Label><asp:Label ID="lbl_sql2" runat="server" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="3%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,YongGongLeiBie%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ZuoYeLeiBie%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ZhunCaoXiangMu%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,QuZhengRiQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,FuShenRiQi%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ZhengShuBianHao%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid2_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="WorkType" HeaderText="LaborCategory">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeOpeType" HeaderText="��ҵ���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeOpeProject" HeaderText="׼����Ŀ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeOpeStartTime" HeaderText="ȡ֤����" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeOpeReviewTime" HeaderText="��������" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeOpeNumber" HeaderText="֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,GRTZSBZYPXXX%>"></asp:Label><asp:Label ID="lbl_sql3" runat="server" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="3%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,YongGongLeiBie%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,SheBeiLeiBie%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ZhunCaoXiangMu%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,QuZhengRiQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,FuShenRiQi%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,ZhengShuBianHao%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid3" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid3_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="WorkType" HeaderText="LaborCategory">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeEquType" HeaderText="�豸���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeEquProject" HeaderText="׼����Ŀ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeEquStartTime" HeaderText="ȡ֤����" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeEquReviewTime" HeaderText="��������" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SpeEquNumber" HeaderText="֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,GRHJCZXMPXXX%>"></asp:Label><asp:Label ID="lbl_sql4" runat="server" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="3%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,ZhengJianBianMa%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,HanGongGangYin%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,ChiZhengXiangMu%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,YouXiaoQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid4_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CertificateNo" HeaderText="֤������">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="WelderSeal" HeaderText="WelderStamp">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="HolderProject" HeaderText="CertifiedProjects">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ValidTime" HeaderText="��Ч��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="page_topbj">
                                                <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,ZYGLGWRYPXXX%>"></asp:Label><asp:Label ID="lbl_sql5" runat="server" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="4%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong></td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong></td>
                                                                    <td width="3%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,ChuShengRiQi%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,YongGongLeiBie%>"></asp:Label></strong></td>
                                                                    <td width="9%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,GangWeiZhiWu%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,ZhengShuBianHao%>"></asp:Label></strong></td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,FaZhengJiGuan%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,QuZhengRiQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,FuShenRiQi%>"></asp:Label></strong></td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label75" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid5" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                    OnPageIndexChanged="DataGrid5_PageIndexChanged" PageSize="15" Width="100%" ShowHeader="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BirthDay" HeaderText="DateOfBirth" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="WorkType" HeaderText="LaborCategory">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Job" HeaderText="Position">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CertificateNo" HeaderText="֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CertificateOffice" HeaderText="��֤����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CertificateTime" HeaderText="ȡ֤����" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CertificateReviewTime" HeaderText="��������" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                </Triggers>
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
