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
            new CategoryInfo {Id = 1, DisplayName = "���", Name = "Chinese"},
            new CategoryInfo {Id = 2, DisplayName = "�^��", Name = "English"},
            new CategoryInfo {Id = 3, DisplayName = "�ƾ�", Name = "Math"},
            new CategoryInfo {Id = 4, DisplayName = "���z", Name = "Physical"},
            new CategoryInfo {Id = 5, DisplayName = "�ƾ�", Name = "Chemical"},
            new CategoryInfo {Id = 6, DisplayName = "���|", Name = "Social"},
            new CategoryInfo {Id = 7, DisplayName = "���v", Name = "History"},
            new CategoryInfo {Id = 8, DisplayName = "�a�z", Name = "Geography"},
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