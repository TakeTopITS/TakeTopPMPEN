<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTBaseDataInner.aspx.cs" Inherits="TTBaseDataInner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

        });
    </script>

    <style type="text/css">
        style1 {
            color: red;
        }
    </style>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="AboveDiv">
                        <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                            <tr>
                                <td height="31" align="left" class="page_topbj">
                                    <table width="98%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="1565" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,JiChuShuJu%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <table>
                                                                <tr>
                                                                    <td>(
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>��
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlLangSwitcher" runat="server" AutoPostBack="true" DataTextField="Language" DataValueField="LangCode" OnSelectedIndexChanged="ddlLangSwitcher_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="padding-top: 3px;">
                                                                        <asp:Button ID="BT_CopyForHomeLanguage" runat="server" CssClass="inpuLong" OnClick="BT_CopyForHomeLanguage_Click" Text="<%$ Resources:lang,CopyFromHomeLanguage%>" />
                                                                    </td>
                                                                    <td>)
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="60px;"></td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td align="right">AI Seeting��</td>
                                                                    <td>
                                                                        <asp:Label ID="LB_AIType" runat="server" Text="AIType"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DL_AIType" AccessKey="A" runat="server" AutoPostBack="false">
                                                                            <asp:ListItem Value="Local" Text="Local"></asp:ListItem>
                                                                            <asp:ListItem Value="Outer" Text="Outer"></asp:ListItem>

                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label4" runat="server" Text="AIURL"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_AIURL" runat="server" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label15" runat="server" Text="AIModel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_AIModel" runat="server" Width="150px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BT_AISave" runat="server" CssClass="inpu" Text="<%$ Resources:lang,BaoCun%>" OnClick="BT_AISave_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                    <b>------&gt;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ZhuYiDai%>"></asp:Label><span style="color: red;">*</span><asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,XKNBCXNZZNXZBNSCHXG%>"></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 98%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="100%" style="padding: 5px 0px 0px 5px;" valign="top">
                                                <table style="width: 100%; height: 53px">
                                                    <tr>
                                                        <td style="width: 50%; height: 22px;" valign="top">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="30%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="15%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,GuanJianCi%>"></asp:Label></strong>
                                                                                </td>
                                                                                <%-- <td width="14%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XiangMuJingLiKeGaiZhuangTai%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="14%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,LXHZDFQLC%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="14%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,XMJDSHDBJHYX%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="14%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,YXJHJDYJHYQR%>"></asp:Label></strong>
                                                                                </td>--%>
                                                                                <td width="10%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,MuBan%>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid20" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                ShowHeader="false" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid20_ItemCommand"
                                                                PageSize="20" Width="100%">
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Type">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="BT_ProjectType" runat="server" CssClass="inpuLong" Text='<%# DataBinder.Eval(Container.DataItem,"Type") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"Type") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="KeyWord" HeaderText="�ؼ���">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <%-- <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <%# ShareClass.GetStatusHomeNameByOtherStatus(Eval("AllowPMChangeStatus").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="14%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="AutoRunWFAfterMakeProject" HeaderText="������Զ���������">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ProgressByDetailImpact" HeaderText="������ϸ��Ӱ��">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                                                    </asp:BoundColumn>

                                                                    <asp:BoundColumn DataField="PlanProgressNeedPlanerConfirm" HeaderText="Ӱ��ƻ�����Ҫ�ƻ�Աȷ��">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                                                    </asp:BoundColumn>--%>
                                                                    <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HyperLink ID="HL_WorkflowWFTemplate" Text="<%$ Resources:lang,LiuCheng%>" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Type", "TTAttachWorkFlowTemplate.aspx?RelatedType=ProjectType&RelatedName={0}") %>'
                                                                                            Target="_blank"></asp:HyperLink></td>
                                                                                    <td>
                                                                                        <asp:HyperLink ID="HL_DocumentTemplate" Text="<%$ Resources:lang,WenDang%>" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Type", "TTProjectTypeRelatedDoc.aspx?RelatedType=ProjectType&RelatedName={0}") %>'
                                                                                            Target="_blank"></asp:HyperLink></td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td style="width: 50%; height: 22px; text-align: left; background-image: url('ImagesSkin/titleBG.jpg');">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LB_SelectedProjectType" runat="server"></asp:Label>&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DeZhuangTaiLieBiao%>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left" width="10%"><strong>
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="15%"><strong>
                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="25%"><strong>
                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                    (Home) </strong></td>
                                                                                <td align="left" width="15%"><strong>
                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="8%"><strong>
                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="15%"><strong>
                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,KongZhiDian%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left"><strong>
                                                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                </strong></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="right" width="6">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid3_ItemCommand" PageSize="20" ShowHeader="false" Width="100%">
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Number">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="BT_StatusID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Status" HeaderText="Status">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TB_HomeName" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="25%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                        <HeaderStyle Font-Bold="True" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="˳��">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TB_SortNumber" runat="server" Width="50px" Text='<%# DataBinder.Eval(Container.DataItem,"SortNumber") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="ReviewControl" HeaderText="���Ƶ�">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            </caption>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <table style="width: 100%;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" rowspan="2">
                                                                        <table style="width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TB_ProjectType" runat="server" Width="100px"></asp:TextBox></td>
                                                                                <td style="text-align: Right;">
                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,GuanJianCi%>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TB_KeyWord" runat="server" Width="80px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label></td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="TB_ProjectTypeSort" runat="server" Width="30px" Text="1"></asp:TextBox></td>

                                                                            </tr>
                                                                            <tr style="display: none;">
                                                                                <td>
                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,YXXMJLGXMZT%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_AllowPMChangeStatus" runat="server">
                                                                                        <asp:ListItem>NO</asp:ListItem>
                                                                                        <asp:ListItem>YES</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,LXHZDFQLC%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_AutoRunWFAfterMakeProject" runat="server">
                                                                                        <asp:ListItem>NO</asp:ListItem>
                                                                                        <asp:ListItem>YES</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,XMJDSXJYX%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_ImpactByDetail" runat="server">
                                                                                        <asp:ListItem>YES</asp:ListItem>
                                                                                        <asp:ListItem>NO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="display: none;">
                                                                                <td colspan="3">
                                                                                    <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,RWJDYXJHJDYQRM%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_PlanProgressNeedPlanerConfirm" runat="server">
                                                                                        <asp:ListItem>NO</asp:ListItem>
                                                                                        <asp:ListItem>YES</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td colspan="3" align="left">
                                                                                    <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,XMQDXYCJYHQRM%>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_ProjectStartupNeedSupperConfirm" runat="server">
                                                                                        <asp:ListItem>NO</asp:ListItem>
                                                                                        <asp:ListItem>YES</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="8" class="formItemBgStyleForAlignCenter">
                                                                                    <asp:Button ID="BT_ProjectTypeNew" runat="server" CssClass="inpu" OnClick="BT_ProejctTypeNew_Click"
                                                                                        Text="<%$ Resources:lang,XinZeng%>" />
                                                                                    &nbsp;
                                                                                <asp:Button ID="BT_ProjectTypeUpdate" runat="server" Enabled="false" CssClass="inpu" OnClick="BT_ProjectTypeUpdate_Click"
                                                                                    Text="<%$ Resources:lang,BaoCun%>" />
                                                                                    &nbsp;
                                                                                <asp:Button ID="BT_ProjectTypeDelete" runat="server" Enabled="false" CssClass="inpu" OnClientClick="return confirm(getDeleteMsgByLangCode())" OnClick="BT_ProjectTypeDelete_Click"
                                                                                    Text="<%$ Resources:lang,ShanChu%>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%;" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <center>
                                                                            <table>
                                                                                <tr>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="LB_ID" runat="server"> </asp:Label>
                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:TextBox ID="TB_ProjectStatus" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,KongZhiDian%>"></asp:Label></td>
                                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                                        <asp:DropDownList ID="DL_ReviewControl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ReviewControl_SelectedIndexChanged">
                                                                                            <asp:ListItem Value="NO" Text="<%$ Resources:lang,Fou%>" />
                                                                                            <asp:ListItem Value="YES" Text="<%$ Resources:lang,Shi%>" />
                                                                                        </asp:DropDownList>
                                                                                        <span style="color: red; font-size: 8pt;">��<asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XuanZeKeGengGai%>"></asp:Label>��
                                                                                        </span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </center>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignCenter">
                                                                        <asp:Button ID="BT_ProjectStatusNew" runat="server" CssClass="inpu" OnClick="BT_ProjectStatusNew_Click"
                                                                            Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                     <asp:Button ID="BT_ProjectStatusDelete" runat="server" CssClass="inpu" OnClientClick="return confirm(getDeleteMsgByLangCode())" OnClick="BT_ProjectStatusDelete_Click"
                                                                         Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                     <asp:Button ID="BT_ProjectStatusSave" runat="server" CssClass="inpu" Text="<%$ Resources:lang,BaoCun%>" OnClick="BT_ProjectStatusSave_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6" style="background-color: yellow;">
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2" valign="top">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td colspan="6" style="background-color: buttonface"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,XuQiuZhuangTaiSheDing%>"></asp:Label>
                                                                        </strong>&nbsp;<span style="color: red;">*</span> </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,RenWuZhuangTaiSheDing%>"></asp:Label>
                                                                        </strong><span style="color: red;">*</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label304" runat="server" Text="<%$ Resources:lang,JiHuaZhuangTaiSheDing%>"></asp:Label>
                                                                        </strong><span style="color: red;">*</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label309" runat="server" Text="<%$ Resources:lang,GongZuoLiuZhuangTai%>"></asp:Label>
                                                                        </strong><span style="color: red;">*</span> </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label322" runat="server" Text="<%$ Resources:lang,CeShiZhuangTai%>"></asp:Label>
                                                                        </strong><span style="color: red;">*</span> </td>
                                                                    <td salign="left">&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%"><strong></strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%"><strong></strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="10%"><strong>
                                                                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%"><strong></strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="10%"><strong>
                                                                                                <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%"><strong></strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="" cellspacing="" width="98%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%">&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="height: 19px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:DataGrid ID="DataGrid5" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid5_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_ReqStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_ReqSortNumber" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_ReqStatusNew" runat="server" CssClass="inpu" OnClick="BT_ReqStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_ReqStatusDelete" runat="server" CssClass="inpu" OnClick="BT_ReqStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_ReqStatusSave" runat="server" CssClass="inpu" OnClick="BT_ReqStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DataGrid ID="DataGrid6" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid6_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TaskStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TaskSortNumber" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_TaskStatusNew" runat="server" CssClass="inpu" OnClick="BT_TaskStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_TaskStatusDelete" runat="server" CssClass="inpu" OnClick="BT_TaskStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_TaskStatusSave" runat="server" CssClass="inpu" OnClick="BT_TaskStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DataGrid ID="DataGrid8" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid8_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label307" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_PlanStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label308" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_PlanStatusSort" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_PlanStatusNew" runat="server" CssClass="inpu" OnClick="BT_PlanStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_PlanStatusDelete" runat="server" CssClass="inpu" OnClick="BT_PlanStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_PlanStatusSave" runat="server" CssClass="inpu" OnClick="BT_PlanStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DataGrid ID="DataGrid12" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid12_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label312" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_WLStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label313" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_WLStatusSort" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_WorkflowStatusNew" runat="server" CssClass="inpu" OnClick="BT_WorkflowStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;<asp:Button ID="BT_WorkflowStatusDelete" runat="server" CssClass="inpu" OnClick="BT_WorkflowStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_WorkflowStatusSave" runat="server" CssClass="inpu" OnClick="BT_WorkflowStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DataGrid ID="DataGrid16" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid16_ItemCommand" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>' Width="100px"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label323" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TestStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label324" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TestStatusSort" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_TestStatusNew" runat="server" CssClass="inpu" OnClick="BT_TestStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_TestStatusDelete" runat="server" CssClass="inpu" OnClick="BT_TestStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_TestStatusSave" runat="server" CssClass="inpu" OnClick="BT_TestStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td rowspan="4">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6" style="background-color: yellow;">
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label325" runat="server" Text="<%$ Resources:lang,ActorGroup%>"></asp:Label>
                                                                        </strong></td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label314" runat="server" Text="<%$ Resources:lang,GongZuoLiuLeiXing%>"></asp:Label>
                                                                            &nbsp;<span style="color: red;">*</span> </strong></td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label338" runat="server" Text="<%$ Resources:lang,QiTaZhuangTai%>"></asp:Label>
                                                                        </strong><span style="color: red;">*</span> </td>
                                                                    <td colspan="2" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,YuJingMingLing%>"></asp:Label>
                                                                        </strong></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="Center" valign="top">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="" cellspacing="" width="98%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label326" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label327" runat="server" Text="<%$ Resources:lang,MIngCheng%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label328" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label329" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="5%">&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid21" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_GroupName" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GroupName") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>' Width="100px"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <br />
                                                                        <asp:Button ID="BT_ActorGroupSave" runat="server" CssClass="inpu" OnClick="BT_ActorGroupSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>

                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label332" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label333" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label334" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label335" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                        <asp:DataGrid ID="DataGrid13" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid13_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Type" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Type") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>' Width="100px"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label336" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_WLType" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label337" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_WLTypeSort" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="TB_WLTypeNew" runat="server" CssClass="inpu" OnClick="TB_WLTypeNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_WLTypeDelete" runat="server" CssClass="inpu" OnClick="BT_WLTypeDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_WFTypeSave" runat="server" CssClass="inpu" OnClick="BT_WFTypeSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>

                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="25%"><strong>
                                                                                                <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="15%"><strong>
                                                                                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid1_ItemCommand" PageSize="2" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_Status" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>' Width="100px"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MakeType" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="Label339" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_OtherStatus" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label340" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_OtherStatusSort" runat="server" Width="101px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_OtherStatusNew" runat="server" CssClass="inpu" OnClick="BT_OtherStatusNew_Click" Text="<%$ Resources:lang,XinZeng%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_OtherStatusDelete" runat="server" CssClass="inpu" OnClick="BT_OtherStatusDelete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        &nbsp;
                                                                    <asp:Button ID="BT_OtherStatusSave" runat="server" CssClass="inpu" OnClick="BT_OtherStatusSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>

                                                                    <td valign="top" align="left" colspan="2">
                                                                        <table width="98%" border="0" cellpadding="" cellspacing="" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>

                                                                                    <table border="0" cellpadding="" cellspacing="" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="40%"><strong>
                                                                                                <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,MingLing%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="30%"><strong>
                                                                                                <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,MingLing%>"></asp:Label>
                                                                                                (Home) </strong></td>
                                                                                            <td align="left" width="20%"><strong>
                                                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,YuYan%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="left" width="10%"><strong>
                                                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="����">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_OrderName" runat="server" CssClass="inpuLong" Text='<%# DataBinder.Eval(Container.DataItem,"InforName") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="40%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="ģ�����ƣ����">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="TB_HomeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"HomeName") %>' Width="99%"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="30%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                    <HeaderStyle Font-Bold="True" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="˳��">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="false">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        </asp:DataGrid>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_EarlyOrderNameSave" runat="server" CssClass="inpu" OnClick="BT_EarlyOrderNameSave_Click" Text="<%$ Resources:lang,BaoCun%>" />
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6" style="background-color: yellow;">WebSite
                                                                    </td>
                                                                </tr>
                                                                <tr>

                                                                    <td colspan="2" align="left"><strong>WebSite: 
                                                                        <asp:Label ID="Label140" runat="server" Text="<%$ Resources:lang,ZhuYongChanPingLeiXing%>"></asp:Label>

                                                                    </strong></td>
                                                                    <td colspan="2" align="left"><strong>WebSite: 
                                                                        <asp:Label ID="Label141" runat="server" Text="<%$ Resources:lang,zhuYongChanPingBanBeng%>"></asp:Label>
                                                                    </strong></td>
                                                                    <td colspan="2" align="left"><strong>WebSite: 
                                                                         <asp:Label ID="Label142" runat="server" Text="<%$ Resources:lang,ShiYongChanPingYuanYing%>"></asp:Label>
                                                                    </strong></td>

                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" valign="top" align="center">

                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="98%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="center" width="5%"><strong>
                                                                                                <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="center" width="20%"><strong>
                                                                                                <asp:Label ID="Label104" runat="server" Text="<%$ Resources:lang,LeiXingMingCheng%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="center" width="10%"><strong>
                                                                                                <asp:Label ID="Label121" runat="server" Text="<%$ Resources:lang,LeiXingMingCheng%>"></asp:Label>EN
                                                                                            </strong></td>
                                                                                            <td align="center" width="25%"><strong>
                                                                                                <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,LeiXingMingCheng%>"></asp:Label>(Home)
                                                                                            </strong></td>
                                                                                            <td align="center" width="35%"><strong>
                                                                                                <asp:Label ID="Label123" runat="server" Text="DemoURL"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="center" width="5%"><strong>
                                                                                                <asp:Label ID="Label105" runat="server" Text="<%$ Resources:lang,PaiXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid4_ItemCommand" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#57CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle CssClass="notTab" HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="��������">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="20%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ENType" HeaderText="ENType">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="HomeTypeName" HeaderText="HomeTypeName">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="25%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="DemoURL" HeaderText="DemoURL">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="35%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_RentProductTypeID" runat="server"></asp:Label>
                                                                        <asp:Label ID="Label106" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_RentProductType" runat="server" Width="410px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label120" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>EN
                                                                        <asp:TextBox ID="TB_RentProductENType" runat="server" Width="110px"></asp:TextBox>

                                                                        <br />
                                                                        <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>(Home)
                                                                        <asp:TextBox ID="TB_HomeRentProductType" runat="server" Width="410px"></asp:TextBox>

                                                                        <br />
                                                                        <asp:Label ID="Label122" runat="server" Text="DemoURL"></asp:Label>
                                                                        <asp:TextBox ID="TB_RentProductDemoURL" runat="server" Width="470px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label119" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_RentProductTypeSort" runat="server" Width="110px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_RentProductTypeNew" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" OnClick="BT_RentProductTypeNew_Click" />
                                                                        <asp:Button ID="BT_RentProductTypeDelete" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" OnClick="BT_RentProductTypeDelete_Click" />

                                                                    </td>
                                                                    <td colspan="2">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="98%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="center" width="10%"><strong>
                                                                                                <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="center" width="30%"><strong>
                                                                                                <asp:Label ID="Label126" runat="server" Text="<%$ Resources:lang,LeiXingMingCheng%>"></asp:Label>
                                                                                            </strong></td>
                                                                                            <td align="center" width="30%"><strong>
                                                                                                <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,LeiXingMingCheng%>"></asp:Label>(Home)
                                                                                            </strong></td>
                                                                                            <td align="center"><strong>
                                                                                                <asp:Label ID="Label127" runat="server" Text="<%$ Resources:lang,PaiXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid25" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid25_ItemCommand" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#57CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle CssClass="notTab" HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="HomeTypeName" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                        </asp:DataGrid>

                                                                        <asp:Label ID="LB_RentProductVersionTypeID" runat="server"></asp:Label>
                                                                        <asp:Label ID="Label128" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_RentProductVersionType" runat="server" Width="110px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>(Home)
                                                                        <asp:TextBox ID="TB_HomeRentProductVersionType" runat="server" Width="110px"></asp:TextBox>

                                                                        <br />
                                                                        <asp:Label ID="Label130" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_RentProductVersionSort" runat="server" Width="110px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_RentProductVersionTypeNew" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" OnClick="BT_RentProductVersionTypeNew_Click" />
                                                                        <asp:Button ID="BT_RentProductVersionTypeDelete" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" OnClick="BT_RentProductVersionTypeDelete_Click" />
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="98%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="center" width="10%"><strong>
                                                                                                <asp:Label ID="Label132" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label>
                                                                                            </strong></td>

                                                                                            <td align="center" width="30%"><strong>
                                                                                                <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,YuanYing%>"></asp:Label>
                                                                                            </strong></td>


                                                                                            <td align="center" width="30%"><strong>
                                                                                                <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,YuanYing%>"></asp:Label>(Home)
                                                                                            </strong></td>
                                                                                            <td align="center"><strong>
                                                                                                <asp:Label ID="Label133" runat="server" Text="<%$ Resources:lang,PaiXu%>"></asp:Label>
                                                                                            </strong></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid31" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid31_ItemCommand" ShowHeader="false" Width="98%">
                                                                            <FooterStyle BackColor="#57CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle CssClass="notTab" HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="HomeTypeName" HeaderText="Type">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LangCode" HeaderText="����">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_TryProductResonTypeID" runat="server"></asp:Label>
                                                                        <asp:Label ID="Label134" runat="server" Text="<%$ Resources:lang,YuanYing%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TryProductResonType" runat="server" Width="110px"></asp:TextBox>

                                                                        <br />
                                                                        <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,YuanYing%>"></asp:Label>(Home)
                                                                        <asp:TextBox ID="TB_HomeTryProductResonType" runat="server" Width="110px"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,ShunXu%>"></asp:Label>
                                                                        <asp:TextBox ID="TB_TryProductResonSort" runat="server" Width="110px"></asp:TextBox>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="BT_TryProductResonTypeNew" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" OnClick="BT_TryProductResonTypeNew_Click" />
                                                                        <asp:Button ID="BT_TryProductResonTypeDelete" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" OnClick="BT_TryProductResonTypeDelete_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
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
