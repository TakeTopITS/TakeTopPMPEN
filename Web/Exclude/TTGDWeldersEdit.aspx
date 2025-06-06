<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDWeldersEdit.aspx.cs" Inherits="TTGDWeldersEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�����༭</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { 

            

        });

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
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,HanGongBianJi%>"></asp:Label>
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
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,HanGongHao%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Welders" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,FaBuRiQi%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_PublicTime" runat="server" onclick="WdatePicker();"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_Status" runat="server"></asp:TextBox>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_WelderName" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingHao%>"></asp:Label>��</td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_RequestCode" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,GongSiMingCheng%>"></asp:Label>��</td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TXT_CompanyName" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                         <td  class="formItemBgStyleForAlignLeft">
                                                             <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZiGe%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="TXT_Qualification" runat="server">
                                                                <asp:ListItem Text="" Value=""/>
                                                                <asp:ListItem Text="C/S" Value="C/S"/>
                                                                <asp:ListItem Text="C/G" Value="C/G"/>
                                                                <asp:ListItem Text="C/GS" Value="C/GS"/>
                                                                <asp:ListItem Text="S/S" Value="S/S"/>
                                                                <asp:ListItem Text="S/G" Value="S/G"/>
                                                                <asp:ListItem Text="S/GS" Value="S/GS"/>
                                                                <asp:ListItem Text="C+S/G" Value="C+S/G"/>
                                                                <asp:ListItem Text="C+S/S" Value="C+S/S"/>
                                                                <asp:ListItem Text="C+S/GS" Value="C+S/GS"/>
                                                                <asp:ListItem Text="SCD" Value="SCD"/>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HanJieWeiZhi%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                            <asp:TextBox ID="TXT_WeldPosition1" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:TextBox ID="TXT_WeldPosition2" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td  class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>��
                                                        </td>
                                                        <td  class="formItemBgStyleForAlignLeft" colspan="7">
                                                            <asp:TextBox ID="TXT_Remarks" runat="server" Width="400"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="8">
                                                            <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="btnOK_Click" />&nbsp;&nbsp;
                                                    <input type="button" value="����" id="BT_Return" class="inpu" onclick="window.location.href = 'TTGDWeldersList.aspx'" />
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
                <asp:HiddenField ID="HF_Welders" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
