<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZPurchaseDetailEdit.aspx.cs" Inherits="TTWZPurchaseDetailEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�ɹ��嵥�༭</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script type="text/javascript">

        $(function () { 

           


        });






        function LoadParentLit() {
            if (navigator.userAgent.indexOf("Firefox") >= 0) {

                window.parent.LoadProjectList();

            }
            else {
                window.parent.LoadProjectList();

            }

            CloseLayer();
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,CaiGouQingDan%>"></asp:Label>
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
                                                        <td  style="width: 100%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft" valign="top">
                                                            <table class="formBgStyle" style="width: 80%;">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_SerialNumber" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,BiaoDuan%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_Tenders" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,CaiGouShuLiang%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_PurchaseNumber" runat="server"></asp:TextBox>&nbsp;
                                                                        <asp:Button ID="BT_PurchaseNumber" CssClass="inpu" runat="server" Text="<%$ Resources:lang,JiSuan%>" OnClick="BT_PurchaseNumber_Click" />
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,HuanSuanShuLiang%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_ConvertNumber" runat="server"></asp:TextBox>&nbsp;
                                                                        <asp:Button ID="BT_ConvertNumber" CssClass="inpu" runat="server" Text="<%$ Resources:lang,JiSuan%>" OnClick="BT_ConvertNumber_Click" />
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YuJiFeiYong%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_PlanMoney" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShengChanChangJia%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="5">
                                                                        <asp:TextBox ID="TXT_Factory" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                    
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,GuiGeShuBianHao%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="5">
                                                                        <asp:TextBox ID="TXT_StandardCode" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>��</td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="5">
                                                                        <asp:TextBox ID="TXT_Remark" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="6" style="text-align: center;">
                                                                        <asp:Button ID="BT_Save" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="BT_Save_Click" />&nbsp;
                                                                        
                                                                        <input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                                 type="button" value="�ر�" />
                                                                        
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
                </div>
                <asp:HiddenField ID="HF_PurchaseDetailID" runat="server" />
                <asp:HiddenField ID="HF_ConvertRatio" runat="server" />
                <asp:HiddenField ID="HF_Market" runat="server" />
                <asp:HiddenField ID="HF_PurchaseCode" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BT_PurchaseNumber" />
                <asp:PostBackTrigger ControlID="BT_ConvertNumber" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
