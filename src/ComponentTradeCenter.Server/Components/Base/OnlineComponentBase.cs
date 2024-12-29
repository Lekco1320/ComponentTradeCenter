using ComponentTradeCenter.Server.Components.Shared;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Base
{
    public class OnlineComponentBase : ComponentBase
    {
        [Inject]
        [NotNull]
        protected CenterDbContext? DbContext { get; set; }

        [Inject]
        [NotNull]
        protected NavigationManager? NavigationManager { get; set; }

        [Inject]
        [NotNull]
        protected OnlineUsersService? OnlineUsersService { get; set; }

        [CascadingParameter]
        [NotNull]
        public MainLayout? MainLayOut { get; set; }

        [NotNull]
        public UserBase? User { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            if (queryParams.TryGetValue("customid", out var customId) &&
                DbContext.TryGetUser(customId, out var user) &&
                OnlineUsersService.IsOnline(user))
            {
                User = user;
                MainLayOut.User = user;
                MainLayOut.UpdateLayout();
                return;
            }

            NavigationManager.NavigateTo("/error?info=Not%20Found");
        }
    }
}
