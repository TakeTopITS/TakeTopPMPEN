<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWPQMRTTable.aspx.cs" Inherits="TTWPQMRTTable" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>RT管理</title>
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,RTGuanLi%>"></asp:Label></td>
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
                                            <asp:DropDownList ID="DL_WeldProCode" runat="server" DataTextField="Code" DataValueField="Code" AutoPostBack="True" OnSelectedIndexChanged="DL_WeldProCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lbl_ID" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,HeGeJiBie%>"></asp:Label>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_QualifiedLevel" runat="server" CssClass="shuru" Width="120px"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,WeiTuoRiQi%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="DLC_RTCommissionedDate" runat="server" CssClass="shuru" Width="120px" ReadOnly="false"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_RTCommissionedDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,WuShongJianCheLeiBie%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_NondestructTestCategory" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,TanShangBiLi%>"></asp:Label></td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_InspectionProportion" runat="server" CssClass="shuru" Width="120px"></asp:TextBox>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShiJianShuLiang%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="NB_NumberSpecimens" runat="server" CssClass="shuru" Width="120px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" style="width: 150px">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,PingDIngBiaoZhun%>"></asp:Label> </td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                            <asp:TextBox ID="TB_RTEvaluationCriteria" runat="server" CssClass="shuru" Width="95%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="width: 150px;" class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                            <asp:Button ID="BT_Add" runat="server" OnClick="BT_Add_Click" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" />&nbsp;
                                        <asp:Button ID="BT_Update" runat="server" OnClick="BT_Update_Click" CssClass="inpu"
                                            Text="<%$ Resources:lang,BaoCun%>" Enabled="False" />
                                            &nbsp;
                                        <asp:Button ID="BT_Delete" runat="server" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" CssClass="inpu"
                                            Text="<%$ Resources:lang,ShanChu%>" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="90%">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,RTXinXi%>"></asp:Label><asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                            &nbsp;&nbsp; <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label><asp:TextBox ID="TextBox2" runat="server" Width="120px" ReadOnly="false"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox2">
                                            </cc1:CalendarExtender>
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,Zhi%>"></asp:Label> <asp:TextBox ID="TextBox3" runat="server" Width="120px" ReadOnly="false"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox3">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="2" cellspacing="0" class="formBgStyle" width="95%">
                                    <tr>
                                        <td align="center"  class="formItemBgStyleForAlignLeft">&nbsp;&nbsp;&nbsp; <asp:Label ID="Label12" runat="server" ></asp:Label>RT列表：</td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="formItemBgStyleForAlignLeft">
                                            <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label></strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong>
                                                                </td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong></td>
                                                                <td width="10%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label> </strong></td>
                                                                <td width="30%" align="center">
                                                                    <strong>
                                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,FaSongRen%>"></asp:Label>asp:Label> </strong></td>
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
                                                    <asp:TemplateColumn HeaderText="编号">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        <ItemTemplate>
                                                            <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="WeldProCode" HeaderText="工艺评定编码">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="QualifiedLevel" HeaderText="合格级别">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RTCommissionedDate" HeaderText="委托日期" DataFormatString="{0:yyyy-MM-dd}">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NondestructTestCategory" HeaderText="无损检测类别">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="InspectionProportion" HeaderText="探伤比例">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NumberSpecimens" HeaderText="试件数量">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                            HorizontalAlign="Center" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RTEvaluationCriteria" HeaderText="RT评定标准">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
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
