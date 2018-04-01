using System;
using System.Collections.Generic;
using System.IO;
using Castle.Components.DictionaryAdapter;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheTruth.Controllers;
using Xunit;
using NSubstitute;
using VideoService.Interface;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using TheTruth.ViewModels;
using Utility;

namespace BLL.VideoService.Test
{
    public class FilePathExtensionTests
    {
        private List<CategoryInfo> _categories = new List<CategoryInfo>
        {
            new CategoryInfo {Id = 1, DisplayName = "國文", Name = "Chinese"},
            new CategoryInfo {Id = 2, DisplayName = "英文", Name = "English"},
            new CategoryInfo {Id = 3, DisplayName = "數學", Name = "Math"},
            new CategoryInfo {Id = 4, DisplayName = "物理", Name = "Physical"},
            new CategoryInfo {Id = 5, DisplayName = "化學", Name = "Chemical"},
            new CategoryInfo {Id = 6, DisplayName = "社會", Name = "Social"},
            new CategoryInfo {Id = 7, DisplayName = "歷史", Name = "History"},
            new CategoryInfo {Id = 8, DisplayName = "地理", Name = "Geography"},
        };

        private string _path;

        public FilePathExtensionTests()
        {
            _path = "c:\\TestRootPath";
            foreach (var category in _categories)
                _path.InitDirectory(category.Name)
                    .InitDirectory(DateTime.Now.ToString("yyyyMMdd"));
        }

        [Fact]
        public void GetChildPathsWillGetAllChildPathInfosFromPath()
        {
            var paths = _path.GetChildPaths().ToList();
        }
    }
}