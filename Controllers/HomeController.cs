using Caching.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Caching.Controllers
{
    public class HomeController : Controller
    {
        IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }
        public void SetCache()
        {
            _memoryCache.Set("employeeName", "Muiddin Impatrino");
        }

        public string GetCache()
        {
            var cachedData = _memoryCache.Get<string>("employeeName");

            return cachedData
                ?? "cache bulunamadı";
        }

        public string RemoveCache()
        {
            _memoryCache.Remove("employeeName");
            return "cache kaldırıldı";
        }
        public string TryGetCache()
        {
            if(_memoryCache.TryGetValue<string>("employeeName", out string data))
            {
                return data;
            }

            return data ?? "cache bulunamadı";
        }

        public void GetOrCreate()
        {
            string name = _memoryCache.GetOrCreate<string>("employeeName", entry =>
            {
                entry.SetValue("Serkanooo");
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);//5'er sn içinde istek geldiği sürece toplam 30 sn boyunca cache bilgisi okunabilir.
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(30);//30 sn'yeni sonunda  cache bilgisi silinir.
                return entry.Value.ToString();
            });
        }

        public void SetCreate()
        {
            //_memoryCache.Set<DateTime>("date", DateTime.Now, options:new() //c# >=9.0
            //{
            //    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
            //    SlidingExpiration = TimeSpan.FromSeconds(5)
            //});
        }


        public string CreateCache()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            options.Priority = CacheItemPriority.NeverRemove;
            _memoryCache.Set("employeeName", "Muiddin Impatrino", options);
            return "Cache oluşturuldu";
        }
    }
}
