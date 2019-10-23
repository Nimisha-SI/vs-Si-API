using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApis.DAL
{
    public class Connection : ICon
    {
        string con;
        public Connection() {
        }
        public IConfiguration IGetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public string GetKeyValueAppSetting(string key, string value)
        {
            var configuration = IGetConfiguration();
            con = configuration.GetSection(key).GetSection(value).Value;
            return con;
        }
        public List<string> LstKeyValueAppSetting(string s1, string s2)
        {
            var configuration = IGetConfiguration();
            List<string> playerdetails = configuration.GetSection(s2 + ":" + s1 + "_PlayerDetails").Get<List<string>>();
            return playerdetails;
        }
    }
}
