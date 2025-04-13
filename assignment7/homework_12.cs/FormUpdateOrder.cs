using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
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

        /*private void button1_Click(object sender, EventArgs e)
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
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 获取并验证输入
                string orderIdText = textBox1.Text.Trim();
                if (!int.TryParse(orderIdText, out int orderId))
                {
                    MessageBox.Show("订单号格式错误，请输入正确的数字！", "错误",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string clientName = textBox2.Text.Trim();
                string[] productNames = textBox3.Text.Trim().Split(' ');
                string[] unitPrices = textBox4.Text.Trim().Split(' ');
                string[] quantities = textBox5.Text.Trim().Split(' ');

                // 2. 输入合法性校验
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

                // 3. 构建新订单明细（使用decimal保证精度）
                List<OrderDetails> newOrderDetails = new List<OrderDetails>();
                for (int i = 0; i < productNames.Length; i++)
                {
                    if (!decimal.TryParse(unitPrices[i], out decimal unitPrice) ||
                        !int.TryParse(quantities[i], out int quantity))
                    {
                        MessageBox.Show($"商品 '{productNames[i]}' 的单价或数量格式错误！", "错误",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    newOrderDetails.Add(new OrderDetails(productNames[i], (double)unitPrice, quantity));
                }

                // 4. 执行数据库更新
                using (var conn = new MySqlConnection("server=localhost;database=orders;user=root;password=200498"))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 4.1 计算总金额
                            decimal totalMoney = newOrderDetails.Sum(d =>(decimal) d.UnitPrice * d.Quantity);

                            // 4.2 更新主订单
                            string updateOrderSql = @"UPDATE orders 
                                           SET ClientName = @ClientName, 
                                               TotalMoney = @TotalMoney
                                           WHERE OrderId = @OrderId";

                            using (var cmd = new MySqlCommand(updateOrderSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@ClientName", clientName);
                                cmd.Parameters.AddWithValue("@TotalMoney", totalMoney);

                                if (cmd.ExecuteNonQuery() == 0)
                                    throw new Exception("订单不存在");
                            }

                            // 4.3 删除旧明细
                            /*string deleteDetailsSql = "DELETE FROM ordersdetail WHERE OrderId = @OrderId";
                            new MySqlCommand(deleteDetailsSql, conn, transaction)
                                .Parameters.AddWithValue("@OrderId", orderId)
                                .ExecuteNonQuery();*/
                            // 4.3 删除旧明细（完整事务版）
                            string deleteDetailsSql = "DELETE FROM ordersdetail WHERE OrderId = @OrderId";
                            try
                            {
                                using (var cmd = new MySqlCommand(deleteDetailsSql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                                    int affectedRows = cmd.ExecuteNonQuery();

                                    if (affectedRows == 0)
                                    {
                                        // 可选：记录日志或抛出异常
                                        Debug.WriteLine($"未找到订单号为 {orderId} 的明细记录");
                                    }
                                }
                            }
                            catch (MySqlException ex)
                            {
                                // 处理数据库错误（如外键约束等）
                                throw new Exception($"删除订单明细失败: {ex.Message}", ex);
                            }

                            // 4.4 插入新明细
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
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // 5. 更新内存数据
                var updatedOrder = OrderService.Instance.GetAllOrders()
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (updatedOrder != null)
                {
                    updatedOrder.OrderDetailsList = newOrderDetails;
                    updatedOrder.TotalMoney = newOrderDetails.Sum(d => d.UnitPrice * d.Quantity);
                }

                // 6. 关闭窗口
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"数据库错误({ex.Number}): {ex.Message}", "错误",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改失败: {ex.Message}", "错误",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
