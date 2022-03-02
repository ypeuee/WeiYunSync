using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync
{
    /// <summary>
    /// 相同文件查找
    /// </summary>
    class SameFile
    {
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path"></param>
        List<FileM> FileLists(string homePath, string path = null)
        {
            if (path == null)
                path = homePath;

            List<FileM> catchFIleList = new List<FileM>();

            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    var items = FileLists(homePath, dir.FullName);
                    catchFIleList.AddRange(items);
                }
            }

            FileM catchFile;
            var files = dirInfo.GetFiles();
            foreach (var file in files)
            {
                catchFile = new FileM()
                {
                    Name = file.Name,
                    FullName = file.FullName,//.Replace(homePath, ""),
                    Path = file.DirectoryName,//.Replace(homePath, ""),
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

        public void GetSameFile(string pathFrom, string pathTo = null)
        {
            var fromFileList = FileLists(pathFrom);
            List<FileM> toFileList;
            if (pathTo == null)
            {
                pathTo = pathFrom;
                toFileList = fromFileList;
            }
            else
            {
                toFileList = FileLists(pathTo);
            }

            //按名称匹配
            var items = MatchName(fromFileList, toFileList);
            //按文件大小匹配
            items = items.Where(m => m.FromFile.Length == m.ToFile.Length).ToList();
            //按文件最后修改时间匹配
            items = items.Where(m => m.FromFile.LastWriteTime == m.ToFile.LastWriteTime).ToList();
       
        
        }

        /// <summary>
        /// 按名称匹配相同文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> MatchName(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.Name equals t.Name into
                        items
                        from item in items.DefaultIfEmpty()
                       where item.Path != f.Path
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = item
                        };

            var list = temps.ToList();
            return list;//.ToList().Where(m => m.ToFile == null).ToList();
        }


    }
}
