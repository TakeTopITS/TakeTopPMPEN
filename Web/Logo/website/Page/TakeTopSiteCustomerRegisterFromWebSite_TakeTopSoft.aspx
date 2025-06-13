<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft.aspx.cs" Inherits="TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft" %>

<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=0.1, user-scalable=1" />
    <title>在线试用</title>
    <link id="mainCss" href="../../../css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link id="flxappCss" href="../../../css/flxapp.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/jquery.min.js" type="text/javascript"></script>
    <script src="../../../js/allAHandlerForWebSite.js" type="text/javascript"></script>

    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
            background-repeat: repeat-x;
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 16px; /* 默认字体大小 */
        }

        a {
            color: black;
            text-decoration: none;
            font-weight: 500;
            font-size: 1rem;
        }

            a:hover {
                color: #000000;
                text-decoration: none;
            }

        .container {
            max-width: 600px;
            margin: 0 auto;
            padding: 1rem;
        }

        .form-item {
            margin-bottom: 1rem;
        }

            .form-item label {
                display: block;
                margin-bottom: 0.5rem;
                font-size: 22px; /* 默认字体大小 */
            }

            .form-item input[type="text"],
            .form-item select {
                width: 100%;
                padding: 0.5rem;
                font-size: 1rem;
                border: 1px solid #ccc;
                border-radius: 4px;
            }

                .form-item input[type="text"]:focus,
                .form-item select:focus {
                    outline: none;
                    border-color: #007bff;
                }

            .form-item .required {
                color: red;
            }

            .form-item .button {
                padding: 0.5rem 1rem;
                background-color: #007bff;
                color: white;
                border: none;
                border-radius: 4px;
                cursor: pointer;
            }

                .form-item .button:hover {
                    background-color: #0056b3;
                }

            .form-item .message {
                color: red;
                margin-top: 0.5rem;
            }

        @media (max-width: 99%) {
            body {
                font-size: 16px;
            }

            .form-item input[type="text"],
            .form-item select {
                font-size: 14px;
            }

            .form-item .button {
                font-size: 14px;
            }
        }
    </style>

    <script type="text/javascript">
        $(function () {
            // 页面加载时的初始化代码
        });

        function openMDIFrom(strMDIPageName) {
            window.open(strMDIPageName, '_blank');
        }
    </script>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div style="font-size: 2rem; text-align: center;">
                        <img align="absmiddle" src="../../../images/onlineTry.jpg" alt="onlineTry" />
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,QXTXYXXXD%>"></asp:Label>
                        <span class="required">*</span>
                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,HMWBTXRHDJZCANXTJZJDNDSYYM%>"></asp:Label>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ChanPing%>"></asp:Label>
                        <asp:Label ID="LB_Product" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="LB_ProductHomeName" runat="server"></asp:Label>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,Company%>"></asp:Label>
                        <asp:TextBox ID="TB_Company" runat="server" CssClass="form-control" Style="width: 100%; height: 2.0rem; font-size: 1rem;"></asp:TextBox>
                        <span class="required">*</span>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>
                        <asp:TextBox ID="TB_ContactPerson" runat="server" CssClass="form-control" Style="width: 100%; height: 2.0rem; font-size: 1rem;"></asp:TextBox>
                        <span class="required">*</span>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,LianXiDianHua%>"></asp:Label>
                        <asp:TextBox ID="TB_PhoneNumber" runat="server" CssClass="form-control" Style="width: 100%; height: 2.0rem; font-size: 1rem;"></asp:TextBox>
                        <span class="required">*</span>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShiYongYuanYin%>"></asp:Label>
                        <asp:DropDownList ID="DL_TryProductResonType" runat="server" DataValueField="Type" DataTextField="HomeTypeName" CssClass="form-control" Style="height: 3.0rem; font-size: 1rem;">
                        </asp:DropDownList>
                    </div>
                    <div class="form-item">
                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TB_CheckCode" runat="server" CssClass="form-control" Style="width: 100px; height: 2.0rem; font-size: 1rem;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Image ID="IM_CheckCode" runat="server" src="../../../TTCheckCode.aspx" Width="100px" Height="35px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="form-item">
                        <asp:Button ID="BT_Summit" runat="server" CssClass="button" OnClick="BT_Summit_Click" Text="<%$ Resources:lang,TiJiao%>" />
                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red" CssClass="message"></asp:Label>
                    </div>
                    <!-- 保留隐藏的表单字段 -->
                    <div class="form-item" style="display: none;">
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,Email%>"></asp:Label>
                        <asp:TextBox ID="TB_EMail" runat="server" CssClass="form-control" Style="width: 100%;"></asp:TextBox>
                        <span class="required">*</span>
                    </div>
                    <div class="form-item" style="display: none;">
                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label>
                        <asp:TextBox ID="TB_Address" runat="server" CssClass="form-control" Style="width: 100%;"></asp:TextBox>
                        <span class="required">*</span>
                    </div>
                    <div class="form-item" style="display: none;">
                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YouBian%>"></asp:Label>
                        <asp:TextBox ID="TB_PostCode" runat="server" CssClass="form-control" Style="width: 100%; height: 2.0rem;"></asp:TextBox>
                    </div>
                    <div class="form-item">
                        <a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes">
                            <img align="absmiddle" src="../../../images/qq.png" />QQ </a>，
                       
                        <a href="tel:02151085119" class="call">Tel：(086) 021-51085119</a>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%);">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="../../../Images/Processing.gif" alt="Loading, please wait..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        var cssDirectory = '<%= Session["CssDirectory"] %>';
        var oLink = document.getElementById('mainCss');
        oLink.href = '../../../css/' + cssDirectory + '/bluelightmain.css';
    </script>
</body>
</html>
