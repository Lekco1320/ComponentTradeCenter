using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Base
{
    public abstract class EntityBase
    {
        [ReadOnly(true)]
        [Display(Name = "ID")]
        public int Id { get; set; }
    }
}
