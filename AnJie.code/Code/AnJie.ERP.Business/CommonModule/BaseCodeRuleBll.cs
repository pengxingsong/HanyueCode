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
    /// �����������
    /// </summary>
    public class BaseCodeRuleBll : RepositoryFactory<Base_CodeRule>
    {
        #region ��ȡ��������б�

        /// <summary>
        /// ��ȡ����滮�б�
        /// </summary>
        /// <returns></returns>
        public List<Base_CodeRule> GetList()
        {
            return Repository().FindList();
        }

        #endregion

        #region ���ύ

        /// <summary>
        /// ��Excelģ�����á����ύ�¼�
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="Entity">����ģ��ʵ��</param>
        /// <param name="ExcelImportDetailJson">����ģ����ϸJson</param>
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
                        //��ԭ����ϸɾ��������������������ȷ���������ظ���ϸֵ
                }
                else
                {

                    base_coderule.Create();
                    database.Insert(base_coderule, isOpenTrans);
                    //������ˮ������
                    Base_CodeRuleSerious base_coderuleserious = new Base_CodeRuleSerious();
                    base_coderuleserious.Create();
                    base_coderuleserious.CodeRuleId = base_coderule.CodeRuleId;
                    base_coderuleserious.NowValue = 1;
                    database.Insert<Base_CodeRuleSerious>(base_coderuleserious, isOpenTrans);
                }
                int i = 1;
                foreach (Base_CodeRuleDetail base_coderuledetail in CodeRuleDetailList)
                {
                    //��������
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

        #region ���ݱ��봦��

        /// <summary>
        /// ��õ�ǰģ��ĵ��ݱ�����û�ж������ͷ��ؿ�
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="code">������</param>
        /// <returns>���ݺ�</returns>
        public string GetBillCode(string userId, string code)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            //���ģ��ID
            string billCode = ""; //���ݺ�
            Base_CodeRule baseCoderule = Repository().FindEntity("Code", code);
            try
            {
                int nowSerious = 0;
                //ȡ����ˮ������
                List<Base_CodeRuleSerious> baseCoderuleseriouslist =
                    database.FindList<Base_CodeRuleSerious>("CodeRuleId", baseCoderule.CodeRuleId);
                //ȡ���������
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
                            //�Զ�����
                            case "0":
                                billCode = billCode + baseCoderuledetail.FormatStr;
                                break;

                            //����
                            case "1":
                                //�����ַ�������
                                billCode = billCode + DateTime.Now.ToString(baseCoderuledetail.FormatStr);
                                //�����Զ�������ˮ��
                                if (baseCoderuledetail.AutoReset == 1)
                                {
                                    //�ж��Ƿ�����ˮ��
                                    if (maxCodeRuleSerious != null)
                                    {
                                        //���ϴθ���ʱ����������ڲ�һ��ʱ������ˮ������
                                        if (maxCodeRuleSerious.LastUpdateDate !=
                                            DateTime.Now.ToString(baseCoderuledetail.FormatStr))
                                        {
                                            maxCodeRuleSerious.LastUpdateDate =
                                                DateTime.Now.ToString(baseCoderuledetail.FormatStr); //����������ʱ��
                                            maxCodeRuleSerious.NowValue = 1; //��������
                                            database.Update<Base_CodeRuleSerious>(maxCodeRuleSerious, isOpenTrans);
                                            //���������Ժ�ɾ����֮ǰ�û�ռ���˵����ӡ�
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
                            //��ˮ��
                            case "2":
                                //���ҵ�ǰ�û��Ƿ�����֮ǰδ�õ�������
                                Base_CodeRuleSerious baseCoderuleserious =
                                    baseCoderuleseriouslist.Find(
                                        delegate(Base_CodeRuleSerious p)
                                        {
                                            return p.UserId == userId && p.Enabled == 1;
                                        });
                                //���û�о�ȡ��ǰ��������
                                if (baseCoderuleserious == null)
                                {
                                    //ȡ��ϵͳ��������
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
                                    //�������Ӹ���
                                    maxCodeRuleSerious.NowValue += 1; //��������
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
                    string.Format("��ȡ{0}���ݱ���ʱ����", baseCoderule.FullName) + ex.Message);
                database.Rollback();
                return billCode;
            }
            database.Commit();
            return billCode;
        }

        /// <summary>
        /// ռ�õ��ݺ�
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="code">���ݱ�����</param>
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
                    //���ҵ�ǰ�û��Ƿ�����֮ǰδ�õ�������
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
                    string.Format("ռ��{0}���ݱ���ʱ����", baseCoderule.FullName) + ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}