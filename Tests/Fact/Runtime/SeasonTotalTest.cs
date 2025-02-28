using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class SeasonTotalTest
    {
        // private readonly YearMonthDay BASEDAY_2022_23 = new(2023, 1, 1);
        private readonly YearMonthDay BASEDAY_2023_24 = new(2024, 1, 1);
        private readonly YearMonthDay BASEDAY_2024_25 = new(2025, 1, 1);

        [Test]
        public async Task ClassTest()
        {
            // var classObject2022_23 = await ClassObjectQuery.All(BASEDAY_2022_23);
            var classObject2023_24 = await ClassObjectQuery.All(BASEDAY_2023_24);
            var classObject2024_25 = await ClassObjectQuery.All(BASEDAY_2024_25);

            // シニアの年齢制限が2022-23シーズンでは15歳だったものが2シーズンかけて17歳へ引きあがる
            // https://www.olympics.com/ja/news/figure-skating-isu-increase-age-limit-form-15-to-17
            // Assert.IsTrue(classObject2022_23.Where(x => x.data.id == "Senior").All(x => x.data.minAge == 15));
            Assert.IsTrue(classObject2023_24.Where(x => x.data.id == "Senior").All(x => x.data.minAge == 16));
            Assert.IsTrue(classObject2024_25.Where(x => x.data.id == "Senior").All(x => x.data.minAge == 17));
        }
    }
}
