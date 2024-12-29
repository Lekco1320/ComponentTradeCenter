using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class TradableInfoTable : EntityTablePageBase<TradableInfo>
    {
        protected override DbSet<TradableInfo> DbSet => DbContext.TradableInfos;

        protected override IQueryable<TradableInfo> Items => DbSet;

        [NotNull]
        protected TradeSuggest? TradeSuggest { get; set; }

        protected string ComponentName = "";
        protected Modal Modal = new Modal();

        private async Task NewTradeSuggest(TradableInfo info)
        {
            TradeSuggest = new TradeSuggest()
            {
                NeedId = info.NeedId,
                SupplyId = info.SupplyId,
                TraderId = User.Id,
                Price = info.NeedPrice,
                Amount = info.NeedAmount
            };
            ComponentName = info.ComponentName;
            await Modal.Toggle();
        }

        private async Task OnModalConfirm()
        {
            await Modal.Toggle();
            DbContext.TradeSuggests.Add(TradeSuggest);
            DbContext.SaveChanges();
        }
    }
}
