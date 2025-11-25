
namespace RealtimeTable.Interfaces
{
    public interface IUnitCacheService
    {
        Dictionary<string, string> Units { get; }
        void SetUnits(Dictionary<string, string> units);
        string? GetUnit(string tag);
    }
}
