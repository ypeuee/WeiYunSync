using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始执行");
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            //查询相同
            if (bool.Parse(config["SameFile:Execute"]))
            {
                Console.WriteLine("查询相同");
                new SameFile().Execute(config);
            }

            //文件同步
            if (bool.Parse(config["FileSync:Execute"]))
            {
                Console.WriteLine("文件同步");
                var fileSync = new FileSync();
                fileSync.Exectue(config);
            }

            Console.WriteLine("完成执行，按任意键退出。");
            Console.ReadKey();
        }


    }
}
