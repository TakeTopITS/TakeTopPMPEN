<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZObjectCodeReplace.aspx.cs" Inherits="TTWZObjectCodeReplace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>物资替换代码导入</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript" type="text/javascript">

        $(function () { 
            $("#btnImport").click(function () {

                if (checkFile($("#fileExcel"), 'xls')) {
                    $("#divLoading").show();
                    return true;
                }

                return false;
            });

           
        });

        /*文件选择验证*/
        function checkFile(obj, postfix) {
            var postfix = postfix || "";
            var fileName = $(obj).val();
            if (fileName == "") {
                alert('请选择要导入的文件！');
                return false;
            }
            // 文件类型验证.
            if (postfix != "") {
                var re = new RegExp("(." + postfix + ")$");
                if (re.test(fileName.toLowerCase())) {
                    return true;
                }
                else {
                    alert("导入的文件必须为." + postfix + "类型的文件！");
                    return false;
                }
            }
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
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,WuZiTiHuanDaiMaDaoRu%>"></asp:Label>
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
                                <td style="padding: 0px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    
                                                    <tr>
                                                        <td  style="width: 45%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft">
                                                            <table class="formBgStyle" width="100%">
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XuanZeDaoRuWenJian%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:FileUpload ID="fileExcel" runat="server" />
                                                                        <asp:Button ID="btnImport" runat="server" Text="<%$ Resources:lang,DaoRu%>" OnClick="btnImport_Click" CssClass="inpu" />&nbsp;
                                                                        <asp:Button ID="BT_Pass" runat="server" Text="<%$ Resources:lang,TiHuan%>" OnClick="BT_Pass_Click" CssClass="inpuLong" />&nbsp;
                                                                        <input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                                    type="button" value="关闭" />
                                                                        <%--<input type="button" value="返回" onclick="window.location.href = 'TTWZObjectCodeList.aspx'" class="inpu" />--%>
                                                                        <div id="divLoading" style="display: none; color: Red; position: absolute;">
                                                                            <img src="resources/Images/Loading.gif" /><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhengZaiDaoRuZhongQingShaoHou%>"></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="padding-top: 5px;">
                                                                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <fieldset>
                                                                            <legend>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ZhuYiShiXiang%>"></asp:Label>：</legend>
                                                                            <font color="red">1、<asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,TXSJSYXSJLWBTXYWZDMXWZDM%>"></asp:Label>；<br />
                                                                                2、<asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DRSHBYQDSJQK%>"></asp:Label>；<br />
                                                                            </font>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="50%" align="center">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YuanWuZiDaiMa%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="50%" align="center">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XinWuZiDaiMa%>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DG_List" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="false"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="OldObjectCode" HeaderText="原物资代码">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="50%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="NewObjectCode" HeaderText="新物资代码">
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="50%" />
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                
                                                                <ItemStyle CssClass="itemStyle" />
                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
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
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
