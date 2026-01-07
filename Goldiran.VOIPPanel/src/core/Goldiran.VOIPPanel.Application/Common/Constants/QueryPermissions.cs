using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Constants;

public class QueryPermissions
{
    public static readonly Dictionary<Type, bool> HasVerticalPrivilege = new Dictionary<Type, bool>();
    static QueryPermissions()
    {
        SetHasVerticalPrivilege();
    }
    static void SetHasVerticalPrivilege()
    {
        HasVerticalPrivilege.Add(typeof(GetGroupOperationsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetPositionsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetUserPositionsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetOperationUsersQuery), true);
        HasVerticalPrivilege.Add(typeof(GetOperationsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetContactDetailsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetPositionQueusQuery), true);
        HasVerticalPrivilege.Add(typeof(GetAskCustomersQuery), true);
        HasVerticalPrivilege.Add(typeof(GetMasterAskCustomersQuery), true);
        HasVerticalPrivilege.Add(typeof(GetMasterWallboardQuery), true);
        HasVerticalPrivilege.Add(typeof(GetWallboardsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetQueueLogsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetAutoDetailsQuery), true);
        HasVerticalPrivilege.Add(typeof(GetSoftPhoneEventsQuery), true);
        //
        //
    }
}
