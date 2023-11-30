using Microsoft.AspNetCore.Mvc;
using TransactionTrackerAPI.models;

namespace TransactionTrackerAPI.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveNotification([FromBody] PaymentNotification notification)
        {
            // Lógica para almacenar la notificación en la base de datos
            _context.NotificationRequests.Add(notification);
            _context.SaveChanges();

            return Ok();
        }
    }
}
