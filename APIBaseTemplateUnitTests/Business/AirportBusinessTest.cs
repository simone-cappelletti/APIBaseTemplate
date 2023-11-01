using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class AirportBusinessTest : BaseTest
    {
        private static WonkaDataset _wonkaDataset = new WonkaDataset();
        private static Random _rnd = new Random();

        public static IEnumerable<object[]> GetAirportData()
        {
            var airportId = _rnd.Next(_wonkaDataset.Airports.Count());
            yield return new object[] { _wonkaDataset.Airports.ElementAt(airportId).AirportId, _wonkaDataset.Airports.ElementAt(airportId) };

            airportId = _rnd.Next(_wonkaDataset.Airports.Count());
            yield return new object[] { _wonkaDataset.Airports.ElementAt(airportId).AirportId, _wonkaDataset.Airports.ElementAt(airportId) };

            airportId = _rnd.Next(_wonkaDataset.Airports.Count());
            yield return new object[] { _wonkaDataset.Airports.ElementAt(airportId).AirportId, _wonkaDataset.Airports.ElementAt(airportId) };
        }

        [Fact]
        public void Create_Airport()
        {
            // Arrange
            var business = CreateBusiness();

            MockData.AirportRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airports);

            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Cities);

            var newDto = new APIBaseTemplate.Datamodel.DTO.Airport()
            {
                Code = "New airport code",
                Name = "New airport name",
                CityId = _wonkaDataset.Cities.ElementAt(_rnd.Next(_wonkaDataset.Cities.Count())).CityId
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.AirportId);
            Assert.Equal(createdDto.Code, newDto.Code);
            Assert.Equal(createdDto.Name, newDto.Name);
            Assert.Equal(createdDto.CityId, newDto.CityId);

            MockData.AirportRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airport>()), Times.Once);
        }

        [Fact]
        public void Delete_Airport()
        {
            // Arrange
            var business = CreateBusiness();
            var entityToDelete = _wonkaDataset.Airports.ElementAt(_rnd.Next(_wonkaDataset.Airports.Count())).AirportId;

            MockData.AirportRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airports);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.AirportRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airport>()), Times.Once);
        }

        [Fact]
        public void Update_Airport()
        {
            // Arrange
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(_wonkaDataset.Airports.ElementAt(_rnd.Next(_wonkaDataset.Airports.Count())));

            var city = _wonkaDataset.Cities
                .Where(x => x.CityId != originalDbItem.CityId)
                .ElementAt(_rnd.Next(_wonkaDataset.Cities.Count() - 1));
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.Airport()
            {
                AirportId = originalDbItem.AirportId,
                Code = "Updated city code",
                Name = "Updated city name",
                CityId = city.CityId
            };

            MockData.AirportRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airports.Where(r => r.AirportId == originalDbItem.AirportId));
            MockData.CityRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Cities.Where(r => r.CityId == city.CityId));

            // Act
             var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.AirportId, originalDbItem.AirportId);
            Assert.NotEqual(updatedDtoItem.Code, originalDbItem.Code);
            Assert.NotEqual(updatedDtoItem.Name, originalDbItem.Name);
            Assert.NotEqual(updatedDtoItem.CityId, originalDbItem.CityId);

            MockData.AirportRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airport>()), Times.Once);
        }

        [MemberData(nameof(GetAirportData), MemberType = typeof(AirportBusinessTest))]
        [Theory]
        public void Get_Airport_by_Id(int airportId, APIBaseTemplate.Datamodel.DbEntities.Airport expectedDbItem)
        {
            // Arrange
            var business = CreateBusiness();

            MockData.AirportRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchAirportRequest>()))
                .Returns(() => _wonkaDataset.Airports.Where(v => v.AirportId == airportId));

            // Act
            var retrievedDtoItem = business.GetById(airportId);
            var retrievedConvertedDbItem = APIBaseTemplate.Datamodel.Mappers.Mappers.Airport.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(airportId, retrievedDtoItem.AirportId);
            Assert.Equal(expectedDbItem.Code, retrievedConvertedDbItem.Code);
            Assert.Equal(expectedDbItem.Name, retrievedConvertedDbItem.Name);
            Assert.Equal(expectedDbItem.CityId, retrievedConvertedDbItem.CityId);
        }

        private IAirportBusiness CreateBusiness()
        {
            return new AirportBusiness(
                MockData.Logger<AirportBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.AirportRepository.Object,
                MockData.CityRepository.Object);
        }
    }
}
