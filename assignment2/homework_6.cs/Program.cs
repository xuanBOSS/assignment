// See https://aka.ms/new-console-template for more information
using System;

class Eratosthenes
{
    static void Sieve(int n)
    {
        bool[] isPrime = new bool[n + 1];
        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }
        //从j^2开始，是因为j的k(<j)的倍数，在对k进行操作的时候已经考虑过了，于是每一个数都从j^2开始，可以减少重复的工作
        for (int j = 2; j * j <= n; j++)
        {
            if (isPrime[j])
            {
                for (int multiple = j * j; multiple <= n; multiple += j)
                {
                    isPrime[multiple] = false;
                }
            }
        }
        for (int i = 2; i <= n; i++)
        {
            if (isPrime[i])
            {
                Console.Write(i + " ");
            }
        }
        Console.WriteLine();
    }
    static void Main()
    {
        int n = 100;
        Console.WriteLine("2~100的素数有：");
        Sieve(n);
        return;
    }
}
