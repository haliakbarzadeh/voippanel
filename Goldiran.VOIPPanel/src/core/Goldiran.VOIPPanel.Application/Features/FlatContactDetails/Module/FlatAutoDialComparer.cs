using Goldiran.VOIPPanel.ReadModel.Dto;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Module;

public class FlatAutoDialComparer : IEqualityComparer<ContactDetailDto>
{
    public bool Equals(ContactDetailDto x, ContactDetailDto y) => x.LinkedId == y.LinkedId && x.Source == y.Source && x.Dest == y.Dest;
    public int GetHashCode(ContactDetailDto obj) => obj.LinkedId.GetHashCode();
}

