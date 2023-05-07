using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SMSApp.Models.DAL
{
    public class SeatResetDAL
    {
        static Database CurrentDataBase = null;

        public SeatResetDAL()
        {
            CurrentDataBase = new SqlDatabase(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
        }

        public void ResetSeatLogToday()
        {
            DbCommand mDbCommand = null;

            mDbCommand = CurrentDataBase.GetStoredProcCommand("spr_ResetSeatLogToday");

            CurrentDataBase.ExecuteDataSet(mDbCommand);
        }

        public string ResetSPLog(string vId, string vIsType)
        {
            DbCommand mDbCommand = null;
            string mId = string.Empty;

            mDbCommand = CurrentDataBase.GetStoredProcCommand("spr_ResetSPLog");

            CurrentDataBase.AddInParameter(mDbCommand, "@vId", DbType.String, vId);
            CurrentDataBase.AddInParameter(mDbCommand, "@vIsType", DbType.String, vIsType);

            mId = CurrentDataBase.ExecuteScalar(mDbCommand).ToString();

            return mId;
        }
    }
}

