using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String erroMessage = "";
        public string successmessage = "";
        
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.address.Length == 0 || clientInfo.name.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.email.Length == 0)
            {
                erroMessage = "All the fields are required";
                return;
            }

            // Save client in DataBase

            try
            {
                String conectionString = "Data Source=.\\sqlexpress;Initial Catalog=MyStore;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(conectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients" + "(name,email,phone,address) VALUES" + "(@name, @email,@phone,@address);";

                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue ("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address",clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                erroMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successmessage = "New Client Added Correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}
