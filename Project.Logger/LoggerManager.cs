//  <copyright file="LoggerManager.cs" company="PlaceholderCompany">
//  Copyright (c) PlaceholderCompany. All rights reserved.
//  </copyright>

namespace Project.Logger
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class LoggerManager : ILoggerManager
    {
        private readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public void LogException(Exception exception)
        {
            this.logger.Error(exception);
        }

        public void LogInformation(string information)
        {
            this.logger.Info(information);
        }

        public void LogTrace(string trace)
        {
            this.logger.Trace(trace);
        }

        public async Task LogSwallowAsync(Task task)
        {
            await this.logger.SwallowAsync(task);
        }

        public void LogWarning(string warning)
        {
            this.logger.Warn(warning);
        }

        public void LogDebug(string debug)
        {
            this.logger.Debug(debug);
        }
    }
}
