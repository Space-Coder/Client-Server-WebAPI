using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexodeAPI.Services;

namespace TexodeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobileController : ControllerBase
    {
        private readonly ILogger<MobileController> _logger;
        private readonly IMobileService _mobileService;
        public MobileController(ILogger<MobileController> logger, IMobileService mobileService)
        {
            _logger = logger;
            _mobileService = mobileService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _mobileService.GetAllData();
            if (result != null)
            {
                return new JsonResult(result);
            }
            ModelState.AddModelError(string.Empty, "Данные отсутсвуют");
            return new JsonResult(ModelState)
            {
                StatusCode = 500
            };
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mobileService.GetByName(name);
            if (result != null)
            {
                return new JsonResult(result);
            }
            ModelState.AddModelError(string.Empty, "Карточки с таким именем не существует");
            return new JsonResult(ModelState)
            {
                StatusCode = 404,
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post(MobileCard model)
        {
            if (model != null)
            {
                try
                {
                    await _mobileService.AddCard(model);
                    return Ok();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return new JsonResult(ModelState)
                    {
                        StatusCode = 500
                    };
                }
            }
            ModelState.AddModelError(string.Empty, "MobileCard не может быть равен null");
            return new JsonResult(ModelState)
            {
                StatusCode = 400
            };

        }

        [HttpPut]
        public async Task<IActionResult> Put(MobileCard model)
        {
            if (model != null)
            {
                try
                {
                    await _mobileService.UpdateCard(model);
                    return Ok();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return new JsonResult(ModelState)
                    {
                        StatusCode = 500
                    };
                }
            }
            ModelState.AddModelError(string.Empty, "MobileCard не может быть равен null");
            return new JsonResult(ModelState)
            {
                StatusCode = 400
            };
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArray(List<MobileCard> model)
        {
            try
            {
                await _mobileService.RemoveCards(model);
                return Ok();
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                return new JsonResult(ModelState)
                {
                    StatusCode = 500
                };
            }
        }
       
    }
}
