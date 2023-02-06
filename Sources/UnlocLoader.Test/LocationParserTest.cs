using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnlocLoader.Loader;
using UnlocLoader.Model;

namespace UnlocLoader.Test
{
    [TestClass]
    public class LocationParserTest
    {
        [TestMethod]
        [DataRow(",\"AD\",\"ALV\",\"Andorra la Vella\",\"Andorra Vella\",,\"--34-6--\",\"AI\",\"0601\",,\"4230N 00131E\",\"\"", "Andorra la Vella", "Andorra Vella")]
        [DataRow(",\"AD\",\"ALV\",\"Name,With,Comma\",\"Name,With,Comma\",,\"--34-6--\",\"AI\",\"0601\",,\"4230N 00131E\",\"\"", "Name,With,Comma", "Name,With,Comma")]
        [DataRow(",\"AD\",\"ALV\",\"!§$%&//()\",\"+#''*\",,\"--34-6--\",\"AI\",\"0601\",,\"4230N 00131E\",\"\"", "!§$%&//()", "+#''*")]
        public void TestNormalLocation(string input, string name, string spellingName)
        {
            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNotNull(result);
            Assert.IsNull(message);
            Assert.AreEqual(ChangeReason.None, result.ChangeReason);
            Assert.IsTrue(result.ChangeDetails.Length == 0);
            Assert.AreEqual("AD", result.CountryId);
            Assert.AreEqual("ADALV", result.UNLOC);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(spellingName, result.SpellingName);
            Assert.AreEqual(42.5, result.Position.Lat, 0.00001);
            Assert.AreEqual(1.516667, result.Position.Lng, 0.00001);

            Assert.IsTrue(result.Functions.Any(f => f == Function.Airport));
            Assert.IsTrue(result.Functions.Any(f => f == Function.RoadTerminal));
            Assert.IsTrue(result.Functions.Any(f => f == Function.MultimodalFunction));
        }

        [TestMethod]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.None)]
        [DataRow("\"X\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.MarkedForDeletion)]
        [DataRow("\"#\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.ChangeLocationName)]
        [DataRow("\"|\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.ChangeOther)]
        [DataRow("\"+\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.Added)]
        [DataRow("\"=\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.Reference)]
        [DataRow("\"!\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", ChangeReason.Retained)]
        public void TestChangeReason(string input, ChangeReason changeReason)
        {
            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.AreEqual(changeReason, result.ChangeReason);
            Assert.IsTrue(result.ChangeDetails.Length == 0);
        }

        [TestMethod]
        public void TestInvalidChangeReason()
        {
            const string input = "\"P\",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",";

            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.AreEqual(ChangeReason.None, result.ChangeReason);
            Assert.AreEqual("Invalid change reason: P", message);
        }

        [TestMethod]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo\"", ChangeDetails.Coordinates)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Fun\"", ChangeDetails.Function)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Nam\"", ChangeDetails.LocationName)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Spe\"", ChangeDetails.SpellingName)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sta\"", ChangeDetails.Status)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sub\"", ChangeDetails.SubdivisionCode)]
        public void TestChangeDetails(string input, ChangeDetails changeDetails)
        {
            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.AreEqual(changeDetails, result.ChangeDetails[0]);
        }

        [TestMethod]
        public void TestCombinedChangeDetails()
        {
            const string input = ",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo@Fun@Nam\"";

            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.IsTrue(result.ChangeDetails.Length == 3);

            Assert.IsTrue(result.ChangeDetails.Any(d => d == ChangeDetails.Coordinates));
            Assert.IsTrue(result.ChangeDetails.Any(d => d == ChangeDetails.Function));
            Assert.IsTrue(result.ChangeDetails.Any(d => d == ChangeDetails.LocationName));

            Assert.IsTrue(result.ChangeDetails.All(d => d != ChangeDetails.SpellingName));
            Assert.IsTrue(result.ChangeDetails.All(d => d != ChangeDetails.SubdivisionCode));
            Assert.IsTrue(result.ChangeDetails.All(d => d != ChangeDetails.Status));
        }

        [TestMethod]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",", "")]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"Foo\"", "Foo")]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\" Foo \"", "Foo")]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Baz\"", "@Baz")]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo\"", "@Coo")]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo Foo\"", "@Coo Foo")]
        public void TestRemarks(string input, string remarks)
        {
            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.AreEqual(remarks, result.Remarks);
        }

        [TestMethod]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"0\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo\"", Function.Unknown)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"1-------\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo\"", Function.Port)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"-2------\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Fun\"", Function.RailTerminal)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"--3-----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Nam\"", Function.RoadTerminal)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"---4----\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Spe\"", Function.Airport)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"----5---\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sta\"", Function.PostalExchangeOffice)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"-----6--\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sub\"", Function.MultimodalFunction)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"------7-\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sub\"", Function.FixedTransportFunction)]
        [DataRow(",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"-------B\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Sub\"", Function.BorderCrossing)]
        public void TestFunctions(string input, Function function)
        {
            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.AreEqual(function, result.Functions[0]);
        }

        [TestMethod]
        public void TestCombinedFunctions()
        {
            const string input = ",\"AE\",\"HAT\",\"Hatta\",\"Hatta\",\"DU\",\"1-3-5-7-\",\"RL\",\"1707\",,\"2447N 05607E\",\"@Coo@Fun@Nam\"";

            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNull(message);
            Assert.IsTrue(result.ChangeDetails.Length == 3);

            Assert.IsTrue(result.Functions.Any(d => d == Function.Port));
            Assert.IsTrue(result.Functions.Any(d => d == Function.RoadTerminal));
            Assert.IsTrue(result.Functions.Any(d => d == Function.PostalExchangeOffice));
            Assert.IsTrue(result.Functions.Any(d => d == Function.FixedTransportFunction));

            Assert.IsTrue(result.Functions.All(d => d != Function.RailTerminal));
            Assert.IsTrue(result.Functions.All(d => d != Function.Airport));
            Assert.IsTrue(result.Functions.All(d => d != Function.MultimodalFunction));
            Assert.IsTrue(result.Functions.All(d => d != Function.BorderCrossing));
        }
    }
}
