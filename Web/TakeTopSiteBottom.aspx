<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSiteBottom.aspx.cs" Inherits="TakeTopSiteBottom" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <meta charset="UTF-8" />
    <meta name="keywords" content="̩��,�ض�,��Ŀ����,��Ŀ�������,��Ŀ����ϵͳ,��Ŀ����ƽ̨,��Ŀ��ERP" />
    <meta name="description" content="̩�����ض�����Ŀ������Ŀ�������Ŀϵͳ��ERP��CRM��OA��Эͬƽ̨���ư칫��̩�����²�Ʒ�����������������ֵ���ɹ�����" />

    <title>��Ŀ����ҵ��������ṩ��-̩���ض���ҳ</title>
    <link href="Logo/website/css/shouye.css" rel="stylesheet" type="text/css" />
    <link href="./css/public.css" rel="stylesheet" type="text/css" />
    <link href="./css/header.css" rel="stylesheet" type="text/css" />
    <link href="./css/forever.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="./js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="./js/public.js"></script>
    <script src="./js/wk_inc.js" language="javascript"></script>
    <script type="text/javascript" src="./js/forever.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>

    <style type="text/css">
        .TextColor {
            color: red;
            background: #017afb;
            padding: 3px;
        }
    </style>

    <script type="text/javascript">
        document.ontouchmove = function (e) {
            e.preventDefault();
        };
        function AddFavorite(sURL, sTitle) {
            try {
                window.external.addFavorite(sURL, sTitle);
            }
            catch (e) {
                try {
                    window.sidebar.addPanel(sTitle, sURL, "");
                }
                catch (e) {
                    alert("�����ղ�ʧ�ܣ���ʹ��Ctrl+D�������");
                }
            }
        }

        function OnMouseDownEventForWholePage(obj) {

            jQuery(obj).parents().find("a").removeClass("current");
            jQuery(obj).parents().find("span").removeClass("TextColor");
            jQuery(obj).parent().find("span").addClass("TextColor");

            //window.parent.frames["msg1"].document.getElementById("��ť��ID")

            //window.parent.frames['SiteTopFrame'].document.all("a").removeClass("current");
            //window.parent.frames['SiteTopFrame'].document.all("span").removeClass("TextColor");

        }


        function adClick(site1, site2) {
            window.open(site1, "SiteRightContainerFrame");
            window.open(site2, "leftFrame");
         /*   window.open("TakeTopSiteBottom.aspx", "SiteTopFrame");*/
        }

    </script>

    <script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?59b6149c7d08132d9262552e18205ae6";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div class="warp">
            <div class="footer">
                <p>
                    <span><a onmousedown="OnMouseDownEvent(this)" href="javascript:adClick('TakeTopSiteContainer.aspx?ModuleName=�������&amp;HomeModuleName=�������', 'TakeTopSiteLeft.aspx?ModuleName=�������&amp;HomeModuleName=�������');" class="current">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,YiJianFanKui%>"></asp:Label>
                    </a>
                    </span>|<span>  <a onmousedown="OnMouseDownEvent(this)" href="javascript:adClick('TakeTopSiteContainer.aspx?ModuleName=��������&amp;HomeModuleName=��������', 'TakeTopSiteLeft.aspx?ModuleName=��������&amp;HomeModuleName=��������');" class="current">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,FaLuShengMing%>"></asp:Label>
                    </a>
                    </span>|<span> <a onmousedown="OnMouseDownEvent(this)" href="javascript:adClick('TakeTopSiteContainer.aspx?ModuleName=��������&amp;HomeModuleName=��������', 'TakeTopSiteLeft.aspx?ModuleName=��������&amp;HomeModuleName=��������');" class="current">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GuanYuWoMen%>"></asp:Label>
                    </a>
                    </span>
                </p>
                <p>CopyRight���ض���Ϣ(TakeTopITS Group)? 2006-2026 TakeTop ITS. <a href="http://www.miitbeian.gov.cn" target="_blank">��ICP��18022438��-1 </a>taketopits.com</p>
            </div>

        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>

