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
    public partial class FormQueryByProductName : Form
    {
        private OrderService orderService;
        public FormQueryByProductName()
        {
            InitializeComponent();
            orderService = OrderService.Instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("请输入商品名称！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var orders = orderService.FindOrderByProductName(productName);

            if (orders.Count > 0)
            {
                DisplayOrders(orders); // 显示查询结果
            }
            else
            {
                MessageBox.Show("没有找到匹配的订单", "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DisplayOrders(List<Order> orders)
        {
            // 这里你可以用一个列表控件来显示订单信息，或者弹出一个 MessageBox 等等
            StringBuilder sb = new StringBuilder();
            foreach (var order in orders)
            {
                sb.AppendLine($"订单号: {order.OrderId}, 客户: {order.ClientName}, 总金额: {order.TotalMoney}");
            }
            MessageBox.Show(sb.ToString(), "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
