// See https://aka.ms/new-console-template for more information
using System;

namespace Geometry
{
    //定义一个基础形状的抽象类
    public abstract class BaseShape  
    {
        public abstract double CaculateArea();    //公开的计算面积的方法
        public abstract bool IsValid();    //公开的判断形状是否合理的方法
    }
    public class Rectangle:BaseShape
    {
        public double length { get; set; }
        public double width { get; set; }

        public Rectangle(double length,double width)
        {
            this.length = length;
            this.width = width;
        }
        public override double CaculateArea()
        {
            //throw new NotImplementedException();
            return length * width; 
        }

        public override bool IsValid()
        {
            //throw new NotImplementedException();
            return length > 0 && width > 0;         
        }
    }
    public class Square:BaseShape
    {
        public double length { get; set; }

        public Square(double length)
        {
            this.length = length;
        }
        public override double CaculateArea()
        {
            //throw new NotImplementedException();
            return length * length;
        }

        public override bool IsValid()
        {
            //throw new NotImplementedException();
            return length > 0;
        }
    }

    public class Triangle:BaseShape
    {
        public double length1 { get; set; }
        public double length2 { get; set; }
        public double length3 { get; set; }

        public Triangle(double length1, double length2, double length3)
        {
            this.length1 = length1;
            this.length2 = length2;
            this.length3 = length3;
        }
        public override double CaculateArea()
        {
            //throw new NotImplementedException();
            double p = (length1 + length2 + length3) / 2;
            return Math.Sqrt(p * (p - length1) * (p - length2) * (p - length3));
        }

        public override bool IsValid()
        {
            //throw new NotImplementedException();
            return length1>0&&length2>0&&length3>0&&(length1 + length2 > length3) && (length1 + length3 > length2) && (length2 + length3 > length1);        
        }
    }
    class Program
    {
        static void Main()
        {
            BaseShape[] baseShapes =
            {
                new Rectangle(30.5,40.5),
                new Square(50.15),
                new Triangle(4,5,6),
                new Triangle(1,2,3)
            };

            foreach (var item in baseShapes)
            {
                Console.WriteLine($"{item.GetType().Name}");//输出类名
                Console.WriteLine($"是否合理：{item.IsValid()}");
                if(item.IsValid())
                {
                    Console.WriteLine($"面积为：{item.CaculateArea()}");
                }
                Console.WriteLine();
            }
        }
    }
}

