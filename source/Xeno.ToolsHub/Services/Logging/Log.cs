/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Log.cs
 * Rev:     3
 * Description:
 *  File and console logging system
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using Microsoft.Build.Utilities;
  using static System.Net.Mime.MediaTypeNames;

  public enum Level
  {
    /// <summary>Debug message.</summary>
    Debug,

    /// <summary>Informational message.</summary>
    Info,

    /// <summary>Warning message.</summary>
    Warn,

    /// <summary>An error has occurred.</summary>
    Error,

    /// <summary>Fatal-error.</summary>
    Fatal
  }

  public static class Log
  {
    private static readonly Assembly LogAssembly = typeof(Log).GetAssembly();
    private static readonly Assembly MscorlibAssembly = typeof(string).GetAssembly();
    private static readonly Assembly SystemAssembly = typeof(Debug).GetAssembly();

    private static ILogger _logDev = new FileLogger();
    private static Level _logLevel = Level.Debug;
    private static bool _muteLogging = false;

    public static ILogger LogDevice
    {
      get => _logDev;
      set => _logDev = value;
    }

    /// <summary>Gets or sets system-wide logging level.</summary>
    public static Level LogLevel
    {
      get => _logLevel;
      set => _logLevel = value;
    }

    /// <summary>Gets or sets a value indicating whether to output logging or not. True == disabled.</summary>
    public static bool MuteLogging
    {
      get => _muteLogging;
      set => _muteLogging = value;
    }

    private static string FormattedTime
    {
      get
      {
        return string.Format(
          "{0:00}:{1:00}:{2:00}.{3:000}",
          DateTime.Now.Hour,
          DateTime.Now.Minute,
          DateTime.Now.Second,
          DateTime.Now.Millisecond);
      }
    }

    public static void Debug(string message, params object[] args)
    {
      LogMessage(Level.Debug, message, args);
    }

    public static void Error(string message, params object[] args)
    {
      LogMessage(Level.Error, message, args);
    }

    public static void Fatal(string message, params object[] args)
    {
      LogMessage(Level.Fatal, message, args);
    }

    public static void Info(string message, params object[] args)
    {
      LogMessage(Level.Info, message, args);
    }

    private static Assembly GetAssembly(this Type type)
    {
      return type.GetTypeInfo().Assembly;
    }

    /// <summary>Extracts calling frame namespace and outputs.</summary>
    /// <param name="logLevel">Custom LogLevel.</param>
    /// <param name="message">User message.</param>
    private static void LogMessage(Level logLevel, string message, params object[] args)
    {
      // Enable when using nLog
      ////var nLogLevel = ToLogLevel(logLevel);

      var stackTrace = new System.Diagnostics.StackTrace();
      var loggerName = string.Empty;      // = (loggerName ?? Name) ?? string.Empty;
      var userFrameIndex = -1;

      // NLogTraceListener and StackTraceUsageUtils.LookupClassNameFromStackFrame(..)
      for (int i = 0; i < stackTrace.FrameCount; ++i)
      {
        var frame = stackTrace.GetFrame(i);
        loggerName = LookupClassNameFromStackFrame(frame);
        if (!string.IsNullOrEmpty(loggerName))
        {
          userFrameIndex = i;
          break;
        }
      }

      System.Diagnostics.Debug.WriteLine($">> [{FormattedTime}] [{logLevel}] [{loggerName}] [{message}");

      if (userFrameIndex >= 0)
      {
        // Enable when using nLog
        ////NLogger.Log(nLogLevel, $"{loggerName}] [{message}");
      }
    }

    public static void Warn(string message, params object[] args)
    {
      LogMessage(Level.Warn, message, args);
    }

    /// <summary>Returns the classname from the provided StackFrame (If not from internal assembly).</summary>
    /// <param name="stackFrame">StackFrame.</param>
    /// <returns>Valid class name, or empty string if assembly was internal.</returns>
    private static string LookupClassNameFromStackFrame(StackFrame stackFrame)
    {
      var method = stackFrame.GetMethod();
      if (method != null && LookupAssemblyFromStackFrame(stackFrame) != null)
      {
        string className = GetStackFrameMethodClassName(method, true, true, true);
        if (!string.IsNullOrEmpty(className))
        {
          if (!className.StartsWith("System.", StringComparison.Ordinal))
            return className;
        }
        else
        {
          className = method.Name ?? string.Empty;
          if (className != "lambda_method" && className != "MoveNext")
            return className;
        }
      }

      return string.Empty;
    }

    /// <summary>Returns the assembly from the provided StackFrame (If not internal assembly).</summary>
    /// <returns>Valid assembly, or null if assembly was internal.</returns>
    private static Assembly LookupAssemblyFromStackFrame(StackFrame stackFrame)
    {
      var method = stackFrame.GetMethod();
      if (method is null)
      {
        return null;
      }

      var assembly = method.DeclaringType?.GetAssembly() ?? method.Module?.Assembly;

      // skip stack frame if the method declaring type assembly is from hidden assemblies list
      if (assembly == LogAssembly)
      {
        return null;
      }

      if (assembly == MscorlibAssembly)
      {
        return null;
      }

      if (assembly == SystemAssembly)
      {
        return null;
      }

      return assembly;
    }

    private static string GetStackFrameMethodClassName(MethodBase method, bool includeNameSpace, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
    {
      if (method is null)
        return null;

      var callerClassType = method.DeclaringType;
      if (cleanAsyncMoveNext && method.Name == "MoveNext" && callerClassType?.DeclaringType != null && callerClassType.Name.IndexOf('<') == 0)
      {
        // NLog.UnitTests.LayoutRenderers.CallSiteTests+<CleanNamesOfAsyncContinuations>d_3'1
        int endIndex = callerClassType.Name.IndexOf('>', 1);
        if (endIndex > 1)
        {
          callerClassType = callerClassType.DeclaringType;
        }
      }

      if (!includeNameSpace
          && callerClassType?.DeclaringType != null
          && callerClassType.IsNested
          && callerClassType.GetFirstCustomAttribute<CompilerGeneratedAttribute>() != null)
      {
        return callerClassType.DeclaringType.Name;
      }

      string className = includeNameSpace ? callerClassType?.FullName : callerClassType?.Name;

      if (cleanAnonymousDelegates && className != null)
      {
        // NLog.UnitTests.LayoutRenderers.CallSiteTests+<>c__DisplayClassa
        int index = className.IndexOf("+<>", StringComparison.Ordinal);
        if (index >= 0)
        {
          className = className.Substring(0, index);
        }
      }

      return className;
    }

    /////// <summary>Converts custom logging level to NLog.LogLevel.</summary>
    /////// <param name="logLevel">Custom Log Level.</param>
    /////// <returns>NLog.LogLevel.</returns>
    ////private NL.LogLevel ToLogLevel(LogLevel logLevel)
    ////{
    ////  NL.LogLevel level = logLevel switch
    ////  {
    ////    LogLevel.Trace => NL.LogLevel.Trace,
    ////    LogLevel.Debug => NL.LogLevel.Debug,
    ////    LogLevel.Info => NL.LogLevel.Info,
    ////    LogLevel.Error => NL.LogLevel.Error,
    ////    LogLevel.Warn => NL.LogLevel.Warn,
    ////    LogLevel.Fatal => NL.LogLevel.Fatal,
    ////    _ => NL.LogLevel.Debug,
    ////  };
    ////
    ////  return level;
    ////}
  }

  public static class ReflectionExtension
  {
    public static TAttr GetFirstCustomAttribute<TAttr>(this Type type)
        where TAttr : Attribute
    {
      return Attribute.GetCustomAttributes(type, typeof(TAttr)).FirstOrDefault() as TAttr;
    }

    public static TAttr GetFirstCustomAttribute<TAttr>(this PropertyInfo info)
        where TAttr : Attribute
    {
      return Attribute.GetCustomAttributes(info, typeof(TAttr)).FirstOrDefault() as TAttr;
    }

    public static TAttr GetFirstCustomAttribute<TAttr>(this Assembly assembly)
        where TAttr : Attribute
    {
      return Attribute.GetCustomAttributes(assembly, typeof(TAttr)).FirstOrDefault() as TAttr;
    }
  }
}
