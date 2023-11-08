using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class FligthServiceBusinessTest : BaseTest
    {
        private static WonkaDataset _wonkaDataset = new WonkaDataset();
        private static Random _rnd = new Random();

        public static IEnumerable<object[]> GetFligthServiceData()
        {
            var fligthServiceId = _rnd.Next(_wonkaDataset.FligthServices.Count());
            yield return new object[] { _wonkaDataset.FligthServices.ElementAt(fligthServiceId).FligthServiceId, _wonkaDataset.FligthServices.ElementAt(fligthServiceId) };

            fligthServiceId = _rnd.Next(_wonkaDataset.FligthServices.Count());
            yield return new object[] { _wonkaDataset.FligthServices.ElementAt(fligthServiceId).FligthServiceId, _wonkaDataset.FligthServices.ElementAt(fligthServiceId) };

            fligthServiceId = _rnd.Next(_wonkaDataset.FligthServices.Count());
            yield return new object[] { _wonkaDataset.FligthServices.ElementAt(fligthServiceId).FligthServiceId, _wonkaDataset.FligthServices.ElementAt(fligthServiceId) };
        }

        [Fact]
        public void Create_FligthService()
        {
            // Arrange
            var business = CreateBusiness();

            MockData.FligthServiceRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.FligthServices);

            MockData.CurrencyRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Currencies);

            MockData.FligthRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Fligths);

            var newDto = new APIBaseTemplate.Datamodel.DTO.FligthService()
            {
                FlightServiceType = (APIBaseTemplate.Datamodel.DTO.FlightServiceType)_rnd.Next(Enum.GetValues(typeof(APIBaseTemplate.Datamodel.DTO.FlightServiceType)).Length),
                Amount = 9999,
                CurrencyId = _wonkaDataset.Currencies.ElementAt(_rnd.Next(_wonkaDataset.Currencies.Count())).CurrencyId,
                FligthId = _wonkaDataset.Fligths.ElementAt(_rnd.Next(_wonkaDataset.Fligths.Count())).FligthId
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.FligthServiceId);
            Assert.Equal(createdDto.FlightServiceType, newDto.FlightServiceType);
            Assert.Equal(createdDto.Amount, newDto.Amount);
            Assert.Equal(createdDto.CurrencyId, newDto.CurrencyId);
            Assert.Equal(createdDto.FligthId, newDto.FligthId);

            MockData.FligthServiceRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.FligthService>()), Times.Once);
        }

        [Fact]
        public void Delete_FligthService()
        {
            // Arrange
            var business = CreateBusiness();
            var entityToDelete = _wonkaDataset.FligthServices.ElementAt(_rnd.Next(_wonkaDataset.FligthServices.Count())).FligthServiceId;

            MockData.FligthServiceRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.FligthServices);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.FligthServiceRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.FligthService>()), Times.Once);
        }

        [Fact]
        public void Update_FligthService()
        {
            // Arrange
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(_wonkaDataset.FligthServices.ElementAt(_rnd.Next(_wonkaDataset.FligthServices.Count())));

            var currency = _wonkaDataset.Currencies
                .Where(x => x.CurrencyId != originalDbItem.CurrencyId)
                .ElementAt(_rnd.Next(_wonkaDataset.Currencies.Count() - 1));
            var fligth = _wonkaDataset.Fligths
                .Where(x => x.FligthId != originalDbItem.FligthId)
                .ElementAt(_rnd.Next(_wonkaDataset.Fligths.Count() - 1));
            var fligthServiceType = Mappers.FligthService.ToDto(originalDbItem.FlightServiceType) == APIBaseTemplate.Datamodel.DTO.FlightServiceType.Fligth
                ? APIBaseTemplate.Datamodel.DTO.FlightServiceType.HoldLuggage
                : APIBaseTemplate.Datamodel.DTO.FlightServiceType.Fligth;
            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.FligthService()
            {
                FligthServiceId = originalDbItem.FligthServiceId,
                FlightServiceType = fligthServiceType,
                Amount = 9999,
                CurrencyId = currency.CurrencyId,
                FligthId = fligth.FligthId
            };

            MockData.FligthServiceRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.FligthServices.Where(r => r.FligthServiceId == originalDbItem.FligthServiceId));
            MockData.CurrencyRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Currencies.Where(r => r.CurrencyId == currency.CurrencyId));
            MockData.FligthRepository
                .Setup(r => r.Query())
                .Returns(() => _wonkaDataset.Fligths.Where(r => r.FligthId == fligth.FligthId));

            // Act
            var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.FligthServiceId, originalDbItem.FligthServiceId);
            Assert.NotEqual(updatedDtoItem.Amount, originalDbItem.Amount);
            Assert.NotEqual(updatedDtoItem.CurrencyId, originalDbItem.CurrencyId);
            Assert.NotEqual(updatedDtoItem.FligthId, originalDbItem.FligthId);

            MockData.FligthServiceRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.FligthService>()), Times.Once);
        }

        [MemberData(nameof(GetFligthServiceData), MemberType = typeof(FligthServiceBusinessTest))]
        [Theory]
        public void Get_FligthService_by_Id(int fligthServiceId, APIBaseTemplate.Datamodel.DbEntities.FligthService expectedDbItem)
        {
            // Arrange
            var business = CreateBusiness();

            MockData.FligthServiceRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchFligthServiceRequest>()))
                .Returns(() => _wonkaDataset.FligthServices.Where(v => v.FligthServiceId == fligthServiceId));

            // Act
            var retrievedDtoItem = business.GetById(fligthServiceId);
            var retrievedConvertedDbItem = APIBaseTemplate.Datamodel.Mappers.Mappers.FligthService.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(fligthServiceId, retrievedDtoItem.FligthServiceId);
            Assert.Equal(expectedDbItem.FlightServiceType, retrievedConvertedDbItem.FlightServiceType);
            Assert.Equal(expectedDbItem.Amount, retrievedConvertedDbItem.Amount);
            Assert.Equal(expectedDbItem.CurrencyId, retrievedConvertedDbItem.CurrencyId);
            Assert.Equal(expectedDbItem.FligthId, retrievedConvertedDbItem.FligthId);
        }

        private IFligthServiceBusiness CreateBusiness()
        {
            return new FligthServiceBusiness(
                MockData.Logger<FligthServiceBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.FligthServiceRepository.Object,
                MockData.FligthRepository.Object,
                MockData.CurrencyRepository.Object);
        }
    }
}
