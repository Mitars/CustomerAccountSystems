using Cas.Account.Api;
using Cas.Account.Core.Dtos;
using Cas.Account.Core.Interfaces;
using Cas.Account.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Cas.Account.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            var customerAccessorMock = new Mock<ICustomerAccessor>();
            customerAccessorMock.Setup(customerAccessor => customerAccessor.Exists(It.IsInRange<long>(1, 10, Moq.Range.Inclusive)))
                .ReturnsAsync(() => true);
            customerAccessorMock.Setup(customerAccessor => customerAccessor.Exists(0))
                .ReturnsAsync(() => false);

            var transactionAccessorMock = new Mock<ITransactionAccessor>();
            transactionAccessorMock.Setup(transactionAccessor => transactionAccessor.Create(It.IsAny<TransactionToCreateDto>()))
                .ReturnsAsync((TransactionToCreateDto transaction) => new TransactionDetailsDto
                {
                    Id = 1,
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    Created = DateTime.UtcNow,
                });
            transactionAccessorMock.Setup(transactionAccessor => transactionAccessor.GetByAccountId(It.IsAny<long>()))
                .ReturnsAsync((long accountId) => new List<TransactionDetailsDto> {
                new TransactionDetailsDto
                {
                    Id = 1,
                    Amount = 500,
                    Description = "Test Account",
                    Created = DateTime.UtcNow,
                }});

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

                        services.RemoveAll(typeof(ICustomerAccessor));
                        services.AddSingleton(serviceProvider => customerAccessorMock.Object);

                        services.RemoveAll(typeof(ITransactionAccessor));
                        services.AddScoped(serviceProvider => transactionAccessorMock.Object);
                    }).Build();
                });
            this.Client = appFactory.CreateClient();
        }

        public HttpClient Client { get; }
    }
}