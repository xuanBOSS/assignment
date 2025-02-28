// See https://aka.ms/new-console-template for more information
using System;

class ArrayCaculator
{
    static int GetMax(int[] array)
    {
        int max = array[0];
        for(int i=0;i<array.Length;i++)
        {
            if (array[i] > max) 
                max = array[i];
        }
        return max;
    }
    static int GetMin(int[] array)
    {
        int min = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] < min)
                min = array[i];
        }
        return min;
    }
    static double GetArrangeVal(int[] array)
    {
        double ans=(double) GetSumVal(array) / array.Length;
        return ans;
    }
    static int GetSumVal(int[] array)
    {
        int ans = 0;
        for (int i = 0; i < array.Length; i++)
        {
            ans += array[i];
        }
        return ans;
    }
    static void Main()
    {
        Console.WriteLine("请输入一组数组: 请用空格分隔，回车结束输入");
        string input = Console.ReadLine();
        string[] dataArray = input.Split(' ');
        int[] array = Array.ConvertAll(dataArray, s => int.Parse(s));
        Console.WriteLine("最大值为："+ GetMax(array));
        Console.WriteLine("最小值为："+ GetMin(array));
        Console.WriteLine("平均值为："+ GetArrangeVal(array));
        Console.WriteLine("求和结果为："+ GetSumVal(array));
    }
}
