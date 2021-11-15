using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Schema.Identity;
using Weasel.Postgresql;

namespace MyLib
{
    public class Class1
    {
        private readonly DocumentStore _documentStore;

        private const string DatabaseConnectionString =
            "Host=localhost;Username=postgres;Password=password;port=5432;Database=test_db1";

        public Class1()
        {
            _documentStore = DocumentStore.For(options =>
            {
                options.Connection(DatabaseConnectionString);
                options.CreateDatabasesForTenants(c =>
                {
                    c.ForTenant()
                        .CheckAgainstPgDatabase()
                        .ConnectionLimit(-1);
                });
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

                options.Schema.For<SomeEntity>()
                    .Identity(x => x.Id).IdStrategy(new NoOpIdGeneration())
                    .UseOptimisticConcurrency(true)
                    .Index(x => x.Id);
            });
        }

        public async Task<SomeEntity> Run(CancellationToken ct)
        {
            await using var session = _documentStore.OpenSession();
            var user = await session.LoadAsync<SomeEntity>(Guid.NewGuid(), ct);
            return user;
        }
    }

    public class SomeEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
