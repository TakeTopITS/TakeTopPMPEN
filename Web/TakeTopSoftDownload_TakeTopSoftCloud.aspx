<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftDownload_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftDownload_TakeTopSoftCloud" %>


<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />
<meta content="企业云、企业管理软件、在线租用" name="keywords">
<meta content="企业云，提供企业管理软件在线租用服务。" name="description">
<meta charset="utf-8" />


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在线加盟</title>
    <link href="Logo/website/css/media.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/qudaohezuo.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        a {
            color: #333;
            text-decoration: none;
        }

        .auto-style1 {
            height: 28px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

            if (top.location != self.location) {

            }
            else {

                window.location.href = 'https://www.taketopits.com';
            }
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <center>
                    <table style="text-align: left;">
                        <tr>
                            <td class="formItemBgStyleForAlignLeft">
                                <div style="top: 29px; width: 980px;">
                                    <table width="100%">
                                        <tr>
                                            <td width="50%">

                                                <div class="qudaohezuo">
                                                    <div class="wenzi">
                                                        <p class="zuyong"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XingYeJuXianWeiCuJinYeJieXiang%>"></asp:Label></p>

                                                        <div id="id5" style="padding-left: 20px;">
                                                            <h3><asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,QingZaiXiaMianTianXieNiDeLian%>"></asp:Label></h3>
                                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="100%">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,GongSi%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Company" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;</td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_PhoneNumber" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>



                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label></td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TB_CheckCode" runat="server" ForeColor="#000000" Style="width: 150px; height: 40px;"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Image ID="IM_CheckCode" runat="server" src="TTCheckCode.aspx" Style="width: 150px; height: 40px;" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;" Text="提 交" />
                                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                </tr>

                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft" width="100px"><asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_Type" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem Value ="Download"  Text="<%$ Resources:lang,XiaZai%>"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="auto-style1"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BanBen%>"></asp:Label></td>
                                                                    <td class="auto-style1">
                                                                        <asp:DropDownList ID="DL_Version" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem Value ="ECMP" Text="<%$ Resources:lang,ZongHeGuanLiPingTai%>" ></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,YongHuShu%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_UserNumber" runat="server" ForeColor="#000000" Style="width: 150px; height: 30px;" Text="100"></asp:TextBox>
                                                                        <strong><asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label></strong> <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft">Email </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_EMail" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,CunChuRongLiang%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                            <asp:ListItem Value="Apply" Text="<%$ Resources:lang,ShenQing%>"></asp:ListItem>

                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong style="font-size: medium;">GB</strong>
                                                                        <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                            </table>

                                                            <br />

                                                            <li><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,XiaZai%>"></asp:Label><asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,LianJie%>"></asp:Label><asp:HyperLink ID="HL_SourceCodeDownload" runat="server"  Text="<%$ Resources:lang,YuanMaXiaZai%>" Visible="false"></asp:HyperLink>
                                                                <br /><asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YaoXieZhuQingLianXiTaiDingKeFu%>"></asp:Label><asp:Label ID="Label53535" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label>：021-51085119）
                                                           
                                                                </br>
                                                                     <p style="text-align: left;">
                                                                         <img alt="" src="Logo/ECMP.png" style="width: 640px; height: 600px;" />

                                                                     </p>
                                                                <%--  &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img align="absmiddle" src="images/qq.png" /><asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,KeFu%>"></asp:Label></a>，<asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label>：<a href="tel:02151085119" class="call">021-51085119</a><br />--%>
                                                            </li>

                                                        </div>


                                                    </div>
                                                </div>


                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
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
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
