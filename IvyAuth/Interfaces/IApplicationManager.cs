namespace IvyAuth.Interfaces
{
	public interface IApplicationManager
    {
        IApplication IvyAuthApp { get; }
        IApplication BuildNumberApp {get;}

        IApplication? GetAppById(string? id);
    }
}