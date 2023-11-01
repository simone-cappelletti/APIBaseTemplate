using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class AirlineBusinessTest : BaseTest
    {
        private static WonkaDataset _wonkaDataset = new WonkaDataset();
        private static Random _rnd = new Random();

        public static IEnumerable<object[]> GetAirlineData()
        {
            var airlineId = _rnd.Next(_wonkaDataset.Airlines.Count());
            yield return new object[] { _wonkaDataset.Airlines.ElementAt(airlineId).AirlineId, _wonkaDataset.Airlines.ElementAt(airlineId) };

            airlineId = _rnd.Next(_wonkaDataset.Airlines.Count());
            yield return new object[] { _wonkaDataset.Airlines.ElementAt(airlineId).AirlineId, _wonkaDataset.Airlines.ElementAt(airlineId) };

            airlineId = _rnd.Next(_wonkaDataset.Airlines.Count());
            yield return new object[] { _wonkaDataset.Airlines.ElementAt(airlineId).AirlineId, _wonkaDataset.Airlines.ElementAt(airlineId) };
        }

        [Fact]
        public void Create_Airline()
        {
            // Arrange
            var business = CreateBusiness();

            MockData.AirlineRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airlines);

            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Regions);

            var newDto = new APIBaseTemplate.Datamodel.DTO.Airline()
            {
                Code = "New airline code",
                Name = "New airline name",
                RegionId = _wonkaDataset.Regions.ElementAt(_rnd.Next(_wonkaDataset.Regions.Count())).RegionId
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.AirlineId);
            Assert.Equal(createdDto.Code, newDto.Code);
            Assert.Equal(createdDto.Name, newDto.Name);
            Assert.Equal(createdDto.RegionId, newDto.RegionId);

            MockData.AirlineRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airline>()), Times.Once);
        }

        [Fact]
        public void Delete_Airline()
        {
            // Arrange
            var business = CreateBusiness();
            var entityToDelete = _wonkaDataset.Airlines.ElementAt(_rnd.Next(_wonkaDataset.Airlines.Count())).AirlineId;

            MockData.AirlineRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airlines);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.AirlineRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airline>()), Times.Once);
        }

        [Fact]
        public void Update_Airline()
        {
            // Arrange
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(_wonkaDataset.Airlines.ElementAt(_rnd.Next(_wonkaDataset.Airlines.Count())));

            var region = _wonkaDataset.Regions
                .Where(x => x.RegionId != originalDbItem.RegionId)
                .ElementAt(_rnd.Next(_wonkaDataset.Regions.Count() - 1));
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.Airline()
            {
                AirlineId = originalDbItem.AirlineId,
                Code = "Updated airline code",
                Name = "Updated airline name",
                RegionId = region.RegionId
            };

            MockData.AirlineRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airlines.Where(r => r.AirlineId == originalDbItem.AirlineId));
            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Regions.Where(r => r.RegionId == region.RegionId));

            // Act
            var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.AirlineId, originalDbItem.AirlineId);
            Assert.NotEqual(updatedDtoItem.Code, originalDbItem.Code);
            Assert.NotEqual(updatedDtoItem.Name, originalDbItem.Name);
            Assert.NotEqual(updatedDtoItem.RegionId, originalDbItem.RegionId);

            MockData.AirlineRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Airline>()), Times.Once);
        }

        [MemberData(nameof(GetAirlineData), MemberType = typeof(AirlineBusinessTest))]
        [Theory]
        public void Get_Airline_by_Id(int airlineId, APIBaseTemplate.Datamodel.DbEntities.Airline expectedDbItem)
        {
            // Arrange
            var business = CreateBusiness();

            MockData.AirlineRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchAirlineRequest>()))
                .Returns(() => _wonkaDataset.Airlines.Where(v => v.AirlineId == airlineId));

            // Act
            var retrievedDtoItem = business.GetById(airlineId);
            var retrievedConvertedDbItem = APIBaseTemplate.Datamodel.Mappers.Mappers.Airline.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(airlineId, retrievedDtoItem.AirlineId);
            Assert.Equal(expectedDbItem.Code, retrievedConvertedDbItem.Code);
            Assert.Equal(expectedDbItem.Name, retrievedConvertedDbItem.Name);
            Assert.Equal(expectedDbItem.RegionId, retrievedConvertedDbItem.RegionId);
        }

        private IAirlineBusiness CreateBusiness()
        {
            return new AirlineBusiness(
                MockData.Logger<AirlineBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.AirlineRepository.Object,
                MockData.RegionRepository.Object);
        }
    }
}
