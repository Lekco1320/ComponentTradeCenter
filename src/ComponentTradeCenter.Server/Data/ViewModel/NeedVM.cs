using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class NeedVM : ViewModelBase<Need>
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

        [ReadOnly(true)]
        [Editable(false)]
        public string CustomerName { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public string ComponentName { get; set; }

        public NeedVM()
        {
            CustomerName = "";
            ComponentName = "";
        }
    }

    public class NeedVMConfiguration : IEntityTypeConfiguration<NeedVM>
    {
        public void Configure(EntityTypeBuilder<NeedVM> builder)
        {
            builder.ToView("V_Needs");
            builder.HasKey(x => x.Id);
        }
    }
}
