using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Supply : ModelBase, ICustomIdProvider
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = "{0}不得为空")]
        public int SupplierId { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "总价不得小于￥0.00")]
        [Required(ErrorMessage = "{0}不得为空")]
        public decimal Price { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "数量不得小于0")]
        [Required(ErrorMessage = "{0}不得为空")]
        public int Amount { get; set; }

        public override ModelType ModelType => ModelType.Supply;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"SPY{id:D4}";
    }

    public class SupplyConfiguration : IEntityTypeConfiguration<Supply>
    {
        public void Configure(EntityTypeBuilder<Supply> builder)
        {
            builder.ToTable("Supplies");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(64).IsRequired();
            builder.Property(s => s.Price).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(s => s.Amount).IsRequired();
        }
    }
}
