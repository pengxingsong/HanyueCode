using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// �����ֵ��
    /// </summary>
    public class BaseDataDictionaryBll : RepositoryFactory<Base_DataDictionary>
    {
        /// <summary>
        /// ��ȡ�����ֵ���ϸ�б�
        /// </summary>
        /// <param name="dataDictionaryId">���� ����ֵ</param>
        /// <returns></returns>
        public List<Base_DataDictionaryDetail> GetDataDictionaryDetailList(string dataDictionaryId)
        {
            if (!string.IsNullOrEmpty(dataDictionaryId))
            {
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" AND DataDictionaryId = @DataDictionaryId Order By SortCode ASC");
                List<DbParameter> parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@DataDictionaryId", dataDictionaryId)
                };
                return DataFactory.Database().FindList<Base_DataDictionaryDetail>(whereSql.ToString(), parameter.ToArray());
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// ��ȡ�����ֵ���ϸ�б�
        /// </summary>
        /// <param name="code">�������</param>
        /// <returns></returns>
        public List<Base_DataDictionaryDetail> GetDataDictionaryDetailListByCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var whereSql = new StringBuilder();
                whereSql.Append(" AND DataDictionaryId IN(SELECT DataDictionaryId FROM Base_DataDictionary WHERE Code=@Code)");
                whereSql.Append(" ORDER BY SortCode ASC");
                var parameter = new List<DbParameter> {DbFactory.CreateDbParameter("@Code", code)};
                return DataFactory.Database().FindList<Base_DataDictionaryDetail>(whereSql.ToString(), parameter.ToArray());
            }
            return null;
        }
    }
}