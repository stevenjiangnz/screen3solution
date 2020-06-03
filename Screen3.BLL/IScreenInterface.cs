using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
public interface IScreenInterface
{
    Task RetrieveData(string code, int start = 0, int end = 0);
    List<TickerEntity> GetEntryMatchTickers(IDictionary<string, object> options);
    // TickerEntity GetExitMatchTicker(TickerEntity entry, IDictionary<string, object> options);
}