using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using TransactionTrackerAPI.models;
using TransactionTrackerAPI.Resources;

namespace TransactionTrackerAPI.Controllers
{
    [ApiController]
    [Route("Notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly LogToFile logging;

        public NotificationController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.logging = new LogToFile();
        }

        [HttpGet]
        [Route("All")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<Notification> notifications = await GetNotificationsAsync("GetNotifications");

                logging.Log($"Notifications retrieved:\nrecords: {notifications.Count()}");
                return Ok(new
                {
                    success = true,
                    message = "Notifications retrieved",
                    result = notifications
                });
            }
            catch (Exception ex)
            {
                return LogAndHandleError("Error getting notifications", ex);
            }
        }

        [HttpGet]
        [Route("ById")]
        public async Task<ActionResult> GetById(int IdNotification)
        {
            try
            {
                List<Notification> notifications = await GetNotificationsAsync("GetNotificationById", IdNotification);

                logging.Log($"Notification retrieved:\nrecords: {notifications.Count()}");
                return Ok(new
                {
                    success = true,
                    message = "Notification retrieved",
                    result = notifications
                });
            }
            catch (Exception ex)
            {
                return LogAndHandleError("Error getting notification", ex);
            }
        }

        private async Task<List<Notification>> GetNotificationsAsync(string storedProcedureName, int? id = null)
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("connection")))
            {
                await conn.OpenAsync();

                using (SqlCommand command = new SqlCommand(storedProcedureName, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (id.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@Idnotification", SqlDbType.Int) { Value = id.Value });
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var notification = MapNotificationFromReader(reader);
                            notifications.Add(notification);
                        }
                    }
                }
            }

            return notifications;
        }

        private Notification MapNotificationFromReader(SqlDataReader reader)
        {
            return new Notification
            {
                IdNotification = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                RequestId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                Document = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                PayerName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                PayerSurname = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                PayerEmail = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                PayerMobile = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                PaymentStatus = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                PaymentMessage = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                PaymentDate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                PaymentBank = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                PaymentMethod = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                NotificationDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12)
            };
        }

        private ActionResult LogAndHandleError(string logMessage, Exception ex)
        {
            logging.Log($"{logMessage}:\n{ex.Message}");

            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

}
