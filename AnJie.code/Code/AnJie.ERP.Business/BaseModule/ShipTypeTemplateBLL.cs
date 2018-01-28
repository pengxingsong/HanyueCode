using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System;
using AnJie.ERP.ViewModel.BaseModule;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 面单模板表
    /// </summary>
    public class ShipTypeTemplateBLL : RepositoryFactory<ShipTypeTemplateEntity>
    {
        /// <summary>
        /// 根据物流方式获取模板列表
        /// </summary>
        /// <param name="shipTypeId">物流方式ID</param>
        /// <param name="onlyValid"></param>
        /// <returns></returns>
        public List<ShipTypeTemplateViewModel> GetList(string shipTypeId, bool onlyValid = false)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT T.*, S.ShipTypeName
                            FROM ShipType_Template T
                            INNER JOIN ShipType S ON S.ShipTypeId = T.ShipTypeId 
                            Where 1 = 1");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(shipTypeId))
            {
                strSql.Append(" AND T.ShipTypeId = @ShipTypeId");
                parameter.Add(DbFactory.CreateDbParameter("@ShipTypeId", shipTypeId));
            }
            if (onlyValid)
            {
                strSql.Append(" AND T.Enabled = 1");
            }
            strSql.Append(" ORDER BY T.SortCode ASC");
            return new Repository<ShipTypeTemplateViewModel>().FindListBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="fileName"></param>
        public void UpdateTemplateImage(string templateId, string fileName)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.ShipType_Template SET BackgroundImage = @BackgroundImage WHERE TemplateId = @TemplateId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@TemplateId", templateId),
                DbFactory.CreateDbParameter("@BackgroundImage", fileName)
            };
            Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="templateContent"></param>
        public void UpdateTemplateContent(string templateId, string templateContent)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.ShipType_Template SET TemplateContent = @TemplateContent WHERE TemplateId = @TemplateId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@TemplateId", templateId),
                DbFactory.CreateDbParameter("@TemplateContent", templateContent)
            };
            Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ShipTypeTemplateEntity GetTemplate(string templateId)
        {
            return Repository().FindEntity("TemplateId", templateId);
        }
    }
}