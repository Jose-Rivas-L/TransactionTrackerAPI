using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
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
        public async Task<ActionResult> HandleWebhook([FromBody] PaymentNotification webhookData)
        {
            try
            {
                var (notificationId, requestId) = await InsertNotificationAsync(webhookData);

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

        private async Task<(int notificationId, int requestId)> InsertNotificationAsync(PaymentNotification webhookData)
        {
            int notificationId = 0;
            int requestId = webhookData.RequestId;

            using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("connection")))
            {
                await conn.OpenAsync();

                using (SqlCommand command = new SqlCommand("InsertNotification", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SetNotificationParameters(command.Parameters, webhookData);

                    var notificationIdParameter = new SqlParameter("@NotificationId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(notificationIdParameter);

                    await command.ExecuteNonQueryAsync();

                    notificationId = (int)notificationIdParameter.Value;
                }
            }

            return (notificationId, requestId);
        }

        private void SetNotificationParameters(SqlParameterCollection parameters, PaymentNotification webhookData)
        {
            parameters.Add("@RequestId", SqlDbType.Int).Value = webhookData.RequestId;
            parameters.Add("@Document", SqlDbType.NVarChar, 50).Value = webhookData.Request.Payer.Document;
            parameters.Add("@PayerName", SqlDbType.NVarChar, 100).Value = webhookData.Request.Payer.Name;
            parameters.Add("@PayerSurname", SqlDbType.NVarChar, 100).Value = webhookData.Request.Payer.Surname;
            parameters.Add("@PayerEmail", SqlDbType.NVarChar, 100).Value = webhookData.Request.Payer.Email;
            parameters.Add("@PayerMobile", SqlDbType.NVarChar, 20).Value = webhookData.Request.Payer.Mobile;
            parameters.Add("@PaymentStatus", SqlDbType.NVarChar, 50).Value = webhookData.Payment[0].Status.Status;
            parameters.Add("@PaymentMessage", SqlDbType.NVarChar, 255).Value = webhookData.Payment[0].Status.Message;
            parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = webhookData.Payment[0].Status.Date;
            parameters.Add("@PaymentBank", SqlDbType.NVarChar, 100).Value = webhookData.Payment[0].IssuerName;
            parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = webhookData.Payment[0].PaymentMethod;
            parameters.Add("@NotificationDate", SqlDbType.DateTime).Value = webhookData.Status.Date;
        }
    }

}
