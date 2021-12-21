using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class FileUpdateMonitoring
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        List<CatchFileM> catchFIleList = new List<CatchFileM>();
        Dictionary<string, int> screenFile = new Dictionary<string, int>();

        public FileUpdateMonitoring(string path)
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

            Path = path;
        }

        public void FileLists()
        {
            FileLists(Path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        void FileLists(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    Console.WriteLine(dir.Name);
                    FileLists(dir.FullName);
                }
            }

            CatchFileM catchFile;
            var files = dirInfo.GetFiles();
            foreach (var file in files)
            {
                catchFile = new CatchFileM()
                {
                    FileName = file.FullName,
                    Path = file.DirectoryName,
                    CreationTime = file.CreationTime,
                    LastWriteTime = file.LastWriteTime,
                    LastAccessTime = file.LastAccessTime,
                    Attributes = file.Attributes
                };
                catchFIleList.Add(catchFile);

                if (!IsScreenFileAttributes(file.Attributes))
                {
                    try
                    {
                        var open = file.OpenRead();
                        open.Close();
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
                    Console.WriteLine($"{file.Name}--{ file.Attributes}");
                }
            }

        }

        /// <summary>
        /// 获取变动的文件
        /// </summary>
        public void GetLastWriteFile()
        {
            DateTime time = DateTime.Now;
            Console.WriteLine(time);
            GetLastWriteFile(Path, time);

            foreach (var file in catchFIleList.Where(m => m.MonitorintTime != time && m.FilOperation != FilOperation.Delete))
            {
                file.FilOperation = FilOperation.Delete;
                file.MonitorintTime = time;
                Console.WriteLine($"删除文件--{file.Path}\\{file.FileName}");
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
                    FileName = file.FullName,
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
                        var open = file.OpenRead();
                        open.Close();
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

                    var oldCatchFile = catchFIleList.FirstOrDefault(m => m.FileName == catchFile.FileName && m.Path == catchFile.Path);
                    if (oldCatchFile == null)
                    {
                        Console.WriteLine($"新加文件--{catchFile.Path}\\{catchFile.FileName}");

                        catchFile.FilOperation = FilOperation.Add;
                        catchFIleList.Add(catchFile);
                        continue;
                    }
                    if (oldCatchFile.LastWriteTime != catchFile.LastWriteTime)
                    {
                        oldCatchFile.FilOperation = FilOperation.Update;
                        oldCatchFile.LastWriteTime = catchFile.LastWriteTime;
                        Console.WriteLine($"修改文件--{catchFile.Path}\\{catchFile.FileName}");
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

    public class CatchFileM
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

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
