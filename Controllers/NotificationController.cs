﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TransactionTrackerAPI.Controllers
{
    [ApiController]
    [Route("Notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public NotificationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("All")]
        public ActionResult GetAll() {
            try
            {
                List<Object> notifications = new List<Object>();
                using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("connection")))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand("GetNotifications", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var notification = new
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

                                notifications.Add(notification);
                                Console.WriteLine(notification);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = true,
                        message = "Notifications retrieved",
                        result = notifications
                    });
                }   
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el procesamiento
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
    }
}
