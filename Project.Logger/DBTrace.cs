namespace Project.Logger
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class DBTrace : DbCommandInterceptor
    {
        private DateTime startTime;

        private void DbQueryLogs(string text, DbCommand command, bool isExecuting, bool isSuccess, bool isFail)
        {
            //  EF Database call took
            TimeSpan duration;
            string message = string.Empty;
            if (isExecuting)
            {
                this.startTime = DateTime.UtcNow;
                message = string.Format(text + "start at {0}.\r\nCommand : \r\n{1} ", this.startTime, command != null ? command.CommandText : "N/A");
            }
            else if (isSuccess)
            {
                duration = DateTime.UtcNow - this.startTime;
                message = string.Format(text + "took {0} sec.\r\nCommand:\r\n{1}", duration.TotalSeconds.ToString("N3"), command != null ? command.CommandText : "N/A");
            }
            else if (isFail)
            {
                duration = DateTime.UtcNow - this.startTime;
                message = string.Format(text + "failed after {0} sec.\r\nCommand:\r\n{1}", duration.TotalSeconds.ToString("N3"), command != null ? command.CommandText : "N/A");
            }

            new LoggerManager().LogTrace(message);
        }

/*        private async Task<int> DbQueryLogsAsync(string text, DbCommand command, bool isExecuting, bool isSuccess, bool isFail)
        {
            //  EF Database call took
            TimeSpan duration;
            string message = string.Empty;
            if (isExecuting)
            {
                this.startTime = DateTime.UtcNow;
                message = string.Format(text + "start at {0}.\r\nCommand : \r\n{1} ", this.startTime, command != null ? command.CommandText : "N/A");
            }
            else if (isSuccess)
            {
                duration = DateTime.UtcNow - this.startTime;
                message = string.Format(text + "took {0} sec.\r\nCommand:\r\n{1}", duration.TotalSeconds.ToString("N3"), command != null ? command.CommandText : "N/A");
            }
            else if (isFail)
            {
                duration = DateTime.UtcNow - this.startTime;
                message = string.Format(text + "failed after {0} sec.\r\nCommand:\r\n{1}", duration.TotalSeconds.ToString("N3"), command != null ? command.CommandText : "N/A");
            }

            new LoggerManager().LogTrace(message);
            
            new LoggerManager().LogSwallowAsync(new Task);

            return 1;
        }*/

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            this.DbQueryLogs("EF Database ReaderExecuting call ", command, isExecuting: true, isSuccess: false, isFail: false);
            return result;
        }

        public override System.Data.Common.DbDataReader ReaderExecuted(System.Data.Common.DbCommand command,
          Microsoft.EntityFrameworkCore.Diagnostics.CommandExecutedEventData eventData,
          System.Data.Common.DbDataReader result)
        {
            this.DbQueryLogs("EF Database ReaderExecuted call ", command, isExecuting: false, isSuccess: true, isFail: false);
            return result;
        }


        public override System.Data.Common.DbCommand CommandCreated(
              Microsoft.EntityFrameworkCore.Diagnostics.CommandEndEventData eventData,
              System.Data.Common.DbCommand result)
        {
            this.DbQueryLogs("EF Database CommandCreated call ", null, isExecuting: false, isSuccess: true, isFail: false);
            return result;
        }

        public override Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<System.Data.Common.DbCommand> CommandCreating(
            Microsoft.EntityFrameworkCore.Diagnostics.CommandCorrelatedEventData eventData,
            Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<System.Data.Common.DbCommand> result)
        {
            this.DbQueryLogs("EF Database CommandCreating call ", null, isExecuting: true, isSuccess: false, isFail: false);
            return result;
        }

        public override void CommandFailed(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandErrorEventData eventData)
        {
            this.DbQueryLogs("EF Database CommandFailed call ", command, isExecuting: false, isSuccess: false, isFail: true);
        }

        public override Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult DataReaderDisposing(System.Data.Common.DbCommand command,
            Microsoft.EntityFrameworkCore.Diagnostics.DataReaderDisposingEventData eventData, Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult result)
        {
            this.DbQueryLogs("EF Database DataReaderDisposing call ", command, isExecuting: true, isSuccess: false, isFail: false);
            return result;
        }

        public override Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<int> NonQueryExecuting(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandEventData eventData, Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<int> result)
        {
            this.DbQueryLogs("EF Database NonQueryExecuting call ", command, isExecuting: true, isSuccess: false, isFail: false);
            return result;
        }

        public override int NonQueryExecuted(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandExecutedEventData eventData, int result)
        {
            this.DbQueryLogs("EF Database NonQueryExecuted call ", command, isExecuting: false, isSuccess: true, isFail: false);
            return result;
        }

        public override object ScalarExecuted(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandExecutedEventData eventData, object result)
        {
            this.DbQueryLogs("EF Database ScalarExecuted call ", command, isExecuting: false, isSuccess: true, isFail: false);
            return result;
        }

        /*---------------------
        public override System.Threading.Tasks.Task CommandFailedAsync(System.Data.Common.DbCommand command,
            Microsoft.EntityFrameworkCore.Diagnostics.CommandErrorEventData eventData,
            System.Threading.CancellationToken cancellationToken)
        {
            this.DbQueryLogs("EF Database CommandFailedAsync call ", command, isExecuting: false, isSuccess: false, isFail: true);
            return null;
        }

        public override System.Threading.Tasks.Task<Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<int>> NonQueryExecutingAsync(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandEventData eventData, Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<int> result, System.Threading.CancellationToken cancellationToken)
        {
            this.DbQueryLogs("EF Database NonQueryExecutingAsync call ", command, isExecuting: true, isSuccess: false, isFail: false);
            return null;
        }

        public override System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ReaderExecutedAsync(System.Data.Common.DbCommand command,
            Microsoft.EntityFrameworkCore.Diagnostics.CommandExecutedEventData eventData, System.Data.Common.DbDataReader result, System.Threading.CancellationToken cancellationToken)
        {
            this.DbQueryLogs("EF Database ReaderExecutedAsync call ", command, isExecuting: false, isSuccess: true, isFail: false);
            return null;
        }

        public override System.Threading.Tasks.Task<int> NonQueryExecutedAsync(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandExecutedEventData eventData, int result, System.Threading.CancellationToken cancellationToken)
        {
            await this.DbQueryLogsAsync("EF Database NonQueryExecutedAsync call ", command, isExecuting: false, isSuccess: true, isFail: false);
            return result;
        }

        public override System.Threading.Tasks.Task<Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<System.Data.Common.DbDataReader>> ReaderExecutingAsync(System.Data.Common.DbCommand command, Microsoft.EntityFrameworkCore.Diagnostics.CommandEventData eventData, Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult<System.Data.Common.DbDataReader> result, System.Threading.CancellationToken cancellationToken)
        {
            this.DbQueryLogs("EF Database ReaderExecutingAsync call ", command, isExecuting: true, isSuccess: false, isFail: false);
            return null;
        }

    --------------*/
    }
}
