﻿// See https://aka.ms/new-console-template for more information
using System;

class Calculator
{
    static void Main()
    {
        Console.Write("请输入第一个数字: ");
        double num1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("请输入运算符 : ");
        char opChar = Convert.ToChar(Console.ReadLine());

        Console.Write("请输入第二个数字: ");
        double num2 = Convert.ToDouble(Console.ReadLine());

        double ans = 0;

        switch (opChar)
        {
            case '+':
                ans = num1 + num2;
                break;
            case '-':
                ans = num1 - num2;
                break;
            case '*':
                ans = num1 * num2;
                break;
            case '/':
                if (num2 == 0)  //异常
                {
                    Console.WriteLine("异常：除数不能为零。");
                    return;
                }
                ans = num1 / num2;
                break;
            default:
                Console.WriteLine("无效！");
                return;
        }

        Console.WriteLine($"结果: {num1} {opChar} {num2} = {ans}");
    }
}

