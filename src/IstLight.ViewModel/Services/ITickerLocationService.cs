namespace IstLight.Services
{
    public interface ITickerLocationService
    {
        IReadOnlyList<string> Load();
        string Save(Ticker ticker);
    }



    //public class TickerLocationService : ITickerLocationService
    //{
    //    public TickerLocationService(ITickerConverterService converters)
    //    {
    //        converters.Load().
    //    }
    //    public IReadOnlyList<string> Load()
    //    {
    //        var ofd = new System.Windows.Forms.OpenFileDialog();
    //        return ofd.FileNames.AsReadOnlyList();
    //    }

    //    public string Save(Ticker ticker)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}