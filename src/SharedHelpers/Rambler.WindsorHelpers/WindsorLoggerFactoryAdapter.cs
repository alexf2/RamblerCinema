using System;
using System.Diagnostics;
using Microsoft.Owin.Logging;
using Rambler.WebApiHelpers;

namespace Rambler.WindsorHelpers
{
    public class WindsorLoggerFactoryAdapter: IExtendedLoggerFactory
    {
        readonly Castle.Core.Logging.ILoggerFactory _wfac;
        public WindsorLoggerFactoryAdapter(Castle.Core.Logging.ILoggerFactory wfac)
        {
            _wfac = wfac;
        }

        public ILogger Create (string name)
        {
            return new WindsorLoggerAdapter(_wfac.Create(name));
        }

        public IExtendedLogger CreateExt (string name)
        {
            return new WindsorLoggerAdapter(_wfac.Create(name));
        }

        sealed class WindsorLoggerAdapter: IExtendedLogger
        {
            readonly Castle.Core.Logging.ILogger _logger;

            public WindsorLoggerAdapter (Castle.Core.Logging.ILogger logger)
            {
                _logger = logger;
            }
           

            public bool WriteCore(
                TraceEventType eventType,
                int eventId,
                object state,
                Exception exception,
                Func<object, Exception, string> formatter)
            {
                Func<string> f;
                if (state == null)
                    f = () => string.Empty;
                else
                    f = () => formatter(state, exception);


                switch (eventType)
                {
                    case TraceEventType.Critical:
                        _logger.Fatal(f);
                        return state != null || _logger.IsFatalEnabled;

                    case TraceEventType.Error:
                        _logger.Error(f);
                        return state != null || _logger.IsErrorEnabled;

                    case TraceEventType.Warning:
                        _logger.Warn(f);
                        return state != null || _logger.IsWarnEnabled;

                    case TraceEventType.Information:
                    case TraceEventType.Start:
                    case TraceEventType.Stop:
                    case TraceEventType.Suspend:
                    case TraceEventType.Resume:
                    case TraceEventType.Transfer:
                        _logger.Info(f);
                        return state != null || _logger.IsInfoEnabled;

                    case TraceEventType.Verbose:
                        _logger.Debug(f);
                        return state != null || _logger.IsDebugEnabled;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(eventType));
                }
            }

            public void Debug(string message) => _logger.Debug(message);            
            public void Debug(Func<string> messageFactory) => _logger.Debug(messageFactory);
            public void Debug(string message, Exception exception) => _logger.Debug(message, exception);

            public void Error(string message) => _logger.Error(message);
            public void Error(Func<string> messageFactory) => _logger.Error(messageFactory);
            public void Error(string message, Exception exception) => _logger.Error(message, exception);

            public void Fatal(string message) => _logger.Fatal(message);
            public void Fatal(Func<string> messageFactory) => _logger.Fatal(messageFactory);
            public void Fatal(string message, Exception exception) => _logger.Fatal(message, exception);

            public void Info(string message) => _logger.Info(message);
            public void Info(Func<string> messageFactory) => _logger.Info(messageFactory);
            public void Info(string message, Exception exception) => _logger.Info(message, exception);

            public void Warn(string message) => _logger.Warn(message);
            public void Warn(Func<string> messageFactory) => _logger.Warn(messageFactory);
            public void Warn(string message, Exception exception) => _logger.Warn(message, exception);
        }
    }
}
