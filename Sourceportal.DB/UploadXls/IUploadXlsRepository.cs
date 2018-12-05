using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.DB.UploadXls;

namespace Sourceportal.DB.UploadXls
{
    public interface IUploadXlsRepository
    {
        List<XlsDataMapDb> XlsDataMapGet(string xlsType, int itemListTypeID);
        List<XLSDataMapObject> XlsAccountGet(int accountId, string xlsType);
    }
}
