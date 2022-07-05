using System;
using System.Threading.Tasks;

namespace Whisbee.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Whisbee.Run();

            Console.ReadKey();
        }
    }
}
