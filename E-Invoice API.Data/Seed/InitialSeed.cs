using E_Invoice_API.Common.Enums;
using E_Invoice_API.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace E_Invoice_API.Data.Seed
{
    public class InitialSeed
    {
        private const string usersPassword = "haslo1";
        private const string notificationEmailTemplate = "NotificationTemplate";
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            #region User

            var hasher = new PasswordHasher<User>();

            var user1 = new User
            {
                FirstName = "Janusz",
                LastName = "Tracz",
                UserName = "jtracz@gmail.com",
                NormalizedUserName = "JTRACZ@GMAIL.COM",
                Email = "jtracz@gmail.com",
                NormalizedEmail = "JTRACZ@GMAIL.COM",
                PhoneNumber = "+48 213321123",
                PasswordHash = hasher.HashPassword(null, usersPassword),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var user2 = new User
            {
                FirstName = "Adam",
                LastName = "Kowalik",
                UserName = "adamkowalik@onet.pl",
                NormalizedUserName = "ADAMKOWALIK@ONET.PL",
                Email = "adamkowalik@onet.pl",
                NormalizedEmail = "ADAMKOWALIK@ONET.PL",
                PhoneNumber = "+48 251341333",
                PasswordHash = hasher.HashPassword(null, usersPassword),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var rUser1 = context.User.Add(user1);
            var rUser2 = context.User.Add(user2);
            context.SaveChanges();

            #endregion

            #region MailHistory

            var mailHistory1 = new MailHistory
            {
                UserId = rUser1.Entity.Id,
                MailSendToEmail = rUser1.Entity.Email,
                SendMailDateTime = DateTime.Parse("01/01/2020 11:00"),
                EmailTemplate = notificationEmailTemplate
            };

            var mailHistory2 = new MailHistory
            {
                UserId = rUser2.Entity.Id,
                MailSendToEmail = rUser2.Entity.Email,
                SendMailDateTime = DateTime.Parse("02/02/2020 22:00"),
                EmailTemplate = notificationEmailTemplate
            };

            var rMailHistory1 = context.MailHistory.Add(mailHistory1);
            var rMailHistory2 = context.MailHistory.Add(mailHistory2);
            context.SaveChanges();

            #endregion

            #region InvoiceStatus

            var invoiceStatus1 = new InvoiceStatus
            {
                UserId = rUser1.Entity.Id,
                Status = EnumInvoiceStatus.Active
            };

            var invoiceStatus2 = new InvoiceStatus
            {
                UserId = rUser2.Entity.Id,
                Status = EnumInvoiceStatus.FirstActivation
            };

            var rInvoiceStatus1 = context.InvoiceStatus.Add(invoiceStatus1);
            var rInvoiceStatus2 = context.InvoiceStatus.Add(invoiceStatus2);
            context.SaveChanges();

            #endregion

            #region Log

            var log1 = new Log
            {
                ChangeDateTime = DateTime.Parse("07/05/2020 19:00"),
                ToCheckFirstActivationDateTime = null,
                PreviousStatus = EnumInvoiceStatus.Active,
                CurrentStatus = EnumInvoiceStatus.Inactive,
                InvoiceStatusId = rInvoiceStatus1.Entity.Id
            };

            var log2 = new Log
            {
                ChangeDateTime = DateTime.Parse("09/05/2020 19:00"),
                ToCheckFirstActivationDateTime = null,
                PreviousStatus = EnumInvoiceStatus.Inactive,
                CurrentStatus = EnumInvoiceStatus.Active,
                InvoiceStatusId = rInvoiceStatus1.Entity.Id
            };

            var log3 = new Log
            {
                ChangeDateTime = DateTime.Parse("03/03/2020 13:00"),
                ToCheckFirstActivationDateTime = DateTime.Parse("03/03/2020 13:00"),
                PreviousStatus = EnumInvoiceStatus.Nonexist,
                CurrentStatus = EnumInvoiceStatus.FirstActivation,
                InvoiceStatusId = rInvoiceStatus2.Entity.Id
            };


            var rLog1 = context.Log.Add(log1);
            var rLog2 = context.Log.Add(log2);
            var rLog3 = context.Log.Add(log3);
            context.SaveChanges();

            #endregion

        }
    }
}
