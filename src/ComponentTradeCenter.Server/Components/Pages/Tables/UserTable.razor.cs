using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class UserTable : EntityTablePageBase<UserInfo>
    {
        protected override DbSet<UserInfo> DbSet => DbContext.UserInfos;

        protected override IQueryable<UserInfo> Items => (User switch
        {
            Administrator => DbSet,
            _ => throw new ArgumentException(nameof(User))
        }).AsNoTracking();

        protected Task<string> GetModelCustomId(object? d)
        {
            var ret = "";
            if (d is TableColumnContext<UserInfo, object?> odata && odata.Value is int id)
            {
                ret = ICustomIdProvider.GetCustomId(id, odata.Row.UserType);
            }
            return Task.FromResult(ret);
        }
    }
}
