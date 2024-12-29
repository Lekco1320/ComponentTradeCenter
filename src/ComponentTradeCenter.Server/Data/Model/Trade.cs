using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Trade : ModelBase, ICustomIdProvider
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public int AgreementId { get; set; }

        public DateTime CompleteTime { get; set; }

        public override ModelType ModelType => ModelType.Trade;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"TRD{id:D4}";
    }

    public class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.ToTable("Trades");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(64).IsRequired();
            builder.Property(a => a.CompleteTime).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        }
    }
}
