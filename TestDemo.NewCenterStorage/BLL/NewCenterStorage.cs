using TestDemo.NewCenterStorage.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.NewCenterStorage.BLL
{
    public class NewCenterStorage:ICenterStorage
    {
        public static IOTMP.Common.NewCenterStoreClient.FileStoreHelper Instance { get; set; } = new IOTMP.Common.NewCenterStoreClient.FileStoreHelper();
        static NewCenterStorage()
        {
            Instance = new IOTMP.Common.NewCenterStoreClient.FileStoreHelper();
            Instance.Init("192.168.14.175", 80, 9080);


        }
    }

}
