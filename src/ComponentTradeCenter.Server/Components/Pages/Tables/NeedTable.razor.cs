using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class NeedTable : EntityTablePageBase<NeedVM>, IEditableTablePage<NeedVM>
    {
        public bool IsAddable => User is Customer or Administrator;

        public bool IsEditable => User is Customer or Administrator;

        public bool IsDeleteable => User is Customer or Administrator;

        public bool CanSelectSupplier => User is Administrator;

        protected override DbSet<NeedVM> DbSet => DbContext.NeedVMs;

        protected override IQueryable<NeedVM> Items => (User switch
        {
            Administrator or Trader => DbSet,
            Customer => DbSet.Where(n => n.CustomerId == User.Id),
            _ => throw new ArgumentException(nameof(User))
        }).AsNoTracking();

        public Task<NeedVM> OnAddAsync()
        {
            return Task.FromResult(new NeedVM
            {
                CustomerId = User is Customer ? User.Id : 0,
            });
        }

        public Task<bool> OnSaveAsync(NeedVM entity, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                var newNeed = new Need()
                {
                    Name = entity.Name,
                    CustomerId = entity.CustomerId,
                    ComponentId = entity.ComponentId,
                    Price = entity.Price,
                    Amount = entity.Amount,
                };
                DbContext.Needs.Add(newNeed);
            }
            else
            {
                var need = DbContext.Needs.First(n => n.Id == entity.Id);
                need.Name = entity.Name;
                need.CustomerId = entity.CustomerId;
                need.ComponentId = entity.ComponentId;
                need.Price = entity.Price;
                need.Amount = entity.Amount;
                DbContext.Needs.Entry(need).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<NeedVM> items)
        {
            foreach (var item in items)
            {
                if (DbContext.Needs.FirstOrDefault(s => s.Id == item.Id) is Need need)
                {
                    need.DeleteTime = DateTime.Now;
                    DbContext.Needs.Entry(need).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
