using E_Invoice_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Invoice_API.Data.EntityConfiguration
{
    public class MailHistoryConfiguration : IEntityTypeConfiguration<MailHistory>
    {
        public void Configure(EntityTypeBuilder<MailHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.MailSendToEmail)
                .IsRequired();

            builder.Property(x => x.SendMailDateTime)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(x => x.EmailTemplate)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserMailHistory)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
