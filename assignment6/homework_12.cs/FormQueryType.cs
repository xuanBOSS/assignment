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
    public partial class FormQueryType : Form
    {
        private OrderService orderService;
        public FormQueryType(OrderService orderService)
        {
            InitializeComponent();
            orderService = OrderService.Instance;  // 获取全局唯一实例
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 打开根据商品名查询的界面
            FormQueryByProductName queryForm = new FormQueryByProductName();
            queryForm.ShowDialog(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 打开根据订单金额查询的界面
            FormQueryByOrderAmount queryForm = new FormQueryByOrderAmount();
            queryForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 打开根据客户名查询的界面
            FormQueryByClirntName queryForm = new FormQueryByClirntName();
            queryForm.ShowDialog(); 
        }
    }
}
