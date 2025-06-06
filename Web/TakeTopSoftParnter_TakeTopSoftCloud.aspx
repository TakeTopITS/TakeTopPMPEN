<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftParnter_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftParnter_TakeTopSoftCloud" %>

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
                                                        <p class="zuyong"><asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,RuGuoNinSuoZaiDeDiQuHuoZheNin%>"></asp:Label></p>

                                                        <%-- <p style="text-indent: 20px;"><strong><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,QuDaoFenChengZhengCe%>"></asp:Label></strong></p>

                                                        <p style="text-indent: 20px;"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,FaZhanDeDaiLiMeiChengYiDanAnCi%>"></asp:Label></p>

                                                        <p style="text-indent: 20px;"><asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,JieShaoPengYouQuFaZhanDeDiYi%>"></asp:Label></p>

                                                        <p style="text-indent: 20px;"><strong><asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,XiaoShouFenChengZhengCe%>"></asp:Label></strong></p>

                                                        <p style="text-indent: 20px;"><asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,TiGongShangJiJiWoMenBingXieZhu%>"></asp:Label></p>

                                                        <p style="text-indent: 20px;"><asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiJieXiaoShouWoMenXieZhuFen%>"></asp:Label><br />                                                          
                                                        </p>--%>


                                                        <div id="id5" style="padding-left: 20px;">
                                                            <h3>&nbsp;&nbsp;&nbsp;
                                                                
                                                                  <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:lang,RuYouYiXiangQingZaiXiaMianTianXieNiDeLianJiXinXi%>"></asp:Label>
                                                            </h3>
                                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="680px">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:lang,GongSi%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Company" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label3" runat="server"  Text="<%$ Resources:lang,LianJiRen%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;</td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server"  Text="<%$ Resources:lang,DianHua%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_PhoneNumber" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>



                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server"  Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
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
                                                                        <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;"  Text="<%$ Resources:lang,DiJiao%>" />
                                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                </tr>

                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft" width="100px"><asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_Type" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem>---</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="auto-style1"><asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BanBen%>"></asp:Label></td>
                                                                    <td class="auto-style1">
                                                                        <asp:DropDownList ID="DL_Version" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem>--------------</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,YongHuShu%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_UserNumber" runat="server" ForeColor="#000000" Style="width: 150px; height: 30px;" Text="0"></asp:TextBox>
                                                                        <strong><asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label></strong> <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft">Email </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_EMail" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft"><asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,CunChuRongLiang%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                            <asp:ListItem Value="Apply" Text="<%$ Resources:lang,ShenQing%>"> </asp:ListItem>

                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong style="font-size: medium;">GB</strong>
                                                                        <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                            </table>

                                                            <br />

                                                            <li>
                                                                <asp:Label ID="Label6" runat="server"  Text="<%$ Resources:lang,DiJiaoChengGongHouKeFuJiangHuiLiKeLianJiNiNiYeKeYiZhiJieLianJiKeFu%>"></asp:Label>：
                                                              <br />
                                                                &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img align="absmiddle" src="images/qq.png" />QQ</a>，Tel：<a href="tel:02151085119" class="call">021-51085119</a><br />
                                                            </li>

                                                        </div>

                                                        <p>&nbsp</p>

                                                        <p style="margin: 0px; padding: 0px; color: rgb(0, 0, 0); text-transform: none; text-indent: 20px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;"><strong><asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,WoMenQuDaoLiuDaTiXi%>"></asp:Label></strong></p>

                                                        <div class="news-title" style="color: rgb(0, 0, 0); text-transform: none; text-indent: 0px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;">&nbsp;&nbsp;</div>

                                                        <div class="news-title" style="color: rgb(0, 0, 0); text-transform: none; text-indent: 20px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;">
                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label1sfs4" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DuoYuanHuaFuWuGeXingHuaDeDa%>"></asp:Label></p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;<asp:Label ID="Label1235434" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,JingXiaoHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label324523424" runat="server" Text="<%$ Resources:lang,ShiZhiYuQianShu%>"></asp:Label><asp:Label ID="Label1sdfs4" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DaiLiXieYiTongGuoDiQuShouQuan%>"></asp:Label><asp:Label ID="Label345325414" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ShiXianYingLiDeHuoBan%>"></asp:Label></p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong><asp:Label ID="Label25644565" runat="server" Text="<%$ Resources:lang,ZengZhiFenXiaoHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label2354234524" runat="server" Text="<%$ Resources:lang,ShiZhiYuQianShu%>"></asp:Label><asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,DiQuZengZhiFenXiaoXieYiBing%>"></asp:Label><asp:Label ID="Label1235423454" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,Bing%>"></asp:Label><asp:Label ID="Label22345348" runat="server" Text="<%$ Resources:lang,ShiXianYingLiDeHuoBan%>"></asp:Label></p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong><asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,ShangJiHeZuoHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label223454" runat="server" Text="<%$ Resources:lang,ShiZhiYuQianShu%>"></asp:Label><asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,ShangJiHeZuoXieYiPingJieZiShen%>"></asp:Label><asp:Label ID="Label1235423544" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,XiaoShouShangJiHuoZheBangZhuWo%>"></asp:Label><asp:Label ID="Label132542357" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label><asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,DanWeiHuoZheGe%>"></asp:Label><asp:Label ID="Label123523457" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label>。</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong><asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ShouQuanFuWuHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label32345230" runat="server" Text="<%$ Resources:lang,ShiZhiHuoDeShiShiFuWuTiXiDeZi%>"></asp:Label></p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong><asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShouQuanPeiXunHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label32358" runat="server" Text="<%$ Resources:lang,ShiZhiNengJieHeXiLie%>"></asp:Label><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZaiPeiXunLingYuJinXingHeZuo%>"></asp:Label></p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong><asp:Label ID="Label323543" runat="server" Text="<%$ Resources:lang,XingYeKaiFaHuoBan%>"></asp:Label></strong></p>

                                                            <p style="margin: 0px; padding: 0px;"><asp:Label ID="Label423452350" runat="server" Text="<%$ Resources:lang,ShiZhiNengGouTongGuoPingTaiHuo%>"></asp:Label><asp:Label ID="Label145634564" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,DeHuoBanGaiXingYe%>"></asp:Label><asp:Label ID="Label1234523454" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label><asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,YiYueDingDeHeZuoMoShiZaiWoMen%>"></asp:Label></p>
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
