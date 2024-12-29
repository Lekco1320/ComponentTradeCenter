using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class ComponentTable : ModelTablePageBase<Component>, IEditableTablePage<Component>
    {
        protected override DbSet<Component> DbSet => DbContext.Components;

        protected override IQueryable<Component> Items => DbSet.Where(c => c.DeleteTime == null);

        public bool IsAddable => User is Customer or Supplier or Administrator;

        public bool IsEditable => User is Administrator;

        public bool IsDeleteable => User is Administrator;

        public Task<Component> OnAddAsync() => Task.FromResult(new Component());

        public Task<bool> OnSaveAsync(Component component, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                DbSet.Add(component);
            }
            else if (DbSet.FirstOrDefault(c => c.Id == component.Id) is Component origin)
            {
                origin.Name = component.Name;
                origin.Color = component.Color;
                origin.Weight = component.Weight;
                origin.Brief = component.Brief;
                DbSet.Entry(origin).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<Component> items)
        {
            foreach (var item in items)
            {
                if (item.DeleteTime == null &&
                    DbContext.Components.FirstOrDefault(s => s.Id == item.Id) is Component component)
                {
                    component.DeleteTime = DateTime.Now;
                    DbContext.Components.Entry(component).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
