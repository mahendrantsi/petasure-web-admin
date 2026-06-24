using Microsoft.AspNetCore.Http;
using Project.Data.DBEntities;
using System.Threading.Tasks;

namespace Project.Web.WebResource.Middleware
{
    public class TransactionUnitMiddleware
    {
        private readonly RequestDelegate next;

        public TransactionUnitMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, ProjectDbContext context)
        {
            string httpVerb = httpContext.Request.Method.ToUpper();

            if (httpVerb == "POST" || httpVerb == "PUT")
            {
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync<object, object>(null!, operation: async (dbctx, state, cancel) =>
                {
                    await using var transaction = await context.Database.BeginTransactionAsync(); 
                    try
                    {
                        await next(httpContext).ContinueWith(async(x) => {
                            x.Wait();
                            await transaction.CommitAsync();
                        });

                        
                    }
                    catch (System.Exception ex)
                    {
                        await transaction.RollbackAsync();
                        using (var db = new ProjectDbContext())
                        {
                            db.AddAsync<ExceptionLogger>(new ExceptionLogger() { InnerException = ex.InnerException?.ToString(), Exception = ex.ToString() });
                            db.SaveChangesAsync();
                        }
                       
                    }

                    return null!;
                }, null);
            }
            else
            {
                await next(httpContext);
            }
        }
    }
}
