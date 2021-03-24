using Cas.Transaction.Api;
using Cas.Transaction.Core.Interfaces;
using Cas.Transaction.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.Net.Http;

namespace Cas.Transaction.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            var accountAccessorMock = new Mock<IAccountAccessor>();
            accountAccessorMock.Setup(accountAccessor => accountAccessor.Exists(It.IsInRange<long>(1, 10, Moq.Range.Inclusive)))
                .ReturnsAsync(() => true);
            accountAccessorMock.Setup(accountAccessor => accountAccessor.Exists(0))
                .ReturnsAsync(() => false);

            string databaseName = Guid.NewGuid().ToString();
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName),
                            ServiceLifetime.Scoped,
                            ServiceLifetime.Scoped);

                        services.RemoveAll(typeof(IAccountAccessor));
                        services.AddScoped(serviceProvider => accountAccessorMock.Object);
                    });
                });
            this.Client = appFactory.CreateClient();
        }

        public HttpClient Client { get; }
    }
}