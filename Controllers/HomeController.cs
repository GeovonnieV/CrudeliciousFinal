using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Crudummy.Models;

namespace Crudummy.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;


        public HomeController(MyContext context)
        {
            _context = context;
        }
        // List all dishes
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllDishes = _context.Dishes
                .OrderByDescending(d => d.CreatedAt);
            return View();
        }
        // displays the NewDish view
        [HttpGet("New")]
        public IActionResult NewDish()
        {
            return View();
        }
        // Adding Dish
        [HttpPost("DishAdd")]
        public IActionResult DishAdd(Dish newDish)
        {
            _context.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // Get a single dish
        [HttpGet("/{dishId}")]
        public IActionResult OneDish(int dishId)
        {
            Dish RetrievedDish = _context.Dishes
             .SingleOrDefault(dish => dish.DishId == dishId);
            ViewBag.OneDish = RetrievedDish;
            return View();
        }
        // Delete a dish
        [HttpGet("delete/{dishId}")]
        public IActionResult DeleteDish(int dishId)
        {
            Dish RetrievedDish = _context.Dishes
             .SingleOrDefault(dish => dish.DishId == dishId);
            
            _context.Dishes.Remove(RetrievedDish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // get update a dish view
        [HttpGet("update/{dishId}")]
        public IActionResult EditDish(int dishId)
        {
            Dish RetrievedDish = _context.Dishes
             .SingleOrDefault(dish => dish.DishId == dishId);
            // display what dish were editing
              ViewBag.OneDish = RetrievedDish;
            return View();
        }
        // Post for update dish that will do the posting logic
         [HttpPost("update/{dishId}")]
        public IActionResult EditDishPost(Dish updatedDish, int dishId)
        {
            Dish RetrievedDish = _context.Dishes
        .FirstOrDefault(dish => dish.DishId == dishId);
            
            RetrievedDish.Name = updatedDish.Name;
            RetrievedDish.Chef = updatedDish.Chef;
            RetrievedDish.Calories = updatedDish.Calories;
            RetrievedDish.Tastiness = updatedDish.Tastiness;
            RetrievedDish.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
