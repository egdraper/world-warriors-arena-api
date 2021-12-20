using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace WWA.RestApi.Filters
{
    public class LoggingCallFilter : IIncomingGrainCallFilter
    {
        private readonly ILogger<LoggingCallFilter> _logger;

        public LoggingCallFilter(ILogger<LoggingCallFilter> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                await context.Invoke();
            }
            catch (Exception e)
            {
                var grainId = context.Grain is IGrainWithStringKey
                    ? context.Grain.GetPrimaryKeyString()
                    : String.Empty;
                _logger.LogError(e, "{GrainType}({GrainId}).{GrainMethod}({GrainArgs}) threw an exception: {Error}",
                    context.Grain.GetType(),
                    grainId,
                    context.InterfaceMethod.Name,
                    string.Join(",", context.Arguments ?? new object[] { }),
                    e.Message);
                throw;
            }
        }
    }
}
