using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace homework_12.cs
{
   
    public partial class FormShowOrders : Form
    {
        private BindingSource _bindingSource = new BindingSource(); // 声明BindingSource
        private OrderService orderService;
        public FormShowOrders()
        {
            InitializeComponent();
            orderService = OrderService.Instance;
            this.Load += FormShowOrders_Load; // 手动绑定事件
        }

        public void RefreshData()
        {
            dataGridView1.AutoGenerateColumns = true; // 允许自动生成列
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = OrderService.Instance.GetAllOrders(); // 直接绑定原始数据
            dataGridView1.DataSource = _bindingSource;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        private void FormShowOrders_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            var orders = OrderService.Instance.GetAllOrders();
            Console.WriteLine($"Debug: 订单数量 = {orders.Count}"); // 调试输出
            RefreshData();
        }
        /* 显示所有订单数据
        private void DisplayOrders()
        {
            // 获取所有订单
            var orders = orderService.GetAllOrders();
            //dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = orders.Select(o => new
            {
                订单号 = o.OrderId,
                客户姓名 = o.ClientName,
                订单金额 = o.TotalMoney  // 自动计算的总金额
            }).ToList();

            //Console.WriteLine($"订单数: {orders.Count}, 首单金额: {orders.FirstOrDefault()?.TotalMoney}");
            // 如果没有订单
            if (orders.Count == 0)
            {
                MessageBox.Show("没有订单可显示", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            /*var testOrders = new List<Order>
            {
                    new Order(1 , "测试客户")
            };
            
            dataGridView1.DataSource = testOrders;
            /*dataGridView1.AutoGenerateColumns = true; // 确保自动生成列
            dataGridView1.DataSource = orders;
            // 填充数据到 DataGridView
            /*foreach (var order in orders)
            {
                dataGridView1.Rows.Add(order.OrderId, order.ClientName, order.TotalMoney);
            }*/
        
    }
}
