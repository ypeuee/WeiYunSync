using System;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            //源路径
            string pathFrom = @"D:\testFrom";
            //目的路径
            string pathTo = @"D:\testTo";


            //计算 

            new FileUpdateMonitoring().CopyAddFile(pathFrom, pathTo);
            new FileUpdateMonitoring().CopyUpdFile(pathFrom, pathTo);
            new FileUpdateMonitoring().CopyDelFile(pathFrom, pathTo);


            Console.ReadKey();
        }


    }
}
