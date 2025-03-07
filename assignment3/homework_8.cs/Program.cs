// See https://aka.ms/new-console-template for more information
/*简单工厂模式使用一个工厂类来创建不同类型的对象实例。
 客户端只需要向工厂类请求对象，工厂类根据传入的参数选择合适的对象并返回。*/
using System;
using FactoryPattern;
namespace FactoryPattern
{
    public abstract class BaseShape
    {
        public abstract double CaculateArea();    //公开的计算面积的方法
        public abstract bool IsValid();    //公开的判断形状是否合理的方法
    }
    public class Rectangle : BaseShape
    {
        public double length { get; set; }
        public double width { get; set; }

        public Rectangle(double length, double width)
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
    public class Square : BaseShape
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

    public class Triangle : BaseShape
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
            return length1 > 0 && length2 > 0 && length3 > 0 && (length1 + length2 > length3) && (length1 + length3 > length2) && (length2 + length3 > length1);
        }
    }

    public class ShapeFactory
    {
        static Random random = new Random();  //生成随机数,要声明为static,否则为实例字段，而CreateShape是static方法，不能够直接访问实例字段，所以要声明为静态字段
        public static BaseShape CreateShape()
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    return new Rectangle(random.Next(1, 10), random.Next(1, 10));
                case 1:
                    return new Square(random.Next(1, 10));
                case 2:
                    return new Triangle(random.Next(1, 10), random.Next(1, 10), random.Next(1, 10));
                default:
                    throw new InvalidOperationException("NO");
            }
        }
    }
    class Program
    {
        public static void Main()
        {
            BaseShape[] baseShapes = new BaseShape[10];  //创建十个对象
            double AreaSum = 0;
            for (int i = 0; i < baseShapes.Length; i++)
            {
                baseShapes[i] = FactoryPattern.ShapeFactory.CreateShape(); //CreateShape要声明为静态函数成员，否则要先创建实例对象，才可调用
                if (baseShapes[i].IsValid())
                {
                    double area = baseShapes[i].CaculateArea();
                    AreaSum += area;
                    Console.WriteLine($"Shape {i + 1}: {baseShapes[i].GetType().Name}, Area:{area}");
                }
                else
                {
                    Console.WriteLine($"Shape {i + 1}: Invalid {baseShapes[i].GetType().Name}");
                }
            }
            Console.WriteLine($"AreaSum:{AreaSum}");
        }
    }
}

