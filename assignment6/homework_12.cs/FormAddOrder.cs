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
    public partial class FormAddOrder : Form
    {
        public Order NewOrder { get; private set; }
        public FormAddOrder()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int orderId = int.Parse(textBox5.Text.Trim());
                string clientName = textBox4.Text.Trim();
                string[] productNames = textBox3.Text.Trim().Split(' ');
                string[] unitPrices = textBox2.Text.Trim().Split(' ');
                string[] quantities = textBox1.Text.Trim().Split(' ');

                if (clientName == "" || productNames.Length == 0)
                {
                    MessageBox.Show("客户姓名和商品信息不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (productNames.Length != unitPrices.Length || productNames.Length != quantities.Length)
                {
                    MessageBox.Show("商品信息格式错误，请确保数量、单价、名称一一对应！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Order newOrder = new Order(orderId, clientName);

                for (int i = 0; i < productNames.Length; i++)
                {
                    double price = double.Parse(unitPrices[i]);
                    int quantity = int.Parse(quantities[i]);
                    newOrder.OrderDetailsList.Add(new OrderDetails(productNames[i], price, quantity));
                }

                // 添加订单到全局 OrderService
                OrderService.Instance.AddOrder(newOrder);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入数据格式有误，请检查！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
