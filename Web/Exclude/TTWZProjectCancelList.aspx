<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZProjectCancelList.aspx.cs" Inherits="TTWZProjectCancelList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>������Ŀ�б�-����</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script type="text/javascript">

        $(function () { 

           

            ControlStatusCloseChange();

        });



        function AlertStock() {
            var varStockCode = document.getElementById("HF_StockCode");
            if (confirm("����Ŀ���=" + varStockCode + "���Ƿ�һ��������")) {
                document.getElementById("BT_Cancel").click();
            }
        }




        function ControlStatusChange(objProgress, objGapValue) {

            $("#BT_NewBrowse").attr("class", "inpu");
            $("#BT_NewBrowse").removeAttr("disabled");

            if (objProgress == "����") {
                $("#BT_NewProjectCancel").attr("class", "inpu");
                $("#BT_NewProjectCancel").removeAttr("disabled");                //��Ŀ����
                $("#BT_NewCancelReturn").attr("disabled", "disabled");
                $("#BT_NewCancelReturn").removeClass("inpu");                         //�����˻�
            }
            else if (objProgress == "����") {
                $("#BT_NewProjectCancel").attr("disabled", "disabled");
                $("#BT_NewProjectCancel").removeClass("inpu");                         //��Ŀ����
                $("#BT_NewCancelReturn").attr("class", "inpu");
                $("#BT_NewCancelReturn").removeAttr("disabled");                //�����˻�
            }
            else {
                $("#BT_NewProjectCancel").attr("disabled", "disabled");
                $("#BT_NewProjectCancel").removeClass("inpu");                         //��Ŀ����
                $("#BT_NewCancelReturn").attr("disabled", "disabled");
                $("#BT_NewCancelReturn").removeClass("inpu");                         //�����˻�
            }

            if (objGapValue == "��")
            {
                $("#BT_NewGapImport").attr("class", "inpu");
                $("#BT_NewGapImport").removeAttr("disabled");                //ȱ�ڵ���
            }
            else if (objGapValue == "��") {
                $("#BT_NewGapImport").attr("disabled", "disabled");
                $("#BT_NewGapImport").removeClass("inpu");                         //ȱ�ڵ���
            } else {
                $("#BT_NewGapImport").attr("disabled", "disabled");
                $("#BT_NewGapImport").removeClass("inpu");                         //ȱ�ڵ���
            }
        }



        function ControlStatusCloseChange() {
            $("#BT_NewProjectCancel").attr("disabled", "disabled");
            $("#BT_NewProjectCancel").removeClass("inpu");
            $("#BT_NewCancelReturn").attr("disabled", "disabled");
            $("#BT_NewCancelReturn").removeClass("inpu");
            $("#BT_NewBrowse").attr("disabled", "disabled");
            $("#BT_NewBrowse").removeClass("inpu");
            $("#BT_NewGapImport").attr("disabled", "disabled");
            $("#BT_NewGapImport").removeClass("inpu");
        }



        //��Ŀ����
        function SelectProjectDesc(projectCode) {

            var url = "TTWZSelectorProjectDesc.aspx?strProjectCode=" + projectCode;

            popShowByURLForFixedSize(url, '', 1200, 500);

        }

        function LoadProjectList() {

            document.getElementById("BT_RelaceLoad").click();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuHeXiao%>"></asp:Label>
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
                                <td style="padding: 5px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="1" cellspacing="0" style="word-break: break-all; word-wrap: break-word;">
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <table class="formBgStyle">
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JinDu%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DDL_Progress" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_Progress_SelectedIndexChanged">
                                                                <asp:ListItem Text="<%$ Resources:lang,QuanBu%>" Value=""/>
                                                                <asp:ListItem Text="<%$ Resources:lang,LuRu%>" Value="¼��"/>
                                                                <asp:ListItem Text="<%$ Resources:lang,LiXiang%>" Value="����"/>
                                                                <asp:ListItem Text="<%$ Resources:lang,KaiGong%>" Value="����"/>
                                                                <asp:ListItem Text="<%$ Resources:lang,HeXiao%>" Value="����"/>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_ProjectCode" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_ProjectName" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Button ID="btnSeach" runat="server" Text="<%$ Resources:lang,ChaXun%>" CssClass="inpu" OnClick="btnSeach_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <table style="width: 30%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>

                                                        <td  class="formItemBgStyleForAlignLeft" colspan="4">&nbsp;
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LieBiaoPaiXu%>"></asp:Label>��
                                                            <asp:Button ID="BT_SortProjectCode" CssClass="inpu" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>" OnClick="BT_SortProjectCode_Click" />&nbsp;
                                                            <asp:Button ID="BT_SortProjectName" CssClass="inpu" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>" OnClick="BT_SortProjectName_Click" />&nbsp;
                                                            <asp:Button ID="BT_SortStartTime" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ZuHePaiXu%>" OnClick="BT_SortStartTime_Click" />
                                                            <asp:HiddenField ID="HF_SortProjectCode" runat="server" />
                                                            <asp:HiddenField ID="HF_SortProjectName" runat="server" />
                                                            <asp:HiddenField ID="HF_SortStartTime" runat="server" />

                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LieBiaoCaoZuo%>"></asp:Label>��
                                                            <asp:Button ID="BT_RepeatMark" runat="server" Text="<%$ Resources:lang,ChongZuoShiYongBiaoJi%>" CssClass="inpuLong" OnClick="BT_RepeatMark_Click" />&nbsp;
                                                            <asp:Button ID="BT_ProjectTotal" runat="server" Text="<%$ Resources:lang,XiangMuTongJi%>" CssClass="inpuLong" OnClick="BT_ProjectTotal_Click" />&nbsp;
                                                            <asp:Button ID="BT_RelaceLoad" runat="server" Text="<%$ Resources:lang,ChongXinJiaZaiLieBiao%>" OnClick="BT_RelaceLoad_Click" CssClass="inpu" Style="display: none;" />

                                                            <asp:Button ID="BT_Cancel" runat="server" Text="<%$ Resources:lang,HeXiaoKuBie%>" CssClass="inpu" OnClick="BT_Cancel_Click" Style="display: none;" />&nbsp;

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,GongChengXiangMuJiLuGong%>"></asp:Label><asp:Label ID="LB_RecordCount" runat="server" Text=""></asp:Label><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,Tiao%>"></asp:Label>&nbsp;
                                                <asp:Button ID="BT_NewProjectCancel" CssClass="inpu" runat="server" Text="<%$ Resources:lang,XiangMuHeXiao%>" OnClick="BT_NewProjectCancel_Click" />&nbsp;
                                                <asp:Button ID="BT_NewCancelReturn" CssClass="inpu" runat="server" Text="<%$ Resources:lang,HeXiaoTuiHui%>" OnClick="BT_NewCancelReturn_Click" />&nbsp;
                                                <asp:Button ID="BT_NewBrowse" CssClass="inpu" runat="server" Text="<%$ Resources:lang,LiuLan%>" OnClick="BT_NewBrowse_Click" />&nbsp;
                                                <asp:Button ID="BT_NewGapImport" CssClass="inpu" runat="server" Text="<%$ Resources:lang,QueKouDaoChu%>" OnClick="BT_NewGapImport_Click" />&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <div style="width: 3600px;">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="1%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="7%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,XiangMuJingLi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,KaiGongRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,WanGongRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShouQuanCaiGou%>"></asp:Label></strong>
                                                                        </td>
                                                                        <%--<td width="3%" align="center">
                                                                            <strong><asp:Label runat="server" Text="<%$ Resources:lang,LingLiaoDanWei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong><asp:Label runat="server" Text="<%$ Resources:lang,DanWeiBianHao%>"></asp:Label></strong>
                                                                        </td>--%>
                                                                        <%--<td width="3%" align="center">
                                                                            <strong><asp:Label runat="server" Text="<%$ Resources:lang,FeiKongZhuGuan%>"></asp:Label></strong>
                                                                        </td>--%>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiaGongGaiSuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZiGouGaiSuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,JianSheDanWei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,JianLiDanWei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XiangMuMiaoShu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,BianZhiRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BianZhiRen%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,KuBie%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,WeiTuoDaiLiRen%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,CaiGouJingLi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,CaiGouGongChengShi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,HeTongYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,CaiJianYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,BaoGuanYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,BuChongBianJi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,JiaLingYuSuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,HeTongJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShiGouJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ShuiJin%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YunZaFei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,FaLiaoJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,CaiGouJinDu%>"></asp:Label>%</strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,JinDu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ShiYongBiaoJi%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DG_List" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="20" ShowHeader="false"
                                                        Width="100%" OnItemCommand="DG_List_ItemCommand" OnPageIndexChanged="DG_List_PageIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="1%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("ProjectCode") +"|"+Eval("Progress") %>' CommandName="click" CssClass="notTab">
                                                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></asp:LinkButton>
                                                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProjectCode") %>' CommandName="cancel" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="����" ? true : false %>'>��Ŀ����</asp:LinkButton>--%>
                                                                    <%--<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProjectCode") %>' CommandName="cancelReturn" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="����" ? true : false %>'>�����˻�</asp:LinkButton>--%>
                                                                    <%--<asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProjectCode") %>' CommandName="browse" CssClass="notTab">���</asp:LinkButton>--%>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ProjectCode" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="ProjectName" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="7%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="7%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("ProjectName").ToString(), 23) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ProjectManagerName" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,KaiGongRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#DataBinder.Eval(Container.DataItem, "StartTime", "{0:yyyy/MM/dd}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,WanGongRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#DataBinder.Eval(Container.DataItem, "EndTime", "{0:yyyy/MM/dd}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="PowerPurchase" HeaderText="��Ȩ�ɹ�">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="PickingUnit" HeaderText="���ϵ�λ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UnitCode" HeaderText="��λ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>--%>
                                                            <%--<asp:BoundColumn DataField="FeeManageName" HeaderText="�ѿ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="ForCost" HeaderText="�׹�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SelfCost" HeaderText="�Թ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="BuildUnit" HeaderText="���赥λ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,JianSheDanWei%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("BuildUnit").ToString(), 11) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <%--<asp:BoundColumn DataField="SupervisorUnit" HeaderText="����λ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,JianLiDanWei%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("SupervisorUnit").ToString(), 11) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <%--<asp:BoundColumn DataField="ProjectDesc" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,XiangMuMiaoShu%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>

                                                                    <a style="color: blue; cursor: pointer;" onclick='SelectProjectDesc("<%# DataBinder.Eval(Container.DataItem,"ProjectCode") %>")' class="notTab" target="_blank">
                                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,XiangMuMiaoShu%>"></asp:Label></a>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <%--<asp:BoundColumn DataField="MarkTime" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,BianZhiRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#DataBinder.Eval(Container.DataItem, "MarkTime", "{0:yyyy/MM/dd}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="MarkerName" HeaderText="������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="StoreRoom" HeaderText="���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="DelegateAgentName" HeaderText="ί�д�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PurchaseManagerName" HeaderText="�ɹ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PurchaseEngineerName" HeaderText="�ɹ�����ʦ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ContracterName" HeaderText="��ͬԱ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CheckerName" HeaderText="�ļ�Ա">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SafekeepName" HeaderText="����Ա">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SupplementEditorName" HeaderText="����༭">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TheBudget" HeaderText="����Ԥ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ContractMoney" HeaderText="��ͬ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AcceptMoney" HeaderText="ʵ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ProjectTax" HeaderText="˰��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TheFreight" HeaderText="���ӷ�">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SendMoney" HeaderText="���Ͻ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="FinishingRate" HeaderText="�ɹ�����%">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Progress" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsMark" HeaderText="ʹ�ñ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        
                                                        <ItemStyle CssClass="itemStyle" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                    </asp:DataGrid>
                                                </div>
                                                <asp:Label ID="LB_ProjectSql" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="HF_ProjectCode" runat="server" />
                <asp:HiddenField ID="HF_StockCode" runat="server" />
                <asp:HiddenField ID="HF_Progress" runat="server" />
                <asp:HiddenField ID="HF_GapValue" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSeach" />
                <asp:PostBackTrigger ControlID="BT_NewGapImport" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
