using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Contracts;

public interface IAnsweringMachineRepository
{
    public Task<bool> Update(AnsweringMachineRequest request);
}
