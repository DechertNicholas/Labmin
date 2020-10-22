using Labmin.Api.Repositories;
using Labmin.Core.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Labmin.Api.Services
{
    public class PoolServiceTest
    {
        protected PoolService ServiceUnderTest { get; }
        protected Mock<IRepository<Pool>> PoolRepositoryMock { get; }

        public PoolServiceTest()
        {
            PoolRepositoryMock = new Mock<IRepository<Pool>>();
            ServiceUnderTest = new PoolService(PoolRepositoryMock.Object);
        }

        public class CreateAsync : PoolServiceTest
        {
            [Fact]
            public async Task Should_return_the_created_Pool()
            {
                // Arrange
                var expectedPool = new Pool { Name = "TestPool1.local" };
                PoolRepositoryMock
                    .Setup(x => x.CreateAsync(expectedPool))
                    .ReturnsAsync(expectedPool);

                // Act
                var result = await ServiceUnderTest.CreateAsync(expectedPool);

                // Assert
                Assert.Same(expectedPool, result);
            }

            [Fact]
            public async Task Should_throw_PoolAlreadyExistsException_if_Pool_exists()
            {
                // Arrange
                var expectedPool = new Pool { Name = "TestPool1.local" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);

                // Act, Assert
                var exception = await Assert.ThrowsAsync<PoolAlreadyExistsException>(() => ServiceUnderTest.CreateAsync(expectedPool));
            }
        }

        public class IsPoolExistsAsync : PoolServiceTest
        {
            [Fact]
            public async Task Should_return_true_if_the_Pool_exists()
            {
                // Arrange
                var expectedPool = new Pool { Name = "testpool1.local" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);

                // Act
                var result = await ServiceUnderTest.IsPoolExistsAsync(expectedPool.Name);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_if_the_Pool_not_exist()
            {
                // Arrange
                var fakePool = new Pool { Name = "FakePool" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(fakePool.Name))
                    .ReturnsAsync(() => null);

                // Act
                var result = await ServiceUnderTest.IsPoolExistsAsync(fakePool.Name);

                // Assert
                Assert.False(result);
            }
        }

        public class DeleteAsync : PoolServiceTest
        {
            [Fact]
            public async Task Should_return_the_deleted_Pool()
            {
                // Arrange
                var expectedPool = new Pool { Name = "testpool1.local" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);
                PoolRepositoryMock
                    .Setup(x => x.DeleteAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);

                // Act
                var result = await ServiceUnderTest.DeleteAsync(expectedPool.Name);

                // Assert
                Assert.Same(expectedPool, result);
            }

            [Fact]
            public async Task Should_throw_PoolNotFoundException_if_Pool_not_exist()
            {
                // Arrange
                var fakePool = new Pool { Name = "fakepool" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(fakePool.Name))
                    .ReturnsAsync(() => null);

                // Act, Assert
                var exception = await Assert.ThrowsAsync<PoolNotFoundException>(() => ServiceUnderTest.DeleteAsync(fakePool.Name));
            }
        }
    }
}
