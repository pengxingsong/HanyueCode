using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 编码规则主表
    /// </summary>
    public class BaseCodeRuleBll : RepositoryFactory<Base_CodeRule>
    {
        #region 获取编码规则列表

        /// <summary>
        /// 获取编码规划列表
        /// </summary>
        /// <returns></returns>
        public List<Base_CodeRule> GetList()
        {
            return Repository().FindList();
        }

        #endregion

        #region 表单提交

        /// <summary>
        /// 【Excel模板设置】表单提交事件
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="Entity">导入模板实体</param>
        /// <param name="ExcelImportDetailJson">导入模板明细Json</param>
        /// <returns></returns>
        public int SubmitForm(string KeyValue, Base_CodeRule base_coderule, string CodeRuleDetailJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<Base_CodeRuleDetail> CodeRuleDetailList = CodeRuleDetailJson.JonsToList<Base_CodeRuleDetail>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    base_coderule.Modify(KeyValue);
                    database.Update(base_coderule, isOpenTrans);
                    database.Delete("Base_CodeRuleDetail", "CodeRuleId", base_coderule.CodeRuleId);
                        //将原有明细删除掉，后面新增进来，确保不会有重复明细值
                }
                else
                {

                    base_coderule.Create();
                    database.Insert(base_coderule, isOpenTrans);
                    //插入流水号种子
                    Base_CodeRuleSerious base_coderuleserious = new Base_CodeRuleSerious();
                    base_coderuleserious.Create();
                    base_coderuleserious.CodeRuleId = base_coderule.CodeRuleId;
                    base_coderuleserious.NowValue = 1;
                    database.Insert<Base_CodeRuleSerious>(base_coderuleserious, isOpenTrans);
                }
                int i = 1;
                foreach (Base_CodeRuleDetail base_coderuledetail in CodeRuleDetailList)
                {
                    //跳过空行
                    if (string.IsNullOrEmpty(base_coderuledetail.FormatStr))
                    {
                        continue;
                    }
                    base_coderuledetail.CodeRuleId = base_coderule.CodeRuleId;
                    base_coderuledetail.CodeRuleDetailId = CommonHelper.GetGuid;
                    base_coderuledetail.SortCode = i;
                    i++;
                    database.Insert(base_coderuledetail, isOpenTrans);
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }

        #endregion

        #region 单据编码处理

        /// <summary>
        /// 获得单前模块的单据编号如果没有定义规则就返回空
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="code">编码编号</param>
        /// <returns>单据号</returns>
        public string GetBillCode(string userId, string code)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            //获得模板ID
            string billCode = ""; //单据号
            Base_CodeRule baseCoderule = Repository().FindEntity("Code", code);
            try
            {
                int nowSerious = 0;
                //取得流水号种子
                List<Base_CodeRuleSerious> baseCoderuleseriouslist =
                    database.FindList<Base_CodeRuleSerious>("CodeRuleId", baseCoderule.CodeRuleId);
                //取得最大种子
                Base_CodeRuleSerious maxCodeRuleSerious =
                    baseCoderuleseriouslist.Find(
                        delegate(Base_CodeRuleSerious p) { return p.ValueType == "0" && p.UserId == null; });

                if (!string.IsNullOrEmpty(baseCoderule.CodeRuleId))
                {
                    List<Base_CodeRuleDetail> baseCoderuledetailList =
                        database.FindList<Base_CodeRuleDetail>("CodeRuleId", baseCoderule.CodeRuleId);
                    foreach (Base_CodeRuleDetail baseCoderuledetail in baseCoderuledetailList)
                    {
                        switch (baseCoderuledetail.FullName)
                        {
                            //自定义项
                            case "0":
                                billCode = billCode + baseCoderuledetail.FormatStr;
                                break;

                            //日期
                            case "1":
                                //日期字符串类型
                                billCode = billCode + DateTime.Now.ToString(baseCoderuledetail.FormatStr);
                                //处理自动更新流水号
                                if (baseCoderuledetail.AutoReset == 1)
                                {
                                    //判断是否有流水号
                                    if (maxCodeRuleSerious != null)
                                    {
                                        //当上次更新时间跟本次日期不一致时重置流水号种子
                                        if (maxCodeRuleSerious.LastUpdateDate !=
                                            DateTime.Now.ToString(baseCoderuledetail.FormatStr))
                                        {
                                            maxCodeRuleSerious.LastUpdateDate =
                                                DateTime.Now.ToString(baseCoderuledetail.FormatStr); //更新最后更新时间
                                            maxCodeRuleSerious.NowValue = 1; //重置种子
                                            database.Update<Base_CodeRuleSerious>(maxCodeRuleSerious, isOpenTrans);
                                            //重置种子以后删除掉之前用户占用了的种子。
                                            StringBuilder deleteSql =
                                                new StringBuilder(
                                                    string.Format(
                                                        "delete Base_CodeRuleSerious where CodeRuleId='{0} AND UserId IS NOT NULL '",
                                                        baseCoderule.CodeRuleId));
                                            database.ExecuteBySql(deleteSql, isOpenTrans);
                                        }
                                    }
                                }
                                break;
                            //流水号
                            case "2":
                                //查找当前用户是否已有之前未用掉的种子
                                Base_CodeRuleSerious baseCoderuleserious =
                                    baseCoderuleseriouslist.Find(
                                        delegate(Base_CodeRuleSerious p)
                                        {
                                            return p.UserId == userId && p.Enabled == 1;
                                        });
                                //如果没有就取当前最大的种子
                                if (baseCoderuleserious == null)
                                {
                                    //取得系统最大的种子
                                    int maxSerious = (int) maxCodeRuleSerious.NowValue;
                                    nowSerious = maxSerious;
                                    baseCoderuleserious = new Base_CodeRuleSerious();
                                    baseCoderuleserious.Create();
                                    baseCoderuleserious.NowValue = maxSerious;
                                    baseCoderuleserious.UserId = userId;
                                    baseCoderuleserious.ValueType = "1";
                                    baseCoderuleserious.Enabled = 1;
                                    baseCoderuleserious.CodeRuleId = baseCoderule.CodeRuleId;
                                    database.Insert<Base_CodeRuleSerious>(baseCoderuleserious, isOpenTrans);
                                    //处理种子更新
                                    maxCodeRuleSerious.NowValue += 1; //种子自增
                                    database.Update<Base_CodeRuleSerious>(maxCodeRuleSerious, isOpenTrans);
                                }
                                else
                                {
                                    nowSerious = (int) baseCoderuleserious.NowValue;
                                }
                                string seriousStr = new string('0', (int) (baseCoderuledetail.FLength)) +
                                                    nowSerious;
                                seriousStr =
                                    seriousStr.Substring(seriousStr.Length - (int) (baseCoderuledetail.FLength));
                                billCode = billCode + seriousStr;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Other, "-1",
                    string.Format("获取{0}单据编码时错误：", baseCoderule.FullName) + ex.Message);
                database.Rollback();
                return billCode;
            }
            database.Commit();
            return billCode;
        }

        /// <summary>
        /// 占用单据号
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="code">单据编码编号</param>
        /// <param name="isOpenTrans"></param>
        /// <returns>true/false</returns>
        public bool OccupyBillCode(string userId, string code, DbTransaction isOpenTrans = null)
        {
            Base_CodeRule baseCoderule = Repository().FindEntity("Code", code);
            try
            {
                IDatabase database = DataFactory.Database();
                if (baseCoderule != null)
                {
                    List<Base_CodeRuleSerious> baseCoderuleseriouslist =
                        database.FindList<Base_CodeRuleSerious>("CodeRuleId", baseCoderule.CodeRuleId);
                    //查找当前用户是否已有之前未用掉的种子
                    Base_CodeRuleSerious baseCoderuleserious =
                        baseCoderuleseriouslist.Find(
                            p => p.UserId == userId && p.Enabled == 1);
                    if (baseCoderuleserious != null)
                    {
                        database.Delete<Base_CodeRuleSerious>(baseCoderuleserious, isOpenTrans);
                    }
                }
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Other, "-1",
                    string.Format("占用{0}单据编码时错误：", baseCoderule.FullName) + ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}