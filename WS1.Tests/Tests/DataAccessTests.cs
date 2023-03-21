using Autofac.Extras.Moq;
using Moq;
using WS1.DataUtils;
using WS1.ModelClasses;
using Xunit;

namespace WS1.Tests.Tests
{
    /// <summary>
    /// Class for testing the DataAccess class
    /// </summary>
    public class DataAccessTests
    {
        [Fact]
        public async Task GeneralDataAccessSaveTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                EntranceLog log = new EntranceLog(DateTime.Today, "123");
                mock.Mock<IDataAccess>().Setup(x => x.SaveData("dbo.spELog_Insert", new { log.Log, log.Date }));
                var cls = mock.Create<DBprocessor>();

                cls.InsertLog(log);
                mock.Mock<IDataAccess>().Verify(x => x.SaveData("dbo.spELog_Insert", new { log.Log, log.Date }), Times.Exactly(1));
            }
        }
        [Fact]
        public void GeneralDataAccessLoadTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataAccess>().Setup(x => x.LoadData<Solider, dynamic>("dbo.spSoliders_GetAll", new { })).Returns(GetSoliders);

                var cls = mock.Create<DBprocessor>();
                var expected = GetSoliders();
                IEnumerable<Solider> enums = expected.Result;
                List<Solider> expectedlist = enums.ToList();
                var actual = cls.LoadSoliders();
                Assert.True(actual != null);
                Assert.Equal(expectedlist.Count, actual.Count);
                Assert.Equal(expectedlist[0].Name, actual[0].Name);
                Assert.Equal(expectedlist[1].Name, actual[1].Name);
                Assert.Equal(expectedlist[2].Name, actual[2].Name);
            }
        }
        /// <summary>
        /// Mocks the return query from database
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Solider>> GetSoliders()
        {
            var solider1 = new Solider(1, "Oleh", "Halytskyi", Enums.Rank.Major);
            var solider2 = new Solider(1, "Ihor", "Halytskyi", Enums.Rank.Captain);
            var solider3 = new Solider(1, "Olha", "Sharamba", Enums.Rank.Sergeant);
            var list = new List<Solider>();
            list.Add(solider3);
            list.Add(solider2);
            list.Add(solider1);
            return Task.FromResult<IEnumerable<Solider>>(list); ;
        }
    }
    /// <summary>
    /// Mock class that wraps around DataAccess class
    /// </summary>
    public class DBprocessor
    {
        /// <summary>
        /// Mock IDataAccess object
        /// </summary>
        private readonly IDataAccess daacc;
        public DBprocessor(IDataAccess dataAccess)
        {
            daacc = dataAccess;
        }
        /// <summary>
        /// Mock function for receiving soliders from database
        /// </summary>
        /// <returns></returns>
        public List<Solider> LoadSoliders()
        {
            Task<IEnumerable<Solider>> test = daacc.LoadData<Solider, dynamic>("dbo.spSoliders_GetAll", new { });
            IEnumerable<Solider> resutlts = test.Result;
            List<Solider> list = resutlts.ToList();
            return list;
        }
        /// <summary>
        /// Mock function for sending Log to the database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task InsertLog(EntranceLog log)
        {
            await daacc.SaveData("dbo.spELog_Insert", new { log.Log, log.Date });
        }
    }
}
