using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class AgreementVM : ViewModelBase<Agreement>
    {
        public int SuggestId { get; set; }

        public byte[]? CustomerSignature { get; set; }

        public byte[]? SupplierSignature { get; set; }

        public bool? TraderIntention { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public decimal Price { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public int Amount { get; set; }

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

        public AgreementVM()
        {
            ComponentName = "";
            NeedName = "";
            SupplyName = "";
            TraderName = "";
        }
    }

    public class AgreementVMConfiguration : IEntityTypeConfiguration<AgreementVM>
    {
        public void Configure(EntityTypeBuilder<AgreementVM> builder)
        {
            builder.ToView("V_Agreements");
            builder.HasKey(a => a.Id);
        }
    }
}
