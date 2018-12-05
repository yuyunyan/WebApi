using Sourceportal.DB.Carrier;
using Sourceportal.Domain.Models.API.Requests.Carrier;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Carrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sourceportal.Domain.Models.API.Responses.Carrier.CarrierListRespose;

namespace SourcePortal.Services.Carrier
{
    public class CarrierService:ICarrierService
    {
        public readonly ICarrierRepository _carrierRepository;
        public CarrierService(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }

        public AccountCarrierListResponse GetAccountCarrierList(int accountId)
        {
            var dbAccountCarrierList = _carrierRepository.GetAccountCarriers(accountId);
            var carriers = new List<AccountCarrierResponse>();
            var response = new AccountCarrierListResponse();
            foreach (var dbCarrier in dbAccountCarrierList)
            {
                var carrier = new AccountCarrierResponse();
                carrier.AccountID = dbCarrier.AccountID;
                carrier.CarrierID = dbCarrier.CarrierID;
                carrier.CarrierName = dbCarrier.CarrierName;
                carrier.IsDefault = dbCarrier.IsDefault;
                carrier.AccountNumber = dbCarrier.AccountNumber;

                carriers.Add(carrier);             
            }
            response.AccountCarriers = carriers;
            return response; 
        }

        public BaseResponse DeleteAccountCarrier(DeleteCarrierRequest deleteCarrierRequest)
        {
            return new BaseResponse
            {
                IsSuccess = _carrierRepository.DeleteAccountCarrier(deleteCarrierRequest)
            };
        }
        public CarriersListResponse GetCarrierList()
        {
            var dbCarrier = _carrierRepository.GetCarrier();
            var carriers = new List<CarrierResponse>();
            var carrierOther = new CarrierResponse();
            var response = new CarriersListResponse();
            foreach (var value in dbCarrier)
            {
                var carrier = new CarrierResponse();
                carrier.CarrierID = value.CarrierID;
                carrier.CarrierName = value.CarrierName;
                carriers.Add(carrier);
            }

            carrierOther.CarrierID = 111;
            carrierOther.CarrierName = "Other- See Shipping Notes";

            carriers.Add(carrierOther);
            response.Carriers = carriers;
            return response;
        }

        public BaseResponse AccountCarrierSet(AccountCarrierInsertUpdateRequest accountCarrierSet)
        {
            var dbCarrierSet = _carrierRepository.CarrierSet( accountCarrierSet);
            return new BaseResponse
            {
                IsSuccess = dbCarrierSet == 1
            };
        }

        public AccountCarrierResponse GetAccountNumber(int accountId, int carrierId)
        {
            var response = new AccountCarrierResponse();
            var dbAccountNumber = _carrierRepository.GetAccountNumber(accountId, carrierId);
            if (dbAccountNumber != null)
            {
                response.AccountNumber = dbAccountNumber.AccountNumber;
                response.AccountID = dbAccountNumber.AccountID;
                response.CarrierID = dbAccountNumber.CarrierID;
            }          
            return response;
        }
    }

}
