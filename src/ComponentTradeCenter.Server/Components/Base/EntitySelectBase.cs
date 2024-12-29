using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Base
{
    public abstract class EntitySelectBase<TItem> : ComponentBase where TItem : EntityBase
    {
        [Inject]
        [NotNull]
        protected CenterDbContext? DbContext { get; set; }

        protected Dictionary<SelectedItem, TItem> Items { get; set; }

        [Parameter]
        public int Value { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        protected int InternalValue
        {
            get => Value;
            set
            {
                Value = value;
                ValueChanged.InvokeAsync(Value);
            }
        }

        public EntitySelectBase()
        {
            Items = new Dictionary<SelectedItem, TItem>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
