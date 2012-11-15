using System;

namespace IstLight.Services
{
    public interface IErrorReporter
    {
        void Add(string error);
        void Add(Exception error);
    }

    public static class ErrorReporterExtensions
    {
        public static void AddIfNotNull(this IErrorReporter errorReporter, string error)
        {
            if (error != null)
                errorReporter.Add(error);
        }

        public static void AddIfNotNull(this IErrorReporter errorReporter, Exception error)
        {
            if (error != null)
                errorReporter.Add(error);
        }
    }
}
