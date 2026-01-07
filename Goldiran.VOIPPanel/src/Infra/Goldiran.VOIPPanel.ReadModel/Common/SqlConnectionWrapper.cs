using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.QueryHandler.Common;

public class SqlConnectionWrapper
{
    public SqlConnection DefaultConnection { get; }
    public SqlConnection SecondaryConnection { get; }

    public SqlConnectionWrapper(SqlConnection defaultConnection, SqlConnection secondaryConnection)
    {
        DefaultConnection = defaultConnection;
        SecondaryConnection = secondaryConnection;
    }
}