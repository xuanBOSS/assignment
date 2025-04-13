using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
            // 1. 检查参数是否为 null
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "订单对象不能为 null");
            }
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
        /*public void DeleteOrder(int orderId)
        {
            //var=auto  FirstOrDefault LINQ扩展，找第一个对应的OrderId
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found");
            orders.Remove(order);
        }*/
        public void DeleteOrder(int orderId)
        {
            const string connStr = "server=localhost;database=orders;user=root;password=200498";

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) // 启用事务
                {
                    try
                    {
                        // 先删除明细（外键约束要求必须先删明细）
                        string detailSql = "DELETE FROM ordersdetail WHERE OrderId = @OrderId";
                        using (var cmd = new MySqlCommand(detailSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 再删除主订单
                        string orderSql = "DELETE FROM orders WHERE OrderId = @OrderId";
                        using (var cmd = new MySqlCommand(orderSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows == 0)
                                throw new Exception("订单不存在");
                        }

                        transaction.Commit();

                        // 同步删除内存数据（保持原有逻辑）
                        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
                        if (order != null) orders.Remove(order);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //修改订单功能  根据订单号先查找，再修改
        /*public void UpdateOrder(int orderId, List<OrderDetails> newOrderDetails)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found");
            order.OrderDetailsList = newOrderDetails;
        }*/
        public void UpdateOrder(int orderId, List<OrderDetails> newOrderDetails)
        {
            const string connStr = "server=localhost;database=orders;user=root;password=200498";

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. 计算新的总金额
                        decimal totalMoney = newOrderDetails.Sum(d => (decimal)(d.UnitPrice * d.Quantity));

                        // 2. 更新主订单信息
                        string updateOrderSql = @"UPDATE orders 
                                        SET ClientName = @ClientName, 
                                            TotalMoney = @TotalMoney
                                        WHERE OrderId = @OrderId";

                        using (var cmd = new MySqlCommand(updateOrderSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@ClientName", newOrderDetails.FirstOrDefault()?.ProductName ?? "");
                            cmd.Parameters.AddWithValue("@TotalMoney", totalMoney);

                            int affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows == 0)
                                throw new Exception("订单不存在");
                        }

                        // 3. 删除原有明细
                        string deleteDetailsSql = "DELETE FROM ordersdetail WHERE OrderId = @OrderId";
                        using (var cmd = new MySqlCommand(deleteDetailsSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 4. 插入新明细
                        string insertDetailSql = @"INSERT INTO ordersdetail 
                                         (OrderId, ProductName, UnitPrice, Quantity)
                                         VALUES (@OrderId, @ProductName, @UnitPrice, @Quantity)";

                        foreach (var detail in newOrderDetails)
                        {
                            using (var cmd = new MySqlCommand(insertDetailSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@ProductName", detail.ProductName);
                                cmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        // 5. 更新内存数据（保持原有逻辑）
                        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
                        if (order != null)
                        {
                            order.OrderDetailsList = newOrderDetails;
                            order.TotalMoney = (double)totalMoney;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //查询订单  按照客户名字查
        /*public List<Order> FindOrderByClientName(string clientName)
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
        }*/
        private const string ConnStr = "server=localhost;database=orders;user=root;password=200498";

        // 按客户名查询（数据库版）
        public List<Order> FindOrderByClientName(string clientName)
        {
            var orders = new List<Order>();
            using (var conn = new MySqlConnection(ConnStr))
            {
                string sql = @"SELECT o.OrderId, o.ClientName, o.TotalMoney, 
                          d.ProductName, d.UnitPrice, d.Quantity
                          FROM orders o
                          JOIN orderdetail d ON o.OrderId = d.OrderId
                          WHERE o.ClientName LIKE @ClientName";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientName", $"%{clientName}%");
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 实现订单合并逻辑（同订单号合并明细）
                            MergeOrderFromReader(orders, reader);
                        }
                    }
                }
            }
            return orders;
        }

        // 按商品名查询（数据库版）
        public List<Order> FindOrderByProductName(string productName)
        {
            var orders = new List<Order>();
            using (var conn = new MySqlConnection(ConnStr))
            {
                string sql = @"SELECT o.OrderId, o.ClientName, o.TotalMoney, 
                          d.ProductName, d.UnitPrice, d.Quantity
                          FROM orders o
                          JOIN orderdetail d ON o.OrderId = d.OrderId
                          WHERE d.ProductName LIKE @ProductName";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", $"%{productName}%");
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MergeOrderFromReader(orders, reader);
                        }
                    }
                }
            }
            return orders;
        }

        // 按金额查询（数据库版）
        public List<Order> FindOrderByTotalMoney(decimal minMoney)
        {
            var orders = new List<Order>();
            using (var conn = new MySqlConnection(ConnStr))
            {
                string sql = @"SELECT o.OrderId, o.ClientName, o.TotalMoney, 
                          d.ProductName, d.UnitPrice, d.Quantity
                          FROM orders o
                          JOIN orderdetail d ON o.OrderId = d.OrderId
                          WHERE o.TotalMoney >= @MinMoney
                          ORDER BY o.TotalMoney DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MinMoney", minMoney);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MergeOrderFromReader(orders, reader);
                        }
                    }
                }
            }
            return orders;
        }

        // 辅助方法：合并订单数据
        private void MergeOrderFromReader(List<Order> orders, MySqlDataReader reader)
        {
            int orderId = reader.GetInt32("OrderId");
            var existingOrder = orders.FirstOrDefault(o => o.OrderId == orderId);

            if (existingOrder == null)
            {
                existingOrder = new Order(
                    orderId,
                    reader.GetString("ClientName"))
                {
                    TotalMoney = (double)reader.GetDecimal("TotalMoney")
                };
                orders.Add(existingOrder);
            }

            existingOrder.OrderDetailsList.Add(new OrderDetails(
                reader.GetString("ProductName"),
                (double)reader.GetDecimal("UnitPrice"),
                reader.GetInt32("Quantity")));
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
