using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Base
{
    public abstract class UserBase : ModelBase
    {
        [Required(ErrorMessage = "{0}不得为空")]
        [StringLength(16, ErrorMessage = "长度不得超过16")]
        public string Password { get; set; }

        [StringLength(15, ErrorMessage = "长度不得超过15")]
        public string? Phone { get; set; }

        public DateTime RegisterTime { get; set; }

        public UserBase()
        {
            Password = "";
        }

        public override bool Equals(object? obj)
        {
            if (obj is UserBase other)
            {
                return other.Id == Id && ModelType == other.ModelType;
            }
            return false;
        }

        public override int GetHashCode()
            => HashCode.Combine(Id, ModelType);
    }
}
