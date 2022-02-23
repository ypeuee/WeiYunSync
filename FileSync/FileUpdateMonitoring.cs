﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSync
{
    class FileUpdateMonitoring
    {


        List<CatchFileM> catchFIleList = new List<CatchFileM>();
        Dictionary<string, int> screenFile = new Dictionary<string, int>();

        public FileUpdateMonitoring()
        {
            screenFile.Add(FileAttributes.ReadOnly.ToString(), 1);
            screenFile.Add(FileAttributes.Hidden.ToString(), 1);
            screenFile.Add(FileAttributes.System.ToString(), 4);
            screenFile.Add(FileAttributes.Directory.ToString(), 16);
            screenFile.Add(FileAttributes.Device.ToString(), 32);
            screenFile.Add(FileAttributes.Temporary.ToString(), 256);
            screenFile.Add(FileAttributes.SparseFile.ToString(), 512);
            screenFile.Add(FileAttributes.ReparsePoint.ToString(), 1024);
            screenFile.Add(FileAttributes.Compressed.ToString(), 2048);
            screenFile.Add(FileAttributes.Offline.ToString(), 4096);
            screenFile.Add(FileAttributes.NotContentIndexed.ToString(), 8192);
            screenFile.Add(FileAttributes.Encrypted.ToString(), 16384);
            screenFile.Add(FileAttributes.IntegrityStream.ToString(), 32768);
            screenFile.Add(FileAttributes.NoScrubData.ToString(), 131072);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        List<CatchFileM> FileLists(  string homePath,string path= null )
        {
            if (path == null)
                path = homePath;

            List<CatchFileM> catchFIleList = new List<CatchFileM>();

            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    Console.WriteLine(dir.Name);
                    var items = FileLists(  homePath , dir.FullName);
                    catchFIleList.AddRange(items);
                }
            }

            CatchFileM catchFile;
            var files = dirInfo.GetFiles();
            foreach (var file in files)
            {
                catchFile = new CatchFileM()
                {
                    Name = file.Name,
                    FullName = file.FullName.Replace(homePath, ""),
                    Path = file.DirectoryName.Replace(homePath, ""),
                    Extension = file.Extension,
                    CreationTime = file.CreationTime,
                    LastWriteTime = file.LastWriteTime,
                    LastAccessTime = file.LastAccessTime,
                    Attributes = file.Attributes,
                    Length = file.Length
                };
                catchFIleList.Add(catchFile);
            }
            return catchFIleList;
        }


        public void CopyAddFile(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                File.Copy(pathFrom + item.FullName, toFile);
            }
             
        }

        public void CopyUpdFile(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);
             
            //upd
            var tempUpds = CalculateUpdFile(fromFileList, toFileList);
            foreach (var item in tempUpds)
            {
                Console.WriteLine($"upd {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");
                File.Copy(pathFrom + item.FromFile.FullName, pathTo+ item.ToFile.FullName, true);
            }
             
        }

        public void CopyDelFile(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);
             
            //del
            var tempDels = CalculateDelFile(fromFileList, toFileList);
            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");

                File.Delete(pathTo + item.FullName);
            }

        }

        
         
        public void CopyAddFile1(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                File.Copy(pathFrom + item.FullName, toFile);
            }

            //upd
            var tempUpds = CalculateUpdFile(fromFileList, toFileList);
            foreach (var item in tempUpds)
            {
                Console.WriteLine($"upd {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");
                File.Copy(item.FromFile.FullName, item.ToFile.FullName, true);
            }

            //move
            //var tempMoves = CalculateMoveFile(fromFileList, toFileList);
            //foreach (var item in tempMoves)
            //{
            //    Console.WriteLine($"move {item.FromFile.FullName} {item.FromFile.Path}:{item.ToFile.Path}");
            //}

            //del
            var tempDels = CalculateDelFile(fromFileList, toFileList);
            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");

                File.Delete(item.FullName);
            }

            //updTo
            var tempUpdTos = CalculateToUpdFile(fromFileList, toFileList);
            foreach (var item in tempUpdTos)
            {
                Console.WriteLine($"updTo {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");

            }

        }


        /// <summary>
        /// 计算新增文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CatchFileM> CalculateAddFile(List<CatchFileM> fromFileList, List<CatchFileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.FullName equals t.FullName into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new
                        {
                            fromFile = f,
                            toFile = item
                        };
            //var list = temps.ToList();
            return temps.ToList().Where(m => m.toFile == null).Select(m => m.fromFile).ToList();
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateUpdFile(List<CatchFileM> fromFileList, List<CatchFileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.FullName equals t.FullName
                        // into items
                        //from item in items.DefaultIfEmpty()
                        //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile.Length != m.ToFile.Length ||
            //m.FromFile.LastAccessTime != m.ToFile.LastAccessTime ||
            m.FromFile.LastWriteTime != m.ToFile.LastWriteTime).ToList();
        }

        /// <summary>
        /// 计算移动文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateMoveFile(List<CatchFileM> fromFileList, List<CatchFileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on new { t.Name, t.Length } equals new { f.Name, f.Length } into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = item,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile == null).ToList();
        }


        /// <summary>
        /// 计算删除文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CatchFileM> CalculateDelFile(List<CatchFileM> fromFileList, List<CatchFileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on t.FullName equals f.FullName into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = item,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile == null).Select(m => m.ToFile).ToList();
        }

        /// <summary>
        /// 计算目标被修改文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateToUpdFile(List<CatchFileM> fromFileList, List<CatchFileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on t.FullName equals f.FullName
                        //into  items
                        //from item in items.DefaultIfEmpty()
                        //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile.Length != m.ToFile.Length ||
           m.FromFile.LastAccessTime != m.ToFile.LastAccessTime ||
           m.FromFile.LastWriteTime != m.ToFile.LastWriteTime).ToList();
        }




        /// <summary>
        /// 获取变动的文件
        /// </summary>
        public void GetLastWriteFile()
        {
            DateTime time = DateTime.Now;
            Console.WriteLine(time);
            //GetLastWriteFile(Path, time);



            foreach (var file in catchFIleList.Where(m => m.MonitorintTime != time && m.FilOperation != FilOperation.Delete))
            {
                file.FilOperation = FilOperation.Delete;
                file.MonitorintTime = time;
                Console.WriteLine($"删除文件--{file.Path}\\{file.FullName}");
            }
        }

        /// <summary>
        /// 获取变动的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="time">上次检测的时间</param>
        void GetLastWriteFile(string path, DateTime time)
        {
            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    GetLastWriteFile(dir.FullName, time);
                }
            }

            CatchFileM catchFile;
            var files = dirInfo.GetFiles();

            foreach (var file in files)
            {
                catchFile = new CatchFileM()
                {
                    FullName = file.FullName,
                    Path = file.DirectoryName,
                    CreationTime = file.CreationTime,
                    LastWriteTime = file.LastWriteTime,
                    LastAccessTime = file.LastAccessTime,
                    Attributes = file.Attributes,
                    MonitorintTime = time,
                };

                if (!IsScreenFileAttributes(file.Attributes))
                {
                    try
                    {
                        //var open = file.OpenRead();
                        //open.Close();
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        Console.WriteLine("System.IO.FileInfo.Name is read-only or is a directory.");
                        catchFile.FileStatus = FileStatus.ReadOnly;
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        Console.WriteLine("The specified path is invalid, such as being on an unmapped drive.");
                        catchFile.FileStatus = FileStatus.Exception;
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine("The file is already open.");
                        catchFile.FileStatus = FileStatus.Opened;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Exception");
                        catchFile.FileStatus = FileStatus.Exception;
                    }

                    var oldCatchFile = catchFIleList.FirstOrDefault(m => m.FullName == catchFile.FullName && m.Path == catchFile.Path);
                    if (oldCatchFile == null)
                    {
                        Console.WriteLine($"新加文件--{catchFile.Path}\\{catchFile.FullName}");

                        catchFile.FilOperation = FilOperation.Add;
                        catchFIleList.Add(catchFile);
                        continue;
                    }
                    if (oldCatchFile.LastWriteTime != catchFile.LastWriteTime)
                    {
                        oldCatchFile.FilOperation = FilOperation.Update;
                        oldCatchFile.LastWriteTime = catchFile.LastWriteTime;
                        Console.WriteLine($"修改文件--{catchFile.Path}\\{catchFile.FullName}");
                    }
                    //更新监控扫描时间
                    oldCatchFile.MonitorintTime = time;
                }
            }



        }

        /// <summary>
        /// 匹配文件属性是否过虑
        /// </summary>
        /// <param name="fileAttributes">文件属性</param>
        /// <returns>ture过虑，false不过虑</returns>
        bool IsScreenFileAttributes(FileAttributes fileAttributes)
        {
            string[] strs = fileAttributes.ToString().Split(", ");
            foreach (string str in strs)
            {
                if (screenFile.ContainsKey(str))
                    return true;
            }
            return false;
        }

    }

    /// <summary>
    /// 计算移动
    /// </summary>
    public class CalcuateFileM
    {
        /// <summary>
        /// 源
        /// </summary>
        public CatchFileM FromFile { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public CatchFileM ToFile { get; set; }
    }

    public class CatchFileM
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 文件当前状态
        /// </summary>
        public FileStatus FileStatus { get; set; } = FileStatus.Normal;

        /// <summary>
        /// 操作属性
        /// </summary>
        public FilOperation FilOperation { get; set; } = FilOperation.Normal;

        /// <summary>
        /// 监控扫描时间
        /// </summary>
        public DateTime MonitorintTime { get; set; }

        /// <summary>
        /// 获取或设置当前文件或目录的属性。
        /// </summary>
        public FileAttributes Attributes { get; set; }

        /// <summary>
        /// 文档大小
        /// </summary>
        public long Length { get; set; }
    }

    /// <summary>
    /// 操作属性
    /// </summary>
    public enum FilOperation
    {
        Normal = 0,
        Add = 1,
        Update = 2,
        Delete = 3,
    }

    /// <summary>
    /// 文件当时状态
    /// </summary>
    public enum FileStatus
    {
        Normal = 0,
        ReadOnly = 1,
        Opened = 2,
        Exception = 3,
    }
}