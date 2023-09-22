namespace IvyAuth.Interfaces
{
	public interface IApplicationManager
    {
        IApplication IvyAuthApp { get; }
          IApplication? GetAppById(string? id);
    }
}