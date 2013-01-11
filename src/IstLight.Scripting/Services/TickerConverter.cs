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
    public class TickerConverter : ScriptNamedItemBase, ITickerConverter
    {
        public TickerConverter(string format, ParallelScriptExecutor executor) : base(executor)
        {
            this.Format = format;
        }

        #region ITickerConverter
        public string Format
        {
            get;
            private set;
        }

        public ValueOrError<Ticker> Read(RawFile rawTicker)
        {
            return executor.SafeExecute<Ticker>(engine => engine.GetVariable("Read")(rawTicker));
        }

        public ValueOrError<RawFile> Save(Ticker ticker)
        {
            return executor.SafeExecute<RawFile>(engine => engine.GetVariable("Save")(ticker));
        }
        #endregion // ITickerConverter
    }
}
