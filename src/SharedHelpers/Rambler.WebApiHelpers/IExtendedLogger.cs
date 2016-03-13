using System;

namespace Rambler.WebApiHelpers
{
    public interface IExtendedLogger: Microsoft.Owin.Logging.ILogger
    {        
        /// <summary>
        /// Logs a debug message.
        /// 
        /// </summary>
        /// <param name="message">The message to log</param>
        void Debug(string message);

        /// <summary>
        /// Logs a debug message with lazily constructed message. The message will be constructed only if the <see cref="P:Castle.Core.Logging.ILogger.IsDebugEnabled"/> is true.
        /// 
        /// </summary>
        /// <param name="messageFactory"/>
        void Debug(Func<string> messageFactory);

        /// <summary>
        /// Logs a debug message.
        /// 
        /// </summary>
        /// <param name="exception">The exception to log</param><param name="message">The message to log</param>
        void Debug(string message, Exception exception);

        

        /// <summary>
        /// Logs an error message.
        /// 
        /// </summary>
        /// <param name="message">The message to log</param>
        void Error(string message);

        /// <summary>
        /// Logs an error message with lazily constructed message. The message will be constructed only if the <see cref="P:Castle.Core.Logging.ILogger.IsErrorEnabled"/> is true.
        /// 
        /// </summary>
        /// <param name="messageFactory"/>
        void Error(Func<string> messageFactory);

        /// <summary>
        /// Logs an error message.
        /// 
        /// </summary>
        /// <param name="exception">The exception to log</param><param name="message">The message to log</param>
        void Error(string message, Exception exception);

        

        /// <summary>
        /// Logs a fatal message.
        /// 
        /// </summary>
        /// <param name="message">The message to log</param>
        void Fatal(string message);

        /// <summary>
        /// Logs a fatal message with lazily constructed message. The message will be constructed only if the <see cref="P:Castle.Core.Logging.ILogger.IsFatalEnabled"/> is true.
        /// 
        /// </summary>
        /// <param name="messageFactory"/>
        void Fatal(Func<string> messageFactory);

        /// <summary>
        /// Logs a fatal message.
        /// 
        /// </summary>
        /// <param name="exception">The exception to log</param><param name="message">The message to log</param>
        void Fatal(string message, Exception exception);

        

        /// <summary>
        /// Logs an info message.
        /// 
        /// </summary>
        /// <param name="message">The message to log</param>
        void Info(string message);

        /// <summary>
        /// Logs a info message with lazily constructed message. The message will be constructed only if the <see cref="P:Castle.Core.Logging.ILogger.IsInfoEnabled"/> is true.
        /// 
        /// </summary>
        /// <param name="messageFactory"/>
        void Info(Func<string> messageFactory);

        /// <summary>
        /// Logs an info message.
        /// 
        /// </summary>
        /// <param name="exception">The exception to log</param><param name="message">The message to log</param>
        void Info(string message, Exception exception);

        

        /// <summary>
        /// Logs a warn message.
        /// 
        /// </summary>
        /// <param name="message">The message to log</param>
        void Warn(string message);

        /// <summary>
        /// Logs a warn message with lazily constructed message. The message will be constructed only if the <see cref="P:Castle.Core.Logging.ILogger.IsWarnEnabled"/> is true.
        /// 
        /// </summary>
        /// <param name="messageFactory"/>
        void Warn(Func<string> messageFactory);

        /// <summary>
        /// Logs a warn message.
        /// 
        /// </summary>
        /// <param name="exception">The exception to log</param><param name="message">The message to log</param>
        void Warn(string message, Exception exception);
    }
}
