using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Base;
using ComponentTradeCenter.Server.Data.Model;
using ComponentTradeCenter.Server.Data.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ComponentTradeCenter.Server.Components.Pages.Tables
{
    public partial class AgreementTable : EntityTablePageBase<AgreementVM>, IEditableTablePage<AgreementVM>
    {
        public bool IsAddable => User is Administrator;

        public bool IsEditable => User is Administrator;

        public bool IsDeleteable => User is Administrator;

        protected override DbSet<AgreementVM> DbSet => DbContext.AgreementVMs;

        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

        protected override IQueryable<AgreementVM> Items => (User switch
        {
            Administrator => DbSet,
            Customer => DbSet.Where(a => a.CustomerId == User.Id),
            Supplier => DbSet.Where(a => a.SupplierId == User.Id),
            Trader => DbSet.Where(a => a.TraderId == User.Id),
            _ => throw new ArgumentException(nameof(User))
        }).AsNoTracking();

        protected Modal Modal = new Modal();

        [NotNull]
        protected Trade? Trade = null;

        [NotNull]
        protected AgreementVM? AgreementVM = null;

        private async Task OnUploadSign(UploadFile file, AgreementVM vm, bool isCustomers)
        {
            var agreement = DbContext.Agreements.Where(a => a.Id == vm.Id).First();

            using var memory = new MemoryStream();
            await file.File!.OpenReadStream().CopyToAsync(memory);
            byte[] bytes = memory.ToArray();

            if (isCustomers)
            {
                agreement.CustomerSignature = bytes;
                vm.CustomerSignature = bytes;
            }
            else
            {
                agreement.SupplierSignature = bytes;
                vm.SupplierSignature = bytes;
            }

            DbContext.Entry(agreement).State = EntityState.Modified;
            DbContext.SaveChanges();
            StateHasChanged();
        }

        private async Task OnCompeleteAgreement(AgreementVM vm)
        {
            Trade = new Trade() { AgreementId = vm.Id };
            AgreementVM = vm;
            await Modal.Toggle();
        }

        private Task OnDropAgreement(AgreementVM vm)
        {
            if (DbContext.Agreements.FirstOrDefault(a => a.Id == vm.Id) is Agreement agreement)
            {
                agreement.TraderIntention = false;
                DbContext.Agreements.Entry(agreement).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            return Task.CompletedTask;
        }

        private async Task OnModalConfirm()
        {
            await Modal.Toggle();
            if (DbContext.Agreements.FirstOrDefault(a => a.Id == AgreementVM.Id) is Agreement agreement)
            {
                agreement.TraderIntention = true;
                DbContext.Agreements.Entry(agreement).State = EntityState.Modified;
            }
            DbContext.Trades.Add(Trade);
            DbContext.SaveChanges();
            NavigationManager.NavigateTo($"/trade-table?customid={((ICustomIdProvider)User)?.CustomId}");
        }

        private async Task ShowSignature(byte[]? bytes, string title)
        {
            if (bytes == null)
            {
                return;
            }

            var base64Image = $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
            await DialogService.Show(new DialogOption
            {
                Title = title,
                BodyTemplate = builder =>
                {
                    builder.OpenElement(0, "img");
                    builder.AddAttribute(1, "src", base64Image);
                    builder.AddAttribute(2, "alt", "图片加载失败");
                    builder.AddAttribute(3, "style", "max-width: 100%; height: auto;");
                    builder.CloseElement();
                }
            });
        }

        public Task<AgreementVM> OnAddAsync()
            => Task.FromResult(new AgreementVM());

        public Task<bool> OnSaveAsync(AgreementVM entity, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                var newAgreement = new Agreement()
                {
                    Name = entity.Name,
                    SuggestId = entity.SuggestId,
                    CustomerSignature = entity.CustomerSignature,
                    SupplierSignature = entity.SupplierSignature,
                    TraderIntention = entity.TraderIntention,
                };
                DbContext.Agreements.Add(newAgreement);
            }
            else if (DbContext.Agreements.FirstOrDefault(s => s.Id == entity.Id) is Agreement agreement)
            {
                agreement.Name = entity.Name;
                agreement.SuggestId = entity.SuggestId;
                agreement.CustomerSignature = entity.CustomerSignature;
                agreement.SupplierSignature = entity.SupplierSignature;
                agreement.TraderIntention = entity.TraderIntention;
                DbContext.Agreements.Entry(agreement).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> OnDeleteAsync(IEnumerable<AgreementVM> items)
        {
            foreach (var item in items)
            {
                if (DbContext.Agreements.FirstOrDefault(s => s.Id == item.Id) is Agreement agreement)
                {
                    agreement.DeleteTime = DateTime.Now;
                    DbContext.Agreements.Entry(agreement).State = EntityState.Modified;
                }
            }
            DbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
