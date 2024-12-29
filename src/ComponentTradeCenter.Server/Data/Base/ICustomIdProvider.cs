using ComponentTradeCenter.Server.Data.Model;

namespace ComponentTradeCenter.Server.Data.Base
{
    public interface ICustomIdProvider
    {
        public abstract string CustomId { get; }

        public static abstract string GetCustomId(int? id);

        public static string GetCustomId(int? id, ModelType type)
        {
            return type switch
            {
                ModelType.Administrator => Administrator.GetCustomId(id),
                ModelType.Customer => Customer.GetCustomId(id),
                ModelType.Supplier => Supplier.GetCustomId(id),
                ModelType.Trader => Trader.GetCustomId(id),
                ModelType.Component => Component.GetCustomId(id),
                ModelType.Need => Need.GetCustomId(id),
                ModelType.Supply => Supply.GetCustomId(id),
                ModelType.TradeSuggest => TradeSuggest.GetCustomId(id),
                ModelType.Agreement => Agreement.GetCustomId(id),
                ModelType.Trade => Trade.GetCustomId(id),
                _ => throw new ArgumentException("Invalid type.", nameof(type))
            };
        }
    }
}
