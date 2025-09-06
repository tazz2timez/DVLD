using System.Diagnostics;

namespace DVLD_DataAccess
{
    internal static class clsErrorsLogger
    {
        public static void LogError(string message)
        {
            string sourceName = "DVLD";
            string logName = "Application";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, logName);
            }

            // Log an error event
            EventLog.WriteEntry(sourceName, message, EventLogEntryType.Error);
        }

        public static void LogWarning() 
        {
            // I will implement it later
        }

        public static void LogInformation() 
        {
            // I will implement it later
        }


    }
}
