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
    public partial class FormAddOrder : Form
    {
        private const string ConnectionString = "server=localhost;database=orders;user=root;password=200498";
        public Order NewOrder { get; private set; }

        public FormAddOrder()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 解析输入数据
                int orderId = int.Parse(textBox5.Text.Trim());
                string clientName = textBox4.Text.Trim();
                string[] productNames = textBox3.Text.Trim().Split(' ');
                string[] unitPrices = textBox2.Text.Trim().Split(' ');
                string[] quantities = textBox1.Text.Trim().Split(' ');

                // 2. 输入验证
                if (string.IsNullOrEmpty(clientName) || productNames.Length == 0)
                {
                    MessageBox.Show("客户姓名和商品信息不能为空！", "错误",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (productNames.Length != unitPrices.Length || productNames.Length != quantities.Length)
                {
                    MessageBox.Show("商品信息格式错误，请确保数量、单价、名称一一对应！", "错误",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. 构建订单对象
                var newOrder = new Order(orderId, clientName);
                decimal totalMoney = 0; // 新增：总金额计算变量

                for (int i = 0; i < productNames.Length; i++)
                {
                    if (!double.TryParse(unitPrices[i], out double price) ||
                        !int.TryParse(quantities[i], out int quantity))
                    {
                        MessageBox.Show("单价或数量格式不正确！", "错误",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var detail = new OrderDetails(productNames[i], price, quantity);
                    newOrder.OrderDetailsList.Add(detail);
                    totalMoney += (decimal)(price * quantity); // 累计总金额
                }

                // 4. 数据库操作（使用事务保证原子性）
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 检查订单号是否已存在
                            string checkSql = "SELECT COUNT(*) FROM orders WHERE OrderId = @OrderId";
                            using (var checkCmd = new MySqlCommand(checkSql, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@OrderId", orderId);
                                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                                if (exists > 0)
                                {
                                    MessageBox.Show($"订单号 {orderId} 已存在，请修改后重试！", "错误",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            // 插入主订单（关键修改：添加TotalMoney值）
                            string orderSql = @"INSERT INTO orders 
                                        (OrderId, ClientName, OrderDate, TotalMoney)
                                        VALUES (@OrderId, @ClientName, @OrderDate, @TotalMoney)";

                            using (var cmd = new MySqlCommand(orderSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@ClientName", clientName);
                                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@TotalMoney", totalMoney); // 使用计算值
                                cmd.ExecuteNonQuery();
                            }

                            // 插入订单明细（修正表名：orderdetail → ordersdetail）
                            string detailSql = @"INSERT INTO ordersdetail 
                                         (OrderId, ProductName, UnitPrice, Quantity)
                                         VALUES (@OrderId, @ProductName, @UnitPrice, @Quantity)";

                            foreach (var detail in newOrder.OrderDetailsList)
                            {
                                using (var cmd = new MySqlCommand(detailSql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                                    cmd.Parameters.AddWithValue("@ProductName", detail.ProductName);
                                    cmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                    cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();

                            // 更新内存对象的总金额
                            newOrder.TotalMoney = (double)totalMoney;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // 5. 更新内存数据
                OrderService.Instance.AddOrder(newOrder);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySqlException ex)
            {
                string errorMsg = ex.Number == 1062 ? "订单号已存在！" :
                                ex.Number == 1146 ? "数据库表不存在，请检查表名！" :
                                $"数据库错误({ex.Number})";
                MessageBox.Show($"{errorMsg}\n{ex.Message}", "错误",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统错误: {ex.Message}", "错误",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
