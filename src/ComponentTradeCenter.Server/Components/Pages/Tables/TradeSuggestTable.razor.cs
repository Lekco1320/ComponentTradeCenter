using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class TradeSuggestTable : EntityTablePageBase<TradeSuggestVM>, IEditableTablePage<TradeSuggestVM>
    {
        public bool IsAddable => User is Administrator;

        public bool IsEditable => User is Administrator;

        public bool IsDeleteable => User is Administrator;

        protected override DbSet<TradeSuggestVM> DbSet => DbContext.TradeSuggestVMs;

        protected override IQueryable<TradeSuggestVM> Items => (User switch
        {
            Administrator => DbSet,
            Customer => DbSet.Where(vm => vm.CustomerId == User.Id),
            Supplier => DbSet.Where(vm => vm.SupplierId == User.Id),
            Trader => DbSet.Where(vm => vm.TraderId == User.Id),
            _ => throw new ArgumentException(nameof(User))
        }).AsNoTracking();

        protected Modal Modal = new Modal();

        [NotNull]
        protected Agreement? Agreement = null;

        [NotNull]
        protected TradeSuggestVM? TradeSuggestVM = null;

        private static string FormatIntention(bool? intention) => intention switch
        {
            true => "已同意",
            false => "已拒绝",
            null => "未决定",
        };

        private Task OnConfirmIntention(TradeSuggestVM vm, bool intention, bool isCustomers)
        {
            var suggest = DbContext.TradeSuggests.First(s => s.Id == vm.Id);
            if (isCustomers)
            {
                suggest.CustomerIntention = intention;
                vm.CustomerIntention = intention;
            }
            else
            {
                suggest.SupplierIntention = intention;
                vm.SupplierIntention = intention;
            }

            DbContext.Entry(suggest).State = EntityState.Modified;
            DbContext.SaveChanges();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task NewAgreement(TradeSuggestVM vm)
        {
            Agreement = new Agreement() { SuggestId = vm.Id };
            TradeSuggestVM = vm;
            await Modal.Toggle();
        }

        private Task CancelSuggest(TradeSuggestVM vm)
        {
            if (DbContext.TradeSuggests.FirstOrDefault(s => s.Id == vm.Id) is TradeSuggest suggest)
            {
                suggest.TraderIntention = false;
                DbContext.TradeSuggests.Entry(suggest).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.CompletedTask;
        }

        private async Task OnModalConfirm()
        {
            await Modal.Toggle();
            DbContext.Agreements.Add(Agreement);
            if (DbContext.TradeSuggests.FirstOrDefault(s => s.Id == TradeSuggestVM.Id) is TradeSuggest suggest)
            {
                suggest.TraderIntention = true;
                DbContext.TradeSuggests.Entry(suggest).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            NavigationManager.NavigateTo($"/agreement-table?customid={((ICustomIdProvider)User)?.CustomId}");
        }

        public Task<TradeSuggestVM> OnAddAsync()
        {
            return Task.FromResult(new TradeSuggestVM());
        }

        public Task<bool> OnSaveAsync(TradeSuggestVM entity, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                var newTradeSuggest = new TradeSuggest()
                {
                    Name = entity.Name,
                    NeedId = entity.NeedId,
                    SupplyId = entity.SupplyId,
                    TraderId = entity.TraderId,
                    Price = entity.Price,
                    Amount = entity.Amount,
                    CustomerIntention = entity.CustomerIntention,
                    SupplierIntention = entity.SupplierIntention,
                    TraderIntention = entity.TraderIntention,
                };
                DbContext.TradeSuggests.Add(newTradeSuggest);
            }
            else if (DbContext.TradeSuggests.First(s => s.Id == entity.Id) is TradeSuggest suggest)
            {
                suggest.Name = entity.Name;
                suggest.NeedId = entity.NeedId;
                suggest.SupplyId = entity.SupplyId;
                suggest.TraderId = entity.TraderId;
                suggest.Price = entity.Price;
                suggest.Amount = entity.Amount;
                suggest.CustomerIntention = entity.CustomerIntention;
                suggest.SupplierIntention = entity.SupplierIntention;
                suggest.TraderIntention = entity.TraderIntention;
                DbContext.TradeSuggests.Entry(suggest).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<TradeSuggestVM> items)
        {
            foreach (var item in items)
            {
                if (DbContext.TradeSuggests.FirstOrDefault(s => s.Id == item.Id) is TradeSuggest suggest)
                {
                    suggest.DeleteTime = DateTime.Now;
                    DbContext.TradeSuggests.Entry(suggest).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
