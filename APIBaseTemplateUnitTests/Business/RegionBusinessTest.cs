using APIBaseTemplate.Services;
using Moq;

namespace APIBaseTemplateUnitTests.Business
{
    public class RegionBusinessTest : BaseTest
    {
        public static IEnumerable<object[]> GetRegionData()
        {
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();

            var regionId = rnd.Next(wonkaDataset.Regions.Count());
            yield return new object[] { wonkaDataset.Regions.ElementAt(regionId).RegionId, wonkaDataset.Regions.ElementAt(regionId) };

            regionId = rnd.Next(wonkaDataset.Regions.Count());
            yield return new object[] { wonkaDataset.Regions.ElementAt(regionId).RegionId, wonkaDataset.Regions.ElementAt(regionId) };

            regionId = rnd.Next(wonkaDataset.Regions.Count());
            yield return new object[] { wonkaDataset.Regions.ElementAt(regionId).RegionId, wonkaDataset.Regions.ElementAt(regionId) };
        }

        [Fact]
        public void Create_Region()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            var newDto = new APIBaseTemplate.Datamodel.DTO.Region()
            {
                Name = "New region name"
            };

            // Act
            var createdDto = business.Create(newDto);

            // Assert
            Assert.NotNull(createdDto);
            Assert.NotNull(createdDto.RegionId);
            Assert.Equal(createdDto.Name, newDto.Name);

            MockData.RegionRepository.Verify(r => r.Add(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Region>()), Times.Once);
        }

        [Fact]
        public void Delete_Region()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();
            var entityToDelete = wonkaDataset.Regions.ElementAt(rnd.Next(wonkaDataset.Regions.Count())).RegionId;

            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Regions);

            // Act
            business.Delete(entityToDelete);

            // Assert
            MockData.RegionRepository.Verify(r => r.Delete(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Region>()), Times.Once);
        }

        [Fact]
        public void Update_Region()
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            // save a copy of object being modified
            var originalDbItem = Clone(wonkaDataset.Regions.ElementAt(rnd.Next(wonkaDataset.Regions.Count())));

            var modifiedDtoItem = new APIBaseTemplate.Datamodel.DTO.Region()
            {
                RegionId = originalDbItem.RegionId,
                Name = "Updated region name"
            };

            MockData.RegionRepository
                .Setup(r => r.Query())
                .Returns(() => wonkaDataset.Regions.Where(r => r.RegionId == originalDbItem.RegionId));

            // Act
            var updatedDtoItem = business.Update(modifiedDtoItem);

            // Assert
            Assert.NotNull(updatedDtoItem);
            Assert.Equal(updatedDtoItem.RegionId, originalDbItem.RegionId);
            Assert.NotEqual(updatedDtoItem.Name, originalDbItem.Name);

            MockData.RegionRepository.Verify(r => r.Update(It.IsAny<APIBaseTemplate.Datamodel.DbEntities.Region>()), Times.Once);
        }

        [MemberData(nameof(GetRegionData), MemberType = typeof(RegionBusinessTest))]
        [Theory]
        public void Get_Region_by_Id(int regionId, APIBaseTemplate.Datamodel.DbEntities.Region expectedDbItem)
        {
            // Arrange
            var wonkaDataset = new WonkaDataset();
            var rnd = new Random();
            var business = CreateBusiness();

            MockData.RegionRepository
                .Setup(r => r.Get(It.IsAny<APIBaseTemplate.Datamodel.DTO.SearchRegionRequest>()))
                .Returns(() => wonkaDataset.Regions.Where(v => v.RegionId == regionId));

            // Act
            var retrievedDtoItem = business.GetById(wonkaDataset.Regions.ElementAt(rnd.Next(wonkaDataset.Regions.Count())).RegionId);
            var retrievedConvertedDbItem = APIBaseTemplate.Datamodel.Mappers.Mappers.Region.ToDb(retrievedDtoItem);

            // Assert
            Assert.NotNull(retrievedDtoItem);
            Assert.Equal(regionId, retrievedDtoItem.RegionId);
            Assert.Equal(expectedDbItem.Name, retrievedConvertedDbItem.Name);
        }

        private IRegionBusiness CreateBusiness()
        {
            return new RegionBusiness(
                MockData.Logger<RegionBusiness>().Object,
                MockData.UnitOfWorkFactory.Object,
                MockData.RegionRepository.Object);
        }
    }
}
