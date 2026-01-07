using System.ComponentModel.DataAnnotations;

namespace Goldiran.VOIPPanel.Application.Common.Enums;
/// <summary>
/// بر اساس اطلاعات
/// </summary>
public enum FormNameEnum
{
    [Display(Description = "گزارش فعالیت کاربرن'")]
    UserOperations = 1,
    [Display(Description = "گزارش جزئیات تماس'")]
    ContactDetails = 2,
    [Display(Description = "گزارش تماس خودکار'")]
    AutoDial = 3,
    [Display(Description = "گزارش نظر سنجی'")]
    AskCustomer = 4,
    [Display(Description = "گزارش تماس امن'")]
    SafeCall = 5,
    [Display(Description = "گزارش جزئیات صف'")]
    QueueLog = 6,
    [Display(Description = "گزارش تماس خودکار جدید'")]
    NewAutoDial = 7,
    [Display(Description = "گزارش ماشین پاسخگو'")]
    AnsweringMachine = 8,
    [Display(Description = "گزارش مدیریت ماشین پاسخگو'")]
    AnsweringMachineManagement = 9,
    [Display(Description = "گزارش تلفن ها")]
    SoftPhoneEvents = 10,
}