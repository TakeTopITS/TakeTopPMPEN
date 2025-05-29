<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTBMSupplierInfo.aspx.cs" Inherits="TTBMSupplierInfo" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload" TagPrefix="Upload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>
    <script type="text/javascript">

        var disPostion = 0;

        function SaveScroll() {
            //disPostion = AssetListDivID.scrollTop;
        }

        function RestoreScroll() {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            //AssetListDivID.scrollTop = disPostion;
        }
    </script>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChengBaoShangDengJi%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label ID="LB_SupplierInfoID" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="padding-top: 5px;">
                                    <table cellpadding="0" cellspacing="0" class="formBgStyle" width="75%">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,QiYeXinXi%>"></asp:Label>��<asp:TextBox ID="txt_SupplierInfo" runat="server" Width="100px"></asp:TextBox>
                                              
                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ChuangJianRiQi%>"></asp:Label>��<asp:TextBox ID="TextBox2" runat="server" ReadOnly="false" Width="80px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox2">
                                                </cc1:CalendarExtender>
                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,Zhi%>"></asp:Label><asp:TextBox ID="TextBox3" runat="server" ReadOnly="false" Width="80px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="yyyy-MM-dd" TargetControlID="TextBox3">
                                                </cc1:CalendarExtender>
                                               
                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Query_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="formItemBgStyleForAlignRight" style="padding: 5px 5px 5px 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label114" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label115" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,QiYeBianMa%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="11%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,QiYeMingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="16%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,GongHuoJingYingFanWei%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,QiYeZiZhiFanWei%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,TuiJianDanWei%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,QiYeDiZhi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <%--    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,MoBan%>"></asp:Label></strong>
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid1_ItemCommand" PageSize="25"
                                                    Width="100%" AllowPaging="True" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                    ShowHeader="false" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Code" HeaderText="��ҵ����">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Name" HeaderText="��ҵ����">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="11%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="SupplyScope" HeaderText="��������Ӫ��Χ">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Qualification" HeaderText="��ҵ���ʷ�Χ">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RecommendedUnits" HeaderText="�Ƽ���λ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Address" HeaderText="��ҵ��ַ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <%--  <asp:TemplateColumn HeaderText="ģ��">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            <ItemTemplate>

                                                                <a href='TTBMBidTemplateFileView.aspx?SupplierType=Enginerring&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                                <a href='TTBMBidTemplateFileView.aspx?SupplierType=Internation&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                                <a href='TTBMBidTemplateFileView.aspx?SupplierType=Retail&BidPlanID=<%# DataBinder.Eval(Container.DataItem,"ID") %>' target="_blank">����</a>
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True"
                                                                Horizontalalign="left" />
                                                        </asp:TemplateColumn>--%>
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
                    </div>



                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label112" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px; text-align: left;">
                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%">
                                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:lang,XiangXiXinXi%>">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,XiangXiXinXi%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" style="width: 100%; margin-top: 5px">

                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    
                                              
                                                    <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,DaLei%>"></asp:Label>��

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_SupplierBigType" DataValueField="Type" DataTextField="Type" AutoPostBack="True" runat="server" OnSelectedIndexChanged="DL_SupplierBigType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft"> <asp:Label ID="Label120" runat="server" Text="<%$ Resources:lang,XiaoLei%>"></asp:Label>��</td>
                                                <td  colspan="3" class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_SupplierSmallType" Width="70%" runat="server" CssClass="shuru"></asp:TextBox>
                                                    <asp:DropDownList ID="DL_SupplierSmallType" DataValueField="Type" DataTextField="Type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_SupplierSmallType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,ChengBaoLeiXing%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                                    <asp:CheckBox ID="CB_IsEnginerringSupplier" Text="<%$ Resources:lang,GongCheng%>" runat="server" />
                                                    &nbsp; &nbsp;
                                                    <asp:CheckBox ID="CB_IsInternationSupplier" Text="<%$ Resources:lang,GuoJi%>" runat="server" />
                                                    &nbsp; &nbsp;
                                                    <asp:CheckBox ID="CB_IsRetailSupplier" Text="<%$ Resources:lang,LingShou%>" runat="server" />
                                                    &nbsp; &nbsp;
                                                    <asp:CheckBox ID="CB_IsStoreSupplier" Text="<%$ Resources:lang,MenDian%>" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,QiYeBianMa %>"></asp:Label>�� </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Code" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,QiYeMingCheng %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Name" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,QiYeJianCheng %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_CompanyFor" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,QiYeXingZhi %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_CompanyType" runat="server">
                                                        <asp:ListItem Value="State-ownedEnterprise" />
                                                        <asp:ListItem Value="PublicInstitution" />
                                                        <asp:ListItem Value="PrivateEnterprise" />
                                                        <asp:ListItem Value="Foreign(JointVenture)" />
                                                        <asp:ListItem Value="ShareholdingSystem" />
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>"/>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LianXiDianHua %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_PhoneNum" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ChuanZhen %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Fax" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YouZhengBianMa %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ZipCode" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,DianZiYouXiang %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Email" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,FaDingDaiBiaoRen %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_LegalRepresentative" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,FaDingDaiBiaoRenJiShuZhiCheng %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_TechnicalTitles" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,FaDingDaiBiaoRenDianHua %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_LegalTel" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ChengLiShiJian %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_SetUpTime" runat="server" CssClass="shuru"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="DLC_SetUpTime_CalendarExtender" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_SetUpTime" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,JiShuFuZeRen %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_TechnicalDirector" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,JiShuFuZeRenJiShuZhiCheng %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_TechnicalTitles_T" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiShuFuZeRenDianHua %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_TechnicalTel" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YuanGongZongRenShu %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_EmployeesNum" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZiZhiZhengShu %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_QualificationCertificate" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,YingYeZhiZhao %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_BusinessLicense" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XiangMuJingLiRenShu %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_PMNumber" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,GaoJiZhiChengRenYuan %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_STNumber" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ZhuCeZiJin %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_RegisteredCapital" runat="server" CssClass="shuru"></asp:TextBox>
                                                    Ԫ</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,KaiHuYinHang %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Bank" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhongJiZhiChengRenYuan %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ITNumber" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ChuJiZhiChengRenYuan %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_PTNumber" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShuiHao %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_EinNo" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,KaiHuZhangHao %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_BankNo" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,JiGongRenYuan %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_MNumber" runat="server" CssClass="shuru">0</asp:TextBox>
                                                    ��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" Enabled="False">
                                                        <asp:ListItem Value="New" />
                                                        <asp:ListItem Value="Qualified" />
                                                        <asp:ListItem Value="Unqualified" />
                                                        <asp:ListItem Value="Archived" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" rowspan="2" style="text-align: left;">
                                                    <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,GHJYF %>"></asp:Label><br />
                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,WYYZZ %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3" rowspan="2">
                                                    <asp:TextBox ID="TB_SupplyScope" runat="server" CssClass="shuru" Height="45px" TextMode="MultiLine" Width="90%"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,FenBaoZhuanYe %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:TextBox ID="TB_SubcontractProfessional" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ZhuCeDiZhi %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:TextBox ID="TB_Address" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" rowspan="2" style="text-align: left;">
                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,QYZZF %>"></asp:Label><br />
                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,WZZZS %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3" rowspan="2">
                                                    <asp:TextBox ID="TB_Qualification" runat="server" CssClass="shuru" Height="45px" TextMode="MultiLine" Width="90%"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,QiYeWangZhi %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:TextBox ID="TB_WebUrl" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,TuiJianDanWei %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:TextBox ID="TB_RecommendedUnits" runat="server" CssClass="shuru" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ShangNianDuGongSi %>"></asp:Label><br />
                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,RuWeiBianHao %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="1">
                                                    <asp:TextBox ID="TB_LastFinalistsNumber" runat="server" CssClass="shuru"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,ShiFouZaiZhongShiYouHuoLanZhouShiHuaRuWei %>"></asp:Label>
                                                    ��</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_IsLand" runat="server">
                                                        <asp:ListItem Value="YES" />
                                                        <asp:ListItem Value="NO" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,FuJian %>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="7">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink ID="HL_Accessories" runat="server" Text="<%$ Resources:lang,ChaKanFuJian%>"></asp:HyperLink>
                                                            </td>
                                                            <td style="padding-left: 50px;">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <div>
                                                                            <Upload:InputFile ID="InputFile1" runat="server" Width="450px" />
                                                                            <br />
                                                                            <Upload:ProgressBar ID="ProgressBar2" runat="server" Height="100px" Width="500px">
                                                                            </Upload:ProgressBar>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="BT_UploadAccessories" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                                <asp:Repeater ID="RP_AccessoriesPath" runat="server">
                                                                    <ItemTemplate>
                                                                        <a href='<%# DataBinder.Eval(Container.DataItem,"AccessoriesPath") %>' target="_blank">
                                                                            <%# DataBinder.Eval(Container.DataItem,"Code") %>����
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <asp:Button ID="BT_UploadAccessories" runat="server" Text="<%$ Resources:lang,ShangChuan %>" CssClass="inpu" OnClick="BT_UploadAccessories_Click" />
                                                                <asp:Button ID="BT_DeleteAccessories" Enabled="False" runat="server" Text="<%$ Resources:lang,ShanChuFuJian %>" CssClass="inpu" OnClick="BT_DeleteAccessories_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <%-- <tr>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:Button ID="BT_NewAA" runat="server" OnClick="BT_NewClaim_Click" Text="<%$ Resources:lang,XinJian %>" CssClass="inpu" />&nbsp;
                                                            <asp:Button ID="BT_UpdateAA" runat="server" OnClick="BT_UpdateClaim_Click" Text="<%$ Resources:lang,BaoCun %>" CssClass="inpu" />&nbsp;
                                                            <asp:Button ID="BT_DeleteAA" runat="server" OnClick="BT_DeleteClaim_Click" Text="<%$ Resources:lang,ShanChu %>" CssClass="inpu" />
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                  
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                            </tr>--%>
                                        </table>
                                        <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="�����ļ�">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ZiZhiWenJian%>"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <table width="98%" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="height: 6px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignRight" style="padding: 5px 5px 5px 5px;">
                                                                <asp:Button ID="BT_CreateFile" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_CreateFile_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td width="5%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label118" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                                    </td>
                                                                                    <td width="5%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label119" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                                    </td>
                                                                                    <td align="left" width="10%"><strong>
                                                                                        <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,ZhengShuBianHao %>"></asp:Label></strong></td>
                                                                                    <td align="left" width="20%"><strong>
                                                                                        <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,ZhengShuMingCheng %>"></asp:Label></strong></td>
                                                                                    <td align="left" width="15%"><strong>
                                                                                        <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,FaZhengJiGuan %>"></asp:Label></strong></td>
                                                                                    <td align="left" width="15%"><strong>
                                                                                        <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,FaZhengRiQi %>"></asp:Label></strong></td>
                                                                                    <td align="left"><strong>
                                                                                        <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,FuJianXiaZai %>"></asp:Label></strong></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="30px" OnItemCommand="DataGrid3_ItemCommand" ShowHeader="False" Width="100%">
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
                                                                        <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                                Horizontalalign="left" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="CertificateNum" HeaderText="Number">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                                Horizontalalign="left" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="CertificateName" HeaderText="֤������">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="LicenseAgency" HeaderText="��֤����">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ReleaseTime" HeaderText="CertificateIssuanceDate" DataFormatString="{0:yyyy-MM-dd}">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:HyperLinkColumn DataNavigateUrlField="Attach" DataNavigateUrlFormatString="{0}" DataTextField="CertificateName" HeaderText="��������" Target="_blank">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                        </asp:HyperLinkColumn>
                                                                    </Columns>

                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                </asp:DataGrid>
                                                                <asp:Label ID="lbl_CerID" runat="server" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="�����Ŀ" TabIndex="0">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,XiangGuanXiangMu%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,XiangMuHao%>"></asp:Label>��
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:TextBox ID="TB_ProjectID" runat="server" Width="99%"></asp:TextBox>
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Button ID="BT_RelatedProject" runat="server" CssClass="inpu" OnClick="BT_RelatedProject_Click"
                                                        Text="<%$ Resources:lang,GuanLian%>" />
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                            <tr>

                                                <td width="7">

                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>

                                                <td>

                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                        <tr>

                                                            <td width="8%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="20%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="15%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="15%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="15%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,LiXiangRiQi%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="9%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="10%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="8%" align="left">

                                                                <strong>

                                                                    <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,ShanChu%>"></asp:Label></strong>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td width="6" align="right">

                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>

                                        <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            ShowHeader="False" Height="1px" OnItemCommand="DataGrid4_ItemCommand" Width="100%"
                                            CellPadding="4" ForeColor="#333333" GridLines="None">

                                            <Columns>

                                                <asp:BoundColumn DataField="ProjectID" HeaderText="Number">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="ProjectName" HeaderText="��Ŀ����">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��ʼ����">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="MakeDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                </asp:BoundColumn>

                                                <asp:TemplateColumn HeaderText="Status">

                                                    <ItemTemplate>

                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                    </ItemTemplate>

                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="��ɳ̶�">

                                                    <ItemTemplate>

                                                        <asp:Label ID="LB_FinishPercent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinishPercent")%> '></asp:Label>

                                                        %
                                                    </ItemTemplate>

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:TemplateColumn>

                                                <asp:ButtonColumn CommandName="Delete" Text="&lt;div&gt;&lt;img src=ImagesSkin/icon_del.gif border=0 alt='Deleted' /&gt;&lt;/div&gt;">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                </asp:ButtonColumn>
                                            </Columns>

                                            <ItemStyle CssClass="itemStyle" />

                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                            <EditItemStyle BackColor="#2461BF" />

                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                        </asp:DataGrid>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="VisiblePersonnel" TabIndex="0">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,KeShiRenYuan%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table width="100%" cellpadding="0" cellspacing="0">

                                            <tr>

                                                <td style="width: 220px; border-right: solid 1px #D8D8D8; padding: 5px 0px 0px 5px"
                                                    valign="top" align="left">

                                                    <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                                        ShowLines="True" Width="220px">

                                                        <RootNodeStyle CssClass="rootNode" />

                                                        <NodeStyle CssClass="treeNode" />

                                                        <LeafNodeStyle CssClass="leafNode" />

                                                        <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                    </asp:TreeView>
                                                </td>

                                                <td width="165px" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">

                                                    <table style="width: 165px; height: 53px">

                                                        <tr>

                                                            <td style="width: 165; text-align: center;" valign="top">

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

                                                                                            <asp:Label ID="Label78" runat="server" Text="<%$ Resources:lang,BuMenRenYuan %>"></asp:Label></strong>
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
                                                                    Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                    ShowHeader="False">

                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                    <EditItemStyle BackColor="#2461BF" />

                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                    <ItemStyle CssClass="itemStyle" />

                                                                    <Columns>

                                                                        <asp:TemplateColumn HeaderText="������Ա��">

                                                                            <ItemTemplate>

                                                                                <asp:Button ID="BT_UserCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>'
                                                                                    CssClass="inpu" />

                                                                                <asp:Button ID="BT_UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                                    CssClass="inpu" />
                                                                            </ItemTemplate>

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />

                                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                                                Font-Strikeout="False" Font-Underline="False" />
                                                                        </asp:TemplateColumn>
                                                                    </Columns>

                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td width="500px" align="left" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">

                                                    <asp:Repeater ID="RP_VendorMember" runat="server" OnItemCommand="RP_VendorMember_ItemCommand">
                                                        <ItemTemplate>
                                                            <asp:Button ID="BT_UserName" runat="server" CssClass="inpuRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                Width="70px" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                    <br />

                                                    <br />

                                                    <table cellpadding="0" cellspacing="0" width="100%">

                                                        <tr>

                                                            <td align="left" style="padding-left: 20px; font-weight: bold; height: 24px; color: #394f66; background-image: url('http://localhost:4783/Web/ImagesSkin/titleBG.jpg')">

                                                                <table>

                                                                    <tr>

                                                                        <td>

                                                                            <asp:Label ID="LB_tbActorGroupList" runat="server" Text="<%$ Resources:lang,ActorGroupList %>" />: </td>

                                                                        <td>��</td>

                                                                        <td>

                                                                            <asp:Label ID="Label79" runat="server" Text="<%$ Resources:lang,JueSeZuMingCheng %>"></asp:Label>��</td>

                                                                        <td>

                                                                            <asp:TextBox ID="TB_ActorGroupName" runat="server" Width="99%"></asp:TextBox></td>

                                                                        <td>

                                                                            <asp:Button ID="BT_FindGroup" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun %>" OnClick="BT_FindGroup_Click" />
                                                                        </td>

                                                                        <td>)
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td align="left">

                                                                <table>

                                                                    <tr>

                                                                        <td align="left">

                                                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">

                                                                                <ItemTemplate>

                                                                                    <asp:Button ID="BT_GroupName" runat="server" CssClass="inpuLongRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"GroupName") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>

                                                                        <td style="height: 10px; text-align: left"></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="������ͬ">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,GuanLianHeTong%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                            <tr>

                                                <td align="left">

                                                    <asp:Label ID="Label81" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                            </tr>

                                            <tr>

                                                <td>

                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                        <tr>

                                                            <td width="7">

                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                            <td>

                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                    <tr>

                                                                        <td width="7%" align="left"><strong>

                                                                            <asp:Label ID="Label82" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong> </td>

                                                                        <td width="17%" align="left"><strong>

                                                                            <asp:Label ID="Label83" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong> </td>

                                                                        <td width="5%" align="left"><strong>

                                                                            <asp:Label ID="Label84" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>

                                                                        <td width="5%" align="left"><strong>

                                                                            <asp:Label ID="Label85" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>

                                                                        <td width="8%" align="left"><strong>

                                                                            <asp:Label ID="Label86" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong> </td>

                                                                        <td width="7%" align="left"><strong>

                                                                            <asp:Label ID="Label87" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong> </td>

                                                                        <td width="5%" align="left"><strong>

                                                                            <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong> </td>

                                                                        <td width="10%" align="left"><strong>

                                                                            <asp:Label ID="Label89" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong> </td>

                                                                        <td width="10%" align="left"><strong>

                                                                            <asp:Label ID="Label90" runat="server" Text="<%$ Resources:lang,YiFangDanWei%>"></asp:Label></strong> </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td width="6" align="right">

                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid6" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                        Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                        <Columns>

                                                            <asp:BoundColumn DataField="ConstractCode" HeaderText="ContractCode">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                            </asp:BoundColumn>

                                                            <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                DataTextField="ConstractName" HeaderText="ContractName" Target="_blank">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="17%" />
                                                            </asp:HyperLinkColumn>

                                                            <asp:BoundColumn DataField="Type" HeaderText="Type">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="Status">

                                                                <ItemTemplate>

                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                            </asp:TemplateColumn>

                                                            <asp:BoundColumn DataField="SignDate" HeaderText="SigningDate" DataFormatString="{0:yyyy/MM/dd}">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Amount" HeaderText="Amount">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Currency" HeaderText="Currency">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="PartA" HeaderText="PartyAUnit">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="PartB" HeaderText="�ҷ���λ">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundColumn>
                                                        </Columns>

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText=" �������ϲɹ�����" TabIndex="0">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label91" runat="server" Text="<%$ Resources:lang,ShangPinCaiGouDingDan%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table width="100%" cellpadding="0" cellspacing="0">

                                            <tr>

                                                <td style="border-right: solid 1px #D8D8D8; padding: 10px 5px 5px 5px"
                                                    valign="top" align="left">

                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">

                                                        <tr>

                                                            <td width="7">

                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                            <td>

                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                    <tr>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label92" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="25%"><strong>

                                                                            <asp:Label ID="Label93" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="15%"><strong>

                                                                            <asp:Label ID="Label94" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="20%"><strong>

                                                                            <asp:Label ID="Label95" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label96" runat="server" Text="<%$ Resources:lang,CaiGouYuan%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label97" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td align="right" width="6">

                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid8" runat="server" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                        ShowHeader="False"
                                                        Width="100%">

                                                        <Columns>

                                                            <asp:BoundColumn DataField="POID" HeaderText="Number">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:HyperLinkColumn DataNavigateUrlField="POID" DataNavigateUrlFormatString="TTGoodsPurchaseOrderView.aspx?POID={0}"
                                                                DataTextField="GPOName" HeaderText="Name" Target="_blank">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                            </asp:HyperLinkColumn>

                                                            <asp:BoundColumn DataField="Amount" HeaderText="�ܽ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="PurTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="�ɹ�ʱ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>

                                                            <asp:HyperLinkColumn DataNavigateUrlField="PurManCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                DataTextField="PurManName" HeaderText="�ɹ�Ա" Target="_blank">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:HyperLinkColumn>

                                                            <asp:TemplateColumn HeaderText="Status">

                                                                <ItemTemplate>

                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:DataGrid></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText=" ��������" TabIndex="0">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label98" runat="server" Text="<%$ Resources:lang,ZiChanCaiGouDingDan%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table width="100%" cellpadding="0" cellspacing="0">

                                            <tr>

                                                <td valign="top" align="left">

                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">

                                                        <tr>

                                                            <td width="7">

                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                            <td>

                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                    <tr>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label99" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="25%"><strong>

                                                                            <asp:Label ID="Label100" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="15%"><strong>

                                                                            <asp:Label ID="Label101" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="20%"><strong>

                                                                            <asp:Label ID="Label102" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label103" runat="server" Text="<%$ Resources:lang,CaiGouYuan%>"></asp:Label></strong> </td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label104" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td align="right" width="6">

                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid10" runat="server" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                        ShowHeader="False"
                                                        Width="100%">

                                                        <Columns>

                                                            <asp:BoundColumn DataField="POID" HeaderText="Number">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:HyperLinkColumn DataNavigateUrlField="POID" DataNavigateUrlFormatString="TTAssetPurchaseOrderView.aspx?POID={0}"
                                                                DataTextField="POName" HeaderText="Name" Target="_blank">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                            </asp:HyperLinkColumn>

                                                            <asp:BoundColumn DataField="Amount" HeaderText="�ܽ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="PurTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="�ɹ�ʱ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>

                                                            <asp:HyperLinkColumn DataNavigateUrlField="PurManCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                DataTextField="PurManName" HeaderText="�ɹ�Ա" Target="_blank">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:HyperLinkColumn>

                                                            <asp:TemplateColumn HeaderText="Status">

                                                                <ItemTemplate>

                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel10" runat="server" HeaderText="��������">

                                    <HeaderTemplate>

                                        <asp:Label ID="Label105" runat="server" Text="<%$ Resources:lang,GuanLianLiaoPin%>"></asp:Label>
                                    </HeaderTemplate>

                                    <ContentTemplate>

                                        <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                            <tr>

                                                <td align="left">

                                                    <asp:Label ID="LB_GoodsOwner" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="padding-right: 5px;">
                                                    <asp:Button ID="BT_CreateGoodsList" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_CreateGoodsList_Click" />
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>

                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">

                                                        <tr>

                                                            <td width="7">

                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>

                                                            <td>

                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                    <tr>

                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label110" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                        </td>
                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label111" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                        </td>

                                                                        <td align="left" width="5%"><strong>
                                                                            <asp:Label ID="Label106" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label134" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>

                                                                        <td align="left" width="20%"><strong>

                                                                            <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>

                                                                        <td align="left" width="15%"><strong>

                                                                            <asp:Label ID="Label139" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>

                                                                        <td align="left" width="13%"><strong>

                                                                            <asp:Label ID="Label136" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label215" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                        <td align="left" width="8%"><strong>

                                                                            <asp:Label ID="Label137" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label138" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td align="right" width="6">

                                                                <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid12" runat="server" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid12_ItemCommand"
                                                        ShowHeader="False"
                                                        Width="100%">

                                                        <Columns>

                                                            <asp:ButtonColumn CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                            </asp:ButtonColumn>
                                                            <asp:TemplateColumn HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="13%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                        </Columns>

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:DataGrid></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label113" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>



                    <div class="layui-layer layui-layer-iframe" id="popFileWindow"
                        style="z-index: 9999; width: 980px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2223">
                            <asp:Label ID="Label116" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table class="formBgStyle" style="width: 100%;" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,ZhengShuBianHao %>"></asp:Label>��</td>
                                    <td style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_CertificateNum" runat="server" Width="110px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                        <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,ZhengShuMingCheng %>"></asp:Label>��</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                        <asp:TextBox ID="TB_CertificateName" runat="server" Width="110px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label75" runat="server" Text="<%$ Resources:lang,FaZhengJiGuan %>"></asp:Label>��
                                    </td>
                                    <td style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_LicenseAgency" runat="server" Width="110px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                        <asp:Label ID="Label76" runat="server" Text="<%$ Resources:lang,FaZhengRiQi %>"></asp:Label>��</td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                        <asp:TextBox ID="DLC_ReleaseTime" runat="server" Width="110px"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_ReleaseTime" Enabled="True">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,FuJian %>"></asp:Label>��
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="HL_DetailFile" runat="server" Text="<%$ Resources:lang,ChaKanFuJian%>"></asp:HyperLink>
                                                </td>
                                                <td style="padding-left: 50px;">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div>
                                                                <Upload:InputFile ID="AttachFile" runat="server" Width="400px" />
                                                                <br />
                                                                <div id="ProgressBar">
                                                                    <Upload:ProgressBar ID="ProgressBar1" runat='server' Width="500px" Height="100px">
                                                                    </Upload:ProgressBar>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="BT_AttachDetailFile" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:Button ID="BT_AttachDetailFile" runat="server" Text="<%$ Resources:lang,ShangChuan %>" CssClass="inpu" OnClick="BT_AttachDetailFile_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td style="height: 1px;;" class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:Button ID="BT_AddFile" runat="server" Enabled="False" Text="<%$ Resources:lang,XinZeng %>"
                                            CssClass="inpu" OnClick="BT_AddFile_Click" />&nbsp;
                                 <asp:Button ID="BT_UpdateFile" runat="server" Enabled="False"
                                     CssClass="inpu" Text="<%$ Resources:lang,BaoCun %>" OnClick="BT_UpdateFile_Click" />
                                        &nbsp;&nbsp;
                                     <asp:Button ID="BT_DeleteFile" runat="server" Enabled="False"
                                         CssClass="inpu" Text="<%$ Resources:lang,shanchu %>" OnClick="BT_DeleteFile_Click" />
                                    </td>
                                </tr>--%>
                            </table>

                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewFile" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewFile_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label117" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popGoodsListWindow"
                        style="z-index: 9999; width: 980px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2223">
                            <asp:Label ID="Label141" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                <tr>

                                    <td width="60%">

                                        <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 10%; ">

                                                    <asp:Label ID="Label142" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="LB_ID" runat="server"></asp:Label></td>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 12%; text-align: left">

                                                    <asp:Label ID="Label143" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:DropDownList ID="DL_GoodsType" runat="server" DataTextField="Type"
                                                        DataValueField="Type">
                                                    </asp:DropDownList><td class="formItemBgStyleForAlignLeft"></td>

                                                    <td class="formItemBgStyleForAlignLeft"></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">

                                                    <asp:Label ID="Label144" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:TextBox ID="TB_GoodsCode" runat="server" Height="20px" Width="95%"></asp:TextBox></td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label145" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>�� </td>

                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >

                                                    <asp:TextBox ID="TB_GoodsName" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label146" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>��</td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" >

                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label147" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>�� </td>

                                                <td  colspan="5" class="formItemBgStyleForAlignLeft">

                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="40px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                    <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun %>" />
                                                    <asp:Button ID="BT_Clear" runat="server" CssClass="inpu" Text="<%$ Resources:lang,QingKong %>" OnClick="BT_Clear_Click" /></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label107" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>��</td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" >

                                                    <asp:TextBox ID="TB_Brand" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label149" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Price" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="85px">0.00</NickLee:NumberBox></td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label150" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>�� </td>

                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >

                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                        Width="64px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td>

                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                            <cc1:TabPanel ID="TabPanel11" runat="server">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label512" runat="server" Text="<%$ Resources:lang,LPKCLB%>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label155" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoCaiGouDeShangPin %>"></asp:Label>��
                                                    <div id="Div4" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table width="200%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                <td>

                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                        <tr>

                                                                            <td width="14%" align="left"><strong>

                                                                                <asp:Label ID="Label156" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>

                                                                            <td width="12%" align="left"><strong>

                                                                                <asp:Label ID="Label157" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>

                                                                            <td width="10%" align="left"><strong>

                                                                                <asp:Label ID="Label158" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                            <td width="15%" align="left"><strong>

                                                                                <asp:Label ID="Label159" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>

                                                                            <td width="10%" align="left"><strong>

                                                                                <asp:Label ID="Label160" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong></td>

                                                                            <td align="left"><strong>

                                                                                <asp:Label ID="Label161" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong></td>

                                                                            <td align="left"><strong>

                                                                                <asp:Label ID="Label162" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>


                                                                            <td align="left"><strong>

                                                                                <asp:Label ID="Label164" runat="server" Text="<%$ Resources:lang,DiZhi %>"></asp:Label></strong></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                                <td width="6" align="right">

                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid13" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                            Height="1px" Width="200%" OnItemCommand="DataGrid13_ItemCommand" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None">
                                                            <Columns>

                                                                <asp:BoundColumn DataField="ID" HeaderText="Number" Visible="False">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="Code">

                                                                    <ItemTemplate>

                                                                        <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode").ToString().Trim() %>' />
                                                                    </ItemTemplate>

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                                                </asp:TemplateColumn>

                                                                <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTGoodsInforView.aspx?GoodsID={0}"
                                                                    DataTextField="GoodsName" HeaderText="Name" Target="_blank">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                </asp:HyperLinkColumn>

                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="Brand">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Position" HeaderText="��ַ">

                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                </asp:BoundColumn>
                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />

                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                            <EditItemStyle BackColor="#2461BF" />

                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        </asp:DataGrid>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel12" runat="server">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,LPCXLB%>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label165" runat="server" Text="<%$ Resources:lang,QingXuanQuShangPin%>"></asp:Label>��
                                                    <div id="Div5" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table width="200%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                            <tr>
                                                                <td width="7">

                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                <td>

                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                        <tr>

                                                                            <td width="16%" align="left"><strong>

                                                                                <asp:Label ID="Label166" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong></td>

                                                                            <td width="15%" align="left"><strong>

                                                                                <asp:Label ID="Label167" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong></td>

                                                                            <td width="10%" align="left"><strong>

                                                                                <asp:Label ID="Label109" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>
                                                                            <td width="25%" align="left"><strong>

                                                                                <asp:Label ID="Label169" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong></td>

                                                                            <td width="15%" align="left"><strong>
                                                                                <asp:Label ID="Label108" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                            <td align="left"><strong>

                                                                                <asp:Label ID="Label170" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                                <td width="6" align="right">

                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid14" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                            Height="1px" Width="200%" OnItemCommand="DataGrid14_ItemCommand" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None">

                                                            <Columns>

                                                                <asp:TemplateColumn HeaderText="Code">

                                                                    <ItemTemplate>

                                                                        <asp:Button ID="BT_ItemCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ItemCode").ToString().Trim() %>' />
                                                                    </ItemTemplate>

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                                </asp:TemplateColumn>

                                                                <asp:HyperLinkColumn DataNavigateUrlField="ItemCode" DataNavigateUrlFormatString="TTItemInforView.aspx?ItemCode={0}"
                                                                    DataTextField="ItemName" HeaderText="Name" Target="_blank">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                </asp:HyperLinkColumn>
                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Specification" HeaderText="Specification">

                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="�ɹ�����">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>
                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />

                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                            <EditItemStyle BackColor="#2461BF" />

                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        </asp:DataGrid>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton7" runat="server" class="layui-layer-btn notTab" OnClick="BT_SaveGoods_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label148" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
