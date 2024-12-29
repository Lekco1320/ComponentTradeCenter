using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Services
{
    public class CenterDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; protected set; }

        public DbSet<Supplier> Suppliers { get; protected set; }

        public DbSet<Trader> Traders { get; protected set; }

        public DbSet<Administrator> Administrators { get; protected set; }

        public DbSet<Component> Components { get; protected set; }

        public DbSet<Supply> Supplies { get; protected set; }

        public DbSet<Need> Needs { get; protected set; }

        public DbSet<Agreement> Agreements { get; protected set; }

        public DbSet<Trade> Trades { get; protected set; }

        public DbSet<TradeSuggest> TradeSuggests { get; protected set; }

        public DbSet<NeedVM> NeedVMs { get; protected set; }

        public DbSet<SupplyVM> SupplyVMs { get; protected set; }

        public DbSet<TradableInfo> TradableInfos { get; protected set; }

        public DbSet<TradeSuggestVM> TradeSuggestVMs { get; protected set; }

        public DbSet<AgreementVM> AgreementVMs { get; protected set; }

        public DbSet<TradeVM> TradeVMs { get; protected set; }

        public DbSet<UserInfo> UserInfos { get; protected set; }

        public CenterDbContext(DbContextOptions<CenterDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CenterDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                       .AddJsonFile("appsettings.json")
                                                       .Build()
                                                       .GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        }

        public bool TryGetUser(string? customId, [NotNullWhen(true)] out UserBase? user)
        {
            user = null;
            if (customId == null || !int.TryParse(customId[3..], out var id))
            {
                return false;
            }
            user = customId[0..3] switch
            {
                "CUS" => Customers.FirstOrDefault(c => c.Id == id),
                "SPR" => Suppliers.FirstOrDefault(s => s.Id == id),
                "TDR" => Traders.FirstOrDefault(t => t.Id == id),
                "ADM" => Administrators.FirstOrDefault(t => t.Id == id),
                _ => null
            };
            return user != null;
        }

        public override void Dispose()
        {
            SaveChanges();
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
