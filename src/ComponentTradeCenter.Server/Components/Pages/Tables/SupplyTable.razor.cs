using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class SupplyTable : EntityTablePageBase<SupplyVM>, IEditableTablePage<SupplyVM>
    {
        public bool IsAddable => User is Supplier or Administrator;

        public bool IsEditable => User is Supplier or Administrator;

        public bool IsDeleteable => User is Supplier or Administrator;

        public bool CanSelectSupplier => User is Administrator;

        protected override DbSet<SupplyVM> DbSet => DbContext.SupplyVMs;

        protected override IQueryable<SupplyVM> Items => (User switch
        {
            Administrator or Trader => DbSet,
            Supplier => DbSet.Where(s => s.SupplierId == User.Id),
            _ => throw new ArgumentException(nameof(User)),
        }).AsNoTracking();

        public Task<SupplyVM> OnAddAsync()
        {
            return Task.FromResult(new SupplyVM()
            {
                SupplierId = User is Supplier ? User.Id : 0,
            });
        }

        public Task<bool> OnSaveAsync(SupplyVM entity, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                var newSupply = new Supply()
                {
                    Name = entity.Name,
                    SupplierId = entity.SupplierId,
                    ComponentId = entity.ComponentId,
                    Price = entity.Price,
                    Amount = entity.Amount,
                };
                DbContext.Supplies.Add(newSupply);
            }
            else
            {
                var supply = DbContext.Supplies.First(n => n.Id == entity.Id);
                supply.Name = entity.Name;
                supply.SupplierId = entity.SupplierId;
                supply.ComponentId = entity.ComponentId;
                supply.Price = entity.Price;
                supply.Amount = entity.Amount;
                DbContext.Supplies.Entry(supply).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<SupplyVM> items)
        {
            foreach (var item in items)
            {
                if (DbContext.Supplies.FirstOrDefault(s => s.Id == item.Id) is Supply supply)
                {
                    supply.DeleteTime = DateTime.Now;
                    DbContext.Supplies.Entry(supply).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
