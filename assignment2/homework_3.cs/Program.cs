// See https://aka.ms/new-console-template for more information
using System;

class PrimeNum
{
    static bool IsPrime(int n)
    {
        if(n<=1) return false;
        for(int i=2;i*i<=n;i++)  //素数的判断方法：试除法，用n开根号，只看这个值以下的数是不是能整除
        {
            if(n%i==0) return false;
        }
        return true;
    }
    static void PrintPrimeFactor(int n)
    {
        Console.WriteLine("素数因子有：");
        for(int i=2;i<=n;i++)
        {
            while(n%i==0&&IsPrime(i))  //i为因子，且i为素数
            {
                Console.Write(i + "");   //依次输出，中间空格隔开
                n = n / i;   //短除法
            }
        }
    }
    static void Main()
    {
        Console.WriteLine("请输入要求素数因子的数：");
        int n=int.Parse(Console.ReadLine());    //要注意类型转换，int n=Console.Read()是错误的
        /*Console.Write(n);*/
        PrintPrimeFactor(n);
    }
}
