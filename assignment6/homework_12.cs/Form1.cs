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
    public partial class Form1 : Form
    {
        private OrderService orderService;
        public Form1()
        {
            InitializeComponent();
            orderService = OrderService.Instance;// 初始化 orderService 实例
        }

        private void RefreshAllOrderForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormShowOrders orderForm) // 检查是否为订单窗口
                {
                    orderForm.RefreshData(); // 调用子窗口的刷新方法
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FormAddOrder addOrderForm = new FormAddOrder();
            if (addOrderForm.ShowDialog() == DialogResult.OK)
            {
                orderService.AddOrder(addOrderForm.NewOrder);
                MessageBox.Show("订单添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshAllOrderForms();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormDeleteOrder deleteOrderForm = new FormDeleteOrder(orderService); // 传递 OrderService 实例
            deleteOrderForm.ShowDialog(); // 以模态窗口方式打开
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FormUpdateOrder updateOrderForm = new FormUpdateOrder(orderService);
                updateOrderForm.ShowDialog(); // 以模态窗口方式打开
                if (updateOrderForm.UpdatedOrder != null)
                {
                    MessageBox.Show($"订单 {updateOrderForm.UpdatedOrder.OrderId} 已修改!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开修改订单窗口时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormQueryType queryTypeForm = new FormQueryType(orderService);
            queryTypeForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormShowOrders showOrdersForm = new FormShowOrders();
            showOrdersForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormSortOrders sortOrdersForm = new FormSortOrders();
            sortOrdersForm.Show();
        }
    }
}
