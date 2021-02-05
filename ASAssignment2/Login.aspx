<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Login.aspx.cs" Inherits="ASAssignment2.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="https://www.google.com/recaptcha/api.js?render=6LcBpDsaAAAAAIeHUeYzG6uN0RyBo4aUlDwy1aJP"></script>
    <style type="text/css">
        .auto-style1 {
            margin-left: 24px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <fieldset>
                <legend>Login Page</legend>
                <p>Email: <asp:Textbox ID="tb_userid" runat="server" Height="25px" Width="137px" CssClass="auto-style1" /></p>
                <p>Password: <asp:TextBox ID="tb_pwd" runat="server" Height="25px" Width="137px" CssClass="auto-style1" /></p>
                <p><asp:Button ID="btnSubmit" runat="server" Text="Login" Height="27px" Width="133px" OnClick="btnSubmit_Click" />
                <br />
                <br />

                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="False">Error message here (lblMessage)</asp:Label>
                </p>
                </fieldset>

                </div>
                </form>
                <script>
                    grecaptcha.ready(function () {
                        grecaptcha.execute(' SITE KEY ', { action: 'Login' }).then(function (token) {
                            document.getElementById("g-recaptcha-response").value = token;
                        });
                    });
                </script>
</body>
</html>
