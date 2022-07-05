using IvyAuth;
using IvyAuth.Config;
using IvyAuth.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IIdentityStore>(x => new StaticIdentityStore(builder.Configuration.GetSection("StaticIdentityStore").Get<StaticIdentityStoreConfig>()));
builder.Services.AddSingleton<ICertificateManager>(x => new StaticCertManager(builder.Configuration.GetSection("StaticCertManager").Get<StaticCertManagerConfig>()));
builder.Services.AddSingleton<IApplicationManager>(x => new ApplicationManager());
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();
