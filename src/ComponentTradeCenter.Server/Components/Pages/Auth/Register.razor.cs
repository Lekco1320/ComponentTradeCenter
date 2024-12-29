using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Auth
{
    public partial class Register
    {
        [CascadingParameter]
        [NotNull]
        protected RegisterModel? Model { get; set; }

        [Inject]
        [NotNull]
        protected CenterDbContext? DbContext { get; set; }

        [Inject]
        [NotNull]
        protected NavigationManager? navigationManager { get; set; }

        [Inject]
        [NotNull]
        protected OnlineUsersService? onlineUsersService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Model = new RegisterModel(DbContext);
        }

        private Task OnValidRegister(EditContext context)
        {
            UserBase? user = null;
            ModelType modelType = (ModelType)Model.UserType;
            switch (modelType)
            {
                case ModelType.Customer:
                    var customer = new Customer()
                    {
                        Name = Model.UserName,
                        Password = Model.Password
                    };
                    DbContext.Customers.Add(customer);
                    user = customer;
                    break;

                case ModelType.Supplier:
                    var supplier = new Supplier()
                    {
                        Name = Model.UserName,
                        Password = Model.Password
                    };
                    DbContext.Suppliers.Add(supplier);
                    user = supplier;
                    break;

                case ModelType.Trader:
                    var trader = new Trader()
                    {
                        Name = Model.UserName,
                        Password = Model.Password
                    };
                    DbContext.Traders.Add(trader);
                    user = trader;
                    break;

                case ModelType.Administrator:
                    var administrator = new Administrator()
                    {
                        Name = Model.UserName,
                        Password = Model.Password
                    };
                    DbContext.Administrators.Add(administrator);
                    user = administrator;
                    break;
            }
            DbContext.SaveChanges();

            if (user != null)
            {
                onlineUsersService.Login(user);
                navigationManager.NavigateTo($"/component-table?customid={((ICustomIdProvider)user).CustomId}");
            }
            return Task.CompletedTask;
        }

        protected Task GoToLogin()
        {
            navigationManager.NavigateTo("/login");
            return Task.CompletedTask;
        }

        public class RegisterModel : IValidatableObject
        {
            [Required(ErrorMessage = "用户名不得为空")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "密码不得为空")]
            public string Password { get; set; }

            [Required(ErrorMessage = "确认密码不得为空")]
            [Compare(nameof(Password), ErrorMessage = "密码与确认密码不相同")]
            public string ConfirmedPassword { get; set; }

            public int UserType { get; set; }

            private readonly CenterDbContext dbContext;

            public RegisterModel(CenterDbContext dbContext)
            {
                UserName = "";
                Password = "";
                ConfirmedPassword = "";
                this.dbContext = dbContext;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                ModelType modelType = (ModelType)UserType;
                if (modelType == ModelType.Administrator)
                {
                    return [new ValidationResult($"管理员不允许由外部注册", [nameof(UserType)])];
                }
                var ret = modelType switch
                {
                    ModelType.Customer => dbContext.Customers.Any(c => c.Name == UserName && c.DeleteTime == null),
                    ModelType.Supplier => dbContext.Suppliers.Any(s => s.Name == UserName && s.DeleteTime == null),
                    ModelType.Trader => dbContext.Traders.Any(t => t.Name == UserName && t.DeleteTime == null),
                    ModelType.Administrator => dbContext.Administrators.Any(a => a.Name == UserName && a.DeleteTime == null),
                    _ => throw new InvalidOperationException()
                };
                return ret ? [new ValidationResult($"用户名重复", [nameof(UserName)])] : [];
            }
        }
    }
}
