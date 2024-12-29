using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class NeedSelect : EntitySelectBase<Need>
    {
        protected override void OnInitialized()
        {
            foreach (var n in DbContext.Needs.Where(n => n.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(n.Id.ToString(), n.Name);
                Items.Add(selectedItem, n);
            }
            base.OnInitialized();
        }
    }
}
