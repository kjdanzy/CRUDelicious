using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Dishes = _context.Dishes
                .OrderByDescending(orderDish => orderDish.CreatedAt)
                .ToList();

            return View();
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("save")]
        public IActionResult Save(Dish newDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newDish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            Console.WriteLine(newDish.Chef);
            return View("New");
        }

        
        [HttpGet("dish/{id}/details")]
        public IActionResult Details(int id)
        {
            ViewBag.detailDish = _context.Dishes
                .FirstOrDefault(dish => dish.DishId == id);
            
            return View();
        }

        [HttpGet("/dish/{id}/edit")]
        public IActionResult Edit(int id)
        {
            Dish thisDish = _context.Dishes
                .FirstOrDefault(dish => dish.DishId == id);

            return View(thisDish);
        }

        [HttpGet("/dish/{id}/delete")]
        public IActionResult Delete(int id)
        {
            Dish DeleteThisDish = _context.Dishes
                .FirstOrDefault(dish => dish.DishId == id);

            _context.Remove(DeleteThisDish);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [HttpPost("dish/submitEdit")]
        public IActionResult SubmitEdit(Dish editedDish)
        {
            Dish updatedDish = _context.Dishes
                .FirstOrDefault(dish => dish.DishId == editedDish.DishId);

            if (ModelState.IsValid)
            {
                updatedDish.DishId = editedDish.DishId;
                updatedDish.Name = editedDish.Name;
                updatedDish.Chef = editedDish.Chef;
                updatedDish.Calories = editedDish.Calories;
                updatedDish.Tastiness = editedDish.Tastiness;
                updatedDish.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return RedirectToAction("Details", new { id = editedDish.DishId });
            }

            return View("Edit", editedDish);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
