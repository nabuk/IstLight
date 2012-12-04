// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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