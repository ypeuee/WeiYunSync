using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            new SameFile().GetSameFile(@"D:\testFrom");

            return;
              var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");
            var config = builder.Build();

 

            //Console.WriteLine(config.GetSection("ConnectionStrings:DevContext").Value); // 分层键

            //源路径
            string pathFrom = config["PathFrom"];// @"D:\testFrom";
            //目的路径
            string pathTo = config["PathTo"];// @"D:\testTo";

            Console.WriteLine($"pathFrom: {pathFrom}");
            Console.WriteLine($"pathTo: {pathTo}");
            do
            {
                var dirSync = new DirectoriesSync();
                var fileSync = new FileSync();
                dirSync.DirectoriesReName(pathFrom, pathTo);

                //文件
                fileSync.CopyAddFile(pathFrom, pathTo);
                fileSync.CopyUpdFile(pathFrom, pathTo);
                fileSync.CopyDelFile(pathFrom, pathTo);
                //文件夹
                dirSync.CopyAddDirectories(pathFrom, pathTo);
                dirSync.CopyDelDirectories(pathFrom, pathTo);

                System.Threading.Thread.Sleep(1000);
            } while (true);


            Console.ReadKey();
        }


    }
}
