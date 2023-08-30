using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTix.API.Repositories.Interfaces;

namespace QuickTix.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILoggerManager _logger;
        protected readonly IMapper _mapper;

        public BaseController(ILoggerManager logger, IMapper mappeer)
        {
            _logger = logger;
            _mapper = mappeer;
        }
    }
}
