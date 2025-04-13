using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace homework_12.cs
{
    internal static class Program
    {
        static void TestConnection()
        {
            var connectionString = "Server=localhost;Database=你的数据库名;User=root;Password=你的密码;";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("MySQL连接成功！");
                } // 连接在此处自动释放
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            TestConnection();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
