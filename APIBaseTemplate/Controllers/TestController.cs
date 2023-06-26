using APIBaseTemplate.Datamodel.Db;
using APIBaseTemplate.Repositories.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IRepository<Region> _regionRepo;

        public TestController(IUnitOfWorkFactory unitOfWorkFactory, IRepository<Region> regionRepo)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _regionRepo = regionRepo;
        }

        [HttpGet]
        public void Test()
        {
            var uof = _unitOfWorkFactory.Get();

            uof.BoundTo(_regionRepo);

            var result = _regionRepo.Query();

            uof.CompleteTransactionScope();
        }
    }
}
