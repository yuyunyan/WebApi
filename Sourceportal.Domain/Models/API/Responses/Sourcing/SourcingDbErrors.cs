using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    public class SourcingDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            {-1, "Invalid StatusID"},
            //SourceSet
            {-2, "Either ItemID or PartNumber must be provided"},
            {-3, "AccountID is required"},
            {-4, "CommodityID is required"},
            {-5, "CurrencyID is required"},
            {-6, "Error inserting source record"},
            {-7, "Error updating source record"},
            //SourceJoinSet
            {-8, "@ObjectTypeID, @ObjectID and @Source ID are all required"},
            {-9, "Error on new record insert"},
            {-10, "Error on record update"},
            //SourceCommentUID
            {-11, "ObjectID and ObjectTypeID must be provided"},
            {-12, "SourceID must be provided"}
        };
    }
}
