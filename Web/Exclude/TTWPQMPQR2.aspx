<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWPQMPQR2.aspx.cs" Inherits="TTWPQMPQR2" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PQR-2����</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
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
                    <table cellpadding="0" cellspacing="0" width="100%" align="center" class="bian">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,PQR-2GuanLi%>"></asp:Label> </td>
                                                    <td width="5">
                                                        <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding-top: 5px;">
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                    <tr>
                                        <td  style="width: 150px" class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,HanJieGongYiPingDing%>"></asp:Label>  
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:DropDownList ID="DL_WeldProCode" runat="server" DataTextField="Code" DataValueField="Code">
                                            </asp:DropDownList>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LaShenShiYanBaoGao%>"></asp:Label>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TensileTestReportNo" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LaShenShiYangkuanDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TenSpeWidth" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LaShenShiYangKuanDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TenSpeThickness" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,WaiGuanJianChajieLun%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_AppInsConclusion" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,WaiGuanJianChaPingDingJieGuo%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_AppInsEvaResults" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,LaShenHengJieMianJi%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TenSpeArea" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,LaShenDuanLieZhaiHe%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TenSpeBreLoad" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,LaShenJianQieQiangDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_TenSpeSheStrength" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,WanQuShiYanBaoGaoBianHao%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_BendTestReportNo" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,LaShenDuanLieBuWeiHeTeZheng%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_TenSpePartChara" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,WanQuShiYanJieGuo%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_BendSpeResults" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,WanQuShiYangLeiXing%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_BendSpeType" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,WanQuShiYangHouDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_BendSpeThickness" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,WanQuShiYangZhiJing%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_BendSpeDiameter" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,WanQuSHiYangJiaoDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_BendSpeAngle" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,DanJiShiYanBaoGaoBianHao%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactTestReportNo" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,DanJiShiYangChiCun%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampSize" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,DanJiQueKouLeiXing%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampType" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,DanJiQueKouWeiZhi%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampPosition" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DanJiShiYanWenDu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampTemperature" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,DanJiXiShouGong%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampFunction" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,DanJiCheXiangPengZhangLiang%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_ImpactSampExpAmount" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,QiTaShiYanMingChen%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_OtherTestName" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DanJiBeiZhu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_ImpactSampRemark" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,QiTaShiYanBeiZhu%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                            <asp:TextBox ID="TB_OtherExpRemark" runat="server" CssClass="shuru" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,QiTaShiYanBaoGaoBianHao%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_OtherTestReportNo" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,QiTaShiYangChiCun%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_OtherTestSize" runat="server" CssClass="shuru"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                        <td  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                        <td  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                        <td  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td  style="width: 150px;" class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="7">
                                            <asp:Button ID="BT_Add" runat="server" OnClick="BT_Add_Click" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" />&nbsp;
                                        <asp:Button ID="BT_Update" runat="server" OnClick="BT_Update_Click" CssClass="inpu"
                                            Text="<%$ Resources:lang,BaoCun%>" Enabled="False" />
                                            &nbsp;
                                        <asp:Button ID="BT_Delete" runat="server" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" CssClass="inpu"
                                            Text="<%$ Resources:lang,ShanChu%>" Enabled="False" />
                                            <asp:Label ID="lbl_ID" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="90%">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,PQR2XinXi%>"></asp:Label><asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="95%">
                                    <tr>
                                        <td align="center"  class="formItemBgStyleForAlignLeft">&nbsp;&nbsp;&nbsp; <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,PQRErGuanLiLieBiao%>"></asp:Label>��</td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="formItemBgStyleForAlignLeft">
                                            <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="8%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,HanJieGongYiPingDing%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,LaShenShiYanBianHao%>"></asp:Label> </strong></td>
                                                                <td width="15%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,DuanLieBuWeiHeTeZheng%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,WanQuShiYanBianHao%>"></asp:Label> </strong></td>
                                                                <td width="15%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,WanQuShiYanJieGuo%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,DanJiShiYanBianHao%>"></asp:Label> </strong></td>
                                                                <td width="12%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,DanJiBeiZhu%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,QiTaShiYanBianHao%>"></asp:Label> </strong></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px"
                                                OnItemCommand="DataGrid2_ItemCommand" Width="98%" CellPadding="4" ForeColor="#333333"
                                                GridLines="None" ShowHeader="false" AllowPaging="true" PageSize="10" OnPageIndexChanged="DataGrid2_PageIndexChanged">
                                                
                                                <ItemStyle CssClass="itemStyle" />
                                                <HeaderStyle HorizontalAlign="Center" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="���">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                        <ItemTemplate>
                                                            <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="WeldProCode" HeaderText="���ӹ�������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="TensileTestReportNo" HeaderText="����������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="TenSpePartChara" HeaderText="���Ѳ�λ������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="BendTestReportNo" HeaderText="����������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="BendSpeResults" HeaderText="����������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ImpactTestReportNo" HeaderText="���������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ImpactSampRemark" HeaderText="�����ע">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="OtherTestReportNo" HeaderText="����������">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditItemStyle BackColor="#2461BF" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
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
