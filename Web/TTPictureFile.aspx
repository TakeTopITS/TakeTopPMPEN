<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTPictureFile.aspx.cs" Inherits="TTPictureFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.7.2.min.js"></script>
    <script>
        //�ж�������Ƿ�֧��HTML5 Canvas           
        window.onload = function () {
            try {
                //��̬����һ��canvasԪ ������ȡ��2Dcontext����������쳣���ʾ��֧��                   
                document.createElement("canvas").getContext("2d");
                //document.getElementById("support").innerHTML = "�����֧��HTML5 CANVAS";
            } catch (e) {
                document.getElementById("support").innerHTML = "�������֧��HTML5 CANVAS";
            }
        };
        //��δ� ��Ҫ�ǻ�ȡ����ͷ����Ƶ������ʾ��Video ǩ��           
        window.addEventListener("DOMContentLoaded", function () {
            var canvas = document.getElementById("canvas"),
                context = canvas.getContext("2d"),
                video = document.getElementById("video"),
                videoObj = { "video": true },
                errBack = function (error) {
                    console.log("Video capture error: ", error.code);
                };
            //navigator.getUserMedia���д����Opera�к�����navigator.getUserMedianow               
            if (navigator.getUserMedia) {
                navigator.getUserMedia(videoObj, function (stream) {
                    video.src = stream;
                    video.play();
                }, errBack);
            } else if (navigator.webkitGetUserMedia) {
                navigator.webkitGetUserMedia(videoObj, function (stream) {
                    video.src = window.webkitURL.createObjectURL(stream);
                    video.play();
                }, errBack);
            }
            //��������հ�ť���¼���       
            $("#snap").click(function () {
                context.drawImage(video, 0, 0, 320, 320);
                CatchCode();
            });
        }, false);                     //��ʱ��         
        //var interval = setInterval(CatchCode, "300");
        //����� ˢ���� ͼ���          
        function CatchCode() {
            //$("#snap").click();
            //ʵ�����ÿɲ�д�����Դ� �� Ϊ�������հ�ť�ͻ�ȡ�˵�ǰͼ����������;            
            var canvans = document.getElementById("canvas");
            //��ȡ�����ҳ��Ļ�������                      
            //���¿�ʼ�� ����                                    
            var imgData = canvans.toDataURL();
            //��ͼ��ת��Ϊbase64����            
            var base64Data = imgData.substr(22);
            //��ǰ�˽�ȡ22λ֮����ַ�����Ϊͼ������              
            //��ʼ�첽��    
            $.post("Handler/uploadImgCode.ashx", { "img": base64Data }, function (data, status) {
                //alert(status+"11"+data);
                if (status == "success") {
                    var resultListItem = data.split('|');
                    if (resultListItem[0] == "OK") {

                        if (navigator.userAgent.indexOf("Firefox") >= 0) {


                            parent.window.document.getElementById("TB_PictureUrl").value = "<img src=" + resultListItem[1] + "/>";

                        }
                        else {


                            window.opener.document.getElementById("TB_PictureUrl").value = "<img src=" + resultListItem[1] + "/>";

                        }

                        CloseLayer();

                    }
                    else {
                        // alert(data);                  
                    }
                } else { alert("������ ʧ��"); }
            }, "text");
        }

    </script>
<script type="text/javascript" src="js/jquery-1.7.2.min.js"></script><script type="text/javascript" src="js/allAHandler.js"></script><script type="text/javascript" language="javascript">$(function () {if (top.location != self.location) { } else { CloseWebPage(); }});</script></head>
<body>
    <%--<form id="form1" runat="server">--%>
    <div>
        <div id="contentHolder">
            <table width="100%">
                <tr>
                    <td>
                        <video id="video" width="320" height="320" autoplay></video>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <button id="snap">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,PaiZhao%>"></asp:Label></button></td>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="support" runat="server"></asp:Label>

                        <canvas style="display: none" id="canvas" width="320" height="320"></canvas>
                    </td>
                </tr>

            </table>




        </div>
    </div>
    <%--</form>--%>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
