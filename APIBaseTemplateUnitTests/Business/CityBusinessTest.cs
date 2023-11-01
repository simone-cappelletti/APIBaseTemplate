using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class CityBusinessTest : BaseTest
    {
        private static WonkaDataset _wonkaDataset = new WonkaDataset();
        private static Random _rnd = new Random();

        public static IEnumerable<object[]> GetCityData()
        {
            var cityId = _rnd.Next(_wonkaDataset.Cities.Count());
            yield return new object[] { _wonkaDataset.Cities.ElementAt(cityId).CityId, _wonkaDataset.Cities.ElementAt(cityId) };

            cityId = _rnd.Next(_wonkaDataset.Cities.Count());
            yield return new object[] { _wonkaDataset.Cities.ElementAt(cityId).CityId, _wonkaDataset.Cities.ElementAt(cityId) };

            cityId = _rnd.Next(_wonkaDataset.Cities.Count());
            yield return new object[] { _wonkaDataset.Cities.ElementAt(cityId).CityId, _wonkaDataset.Cities.ElementAt(cityId) };
        }

        [Fact]
        public void Create_City()
        {
            // Arrange
            var business = CreateBusiness();

            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Regions);

            var newDto = new APIBaseTemplate.Datamodel.DTO.City()
            {
                Name = "New city name",
                RegionId = _wonkaDataset.Regions.ElementAt(_rnd.Next(_wonkaDataset.Regions.Count())).RegionId,
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
            var business = CreateBusiness();
            var entityToDelete = _wonkaDataset.Cities.ElementAt(_rnd.Next(_wonkaDataset.Cities.Count())).CityId;

            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Cities);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.CityRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.City>()), Times.Once);
        }

        [Fact]
        public void Update_City()
        {
            // Arrange
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(_wonkaDataset.Cities.ElementAt(_rnd.Next(_wonkaDataset.Cities.Count())));

            var region = _wonkaDataset.Regions
                .Where(x => x.RegionId != originalDbItem.RegionId)
                .ElementAt(_rnd.Next(_wonkaDataset.Regions.Count() - 1));
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.City()
            {
                CityId = originalDbItem.CityId,
                Name = "Updated city name",
                RegionId = region.RegionId
            };

            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Cities.Where(r => r.CityId == originalDbItem.CityId));
            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Regions.Where(r => r.RegionId == region.RegionId));

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
            var business = CreateBusiness();

            MockData.CityRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchCityRequest>()))
                .Returns(() => _wonkaDataset.Cities.Where(v => v.CityId == cityId));

            // Act
            var retrievedDtoItem = business.GetById(cityId);
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
