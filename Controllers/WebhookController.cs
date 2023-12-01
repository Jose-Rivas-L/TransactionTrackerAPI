using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
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
        [Route("save")]
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


                // Save the info in database
                Console.WriteLine($"Request ID: {requestId}");               
                Console.WriteLine($"Payer Document: {payerDocument}");
                Console.WriteLine($"Payer Name: {payerName}");
                Console.WriteLine($"Payer Surname: {payerSurname}");
                Console.WriteLine($"Payer Email: {payerEmail}");
                Console.WriteLine($"Payer Mobile: {payerMobile}");
                Console.WriteLine($"Payment status: {paymentStatus}");
                Console.WriteLine($"Payment Message: {paymentMessage}");
                Console.WriteLine($"Payment Date: {paymentDate}");
                Console.WriteLine($"Payment Amount: {paymentAmount}");
                Console.WriteLine($"Payment Bank: {paymentBank}");
                Console.WriteLine($"Payment Method: {paymentMethod}");



                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el procesamiento
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }


}
