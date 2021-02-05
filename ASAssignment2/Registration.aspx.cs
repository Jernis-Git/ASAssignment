using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Configuration;

namespace ASAssignment2
{
    public partial class Registration : System.Web.UI.Page
    {
        string ASdb = ConfigurationManager.ConnectionStrings["ASdb"].ConnectionString; //change the MYDBConnection
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            //score 2 weak (can add capital letters, symbols and numbers)
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            //score 3 medium (can add symbols and numbers)
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            //score 4 strong (can add symbols)
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            //score 5 excellent
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }
            return score;

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            int scores = checkPassword(tb_password.Text);
            string status = " ";
            switch (scores)
            {
                case 1:
                    status = "Very weak password! Please make a password that has consists of at least 8 characters of letters, special characters and numbers.";
                    break;

                case 2:
                    status = "Weak Password, you can add capital letters, special characters and numbers to make your password stronger.";
                    break;

                case 3:
                    status = "Medium Strength Password, you can add special characters and numbers to make your password stronger";
                    break;

                case 4:
                    status = "Strong Password, you can add special characters to make your password stronger.";
                    break;

                case 5:
                    status = "Excellent Password!";
                    break;
            }
            pwd_checker.Text = "Status: " + status;
            if (scores < 4)
            {
                pwd_checker.ForeColor = Color.Red;
                return;
            }
            pwd_checker.ForeColor = Color.Green;


            //string pwd = get value from your textbox

            string pwd = tb_password.Text.ToString().Trim(); ;
            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            System.Diagnostics.Debug.WriteLine(cipher);
            cipher.GenerateKey();
            Key = cipher.Key;
            //Key = Encoding.ASCII.GetBytes("s698qwqB9z8RxKSK3h/q/FWSe5pEFLzN6HdIinrpkPc=");
            IV = cipher.IV;


            createAccount();


        }
        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ASdb)) 
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Register VALUES(@Email, @FName, @LName, @DOB, @PasswordHash, @PasswordSalt, @CCNo, @CCExpiry, @CVV, @Key, @IV)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@CCNo", Convert.ToBase64String(encryptData(tb_ccno.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CCExpiry", tb_ccexpirymth.Text.Trim());
                            cmd.Parameters.AddWithValue("@CVV", tb_cvv.Text.Trim());
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.Key = Key;
                cipher.IV = IV;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}