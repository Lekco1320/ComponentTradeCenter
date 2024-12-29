using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Base
{
    public abstract class ViewModelBase<TModel> : EntityBase, ICustomIdProvider where TModel : ModelBase, ICustomIdProvider
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}不得为空")]
        [StringLength(64, ErrorMessage = "长度不得超过64")]
        public string Name { get; set; }

        public virtual string CustomId
            => TModel.GetCustomId(Id);

        public static string GetCustomId(int? id)
            => TModel.GetCustomId(id);

        public ViewModelBase()
        {
            Name = "";
        }
    }
}
