using System;
using System.Collections.Generic;
using System.Text;

namespace E_Invoice_API.Data.Models
{
    public class MailHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string MailSendToEmail { get; set; }
        public DateTime SendMailDateTime { get; set; }
        public string EmailTemplate { get; set; }
    }
}
