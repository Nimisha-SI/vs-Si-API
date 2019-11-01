using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApis.BOL
{
    public interface IAddUpdateIndex
    {
        bool AddUpdateElasticIndex(dynamic sampleDatas, int sportid = 1, bool isfull = true, bool isS2 = false);
        //void Add_UpdateCricketS1Data(dynamic sampleDatas);
    }
}
