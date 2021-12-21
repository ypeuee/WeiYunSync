using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string path = @"D:\Sync\Work\";

            var file = new FileUpdateMonitoring(path);
            file.FileLists();


            do
            {
                System.Threading.Thread.Sleep(1000);


                file.GetLastWriteFile();

            } while (true);
        }
    }
}
