using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class TradeSuggest : ModelBase, ICustomIdProvider
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public int NeedId { get; set; }

        [Required(ErrorMessage = "{0}不得为空")]
        public int SupplyId { get; set; }

        [Required(ErrorMessage = "{0}不得为空")]
        public int TraderId { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "报价不得小于￥0.00")]
        [Required(ErrorMessage = "{0}不得为空")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "数量不得小于0")]
        [Required(ErrorMessage = "{0}不得为空")]
        public int Amount { get; set; }

        public bool? CustomerIntention { get; set; }

        public bool? SupplierIntention { get; set; }

        public bool? TraderIntention { get; set; }

        public override ModelType ModelType => ModelType.TradeSuggest;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"SUG{id:D4}";
    }

    public class TradeSuggestConfiguration : IEntityTypeConfiguration<TradeSuggest>
    {
        public void Configure(EntityTypeBuilder<TradeSuggest> builder)
        {
            builder.ToTable("TradeSuggests");
            builder.HasKey(ts => ts.Id);
            builder.Property(ts => ts.Name).HasMaxLength(64).IsRequired();
            builder.Property(ts => ts.Price).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(ts => ts.Amount).IsRequired();
        }
    }
}
