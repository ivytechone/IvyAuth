using IvyAuth;
using IvyAuth.Config;
using IvyAuth.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = Logger.GetLogger(builder.Configuration.GetSection("SerilogConfig").Get<SerilogConfig>());

try
{
	builder.Services.AddLogging(x => x.AddSerilog(logger));
	builder.Services.AddSingleton<IIdentityStore>(x => new StaticIdentityStore(builder.Configuration.GetSection("StaticIdentityStore").Get<StaticIdentityStoreConfig>()));
	builder.Services.AddSingleton<ICertificateManager>(x => new StaticCertManager(builder.Configuration.GetSection("StaticCertManager").Get<StaticCertManagerConfig>()));
	builder.Services.AddSingleton<IApplicationManager>(x => new ApplicationManager());
	builder.Services.AddControllers();
	var app = builder.Build();
	app.MapControllers();
	app.UseMiddleware<RequestLogger>();
	app.Run();
}
catch (Exception ex)
{
	logger.Fatal("Unhandled Excpetion", ex);
}
