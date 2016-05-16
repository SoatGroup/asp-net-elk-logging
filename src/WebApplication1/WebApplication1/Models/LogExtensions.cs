using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using log4net.Core;
using Microsoft.VisualBasic;

namespace WebApplication1.Models
{
    public static class LogExtensions
    {
        private static void LogEvent(this ILog log, string message, IDictionary<string, object> properties, Level level, Exception ex = null)
        {
            var logEvent = new LoggingEvent(log.GetType(), log.Logger.Repository, log.Logger.Name, level, message, ex);
            foreach (var property in properties)
            {
                string key = property.Key;
                if (key != "type")
                {
                    key = CreateSafeKey(property);
                }
                logEvent.Properties[key] = property.Value;
            }
            log.Logger.Log(logEvent);
        }

        private static string CreateSafeKey(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var prefixeKey = "o-";
            if (Information.IsNumeric(kvp.Value ))
                prefixeKey = "n-";
            if (kvp.Value is string)
                prefixeKey = "s-";
            if (kvp.Value is bool)
                prefixeKey = "b-";
            if (kvp.Value is DateTime)
                prefixeKey = "d-";

            return prefixeKey+key;
        }

        public static void LogTechnicalInfo(this ILog log, string message, IDictionary<string, object> properties)
        {
            var level = Level.Info;
            properties["type"] = "technical";
            log.LogEvent(message, properties, level);
        }
        public static void LogTechnicalWarn(this ILog log, string message, IDictionary<string, object> properties)
        {
            var level = Level.Warn;
            properties["type"] = "technical";
            log.LogEvent(message, properties, level);
        }

        public static void LogTechnicalError(this ILog log, string message, IDictionary<string, object> properties, Exception exception)
        {
            var level = Level.Error;
            properties["type"] = "technical";
            log.LogEvent(message, properties, level, exception);
        }

        public static void LogBusiness(this ILog log, string message, IDictionary<string, object> properties)
        {
            var level = Level.Info;
            properties["type"] = "business";
            log.LogEvent(message, properties, level);
        }

        public static void LogPerf(this ILog log, long duration, IDictionary<string, object> properties)
        {
            var level = Level.Info;
            if (duration > 1000)
                level = Level.Warn;
            properties["duration"] = duration;
            properties["type"] = "performance";
            log.LogEvent($"Action took {duration} ms to be executed", properties, level);
        }

        public static IDictionary<string, object> ToDictionary(this NameValueCollection nvc)
        {
            var props = new Dictionary<string, object>();
            foreach (var prop in nvc.AllKeys)
            {
                props.Add(prop.ToLowerInvariant(), nvc[prop]);
            }

            return props;
        }

        public static IDictionary<string, object> ToDictionary(this RouteValueDictionary rvd)
        {
            var props = new Dictionary<string, object>();
            foreach (var prop in rvd)
            {
                props.Add(prop.Key.ToLowerInvariant(), prop.Value);
            }
            return props;
        }

        public static IDictionary<string, object> ToDictionary(this ModelStateDictionary msd)
        {
            var props = new Dictionary<string, object>();
            foreach (var prop in msd.Keys)
            {
                props.Add(prop.ToLowerInvariant(), msd[prop]);
            }

            return props;
        }
    }
}
