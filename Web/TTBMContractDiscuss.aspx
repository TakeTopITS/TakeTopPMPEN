<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTBMContractDiscuss.aspx.cs" Inherits="TTBMContractDiscuss" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

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
                        <table cellpadding="0" cellspacing="0" width="100%" align="left" class="bian">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HeTongTongZhi%>"></asp:Label></td>
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
                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="75%">
                                        <tr>
                                               <td align="center">
                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,HeTongXinXi%>"></asp:Label>：<asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                        
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ChengBaoShangXinXi%>"></asp:Label>：<asp:TextBox ID="TextBox2" runat="server" Width="120px"></asp:TextBox>
                                          
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,TongZhiRiQi%>"></asp:Label>：<asp:TextBox ID="TextBox3" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox3">
                                                </cc1:CalendarExtender>
                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,Zhi%>"></asp:Label><asp:TextBox ID="TextBox4" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox4">
                                                </cc1:CalendarExtender>
                                        
                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                        <tr>
                                            <td class="formItemBgStyleForAlignRight" style="padding: 5px 5px 5px 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,BianJi %>" />
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ChengBaoShang%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,HeTongJia%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,BianZhiRen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,TongZhiRiQi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="17%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,YaoDianJiYao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,HeTongWenJian%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px" 
                                                    OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None" ShowHeader="false" AllowPaging="true" PageSize="25" OnPageIndexChanged="DataGrid2_PageIndexChanged">

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
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
                                                        <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SupplierName" HeaderText="Supplier">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ContractPrice" HeaderText="合同价">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="EnterPer" HeaderText="编制人">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EnterDate" HeaderText="编制日期" DataFormatString="{0:yyyy-MM-dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PointSummary" HeaderText="要点纪要">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="17%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="DiscussFilePath" DataNavigateUrlFormatString="{0}" DataTextField="DiscussFileName" HeaderText="洽谈文件" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:HyperLinkColumn>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label112" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px; text-align: left;">

                            <table cellpadding="2" cellspacing="0" class="formBgStyle" width="900px">
                                <tr>
                                    <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                    </td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                        <asp:TextBox ID="TB_Name" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td  style="width: 150px; height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ChengBaoShang%>"></asp:Label>：
                                    </td>
                                    <td  style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_SupplierCode" runat="server" DataTextField="Name" DataValueField="Code">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,HeTongJia%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox ID="NB_ContractPrice" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" CssClass="shuru">0.00</NickLee:NumberBox></td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,BianZhiRen%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_EnterPer" runat="server" CssClass="shuru"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BianZhiBuMen%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_EnterUnit" runat="server" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,TongZhiRiQi%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="DLC_EnterDate" runat="server" ReadOnly="false" CssClass="shuru"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_EnterDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:DropDownList ID="DL_Status" runat="server" Enabled="False">
                                            <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                            <asp:ListItem Value="Qualified" Text="<%$ Resources:lang,HeGe%>" />
                                            <asp:ListItem Value="Unqualified" Text="<%$ Resources:lang,BuHeGe%>" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YaoDianJiYao%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                        <asp:TextBox ID="TB_PointSummary" runat="server" Width="90%" Height="35px" TextMode="MultiLine" CssClass="shuru"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,HeTongWenJian%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div>
                                                    <Upload:InputFile ID="AttachFile" runat="server" Width="420px" />
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,WJZNSYGDGQXDBYSHZSC%>"></asp:Label><br />
                                                    <div id="ProgressBar">
                                                        <Upload:ProgressBar ID="ProgressBar1" runat='server' Width="500px" Height="100px">
                                                        </Upload:ProgressBar>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BT_New" />
                                           
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr style="color: #000000; display: none;">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShenPiRen%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_Reviewer" runat="server" Enabled="False" CssClass="shuru"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShenPiRiQi%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="DLC_ReviewDate" runat="server" ReadOnly="false" Enabled="False" CssClass="shuru"></asp:TextBox>
                                        <cc1:CalendarExtender ID="DLC_ReviewDate_CalendarExtender" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_ReviewDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="color: #000000; display: none;">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShenPiNeiRong%>"></asp:Label>： </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px" colspan="3">
                                        <asp:TextBox ID="TB_ReviewResult" runat="server" CssClass="shuru" Height="40px" TextMode="MultiLine" Width="90%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                            <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_ConstractId" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
