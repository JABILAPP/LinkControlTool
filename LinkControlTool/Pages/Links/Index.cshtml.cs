using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient; 

namespace LinkControlTool.Pages.Links
{
    public class IndexModel : PageModel
    {
        public List<LinkInfo> ListLink = new List<LinkInfo>();
        public List<Status> ListStatus= new List<Status>();
        public void OnGet()
        {
            try
            {
                String ConnectionString = "Data Source=BRMANM0SQL01;Initial Catalog=MESLogs ;Persist Security Info=True;User ID=app_Colaboradores;Password=base!colab; Connection Timeout=0";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    String Sql = "select [Imei],[Status],[LinkDate] from dbo.uploadNumber ";
                    String GetUsed = "select count(*) from dbo.uploadNumber where Status = 1";
                    //String GetNotUsed = "select count(*) from dbo.uploadNumber where Status = 0";

                    using (SqlCommand command = new SqlCommand(GetUsed, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Status status = new Status();

                                status.Used= reader.GetInt32(0); 

                            }
                        }

                    }

                    using (SqlCommand command = new SqlCommand(Sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LinkInfo LinkLc = new LinkInfo();
                                LinkLc.Imei = reader.GetString(0);
                                LinkLc.Status = reader.GetBoolean(1);


                                if (!reader.IsDBNull(2))
                                {
                                    LinkLc.LinkDate = reader.GetString(2);
                                }
                                else
                                {
                                    LinkLc.LinkDate += "Not Linked";
                                }




                                ListLink.Add(LinkLc);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Execption" + ex.ToString()); 

            }



        }
    }

    public class LinkInfo
    {
        public String Imei;
        public Boolean Status;
        public String LinkDate;
    }
    public class Status
    {
        public int Avaible { get; set; }
        public int Used { get; set; }
    }
        
}
