<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTBMBidPlan.aspx.cs" Inherits="TTBMBidPlan" %>

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
    <script type="text/javascript">
        function SelectAllDiv(item) {
            var thecheckbox = document.getElementsByTagName("input");
            for (i = 0; i < thecheckbox.length; i++) {
                if (thecheckbox[i].type == "checkbox") {
                    if (thecheckbox[i].id == "Checkbox1") {
                        thecheckbox[i].checked = item.checked;
                    }
                    else {
                        if (thecheckbox[i].id == "Checkbox2") {
                        }
                        else {
                            thecheckbox[i].checked = item.checked;
                        }
                    }
                }
            }
        }
        function ReSelectDiv(item) {
            var thecheckbox = document.getElementsByTagName("input");
            for (i = 0; i < thecheckbox.length; i++) {
                if (thecheckbox[i].type == "checkbox") {
                    if (thecheckbox[i].id == "Checkbox2") {
                        thecheckbox[i].checked = item.checked;
                    }
                    else {
                        if (thecheckbox[i].id == "Checkbox1") {
                        }
                        else {
                            if (thecheckbox[i].checked) {
                                thecheckbox[i].checked = false;
                            }
                            else {
                                thecheckbox[i].checked = true;
                            }
                        }
                    }
                }
            }
        }
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
                                <td height="31" class="page_topbj" colspan="2">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZhaoBiaoFangAn%>"></asp:Label>
                                                        </td>
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
                                <td align="left" style="padding-top: 5px;">
                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="75%">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ZhaoBiaoXinXi %>"></asp:Label>��<asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>

                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShenQingXinXi %>"></asp:Label>��<asp:TextBox ID="TextBox2" runat="server" Width="120px"></asp:TextBox>

                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZhaoBiaoRiQi %>"></asp:Label>��<asp:TextBox ID="TextBox3" runat="server" Width="90px"></asp:TextBox><cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox3" Enabled="True"></cc1:CalendarExtender>
                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,Zhi %>"></asp:Label><asp:TextBox ID="TextBox4" runat="server" Width="90px"></asp:TextBox><cc1:CalendarExtender ID="CalendarExtender6" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox4" Enabled="True"></cc1:CalendarExtender>

                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun %>" /></td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="padding: 5px 5px 5px 5px; width: 100%">
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
                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td width="5%" align="left"><strong>
                                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                                    <td width="13%" align="left"><strong>
                                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>
                                                                    <td width="13%" align="left"><strong>
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShenQingJiHua %>"></asp:Label></strong></td>
                                                                    <td width="10%" align="left"><strong>
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,KaiShiRiQi %>"></asp:Label></strong></td>
                                                                    <td width="10%" align="left"><strong>
                                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,JieShuRiQi %>"></asp:Label></strong></td>
                                                                    <td width="12%" align="left"><strong>
                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,FangAnWenBen %>"></asp:Label></strong></td>
                                                                    <td align="left" width="5%"><strong></strong></td>
                                                                    <td width="7%" align="left"><strong>
                                                                        <asp:Label ID="Label29" runat="server" Text="Document"></asp:Label></strong></td>
                                                                    <td align="left"><strong>
                                                                        <asp:Label ID="Label48" runat="server" Text="Ͷ���ļ�ģ��"></asp:Label></strong></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px" PageSize="25"
                                                    OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None" ShowHeader="False" AllowPaging="True" OnPageIndexChanged="DataGrid2_PageIndexChanged">
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
                                                        <asp:TemplateColumn HeaderText="Workflow">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LBT_Workflow" CommandName="Workflow" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Assign.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Name" HeaderText="Name">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="13%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PurchaseAppName" HeaderText="�ɹ���������">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="13%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BidType" HeaderText="�б�����" Visible="False">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BidWay" HeaderText="�б귽ʽ" Visible="False">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EnterPer" HeaderText="������" Visible="False">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BidStartDate" HeaderText="��ʼ����" DataFormatString="{0:yyyy-MM-dd}">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BidEndDate" HeaderText="��������" DataFormatString="{0:yyyy-MM-dd}">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BidRemark" HeaderText="�����ı�">
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="��������">
                                                            <ItemTemplate><a href='TTRelatedDIYWorkflowForm.aspx?RelatedType=BMBidType&BMBidType=<%# DataBinder.Eval(Container.DataItem,"BidType").ToString().Trim() %>&RelatedID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">ѯ�� </a></ItemTemplate>
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="�б��ļ�">
                                                            <ItemTemplate><a href='TTBMBidFile.aspx?BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">�б��ļ�</a></ItemTemplate>
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="ģ��">
                                                            <ItemTemplate>
                                                                <a href='TTBMBidTemplateFile.aspx?SupplierType=Enginerring&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                                <a href='TTBMBidTemplateFile.aspx?SupplierType=Internation&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                                <a href='TTBMBidTemplateFile.aspx?SupplierType=Retail&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                                <a href='TTBMBidTemplateFile.aspx?SupplierType=Store&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">�ŵ�</a>

                                                            </ItemTemplate>
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                </asp:DataGrid></td>
                                        </tr>
                                    </table>

                                    <br />
                                    <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </td>

                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;" Width="500px">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="BT_ActiveNo" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Fou%>" OnClick="BT_ActiveNo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label112" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px; text-align: left;">

                            <table>
                                <tr>
                                    <td style="width: 75%;">
                                        <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                            <tr>
                                                <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label47" runat="server" Text="�а�������"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                                    <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                                    <asp:CheckBox ID="CB_IsEnginerringSupplier" Text="����" runat="server" />
                                                    &nbsp;
                                                    &nbsp;
                                                    <asp:CheckBox ID="CB_IsInternationSupplier" Text="����" runat="server" />
                                                    &nbsp;
                                                    &nbsp;
                                                    <asp:CheckBox ID="CB_IsRetailSupplier" Text="����" runat="server" />
                                                    &nbsp;
                                                    &nbsp;
                                                    <asp:CheckBox ID="CB_IsStoreSupplier" Enabled="false" Text="�ŵ�" runat="server" />
                                                </td>
                                                <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label2" runat="server" Text="�б��޼�"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_BidLimitedPrice" runat="server" Width="120px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhaoBiaoFangShi%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_BidWay" runat="server">
                                                        <asp:ListItem Value="InvitationToBid" Text="<%$ Resources:lang,YaoQingZhaoBiao%>" />
                                                        <asp:ListItem Value="PublicTender" Text="<%$ Resources:lang,GongKaiZhaoBiao%>" />
                                                        <asp:ListItem Value="NegotiatedBidding" Text="<%$ Resources:lang,YiBiao%>" />
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhaoBiaoLeiXing%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                                    <asp:DropDownList ID="ddl_BidType" runat="server" DataValueField="Type" DataTextField="Type" AutoPostBack="True" OnSelectedIndexChanged="ddl_BidType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZhaoBiaoKaiShiRiQi%>"></asp:Label></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_BidStartDate" runat="server" ReadOnly="false" CssClass="shuru"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_BidStartDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZhaoBiaoJieShuRiQi%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                                    <asp:TextBox ID="DLC_BidEndDate" runat="server" ReadOnly="false" CssClass="shuru"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="DLC_BidEndDate_CalendarExtender" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_BidEndDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhaoBiaoDiDian%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                                    <asp:TextBox ID="TB_BidAddress" runat="server" Width="90%" CssClass="shuru"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,PingBiaoChengYuan%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3" style="height: 30px">
                                                    <asp:TextBox ID="TB_UserCodeList" runat="server" Width="80%" Enabled="False"></asp:TextBox>
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,QingXuanZeYouBianDeZhuanJia%>"></asp:Label></td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,FangAnWenBen%>"></asp:Label>�� </td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px" colspan="3">
                                                    <asp:TextBox ID="TB_BidRemark" runat="server" CssClass="shuru" Height="40px" TextMode="MultiLine" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,FenBaoShenQing%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="1">
                                                    <asp:DropDownList ID="DL_PurchaseAppID" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="DL_PurchaseAppID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,FenBaoBianMa%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_BMPurchaseApplicationCode" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,FangAnMingCheng%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="1">
                                                    <asp:TextBox ID="TB_Name" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BianZhiRen%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_EnterPer" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="color: #000000">
                                                <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BianZhiBuMen%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="1">
                                                    <asp:TextBox ID="TB_EnterDepart" runat="server" CssClass="shuru" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,BianZhiRiQi%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_EnterDate" runat="server" CssClass="shuru" ReadOnly="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_EnterDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                                        <tr>
                                                            <td align="left">&nbsp;&nbsp;&nbsp;
                                                                 <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ZhuanJia%>"></asp:Label>��<asp:TextBox ID="txt_SupplierInfo" runat="server" Width="100px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="Button1" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="Button1_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                                        <tr>
                                                            <td  class="formItemBgStyleForAlignLeft">&nbsp;&nbsp;<asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZhuanJiaLieBiao%>"></asp:Label>��</td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <input id="Checkbox1" type="checkbox" onclick="SelectAllDiv(this);" />
                                                                <%--<a href="javascript:void(0);" title="ȫѡ" onclick="SelectAllDiv();" class="auto-style1">--%><asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,QuanXuan%>"></asp:Label><%--</a>--%>|
                                                                <input id="Checkbox2" type="checkbox" onclick="ReSelectDiv(this);" />
                                                                <%--<a href="javascript:void(0);" title="��ѡ" onclick="ReSelectDiv();" class="auto-style1"> --%><asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,FanXuan%>"></asp:Label><%--</a>--%></td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Button ID="btnConfirmUp" runat="server" CssClass="inpu" OnClick="btnConfirm_Click" Text="<%$ Resources:lang,QueRen%>" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="10%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,XuanZe%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="25%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,ZhuanJia%>"></asp:Label></strong>
                                                                                    </td>
                                                                                    <td width="25%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong></td>
                                                                                    <td align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ZhuanJiaXinXi%>"></asp:Label></strong></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false">
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn HeaderText="ѡ��">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="cbSelect" />
                                                                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID")%>' />
                                                                                <asp:HiddenField runat="server" ID="hfName" Value='<%# Eval("Name")%>' />
                                                                                <asp:HiddenField runat="server" ID="hfExpertCode" Value='<%# Eval("ExpertCode")%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                                Horizontalalign="left" />
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="Name" HeaderText="Expert">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="25%" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                                Horizontalalign="left" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                                Horizontalalign="left" />
                                                                        </asp:BoundColumn>
                                                                        <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTBMExpertInfoView.aspx?ExpertID={0}" DataTextField="Name" HeaderText="Expert" Target="_blank">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
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
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lbl_UserCodeList" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindowWorkflow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label50" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px; text-align: left;">
                            <table width="98%" cellpadding="4" cellspacing="0">
                                <tr>
                                    <td align="right" style="width: 100px; height: 27px">
                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,LiuChengMuBan%>"></asp:Label>�� </td>
                                    <td align="left" style="width: 650px; height: 27px">
                                        <asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                            Width="228px">
                                        </asp:DropDownList><asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��<asp:DropDownList ID="DL_WFType" runat="server">
                                            <asp:ListItem Value="TenderManagement" Text="<%$ Resources:lang,ZhaoBiaoGuanLi%>" />
                                        </asp:DropDownList><asp:HyperLink
                                            ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx" Target="_blank">
                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                        </asp:HyperLink><asp:Button
                                            ID="BT_Reflash" runat="server" OnClick="BT_Reflash_Click" Text="<%$ Resources:lang,ShuaXin%>" CssClass="inpu" /></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 100px; height: 27px"></td>
                                    <td align="left" style="width: 650px; height: 27px"><span style="font-size: 10pt"></span>
                                        <asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox
                                            ID="CB_Mail" runat="server" Font-Size="10pt" Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt"><asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXinXi%>"></asp:Label></span><asp:Button ID="BT_SubmitApply" runat="server" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" /><cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True"
                                                TargetControlID="BT_SubmitApply" PopupControlID="Panel2" CancelControlID="IMBT_Close"
                                                BackgroundCssClass="modalBackground" DynamicServicePath="">
                                            </cc1:ModalPopupExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 22px; text-align: left;">
                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>�� </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="10%" align="left"><strong>
                                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>&#160;&#160;</td>
                                                            <td width="50%" align="left"><strong>
                                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>&#160;&#160;</td>
                                                            <td width="20%" align="left"><strong>
                                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>&#160;&#160;</td>
                                                            <td width="10%" align="left"><strong>
                                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>&#160;&#160;</td>
                                                            <td width="10%" align="left"><strong></strong></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px"
                                            ShowHeader="False" PageSize="5" Width="100%" CellPadding="4" ForeColor="#333333"
                                            GridLines="None">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            <ItemStyle CssClass="itemStyle" />
                                            <Columns>
                                                <asp:BoundColumn DataField="WLID" HeaderText="Number">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTMyWorkDetailMain.aspx?WLID={0}"
                                                    DataTextField="WLName" HeaderText="Workflow" Target="_blank">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="50%" />
                                                </asp:HyperLinkColumn>
                                                <asp:BoundColumn DataField="CreateTime" HeaderText="����ʱ��">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Status">
                                                    <ItemTemplate></ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                            Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder" alt="" /></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid></td>
                                </tr>
                            </table>

                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
