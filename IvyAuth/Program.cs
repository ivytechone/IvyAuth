using IvyAuth;
using IvyAuth.Config;
using IvyAuth.Interfaces;
using IvyTech.IvyWebApi;
using IvyTech.Logging;
using Serilog;

// App level logging context
IvyTech.Logging.AppContext.SetAppName("IvyAuth");
IvyTech.Logging.AppContext.SetVersion(AppInfo.Version);

var builder = WebApplication.CreateBuilder(args);
var logger = DebugLogger.CreateLogger(builder.Configuration);

try
{
	builder.UseIvyWebApi("IvyAuth", AppInfo.Version);
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddLogging(x => x.AddSerilog(logger));
	builder.Services.AddSingleton<IIdentityStore>(x => new StaticIdentityStore(builder.Configuration.GetSection("StaticIdentityStore").Get<StaticIdentityStoreConfig>()));
	builder.Services.AddSingleton<ICertificateManager>(x => new StaticCertManager(builder.Configuration.GetSection("StaticCertManager").Get<StaticCertManagerConfig>()));
	builder.Services.AddSingleton<IApplicationManager>(x => new ApplicationManager());
	builder.Services.AddControllers();
	var app = builder.Build();
	app.AddPing();
	app.MapControllers();
	app.UseIvyLogging();
	app.Run();
}
catch (Exception ex)
{
	logger?.Fatal("Unhandled Excpetion", ex);
}
