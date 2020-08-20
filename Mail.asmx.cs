using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.Services;

namespace LotteryWebService
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Mail : System.Web.Services.WebService
    {

        SqlConnection SqlCon;
        SqlCommand SqlCmd;
        SqlDataReader Sqldr;
        WebServiceResponse wsr;


        public Mail()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);
        }

        [WebMethod]
        public WebServiceResponse SendActivationEmail(string EmailId, string Name, string url,string Password)
        {
            try
            {
                wsr = new WebServiceResponse();
                string activationCode = Guid.NewGuid().ToString();
                using (MailMessage mm = new MailMessage("sakigokul97@gmail.com", EmailId))
                {
                    string newurl = url.Replace("Signup.aspx", "Activation.aspx?ActivationCode=" + activationCode);
                    mm.Subject = "Account Activation";
                    string body = "Hello " + Name + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + newurl + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sakigokul97@gmail.com", "(Sakilove2ani)");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

                using (SqlCmd = new SqlCommand("INSERT INTO UserLoginInfo VALUES('" + EmailId + "','" + Password + "','0','" + activationCode + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res= SqlCmd.ExecuteNonQuery();
                    if(res==1)
                    {
                        wsr.Status = "1";

                    }
                    else
                    {
                        wsr.Status = "0";
                    }
                    SqlCon.Close();
                }

                return wsr;
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;
                    
                }
                else
                {
                    wsr.Error = ex.Message;
                }
                return wsr;
            }

        }

        [WebMethod]
        public WebServiceResponse SendForgetEmail(string EmailId,string url, DateTime dt)
        {
            try
            {
                wsr = new WebServiceResponse();
                string PasswordResetCode = Guid.NewGuid().ToString();
                using (MailMessage mm = new MailMessage("sakigokul97@gmail.com", EmailId))
                {
                    string body;
                    string newurl = url.Replace("Rest.aspx", "Reset.aspx?ResetCode=" + PasswordResetCode);
                    mm.Subject = "Password Reset";
                   // string body = "Hello " + Name + ",";
                    body = "<br /><br />Please click the following link to Reset your account Password";
                    body += "<br /><a href = '" + newurl + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sakigokul97@gmail.com", "(Sakilove2ani)");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

                using (SqlCmd = new SqlCommand("INSERT INTO ForgetPassword VALUES('" + PasswordResetCode + "','" + EmailId + "','" + dt + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        wsr.Status = "1";

                    }
                    else
                    {
                        wsr.Status = "0";
                    }
                    SqlCon.Close();
                }

                return wsr;
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }
                else
                {
                    wsr.Error = ex.Message;
                }
                return wsr;
            }

        }
        [WebMethod]
        public WebServiceResponse VerifyActivationEmail(string activationCode)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCommand cmd = new SqlCommand("SELECT ActivationCode FROM UserLoginInfo WHERE ActivationCode = '" + activationCode + "' and Status='0' ", SqlCon))
                {
                    SqlCon.Open();
                    Sqldr = cmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        Sqldr.Close();               
                        using (SqlCmd = new SqlCommand("UPDATE UserLoginInfo SET Status='1' WHERE ActivationCode='" + activationCode + "'",SqlCon)) 
                        {
                            int rel = SqlCmd.ExecuteNonQuery();
                            if (rel == 1)
                            {
                                SqlCon.Close();
                                wsr.Status = "1";
                                
                            }
                            else
                            {
                                SqlCon.Close();
                                wsr.Status = "0";
                            }

                           
                        }
                    }
                    else
                    {
                        Sqldr.Close();
                        wsr.Status = "0";
                    }
                    return wsr;                   
                    
                }
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }
                else
                {
                    wsr.Error = ex.Message;
                }
                return wsr;
            }



        }
    }
}
