using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using homework_11;
namespace homework_11
{
    public class Order
    {
        public int OrderId { get; set; }  //订单号
        public string ClientName { get; set; }  //客户名称
        public DateTime OrderDate { get; set; }  // 订单日期
        //订单明细列表，包含所有的商品信息
        public List<OrderDetails> OrderDetailsList { get; set; } = new List<OrderDetails>();
        public double TotalMoney //订单金额
        { 
            //订单明细中所有商品的价值总和
            get { return OrderDetailsList.Sum(d => d.Quantity*d.UnitPrice); } 
        }  
        
        public Order(int orderId,string clientName)
        {
            OrderId = orderId;
            ClientName = clientName;
            OrderDate = DateTime.Now;
        }

        //重写Equal 判断两个 Order 对象是否相等
        public override bool Equals(object obj)
        {
            return obj is Order order && OrderId == order.OrderId;
        }
        //和Equal一起 可以确保 Order 对象在逻辑上按照 OrderId 进行相等性比较，而不是默认的引用比较
        public override int GetHashCode()
        {
            return OrderId.GetHashCode();
        }

        //重写ToString
        public override string ToString()
        {
            return $"Order ID: {OrderId}, Client: {ClientName}, Date: {OrderDate}, Total: {TotalMoney:C}";
        }
    }
}
