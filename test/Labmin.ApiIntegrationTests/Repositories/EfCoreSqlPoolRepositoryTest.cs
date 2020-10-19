using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labmin.Api.Data;
using Labmin.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Labmin.Api.Repositories.EfCore
{
    public class EfCoreSqlPoolRepositoryTest : IDisposable
    {
        protected EfCoreSqlPoolRepository RepositoryUnderTest { get; }
        protected DbContext Context { get; }
        protected IConfiguration Configuration { get; }

        public EfCoreSqlPoolRepositoryTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = configBuilder.Build();

            // Build our SQL string. Eventually this will change so tests can be run locally by different people
            var sqlString = new SqlConnectionStringBuilder(Configuration.GetConnectionString("LabminDBContext"))
            {
                ApplicationName = "Labmin Repository Test Dev",
                InitialCatalog = $"Labmin_Development_{Environment.MachineName}",
                IntegratedSecurity = true
            };

            // Build our DbContext
            var builder = new DbContextOptionsBuilder<LabminDbContext>();
            builder.UseSqlServer(sqlString.ConnectionString)
                .UseInternalServiceProvider(serviceProvider);

            // Assign our properties and ensure our test database is created and migrated
            Context = new LabminDbContext(builder.Options);
            RepositoryUnderTest = new EfCoreSqlPoolRepository(Context);
            Context.Database.EnsureCreated();
            Context.Database.Migrate();

        }

        public void Dispose()
        {
            Context.Database.CloseConnection();
        }

        /*
         * Running these tests in parallel isn't allowed, as data is seeded and removed per test.
         * In parallel, more data is seeded than each test expectes to read, so read tests fail,
         * update tests fail, and duplicate data is added.
         */

        [Collection("Non-Parallel SQL Collection 1")]
        public class ReadAllAsync : EfCoreSqlPoolRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_expected_Pools()
            {
                // Arrange
                var expectedPools = new List<Pool>();
                var poolsToAdd = new Pool[]
                {
                    new Pool { Name = "testpool1.local" },
                    new Pool { Name = "testpool2.local" },
                    new Pool { Name = "testpool3.local" }
                };

                // Add Data
                foreach (var pool in poolsToAdd)
                {
                    expectedPools.Add(Context.Set<Pool>().Add(pool).Entity);
                    await Context.SaveChangesAsync();
                }

                // Act
                var result = await RepositoryUnderTest.ReadAllAsync();

                // Cleanup
                foreach (var pool in expectedPools)
                {
                    Context.Set<Pool>().Remove(pool);
                    await Context.SaveChangesAsync();
                }

                // Assert
                Assert.Collection(result,
                    pool => Assert.Same(expectedPools[0], pool),
                    pool => Assert.Same(expectedPools[1], pool),
                    pool => Assert.Same(expectedPools[2], pool)
                );
            }

        }

        [Collection("Non-Parallel SQL Collection 1")]
        public class ReadOneAsync : EfCoreSqlPoolRepositoryTest
        {
            [Fact]
            public async Task Should_return_the_expected_Pool()
            {
                // Arrange
                var poolToAdd = new Pool { Name = "testpool1.local" };

                // Add data
                var expectedPool = Context.Set<Pool>().Add(poolToAdd);
                await Context.SaveChangesAsync();

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(poolToAdd.Name);

                // Cleanup
                Context.Set<Pool>().Remove(expectedPool.Entity);
                await Context.SaveChangesAsync();

                // Assert
                Assert.Same(expectedPool.Entity, result);
            }

            [Fact]
            public async Task Should_return_null_if_the_Pool_does_not_exist()
            {
                // Arrange
                var unexistingPool = "notapool";

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(unexistingPool);

                // Assert
                Assert.Null(result);
            }
        }

        [Collection("Non-Parallel SQL Collection 1")]
        public class CreateAsync : EfCoreSqlPoolRepositoryTest
        {
            [Fact]
            public async Task Should_create_the_expected_Pool()
            {
                // Arrange
                var poolToCreate = new Pool { Name = "testpool1.local" };

                // Act
                var createdPool = await RepositoryUnderTest.CreateAsync(poolToCreate);
                var result = await RepositoryUnderTest.ReadOneAsync(createdPool.Name);

                // Cleanup
                Context.Set<Pool>().Remove(createdPool);
                await Context.SaveChangesAsync();

                // Assert
                Assert.Same(createdPool, result);
            }

            [Fact]
            public async Task Should_return_the_existing_Pool_if_Pool_already_exists()
            {
                // Arrange
                var poolToAdd = new Pool { Name = "testpool1.local" };

                // Add data
                var expectedPool = Context.Set<Pool>().Add(poolToAdd);
                await Context.SaveChangesAsync();

                // Act
                var result = await RepositoryUnderTest.CreateAsync(poolToAdd);

                // Cleanup
                Context.Set<Pool>().Remove(expectedPool.Entity);
                await Context.SaveChangesAsync();

                // Assert
                Assert.Same(expectedPool.Entity, result);
            }


        }

        [Collection("Non-Parallel SQL Collection 1")]
        public class UpdateAsync : EfCoreSqlPoolRepositoryTest
        {
            [Fact]
            public async Task Should_update_the_existing_Pool()
            {
                // Arrange
                var poolToAdd = new Pool { Name = "testpool1.local" };

                // Add data
                var expectedPool = Context.Set<Pool>().Add(poolToAdd);
                await Context.SaveChangesAsync();

                // Act
                expectedPool.Entity.Name = "updatedtestpool1.local";
                var result = await RepositoryUnderTest.UpdateAsync(expectedPool.Entity);

                // Cleanup
                Context.Set<Pool>().Remove(expectedPool.Entity);
                await Context.SaveChangesAsync();

                // Assert
                Assert.Same(expectedPool.Entity, result);
            }

            [Fact]
            public async Task Should_return_null_if_Pool_not_exist()
            {
                // Arrange
                var fakePool = new Pool { Name = "fakepool" };

                // Act
                var result = await RepositoryUnderTest.UpdateAsync(fakePool);

                // Assert
                Assert.Null(result);
            }

        }

        [Collection("Non-Parallel SQL Collection 1")]
        public class DeleteAsync : EfCoreSqlPoolRepositoryTest
        {
            [Fact]
            public async Task Should_return_the_deleted_Pool()
            {
                // Arrange
                var poolToAdd = new Pool { Name = "testpool1.local" };

                // Add data
                var expectedPool = Context.Set<Pool>().Add(poolToAdd);
                await Context.SaveChangesAsync();

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(expectedPool.Entity.Name);

                // Assert
                Assert.Same(expectedPool.Entity, result);
            }

            [Fact]
            public async Task Should_return_null_if_Pool_not_exist()
            {
                // Arrange
                var fakePool = new Pool { Name = "fakepool" };

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(fakePool.Name);

                // Assert
                Assert.Null(result);
            }

        }
    }
}
