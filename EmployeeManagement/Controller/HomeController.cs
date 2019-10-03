using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //[Route("Home")]
    //[Route("[controller]/[action]")]
    [Authorize]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeeRepoistory;
        private IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository, 
            IHostingEnvironment hostingEnvironment,
            ILogger<HomeController> logger)
        {
            _employeeeRepoistory = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        //[Route("")]
        ////[Route("Index")]
        //[Route("~/Home")]
        //[Route("~/")]
        [AllowAnonymous]
        public ViewResult Index()
        {
            //return _employeeeRepoistory.GetEmployee(1).Name;
            var model = _employeeeRepoistory.GetAllEmployees();
            return View(model);
        }
        //[Route("Details/{id?}")]
        //[Route("{id?}")]
        public ViewResult Details(int? id)
        {
            //Employee employee = _employeeeRepoistory.GetEmployee(1);
            //ViewBag.Employee = employee;
            //ViewBag.Title = "Employee Details";
            // return View(employee);
            //throw new Exception("Error");
            Employee employee = _employeeeRepoistory.GetEmployee(id.Value);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                //if (model.Photos != null && model.Photos.Count > 0)
                //{
                //    foreach (IFormFile photo in model.Photos)
                //    {
                //        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //    }

                //}

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName

                };
                _employeeeRepoistory.Add(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });

            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var employee = _employeeeRepoistory.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeeRepoistory.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photos != null)
                {
                    if (employee.PhotoPath != null)
                    {
                        string filename = Path.Combine(hostingEnvironment.WebRootPath, "images", employee.PhotoPath);
                        System.IO.File.Delete(filename);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }


                _employeeeRepoistory.Update(employee);


            }
            return RedirectToAction("Index");

        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photos.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}