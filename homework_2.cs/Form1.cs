using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace homework_2.cs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbOperator.Items.Add("+");
            cmbOperator.Items.Add("-");
            cmbOperator.Items.Add("*");
            cmbOperator.Items.Add("/");
            cmbOperator.SelectedIndex = 0; // 默认选中 +
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double num1 = Convert.ToDouble(txtNum1.Text);
                double num2 = Convert.ToDouble(txtNum2.Text);
                string opChar = cmbOperator.SelectedItem.ToString();

                double result = 0;

                switch (opChar)
                {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        if (num2 == 0)
                        {
                            MessageBox.Show("错误：除数不能为零！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            result = num1 / num2;
                        break;
                    default:
                        return;
                }

                lblResult.Text = $"计算结果: {result}";
                /*Console.WriteLine($"输入数字1: {num1}, 输入数字2: {num2}, 运算符: {operatorChar}, 计算结果: {result}");*/

            }
            catch (FormatException)
            {
                MessageBox.Show("请输入有效的数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
