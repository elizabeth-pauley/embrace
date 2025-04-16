using System.ComponentModel;

namespace Embrace.Models
{
    public enum DiscussionType
    {
        [Description("Conversation")]
        Conversation,

        [Description("Meet-Up")]
        MeetUp,

        [Description("Question")]
        Question,

        [Description("Announcement")]
        Announcement,

        [Description("Event")]
        Event,

        [Description("Other")]

        Other
    }
}
