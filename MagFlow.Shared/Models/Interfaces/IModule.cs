namespace MagFlow.Shared.Models.Interfaces
{
    public interface IModule : IDisposable
    {
        string TabTitle { get; }
        string Icon { get; }
    }
}
