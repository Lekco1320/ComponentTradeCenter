using ComponentTradeCenter.Server.Components;
using ComponentTradeCenter.Server.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

builder.Services.AddBootstrapBlazor(null, options =>
    options.IgnoreLocalizerMissing = true
);

builder.Services.AddDbContext<CenterDbContext>(options =>
    options.UseMySql(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

// 增加 Pdf 导出服务
builder.Services.AddBootstrapBlazorTableExportService();

// 增加 Html2Pdf 服务
builder.Services.AddBootstrapBlazorHtml2PdfService();

// 增加 SignalR 服务数据传输大小限制配置
builder.Services.Configure<HubOptions>(option => option.MaximumReceiveMessageSize = null);

builder.Services.AddSingleton<OnlineUsersService>();
builder.Services.AddSingleton<DumpDbService>();
builder.Services.AddSingleton<ClearDumpService>();
builder.Services.AddHostedService<BackgroundDumpService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();

GC.Collect();
