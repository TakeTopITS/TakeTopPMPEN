<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZSupplierBrowse.aspx.cs" Inherits="TTWZSupplierBrowse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">

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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongFangBianJi%>"></asp:Label>
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
                                                        <td  style="width: 45%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft" valign="top">
                                                            <table class="formBgStyle" width="100%">
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GongFangBianHao%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:HiddenField ID="HF_SupplierCode" runat="server" />
                                                                        <asp:TextBox ID="TXT_SupplierNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_SupplierName" runat="server" Width="500px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,KaiHuHang%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_OpeningBank" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,FaRenDaiBiao%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_PersonDelegate" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZhangHao%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_AccountNumber" runat="server" Width="500px"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DanWeiDianHua%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_UnitPhone" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ShuiHao%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                        <asp:TextBox ID="TXT_RateNumber" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,DanWeiDiZhi%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_UnitAddress" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YouBian%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_ZipCode" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,RuWangWenJian%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        
                                                                        <asp:Literal ID="LT_InDocument" runat="server"></asp:Literal>
                                                                        <asp:HiddenField ID="HF_InDocument" runat="server" />
                                                                        <asp:HiddenField ID="HF_InDocumentURL" runat="server" />
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,WeiTuoDaiLiRen%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_DelegateAgent" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,GuDingDianHua%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_ContactPhone" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShouJi%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_Mobile" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">QQ：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_QQ" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">E-mail：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_Email" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShenQingRiQi%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_InTime" runat="server" ReadOnly="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,TuiJianDanWei%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_PushUnit" runat="server" Width="400px"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,TuiJianRen%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:HiddenField ID="HF_PushPerson" runat="server" />
                                                                        <asp:TextBox ID="TXT_PushPerson" runat="server"></asp:TextBox>&nbsp;
                                                                        <input type="button" id="btnPerson" class="inpu" runat="server" value="选择" onclick="SelectEmployee('TTWZSelectorMember.aspx', 'HF_PushPerson', 'TXT_PushPerson')" style="display:none;" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuGongWuZi%>"></asp:Label>：</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="5">
                                                                        <asp:TextBox ID="TXT_MainSupplier" runat="server" Width="400px"></asp:TextBox>&nbsp;
                                                                        <input type="button" class="inpu" value="选择" onclick="SelectDLCode('TTWZSelectorDLCode.aspx','TXT_MainSupplier','');" style="display:none;" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="6">
                                                                        <%--<asp:Button ID="btnSave" runat="server" Text="保存" CssClass="inpu" OnClick="btnSave_Click" />--%>&nbsp;&nbsp;
                                                                        <input type="button" value="关闭" id="BT_Return" class="inpu" onclick="CloseLayer();" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display:none;">
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="8">
                                                                        <a href="Doc/Test/《物资管理信息系统》软件升级系统设.doc">
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,QiYeZiZhiXinXiDengMuBan%>"></asp:Label></a>
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
                <asp:HiddenField ID="HF_ID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
