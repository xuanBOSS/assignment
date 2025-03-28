using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework_12.cs
{
    public class OrderService
    {
        private static OrderService instance;
        private List<Order> orders;

        public OrderService() { orders = new List<Order>(); }

        // 获取全局唯一实例
        public static OrderService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderService();
                }
                return instance;
            }
        }

        //添加订单功能
        public void AddOrder(Order order)
        {
            if (orders == null)
            {
                orders = new List<Order>();  // 确保 orders 在使用前被初始化
            }
            if (orders.Any(o => o.OrderId == order.OrderId))
            {
                throw new InvalidOperationException("订单已存在");
            }
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
        // 按订单金额排序
        public void SortOrdersByAmount()
        {
            orders = orders.OrderBy(o => o.TotalMoney).ToList(); // 按金额升序排序
        }

        // 按订单ID排序
        public void SortOrdersByOrderId()
        {
            orders = orders.OrderBy(o => o.OrderId).ToList(); // 按ID升序排序
        }

        // 获取所有订单
        public List<Order> GetAllOrders()
        {
            return orders ?? new List<Order>() ;
        }

        public void UpdateOrders(List<Order> sortedOrders)
        {
            orders = sortedOrders;  
        }
    }
}
