using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.Report
{
    public class ScalarReport
    {
        public ScalarReport(string category,string name,string value)
        {
            this.Category = category;
            this.Name = name;
            this.Value = value;
        }

        public string Category { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
    }

    public static class ScalarReportExtensions
    {
        public static IReadOnlyList<KeyValuePair<string, IReadOnlyList<KeyValuePair<string, string>>>> HierarchView(this IEnumerable<ScalarReport> items)
        {
            return
                items
                .GroupBy(i => i.Category)
                .Select(gi =>
                    new KeyValuePair<string, IReadOnlyList<KeyValuePair<string, string>>>(
                        gi.Key,
                        gi.Select(i => new KeyValuePair<string, string>(i.Name, i.Value))
                            .OrderBy(i => i.Key)
                            .AsReadOnlyList()))
                .OrderBy(gi => gi.Key)
                .AsReadOnlyList();
        }
    }
}
