// <copyright file="MetricsHttpContextExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    public static class MetricsHttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__App.Metrics.CurrentRouteName__";
        private static readonly string MetricsTagPrefix = "__App.Metrics.TagPrefix__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            if (!context.Items.ContainsKey(MetricsCurrentRouteName))
            {
                context.Items.Add(MetricsCurrentRouteName, metricName);
            }
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            if (context.Items.TryGetValue(MetricsCurrentRouteName, out var item))
            {
                var route = item as string;
                if (route.IsPresent())
                {
                    return $"{context.Request.Method} {route}";
                }
            }

            return context.Request.Method;
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context) { return context.Items.ContainsKey(MetricsCurrentRouteName); }

        public static (string name, string value)[] GetMetricsTags(this HttpContext context) =>        
            context.Items
                .Where(i => (i.Key as string).StartsWith(MetricsTagPrefix))
                .Select(i => ((i.Key as string).Substring(MetricsTagPrefix.Length), i.Value as string)).ToArray();
        
    }
}