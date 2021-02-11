using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ASAssignment2
{
    public partial class Login : System.Web.UI.Page
    {
        string ASdb = ConfigurationManager.ConnectionStrings["ASdb"].ConnectionString;
        //lock user codes below (2) -------
        //static string lockstatus;
        //static int attemptcount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        //refer to captcha prac page 13 backend code and write 
        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits recaptcha form, user receives a response post parameter
            //captchaResponse consist of the user click pattern.
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and secret key
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LcBpDsaAAAAAB1gta-rzh9l6Z0Pm4ielvDdq0IS &response=" + captchaResponse);

            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //This response is in JSON format from Google server
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response (success or error)
                        //Deseralize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool False or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //lock user codes below (2) -------
            //SqlConnection connection = new SqlConnection(ASdb); //change the MYDBConnection
            //string myquery = "SELECT * from ";


            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();

            if (String.IsNullOrEmpty(userid) && String.IsNullOrEmpty(pwd))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Your email or password is empty. Please fill it up.";
            }
            

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        Session["LoggedIn"] = tb_userid.Text.Trim();

                        // create a GUID and save into session
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;

                        //create a new cookie with this guid value
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                        Response.Redirect("Homepage.aspx");
                    }
                    else
                    {
                        //errorMsg = "Userid or password is not valid. Please try again.";
                        lblMessage.Text = "Wrong username or password";
                        Response.Redirect("Login.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }

        }
        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(ASdb); //change MyDBConnectionString
            string sql = "select PasswordHash FROM Register WHERE Email=@UserID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            h = reader["PasswordHash"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(ASdb); //change MyDBConnectionString to DBName
            string sql = "select PASSWORDSALT FROM Register WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;

        }

       
    }
}