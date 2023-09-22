using IvyAuth;
using IvyAuth.Config;
using IvyAuth.Interfaces;
using IvyTech.IvyLogging.Extensions;
using IvyTech.IvyWebApi;
using IvyTech.Logging;

var builder = WebApplication.CreateBuilder(args);

try
{
	var certificateManager = new StaticCertManager(builder.Configuration.GetSection("StaticCertManager").Get<StaticCertManagerConfig>());
	
	builder.UseIvyWebApi("IvyAuth", AppInfo.Version);
	builder.AddIvyDebugLogger();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddSingleton<IIdentityStore>(x => new StaticIdentityStore(builder.Configuration.GetSection("StaticIdentityStore").Get<StaticIdentityStoreConfig>()));
	builder.Services.AddSingleton<ICertificateManager>(x => certificateManager);
	builder.Services.AddSingleton<IApplicationManager>(x => new ApplicationManager());
	builder.Services.AddSingleton<ITokenGenerator>(x => new TokenGenerator(certificateManager));
	builder.Services.AddControllers();
	var app = builder.Build();
	app.AddPing();
	app.MapControllers();
	app.UseIvyLogging();
	app.Run();
}
catch (Exception ex)
{
	DebugLogger.Logger?.Fatal("Unhandled Excpetion", ex);
}
