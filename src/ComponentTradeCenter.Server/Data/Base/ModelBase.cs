using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Base
{
    public abstract class ModelBase : EntityBase
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}不得为空")]
        [StringLength(64, ErrorMessage = "长度不得超过64")]
        public string Name { get; set; }

        public DateTime? DeleteTime { get; set; }

        public abstract ModelType ModelType { get; }

        public ModelBase()
        {
            Name = "";
        }
    }
}
