using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;


namespace QR_QSIS_SP_SQL_Timer
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Catch Errors */
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = "prodnhsesql101";
                builder["Initial Catalog"] = "NHSE_Sandbox_DC";
                builder["Integrated Security"] = true;

                using (SqlConnection connector = new SqlConnection(builder.ConnectionString))
                {
                    connector.Open();
                    String SQL_instruction = "EXEC NHSE_Sandbox_DC.[DBO].QR_SP_Maternity_SE_AH";

                    using (SqlCommand command = new SqlCommand(SQL_instruction, connector))
                    {
                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                String ErrorMessage = e.ToString();
                using StreamWriter Error = new StreamWriter(@"Status\QSIS_Error-Message.txt");
                Error.WriteLineAsync(ErrorMessage);
            }
            _ = Program.Status();
        }

        public static async Task Status()
        {
            String Message = "Confirmation that your SQL QSIS SP has completed it's tasks at" + DateTime.Now + " with no problems";
            using StreamWriter notification = new StreamWriter(@"Status\QSIS_Status-Message.txt");
            await notification.WriteLineAsync(Message);


        }
        public static async Task SendMessage()
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("hirstarh@gmail.com", "vbjh vynd enrk pqdt"),
                EnableSsl = true,
            };
            smtpClient.Send("hirstarh@gmail.com", "hirstarh@gmail.com", "System Notification - QSIS", "Confirmation that the QSIS table has been updated at" + DateTime.Now);
        }
    }
}

