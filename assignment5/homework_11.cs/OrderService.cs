using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using homework_11;
/*添加订单、删除订单、修改订单、查询订单（按照订单号、商品名称、
客户、订单⾦额等进⾏查询）功能*/
/*在订单删除、修改失败时，能够产⽣异常并显示给客户错误信息。*/
/*OrderService提供排序⽅法对保存的订单进⾏排序。默认按照订单号排序，也可以使⽤Lambda表达式进⾏
⾃定义排序*/
namespace homework_11
{
    public class OrderService
    {
        private List<Order> orders = new List<Order>();

        //添加订单功能
        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
                throw new Exception("Order already exists.");
            orders.Add(order);
        }

        //删除订单功能  根据订单号去删除订单
        public void DeleteOrder(int orderId)
        {
            //var=auto  FirstOrDefault LINQ扩展，找第一个对应的OrderId
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found");
            orders.Remove(order);
        }

        //修改订单功能  根据订单号先查找，再修改
        public void UpdateOrder(int orderId, List<OrderDetails> newOrderDetails)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found");
            order.OrderDetailsList = newOrderDetails;
        }

        //查询订单  按照客户名字查
        public List<Order> FindOrderByClientName(string clientName)
        {
            return orders.Where(o => o.ClientName == clientName).ToList();
        }
        //查询订单  按照单号查
        public List<Order> FindOrderByProductName(string productName)
        {
            return orders.Where(o => o.OrderDetailsList.Any(d => d.ProductName == productName)).ToList();
        }
        //查询订单  按照订单金额
        public List<Order> FindOrderByTotalMoney(double minMoney)
        {
            return orders.Where(o => o.TotalMoney >= minMoney).ToList();
        }
        //排序
        /*keySelector是Func<Order,object>类型的委托（函数指针），可以用于传入排序的方式
        如果为空，就按默认的订单号排序*/
        public void SortOrders(Func<Order,object> keySelector=null)
        {
            orders = (keySelector == null ? orders.OrderBy(o => o.OrderId) : orders.OrderBy(keySelector)).ToList();
        }
    }
}
