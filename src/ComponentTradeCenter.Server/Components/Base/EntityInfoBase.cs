using ComponentTradeCenter.Server.Data.Base;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Base
{
    public class EntityInfoBase<TItem> : ComponentBase where TItem : EntityBase
    {
        [Parameter]
        [NotNull]
        public TItem? Value { get; set; }

        protected virtual string Format(string? str, string defaultStr = "无")
            => string.IsNullOrEmpty(str) ? defaultStr : str;
    }
}
