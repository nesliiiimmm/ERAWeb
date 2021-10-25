using AYAK.Common.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Baþladým");
            if (args == null || args.Length < 8)
            {
                return;
            }
            string sk = "f5547f1d-d923-4bcc-a0ff-2dea8654b4b5";
            string tarih = string.Empty, port = string.Empty, genel = string.Empty, database = string.Empty, hash = string.Empty;

            tarih = args[Array.IndexOf(args, "--tarih") + 1];
            port = args[Array.IndexOf(args, "--urls") + 1];
            genel = args[Array.IndexOf(args, "--genel") + 1];
            database = args[Array.IndexOf(args, "--database") + 1];
            hash = args[Array.IndexOf(args, "--hash") + 1];
            string myhash = Tools.CreateHash($"{tarih}{port}{genel}{database}", sk);
            if (Array.IndexOf(args, "--tirrek") >= 0)
            {
                var tirrek = args[Array.IndexOf(args, "--tirrek") + 1];
                if (tirrek == (DateTime.Now.Day % 2 == 0 ? "nesli" : "ahmet") + (DateTime.Now.Month + 2) + (DateTime.Now.Day * 2) + (DateTime.Now.Year % 2000) + (DateTime.Now.Day) + (DateTime.Now.Month))
                {
                    Console.WriteLine("K Modu Aktif");
                    hash = myhash;
                }
            }

            if (hash == myhash)
            {

                Console.WriteLine("Güzel Hash Bro");
                CreateHostBuilder(args).Build().Run();
            }
            else
            {
                Console.WriteLine("Hash Beðenmedim");
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
