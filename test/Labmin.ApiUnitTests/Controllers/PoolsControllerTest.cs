using Labmin.Api.Services;
using Labmin.Core.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Labmin.Api.Controllers
{
    public class PoolsControllerTest
    {
        protected PoolsController ControllerUnderTest { get; }
        protected Mock<IPoolService> PoolServiceMock { get; }

        public PoolsControllerTest()
        {
            PoolServiceMock = new Mock<IPoolService>();
            ControllerUnderTest = new PoolsController(PoolServiceMock.Object);
        }

        public class ReadAllAsync : PoolsControllerTest
        {
            [Fact]
            public async Task Should_return_all_Pools()
            {
                // Arrange
                var expectedPools = new Pool[]
                {
                    new Pool { Name = "testpool1.local" },
                    new Pool { Name = "testpool2.local" },
                    new Pool { Name = "testpool3.local" }
                };
                PoolServiceMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedPools);

                // Act
                var result = await ControllerUnderTest.GetPools();

                // Assert
                Assert.Same(expectedPools, result);
            }
        }

        public class ReadOneAsync : PoolsControllerTest
        {
            [Fact]
            public async Task Should_return_expected_Pool()
            {
                // Arrange
                var expectedPool = new Pool { Name = "testpool1.local" };

                PoolServiceMock
                    .Setup(x => x.ReadOneAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);

                // Act
                var result = await ControllerUnderTest.GetPool(expectedPool.Name);

                Assert.Same(expectedPool, result);
            }
        }
    }
}
