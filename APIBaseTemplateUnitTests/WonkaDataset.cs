using APIBaseTemplate.Datamodel.DbEntities;

namespace APIBaseTemplateUnitTests
{
    public class WonkaDataset
    {
        private readonly int _cycles = 10;

        private readonly List<Currency> _currencies = new();
        private readonly List<City> _cities = new();
        private readonly List<Region> _regions = new();
        private readonly List<Airline> _airlines = new();
        private readonly List<Airport> _airports = new();
        private readonly List<Fligth> _fligths = new();
        private readonly List<FligthService> _fligthServices = new();

        public IQueryable<Currency> Currencies { get => _currencies.AsQueryable(); }
        public IQueryable<City> Cities { get => _cities.AsQueryable(); }
        public IQueryable<Region> Regions { get => _regions.AsQueryable(); }
        public IQueryable<Airline> Airlines { get => _airlines.AsQueryable(); }
        public IQueryable<Airport> Airports { get => _airports.AsQueryable(); }
        public IQueryable<Fligth> Fligths { get => _fligths.AsQueryable(); }
        public IQueryable<FligthService> FligthServices { get => _fligthServices.AsQueryable(); }

        public WonkaDataset()
        {
            InitCurrencies();
            InitRegions();
            InitCities();
            InitAirlines();
            InitAirport();
            InitFligths();
            InitFligthServices();
        }

        private void InitCurrencies()
        {
            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                    _currencies.Add(new Currency()
                    {
                        CurrencyId = ++id,
                        Code = $"CurrencyCode_{id}",
                        Name = $"CurrencyName_{id}"
                    }
                    ));
        }

        private void InitRegions()
        {
            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                    _regions.Add(new Region()
                    {
                        RegionId = ++id,
                        Name = $"RegionName_{id}"
                    }
                    ));
        }

        private void InitCities()
        {
            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                _cities.Add(new City()
                {
                    CityId = ++id,
                    Name = $"CityName_{id}",
                    RegionId = id - 1,
                    Region = _regions[id - 1]
                }
                ));
        }

        private void InitAirlines()
        {
            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                _airlines.Add(new Airline()
                {
                    AirlineId = ++id,
                    Code = $"AirlineCode_{id}",
                    Name = $"AirlineName_{id}",
                    RegionId = id - 1,
                    Region = _regions[id - 1]
                }
                ));
        }

        private void InitAirport()
        {
            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                _airports.Add(new Airport()
                {
                    AirportId = ++id,
                    Code = $"AirportCode_{id}",
                    Name = $"AirportName_{id}",
                    CityId = id - 1,
                    City = _cities[id - 1]
                }
                ));
        }

        private void InitFligths()
        {
            var rnd = new Random();

            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                _fligths.Add(new Fligth()
                {
                    FligthId = ++id,
                    Code = $"FligthCode_{id}",
                    AirlineId = id - 1,
                    Airline = _airlines[id - 1],
                    ArrivalAirportId = id - 1,
                    ArrivalAirport = _airports[id - 1],
                    ArrivalTime = DateTime.Now.AddHours(rnd.Next(6, 10)),
                    DepartureAirportId = id - 1,
                    DepartureAirport = _airports[id - 1],
                    DepartureTime = DateTime.Now.AddHours(rnd.Next(1, 5)),
                    Gate = $"FligthGate_{id}",
                    Terminal = $"FligthTerminal_{id}",
                    FligthServices = _fligthServices.Take(rnd.Next(_fligthServices.Count)),
                    IsDeleted = false
                }
                ));
        }

        private void InitFligthServices()
        {
            var rnd = new Random();
            var flightServiceTypeEnumLength = Enum.GetValues(typeof(FlightServiceType)).Length;

            Enumerable
                .Range(0, _cycles)
                .ToList()
                .ForEach((id) =>
                _fligthServices.Add(new FligthService()
                {
                    FligthServiceId = ++id,
                    Amount = rnd.Next(100),
                    CurrencyId = id - 1,
                    Currency = _currencies[id - 1],
                    FlightServiceType = (FlightServiceType)rnd.Next(flightServiceTypeEnumLength),
                    FligthId = id - 1,
                    Fligth = _fligths[id - 1]
                }
                ));
        }
    }
}
