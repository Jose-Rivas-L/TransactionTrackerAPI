using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Transactions;
using System.Xml.Linq;
using TransactionTrackerAPI.models;

namespace TransactionTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public WebhookController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult HandleWebhook(PaymentNotification webhookData)
        {
            try
            {
                int requestId = webhookData.RequestId;
                string payerName = webhookData.Request.Payer.Name;
                string payerSurname = webhookData.Request.Payer.Surname;
                string payerDocument = webhookData.Request.Payer.Document;
                string payerEmail = webhookData.Request.Payer.Email;
                string payerMobile = webhookData.Request.Payer.Mobile;
                string paymentStatus = webhookData.Payments[0].Status.Status;
                string paymentMessage = webhookData.Payments[0].Status.Message;
                DateTime paymentDate = webhookData.Payments[0].Status.Date;
                int paymentAmount = webhookData.Payments[0].Amount.Total;
                string paymentBank = webhookData.Payments[0].IssuerName;
                string paymentMethod = webhookData.Payments[0].PaymentMethod;
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
                        // ... (otros campos),
                        notificationId
                    }
                });
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el procesamiento
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }


}
