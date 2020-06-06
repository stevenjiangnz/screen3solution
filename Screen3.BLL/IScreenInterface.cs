using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
public interface IScreenInterface
{
    Task<List<TickerEntity>> RetrieveData(string code, string type = "day", int start = 0, int end = 0, IDictionary<string, object> options = null);
    List<TickerEntity> GetEntryMatchTickers(IDictionary<string, object> options);
    // TickerEntity GetExitMatchTicker(TickerEntity entry, IDictionary<string, object> options);
}