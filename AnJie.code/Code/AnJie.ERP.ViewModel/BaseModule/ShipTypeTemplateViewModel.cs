using System;
using System.ComponentModel;

namespace AnJie.ERP.ViewModel.BaseModule
{
    /// <summary>
    /// �浥ģ���
    /// </summary>
    public class ShipTypeTemplateViewModel
    {
        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        public string TemplateId { get; set; }

        /// <summary>
        /// ����������ʽ
        /// </summary>
        /// <returns></returns>
        public string ShipTypeId { get; set; }

        /// <summary>
        /// ����������ʽ
        /// </summary>
        public string ShipTypeName { get; set; }

        /// <summary>
        /// �浥����
        /// </summary>
        /// <returns></returns>
        public string TemplateName { get; set; }

        /// <summary>
        /// �浥�ļ�
        /// </summary>
        /// <returns></returns>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// �Ƿ��ǵ����浥
        /// </summary>
        /// <returns></returns>
        public int IsElectronicBill { get; set; }

        /// <summary>
        /// ���(mm)
        /// </summary>
        /// <returns></returns>
        public int Width { get; set; }

        /// <summary>
        /// �߶�(mm)
        /// </summary>
        /// <returns></returns>
        public int Height { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        /// <returns></returns>
        public string TemplateContent { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        /// <summary>
        /// ��Ч
        /// </summary>
        /// <returns></returns>
        public int Enabled { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public int SortCode { get; set; }

        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int DeleteMark { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }

        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
    }
}