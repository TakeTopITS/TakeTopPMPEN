<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDIsomFriEdit.aspx.cs" Inherits="TTGDIsomFriEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
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
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShouYingXiangTuZhi%>"></asp:Label>
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
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DDL_GDProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_GDProject_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DanXianTuHao%>"></asp:Label>£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DDL_Isom_no" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri1£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri1" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri2£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri2" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri3£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri3" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri4£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri4" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri5£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri5" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri6£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri6" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri7£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri7" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri8£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Fri8" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">Fri9£º
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                            <asp:TextBox ID="TXT_Fri9" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="6">
                                                            <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="btnOK_Click" />&nbsp;&nbsp;
                                                            <input id="btnClose()" class="inpu" onclick="window.returnValue = false;CloseLayer();"
                                                                type="button" value="¹Ø±Õ" />
                                                            <%--<input type="button" value="·µ»Ø" id="BT_Return" class="inpu" onclick="window.location.href = 'TTGDIsomFriList.aspx'" />--%>
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
