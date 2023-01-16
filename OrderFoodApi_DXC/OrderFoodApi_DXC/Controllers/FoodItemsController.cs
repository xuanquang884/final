using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OrderFoodApi_DXC.Api.Data;
using OrderFoodApi_DXC.Data;

namespace OrderFoodApi_DXC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly OrderFoodDBContext _context;
        public FoodItemsController(OrderFoodDBContext context)
        {
            _context = context;
        }

        // GET: api/FoodItems
        [HttpGet]
        /* [Authorize]*/
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
        {
            return await _context.FoodItems
                .Select(x => new FoodItem()
                {
                    FoodId = x.FoodId,
                    Title = x.Title,
                    Desc = x.Desc,
                    ImageName = x.ImageName,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
                }).ToListAsync();
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(int id)
        {
            try
            {
                var foodItem = await _context.FoodItems.FindAsync(id);

                if (foodItem == null)
                {
                    return NotFound();
                }

                return foodItem;
            }
            catch (Exception)
            {

                throw;
            }

        }

        // PUT: api/FoodItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, [FromForm] FoodItem foodItem)
        {
            if (id != foodItem.FoodId)
            {
                return BadRequest();
            }

            if (foodItem.ImageFile != null)
            {
                DeleteImage(foodItem.ImageName);
                foodItem.ImageName = await SaveImage(foodItem.ImageFile);
            }

            _context.Entry(foodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch 
            {
                return BadRequest();
            }

            return NoContent();
        }


        // POST: api/FoodItems
        [HttpPost]
        public async Task<ActionResult<FoodItem>> PostFoodItem([FromForm] FoodItem food)
        {
            food.ImageName = await SaveImage(food.ImageFile);
            _context.FoodItems.Add(food);
            await _context.SaveChangesAsync();
            return Ok(food);
        }
        // POST With Image: api/FoodItems


        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodItem>> DeleteEmployeeModel(int id)
        {
            var food = await _context.FoodItems.SingleOrDefaultAsync(e=>e.FoodId==id);
            if (food == null)
            {
                return NotFound();
            }
            /*DeleteImage(food.ImageName);*/
            _context.FoodItems.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
