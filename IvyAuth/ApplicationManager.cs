using IvyAuth.Interfaces;
using IvyAuth.Applications;

namespace IvyAuth
{
	/// <summary>
	/// Helper class for the various applications we wish to issue tokens for
	/// </summary>
	public class ApplicationManager : IApplicationManager
    {
        private IApplication ivyAuthApp;

        public ApplicationManager()
        {
            ivyAuthApp = new IvyAuthApp();
        }

        public IApplication IvyAuthApp 
        { 
            get
            {
                return ivyAuthApp;
            }
        }
    }
}