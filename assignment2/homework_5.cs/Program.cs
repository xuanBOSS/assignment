// See https://aka.ms/new-console-template for more information
using System;

class ToeplitzMatrix 
{
    static bool IsToeplitzMatrix(int[,] m)
    {
        int r = m.GetLength(0);   //行数
        int c = m.GetLength(1);   //列数
        //m[i][j]=m[i-1][j-1]
        for (int i = 1; i < r; i++) 
        {
            for (int j = 1; j < c; j++) 
            {
                if (m[i, j] != m[i - 1, j - 1]) 
                {
                    return false; 
                }
            }
        }
        return true;
    }
    static void Main()
    {
        //用户输入矩阵
        Console.Write("请输入矩阵的行数: ");
        int rows = int.Parse(Console.ReadLine());

        Console.Write("请输入矩阵的列数: ");
        int cols = int.Parse(Console.ReadLine());

        int[,] m = new int[rows, cols];

        Console.WriteLine("请依次输入矩阵的元素：");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"请输入第 {i + 1} 行，第 {j + 1} 列的元素: ");
                m[i, j] = int.Parse(Console.ReadLine()); 
            }
        }
        Console.WriteLine("\n你输入的矩阵是：");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(m[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("\n你输入的矩阵是托普利茨矩阵吗？");
        bool ans=IsToeplitzMatrix(m);
        Console.WriteLine(ans);
        return;
    }
}
