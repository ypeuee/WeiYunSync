using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            

             new SameFile().Execute(config);
            //return;

            //文件同步
            var fileSync = new FileSync();
            fileSync.Exectue(config);
             
            Console.ReadKey();
        }


    }
}
