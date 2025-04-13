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
    /*
     public partial class FormShowOrders : Form
     {
         private const string ConnectionString = "server=localhost;database=orders;user=root;password=200498";
         private BindingSource _bindingSource = new BindingSource(); // 声明BindingSource
         private OrderService orderService = OrderService.Instance;
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

    //}
    public partial class FormShowOrders : Form
    {
        private const string ConnectionString = "server=localhost;database=orders;user=root;password=200498";
        private readonly BindingSource _bindingSource = new BindingSource();

        public FormShowOrders()
        {
            InitializeComponent();
            ConfigureDataGridView();
            this.Load += FormShowOrders_Load; // 修正：使用标准事件绑定方式
        }

        // 新增：Load事件处理方法（解决CS1061错误）
        private void FormShowOrders_Load(object sender, EventArgs e)
        {
            LoadOrdersDirect();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                DataPropertyName = "OrderId",
                HeaderText = "订单号",
                Width = 120
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClientName",
                DataPropertyName = "ClientName",
                HeaderText = "客户姓名",
                Width = 150
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderDate",
                DataPropertyName = "OrderDate",
                HeaderText = "订单日期",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "yyyy-MM-dd HH:mm"  // 明确日期格式
                }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalMoney",
                DataPropertyName = "TotalMoney",
                HeaderText = "总金额",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "C2"
                }
            });
        }

        private void LoadOrdersDirect()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT OrderId, ClientName, OrderDate, TotalMoney FROM orders";

                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        _bindingSource.DataSource = dataTable;
                        dataGridView1.DataSource = _bindingSource;

                        MessageBox.Show($"成功加载 {dataTable.Rows.Count} 条订单",
                                      "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"数据库连接失败: {ex.Message}\n错误代码: {ex.Number}",
                              "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统错误: {ex.Message}",
                              "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            LoadOrdersDirect();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 预留单元格点击事件
        }
    }
}
