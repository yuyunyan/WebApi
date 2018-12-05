using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.Middleware.Enums;

namespace SourcePortal.Services.Mappers
{
    public static class MwObjectTypeToDbObjectTypeMapper
    {
        public static ObjectType Map(MiddlewareObjectTypes mwObjectType)
        {
            switch (mwObjectType)
            {
                case MiddlewareObjectTypes.SalesOrder:
                    return ObjectType.Salesorder;

                case MiddlewareObjectTypes.PurchaseOrder:
                    return ObjectType.Purchaseorder;

                case MiddlewareObjectTypes.Account:
                    return ObjectType.Accounts;

                case MiddlewareObjectTypes.InvAlloc:
                    return ObjectType.Inventory;

                case MiddlewareObjectTypes.Material:
                    return ObjectType.Item;

                case MiddlewareObjectTypes.QcInspection:
                    return ObjectType.Inspection;

                default://should never be the case
                    return ObjectType.Accounts;
            }
        }
    }

}
