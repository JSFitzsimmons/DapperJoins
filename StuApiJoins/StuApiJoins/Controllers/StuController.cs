using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using StuApiJoins.Models;
using StuApiJoins.Repository;

namespace StuApiJoins.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuController : ControllerBase
    {
        private IRepository _repository;

        public StuController(IRepository context)
        {
            _repository = context;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return _repository.getStudents();
        }

        [HttpGet]
        [Route("range")]
        public ActionResult<float[]> GetRange()
        {
            return _repository.getRange();
        }

        [HttpGet]
        [Route("inv")]
        public ActionResult<List<Invoice>> GetInv()
        {
            return _repository.GetInventory();
        }

        [HttpGet]
        [Route("cusInv")]
        public ActionResult<List<Customer>> GetCusInv()
        {
            return _repository.GetCustomerInventory();
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public ActionResult<Student> GetById(string id)
        {
            return _repository.getStudentById(id);
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            _repository.addStudent(student);
            return CreatedAtRoute("GetStudent", new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Student student)
        {
            _repository.updateStudent(student, id);
            return CreatedAtRoute("GetStudent", new { id = student.StudentId }, student);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var stu = _repository.getStudentById(id);
            if (stu == null)
            {
                return NotFound();
            }
            _repository.deleteStudent(id);
            return NoContent();
        }

    }
}