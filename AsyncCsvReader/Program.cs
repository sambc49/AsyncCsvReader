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
            var queue = new BlockingCollection<string>();
            var employeeCollection = new BlockingCollection<Employee>();

            var readFileTask = Task.Run(async () => await ReadFile(queue).ConfigureAwait(false));
            var writeToFileTask = Task.Run(async () => await WriteToFile(employeeCollection).ConfigureAwait(false));

            var getEmployeeInfo = Task.Run(async () =>
            {
                foreach (var item in queue.GetConsumingEnumerable())
                {
                    employeeCollection.Add(await ProcessEmployee(item).ConfigureAwait(false));
                }
                employeeCollection.CompleteAdding();
            });


            var writeToConsole = Task.Run(async () => await WriteToConsole().ConfigureAwait(false));
            Task.WaitAll(readFileTask, writeToFileTask, getEmployeeInfo);

            _logToConsole = false;
            writeToConsole.Wait();
        }
        static async Task ReadFile(BlockingCollection<string> queue)
        {
            using (StreamReader reader = File.OpenText("Resources/EmployeeData.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync().ConfigureAwait(false);

                    var values = line.Split(',');

                    queue.Add(values[1]);
                    Interlocked.Increment(ref _totalRecordsRead);
                }
                queue.CompleteAdding();
            }
        }

        private static async Task<Employee> ProcessEmployee(string id)
        {
            try
            {
                //Here do the provider look up
                return await GetEmployeeFromService(id).ConfigureAwait(false);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new Employee();
        }

        private async static Task<Employee> GetEmployeeFromService(string id)
        {
            var employeeProvider = new EmployeeProvider();
            var result = employeeProvider.GetEmployee(id);

            return await result;
        }

        static async Task WriteToFile(BlockingCollection<Employee> collection)
        {
            using (StreamWriter file = new StreamWriter("Resources/EmployeeOutput.json", true))
            {
                foreach (var item in collection.GetConsumingEnumerable())
                {
                    Interlocked.Increment(ref _totalRecordsUpdated);
                    await file.WriteLineAsync(JsonConvert.SerializeObject(item)).ConfigureAwait(false);
                }
            }
        }

        static async Task WriteToConsole()
        {
            Console.WriteLine("Starting csv reader...");
            while (_logToConsole)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    Console.WriteLine($"Total number of records added: {_totalRecordsUpdated}");
                    Console.WriteLine($"Total number of records read: {_totalRecordsRead}");
                });
            }
        }

    }
}
