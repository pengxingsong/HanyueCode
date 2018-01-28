using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// �̻����̱�
    /// </summary>
    public class MerchantMallBLL : RepositoryFactory<MerchantMallEntity>
    {
        /// <summary>
        /// ��ȡ �̻������� �б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    MerchantId,				    --�̻�ID
												MerchantId AS MallId ,  --����ID
                                                Code ,					    --����
                                                FullName AS MallName ,  --����
                                                '' AS ParentId ,	        --�ڵ�ID
                                                SortCode,				    --�������
                                                'Merchant' AS Sort		    --����
                                      FROM      Merchant		    	    --�̻���
                                      UNION
                                      SELECT    MerchantId,				    --�̻�ID
												MallId,			        --����ID
                                                Code ,					    --����
                                                MallName ,			    --����
                                                MerchantId AS ParentId ,    --�ڵ�ID
                                                SortCode,				    --�������
                                                'Mall' AS Sort	        --����
                                      FROM      Merchant_Mall		    --���̱�
                                    ) T WHERE 1=1 ");
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// �����̻�id��ȡ�����б�
        /// </summary>
        /// <param name="merchantId">�̻�ID</param>
        /// <returns></returns>
        public DataTable GetList(string merchantId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    c.MallId ,			    --����
                                                m.FullName AS MerchantName ,--�����̻�
                                                c.MerchantId ,				--�����̻�Id
                                                c.Code ,					--����
                                                c.MallName ,    		--��������                                              
                                                c.Enabled ,					--��Ч
                                                c.SortCode,                 --������
                                                c.Remark					--˵��
                                      FROM      Merchant_Mall c
                                                INNER JOIN Merchant m ON c.MerchantId = m.MerchantId
                                    ) T WHERE 1=1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND MerchantId = @MerchantId");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY MerchantId ASC, SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }


        public DataTable GetListAll()
        {
            return Repository().FindTableByProc("Proc_S_MerchantMall");
        }


    }
}