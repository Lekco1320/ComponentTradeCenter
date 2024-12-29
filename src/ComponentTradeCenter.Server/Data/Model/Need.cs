using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Need : ModelBase, ICustomIdProvider
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = "{0}不得为空")]
        public int CustomerId { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "预算不得小于￥0.00")]
        [Required(ErrorMessage = "{0}不得为空")]
        public decimal Price { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "数量不得小于0")]
        [Required(ErrorMessage = "{0}不得为空")]
        public int Amount { get; set; }

        public override ModelType ModelType => ModelType.Need;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"NED{id:D4}";
    }

    public class NeedConfiguration : IEntityTypeConfiguration<Need>
    {
        public void Configure(EntityTypeBuilder<Need> builder)
        {
            builder.ToTable("Needs");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Name).HasMaxLength(64).IsRequired();
            builder.Property(n => n.Price).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(n => n.Amount).IsRequired();
        }
    }
}
