using practise_20240426.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practise_20240426.service
{
    public class PraciseInputService
    {
        /// <summary>
        /// Postgresql連線
        /// </summary>
        //private string _PostgresConnect = ConfigHelper.GetSectionValue("Reporting:PostgresConnect"); //無報表不放
        string _OracleConnect = ConfigHelper.GetSectionValue("DBS:OracleConnect");

        /// <summary>
        /// 紀錄Log
        /// </summary>
        public Logger Logger { get; set; }

        /// <summary>
        /// 覆核模組
        /// </summary>
        public MkCk.MkCk MkCk { get; set; }

        #region 0300 ETT3003 委託及成交回報查詢
        public OutputEttOrderDealQueryModel EttDataQuery(InputEttOrderDealQueryModel inputettdata)
        {
            OutputEttOrderDealQueryModel result = new OutputEttOrderDealQueryModel();
            switch (inputettdata.action_type)
            {
                case ""://預設值
                    result = QueryOrderDefault(inputettdata);
                    break;
                case "3"://條件查詢
                    result = QueryOrderResult(inputettdata);
                    break;
                case "80"://雙擊
                    result = DoubleClickReoprt(inputettdata);
                    break;
            }
            return result;
        }
        #endregion
        //預設查詢
        private string QueryOrderDefault(InputLogPractice input)
        {
            OutputLogPracise DefaultResult = new OutputLogPracise() { ret_code = "000000", ret_msg = "成功" };
            using (var cn = new OracleConnection(_OracleConnect))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    DefaultResult.program_setup = MkCk.GetProgramSetup(input.program_code, input.login_account_id);
                    string default_sql = $@"select cust_no from odrded";
                    UtilityOracleDynamicParameters dynParams = new UtilityOracleDynamicParameters();
                    dynParams.Add("rst1", OracleDbType.RefCursor, ParameterDirection.Output);
                    var queryResult = cn.QueryMultiple(sql, dynParams);
                    DefaultQuery dorpdownlist = new DefaultQuery();
                    dorpdownlist.cust_no = queryResult.Read<String>.Tolist();
                    DefaultResult.query_output = dorpdownlist;
                }
                catch (Exception ex) //錯誤 固定
                {
                    Logger.Error(ex);
                    DefaultResult.ret_code = ErrorHandler.GetEnumErrorCode(ErrorHandler.ErrorCodes.ERR_SYSTEM_ERROR);
                    DefaultResult.ret_msg = ErrorHandler.GetEnumDescription(ErrorHandler.ErrorCodes.ERR_SYSTEM_ERROR);
                    return DefaultResult;
                }
                return DefaultResult;
            }
        }

        //條件查詢
        private string QueryOrderResult(InputLogPractice input)
        {
            OutputLogPracise FirstQuery = new OutputLogPracise() { ret_code = "000000", ret_msg = "成功" };
            using (var cn = new OracleConnection(_OracleConnect))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    FirstQuery.program_setup = MkCk.GetProgramSetup(input.program_code, input.login_account_id);
                    string default_sql = $@"select cust_no from odrded where cust_no='{input.query_input.customer_no}'";  //查詢條件
                    UtilityOracleDynamicParameters dynParams = new UtilityOracleDynamicParameters();
                    dynParams.Add("rst1", OracleDbType.RefCursor, ParameterDirection.Output);
                    var queryResult = cn.QueryMultiple(sql, dynParams);
                    queryResult.Read.List .List<string>


                    DefaultQuery dorpdownlist = new DefaultQuery();
                    dorpdownlist.cust_no = queryResult.Read<String>.Tolist();
                    FirstQuery.query_output = dorpdownlist;
                }
                catch (Exception ex) //錯誤 固定
                {
                    Logger.Error(ex);
                    FirstQuery.ret_code = ErrorHandler.GetEnumErrorCode(ErrorHandler.ErrorCodes.ERR_SYSTEM_ERROR);
                    FirstQuery.ret_msg = ErrorHandler.GetEnumDescription(ErrorHandler.ErrorCodes.ERR_SYSTEM_ERROR);
                    return FirstQuery;
                }
                return FirstQuery;
            }
        }
    }
}
