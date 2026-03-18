
using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;

[ApiController]
[Route("api/[controller]")]
public class Object2DController : ControllerBase
{
    private readonly IObject2DRepository _objectRepo;
    private readonly IEnvironment2DRepository _environmentRepo;

    public Object2DController(
        IObject2DRepository objectRepo,
        IEnvironment2DRepository environmentRepo)
    {
        _objectRepo = objectRepo;
        _environmentRepo = environmentRepo;
    }

    // GET api/object2d?environmentId=1&userId=1
    [HttpGet]
    public async Task<IActionResult> GetByEnvironment([FromQuery] int environmentId, [FromQuery] int userId)
    {
        var env = await _environmentRepo.GetByIdAsync(environmentId);
        if (env == null)
            return NotFound("Environment not found.");

        if (env.UserID != userId)
            return Forbid("You can only view your own worlds.");

        var objects = await _objectRepo.GetByEnvironmentAsync(environmentId);
        return Ok(objects);
    }

    // POST api/object2d
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int userId, [FromBody] Object2D obj)
    {
        var env = await _environmentRepo.GetByIdAsync(obj.EnvironmentID);
        if (env == null)
            return NotFound("Environment not found.");

        if (env.UserID != userId)
            return Forbid("You can only modify your own worlds.");

        var created = await _objectRepo.CreateAsync(obj);
        return Ok(created);
    }
}

//using Microsoft.AspNetCore.Mvc;
//using MySecureBackend.WebApi.Models;
//using MySecureBackend.WebApi.Repositories;

//namespace MySecureBackend.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class Object2DController : ControllerBase
//    {
//        private readonly IObject2DRepository _objectRepo;

//        public Object2DController(IObject2DRepository objectRepo)
//        {
//            _objectRepo = objectRepo;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(Object2D obj)
//        {
//            var userId = 1;

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var created = await _objectRepo.CreateAsync(obj, userId);
//            return Ok(created);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, Object2D obj)
//        {
//            var userId = 1;
//            obj.ID = id;

//            var success = await _objectRepo.UpdateAsync(obj, userId);
//            if (!success)
//                return NotFound();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var userId = 1;
//            var success = await _objectRepo.DeleteAsync(id, userId);
//            if (!success)
//                return NotFound();

//            return NoContent();
//        }
//    }
//}
