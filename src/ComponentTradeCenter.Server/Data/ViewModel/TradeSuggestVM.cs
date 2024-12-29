using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class TradeSuggestVM : ViewModelBase<TradeSuggest>
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

        [ReadOnly(true)]
        [Editable(false)]
        public string NeedName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string SupplyName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int CustomerId { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int SupplierId { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string TraderName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string ComponentName { get; set; }

        public TradeSuggestVM()
        {
            NeedName = "";
            SupplyName = "";
            TraderName = "";
            ComponentName = "";
        }
    }

    public class TradeSuggestVMConfiguration : IEntityTypeConfiguration<TradeSuggestVM>
    {
        public void Configure(EntityTypeBuilder<TradeSuggestVM> builder)
        {
            builder.ToView("V_TradeSuggests");
            builder.HasKey(s => s.Id);
        }
    }
}
