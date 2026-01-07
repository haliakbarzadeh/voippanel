using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Domain.Common.Models;

public class PermissionDefinitionResponse
{
    public static List<PermissionNodes> PermissionNodes;
    static PermissionDefinitionResponse()
    {
        if (PermissionNodes.IsNullOrEmpty())
        {
            GenerateClaimDefinition();
        }
    }

    private static void GenerateClaimDefinition()
    {
        PermissionNodes = new List<PermissionNodes>();
        //
        //
        PermissionNodes permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 1,
            Name = "Users",
            Description = "مدیریت کاربران",
            Order = 1,
            ParentNodeId=null
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 2,
            Name = "UsersList",
            Description = "کاربران",
            Order = 2,
            ParentNodeId = 1
        };
        PermissionNodes.Add(permissionNode);        
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 3,
            Name = "Users_create",
            Description = "ثبت",
            Order = 3,
            ParentNodeId = 2
        };
        PermissionNodes.Add(permissionNode);
       
       
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 4,
            Name = "Users_get",
            Description = "مشاهده",
            Order = 3,
            ParentNodeId = 2
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 5,
            Name = "Users_update",
            Description = "ویرایش",
            Order = 3,
            ParentNodeId = 2
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 6,
            Name = "Users_delete",
            Description = "حذف",
            Order = 3,
            ParentNodeId = 2
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 10,
            Name = "Position",
            Description = "سمت ها",
            Order = 2,
            ParentNodeId = 1
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 11,
            Name = "Position_create",
            Description = "ثبت",
            Order = 3,
            ParentNodeId = 10
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 12,
            Name = "Position_update",
            Description = "ویرایش",
            Order = 3,
            ParentNodeId = 10
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 13,
            Name = "Position_get",
            Description = "مشاهده",
            Order = 3,
            ParentNodeId = 10
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 14,
            Name = "Position_delete",
            Description = "حذف",
            Order = 3,
            ParentNodeId = 10
        };
        PermissionNodes.Add(permissionNode);
        //
       
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 20,
            Name = "Roles",
            Description = "نقش ها",
            Order = 2,
            ParentNodeId = 1
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 21,
            Name = "Roles_create",
            Description = "ثبت",
            Order = 3,
            ParentNodeId = 20
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 22,
            Name = "Roles_update",
            Description = "ویرایش",
            Order = 3,
            ParentNodeId = 20
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 23,
            Name = "Roles_get",
            Description = "مشاهده",
            Order = 3,
            ParentNodeId = 20
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 24,
            Name = "Roles_delete",
            Description = "حذف",
            Order = 3,
            ParentNodeId = 20
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 30,
            Name = "AccessManagement",
            Description = "سطح دسترسی ها",
            Order = 2,
            ParentNodeId = 1
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 31,
            Name = "AccessManagement_create",
            Description = "ثبت",
            Order = 3,
            ParentNodeId = 30
        };
        PermissionNodes.Add(permissionNode);
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 32,
            Name = "AccessManagement_get",
            Description = "مشاهده",
            Order = 3,
            ParentNodeId = 30
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 33,
            Name = "MonitoredPositions",
            Description = "سمت های ناظر",
            Order = 2,
            ParentNodeId = 1
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 40,
            Name = "Dashboard",
            Description = "داشبورد",
            Order = 1,
            ParentNodeId = null
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 50,
            Name = "Rasad",
            Description = "رصد",
            Order = 1,
            ParentNodeId = null
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 51,
            Name = "RasadUserStatus",
            Description = "وضعیت فعالیت کاربران",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 52,
            Name = "RRST",
            Description = "رصد صف تماس",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 53,
            Name = "RNK",
            Description = "نگاه کلی",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 54,
            Name = "RRB",
            Description = "رصد بازاریابی",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 55,
            Name = "RKSS",
            Description = "کارکرد ساعتی صف",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 56,
            Name = "RNKB",
            Description = "نگاه کلی بازاریابی",
            Order = 2,
            ParentNodeId = 50
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 70,
            Name = "Report",
            Description = "گزارشات",
            Order = 1,
            ParentNodeId = null
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 71,
            Name = "GGTT",
            Description = "گزارش تفصیلی تماس",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 72,
            Name = "GGVK",
            Description = "گزارش وضعیت کاربران'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 73,
            Name = "GGJT",
            Description = "گزارش جزئیات تماس'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 74,
            Name = "GGT",
            Description = "گزارش تلفن ها'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 75,
            Name = "GGTKH",
            Description = "گزارش تماس های خودکار'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 76,
            Name = "GGN",
            Description = "گزارش نظرسنجی'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 77,
            Name = "GGTA",
            Description = "گزارش تماس امن'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 78,
            Name = "GGJS",
            Description = "گزارش جزئیات صف'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 79,
            Name = "GGMP",
            Description = "گزارش ماشین پاسخگو'",
            Order = 2,
            ParentNodeId = 70
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 100,
            Name = "Settings",
            Description = "تنظیمات پشتیبانی'",
            Order = 1,
            ParentNodeId = null
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 101,
            Name = "TMHS",
            Description = "تنظیمات'",
            Order = 2,
            ParentNodeId = 100
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 102,
            Name = "TMS",
            Description = "مدیریت صف'",
            Order = 2,
            ParentNodeId = 100
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 103,
            Name = "TT",
            Description = "تنظیمات'",
            Order = 2,
            ParentNodeId = 100
        };
        PermissionNodes.Add(permissionNode);
        //
        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 110,
            Name = "CM",
            Description = "مدیریت تماس ها'",
            Order = 1,
            ParentNodeId = null
        };
        PermissionNodes.Add(permissionNode);

        permissionNode = new PermissionNodes()
        {
            Checked = false,
            NodeId = 111,
            Name = "MMMP",
            Description = "مدیریت تماس ها'",
            Order = 2,
            ParentNodeId = 110
        };
        PermissionNodes.Add(permissionNode);
    }
    


}

public class PermissionNodes
{
    public int NodeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Checked { get; set; }
    public int? ParentNodeId { get; set; }
    public int Order { get; set; } 
}
