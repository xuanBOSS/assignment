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
    public partial class FormDeleteOrder : Form
    {
        private OrderService orderService;
        public FormDeleteOrder(OrderService service)
        {
            InitializeComponent();
            orderService = service;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string orderNumber = textBox1.Text.Trim();  // 获取用户输入的订单号

                // 确保订单号是有效的整数
                if (!int.TryParse(orderNumber, out int orderNumberInt))
                {
                    MessageBox.Show("订单号格式错误，请输入正确的数字！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 先检查订单是否存在
                var existingOrder = OrderService.Instance.GetAllOrders().FirstOrDefault(o => o.OrderId == orderNumberInt);
                if (existingOrder == null)
                {
                    MessageBox.Show("订单不存在！请检查订单号是否正确。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 删除订单
                OrderService.Instance.DeleteOrder(orderNumberInt);
                MessageBox.Show("订单删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();  // 关闭删除订单窗口
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除订单失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
