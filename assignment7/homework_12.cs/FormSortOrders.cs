using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace homework_12.cs
{
    public partial class FormSortOrders : Form
    {
        private OrderService orderService;
        public FormSortOrders()
        {
            InitializeComponent();
            orderService = OrderService.Instance;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择排序方式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedOption = comboBox1.SelectedItem.ToString();
            List<Order> sortedOrders;

            if (selectedOption == "按订单号排序")
            {
                sortedOrders = orderService.GetAllOrders().OrderBy(o => o.OrderId).ToList();
            }
            else if (selectedOption == "按订单金额排序")
            {
                sortedOrders = orderService.GetAllOrders().OrderBy(o => o.TotalMoney).ToList();
            }
            else
            {
                sortedOrders = orderService.GetAllOrders().OrderBy(o => o.OrderId).ToList();
            }

            // ✅ 更新 OrderService 里的订单数据
            orderService.UpdateOrders(sortedOrders);

            // ✅ 让所有打开的 FormShowOrders 重新刷新
            RefreshAllOrderForms();

            MessageBox.Show("排序成功！请重新点击 '显示订单' 查看排序结果", "排序完成", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择排序方式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ 直接调用 OrderService 的排序方法
            switch (comboBox1.SelectedItem.ToString())
            {
                case "按订单号排序":
                    orderService.SortOrdersByOrderId();
                    break;
                case "按订单金额排序":
                    orderService.SortOrdersByAmount();
                    break;
                default:
                    orderService.SortOrdersByOrderId(); // 默认按订单号排序
                    break;
            }

            // ✅ 刷新所有订单显示窗体
            RefreshAllOrderForms();

            MessageBox.Show("排序成功！", "排序完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // 可选：关闭排序窗口
        }
        private void RefreshAllOrderForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormShowOrders showOrdersForm)
                {
                    showOrdersForm.RefreshData(); // 让 FormShowOrders 重新加载数据
                }
            }
        }
    }
}
