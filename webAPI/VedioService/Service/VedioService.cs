using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting.Internal;
using Repository.Interface;
using Repository.Repository;
using Utility;
using VideoService.Interface;

namespace VideoService.Service
{
    public class VideoService : IVideoService
    {
        private readonly ReservationRepository _reservation;
        private readonly CategoryRepository _category;
        private readonly ClientIdentityRepository _clientIdentity;
        private readonly string _videoPath;

        public IEnumerable<Video> Videos => _videoPath.GetChildPaths().Select(PathInfoToVideoInfo);

        public IEnumerable<Category> Categories => _category.GetAll();

        public VideoService(
            HostingEnvironment hostingEnvironment,
            IRepository<Category> category,
            IRepository<ClientIdentity> clientIdentity,
            IRepository<Reservation> reservation)
        {
            _reservation = reservation as ReservationRepository;
            _category = category as CategoryRepository;
            _clientIdentity = clientIdentity as ClientIdentityRepository;
            _videoPath = $"{hostingEnvironment.WebRootPath ?? hostingEnvironment.ContentRootPath}\\Videos";

            Init();
            CheckTime(1000);
        }

        private void Init()
        {
            foreach(var identity in _clientIdentity.GetAll())
                VideoUtility.AddClientIdentity(identity);

            foreach (var category in _category.GetAll())
                _videoPath.InitDirectory(category.Name)
                    .InitDirectory(DateTime.Now.ToString("yyyyMMdd"));
        }

        public IEnumerable<Video> SearchVideos(IEnumerable<int> categoryIds, DateTime? beginTime, DateTime? endTime)
        {
            var query = Videos;

            if(categoryIds.Any())
                query = query.Where(w => categoryIds.Any(where => where == w.CategoryId));

            if(beginTime != null && beginTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime >= beginTime);
            else
                query = query.Where(w => w.DateTime >= DateTime.Today.AddMonths(-1));

            if(endTime != null && endTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime <= endTime);
            else
                query = query.Where(w => w.DateTime <= DateTime.Now);

            return query.ToList();
        }

        public string SetVideos(IEnumerable<string> codes, string id, DateTime startTime, DateTime endTime)
        {
            //_reservation
            if(startTime > DateTime.Now) { 
                
                _reservation.AddClientReservation(id,startTime,endTime,codes);
                return VideoUtility.GetConnectionIdByIp(id);
            }
            var videos = new List<Video>();

            foreach(var code in codes)
            {
                var video = videos
                    .Where(w => w.Code == code);

                if(video.Any())
                    videos.Add(video.First());
            }

            VideoUtility.UpdateVideo(id, videos);
            VideoUtility.UpdateActiveStatus(id, true);
            return VideoUtility.GetConnectionIdByIp(id);
        }

        public string CleanVideo(string id)
        {
            VideoUtility.UpdateVideo(id, new List<Video>());
            VideoUtility.UpdateActiveStatus(id, false);
            return VideoUtility.GetConnectionIdByIp(id);
        }

        public void AddConnetionId(string id, string connectionId)
        {
            VideoUtility.AddConnetionId(id, connectionId);
        }

        public void UpdateOnlineStatus(string id, bool isOnline)
        {
            VideoUtility.UpdateOnlineStatus(id, isOnline);
        }

        public void RemoveConnetionId(string id)
        {
            VideoUtility.RemoveConnetionId(id);
        }

        public IEnumerable<Video> GetVideoListById(string id)
        {
            return VideoUtility.GetClientVideo(id);
        }

        public string GetVideoByCode(string code, string id)
        {
            return VideoUtility.GetClientVideo(id)
                .FirstOrDefault(v => v.Code == code) ?
                .Url
                .Replace(_videoPath, "~/Videos");
        }

        public IEnumerable<Category> GetCategories()
        {
            return Categories;
        }

        public IEnumerable<ClientIdentity> GetClientIdentities()
        {
            return VideoUtility.GetAllClientInfo();
        }

        private Video PathInfoToVideoInfo(PathInfo pathInfo)
        {
            var category = pathInfo.Hierarchy[0];
            var date = pathInfo.Hierarchy[1];

            var cate = Categories.FirstOrDefault(c => c.Name == category);

            return new Video
            {
                CategoryId = cate?.Id ?? 0,
                    CategoryName = cate?.DisplayName,
                    Category = category,
                    Date = date,
                    Url = pathInfo.FullName,
                    Name = pathInfo.Name
            };
        }

        public void CheckTime(int intervalTime)
        {
            Task.Run(() =>
            {
                CheckTimeService(intervalTime);
            });
        }

        private void CheckTimeService(int interval)
        {
            while(true)
            {
                var now = DateTime.Now;
                //時間到移除影片跟預約
                CheckTimeToRevomeVideo(now);

                //時間到設定影片
                CheckTimeToSetVideo(now);

                Thread.Sleep(interval);
            }
        }

        private void CheckTimeToSetVideo(DateTime now)
        {
            foreach(var clientReservation in _reservation.GetAll())
            {
                foreach(var setVideo in clientReservation.Reservations.Where(r => r.StartTime >= now))
                {
                    // 新增影片
                    SetVideos(setVideo.Codes, clientReservation.ClientId, setVideo.StartTime, setVideo.EndTime);
                }
            }
        }

        private void CheckTimeToRevomeVideo(DateTime now)
        {
            foreach(var clientReservation in _reservation.GetAll())
            {
                foreach(var removeVideo in clientReservation.Reservations.Where(r => r.EndTime >= now))
                {
                    // 移除影片
                    CleanVideo(clientReservation.ClientId);
                    // 移除預約
                    _reservation.RemoveClientReservation(clientReservation.ClientId, removeVideo.Tick);
                }
            }
        }
    }
}