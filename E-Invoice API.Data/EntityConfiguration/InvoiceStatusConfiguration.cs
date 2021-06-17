using E_Invoice_API.Common.Enums;
using E_Invoice_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace E_Invoice_API.Data.EntityConfiguration
{
    public class InvoiceStatusConfiguration : IEntityTypeConfiguration<InvoiceStatus>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatus> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            var converter = new ValueConverter<EnumInvoiceStatus, char>(
                v => FromEnum(v),
                v => FromChar(v));

            builder.Property(x => x.Status)
                .HasColumnType("char(1)")
                .HasConversion(converter)
                .IsRequired();

            builder.HasMany(x => x.UserLogs)
                .WithOne(y => y.InvoiceStatus)
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
