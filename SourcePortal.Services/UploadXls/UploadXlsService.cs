using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.UploadXls;
using Sourceportal.Domain.Models.API.Responses.UploadXls;

namespace SourcePortal.Services.UploadXls
{
    public class UploadXlsService : IUploadXlsService
    {
        public readonly IUploadXlsRepository _uploadRepository;

        public UploadXlsService(IUploadXlsRepository UploadRepository)
        {
            _uploadRepository = UploadRepository;
        }

        public XlsDataMapsGetResponse XlsDataMapGet(string xlsType, int itemListTypeID)
        {
            var dataMaps = new List<XlsDataMapGetObject>();
            var dataMapsDb = _uploadRepository.XlsDataMapGet(xlsType, itemListTypeID );

            foreach (var xlsDataMapDb in dataMapsDb)
            {
                var xlsDataMapObject = new XlsDataMapGetObject
                {
                    XlsDataMapID = xlsDataMapDb.XlsDataMapID,
                    FieldLabel = xlsDataMapDb.FieldLabel,
                    IsRequired = xlsDataMapDb.IsRequired
                };
                dataMaps.Add(xlsDataMapObject);
            }
            return new XlsDataMapsGetResponse
            {
                XlsDataMaps = dataMaps
            };
        }

        public XlsAccountGetResponse XlsAccountGet(int accountId, string xlsType)
        {
            var xlsAccounts = new List<XlsAccountObject>();

            var xlsAccountDbs = _uploadRepository.XlsAccountGet(accountId, xlsType);

            foreach (var xlsAccountDb in xlsAccountDbs)
            {
                var xlsAccountObject = new XlsAccountObject
                {
                    XlsDataMapID = xlsAccountDb.XlsDataMapID,
                    ColumnIndex = xlsAccountDb.ColumnIndex
                };
                xlsAccounts.Add(xlsAccountObject);
            }

            return new XlsAccountGetResponse
            {
                XlsAccounts = xlsAccounts
            };
        }
    }
}
