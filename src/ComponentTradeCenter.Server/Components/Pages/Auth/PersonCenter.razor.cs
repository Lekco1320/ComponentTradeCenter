using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Auth
{
    public partial class PersonCenter : OnlineComponentBase
    {
        [NotNull]
        protected EditUserModel? EditModel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            EditModel = new EditUserModel(User, DbContext);
        }

        public class EditUserModel : IValidatableObject
        {
            public string CustomId { get; set; }

            public string Category { get; set; }

            [Required(ErrorMessage = "{0}不得为空")]
            public string NewName { get; set; }

            [Required(ErrorMessage = "{0}不得为空")]
            public string NewPassword { get; set; } = "";

            [Required(ErrorMessage = "{0}不得为空")]
            [Compare(nameof(NewPassword), ErrorMessage = "确认密码不同")]
            public string NewConfirmPassword { get; set; } = "";

            public string? Phone { get; set; }

            public string? Address { get; set; }

            private readonly UserBase _user;
            private readonly CenterDbContext dbContext;

            public EditUserModel(UserBase user, CenterDbContext dbContext)
            {
                _user = user;
                NewName = _user.Name;
                Phone = _user.Phone;
                CustomId = ICustomIdProvider.GetCustomId(_user.Id, _user.ModelType);
                Category = _user.ModelType.ToDisplayName();
                if (_user is Customer customer)
                {
                    Address = customer.Address;
                }
                if (_user is Supplier supplier)
                {
                    Address = supplier.Address;
                }
                this.dbContext = dbContext;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var ret = new List<ValidationResult>();
                if (string.IsNullOrEmpty(NewPassword))
                {
                    ret.Add(new ValidationResult("密码不得为空", [nameof(NewPassword)]));
                }
                bool repeat = NewName != _user.Name && _user.ModelType switch
                {
                    ModelType.Customer => dbContext.Customers.Any(c => c.Name == NewName && c.DeleteTime == null),
                    ModelType.Supplier => dbContext.Suppliers.Any(s => s.Name == NewName && s.DeleteTime == null),
                    ModelType.Trader => dbContext.Traders.Any(t => t.Name == NewName && t.DeleteTime == null),
                    ModelType.Administrator => dbContext.Administrators.Any(a => a.Name == NewName && a.DeleteTime == null),
                    _ => throw new InvalidOperationException()
                };
                if (repeat)
                {
                    ret.Add(new ValidationResult("用户名重复", [nameof(NewName)]));
                }
                return ret;
            }
        }

        private Task OnConfirmChanges(EditContext context)
        {
            User.Name = EditModel.NewName;
            User.Phone = EditModel.Phone;
            User.Password = EditModel.NewPassword;
            if (User is Customer customer)
            {
                customer.Address = EditModel.Address;
            }
            if (User is Supplier supplier)
            {
                supplier.Address = EditModel.Address;
            }
            DbContext.Entry(User).State = EntityState.Modified;
            DbContext.SaveChanges();
            return Task.CompletedTask;
        }

        private Task OnConfirmLogout()
        {
            OnlineUsersService.Logout(User);
            NavigationManager.NavigateTo("/login");
            return Task.CompletedTask;
        }

        private Task OnConfirmLogoff()
        {
            OnlineUsersService.Logout(User);
            switch (User)
            {
                case Customer customer:
                    customer.DeleteTime = DateTime.Now;
                    break;
                case Supplier supplier:
                    supplier.DeleteTime = DateTime.Now;
                    break;
                case Trader trader:
                    trader.DeleteTime = DateTime.Now;
                    break;
                case Administrator administrator:
                    administrator.DeleteTime = DateTime.Now;
                    break;
            }
            DbContext.SaveChanges();
            NavigationManager.NavigateTo("/login");
            return Task.CompletedTask;
        }
    }
}
