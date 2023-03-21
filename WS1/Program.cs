using System.Buffers.Text;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using WS1.DataUtils;
using WS1.ModelClasses;
using WS1.RabbitMQ;
using WS1.UtilsClasses;
using WS1.XMLTools;

namespace WS1
{
    public static class Program
    {
        public static MilitaryBase milbase;
        static void Main(string[] args)
        {
            RMQClient client = new RMQClient();
            Console.ReadLine();
            client.CloseConnection();

            //  MockXML();
            //  Console.WriteLine(new XSDValidator().ValidateXML(@"C:\Users\user\source\repos\WS1\WS1.Tests\bin\Debug\TestAssets\final2.xsd", @"D:\MyFolder\final1.xml"));

            //  Task task = SendBaseToDBAsync(milbase);
            //  Console.ReadLine();
            //  @"D:\MyFolder\final1.xml"));
        }
        /// Further functions are for testing purposes
        public static void MockXML()
        {
            Solider sol1 = new Solider(1, "Oleh", "Halytskyi", Enums.Rank.Major);
            Solider sol2 = new Solider(1, "Ihor", "Haly", Enums.Rank.Captain);
            Solider sol3 = new Solider(1, "Jake", "Meoff", Enums.Rank.Private);
            Solider sol4 = new Solider(1, "Jacks", "Bee", Enums.Rank.Sergeant);
            Solider sol5 = new Solider(1, "SSee", "Shard", Enums.Rank.Major);

            Vehicle vehicle1 = new Vehicle(1, "Leopard 2 A7", 228, sol1);
            Vehicle vehicle2 = new Vehicle(1, "Leopard 1 A6", 2222, sol2);
            Vehicle vehicle3 = new Vehicle(1, "Leopard 3 A5", 345, sol3);
            Vehicle vehicle4 = new Vehicle(1, "Leopard 87 A4", 244, sol4);
            Vehicle vehicle5 = new Vehicle(1, "Leopard 6 A4", 26, sol5);

            List<Solider> baseSols = new List<Solider>();
            List<Vehicle> baseVeh = new List<Vehicle>();

            baseSols.Add(sol1);
            baseSols.Add(sol2);
            baseSols.Add(sol3);
            baseSols.Add(sol4);
            baseSols.Add(sol5);

            baseVeh.Add(vehicle1);
            baseVeh.Add(vehicle2);
            baseVeh.Add(vehicle3);
            baseVeh.Add(vehicle4);
            baseVeh.Add(vehicle5);

            MilitaryBase mb = new MilitaryBase("MAIN UKRAINE BASE","KIEV, UA",sol1,baseSols,baseVeh);
            new XMLSerializer().SerializeObject(mb, @"D:\MyFolder\final1.xml");
        }
        public static async Task SendBaseToDBAsync(MilitaryBase milbase)
        {
            DataAccess da = new DataAccess();
            await da.SaveData("dbo.spBases_Insert", new { milbase.Name, milbase.Location, captainID = milbase.Captain.Id });
            foreach(Vehicle veh in milbase.Vehicles)
            {
               await da.SaveData("dbo.spVehicles_Insert", new { veh.Name, veh.Weight, CaptainID = veh.Captain.Id, veh.BaseID });
            }
            foreach(Solider sol in milbase.Soliders)
            {
                await da.SaveData("dbo.spSoliders_Insert", new { sol.BaseId, sol.Name, sol.Surname, rank = sol.Rank.ToString() });
            }
        }
    }
}