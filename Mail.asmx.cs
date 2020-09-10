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
        private SqlConnection SqlCon;
        private SqlCommand SqlCmd;
        private SqlDataReader Sqldr;
        private WebServiceResponse wsr;
        int res;
        public Mail()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);
        }

        [WebMethod]
        public WebServiceResponse SendActivationEmail(string PhoneNumber,string EmailId, string Name, string Url,string Password)
        {
            try
            {
                wsr = new WebServiceResponse();
                string activationCode = Guid.NewGuid().ToString();
                using (MailMessage mm = new MailMessage("admin@vos-it.net", EmailId))
                {
                    mm.IsBodyHtml = true;
                    string newurl = Url.Replace("Signup.aspx", "Activation.aspx?ActivationCode=" + activationCode +"&Id=" + EmailId);
                    mm.Subject = "Account Activation";
                    string body = "Hello " + Name + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + newurl + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.vos-it.net";
                    smtp.EnableSsl = true;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    NetworkCredential NetworkCred = new NetworkCredential("admin@vos-it.net", "REfwM6S?GisC");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
                using (SqlCmd = new SqlCommand("INSERT INTO Activation VALUES('" + EmailId + "','" + activationCode + "')", SqlCon))
                {
                    SqlCon.Open();
                    res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        using (SqlCmd = new SqlCommand("INSERT INTO UserLoginInfo VALUES('"+ PhoneNumber +"','" + EmailId + "','" + Password + "','0')", SqlCon))
                        {
                            res = SqlCmd.ExecuteNonQuery();
                            if (res == 1)
                            {
                                wsr.Status = "1";
                            }
                            SqlCon.Close();
                        }
                    }
                }
                return wsr;
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;               
                return wsr;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                   
                }
            }
        }

        [WebMethod]
        public WebServiceResponse SendForgetEmail(string EmailId,string Url)
        {
            try
            {
                using (SqlCmd = new SqlCommand("Select EmailId From UserLoginInfo where EmailId='" + EmailId + "'  ", SqlCon))
                {
                    SqlCon.Open();
                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        SqlCon.Close();                       
                        wsr = new WebServiceResponse();
                        DateTime dt = DateTime.Now;
                        string PasswordResetCode = Guid.NewGuid().ToString();
                        using (MailMessage mm = new MailMessage("admin@vos-it.net", EmailId))
                        {
                            string body;
                            mm.IsBodyHtml = true;
                            string newurl = Url.Replace("Reset.aspx", "Reset.aspx?ResetCode=" + PasswordResetCode+"&Id="+EmailId); 
                            mm.Subject = "Password Reset";
                            // string body = "Hello " + Name + ",";
                            body = "<br /><br />Please click the following link to Reset your account Password";
                            body += "<br /><a href = '" + newurl + "'>Click here to activate your account.</a>";
                            body += "<br /><br />Thanks";
                            mm.Body = body;
                            mm.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "mail.vos-it.net";
                            smtp.EnableSsl = false;
                            NetworkCredential NetworkCred = new NetworkCredential("admin@vos-it.net", "REfwM6S?GisC");
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                        }
                        using (SqlCmd = new SqlCommand("INSERT INTO ResetPassword VALUES('" + PasswordResetCode + "','" + EmailId + "','" + dt.ToString("yyy/MM/dd HH:mm:ss") + "')", SqlCon))
                        {
                            SqlCon.Open();
                            int res = SqlCmd.ExecuteNonQuery();
                            if (res == 1)
                            {
                                wsr.Status = "1";
                            }
                            SqlCon.Close();
                        }
                        return wsr;
                    }
                    else
                    {                        
                        return wsr;
                    }
                }                
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;                
                return wsr;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                  

                }
            }
        }
        [WebMethod]
        public WebServiceResponse VerifyActivationEmail(string ActivationCode,string EmailId)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCommand cmd = new SqlCommand("DELETE ActivationCode FROM Activation WHERE ActivationCode = '" + ActivationCode + "' and Status='0' ", SqlCon))
                {
                    SqlCon.Open();
                    Sqldr = cmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        Sqldr.Close();               
                        using (SqlCmd = new SqlCommand("UPDATE UserLoginInfo SET Status='1' WHERE EmailId='" + EmailId + "'",SqlCon)) 
                        {
                            int rel = SqlCmd.ExecuteNonQuery();
                            if (rel == 1)
                            {
                                SqlCon.Close();
                                wsr.Status = "1";                                
                            }
                        }
                    }
                     return wsr; 
                }
            }
            catch (Exception ex)
            {                 
                wsr.Status = "0";
                wsr.Error = ex.Message;               
                return wsr;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }
        }

        [WebMethod]
        public WebServiceResponse VerifyResetPassword(string ResetCode,string EmailId)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM ResetPassword WHERE ResetCode = '" + ResetCode + "' and EmailId='" + EmailId + "' ", SqlCon))
                {
                    SqlCon.Open();
                    int res=cmd.ExecuteNonQuery();
                    if (res==1)
                    {
                        SqlCon.Close();
                        wsr.Status = "1";  
                    } 
                    return wsr;
                }
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                return wsr;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }
            }

        }
    }
}
