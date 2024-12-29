using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class TradableInfo : EntityBase
    {
        public int ComponentId { get; set; }

        public string ComponentName { get; set; }

        public int NeedId { get; set; }

        public string NeedName { get; set; }

        public decimal NeedPrice { get; set; }

        public int NeedAmount { get; set; }

        public int SupplyId { get; set; }

        public string SupplyName { get; set; }

        public decimal SupplyPrice { get; set; }

        public int SupplyAmount { get; set; }

        public TradableInfo()
        {
            NeedName = "";
            SupplyName = "";
            ComponentName = "";
        }
    }

    public class TradableInfoConfiguration : IEntityTypeConfiguration<TradableInfo>
    {
        public void Configure(EntityTypeBuilder<TradableInfo> builder)
        {
            builder.ToView("V_TradableInfos");
            builder.HasKey(x => x.Id);
        }
    }
}
