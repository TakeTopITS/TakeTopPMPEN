<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSiteDefaultLeftRight_TakeTopSoft.aspx.cs" Inherits="TakeTopSiteDefaultLeftRight_TakeTopSoft" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>��Ŀ�����������ѯ�����ṩ��-̩���ض�</title>

    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>

    <script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?59b6149c7d08132d9262552e18205ae6";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();

        //ȡ�����Ӵ��������ֵ
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);

            if (r != null)
                return unescape(r[2]);
            return null;
        }

        function redirectHomePage() {

            var url = window.location.host;

            if (url.toLowerCase() == "www.ourpm.net") {

                window.location.href = "http://www.ourpm.net/error.html";

            }
        }


        //������ҳ��SRC
        function setHomePageSrc() {

            var pageUrl = window.location.href;

            //��������վӦ����������վ����ת����վҳ��
            if (pageUrl.indexOf("TDSite") < 0 && (pageUrl.toLowerCase().indexOf("taketopits.com") >= 0 || pageUrl.toLowerCase().indexOf("taketopsoft.com") >= 0)) {

                redirectHomePage();
            }

            this.document.getElementById("SiteTopFrameID").src = "TakeTopSiteTop.aspx";
        }

    </script>

</head>
<frameset rows="118,*" onload="setHomePageSrc()">
    <frame id="SiteTopFrameID" name="SiteTopFrame" frameborder="no" scrolling="no" marginwidth="0" marginheight="0" marginbottom="0" />
    <frameset id="TakeTopSiteMDI" name="TakeTopSiteMDI" cols="0,*" rows="*" frameborder="no" border="0" framespacing="0">
        <frame id="SiteLeftFrameID" name="leftFrame" src="TakeTopSiteLeft.aspx" width="100%" height="100%" scrolling="yes" />
        <div style="overflow: scroll !important; -webkit-overflow-scrolling: touch !important;">
            <frame id="SiteRightContainerFrameID" name="SiteRightContainerFrame" width="100%" height="100%" scrolling="yes" />
        </div>
    </frameset>
</frameset>

<noframes>
    <body>����������֧�ֿ�ܣ�</body>
</noframes>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
