using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentTradeCenter.Server.Data.ViewModel
{
    public class UserInfo : ModelBase
    {
        public int UserId { get; set; }

        public string? Phone { get; set; }

        public DateTime RegisterTime { get; set; }

        public ModelType UserType { get; set; }

        public override ModelType ModelType => UserType;
    }

    public class UserVMConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToView("V_UserInfos");
            builder.HasKey(u => u.Id);
        }
    }
}
