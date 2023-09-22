using IvyAuth.Interfaces;
using IvyAuth.Applications;

namespace IvyAuth
{
	/// <summary>
	/// Helper class for the various applications we wish to issue tokens for
	/// </summary>
	public class ApplicationManager : IApplicationManager
    {
        private IApplication _ivyAuthApp;

        private Dictionary<string, IApplication> _apps;

        public ApplicationManager()
        {
            _apps = new Dictionary<string, IApplication>();
            _ivyAuthApp = registerApp(new IvyAuthApp());
            registerApp(new GetBuildNumberApp());
            registerApp(new TestApp());
        }

        private IApplication registerApp(IApplication app)
        {
            _apps.Add(app.Id, app);
            return app;
        }

        public IApplication? GetAppById(string? id)
        {
            if (id is null)
            {
                return null;
            }
            
            _apps.TryGetValue(id, out IApplication? app);
            return app;
        }

        public IApplication IvyAuthApp => _ivyAuthApp;
    }
}