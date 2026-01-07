using System.ComponentModel;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
public enum HourType
{
    [Description("00:00_01:00")]
    One = 1,
    [Description("01:00_02:00")]
    Two = 2,
    [Description("02:00_03:00")]
    Three = 3,
    [Description("03:00_04:00")]
    Four = 4,
    [Description("04:00_05:00")]
    Five = 5,
    [Description("05:00_06:00")]
    Six = 6,
    [Description("06:00_07:00")]
    Seven = 7,
    [Description("07:00_08:00")]
    Eight = 8,
    [Description("08:00_09:00")]
    Nine = 9,
    [Description("09:00_10:00")]
    Ten = 10,
    [Description("10:00_11:00")]
    Eleven = 11,
    [Description("11:00_12:00")]
    Twoelve = 12,
    [Description("12:00_13:00")]
    Thirteen = 13,
    [Description("13:00_14:00")]
    Fourteen = 14,
    [Description("14:00_15:00")]
    Fifteen = 15,
    [Description("15:00_16:00")]
    Sixteen = 16,
    [Description("16:00_17:00")]
    Seventeen = 17,
    [Description("17:00_18:00")]
    Eighteen = 18,
    [Description("18:00_19:00")]
    Nineteen = 19,
    [Description("19:00_20:00")]
    Twoenty = 20,
    [Description("20:00_21:00")]
    Twoentyone = 21,
    [Description("21:00_22:00")]
    Twoentytwo = 22,
    [Description("22:00_23:00")]
    Twoentythree = 23,
    [Description("23:00_24:00")]
    Twoentyfour = 24
}
