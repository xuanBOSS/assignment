using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using homework_11;
namespace homework_11
{
    public class OrderDetails  //订单明细代表订单中的单个商品
    {
        public string ProductName { get; set; } //商品名称
        public double UnitPrice { get; set; } //商品单价
        public int Quantity { get; set; } //购买数量
        public double TotalPrice
        {
            get { return UnitPrice * Quantity; }  // 计算当前商品的小计金额
        }
        public OrderDetails(string productName, double unitPrice, int quantity)
        {
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderDetails details && ProductName == details.ProductName;
        }
        public override int GetHashCode()
        {
            return ProductName.GetHashCode();
        }

        public override string ToString()
        {
            return $"{ProductName}: {Quantity} * {UnitPrice:C} = {TotalPrice:C}";
        }
    }
}
