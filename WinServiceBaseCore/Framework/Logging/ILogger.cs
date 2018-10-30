using System;

namespace WinServiceBaseCore.Framework.Logging
{
    public interface ILogger
    {
        void Trace( string format, params object[] args );
        void Debug( string format, params object[] args );
        void Info( string format, params object[] args );
        void Warn( string format, params object[] args );
        void Error( string format, params object[] args );
        void ErrorException( Exception err, string format, params object[] args );
        void Fatal( string format, params object[] args );
        void FatalException( Exception err, string format, params object[] args );
    }
}
