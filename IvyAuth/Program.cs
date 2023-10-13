using IvyAuth;
using IvyAuth.Config;
using IvyAuth.Interfaces;
using IvyTech.IvyLogging.Extensions;
using IvyTech.IvyWebApi;
using IvyTech.Logging;

var builder = WebApplication.CreateBuilder(args);

try
{
	builder.UseIvyWebApi("IvyAuth", AppInfo.Version);
	builder.AddIvyDebugLogger();

	var certificateManager = new StaticCertManager(builder.Configuration.GetSection("StaticCertManager").Get<StaticCertManagerConfig>());
	var staticIdentityStoreConfig = builder.Configuration.GetSection("StaticIdentityStore").Get<StaticIdentityStoreConfig>();

	IIdentityStore identityStore;
	if (staticIdentityStoreConfig != null)
	{
		identityStore = new StaticIdentityStore(staticIdentityStoreConfig);
	}
	else
	{
		identityStore = new IdentityStore();
	}
	
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddSingleton<IIdentityStore>(x => identityStore);
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
	Console.WriteLine(ex.ToString());
	DebugLogger.Logger?.Fatal("Unhandled Excpetion", ex);
}
