using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WWA.RestApi.HostedServices
{
    public class ClusterClientService : IHostedService
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger<ClusterClientService> _logger;

        public ClusterClientService(IClusterClient clusterClient, ILogger<ClusterClientService> logger)
        {
            _clusterClient = clusterClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _clusterClient.Connect(async exception =>
            {
                _logger.LogWarning("Exception while attemtping to connect to Orleans cluster: {Exception}",
                    exception);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                return true;
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _clusterClient.Close();
        }
    }
}
