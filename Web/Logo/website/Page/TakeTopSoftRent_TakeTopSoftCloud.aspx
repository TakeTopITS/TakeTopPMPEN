<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftRent_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftRent_TakeTopSoftCloud" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<meta content="企业云、企业管理软件、在线租用" name="keywords">
<meta content="企业云，提供企业管理软件在线租用服务。" name="description">
<meta charset="utf-8" />
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在线租用---泰顶拓鼎</title>
    <link href="../../../Logo/website/css/media.css" rel="stylesheet" type="text/css" />
    <link href="../../../Logo/website/css/qudaohezuo.css" rel="stylesheet" type="text/css" />
    <link href="../../../Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../../Logo/website/js/jquery-1.3.1.js"></script>
    <script src="../../../js/jquery.min.js" type="text/javascript"></script>
    <script src="../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../js/allAHandlerForWebSite.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            if (top.location != self.location) {

            }
            else {

               /* window.location.href = 'https://www.taketopits.com';*/
            }


            aHandlerForCurentWindow();

        });

        function checkEmailFormat(tbEmail) {

            var strEmail = this.document.getElementById(tbEmail).value;

            if (strEmail.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) != -1)
                return true;
            else
                this.document.getElementById(LB_MailMsg).value = "EMail格式不正确！";

        }
    </script>

    <style type="text/css">
        a {
            color: #333;
            text-decoration: none;
        }
    </style>

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
                                                <p class="zuyong" style="text-align: center;">
                                                    <strong>
                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,TaiDingYunJiangDiChengBenJiShi%>"></asp:Label></strong>
                                                </p>
                                                <p class="zuyong">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZaiTaiDingYunShangZuYongTai%>"></asp:Label></p>
                                                <div id="id4">
                                                    <h3>
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,JiaGeBiao%>"></asp:Label><h3>
                                                            <div class="main">
                                                                <p style="text-align: center; padding-bottom: 30px; font-size: 28px;">
                                                                    <br />
                                                                    <span style="text-align: center; font-size: small;">
                                                                        <a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,GongNengMoKuaiYuJiaGe%>"></asp:Label></a>
                                                                    </span>
                                                                </p>
                                                            </div>
                                                            <p>
                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,GeBanBenJiaGeCanJian%>"></asp:Label></p>
                                                            <table border="1" class="ziti" style="text-align: center; width: 95%; border-collapse: collapse; border: 1px dotted #C0C0C0;">
                                                                <tr>
                                                                    <td width="25%">&nbsp;</td>
                                                                    <td><strong>
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,BiaoZhunBan%>"></asp:Label></strong><br />
                                                                    </td>
                                                                    <td><strong>
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,QiYeBan%>"></asp:Label></strong><br />
                                                                    </td>
                                                                    <td><strong>
                                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,JiTuanBan%>"></asp:Label></strong></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,JiaGe%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,YuanRenYue%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,YuanRenYue%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,YuanRenYue%>"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,MoKuai%>"></asp:Label></td>
                                                                    <td><a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">
                                                                        <asp:Label ID="Label336578678" runat="server" Text="<%$ Resources:lang,ChaKan%>"></asp:Label></a></td>
                                                                    <td><a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">
                                                                        <asp:Label ID="Label33575" runat="server" Text="<%$ Resources:lang,ChaKan%>"></asp:Label></a></td>
                                                                    <td><a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">
                                                                        <asp:Label ID="Label339789" runat="server" Text="<%$ Resources:lang,ChaKan%>"></asp:Label></a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,JiLuShu%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label3456457" runat="server" Text="<%$ Resources:lang,BuXian%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label334567" runat="server" Text="<%$ Resources:lang,BuXian%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label3734563" runat="server" Text="<%$ Resources:lang,BuXian%>"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label434564560" runat="server" Text="<%$ Resources:lang,WeiXin%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label4685681" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label4175467" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label446461" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label44567657" runat="server" Text="<%$ Resources:lang,JiTuanGuanKong%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label3446465" runat="server" Text="<%$ Resources:lang,Bu%>"></asp:Label><asp:Label ID="Label3456341" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label348678" runat="server" Text="<%$ Resources:lang,Bu%>"></asp:Label><asp:Label ID="Label42345251" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label4456751" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label4889789" runat="server" Text="<%$ Resources:lang,YuanChengFuWu%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label418678" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label41346345" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="Label414643" runat="server" Text="<%$ Resources:lang,ZhiChi%>"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,CunChuKongJian%>"></asp:Label></td>
                                                                    <td colspan="3" style="text-align: center;">
                                                                        <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,TiGongMianFeiKongJianKuoRong%>"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                            <span style="font-size: small; color: grey;">
                                                                <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,RenZhiYaoDengLuShiYongXiTongDe%>"></asp:Label></span>
                                                            <h3></h3>
                                                            <h3></h3>
                                                            <h3></h3>
                                                            <h3></h3>
                                                            <h3></h3>
                                                        </h3>
                                                </div>
                                                <div id="id5">
                                                    <h3>
                                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,ZuYongShenQing%>"></asp:Label></h3>
                                                    <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="680px">
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft" style="vertical-align:middle;">

                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChanPin%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:DropDownList ID="DL_Type" DataValueField="Type" DataTextField="Type" runat="server" Style="height: 50px;">
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
                                                                            <asp:DropDownList ID="DL_Version" DataValueField="Type" DataTextField="Type" runat="server" Style="height: 50px;">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td style="vertical-align: middle;">
                                                                            <a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">

                                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,MoKuaiYuJiaGe%>"></asp:Label>

                                                                            </a>
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
                                                                &nbsp;<font color="#FF0000">*</font> </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LianJiRen%>"></asp:Label>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft" style="vertical-align:middle;">

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
                                                            <td class="formItemBgStyleForAlignLeft" style="padding-bottom: 25px;">Email </td>
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
                                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,Ren%>"></asp:Label></strong> <font color="#FF0000">*</font> </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                                <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
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
                                                            <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;" Text="<%$ Resources:lang,DiJiao%>" />
                                                                <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                <br />
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none;">

                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label></td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                &nbsp;<font color="#FF0000">*</font>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">

                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,CunChuRongLiang%>"></asp:Label></td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Rent" Text="<%$ Resources:lang,ZuYong%>"></asp:ListItem>
                                                                    <asp:ListItem Value="Self" Text="<%$ Resources:lang,ZiBei%>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong style="font-size: medium;">GB</strong>
                                                                <font color="#FF0000">*</font> </td>
                                                        </tr>
                                                    </table>

                                                    <br />

                                                    <li>

                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DiJiaoChengGongHouKeFuJiangHuiLiKeLianJiNiBingWeiNiKaiTongPingTai%>"></asp:Label>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,NiYeKeYiZhiJieLianJiKeFu%>"></asp:Label>：
                                                        <br />
                                                        &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img align="absmiddle" src="../../../images/qq.png" />QQ</a>，Tel：<a href="tel:02151085119" class="call">021-51085119</a>。<br />
                                                    </li>
                                                </div>
                                                <div id="id7" class="section">
                                                    <h2>3&nbsp;&nbsp;
                                                        

                                                         <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,FuJiaShuiMing%>"></asp:Label>
                                                    </h2>
                                                    <ul style="list-style: upper-alpha;">
                                                        <li>

                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PingTaiKaiTongHouKeFuHuiPeiXunNiShiYongShiYongJiBuChaoYiZhou%>"></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShiYongManYiHouBiXuFuFeiCai%>"></asp:Label><asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,NengShiYongXiTongDanKeYiXiaZai%>"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,KeYongZhiFuBaoHuoYinXingZhuan%>"></asp:Label></li>
                                                    </ul>

                                                    <table style="font-size: large;">
                                                        <tr>
                                                            <td>
                                                                <asp:Image ID="IMG_TakeTopAliPay" ImageUrl="../../../Images/TakeTopSAASAliPay.png" Width="320" Height="320" runat="server"></asp:Image>
                                                            </td>
                                                            <td style="vertical-align: middle;" class="section">

                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,KaiHuHangQiaoShangYinHangGuFenYouXianGongSiShangHaiPuDongDaDaoZhiHang%>"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <asp:Label ID="Label18" runat="server" Text="开户名：策顶信息科技（上海）有限公司 "></asp:Label>

                                                                <br />
                                                                <br />

                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ZhangHao%>"></asp:Label>
                                                                ：
                                                                121916330110501
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </div>

                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <div class="layui-layer layui-layer-iframe" id="popwindow" name="fixedDiv"
                        style="z-index: 9999; width: 680px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label205" runat="server" Text="&lt;div&gt;&lt;img src=../../../ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <iframe id="IFrame_BuildSite" src="TakeTopSoftRent_BuildSite.aspx" style="width: 520px; height: 540px; border: none;" runat="server"></iframe>

                        </div>
                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label206" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>

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
