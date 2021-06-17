using E_Invoice_API.Common.Enums;
using E_Invoice_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace E_Invoice_API.Data.EntityConfiguration
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ChangeDateTime)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(x => x.ToCheckFirstActivationDateTime)
                .HasColumnType("datetime2(0)"); 

            builder.HasIndex(x => x.ToCheckFirstActivationDateTime)
                .HasDatabaseName("Index_UserFirstTimeSubscription")
                .HasFilter("[ToCheckFirstActivationDateTime] IS NOT NULL");

            var converter = new ValueConverter<EnumInvoiceStatus, char>(
                v => FromEnum(v),
                v => FromChar(v));

            builder.Property(x => x.PreviousStatus)
                .HasColumnType("char(1)")
                .HasConversion(converter)
                .IsRequired();

            builder.Property(x => x.CurrentStatus)
                .HasColumnType("char(1)")
                .HasConversion(converter)
                .IsRequired();

            builder.Property(x => x.InvoiceStatusId)
                .IsRequired();

            builder.HasOne(x => x.InvoiceStatus)
                .WithMany(x => x.UserLogs)
                .HasForeignKey(x => x.InvoiceStatusId)
                .OnDelete(DeleteBehavior.Restrict); 
        }

        private char FromEnum(EnumInvoiceStatus v)
        {
            switch (v)
            {
                case EnumInvoiceStatus.Active:
                    return 'A';
                case EnumInvoiceStatus.Inactive:
                    return 'I';
                case EnumInvoiceStatus.FirstActivation:
                    return 'F';
                case EnumInvoiceStatus.Nonexist:
                    return 'N';
                default:
                    throw new System.Exception("No conversion for given type");
            }
        }

        private EnumInvoiceStatus FromChar(char v)
        {
            switch (v)
            {
                case 'A':
                    return EnumInvoiceStatus.Active;
                case 'I':
                    return EnumInvoiceStatus.Inactive;
                case 'F':
                    return EnumInvoiceStatus.FirstActivation;
                case 'N':
                    return EnumInvoiceStatus.Nonexist;
                default:
                    throw new System.Exception("No conversion for given type");
            }
        }
    }
}
