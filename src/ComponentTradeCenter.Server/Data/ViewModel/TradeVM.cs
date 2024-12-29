using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class TradeVM : ViewModelBase<Trade>
    {
        [Required(ErrorMessage = "{0}不得为空")]
        public int AgreementId { get; set; }

        public DateTime CompleteTime { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string ComponentName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int CustomerId { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string NeedName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int SupplierId { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string SupplyName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int TraderId { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string TraderName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public decimal Price { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int Amount { get; set; }

        public TradeVM()
        {
            ComponentName = "";
            NeedName = "";
            SupplyName = "";
            TraderName = "";
        }
    }

    public class TradeVMConfiguration : IEntityTypeConfiguration<TradeVM>
    {
        public void Configure(EntityTypeBuilder<TradeVM> builder)
        {
            builder.ToView("V_Trades");
            builder.HasKey(t => t.Id);
        }
    }
}
