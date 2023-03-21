using WS1.Enums;
using WS1.ModelClasses;
using WS1.UtilsClasses;
using Xunit;

namespace WS1.Tests.Tests
{
    /// <summary>
    /// Tests for xml serializer
    /// </summary>
    public class XMLToolsTests
    {
        string AssetsPath = "..\\TestAssets\\";
        [Fact]
        public void TestSerialization()
        {
            XMLTools.XMLSerializer serializer = new XMLTools.XMLSerializer();
            Solider sol = new Solider(1, "Oleh", "Halytskyi", Rank.Captain);
            serializer.SerializeObject(sol, AssetsPath + "Test.xml");
            string test = FileManager.SelectFile(AssetsPath + "Test.xml");
            Assert.Contains("Halytskyi", test);
            Assert.Contains("Oleh", test);
            Assert.Contains("Captain", test);
        }
        [Fact]
        public void TestDeserilization()
        {
            XMLTools.XMLSerializer serializer = new XMLTools.XMLSerializer();
            Solider sol = serializer.DeserializeObject<Solider>(AssetsPath + "Test.xml");
            Assert.Equal("Halytskyi", sol.Surname);
            Assert.Equal("Oleh", sol.Name);
            Assert.Equal(Rank.Captain, sol.Rank);
        }
        [Fact]
        public void TestXSDValidation()
        {
            XMLTools.XSDValidator validator = new XMLTools.XSDValidator();
            Assert.True(validator.ValidateXML(AssetsPath + "final2.xsd", AssetsPath + "final2.xml"));
        }
    }
}
