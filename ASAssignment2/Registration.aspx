<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Registration.aspx.cs" Inherits="ASAssignment2.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Page</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            if (str.length < 8) {
                document.getElementById("pwd_checker").innerHTML = "Password Length must contain at least 8 characters.";
                document.getElementById("pwd_checker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwd_checker").innerHTML = "Password Length must contain at least 1 number.";
                document.getElementById("pwd_checker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1){
                document.getElementById("pwd_checker").innerHTML = "Password Length must contain at least 1 capital letter.";
                document.getElementById("pwd_checker").style.color = "Red";
                return ("no_capitals");
            }
            else if (str.search(/[a-z]/) == -1){
                document.getElementById("pwd_checker").innerHTML = "Password Length must contain at least 1 lower case letter.";
                document.getElementById("pwd_checker").style.color = "Red";
                return ("no_small");
            }
            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("pwd_checker").innerHTML = "Password Length must contain at least 1 special character.";
                document.getElementById("pwd_checker").style.color = "Red";
                return ("no_specials");

            }
            document.getElementById("pwd_checker").innerHTML = "Excellent Password.";
            document.getElementById("pwd_checker").style.color = "Green";
        }
    </script>
    <script src="https://www.google.com/recaptcha/api.js"></script>
    <style type="text/css">
        .auto-style4 {
            margin-left: 75px;
        }
        .auto-style5 {
            margin-left: 34px;
        }
        #welcome{
            font-weight: bold;
            font-size: 40px;
        }
        .auto-style6 {
            margin-left: 4px;
        }
        .auto-style7 {
            margin-left: 359px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p id="welcome"> WELCOME!</p>
            <p>Let's sign you up for your account with us.</p>

        </div>
        <div>
            <p>First Name:<asp:TextBox ID="tb_fname" runat="server" Width="200px" style="margin-left: 67px"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Last Name: <asp:TextBox ID="tb_lname" runat="server" Width="200px"></asp:TextBox>
            &nbsp;</p>
<p>
    Date Of Birth (DD-MM-YY): <asp:TextBox ID="tb_dob" runat="server" Width="200px"></asp:TextBox>
</p>

<p>
    Email Address:
    <asp:TextBox ID="tb_email" runat="server" Width="262px" CssClass="auto-style5"></asp:TextBox>
</p>
<p>
    Password:
    <asp:TextBox ID="tb_password" runat="server" Width="151px" CssClass="auto-style4" onkeyup="javascript:validate()"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="pwd_checker" runat="server" Text="*"></asp:Label>
</p>
        </div>
        <div>
            <p>
    Credit Card Number:
    <asp:TextBox ID="tb_ccno" runat="server" Width="193px" style="margin-left: 7px"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Card Expiration Date (MM-YY):
    <asp:TextBox ID="tb_ccexpirymth" runat="server" Width="40px"></asp:TextBox>
            </p>
            <p>
                 Card Verification Value (CVV):
    <asp:TextBox ID="tb_cvv" runat="server" Width="38px" CssClass="auto-style6" ></asp:TextBox>
</p>
            <p>
                 <asp:Label ID="err_checker" runat="server" Text="*"></asp:Label>
            </p>
            <p>
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</p>
        </div>
    <div>
    <p>
        <asp:Button ID="btn_Register"  Text="Sign Me Up!" runat="server" Width="97px" style="margin-top: 0;" Height="40px" OnClick="btn_Register_Click" CssClass="auto-style7"></asp:Button>
    </p>
        </div>
    </form>
</body>
</html>
