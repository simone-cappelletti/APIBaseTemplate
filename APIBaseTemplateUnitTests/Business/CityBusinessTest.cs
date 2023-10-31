using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class CityBusinessTest : BaseTest
    {
        public static IEnumerable<object[]> GetCityData()
        {
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();

            var cityId = rnd.Next(wonkaDataset.Cities.Count());
            yield return new object[] { wonkaDataset.Cities.ElementAt(cityId).CityId, wonkaDataset.Cities.ElementAt(cityId) };

            cityId = rnd.Next(wonkaDataset.Cities.Count());
            yield return new object[] { wonkaDataset.Cities.ElementAt(cityId).CityId, wonkaDataset.Cities.ElementAt(cityId) };

            cityId = rnd.Next(wonkaDataset.Cities.Count());
            yield return new object[] { wonkaDataset.Cities.ElementAt(cityId).CityId, wonkaDataset.Cities.ElementAt(cityId) };
        }

        [Fact]
        public void Create_City()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Regions);

            var newDto = new APIBaseTemplate.Datamodel.DTO.City()
            {
                Name = "New city name",
                RegionId = wonkaDataset.Regions.ElementAt(rnd.Next(wonkaDataset.Regions.Count())).RegionId,
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.CityId);
            Assert.Equal(createdDto.Name, newDto.Name);
            Assert.Equal(createdDto.RegionId, newDto.RegionId);

            MockData.CityRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.City>()), Times.Once);
        }

        [Fact]
        public void Delete_City()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();
            var entityToDelete = wonkaDataset.Cities.ElementAt(rnd.Next(wonkaDataset.Cities.Count())).CityId;

            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Cities);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.CityRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.City>()), Times.Once);
        }

        [Fact]
        public void Update_City()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(wonkaDataset.Cities.ElementAt(rnd.Next(wonkaDataset.Cities.Count())));

            var region = wonkaDataset.Regions
                .Where(x => x.RegionId != originalDbItem.RegionId)
                .ElementAt(rnd.Next(wonkaDataset.Regions.Count() - 1));
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.City()
            {
                CityId = originalDbItem.CityId,
                Name = "Updated city name",
                RegionId = region.RegionId
            };

            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Cities.Where(r => r.CityId == originalDbItem.CityId));
            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Regions.Where(r => r.RegionId == region.RegionId));

            // Act
            var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.CityId, originalDbItem.CityId);
            Assert.NotEqual(updatedDtoItem.Name, originalDbItem.Name);
            Assert.NotEqual(updatedDtoItem.RegionId, originalDbItem.RegionId);

            MockData.CityRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.City>()), Times.Once);
        }

        [MemberData(nameof(GetCityData), MemberType = typeof(CityBusinessTest))]
        [Theory]
        public void Get_City_by_Id(int cityId, APIBaseTemplate.Datamodel.DbEntities.City expectedDbItem)
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            MockData.CityRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchCityRequest>()))
                .Returns(() => wonkaDataset.Cities.Where(v => v.CityId == cityId));

            // Act
            var retrievedDtoItem = business.GetById(wonkaDataset.Cities.ElementAt(rnd.Next(wonkaDataset.Cities.Count())).CityId);
            var retrievedConvertedDbItem = APIBaseTemplate.Datamodel.Mappers.Mappers.City.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(cityId, retrievedDtoItem.CityId);
            Assert.Equal(expectedDbItem.Name, retrievedConvertedDbItem.Name);
            Assert.Equal(expectedDbItem.RegionId, retrievedConvertedDbItem.RegionId);
        }

        private ICityBusiness CreateBusiness()
        {
            return new CityBusiness(
                MockData.Logger<CityBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.CityRepository.Object,
                MockData.RegionRepository.Object);
        }
    }
}
