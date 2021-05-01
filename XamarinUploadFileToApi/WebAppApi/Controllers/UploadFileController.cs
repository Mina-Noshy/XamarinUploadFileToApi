using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;

        public UploadFileController(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Employee model)
        {
            // get id value from request header
            model.Id = int.Parse(Request.Headers["Id"].ToString());

            //root path
            var folderPath = Path.Combine(_environment.WebRootPath, "Images");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

            var fullPath = Path.Combine(folderPath, fileName);

            if (model.Image.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            if(System.IO.File.Exists(fullPath))
            {
                return Ok(new EmployeeVM 
                { 
                    Id = model.Id,
                    Name = model.Name,
                    Phone = model.Phone,
                    Image = fullPath 
                });
            }

            return BadRequest("upload faild!");
        }


        [HttpGet]
        public async Task<EmployeeVM> Get()
        {
            EmployeeVM _employee = new EmployeeVM
            {
                Id = 3,
                Name = "Mina Noshy",
                Phone = "01111257052",
                Image = "http://eastaria.com/wwwroot/Images/software-company.jpg"
            };

            await Task.Delay(1);

            return _employee;
        }

    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public IFormFile Image { get; set; }
    }

    public class EmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }

}
