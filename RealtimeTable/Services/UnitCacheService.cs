using RealtimeTable.Interfaces;

namespace RealtimeTable.Services
{
    public class UnitCacheService : IUnitCacheService
    {
        private Dictionary<string, string> _units = new();

        public Dictionary<string, string> Units => _units;

        public void SetUnits(Dictionary<string, string> units)
        {
            _units = units;
        }

        public string? GetUnit(string tag)
        {
            if (_units.TryGetValue(tag, out var u))
                return u;
            return null;
        }
    }

}
