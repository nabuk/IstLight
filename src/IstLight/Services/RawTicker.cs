using System;

namespace IstLight.Services
{
    public class RawTicker
    {
        public RawTicker(string name, string format, byte[] data)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (format == null) throw new ArgumentNullException("format");
            if (data == null) throw new ArgumentNullException("data");

            this.Name = name;
            this.Format = format;
            this.Data = data;
        }

        public string Name { get; private set; }
        public string Format { get; private set; }
        public byte[] Data { get; private set; }
    }
}
