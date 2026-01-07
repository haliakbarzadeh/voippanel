using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;

public class Operation:AggregateRoot<long>
{
    public long UserId {  get; private set; }
    public long PositionId { get; private set; }
    public OperationType OperationTypeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public DateTime? EndDate { get; private set; }
    public TimeSpan? EndTime { get; private set; }
    public bool IsCurrentStatus { get; private set; }
    public string Queues { get; private set; }
    public int Penalty { get; private set; }
    public int StatusDuration { get; private set; }
    public long? ManagerUserId { get; private set; }
    public string? Reason { get; private set; }
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    public Operation(long userId,long positionId, OperationType operationTypeId, string queues)
    {
        var date = DateTime.Now;
        UserId = userId;
        PositionId= positionId;
        OperationTypeId = operationTypeId;
        StartDate = date.Date;
        StartTime = date.TimeOfDay;
        EndDate = null;
        EndTime = null;
        IsCurrentStatus = true;
        Queues = queues;
        StatusDuration = 0;
    }

    public Operation(long userId, long positionId, OperationType operationTypeId, string queues,long managerUserId,string reason)
    {
        var date = DateTime.Now;
        UserId = userId;
        PositionId= positionId;
        OperationTypeId = operationTypeId;
        StartDate = date.Date;
        StartTime = date.TimeOfDay;
        EndDate = null;
        EndTime = null;
        IsCurrentStatus = true;
        Queues = queues;
        StatusDuration = 0;
        ManagerUserId=managerUserId;
        Reason = reason;
    }

    public void SetFinishOperation()
    {
        var date = DateTime.Now;
        EndDate = date.Date;
        EndTime = date.TimeOfDay;
        IsCurrentStatus = false;
        StatusDuration =(int) (((TimeSpan)(EndTime - StartTime))+((DateTime)EndDate-StartDate)).TotalMinutes;


    }

    public void SetPenalty(int penalty)
    {
        Penalty=penalty;
    }
}
