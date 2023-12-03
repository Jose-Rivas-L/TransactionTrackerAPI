using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TransactionTrackerAPI.models;
using TransactionTrackerAPI.Resources;

namespace TransactionTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public LogToFile logging = new LogToFile();
        public WebhookController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult HandleWebhook([FromBody] PaymentNotification webhookData)
        {
            try
            {
                int requestId = webhookData.RequestId;
                string payerName = webhookData.Request.Payer.Name;
                string payerSurname = webhookData.Request.Payer.Surname;
                string payerDocument = webhookData.Request.Payer.Document;
                string payerEmail = webhookData.Request.Payer.Email;
                string payerMobile = webhookData.Request.Payer.Mobile;
                string paymentStatus = webhookData.Payment[0].Status.Status;
                string paymentMessage = webhookData.Payment[0].Status.Message;
                DateTime paymentDate = webhookData.Payment[0].Status.Date;
                int paymentAmount = webhookData.Request.Payment.Amount.Total;
                string paymentBank = webhookData.Payment[0].IssuerName;
                string paymentMethod = webhookData.Payment[0].PaymentMethod;
                DateTime notificationDate = webhookData.Status.Date;
                int notificationId = 0;
                using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("connection")))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand("InsertNotification", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Ajustar los nombres de los parámetros según el procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.Int) { Value = requestId });
                        command.Parameters.Add(new SqlParameter("@Document", SqlDbType.NVarChar, 50) { Value = payerDocument });
                        command.Parameters.Add(new SqlParameter("@PayerName", SqlDbType.NVarChar, 100) { Value = payerName });
                        command.Parameters.Add(new SqlParameter("@PayerSurname", SqlDbType.NVarChar, 100) { Value = payerSurname });
                        command.Parameters.Add(new SqlParameter("@PayerEmail", SqlDbType.NVarChar, 100) { Value = payerEmail });
                        command.Parameters.Add(new SqlParameter("@PayerMobile", SqlDbType.NVarChar, 20) { Value = payerMobile });
                        command.Parameters.Add(new SqlParameter("@PaymentStatus", SqlDbType.NVarChar, 50) { Value = paymentStatus });
                        command.Parameters.Add(new SqlParameter("@PaymentMessage", SqlDbType.NVarChar, 255) { Value = paymentMessage });
                        command.Parameters.Add(new SqlParameter("@PaymentDate", SqlDbType.DateTime) { Value = paymentDate });
                        command.Parameters.Add(new SqlParameter("@PaymentBank", SqlDbType.NVarChar, 100) { Value = paymentBank });
                        command.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50) { Value = paymentMethod });
                        command.Parameters.Add(new SqlParameter("@NotificationDate", SqlDbType.DateTime) { Value = notificationDate });

                        var notificationIdParameter = new SqlParameter("@NotificationId", SqlDbType.Int);
                        notificationIdParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(notificationIdParameter);

                        command.ExecuteNonQuery();

                        notificationId = (int)notificationIdParameter.Value;
                    }
                }
                return Ok(new
                {
                    success = true,
                    message = "Notification saved",
                    result = new
                    {
                        requestId,
                        notificationId
                    }
                });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error saving notification",
                    error = $"SQL Error: {sqlEx.Message}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal Server Error",
                    error = $"Error handling webhook: {ex.Message}"
                });
            }
        }
    }
}
