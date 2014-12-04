
using System;
using System.Collections.Generic;
using System.Text;


namespace wkHtmlToPdfSharp.Logging
{


    public class LogManager
    {

        private static ILog SetupLogger()
        {
            System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());

            return new ILog();
        }


        public static ILog CurrentLogger = SetupLogger();


        public static ILog GetCurrentClassLogger()
        {
            return CurrentLogger; 
        }

    }


    public class ILog
    {

        public bool IsTraceEnabled = false;

        private enum LogType_t
        {
             None
            ,Fatal
            ,Error
            ,Trace
            ,Warn
        }


        private void Log(LogType_t LogType, string Format, params object[] args)
        {
            string str = string.Format(Format, args);
            System.Diagnostics.Debug.WriteLine(string.Format("{0}:\n{1}", LogType.ToString(), str));
        }


        // _log.Fatal("Exception in SynchronizedDispatcherThread \"" + Thread.CurrentThread.Name + "\"", e);
        public void Fatal(string Format, params object[] args)
        {
            Log(LogType_t.Fatal, Format, args);
        }


        // _log.Error("T:" + Thread.CurrentThread.Name + " Conversion Error: " + errorText);
        public void Error(string Format, params object[] args)
        {
            Log(LogType_t.Error, Format, args);
        }


        // Log.Trace("T:" + Thread.CurrentThread.Name + " Deinitializing library (wkhtmltopdf_deinit)");
        public void Trace(string Format, params object[] args)
        {
            Log(LogType_t.Trace, Format, args);
        }


        public void Warn(string Format, params object[] args)
        {
            Log(LogType_t.Warn, Format, args);
        }

    }



}
