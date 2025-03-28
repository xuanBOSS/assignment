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
    public partial class FormUpdateOrder : Form
    {
        private OrderService orderService;
        public Order UpdatedOrder { get; private set; }
        public FormUpdateOrder(OrderService orderService)
        {
            InitializeComponent();
            this.orderService = orderService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的订单号
                string orderIdText = textBox1.Text.Trim();
                if (!int.TryParse(orderIdText, out int orderId))
                {
                    MessageBox.Show("订单号格式错误，请输入正确的数字！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string clientName = textBox2.Text.Trim();
                string[] productNames = textBox3.Text.Trim().Split(' ');
                string[] unitPrices = textBox4.Text.Trim().Split(' ');
                string[] quantities = textBox5.Text.Trim().Split(' ');

                // 校验输入是否合法
                if (string.IsNullOrEmpty(clientName) || productNames.Length == 0)
                {
                    MessageBox.Show("客户姓名和商品信息不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (productNames.Length != unitPrices.Length || productNames.Length != quantities.Length)
                {
                    MessageBox.Show("商品信息格式错误，请确保数量、单价、名称一一对应！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 检查订单是否存在
                var existingOrder = OrderService.Instance.GetAllOrders().FirstOrDefault(o => o.OrderId == orderId);
                if (existingOrder == null)
                {
                    MessageBox.Show("订单不存在，无法修改！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建新订单明细
                List<OrderDetails> newOrderDetails = new List<OrderDetails>();
                for (int i = 0; i < productNames.Length; i++)
                {
                    if (!double.TryParse(unitPrices[i], out double unitPrice) || !int.TryParse(quantities[i], out int quantity))
                    {
                        MessageBox.Show($"商品 '{productNames[i]}' 的单价或数量格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    newOrderDetails.Add(new OrderDetails(productNames[i], unitPrice, quantity));
                }

                // 更新订单
                OrderService.Instance.UpdateOrder(orderId, newOrderDetails);

                MessageBox.Show("订单修改成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 更新界面数据并关闭窗口
                this.UpdatedOrder = existingOrder;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改订单失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
