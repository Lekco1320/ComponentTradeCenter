using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Shared
{
    public sealed partial class MainLayout
    {
        [NotNull]
        private List<MenuItem>? Menus { get; set; }

        [NotNull]
        public UserBase? User { get; set; }

        public void UpdateLayout()
        {
            Menus = GetIconSideMenuItems();
            StateHasChanged();
        }

        private List<MenuItem> GetIconSideMenuItems()
        {
            var ret = new List<MenuItem>();
            if (User == null)
            {
                return ret;
            }

            ret.Add(new MenuItem()
            {
                Text = "个人中心",
                Icon = "fa-solid fa-fw fa-user",
                Url = $"/person-center?customid={((ICustomIdProvider)User).CustomId}"
            });

            ret.Add(new MenuItem()
            {
                Text = "零件信息",
                Icon = "fa-solid fa-fw fa-wrench",
                Url = $"/component-table?customid={((ICustomIdProvider)User).CustomId}"
            });

            if (User is Customer or Administrator)
            {
                ret.Add(new MenuItem()
                {
                    Text = "收购信息",
                    Icon = "fa-solid fa-fw fa-shopping-cart",
                    Url = $"/need-table?customid={((ICustomIdProvider)User).CustomId}"
                });
            }

            if (User is Supplier or Administrator)
            {
                ret.Add(new MenuItem()
                {
                    Text = "出售信息",
                    Icon = "fa-solid fa-fw fa-warehouse",
                    Url = $"/supply-table?customid={((ICustomIdProvider)User).CustomId}"
                });
            }

            if (User is Trader)
            {
                ret.Add(new MenuItem()
                {
                    Text = "供需关系",
                    Icon = "fa-solid fa-fw fa-exchange-alt",
                    Url = $"/tradable-info-table?customid={((ICustomIdProvider)User).CustomId}"
                });
            }

            ret.Add(new MenuItem()
            {
                Text = "交易建议",
                Icon = "fa-solid fa-fw fa-lightbulb",
                Url = $"/trade-suggest-table?customid={((ICustomIdProvider)User).CustomId}"
            });

            ret.Add(new MenuItem()
            {
                Text = "在途交易",
                Icon = "fa-solid fa-fw fa-balance-scale",
                Url = $"/agreement-table?customid={((ICustomIdProvider)User).CustomId}"
            });

            ret.Add(new MenuItem()
            {
                Text = "交易记录",
                Icon = "fa-solid fa-fw fa-database",
                Url = $"/trade-table?customid={((ICustomIdProvider)User).CustomId}"
            });

            if (User is Administrator)
            {
                ret.Add(new MenuItem()
                {
                    Text = "用户信息",
                    Icon = "fa-solid fa-fw fa-users",
                    Url = $"/user-table?customid={((ICustomIdProvider)User).CustomId}"
                });
            }

            return ret;
        }
    }
}
