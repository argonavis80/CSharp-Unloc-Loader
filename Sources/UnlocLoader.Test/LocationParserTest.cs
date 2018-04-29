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
        public void TestNormalLocation()
        {
            const string input = ",\"AD\",\"ALV\",\"Andorra la Vella\",\"Andorra Vella\",,\"--34-6--\",\"AI\",\"0601\",,\"4230N 00131E\",\"\"";

            var target = new LocationParser();

            var result = target.Parse(input, out var message);

            Assert.IsNotNull(result);
            Assert.IsNull(message);
            Assert.AreEqual(ChangeReason.None, result.ChangeReason);
            Assert.IsTrue(result.ChangeDetails.Length == 0);
            Assert.AreEqual("AD", result.CountryId);
            Assert.AreEqual("ADALV", result.UNLOC);
            Assert.AreEqual("Andorra la Vella", result.Name);
            Assert.AreEqual("Andorra Vella", result.SpellingName);
            Assert.AreEqual(42.5, result.Position.Lat, 0.00001);
            Assert.AreEqual(1.516667, result.Position.Lng, 0.00001);
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
    }
}
