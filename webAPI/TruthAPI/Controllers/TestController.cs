using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TruthAPI.ViewModels;

namespace TruthAPI.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private static readonly ConcurrentBag<CategoryViewModel> Categories = new ConcurrentBag<CategoryViewModel>
        {
            new CategoryViewModel {DisplayName = "English 1", Id = 1},
            new CategoryViewModel {DisplayName = "English 2", Id = 2},
            new CategoryViewModel {DisplayName = "English 3", Id = 3},
            new CategoryViewModel {DisplayName = "Chiness 1", Id = 4},
            new CategoryViewModel {DisplayName = "Chiness 2", Id = 5},
            new CategoryViewModel {DisplayName = "Chiness 3", Id = 6},
        };
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var categories = Categories.Where(w => w.Id == id);

            return categories.Any()
                ? Ok(categories)
                : NotFound(id) as IActionResult;
        }
    }
}
