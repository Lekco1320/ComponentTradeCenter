using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Auth
{
    public partial class Login
    {
        [NotNull]
        protected LoginModel? Model { get; set; }

        [Inject]
        [NotNull]
        protected CenterDbContext? DbContext { get; set; }

        [Inject]
        [NotNull]
        protected OnlineUsersService? OnlineUsersService { get; set; }

        [Inject]
        [NotNull]
        protected NavigationManager? NavigationManager { get; set; }

        private Task OnLoginSuccess(EditContext context)
        {
            var user = Model.GetUser()!;
            if (OnlineUsersService.Login(user))
            {
                NavigationManager.NavigateTo($"/person-center?customid={((ICustomIdProvider)user).CustomId}");
            }
            else
            {
                NavigationManager.NavigateTo("/error?info=User%20has%20logined.");
            }
            return Task.CompletedTask;
        }

        private void OnRegister()
        {
            NavigationManager.NavigateTo("/register");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Model = new LoginModel(DbContext, OnlineUsersService);
        }

        public class LoginModel : IValidatableObject
        {
            [Required(ErrorMessage = "用户名不得为空")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "密码不得为空")]
            public string Password { get; set; }

            public int ModelType { get; set; }

            private UserBase? user;

            private readonly CenterDbContext dbContext;

            private readonly OnlineUsersService onlineUsersService;

            public LoginModel(CenterDbContext dbContext, OnlineUsersService service)
            {
                UserName = "";
                Password = "";
                this.dbContext = dbContext;
                onlineUsersService = service;
            }

            public UserBase? GetUser()
            {
                UserBase? user = ModelType switch
                {
                    0 => dbContext.Customers.FirstOrDefault(c => c.Name == UserName && c.DeleteTime == null),
                    1 => dbContext.Suppliers.FirstOrDefault(s => s.Name == UserName && s.DeleteTime == null),
                    2 => dbContext.Traders.FirstOrDefault(t => t.Name == UserName && t.DeleteTime == null),
                    3 => dbContext.Administrators.FirstOrDefault(a => a.Name == UserName && a.DeleteTime == null),
                    _ => throw new InvalidOperationException()
                };
                this.user = user;
                return user;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                GetUser();
                if (user == null || user.Password != Password)
                {
                    return [new ValidationResult("用户名或密码错误", [nameof(UserName), nameof(Password)])];
                }
                if (onlineUsersService.IsOnline(user))
                {
                    return [new ValidationResult("用户已在线，不可重复登录")];
                }
                return [];
            }
        }
    }
}
