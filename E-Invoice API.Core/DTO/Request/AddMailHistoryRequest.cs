
namespace E_Invoice_API.Core.DTO.Request
{
    public class AddMailHistoryRequest
    {
        public int UserId { get; set; }
        public string MailSendToEmail { get; set; }
        public string EmailTemplate { get; set; }
    }
}
