<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDPressureTestEdit.aspx.cs" Inherits="TTGDPressureTestEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ѹ������/Ԥ�Գ��༭</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script type="text/JavaScript">



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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,YaLiShiYan%>"></asp:Label>
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
                                                <table style="width: 80%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Test Loop No��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DDL_TestLoopNo" runat="server"></asp:DropDownList>
                                                            <%--<asp:TextBox ID="TXT_TestLoopNo" runat="server"></asp:TextBox>--%>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Press Test Rec��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_PressTestRec" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Date��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_PressDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Reinst Rec��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_ReinstRec" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Date��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_ReinstDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Flushing Block��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_FlushingBlock" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Flushing Rec��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_FlushingRec" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Date��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_FlushingDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Leak Test Rec��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_LeakTestRec" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Date��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_LeakDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Remarks��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                            <asp:TextBox ID="TXT_Remarks" runat="server" Width="400"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="6">
                                                            <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="btnOK_Click" />&nbsp;&nbsp;
                                                            <input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                type="button" value="�ر�" />
                                                            <%--<input type="button" value="����" id="BT_Return" class="inpu" onclick="window.location.href = 'TTGDPressureTestList.aspx'" />--%>
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
                <asp:HiddenField ID="HF_ID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
