using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Trader : UserBase, ICustomIdProvider
    {
        [StringLength(255, ErrorMessage = "长度不得超过255")]
        public string? Brief { get; set; }

        public override ModelType ModelType => ModelType.Trader;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"TDR{id:D4}";
    }

    public class TraderConfiguration : IEntityTypeConfiguration<Trader>
    {
        public void Configure(EntityTypeBuilder<Trader> builder)
        {
            builder.ToTable("Traders");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(64).IsRequired();
            builder.Property(t => t.Password).HasMaxLength(16).IsRequired();
            builder.Property(t => t.Phone).HasMaxLength(15);
            builder.Property(t => t.Brief).HasMaxLength(255);
            builder.Property(t => t.RegisterTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
