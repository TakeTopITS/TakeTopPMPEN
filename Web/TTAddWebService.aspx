<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAddWebService.aspx.cs" Inherits="TTAddWebService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����Web Service</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

            $("#BT_Save").click(function () {
                var strWebServiceName = $("#TXT_WebServiceName").val();
                if (strWebServiceName == "" || strWebServiceName == null) {
                    alert("����дWeb Service���ƣ�");
                    return;
                }
                var strWebServiceURL = $("#TXT_WebServiceURL").val();
                if (strWebServiceURL == "" || strWebServiceURL == null) {
                    alert("����дWeb Service URL ��ַ��");
                    return;
                }
                var strMethodName = $("#TXT_MethodName").val();
                if (strMethodName == "" || strMethodName == null) {
                    alert("����дWeb Service �������ƣ�");
                    return;
                }
                var strArgArray = $("#TXT_ArgArray").val();
                var strComment = $("#TXT_Comment").val();
                var intID = $("#HF_ID").val();
                var da = "strWebServiceName=" + escape(strWebServiceName) + "&strWebServiceURL=" + escape(strWebServiceURL) + "&strMethodName=" + escape(strMethodName) + "&strArgArray=" + escape(strArgArray) + "&strComment=" + escape(strComment) + "&intID=" + intID;

                $.ajax({
                    type: "POST",
                    url: "Handler/addWebServiceHandler.ashx",
                    data: da,
                    success: function (json) {
                        alert(json);
                        if (json == "����ɹ���") {
                            window.location.href = "TTWebServiceList.aspx";
                        }
                    },
                    error: function () {
                        alert("Failed");
                    }

                });

            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
            <tr>
                <td height="31" class="page_topbj">
                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="29">
                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                        </td>
                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,TianJiaWebService%>"></asp:Label>
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
                    <table width="90%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" style="padding-top: 5px;">
                                <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft" style="width: 10%">
                                            <span style="font-size: 11pt">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,WebServiceMingCheng%>"></asp:Label>��</span>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft" style="width: 15%">
                                            <asp:TextBox ID="TXT_WebServiceName" runat="server" Width="320"></asp:TextBox>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft"><span style="font-size: 11pt">
                                            <asp:Label ID="Label3" runat="server" ></asp:Label>WebServiceURL��</span></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TXT_WebServiceURL" runat="server" Width="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <span style="font-size: 11pt">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,FangFaMing%>"></asp:Label>��</span>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TXT_MethodName" runat="server" Width="320"></asp:TextBox>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft"><span style="font-size: 11pt">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,CanShuShuZu%>"></asp:Label>��</span></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TXT_ArgArray" runat="server" Width="500"></asp:TextBox><font color="red"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DuoGeYiFenKaiBanJiao%>"></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                            <span style="font-size: 11pt">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZhuShi%>"></asp:Label>��</span><br />
                                            <textarea id="TXT_Comment" cols="150" rows="4" runat="server"></textarea>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold; font-size: 11pt">
                                        <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                            <input type="button" value="New" id="BT_Save" class="inpu" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HF_ID" runat="server" />
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
