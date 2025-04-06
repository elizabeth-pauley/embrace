using System.ComponentModel;

namespace Embrace.Models
{
    public enum ResourceType
    {
        [Description("In-Person")]
        Local,

        [Description("Online")]
        General
    }
}
