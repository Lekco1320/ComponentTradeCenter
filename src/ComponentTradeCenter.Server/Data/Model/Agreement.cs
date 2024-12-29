using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentTradeCenter.Server.Data.Model
{
    public class Agreement : ModelBase, ICustomIdProvider
    {
        public int SuggestId { get; set; }

        public byte[]? CustomerSignature { get; set; }

        public byte[]? SupplierSignature { get; set; }

        public bool? TraderIntention { get; set; }

        public override ModelType ModelType => ModelType.Agreement;

        public string CustomId => GetCustomId(Id);

        public static string GetCustomId(int? id) => $"ARG{id:D4}";
    }

    public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
    {
        public void Configure(EntityTypeBuilder<Agreement> builder)
        {
            builder.ToTable("Agreements");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(64).IsRequired();
        }
    }
}
