using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using StuApiJoins.Models;

namespace StuApiJoins.Repository
{
    public class StuRepository : IRepository
    {
        string connectStr = "Server=localhost; Database=warehousedb; UID=bill; Password=1777";

        public List<Student> getStudents()
        {
            string sql = "Select * from Student";
            using (var connection = new MySqlConnection(connectStr))
            {
                List<Student> stu = connection.Query<Student>(sql).ToList();
                return stu;
            }
        }

        public Student getStudentById(string id)
        {

            string code = "Select * from Student where StudentId=@StudentId";
            using (var connection = new MySqlConnection(connectStr))
            {
                Student stu = connection.QuerySingleOrDefault<Student>(code, new { StudentId = id });
                return stu;
            }
        }

        public float[] getRange()
        {

            string code = "Select min(studentGpa) as min, max(StudentGpa) as max from Student";
            float[] range = new float[2];
            using (var connection = new MySqlConnection(connectStr))
            {
                var result = connection.Query(code).Single();
                range[0] = result.min();
                range[1] = result.max();
                return range;
            }
        }


        public void addStudent(Student stu)
        {
            string code = "insert into Student (StudentId, StudentName, StudentGpa) " +
                "Values (@StudentId, @StudentName, @StudentGpa);";
            using (var connection = new MySqlConnection(connectStr))
            {
                connection.Execute(code, new { StudentName = stu.StudentName, StudentId = stu.StudentId, StudentGpa = stu.StudentGpa });
            }
        }

        public void updateStudent(Student stu, string id)
        {
            string code = "UPDATE Student SET StudentId = @StudentId, StudentName = @StudentName, " +
                "StudentGpa = @StudentGPA WHERE StudentId = @StudentId;";
            using (var connection = new MySqlConnection(connectStr))
            {
                connection.Execute(code, new { StudentName = stu.StudentName, StudentId = stu.StudentId, StudentGpa = stu.StudentGpa });
            }
        }


        public void deleteStudent(string id)
        {
            string code = "delete from Student where StudentId = @StudentId;";
            using (var connection = new MySqlConnection(connectStr))
            {
                connection.Execute(code, new { StudentId = id });
            }
        }
//-------------------------------------------------------------------------------------------------------------
        public List<Invoice> GetInventory()
        {
            string sql = "SELECT * FROM INVOICE AS A INNER JOIN LINE AS B ON A.INV_NUMBER = B.INV_NUMBER";

            List<Invoice> list;

            using (var connection = new MySqlConnection(connectStr))
            {
                var invoiceDictionary = new Dictionary<int, Invoice>();

                list = connection.Query<Invoice, LineItem, Invoice>(
                   sql,
                   (invoice, lineItem) =>
                   {
                       Invoice invoiceEntry;

                       if (!invoiceDictionary.TryGetValue(invoice.INV_NUMBER, out invoiceEntry))
                       {
                           invoiceEntry = invoice;
                           invoiceEntry.InvoiceLineItems = new List<LineItem>();
                           invoiceDictionary.Add(invoiceEntry.INV_NUMBER, invoiceEntry);
                       }

                       invoiceEntry.InvoiceLineItems.Add(lineItem);
                       return invoiceEntry;
                   },
                   splitOn: "LINE_NUMBER")
               .Distinct()
               .ToList();
            }

            return list;
        }
//--------------------------------------------------------------------------------------------------------
        public List<Customer> GetCustomerInventory()
        {
            string sql = "SELECT * FROM CUSTOMER AS A INNER JOIN INVOICE AS B ON A.CUS_CODE = B.CUS_CODE";

            List<Customer> list;

            using (var connection = new MySqlConnection(connectStr))
            {
                var cusDictionary = new Dictionary<int, Customer>();

                list = connection.Query<Customer, Invoice, Customer>(
                   sql,
                   (customer, invoice) =>
                   {
                       Customer cusEntry;

                       if (!cusDictionary.TryGetValue(customer.Cus_Code, out cusEntry))
                       {
                           cusEntry = customer;
                           cusEntry.Cus_Invoices = new List<Invoice>();
                           cusDictionary.Add(cusEntry.Cus_Code, cusEntry);
                       }

                       cusEntry.Cus_Invoices.Add(invoice);
                       return cusEntry;
                   },
                   splitOn: "INV_DATE")
               .Distinct()
               .ToList();
            }

            return list;
        }

    }
}