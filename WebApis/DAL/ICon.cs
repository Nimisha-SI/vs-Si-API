using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApis.DAL
{
   public interface ICon
    {
        IConfiguration IGetConfiguration();
        string GetKeyValueAppSetting(string conString, string subCon);
        List<string> LstKeyValueAppSetting(string s1, string s2);
    }
}
