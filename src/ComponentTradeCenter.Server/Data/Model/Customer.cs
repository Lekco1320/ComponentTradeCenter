using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Customer : UserBase, ICustomIdProvider
    {
        [StringLength(128, ErrorMessage = "长度不得超过128")]
        public string? Address { get; set; }

        public override ModelType ModelType => ModelType.Customer;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"CUS{id:D4}";
    }

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(64).IsRequired();
            builder.Property(c => c.Password).HasMaxLength(16).IsRequired();
            builder.Property(c => c.Phone).HasMaxLength(15);
            builder.Property(c => c.Address).HasMaxLength(128);
            builder.Property(c => c.RegisterTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
