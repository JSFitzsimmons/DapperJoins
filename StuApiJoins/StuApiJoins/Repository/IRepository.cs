//-------------------------------------------------------------
//IRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StuApiJoins.Models;
namespace StuApiJoins.Repository
{
    public interface IRepository
    {
        List<Student> getStudents();

        void addStudent(Student student);

        Student getStudentById(string id);

        void deleteStudent(string id);

        void updateStudent(Student stu, string id);

        float[] getRange();

        List<Invoice> GetInventory();

        List<Customer> GetCustomerInventory();

    }
}