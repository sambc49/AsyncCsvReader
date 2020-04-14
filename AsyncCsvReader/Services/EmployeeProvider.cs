using System;
using System.Threading.Tasks;
using AsyncCsvReader.Models;

namespace AsyncCsvReader.Services
{
    public class EmployeeProvider
    {
        public async Task<Employee> GetEmployee(string row)
        {
            return new Employee()
            {

                Id = "test",
                FullName = "sam",
                Salary = 0
            };
        }
    }
}
