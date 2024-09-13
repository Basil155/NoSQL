using System;
using System.Net.Http;
using Pcf.GivingToCustomer.IntegrationTests.Data;

namespace Pcf.GivingToCustomer.IntegrationTests
{
    public class EfDatabaseFixture: IDisposable
    {
        private readonly EfTestDbInitializer _efTestDbInitializer;
        
        public EfDatabaseFixture()
        {
            DbContext = new TestDataContext();

            _efTestDbInitializer= new EfTestDbInitializer(DbContext);
            _efTestDbInitializer.InitializeDb();

            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri("http://localhost:8094/");
        }

        public void Dispose()
        {
            _efTestDbInitializer.CleanDb();
        }

        public TestDataContext DbContext { get; private set; }

        public HttpClient HttpClient { get; private set; }
    }
}