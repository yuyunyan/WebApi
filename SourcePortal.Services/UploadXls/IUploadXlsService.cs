using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.UploadXls;


namespace SourcePortal.Services.UploadXls
{
    public interface IUploadXlsService
    {
        XlsDataMapsGetResponse XlsDataMapGet(string xlsType, int itemListTypeID);
        XlsAccountGetResponse XlsAccountGet(int accountId, string xlsType);
    }
}
