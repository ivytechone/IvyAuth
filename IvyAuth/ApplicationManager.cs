using IvyAuth.Interfaces;
using IvyAuth.Applications;

namespace IvyAuth
{
	/// <summary>
	/// Helper class for the various applications we wish to issue tokens for
	/// </summary>
	public class ApplicationManager : IApplicationManager
    {
        private IvyAuthApp _ivyAuthApp;
        private GetBuildNumberApp _getBuildNumberApp;

        private Dictionary<string, IApplication> _apps;

        public ApplicationManager()
        {
            _apps = new Dictionary<string, IApplication>();
            _ivyAuthApp = new IvyAuthApp();
            _getBuildNumberApp = new GetBuildNumberApp();
          
            _apps.Add(_ivyAuthApp.Id, _ivyAuthApp);
            _apps.Add(_getBuildNumberApp.Id, _getBuildNumberApp);
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
        public IApplication BuildNumberApp => _getBuildNumberApp;
    }
}