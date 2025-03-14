// See https://aka.ms/new-console-template for more information
/*为示例中的泛型链表类添加类似于List<T>类的ForEach(Action<T> action)⽅法。通过调
⽤这个⽅法打印链表元素，求最⼤值、最⼩值和求和（使⽤lambda表达式实现）*/
using System;

public class Node<T>
{
    public Node<T> Next { get; set; }
    public T Value { get; set; }
    public Node(T val)
    {
        Next = null;
        Value = val;
    }
}
public class GenericList<T>
{
    private Node<T> head;
    private Node<T> tail;
    public GenericList()
    {
        tail = head = null;
    }
    public Node<T> Head
    {
        get => head;
    }
    public void Add(T t)
    {
        Node<T> n = new Node<T>(t);
        if(head==null)
        {
            head = tail = n;
        }
        else
        {
            tail.Next = n;
            tail = n;
        }
    }
    //ForEach(Action<T> action)方法
    public void ForEach(Action<T> action)
    {
        Node<T> cur = head;
        while(cur!=null)
        {
            action(cur.Value);
            cur = cur.Next;
        }
    }
}

class Program
{
    static void Main()
    {
        GenericList<int> genericList = new GenericList<int>();
        genericList.Add(1);
        genericList.Add(20);
        genericList.Add(300);
        genericList.Add(4000);
        //打印链表元素
        Console.WriteLine("链表元素：");
        //lambda表达式it => Console.Write(it + " ") 是 Action<int> 的实现
        genericList.ForEach(it => Console.WriteLine(it /*+ " "*/));
        Console.WriteLine();

        //求最大值
        int max = int.MinValue;
        genericList.ForEach(it => { if (it > max) max = it; });
        Console.WriteLine("最大值为：" + max);
        Console.WriteLine();

        //求最小值
        int min = int.MaxValue;
        genericList.ForEach(it => { if (it < min) min = it; });
        Console.WriteLine("最小值为：" + min);
        Console.WriteLine();

        //求和
        int sum = 0;
        genericList.ForEach(it => sum += it);
        Console.WriteLine("和为：" + sum);
        Console.WriteLine();
    }
}


