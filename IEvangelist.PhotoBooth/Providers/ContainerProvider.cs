using System;
using System.Threading;
using System.Threading.Tasks;
using Centare.Extensions;
using IEvangelist.PhotoBooth.Configuration;
using IEvangelist.PhotoBooth.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace IEvangelist.PhotoBooth.Providers
{
    public class ContainerProvider : IContainerProvider
    {
        private static AsyncLazy<CloudBlobContainer> _initialization;

        public ContainerProvider(
            IOptions<ImageRepositoryOptions> repositoryOptions,
            ILogger<ContainerProvider> logger)
        {
            var repositoryOptionsValue = repositoryOptions?.Value ?? throw new ArgumentNullException(nameof(repositoryOptions));
            
            Interlocked.Exchange(ref _initialization, new AsyncLazy<CloudBlobContainer>(InitializeAsync));

            async Task<CloudBlobContainer> InitializeAsync()
            {
                var connectionString = Environment.GetEnvironmentVariable(repositoryOptionsValue.ConnectionStringKey);
                if (!CloudStorageAccount.TryParse(connectionString, out var account))
                {
                    throw new Exception("Unable to connect to Azure Storage Account Container!");
                }

                try
                {
                    var client = account.CreateCloudBlobClient();
                    var container = client.GetContainerReference(repositoryOptionsValue.ContainerName);
                    await container.CreateIfNotExistsAsync();
                    await container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });

                    return container;
                }
                catch (Exception ex)
                {
                    ex.TryLogException(logger);
                    throw;
                }
            }
        }

        public Task<CloudBlobContainer> GetContainerAsync() => _initialization.Value;
    }
}