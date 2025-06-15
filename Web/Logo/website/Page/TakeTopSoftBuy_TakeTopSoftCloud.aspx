<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftBuy_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftBuy_TakeTopSoftCloud" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<meta content="企业云、企业管理软件、在线租用" name="keywords">
<meta content="企业云，提供企业管理软件在线租用服务。" name="description">
<meta charset="utf-8" />
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在线购买---泰顶拓鼎</title>
    <link href="../../../Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />
    <link href="../../../Logo/website/css/website.css" rel="stylesheet" type="text/css" />
    <link href="../../../Logo/website/css/goumai.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../Logo/website/js/jquery-1.3.1.js"></script>
    <script src="../../../js/jquery.min.js" type="text/javascript"></script>
    <script src="../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../js/allAHandlerForWebSite.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (top.location != self.location) {

            }
            else {

              /*  window.location.href = 'https://www.taketopits.com';*/
            }


            aHandlerForCurentWindow();

            $('.bigbox').hover(function () {
                $(".pointer", this).stop().animate({ top: '190px' }, { queue: false, duration: 160 });
            }, function () {
                $(".pointer", this).stop().animate({ top: '278px' }, { queue: false, duration: 160 });
            });

        });
    </script>
    <style type="text/css">
        a {
            color: #333;
            text-decoration: none;
        }
    </style>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <center>
                    <p>
                        &nbsp;
                    </p>
                    <div class="title">
                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,QiYeJiKuanKeKuoZhanRuanJian%>"></asp:Label>
                    </div>
                    <div class="title1">
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,JiMuHuaJieGouKeAnXuXuanPeiZi%>"></asp:Label>
                    </div>
              
                    <div class="box">
                        <div id="list">
                            <ul id="smalllist">
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201312231521547781.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XiangMuBao%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%--<asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,TiGongXiangMuJiHuaJinDuRenWu%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201411328407512.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XiTongJiChengXiangMuGuanLiPing%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%--<asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label13341233128" runat="server" Text="<%$ Resources:lang,XiTongJiChengXiangMuGuanLiPing%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/2013122395112630.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YanFaXiangMuGuanLiPingTai%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%--<asp:Label ID="Label1914124" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ChanPinYuJiShuYanFaGuanLi%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201312181423536202.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,GongChengXiangMuGuanLiPingTai%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%--<asp:Label ID="Label1346546549" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,GongChengSheJiShiGongXiangMu%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/20131218142463453.png" /></a>
                                    </div>

                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,RuanJianShiShiXiangMuGuanLi%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%-- <asp:Label ID="Label6767519" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,RuanJianXiTongShiShiXiangMu%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201312231521462477.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,XiangMuXingERPPingTai%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%-- <asp:Label ID="Label67876819" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,XiangMuXingZhiZaoGuanLi%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201312181423378443.png" /></a>
                                    </div>

                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ZhengWuXiangMuGuanLiPingTai%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%-- <asp:Label ID="Label19976976" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,ZhengWuXiangMuGuanLi%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="../../../Logo/website/Images/201312181423459253.png" /></a>
                                    </div>


                                    <h3>
                                        <a href="#">
                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,XiangMuGuanLiZiXun%>"></asp:Label></a></h3>
                                    <h3>
                                        <br />
                                        <%--   <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,YuanQi%>"></asp:Label>--%>

                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">
                                                <asp:Label ID="Label3789796" runat="server" Text="<%$ Resources:lang,XiangMuGuanLiZiXun%>"></asp:Label><asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,PeiXunRenZheng%>"></asp:Label></a>
                                        </p>
                                    </div>
                                </li>
                            </ul>
                            <p>
                                &nbsp;
                            </p>
                            <table style="text-align: left; width: 880px;">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <div style="top: 29px; width: 99%;">
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%">
                                                        <p class="zuyong">
                                                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<strong><asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,GouMaiTaiDingTuoDingGuanLiRuan%>"></asp:Label>，</strong><asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ShiYongShiJianBuXianRuanJianAn%>"></asp:Label>：
                                                        </p>
                                                        <div id="id5">
                                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="680px">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" style="vertical-align:middle;">
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_Type" DataValueField="Type" DataTextField="HomeTypeName" runat="server" Style="height: 50px;">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" style="vertical-align:middle;">

                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BanBen%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_Version" DataValueField="Type" DataTextField="HomeTypeName" runat="server" Style="height: 50px;">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                          
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GongSi%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Company" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LianJiRen%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" style="padding-bottom: 25px;">

                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShouJi%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_PhoneNumber" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                        <br />
                                                                        <span style="font-size: xx-small;">

                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZhuYaoJieShouChongYaoXinXiQingZhengQueTianXie%>"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" style="padding-bottom: 25px;">Email
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_EMail" runat="server" Style="width: 350px; height: 30px;" onclick="checkEmailFormat('TB_EMail')"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                        <br />
                                                                        <span style="font-size: xx-small;">

                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuYaoJieShouChongYaoXinXiQingZhengQueTianXie%>"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,YongHuShu%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_UserNumber" runat="server" ForeColor="#000000" Style="width: 150px; height: 30px;"></asp:TextBox>
                                                                        <strong>
                                                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label></strong> <font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft" style="vertical-align: bottom;">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TB_CheckCode" runat="server" ForeColor="#000000" Style="width: 150px; height: 40px;"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Image ID="IM_CheckCode" runat="server" src="../../../TTCheckCode.aspx" Style="width: 150px; height: 40px;" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;" Text="<%$ Resources:lang,DiJiao%>" />
                                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,CunChuRongLiang%>"></asp:Label></td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                            <asp:ListItem Value="Buy" Text="<%$ Resources:lang,GouMai%>"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong
                                                                            style="font-size: medium;">GB</strong> <font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <li>
                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DiJiaoChengGongHouKeFuJiangHuiLiKeLianJiNiNiYeKeYiZhiJieLianJiZhuGuan%>"></asp:Label>
                                                                <br />
                                                                &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img
                                                                    align="absmiddle" src="../../../images/qq.png" />QQ</a>，Tel：<a href="tel:02151085119"
                                                                        class="call">(086) 021-51085119</a></li>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                &nbsp;
                            </p>
                        </div>
                    </div>
                    <div class="layui-layer layui-layer-iframe" id="popwindow" name="fixedDiv" style="z-index: 9999; width: 680px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label205" runat="server" Text="&lt;div&gt;&lt;img src=../../../ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <iframe id="IFrame_BuildSite" src="TakeTopSoftRent_BuildSite.aspx" style="width: 520px; height: 540px; border: none;"
                                runat="server"></iframe>
                        </div>
                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label206" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab"
                            href="javascript:;"></a></span>
                    </div>
                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;">
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 50%; top: 50%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="../../../Images/Processing.gif" alt="Loading,please wait..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '../../../' + '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = '../../../' + 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
