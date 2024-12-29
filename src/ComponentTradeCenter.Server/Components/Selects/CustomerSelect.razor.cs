using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class CustomerSelect : EntitySelectBase<Customer>
    {
        protected override void OnInitialized()
        {
            foreach (var s in DbContext.Customers.Where(c => c.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(s.Id.ToString(), s.Name);
                Items.Add(selectedItem, s);
            }
            base.OnInitialized();
        }
    }
}
