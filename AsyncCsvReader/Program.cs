using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsyncCsvReader.Models;
using AsyncCsvReader.Services;
using Newtonsoft.Json;

namespace AsyncCsvReader
{
    public class Program
    {
        private static int _totalRecordsUpdated = 0;
        private static int _totalRecordsRead = 0;
        private static bool _logToConsole = true;

        static void Main(string[] args)
        {
            //Read file
            //Convert to employee object
            //Write to file
        }
        static async Task ReadFile(BlockingCollection<string> queue)
        {
            
        }

        private static async Task<Employee> ProcessEmployee(string id)
        {
            try
            {
                return new Employee()
                {
                    
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new Employee();
        }

        static async Task WriteToFile(BlockingCollection<Employee> collection)
        {
            using (StreamWriter file = new StreamWriter("output path", true))
            {
                foreach (var item in collection.GetConsumingEnumerable())
                {
                    Interlocked.Increment(ref _totalRecordsUpdated);
                    await file.WriteLineAsync(JsonConvert.SerializeObject(item)).ConfigureAwait(false);
                }
            }
        }

    }
}
