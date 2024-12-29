using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class AgreementSelect : EntitySelectBase<Agreement>
    {
        protected override void OnInitialized()
        {
            foreach (var a in DbContext.Agreements.Where(a => a.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(a.Id.ToString(), a.Name);
                Items.Add(selectedItem, a);
            }
            base.OnInitialized();
        }
    }
}
