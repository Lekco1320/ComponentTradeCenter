using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Supplier : UserBase, ICustomIdProvider
    {
        [StringLength(128, ErrorMessage = "长度不得超过128")]
        public string? Address { get; set; }

        [StringLength(255, ErrorMessage = "长度不得超过255")]
        public string? Brief { get; set; }

        public override ModelType ModelType => ModelType.Supplier;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"SPR{id:D4}";
    }

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(64).IsRequired();
            builder.Property(s => s.Password).HasMaxLength(16).IsRequired();
            builder.Property(s => s.Phone).HasMaxLength(15);
            builder.Property(s => s.Address).HasMaxLength(128);
            builder.Property(s => s.Brief).HasMaxLength(255);
            builder.Property(s => s.RegisterTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
