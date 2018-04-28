using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using Repository.Interface;
using Repository.Repository;
using Utility;
using VideoService.Interface;

namespace VideoService.Service
{
    public class VideoService : IVideoService
    {
        private IEnumerable<Video> _allVideos;

        private IEnumerable<Category> _categories;
        private ReservationRepository _reservation;

        public VideoService(IRepository<Reservation> reservation)
        {
            _reservation = reservation as ReservationRepository;
            CheckTime(1000);
        }

        private string _rootPath;

        public void Init(string rootPath,
            IEnumerable<Category> categories,
            IEnumerable<ClientIdentity> clientIdentities,
            IEnumerable<Reservation> reservation)
        {
            _rootPath = rootPath;
            _categories = categories;
            foreach(var identity in clientIdentities)
            {
                VideoUtility.AddClientIdentity(identity);
            }

            InitDirectories();
            InitVideos();
        }

        public IEnumerable<Video> SearchVideos(
            IEnumerable<int> categoryIds, DateTime? beginTime,
            DateTime? endTime, string rootPath)
        {
            InitVideos();

            var query = _allVideos;

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

        public string SetVideos(IEnumerable<string> codes,
            string ip, string rootPath, DateTime startTime, DateTime endTime)
        {
            //_reservation
            if(startTime > DateTime.Now) { 
                
                _reservation.AddClientReservation(ip,startTime,endTime,codes);
                return VideoUtility.GetConnectionIdByIp(ip);
            }
            var videos = new List<Video>();

            foreach(var code in codes)
            {
                var video = _allVideos
                    .Where(w => w.Code == code);

                if(video.Any())
                    videos.Add(video.First());
            }

            VideoUtility.UpdateVideo(ip, videos);
            VideoUtility.UpdateActiveStatus(ip, true);
            return VideoUtility.GetConnectionIdByIp(ip);
        }

        public string CleanVideo(string ip)
        {
            VideoUtility.UpdateVideo(ip, new List<Video>());
            VideoUtility.UpdateActiveStatus(ip, false);
            return VideoUtility.GetConnectionIdByIp(ip);
        }

        public void AddConnetionId(string ip, string connectionId)
        {
            VideoUtility.AddConnetionId(ip, connectionId);
        }

        public void UpdateOnlineStatus(string ip, bool isOnline)
        {
            VideoUtility.UpdateOnlineStatus(ip, isOnline);
        }

        public void RemoveConnetionId(string ip)
        {
            VideoUtility.RemoveConnetionId(ip);
        }

        public IEnumerable<Video> GetVideoListByIp(string ip)
        {
            return VideoUtility.GetClientVideo(ip);
        }

        public string GetVideoByCode(string code, string rootPath, string ip)
        {
            return VideoUtility.GetClientVideo(ip)
                .FirstOrDefault(v => v.Code == code) ?
                .Url
                .Replace(rootPath, "~/Videos");
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public IEnumerable<ClientIdentity> GetClientIdentities()
        {
            return VideoUtility.GetAllClientInfo();
        }

        private void InitVideos()
        {
            _allVideos = _rootPath
                .GetChildPaths()
                .Select(PathInfoToVideoInfo);
        }

        private Video PathInfoToVideoInfo(PathInfo pathInfo)
        {
            var category = pathInfo.Hierarchy[0];
            var date = pathInfo.Hierarchy[1];

            var cate = _categories
                .FirstOrDefault(c => c.Name == category);

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

        private void InitDirectories()
        {
            foreach(var category in _categories)
                _rootPath.InitDirectory(category.Name)
                .InitDirectory(DateTime.Now.ToString("yyyyMMdd"));
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
                    SetVideos(setVideo.Codes, clientReservation.ClientId, null, setVideo.StartTime, setVideo.EndTime);
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