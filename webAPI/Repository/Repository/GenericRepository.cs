using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataAccess;
using Repository.Interface;

namespace Repository.Repository
{
    public class GenericRepository
    {
    }

    public class GenericFileRepository : IRepository<Video>
    {
        private readonly string _rootPath;

        public GenericFileRepository(string rootPath)
        {
            _rootPath = rootPath;
        }

        public List<Video> GetAll()
        {
            var dirInfo = new DirectoryInfo(_rootPath);

            var files = new List<Video>();

            foreach (var dir in dirInfo.GetDirectories())
            {
                files.AddRange(GetVideosWithCategory(dir, dir.Name));
            }

            return files;
        }

        private List<Video> GetVideosWithCategory(DirectoryInfo dir, string category)
        {
            var result = new List<Video>();

            foreach (var d in dir.GetDirectories())
            {
                var files = GetVideosWithDate(d, d.Name).Select(s =>
                {
                    s.Category = category; return s;
                });

                result.AddRange(files);
            }

            return result;
        }

        private List<Video> GetVideosWithDate(DirectoryInfo dir, string date)
        {
            var files = dir.GetFiles();

            return files.Select(f => new Video()
            {
                Date = date,
                Url = f.FullName,
            }).ToList();
        }
    }
}