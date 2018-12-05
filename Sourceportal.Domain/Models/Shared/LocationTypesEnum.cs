using System.ComponentModel;

namespace Sourceportal.Domain.Models.Shared
{
    public enum LocationTypesEnum
    {
        [Description("Bill-To")]
        BillTo = 1,
        [Description("Ship-To")]
        ShipTo = 2,
        Other = 4
    }
}