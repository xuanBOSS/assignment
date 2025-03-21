// See https://aka.ms/new-console-template for more information
/*实现添加订单、删除订单、修改订单、查询订单（按照订单号、商品名称、
客户、订单⾦额等进⾏查询）功能。并对各个Public⽅法编写测试⽤例。*/
using System;
using homework_11;

namespace homework_11
{
    public class Program
    {
        static void Main()
        {
            OrderService orderService = new OrderService();
            //开始输出用户可视的操作
            while (true)
            {
                Console.WriteLine("\n***订单管理控制台程序***");
                Console.WriteLine("1 : 添加订单");
                Console.WriteLine("2 : 删除订单");
                Console.WriteLine("3 : 修改订单");
                Console.WriteLine("4 : 查询订单（按客户名）");
                Console.WriteLine("5 : 查询订单（按商品名）");
                Console.WriteLine("6 : 查询订单（按订单金额）");
                Console.WriteLine("7 : 显示所有订单");
                Console.WriteLine("8 : 对订单进行排序");
                Console.WriteLine("9 : 退出");
                Console.Write("请选择操作：");
                //获取用户的输入
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddOrder(orderService);
                        break;
                    case "2":
                        DeleteOrder(orderService);
                        break;
                    case "3":
                        UpdateOrder(orderService);
                        break;
                    case "4":
                        FindOrderByClientName(orderService);
                        break;
                    case "5":
                        FindOrderByProductName(orderService);
                        break;
                    case "6":
                        FindOrderByTotalMoney(orderService);
                        break;
                    case "7":
                        ShowAllOrders(orderService);
                        break;
                    case "8":
                        SortOrders(orderService);
                        break;
                    case "9":
                        Console.WriteLine("退出系统。");
                        return;
                    default:
                        Console.WriteLine("无效输入，请您重新选择需要办理的项目");
                        break;
                }
            }
        }
        //添加订单
        static void AddOrder(OrderService orderService)
        {
            //存在异常输出，用try catch语句
            try
            {
                Console.WriteLine("请输入订单号：");
                int orderId = int.Parse(Console.ReadLine());
                Console.WriteLine("请输入客户的名字：");
                string clientName = Console.ReadLine();

                Order order = new Order(orderId, clientName);

                while (true)
                {
                    Console.Write("请输入商品名称：（end表示输入结束）");
                    string productName = Console.ReadLine();
                    if (productName.ToLower() == "end") break;

                    Console.Write("请输入商品单价： ");
                    double unitPrice = double.Parse(Console.ReadLine());
                    Console.Write("请输入购买数量： ");
                    int quantity = int.Parse(Console.ReadLine());

                    order.OrderDetailsList.Add(new OrderDetails(productName, unitPrice, quantity));
                }
                orderService.AddOrder(order);
                Console.WriteLine("添加成功！");
            }
            catch (Exception e)
            {
                Console.WriteLine($"异常：{e.Message}");
            }
        }

        // 删除订单
        static void DeleteOrder(OrderService orderService)
        {
            Console.Write("请输入要删除的订单号： ");
            int orderId = int.Parse(Console.ReadLine());

            try
            {
                orderService.DeleteOrder(orderId);
                Console.WriteLine("订单删除成功！");
            }
            catch (Exception e)
            {
                Console.WriteLine($"异常：{e.Message}");
            }
        }

        // 修改订单
        static void UpdateOrder(OrderService orderService)
        {
            Console.Write("请输入要修改的订单号： ");
            int orderId = int.Parse(Console.ReadLine());

            try
            {
                List<OrderDetails> newDetails = new List<OrderDetails>();
                while (true)
                {
                    Console.Write("请输入新的商品名称：（end表示输入结束）");
                    string productName = Console.ReadLine();
                    if (productName.ToLower() == "end") break;

                    Console.Write("请输入新的商品单价： ");
                    double unitPrice = double.Parse(Console.ReadLine());
                    Console.Write("请输入新的购买数量： ");
                    int quantity = int.Parse(Console.ReadLine());

                    newDetails.Add(new OrderDetails(productName, unitPrice, quantity));
                }

                orderService.UpdateOrder(orderId, newDetails);
                Console.WriteLine("订单修改成功！");
            }
            catch (Exception e)
            {
                Console.WriteLine($"异常：{e.Message}");
            }
        }
        // 查询订单（按客户名）
        static void FindOrderByClientName(OrderService orderService)
        {
            Console.Write("请输入客户名称： ");
            string clientName = Console.ReadLine();
            var orders = orderService.FindOrderByClientName(clientName);
            DisplayOrders(orders);
        }

        // 查询订单（按商品名）
        static void FindOrderByProductName(OrderService orderService)
        {
            Console.Write("请输入商品名称： ");
            string productName = Console.ReadLine();
            var orders = orderService.FindOrderByProductName(productName);
            DisplayOrders(orders);
        }

        // 查询订单（按订单金额）
        static void FindOrderByTotalMoney(OrderService orderService)
        {
            Console.Write("请输入最小订单金额： ");
            double minMoney = double.Parse(Console.ReadLine());
            var orders = orderService.FindOrderByTotalMoney(minMoney);
            DisplayOrders(orders);
        }

        // 显示订单信息
        static void DisplayOrders(List<Order> orders)
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("没有找到符合条件的订单。");
                return;
            }
            //==for中的auto :
            foreach (var order in orders)
            {
                Console.WriteLine(order);
                foreach (var detail in order.OrderDetailsList)
                {
                    Console.WriteLine($"  {detail}");
                }
            }
        }

        // 显示所有订单
        static void ShowAllOrders(OrderService orderService)
        {
            DisplayOrders(orderService.FindOrderByTotalMoney(0));
        }

        // 排序订单
        static void SortOrders(OrderService orderService)
        {
            Console.WriteLine("请选择排序方式：");
            Console.WriteLine("1 ：按订单号排序");
            Console.WriteLine("2 ：按订单金额排序");
            Console.WriteLine("3 ：按客户姓名排序");
            Console.Write("选择排序方式: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    orderService.SortOrders(o => o.OrderId);
                    break;
                case "2":
                    orderService.SortOrders(o => o.TotalMoney);
                    break;
                case "3":
                    orderService.SortOrders(o => o.ClientName);
                    break;
                default:
                    Console.WriteLine("无效输入，默认按订单号排序。");
                    orderService.SortOrders();
                    break;
            }

            Console.WriteLine("排序结束！");
        }

    }
}