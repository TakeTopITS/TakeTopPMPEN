<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMultiFileUploadDemo.aspx.cs" Inherits="TTMultiFileUploadDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="jquery-1.9.1.min.js"></script>
    <link href="webuploader.css" rel="stylesheet" />
    <script src="webuploader.js"></script>
    <script type="text/javascript">
        // �ļ��ϴ�
        jQuery(function () {
            var $ = jQuery,
                $list = $('#thelist'),
                $btn = $('#ctlBtn'),
                state = 'pending',
                uploader;

            uploader = WebUploader.create({

                // ��ѹ��image
                resize: false,

                // swf�ļ�·��
                swf: 'WebUploader/Uploader.swf',

                // �ļ����շ���ˡ�
                server: 'UploadHandler.ashx',

                // ѡ���ļ��İ�ť����ѡ��
                // �ڲ����ݵ�ǰ�����Ǵ�����������inputԪ�أ�Ҳ������flash.
                pick: '#picker'

            });


            //�ϴ�ǰ��������
            uploader.on('uploadBeforeSend', function (obj, data) {

                //�ж��ĵ������Ƿ�Ϊ��
                if (document.getElementById("TB_DocType").value == "") {
                    alert("���棬��ѡ���ļ����ͣ�Warning,Doc type can not be null����")
                    return;
                }

                //���������
                data = $.extend(data, {
                    a: $("#<%=TB_DocType.ClientID%>").val(),
                    b: "b",
                    c: "c"
                });


            });

            // �����ļ���ӽ�����ʱ��
            uploader.on('fileQueued', function (file) {
                $list.append('<div id="' + file.id + '" class="item">' +
                    '<h4 class="info">' + file.name + '</h4>' +
                    '<p class="state">�ȴ��ϴ�...</p>' +
                    '</div>');
            });

            // �ļ��ϴ������д���������ʵʱ��ʾ��
            uploader.on('uploadProgress', function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress .progress-bar');

                // �����ظ�����
                if (!$percent.length) {
                    $percent = $('<div class="progress progress-striped active">' +
                        '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                        '</div>' +
                        '</div>').appendTo($li).find('.progress-bar');
                }

                $li.find('p.state').text('�ϴ���');

                $percent.css('width', percentage * 100 + '%');
            });

            uploader.on('uploadSuccess', function (file) {
                $('#' + file.id).find('p.state').text('���ϴ�');
            });

            uploader.on('uploadError', function (file) {
                $('#' + file.id).find('p.state').text('�ϴ�����');
            });

            uploader.on('uploadComplete', function (file) {
                $('#' + file.id).find('.progress').fadeOut();
            });

            uploader.on('all', function (type) {
                if (type === 'startUpload') {
                    state = 'uploading';
                } else if (type === 'stopUpload') {
                    state = 'paused';
                } else if (type === 'uploadFinished') {
                    state = 'done';
                }

                if (state === 'uploading') {
                    $btn.text('��ͣ�ϴ�');
                } else {
                    $btn.text('��ʼ�ϴ�');
                }
            });

            $btn.on('click', function () {
                if (state === 'uploading') {
                    uploader.stop();
                } else {
                    uploader.upload();
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td style="width: 100%; height: 25px; text-align: left;">
                    <asp:Label ID="LB_tbType" runat="server" Text="a"></asp:Label>��
                    <asp:Label ID="LB_DocTypeID" runat="server"></asp:Label><asp:TextBox ID="TB_DocType" runat="server" Width="125px"></asp:TextBox>

                    <asp:Label ID="LB_tbAuthority" runat="server" Text="b"></asp:Label>:<asp:DropDownList ID="DL_Visible" runat="server"  DataTextField="HomeName"
                        DataValueField="GroupName" CssClass="DDList">
                    </asp:DropDownList><asp:Label ID="LB_tbAuthor" runat="server" Text="c"></asp:Label>:<asp:TextBox ID="TB_Author" runat="server" Width="74px"></asp:TextBox></td>
            </tr>
        </table>

        <div id="uploader" class="wu-example">
            <div id="thelist" class="uploader-list"></div>
            <div class="btns">
                <div id="picker">ѡ���ļ�</div>

                <button id="ctlBtn" class="btn btn-default">��ʼ�ϴ�</button>

            </div>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
