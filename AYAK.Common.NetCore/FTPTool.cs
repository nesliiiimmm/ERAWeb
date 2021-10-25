
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AYAK.Common.NetCore
{
    public class FTPTool
    {

        public static string User
        {
            get
            {
                return "Oda";
            }
        }
        public static string PWD
        {
            get
            {
                return "";
            }
        }
        public static List<string> List(string address)
        {
            List<string> names = new List<string>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(address);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(User, PWD);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 3)
                {
                    string name = "";
                    for (int i = 8; i < tokens.Length; i++)
                    {
                        name += tokens[i] + " ";
                    }
                    names.Add(name);
                }

            }



            reader.Close();
            response.Close();
            return names;
        }

        public static FTPDir DirTree(string root,string path)
        {
            List<string> lines = new List<string>();


            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(root+"/"+path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(User, PWD);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                lines.Add(line);
            }
            
            reader.Close();
            response.Close();

            FTPDir dir = new FTPDir();
            dir.Path = path;

            foreach (string line in lines)
            {
                var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 3)
                {

                    string auth = tokens[0];
                    string name = tokens[tokens.Length-1];
                    

                    if (auth == "drwxr-xr-x")
                    {
                        FTPDir subdir = FTPTool.DirTree(root ,path+"/"+name);
                        subdir.Name = name;
                        subdir.Path = path + "/" + name;
                        dir.SubDirs.Add(subdir);
                    }
                    else
                    {
                        FTPFile file = new FTPFile();
                        file.Name = name;
                        file.Path = dir.Path+"/"+name;
                        dir.Files.Add(file);
                    }
                }

            }
            return dir;

        }

        public static void DownloadFile(string sourceURL, string targetPath)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Credentials = new NetworkCredential(User, PWD);

                try
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {

                    };

                    wc.DownloadFile(sourceURL, targetPath);


                    wc.DownloadFileCompleted += (s, e) =>
                    {

                    };
                }
                catch (Exception e)
                {
                    
                }
            }
        }
        
    }




    public class FTPDir
    {
        public FTPDir()
        {
            Files = new List<FTPFile>();
            SubDirs = new List<FTPDir>();
        }
        public List<FTPFile> Files { get; set; }
        public List<FTPDir> SubDirs { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class FTPFile
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
