using Oracle.ManagedDataAccess.Client;
using AutoLedger.Models;

namespace AutoLedger.Services
{
    public class DbService
    {
        string connectionString =
            "User Id=system;Password=orcl21c08;Data Source=localhost:1521/XE";

        public void Insert(List<Transaction> transactions)
        {
            using var connection = new OracleConnection(connectionString);
            connection.Open();

            foreach (var t in transactions)
            {
                string sql = @"INSERT INTO SYSTEM.BANK_TRANSACTIONS
                               (TXN_DATE, DESCRIPTION, DEBIT, CREDIT, BALANCE, MODIFIEDTIME, USER_ID, VOUCHER_TYPE)
                               VALUES (:txnDate, :txnDesc, :debit, :credit, :balance, :modifiedtime, :userid, :trantype)";

                using var cmd = new OracleCommand(sql, connection);

                cmd.Parameters.Add(":txnDate", t.Date);
                cmd.Parameters.Add(":txnDesc", t.Description);
                cmd.Parameters.Add(":debit", t.Debit);
                cmd.Parameters.Add(":credit", t.Credit);
                cmd.Parameters.Add(":balance", t.Balance);
                cmd.Parameters.Add(":modifiedtime", t.ModifiedTime);
                cmd.Parameters.Add(":userid", t.UserId);
                cmd.Parameters.Add(":trantype", t.TranType);

                cmd.ExecuteNonQuery();
            }
        }
    }
}