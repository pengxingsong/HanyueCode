using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.ViewModel.OrderModule;

namespace AnJie.ERP.Service
{
    public class HandoverService
    {
        private readonly HandoverBLL _handoverBll = new HandoverBLL();

        private readonly SaleOrderBLL _saleOrderBll = new SaleOrderBLL();

        private readonly HandoverItemBLL _handoverItemBll = new HandoverItemBLL();

        private readonly PickItemBLL _pickItemBll = new PickItemBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <param name="expressNum"></param>
        /// <param name="createUserId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ScanExpressNum(string shipTypeId, string expressNum, string createUserId, out string message)
        {
            var handover = _handoverBll.GetUnPrintHandoverByShipType(shipTypeId, createUserId);
            if (handover == null)
            {
                handover = new HandoverEntity();
                handover.Create();
                handover.HandoverNo = "";
                handover.IsPrinted = 0;
                handover.ShipTypeId = shipTypeId;
                _handoverBll.Repository().Insert(handover);
            }

            SaleOrderEntity orderEntity = _saleOrderBll.GetSaleOrderByExpressNum(expressNum);
            if (orderEntity == null)
            {
                message = string.Format("无效的物流单号[{0}]", expressNum);
                return false;
            }


            if (orderEntity.Status != (int) OrderStatus.OutStock && orderEntity.Status != (int)OrderStatus.Handover)
            {
                message = string.Format("订单[{0}]不是已出库或已交接状态，不能交接扫描", orderEntity.OrderNo);
                return false;
            }

            if (orderEntity.ShipTypeId != shipTypeId)
            {
                message = string.Format("物流单号[{0}]与所选物流方式不一致，不能扫描", orderEntity.ExpressNum);
                return false;
            }

            if (orderEntity.IsSuspended)
            {
                message = string.Format("订单[{0}]已被挂起，不能与配送商交接", orderEntity.OrderNo);
                return false;
            }


            HandoverItemEntity itemEntity = _handoverBll.GetHandOverItem(expressNum);

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                var picks = _pickItemBll.GetPickItemListByOrderNo(orderEntity.OrderNo);
                message = string.Empty;
                if (itemEntity == null)
                {
                    HandoverItemEntity handoverItemEntity = new HandoverItemEntity();
                    handoverItemEntity.Create();
                    handoverItemEntity.HandoverId = handover.HandoverId;
                    handoverItemEntity.ExpressNum = expressNum;
                    handoverItemEntity.OrderNo = orderEntity.OrderNo;
                    handoverItemEntity.ScanedTime = DateTime.Now;
                    _handoverItemBll.Repository().Insert(handoverItemEntity, isOpenTrans);

                    orderEntity.Status = (int) OrderStatus.Handover;
                    bool flag = _saleOrderBll.UpdateStatus(orderEntity, OrderStatus.OutStock, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception("修改订单状态出现异常，请重新操作");
                    }

                    //foreach (var item in picks)
                    //{
                    //    bool moveIn = _inventoryLocationBLL.UpdateInventoryByMoveIn(orderEntity.WarehouseId, item.ProductId, item.ToLocationCode, item.Qty, isOpenTrans);
                    //    if (!moveIn)
                    //    {
                    //        throw new Exception("更新目的储位库存失败");
                    //    }
                    //}
                }
                else
                {
                    if (itemEntity.HandoverId == handover.HandoverId)
                    {
                        if (_handoverBll.CancelItem(handover.HandoverId, itemEntity.ExpressNum))
                        {
                            orderEntity.Status = (int)OrderStatus.OutStock;
                            bool flag = _saleOrderBll.UpdateStatus(orderEntity, OrderStatus.Handover, isOpenTrans);
                            if (!flag)
                            {
                                throw new Exception("修改订单状态出现异常，请重新操作");
                            }

                            message = string.Format("物流单号[{0}]已取消扫描", orderEntity.ExpressNum);
                        }
                        else
                        {
                            throw new Exception(string.Format("物流单号[{0}]取消扫描失败", orderEntity.ExpressNum));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("物流单号[{0}]已扫入其他交接单", orderEntity.ExpressNum));
                    }

                    //foreach (var item in picks)
                    //{
                    //    bool moveIn = _inventoryLocationBLL.UpdateInventoryByMoveIn(orderEntity.WarehouseId, item.ProductId, item.ToLocationCode, -1 * item.Qty, isOpenTrans);
                    //    if (!moveIn)
                    //    {
                    //        throw new Exception("更新目的储位库存失败");
                    //    }
                    //}
                }
                database.Commit();
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("订单交接扫描异常：{0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <param name="createUserId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CloseHandover(string shipTypeId, string createUserId, out string message)
        {
            var handover = _handoverBll.GetUnPrintHandoverByShipType(shipTypeId, createUserId);
            if (handover == null)
            {
                message = "当前操作用户该物流方式不存在未关闭的交接单";
                return false;
            }

            bool flag =  _handoverBll.CloseHandover(handover);
            if (!flag)
            {
                message = "交接单关闭出现异常，请重新操作";
                return false;
            }
            message = handover.HandoverId;
            return true;
        }

        public string GetPrintTemplateContent(HandoverEntity handoverEntity, List<HandoverItemEntity> itemList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "<style>body{margin:0; padding:0;font-family: 'microsoft yahei',Helvetica;font-size:12px;color: #333;}.table-hd{ margin:0;line-height:30px; float:left; background: #f5f5f5;padding:0 10px;  margin-top:30px;}.table-hd strong{font-size:14px;font-weight:normal; float:left}.table-hd span{ font-weight:normal; font-size:12px;float:right}table{border: 1px solid #ddd;width:100%;border-collapse: collapse;border-spacing: 0; font-size:12px; float:left}table th,table td{border:1px solid #ddd;padding: 8px; text-align:center}table th{border-top:0;}</style>");
            stringBuilder.Append("<body>");
            stringBuilder.AppendFormat("<h3 class=\"table-hd\"><strong>{0}发货单</strong><span>配送方式：{1}（{2}）</span></h3>",
                "", handoverEntity.ShipTypeId, handoverEntity.CreateDate.ToString());
            stringBuilder.Append(
                "<table class=\"table table-bordered\"><thead><tr><th>物流单号</th><th>订单号</th><th>备注</th></tr></thead><tbody>");
            foreach (HandoverItemEntity itemEntity in itemList)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.AppendFormat("<td style=\"text-align:left\">{0}</td>", itemEntity.ExpressNum);
                stringBuilder.AppendFormat("<td>{0}</td>", itemEntity.OrderNo);
                stringBuilder.AppendFormat("<td>{0}</td>", "");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.AppendFormat(
                "<tr><td style=\"text-align:right\" colspan=\"3\"><span>核对人：{0} &nbsp; 签收人：{1}</span> &nbsp; <b>签收日期：{2}</b></td></tr>",
                "", "", "");
            stringBuilder.AppendLine("</tbody></table>");
            stringBuilder.Append("</body>");
            return stringBuilder.ToString();
        }
    }
}