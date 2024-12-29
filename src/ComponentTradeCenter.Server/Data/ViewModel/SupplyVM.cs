using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class SupplyVM : ViewModelBase<Supply>
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

        [ReadOnly(true)]
        [Editable(false)]
        public string SupplierName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string ComponentName { get; set; }

        public SupplyVM()
        {
            SupplierName = "";
            ComponentName = "";
        }
    }

    public class SupplyVMConfiguration : IEntityTypeConfiguration<SupplyVM>
    {
        public void Configure(EntityTypeBuilder<SupplyVM> builder)
        {
            builder.ToView("V_Supplies");
            builder.HasKey(s => s.Id);
        }
    }
}
