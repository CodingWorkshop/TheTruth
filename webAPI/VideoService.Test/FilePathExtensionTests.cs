using System;
using System.Collections.Generic;
using System.IO;
using Castle.Components.DictionaryAdapter;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TruthAPI.Controllers;
using Xunit;
using NSubstitute;
using VideoService.Interface;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using TruthAPI.ViewModels;
using Utility;

namespace BLL.VideoService.Test
{
    public class FilePathExtensionTests
    {
        private List<Category> _categories = new List<Category>
        {
            new Category {Id = 1, DisplayName = "���", Name = "Chinese"},
            new Category {Id = 2, DisplayName = "�^��", Name = "English"},
            new Category {Id = 3, DisplayName = "�ƾ�", Name = "Math"},
            new Category {Id = 4, DisplayName = "���z", Name = "Physical"},
            new Category {Id = 5, DisplayName = "�ƾ�", Name = "Chemical"},
            new Category {Id = 6, DisplayName = "���|", Name = "Social"},
            new Category {Id = 7, DisplayName = "���v", Name = "History"},
            new Category {Id = 8, DisplayName = "�a�z", Name = "Geography"},
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