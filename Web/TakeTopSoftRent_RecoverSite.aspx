<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftRent_RecoverSite.aspx.cs" Inherits="TakeTopSoftRent_RecoverSite" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />
<meta content="企业云、企业管理软件、在线租用" name="keywords">
<meta content="企业云，提供企业管理软件在线租用服务。" name="description">
<meta charset="utf-8" />

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在线租用---云平台</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/media.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/qudaohezuo.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Logo/website/js/jquery-1.3.1.js"></script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            //aHandler();

        });

        function openMDIFrom(strMDIPageName) {

            window.open(strMDIPageName, '_top');

        }

        function hideRelatedUI() {
            this.parent.window.document.getElementById("DIV_SiteMsg").style.display = "none";
        }

        function displayRelatedUI() {

            this.parent.window.document.getElementById("DIV_SiteMsg").style.display = "none";
            this.document.getElementById("DIV_Top").style.display = "none";
            this.document.getElementById("DIV_Message").style.display = "block";

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
        <center>
            <table style="text-align: left;">
                <tr>
                    <td class="formItemBgStyleForAlignLeft">
                        <div style="top: 29px; width: 100%;">
                            <table width="100%">
                                <tr>
                                    <td width="70%">

                                        <div id="DIV_Message"  style="text-align:center;padding-top:100px;display:none;">
                                             <table>
                                                 <tr>
                                                    <td style="color:red;"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZhengZaiHuiFuNiDeYingYongZhan%>"></asp:Label><asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                                    </td>
                                                 </tr>
                                                   <tr>
                                                     <td style="text-align:center;font-size:small;">
                                                        <asp:HyperLink ID="HL_SiteURL" runat="server"></asp:HyperLink>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td style="text-align:center;font-size:small;"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,QingBuYaoGuanBiCiYeMian%>"></asp:Label></td>
                                                 </tr>
                                             </table>
                                        </div> 
                                            
                                        <div id="DIV_Top">
                                            <h3><asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhanDianXinXi%>"></asp:Label></h3>
                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="100%">
                                                <tr>
                                                    <td class="formItemBgStyleForAlignLeft"  ><asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,PingTaiMingChen%>"></asp:Label><font color="#FF0000">*</font>
                                                        <br />
                                                        <asp:TextBox ID="TB_SiteAppSystemName" runat="server" ForeColor="#000000"  Style="width: 350px; height: 30px;" ></asp:TextBox>
                                                      
                                                        <br />
                                                        <span style="font-size :xx-small;"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZhuXiangMuGuanLiPingTaiZhiNeng%>"></asp:Label></span> 
                                                    </td>
                                                </tr>
                                                <tr>
                                                       <td class="formItemBgStyleForAlignLeft"  ><asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZhanDianMingChen%>"></asp:Label><font color="#FF0000">*</font>
                                                           <br />
                                                        <asp:TextBox ID="TB_SiteAppName" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;" ></asp:TextBox>
                                                        
                                                        <br />
                                                        <span style="font-size :xx-small;"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuZhiNengYouZiMuZuCheng%>"></asp:Label></span> 
                                                    </td>
                                                </tr>
                                                <tr >
                                                    
                                                    <td class="formItemBgStyleForAlignLeft"  >
                                                        <asp:Button ID="BT_Summit" runat="server" CssClass="inpu"  Height="30px" OnClientClick ="displayRelatedUI()"  OnClick="BT_Summit_Click"   Text="<%$ Resources:lang,HuiFuZhanDian%>" />
                                                        <br />
                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>

                                             <br/>

                                            <li><asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,YinWeiYaoFuZhiZhanDianWenJian%>"></asp:Label></li>
                                            <li><asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,RuYouWenTiQingZhiJieLianXiKeFu%>"></asp:Label><br />
                                                <a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img align="absmiddle" src="images/qq.png" /><asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,KeFu%>"></asp:Label></a><asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label><a href="tel:02151085119" class="call">021-51085119</a><br />
                                            </li>

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </center>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
