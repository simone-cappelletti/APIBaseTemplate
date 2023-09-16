using APIBaseTemplate.Datamodel.DbEntities;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IRepository<Region> _regionRepo;
        private readonly IRepository<Fligth> _fligthRepo;
        private readonly IRepository<Airport> _airportRepo;
        private readonly IRepository<City> _cityRepo;

        public TestController(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<Region> regionRepo,
            IRepository<City> cityRepo,
            IRepository<Fligth> fligthRepo,
            IRepository<Airport> airportRepo)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _regionRepo = regionRepo;
            _cityRepo = cityRepo;
            _fligthRepo = fligthRepo;
            _airportRepo = airportRepo;
        }

        [HttpGet]
        public void AddDemoData()
        {
            using (var uof = _unitOfWorkFactory.Get()
                .BoundTo(_regionRepo, _cityRepo, _fligthRepo, _airportRepo))
            {
                var lazioRegion = new Region()
                {
                    Name = "Lazio",
                };
                var lombardiaRegion = new Region
                {
                    Name = "Lombardia",
                };

                var romeCity = new City()
                {
                    Name = "Rome",
                    Region = lazioRegion
                };
                var milanCity = new City()
                {
                    Name = "Milan",
                    Region = lombardiaRegion
                };

                var ciampinoAirport = new Airport()
                {
                    City = romeCity,
                    Code = "0001",
                    Name = "Ciampino"
                };
                var linateAirport = new Airport()
                {
                    City = milanCity,
                    Code = "0002",
                    Name = "Linate"
                };

                var romeMialnFligth = new Fligth()
                {

                };

                _regionRepo.Add(lazioRegion);
                _cityRepo.Add(romeCity);
                _airportRepo.Add(ciampinoAirport);

                _fligthRepo.Add(new Fligth()
                {

                });

                uof.SaveChanges();
                uof.CompleteTransactionScope();
            }
        }

        [HttpPost]
        public void Test([FromBody] Airline airline)
        {

        }
    }
}
