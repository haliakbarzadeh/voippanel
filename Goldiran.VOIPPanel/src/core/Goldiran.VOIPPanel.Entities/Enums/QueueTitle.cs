using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using System.ComponentModel;

namespace Goldiran.VOIPPanel.ReadModel.Enums;

public enum QueueTitle
{
    [Description("Test")]
    Test = 5,
    [Description("Install")]
    Install = 11,
    [Description("Repair")]
    Repair = 12,
    [Description("Gift")]
    Gift = 16,
    [Description("AutoDial")]
    AutoDial = 20,
    [Description("MarketingAD")]
    MarketingAD = 22,
    [Description("marketingTamdid")]
    marketingTamdid = 23,
    [Description("ExpSalesCouncil")]
    ExpSalesCouncil = 24,
    [Description("tamdidAD")]
    tamdidAD = 25,
    [Description("ExpGplus1")]
    ExpGplus1 = 26,
    [Description("ExpGplus2")]
    ExpGplus2 = 27,
    [Description("CashSell")]
    CashSell = 29,
    [Description("marketingService")]
    marketingService = 30,
    [Description("HC-users")]
    HC_Users = 35,
    [Description("itTest")]
    itTest = 37,
    [Description("itTest2")]
    itTest2 = 38,
    [Description("secureCall-users")]
    secureCall = 40,
    [Description("CreditSell")]
    CreditSell = 48,
    [Description("AIC")]
    AIC = 31,
    [Description("CSDiald")]
    CSDiald = 50,
    [Description("Divar")]
    Divar = 51
}

