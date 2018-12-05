using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class SetItemDetailsDb
    {
        public int ItemID;
        public string MPN;
        public int ManufacturerID;
        public int StatusID;
        public int GroupID;
        public string Description;
        public bool Eurohs;
        public bool Cnrohs;
        public string Eccn;
        public string Error;
    }
}
