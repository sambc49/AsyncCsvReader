using System;
using AsyncCsvReader.Models;

namespace AsyncCsvReader.Services
{
    public class EmployeeProvider
    {
        public Employee GetEmployee(string row)
        {
            //split the row to get values
            return new Employee()
            {
                Id = "test",
                FullName = "sam",
                Salary = 0
            };
        }
    }
}
