using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTix.Service.Interfaces;

namespace QuickTix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IBusinessRule _businessRule;

    }
}
