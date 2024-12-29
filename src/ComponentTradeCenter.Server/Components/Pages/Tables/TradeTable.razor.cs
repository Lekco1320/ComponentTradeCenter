using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class TradeTable : EntityTablePageBase<TradeVM>, IEditableTablePage<TradeVM>
    {
        protected override DbSet<TradeVM> DbSet => DbContext.TradeVMs;

        public bool IsAddable => User is Administrator;

        public bool IsEditable => User is Administrator;

        public bool IsDeleteable => User is Administrator;

        protected override IQueryable<TradeVM> Items => (User switch
        {
            Administrator => DbSet,
            Customer => DbSet.Where(t => t.CustomerId == User.Id),
            Supplier => DbSet.Where(t => t.SupplierId == User.Id),
            Trader => DbSet.Where(t => t.TraderId == User.Id),
            _ => throw new ArgumentException(nameof(User))
        }).AsNoTracking();

        public Task<TradeVM> OnAddAsync()
            => Task.FromResult(new TradeVM());

        public Task<bool> OnSaveAsync(TradeVM entity, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                var newTrade = new Trade()
                {
                    Name = entity.Name,
                    AgreementId = entity.AgreementId,
                    CompleteTime = entity.CompleteTime,
                };
                DbContext.Trades.Add(newTrade);
            }
            else if (DbContext.Trades.FirstOrDefault(s => s.Id == entity.Id) is Trade trade)
            {
                trade.Name = entity.Name;
                trade.AgreementId = entity.AgreementId;
                trade.CompleteTime = entity.CompleteTime;
                DbContext.Trades.Entry(trade).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<TradeVM> items)
        {
            foreach (var item in items)
            {
                if (DbContext.Trades.FirstOrDefault(s => s.Id == item.Id) is Trade trade)
                {
                    trade.DeleteTime = DateTime.Now;
                    DbContext.Trades.Entry(trade).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
