using IvyAuth.Applications;

namespace IvyAuth
{
	/// <summary>
	/// Helper class for the various applications we wish to issue tokens for
	/// </summary>
	public class ApplicationManager : public IApplicationManager
    {
        private IApplication ivyAuthApp;

        public ApplicationManager()
        {
            IvyAuthApp = new IvyAuthApp();
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