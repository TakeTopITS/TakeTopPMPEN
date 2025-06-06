<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZObjectCodeDetail.aspx.cs" Inherits="TTWZObjectCodeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>���ʴ���</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">
        $(function () { 

           

        });



        function LoadParentLit() {
            if (navigator.userAgent.indexOf("Firefox") >= 0) {
                //alert("111");


                parent.window.dialogArguments.LoadObjectList();

            }
            else {
                //alert("121");

                window.opener.LoadObjectList();

            }


            //window.close();
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,WuZiDaiMa%>"></asp:Label>
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

                                                        <td class="formItemBgStyleForAlignLeft" colspan="2">
                                                            <div style="width: 100%;">
                                                                <table class="formBgStyle" width="80%">
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                       <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,WuZiDaiMa%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                            <asp:Label ID="LB_ObjectCode" runat="server" ></asp:Label>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            &nbsp;</td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formItemBgStyleForAlignLeft" >
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,WuZiMingCheng%>"></asp:Label>
                                                                            ��</td>
                                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                                            <asp:TextBox ID="TXT_ObjectName" runat="server" Width="400px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="formItemBgStyleForAlignLeft" >
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JiBie%>"></asp:Label>
                                                                            &nbsp;&nbsp;��</td>
                                                                        <td class="formItemBgStyleForAlignLeft" >
                                                                            <asp:TextBox ID="TXT_Level" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,GuiGeXingHao%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                            <asp:TextBox ID="TXT_Model" runat="server" Width="760px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,JiLiangDanWei%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_Unit" DataTextField="UnitName" DataValueField="ID" runat="server"></asp:DropDownList>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,HuanSuanDanWei%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:DropDownList ID="DDL_ConvertUnit" DataTextField="UnitName" DataValueField="ID" runat="server"></asp:DropDownList>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="6">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShiChangHangQing%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_Market" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,HuanSuanXiShu%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:TextBox ID="TXT_ConvertRatio" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BiaoZhun%>"></asp:Label>&nbsp;&nbsp;��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                            <asp:TextBox ID="TXT_Standard" runat="server" Width="400px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,DuiZhaoBiaoZhun%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                            <asp:TextBox ID="TXT_ReferStandard" runat="server" Width="400px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td  class="formItemBgStyleForAlignLeft">
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,DuiZhaoMiaoShu%>"></asp:Label>��</td>
                                                                        <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                            <asp:TextBox ID="TXT_ReferDesc" runat="server" Width="760px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="6">
                                                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:lang,BaoCun%>" OnClick="btnSave_Click" CssClass="inpu" />&nbsp;
                                                                            <input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                                type="button" value="�ر�" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </div>
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
                <asp:HiddenField ID="HF_ObjectCode" runat="server" />
                <asp:HiddenField ID="HF_DLCode" runat="server" />
                <asp:HiddenField ID="HF_ZLCode" runat="server" />
                <asp:HiddenField ID="HF_XLCode" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>

    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
