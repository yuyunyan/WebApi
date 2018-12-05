using System.ComponentModel;

namespace Sourceportal.Domain.Models.Shared
{
    public enum ContactMethodEnum
    {
        [Description("Office Phone")]
        OfficePhone = 1,
        [Description("Mobile Phone")]
        MobilePhone = 2,
        Fax = 3,
        Email = 4

    }
}
