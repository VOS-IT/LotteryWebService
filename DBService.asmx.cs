﻿using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Management;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LotteryWebService
{
    /// <summary>
    /// Summary description for DBService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class DBService : System.Web.Services.WebService
    {
        private SqlConnection SqlCon;
        private SqlCommand SqlCmd;
        private SqlDataReader Sqldr;
        private SqlDataAdapter Sqlda;

        WebServiceResponse wsr;
        UserInfo ui;
        TicketInfo ti;
        AdminInfo ai;

        private  DataSet Usersds;
        private  DataSet Ticketsds;
        private DataSet Storeds;
        private DataSet Referralds;

        private DataTable Responsedt;
        private DataTable Usersdt;
        private DataTable Ticketsdt;
        private DataTable Storedt;
        private DataTable Referraldt;
        private DataTable Level1dt;
        private DataTable Level2dt;
        private DataTable Level3dt;
        private DataTable Level4dt;
        private DataTable Level5dt;


        public DBService()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);

        }
        [WebMethod]
        public string  login(string UserId,string Password)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("Select UserId From UserLoginInfo where Userid='" + UserId + "' and Password='" + Password + "' and Status=1 ", SqlCon))
                {
                    SqlCon.Open();
                    var name = SqlCmd.ExecuteScalar();

                    if (name != null)
                    {
                        wsr.Status = name.ToString();

                    }

                }
                SqlCon.Close();
                return "true";

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }
                return "false" ;
            }
        }
        [WebMethod]
        public WebServiceResponse VerifyUserLogin(string UserId, string Password)
        {

            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("Select UserId From UserLoginInfo where Userid='" + UserId + "' and Password='" + Password + "' and Status=1 ", SqlCon))
                {
                    SqlCon.Open();
                    var name = SqlCmd.ExecuteScalar();

                    if (name != null)
                    {
                        wsr.Status = name.ToString();

                    }                              

                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                   

                }                
                return wsr;
            }


        }
        [WebMethod]
        public AdminInfo VerifyAdminLogin(string UserId, string Password)
        {

            try
            {
                ai = new AdminInfo();

                using (SqlCmd = new SqlCommand("Select Name,Type From AdminLoginInfo where Userid='" + UserId + "' and Password='" + Password + "' ", SqlCon))
                {
                    SqlCon.Open();

                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        ai.Name = Sqldr.GetString(0);
                        ai.Type = Sqldr.GetString(1);
                        ai.Status = "1";
                        SqlCon.Close();
                    }
                }
                SqlCon.Close();

                return ai;

            }
            catch (Exception ex)
            {
                ai.Status = "0";
                ai.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                  

                }
               
                return ai;
            }

        }
        [WebMethod]
        public WebServiceResponse IsExistingUser(string UserId)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("Select UserId From UserLoginInfo where UserId='" + UserId + "'  ", SqlCon))
                {
                    SqlCon.Open();
                    var name = SqlCmd.ExecuteScalar();

                    if (name != null)
                    {
                        wsr.Status = "1";

                    }                    
                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                  

                }
                
                return wsr;
            }

        }
        [WebMethod]
        public WebServiceResponse InsertUserInfo(string FirstName, string LastName, string PhoneNumber, string Email,string Password, string DOB, string Country, string IdType, string IdNo, string Address, string State, string City, string Code)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("INSERT INTO UserInfo(FirstName,LastName,PhoneNumber,Email,Password,DateOfBirth,Nationality,IDType,IdNo,Address,State,City,Code) VAULES('" + FirstName + "','" + LastName + "','" + PhoneNumber + "','" + Email + "','" + Password + "','" + DOB + "','" + Country + "','" + IdType + "','" + IdNo + "','" + Address + "','" + State + "','" + City + "','" + Code + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();

                    if (res == 1)
                    {
                        wsr.Status = "1";

                    }
                    SqlCon.Close();
                    return wsr;
                }
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }
                return wsr;
            }

        }
        [WebMethod]
        public WebServiceResponse InsertTractionInfo()
        {
            try
            {
                wsr = new WebServiceResponse();
                //using (SqlCmd = new SqlCommand("insert into UserInfo values('" + FirstName + "','" + LastName + "','" + PhoneNumber + "','" + Email + "','" + Password + "','" + DOB + "','" + Country + "','" + IdType + "','" + IdNo + "','" + Address + "','" + State + "','" + City + "','" + Code + "')", SqlCon))
                //{
                //    SqlCon.Open();
                //    int res = SqlCmd.ExecuteNonQuery();

                //    if (res == 1)
                //    {
                //        wsr.Status = "1";

                //    }
                //   

                //}
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }                
                return wsr;
            }

        }
        [WebMethod]
        public WebServiceResponse InsertTicketInfo(string TicketNo,int TicketPrice,int PriceAmount,DateTime DisplayDate,DateTime CloseDate,DateTime DrawDate,string Status)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("INSERT into TicketInfo(TicketNo,TicketPrice,PriceAmount,DisplayDate,CloseDate,DrawDate,Status) values('" + TicketNo + "','" + TicketPrice + "','" + PriceAmount + "','" + Convert.ToDateTime(DisplayDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(CloseDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(DrawDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Status + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {                       
                        wsr.Status = "1";
                    }                    
                   
                }
                SqlCon.Close();
                return wsr;

            }
            catch(Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                   

                }                

                return wsr;
            }
            

        }
        [WebMethod]
        public WebServiceResponse InsertStoreInfo(string Store, string Address,string Timing)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("insert into StoreInfo values('" + Store + "','" + Address + "','" + Timing + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        wsr.Status = "1";
                    }

                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }

                return wsr;
            }


        }

        //Update Methods

        [WebMethod]
        public WebServiceResponse UpdateUserInfo(string FirstName, string LastName, string PhoneNumber, string Email,  string DOB, string Country, string IdType, string IdNo, string Address, string State, string City, string Code)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("UPDATE UserInfo SET FirstName='" + FirstName + "',LastName='" + LastName + "',PhoneNumber='" + PhoneNumber + "',Email='" + Email + "',DateOfBirth='" + DOB + "',Nationality='" + Country + "',IDType='" + IdType + "',IdNo='" + IdNo + "',Address='" + Address + "',State='" + State + "',City='" + City + "',Code='" + Code + "' WHERE PhoneNumber='" + PhoneNumber + "'", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();

                    if (res == 1)
                    {
                        wsr.Status = "1";

                    }
                    SqlCon.Close();
                    return wsr;
                }
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }
                return wsr;
            }

        }


        [WebMethod]
        public WebServiceResponse UpdateTicketInfo(string TicketNo, string Status)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("UPDATE TicketInfo SET Status='" + Status + "' WHERE TicketNo='" + TicketNo + "' ", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {

                        wsr.Status = "1";
                    }

                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {
                 wsr.Status="0";
                 wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    

                }               

                return wsr;
            }


        }







        [WebMethod]
        public TicketInfo GetTicketInfo()
        {
            try
            {
                ti = new TicketInfo();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT ((CASE WHEN User1 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User2 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User3 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User4 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User5 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User6 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User7 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User8 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User9 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User10 IS NULL THEN 1 ELSE 0 END)) AS TicketCount,TicketNo,TicketPrice,PriceAmount,CloseDate FROM TicketInfo ORDER BY TicketNo DESC", SqlCon))
                {
                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        ti.TicketCount = Sqldr.GetInt32(0);
                        ti.TicketNo = Sqldr.GetValue(1).ToString();
                        ti.TicketPrice =Sqldr.GetInt32(2);
                        ti.PriceAmount = Sqldr.GetInt32(3); 
                        ti.CloseDate = Sqldr.GetDateTime(4);
                        ti.Status = 1;
                    }                   
                    SqlCon.Close();
                    return ti;
                }
                    
            }
            catch(Exception ex)
            {
                ti.Status = 0;
                ti.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                   

                }             

                return ti;
            }
            
        }

        [WebMethod]
        public WebServiceResponse GetUserCount()
        {
            try
            {
                wsr = new WebServiceResponse();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT COUNT(*) AS UserCount FROM UserInfo ", SqlCon))
                {
                    string Count = SqlCmd.ExecuteScalar().ToString();

                    if (Count!="")
                    {
                        wsr.Status = Count;                        
                    }
                    SqlCon.Close();
                    return wsr;
                }

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }

                return wsr;
            }

        }

        [WebMethod]
        public WebServiceResponse GetTicketCount()
        {
            try
            {
                wsr = new WebServiceResponse();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT COUNT(*) AS TicketCount FROM TicketInfo ", SqlCon))
                {
                    string Count = SqlCmd.ExecuteScalar().ToString();

                    if (Count != "")
                    {
                        wsr.Status = Count;
                    }
                    SqlCon.Close();
                    return wsr;
                }

            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                wsr.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();

                }

                return wsr;
            }

        }
        [WebMethod]
        public UserInfo GetUserInfo(string EmailId)
        {
            try
            {
                ui = new UserInfo();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo Where Email='"+EmailId+"'", SqlCon))
                {
                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                       
                        ui.FirstName = Sqldr.GetValue(0).ToString();
                        ui.LastName = Sqldr.GetValue(1).ToString();
                        ui.PhoneNumber = Sqldr.GetValue(2).ToString();
                        ui.Email = Sqldr.GetValue(3).ToString();
                        ui.Password = Sqldr.GetValue(4).ToString();
                        ui.DateOfBirth = Sqldr.GetValue(5).ToString();
                        ui.Nationality = Sqldr.GetValue(6).ToString();
                        ui.IDType = Sqldr.GetValue(7).ToString();
                        ui.IdNo = Sqldr.GetValue(8).ToString();
                        ui.Address = Sqldr.GetValue(9).ToString();
                        ui.State = Sqldr.GetValue(10).ToString();
                        ui.City = Sqldr.GetValue(11).ToString();
                        ui.Code = Sqldr.GetValue(12).ToString();
                        ui.Status = 1;
                    }
                    
                    SqlCon.Close();
                    return ui;
                }
            }
            catch (Exception ex)
            {
                ui.Status = 0;
                ui.Error = ex.Message;
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                   

                }
                
                return ui;
            }
        }     
        [WebMethod]
        public DataSet GetUsersInfo()
        {
              

            try
            {
                Usersds = new DataSet();
                Usersdt = Usersds.Tables.Add("UsersInfo");
                Responsedt = Usersds.Tables.Add("Response");
                Usersds.Tables["Response"].Columns.Add("Status", typeof(string));
                Usersds.Tables["Response"].Columns.Add("Error", typeof(string));

                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Usersds,"UsersInfo");
                        if (Usersds.Tables["UsersInfo"].Rows.Count>0)
                        {
                            Usersds.Tables["Response"].Rows.Add("1","");
                           
                        }
                        SqlCon.Close();
                        return Usersds;
                        
                    }                       
                   
                }
            }
            catch (Exception ex)
            {                
               
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                  
                    Usersds.Tables["Respone"].Rows.Add("0",ex.Message);
                }
                else
                {
                    Usersds.Tables["Respone"].Rows.Add("0",ex.Message);
                }
                return Usersds;
            }
        }
        [WebMethod]
        public DataSet GetTicketsInfo()
        {           

            try
            {
                Ticketsds = new DataSet();
                Ticketsdt = Ticketsds.Tables.Add("TicketsInfo");
                Responsedt = Ticketsds.Tables.Add("Response");
                Ticketsds.Tables["Response"].Columns.Add("Status", typeof(string));
                Ticketsds.Tables["Response"].Columns.Add("Error", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT TicketNo,TicketPrice,PriceAmount,DisplayDate,CloseDate,DrawDate,Status FROM TicketInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Ticketsds, "TicketsInfo");
                        if (Ticketsds.Tables["TicketsInfo"].Rows.Count > 0)
                        {
                            Ticketsds.Tables["Response"].Rows.Add("1","");
                           
                        }
                        SqlCon.Close();
                        return Ticketsds;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                
                
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Ticketsds.Tables["Respone"].Rows.Add("0",ex.Message);
                }
                else
                {
                    Ticketsds.Tables["Respone"].Rows.Add("0",ex.Message);
                }
                return Ticketsds;
            }
        }
        [WebMethod]
        public DataSet GetStoreInfo()
        {

            try
            {
                Storeds = new DataSet();
                Storedt = Storeds.Tables.Add("StoreInfo");
                Responsedt = Storeds.Tables.Add("Response");
                Storeds.Tables["Response"].Columns.Add("Status", typeof(string));
                Storeds.Tables["Response"].Columns.Add("Error", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT Store,Address,Timing FROM StoreInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Storeds, "StoreInfo");
                        if (Storeds.Tables["StoreInfo"].Rows.Count > 0)
                        {
                            Storeds.Tables["Response"].Rows.Add("1", "");
                        }
                        SqlCon.Close();
                        return Storeds;
                    }

                }
            }
            catch (Exception ex)
            {
                Storeds.Tables["Respone"].Rows.Add("0", ex.Message);
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                   
                }                
                return Ticketsds;
            }
        }
        [WebMethod]
        public DataSet GetTransactionsInfo()
        {
            DataSet Transactionds = new DataSet();            

            try
            {
                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");


                Transactionds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            Transactionds.Tables["Response"].Rows.Add("1");
                            return Transactionds;
                        }
                        else
                        {
                            Transactionds.Tables["Response"].Rows.Add("0");
                        }
                        SqlCon.Close();
                    }
                    return Transactionds;
                }
            }
            catch (Exception ex)
            {
                Transactionds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Transactionds;
            }
        }
        [WebMethod]
        public DataSet GetTransactionInfo(string UserId)
        {
            DataSet Transactionds = new DataSet();

            try
            {
                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");


                Transactionds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            Transactionds.Tables["Response"].Rows.Add("1");
                            return Transactionds;
                        }
                        else
                        {  
                            Transactionds.Tables["Response"].Rows.Add("0");
                        }
                        SqlCon.Close();
                    }
                    return Transactionds;
                }
            }
            catch (Exception ex)
            {
                Transactionds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Transactionds;
            }
        }


        
        [WebMethod]
        public DataSet GetResultsInfo(string UserId)
        {
            DataSet Transactionds = new DataSet();

            try
            {
                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");


                Transactionds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            Transactionds.Tables["Response"].Rows.Add("1");
                            return Transactionds;
                        }
                        else
                        {
                            Transactionds.Tables["Response"].Rows.Add("0");
                        }
                        SqlCon.Close();
                    }
                    return Transactionds;
                }
            }
            catch (Exception ex)
            {
                Transactionds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Transactionds;
            }
        }


        [WebMethod]
        public DataSet GetReferralInfo(string UserId)
        {
            
            try
            {
                string ReferenceCode=null;
                //int Count = 0;
                Referralds = new DataSet();
                Referraldt = Referralds.Tables.Add("ReferralInfo");                
                Responsedt = Referralds.Tables.Add("Response");

                Referralds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd=new SqlCommand("SELECT ReferenceCode FROM UserInfo WHERE Email='"+UserId+"'",SqlCon))
                {
                    SqlCon.Open();
                    Sqldr = SqlCmd.ExecuteReader();
                    if(Sqldr.Read())
                    {
                        ReferenceCode = Sqldr.GetString(0);
                    }
                    SqlCon.Close();
                    SqlCmd.Dispose();
                }
               
                // chack level 1
                using (SqlCmd = new SqlCommand("SELECT FirstName,ReferenceCode FROM UserInfo Where ReferenceBy='" + ReferenceCode + "'", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Referralds, "ReferralInfo");
                        if (Referralds.Tables["ReferralInfo"].Rows.Count > 0)
                        {                            
                            Referralds.Tables.Remove("ReferralInfo");                            
                            Level1dt = Referralds.Tables.Add("Level1Info");
                            Sqlda.Fill(Referralds, "Level1dt");
                           
                            // check level 2
                            for (int i1 = 0; i1 < Referralds.Tables["Level1dt"].Rows.Count; i1++)
                            { 
                                using (SqlCmd = new SqlCommand("SELECT FirstName,ReferenceCode FROM UserInfo WHERE ReferenceBy='" + Referralds.Tables["Level1dt"].Rows[i1][1] + "'", SqlCon))
                                {
                                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                                    {
                                        Sqlda.Fill(Referralds, "ReferralInfo");
                                        if (Referralds.Tables["ReferralInfo"].Rows.Count > 0)
                                        {
                                            Referralds.Tables.Remove("ReferralInfo");
                                            Level2dt = Referralds.Tables.Add("Level2Info");
                                            Sqlda.Fill(Referralds, "Level2dt");
                                           

                                            //check level 3
                                            for (int i2 = 0; i2 < Referralds.Tables["Level2dt"].Rows.Count; i2++)
                                            {
                                                using (SqlCmd = new SqlCommand("SELECT FirstName,ReferenceCode FROM UserInfo WHERE ReferenceBy='" + Referralds.Tables["Level2dt"].Rows[i2][1] + "'", SqlCon))
                                                {
                                                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                                                    {
                                                        Sqlda.Fill(Referralds, "ReferralInfo");
                                                        if (Referralds.Tables["ReferralInfo"].Rows.Count > 0)
                                                        {
                                                            Referralds.Tables.Remove("ReferralInfo");
                                                            Level3dt = Referralds.Tables.Add("Level3Info");
                                                            Sqlda.Fill(Referralds, "Level3dt");
                                                           
                                                            //check level 4
                                                            for (int i3 = 0; i3 < Referralds.Tables["Level3dt"].Rows.Count; i3++)
                                                            {
                                                                using (SqlCmd = new SqlCommand("SELECT FirstName,ReferenceCode FROM UserInfo WHERE ReferenceBy='" + Referralds.Tables["Level3dt"].Rows[i3][1] + "'", SqlCon))
                                                                {
                                                                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                                                                    {
                                                                        Sqlda.Fill(Referralds, "ReferralInfo");
                                                                        if (Referralds.Tables["ReferralInfo"].Rows.Count > 0)
                                                                        {
                                                                            Referralds.Tables.Remove("ReferralInfo");
                                                                            Level4dt = Referralds.Tables.Add("Level4Info");
                                                                            Sqlda.Fill(Referralds, "Level4dt");
                                                                           

                                                                            //check level 5
                                                                            for (int i4 = 0; i4 < Referralds.Tables["Level4dt"].Rows.Count; i4++)
                                                                            {
                                                                                using (SqlCmd = new SqlCommand("SELECT FirstName,ReferenceCode FROM UserInfo WHERE ReferenceBy='" + Referralds.Tables["Level4dt"].Rows[i4][1] + "'", SqlCon))
                                                                                {
                                                                                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                                                                                    {
                                                                                        Sqlda.Fill(Referralds, "ReferralInfo");
                                                                                        if (Referralds.Tables["ReferralInfo"].Rows.Count > 0)
                                                                                        {
                                                                                            Referralds.Tables.Remove("ReferralInfo");
                                                                                            Level5dt = Referralds.Tables.Add("Level5Info");
                                                                                            Sqlda.Fill(Referralds, "Level5dt");
                                                                                            Referralds.Tables["Response"].Rows.Add("Level5");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Referralds.Tables["Response"].Rows.Add("Level4");
                                                                                        }

                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Referralds.Tables["Response"].Rows.Add("Level3");
                                                                        }

                                                                    }
                                                                }
                                                                  
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Referralds.Tables["Response"].Rows.Add("Level2");
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Referralds.Tables["Response"].Rows.Add("Level1");
                                        }

                                    }
                                }
                            }                           
                           
                        }
                        else
                        {
                            Referralds.Tables["Response"].Rows.Add("Level0");
                        }                       
                    }

                    Referralds.Tables.Remove("ReferralInfo");
                    return Referralds;
                }
            }
            catch (Exception ex)
            {
               
                Referralds.Tables["Response"].Columns.Add("Error", typeof(string));
                Referralds.Tables["Respone"].Rows.Add("0",ex.Message);
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                   
                }                
                return Referralds;
            }
        }
        



        // Calculation

        [WebMethod]
        public WebServiceResponse ReferralBounceCalculate(string UserId, double Amount)
        {
            DataSet Transactionds = new DataSet();
            wsr = new WebServiceResponse();

            try
            {

                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");

               
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            wsr.Status = "1";
                           
                        }                  
                        
                        SqlCon.Close();
                    }
                    return wsr;
                }
            }
            catch (Exception ex)
            {
                wsr.Status = "0";
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    
                }                
                return wsr;
            }
        }
    }
}
