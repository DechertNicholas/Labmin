﻿using Labmin.Api.Repositories;
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

        public class ReadAllAsync : PoolServiceTest
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
                PoolRepositoryMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedPools);

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Collection(result,
                    pool => Assert.Same(expectedPools[0], pool),
                    pool => Assert.Same(expectedPools[1], pool),
                    pool => Assert.Same(expectedPools[2], pool)
                );
            }
        }

        public class ReadOneAsync : PoolServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_Pool()
            {
                // Arrange
                var expectedPool = new Pool { Name = "testpool1.local" };
                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(expectedPool.Name))
                    .ReturnsAsync(expectedPool);

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(expectedPool.Name);

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
                var exception = await Assert.ThrowsAsync<PoolNotFoundException>(() => ServiceUnderTest.ReadOneAsync(fakePool.Name));
            }
        }

        public class UpdateAsync : PoolServiceTest
        {
            [Fact]
            public async Task Should_return_the_updated_Pool()
            {
                // in prod we probably don't want to allow users to rename pools, but this test is just for updating an entity
                // Arrange
                var poolInRepository = new Pool { Id = 1, Name = "preupdatepool.local" };
                var poolWithUpdates = new Pool { Name = "postupdatepool.local" };
                var poolInRepositoryWithUpdates = new Pool { Id = 1, Name = poolWithUpdates.Name };

                PoolRepositoryMock
                    .Setup(x => x.ReadOneAsync(poolWithUpdates.Name))
                    .ReturnsAsync(poolInRepository);
                PoolRepositoryMock
                    .Setup(x => x.UpdateAsync(poolWithUpdates))
                    .ReturnsAsync(poolInRepositoryWithUpdates);

                // Act
                var result = await ServiceUnderTest.UpdateAsync(poolWithUpdates);

                // Assert
                Assert.Same(poolInRepositoryWithUpdates, result);
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
                var exception = await Assert.ThrowsAsync<PoolNotFoundException>(() => ServiceUnderTest.ReadOneAsync(fakePool.Name));
            }
        }
    }
}
