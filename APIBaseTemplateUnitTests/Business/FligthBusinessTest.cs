using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class FligthBusinessTest : BaseTest
    {
        private static WonkaDataset _wonkaDataset = new WonkaDataset();
        private static Random _rnd = new Random();

        public static IEnumerable<object[]> GetFligthData()
        {
            var fligthId = _rnd.Next(_wonkaDataset.Fligths.Count());
            yield return new object[] { _wonkaDataset.Fligths.ElementAt(fligthId).FligthId, _wonkaDataset.Fligths.ElementAt(fligthId) };

            fligthId = _rnd.Next(_wonkaDataset.Fligths.Count());
            yield return new object[] { _wonkaDataset.Fligths.ElementAt(fligthId).FligthId, _wonkaDataset.Fligths.ElementAt(fligthId) };

            fligthId = _rnd.Next(_wonkaDataset.Fligths.Count());
            yield return new object[] { _wonkaDataset.Fligths.ElementAt(fligthId).FligthId, _wonkaDataset.Fligths.ElementAt(fligthId) };
        }

        [Fact]
        public void Create_Fligth()
        {
            // Arrange
            var business = CreateBusiness();
            var departureTime = DateTime.UtcNow;
            var arrivalTime = departureTime.AddHours(2);

            MockData.FligthRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Fligths);

            MockData.AirlineRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airlines);

            MockData.AirportRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airports);

            MockData.FligthServiceRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.FligthServices);

            var newDto = new APIBaseTemplate.Datamodel.DTO.Fligth()
            {
                Code = "New fligth code",
                AirlineId = _wonkaDataset.Airlines.ElementAt(_rnd.Next(_wonkaDataset.Airlines.Count())).AirlineId,
                DepartureAirportId = _wonkaDataset.Airports.ElementAt(_rnd.Next(_wonkaDataset.Airports.Count())).AirportId,
                ArrivalAirportId = _wonkaDataset.Airports.ElementAt(_rnd.Next(_wonkaDataset.Airports.Count())).AirportId,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                Terminal = "New fligth terminal",
                Gate = "New fligth gate",
                FligthServices = Mappers.FligthService.ToDto(_wonkaDataset.FligthServices.Take(3))
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.AirlineId);
            Assert.Equal(createdDto.Code, newDto.Code);
            Assert.Equal(createdDto.AirlineId, newDto.AirlineId);
            Assert.Equal(createdDto.DepartureAirportId, newDto.DepartureAirportId);
            Assert.Equal(createdDto.ArrivalAirportId, newDto.ArrivalAirportId);
            Assert.Equal(createdDto.DepartureTime, newDto.DepartureTime);
            Assert.Equal(createdDto.ArrivalTime, newDto.ArrivalTime);
            Assert.Equal(createdDto.Terminal, newDto.Terminal);
            Assert.Equal(createdDto.Gate, newDto.Gate);
            Assert.Equivalent(createdDto.FligthServices, newDto.FligthServices, true);

            MockData.FligthRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Fligth>()), Times.Once);
        }

        [Fact]
        public void Delete_Fligth()
        {
            // Arrange
            var business = CreateBusiness();
            var entityToDelete = _wonkaDataset.Fligths.ElementAt(_rnd.Next(_wonkaDataset.Fligths.Count())).FligthId;

            MockData.FligthRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Fligths);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.FligthRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Fligth>()), Times.Once);
        }

        [Fact]
        public void Update_Fligth()
        {
            // Arrange
            var business = CreateBusiness();
            var departureTime = DateTime.UtcNow;
            var arrivalTime = departureTime.AddHours(2);

            // save a copy of object being modified
            var originalDbItem = Clone(_wonkaDataset.Fligths.ElementAt(_rnd.Next(_wonkaDataset.Fligths.Count())));

            var airline = _wonkaDataset.Airlines
                .Where(x => x.AirlineId != originalDbItem.AirlineId)
                .ElementAt(_rnd.Next(_wonkaDataset.Airlines.Count() - 1));
            var departureAirport = _wonkaDataset.Airports
                .Where(x => x.AirportId != originalDbItem.DepartureAirportId)
                .ElementAt(_rnd.Next(_wonkaDataset.Airports.Count() - 1));
            var arrivalAirport = _wonkaDataset.Airports
                .Where(x => x.AirportId != originalDbItem.ArrivalAirportId)
                .ElementAt(_rnd.Next(_wonkaDataset.Airports.Count() - 1));
            var fligthServices = Mappers.FligthService.ToDto(_wonkaDataset.FligthServices.Where(
                fs =>
                    !originalDbItem.FligthServices.Select(f => f.FligthServiceId).Contains(fs.FligthId)
                    ).Take(3));
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.Fligth()
            {
                FligthId = originalDbItem.FligthId,
                Code = "Updated fligth code",
                AirlineId = airline.AirlineId,
                DepartureAirportId = departureAirport.AirportId,
                ArrivalAirportId = arrivalAirport.AirportId,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                Terminal = "Updated fligth terminal",
                Gate = "Updated fligth gate",
                FligthServices = fligthServices
            };

            MockData.FligthRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Fligths.Where(r => r.FligthId == originalDbItem.FligthId));
            MockData.AirlineRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airlines.Where(r => r.AirlineId == airline.AirlineId));
            MockData.AirportRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Airports.Where(
                    r =>
                        r.AirportId == departureAirport.AirportId ||
                        r.AirportId == arrivalAirport.AirportId));
            MockData.FligthServiceRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.FligthServices.Where(
                    r =>
                        fligthServices.Select(f => f.FligthServiceId).Contains(r.FligthServiceId)));

            // Act
            var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.FligthId, originalDbItem.FligthId);
            Assert.NotEqual(updatedDtoItem.Code, originalDbItem.Code);
            Assert.NotEqual(updatedDtoItem.AirlineId, originalDbItem.AirlineId);
            Assert.NotEqual(updatedDtoItem.DepartureAirportId, originalDbItem.DepartureAirportId);
            Assert.NotEqual(updatedDtoItem.ArrivalAirportId, originalDbItem.ArrivalAirportId);
            Assert.NotEqual(updatedDtoItem.DepartureTime, originalDbItem.DepartureTime);
            Assert.NotEqual(updatedDtoItem.ArrivalTime, originalDbItem.ArrivalTime);
            Assert.NotEqual(updatedDtoItem.Terminal, originalDbItem.Terminal);
            Assert.NotEqual(updatedDtoItem.Gate, originalDbItem.Gate);
            Assert.NotEqual(updatedDtoItem.FligthServices, Mappers.FligthService.ToDto(originalDbItem.FligthServices));

            MockData.FligthRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Fligth>()), Times.Once);
        }

        [MemberData(nameof(GetFligthData), MemberType = typeof(FligthBusinessTest))]
        [Theory]
        public void Get_Fligth_by_Id(int fligthId, APIBaseTemplate.Datamodel.DbEntities.Fligth expectedDbItem)
        {
            // Arrange
            var business = CreateBusiness();

            MockData.FligthRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchFligthRequest>()))
                .Returns(() => _wonkaDataset.Fligths.Where(v => v.FligthId == fligthId));

            // Act
            var retrievedDtoItem = business.GetById(fligthId);
            var retrievedConvertedDbItem = Mappers.Fligth.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(fligthId, retrievedDtoItem.FligthId);
            Assert.Equal(expectedDbItem.Code, retrievedConvertedDbItem.Code);
            Assert.Equal(expectedDbItem.AirlineId, retrievedConvertedDbItem.AirlineId);
            Assert.Equal(expectedDbItem.DepartureAirportId, retrievedConvertedDbItem.DepartureAirportId);
            Assert.Equal(expectedDbItem.ArrivalAirportId, retrievedConvertedDbItem.ArrivalAirportId);
            Assert.Equal(expectedDbItem.DepartureTime, retrievedConvertedDbItem.DepartureTime);
            Assert.Equal(expectedDbItem.ArrivalTime, retrievedConvertedDbItem.ArrivalTime);
            Assert.Equal(expectedDbItem.Terminal, retrievedConvertedDbItem.Terminal);
            Assert.Equal(expectedDbItem.Gate, retrievedConvertedDbItem.Gate);
            Assert.Equal(expectedDbItem.FligthServices, retrievedConvertedDbItem.FligthServices);
        }

        private IFligthBusiness CreateBusiness()
        {
            return new FligthBusiness(
                MockData.Logger<FligthBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.FligthRepository.Object,
                MockData.AirlineRepository.Object,
                MockData.AirportRepository.Object,
                MockData.FligthServiceRepository.Object);
        }
    }
}
