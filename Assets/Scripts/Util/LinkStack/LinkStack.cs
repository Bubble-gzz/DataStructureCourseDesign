#define LogInfo
    using System;
    using System.Threading;

    public class StackElement
    {
        public LinkStack stack;
        public int level;
        public bool exist;
        public int value;
        public StackElement below;
        

        public StackElement()
        {
            value = 0;
            below = null;
            exist = true;
            stack = null;
            level = 0;
        }

        public StackElement(int _value)
        {
            value = _value;
            below = null;
            exist = true;
            stack = null;
            level = 0;
        }
        public void Print()
        {
            Console.Write(value);
        }
    }
    public class LinkStack
    {
        private StackElement topElement;
        private int count, capacity;

        public enum PrintOption
        {
            Short,
            Full
        }
        public LinkStack()
        {
            count = 0;
            capacity = 100;
            topElement = null;
        }

        public LinkStack(int size)
        {
            count = 0;
            capacity = size;
            topElement = null;
        }

        public bool IsEmpty() {
            return count < 1;
        }

        public bool IsFull() {
            return count >= capacity;
        }
        public StackElement Pop(bool destroy = true)
        {
#if LogInfo
            Console.Write("Pop  ");        
#endif
            if (IsEmpty())
            {
                Console.WriteLine("Exception: The Stack is already empty.");
                return null;
            }

            StackElement result = topElement;
            if (destroy) result.exist = false;
            topElement = topElement.below;
            count--;
            return result;
        }

        public void Clear()
        {
            #if LogInfo
                Console.Write("Clear  ");        
            #endif
            while (!IsEmpty()) Pop();
        }
        public void Push(StackElement newElement)
        {
            #if LogInfo
                    Console.Write("Push {0}  ", newElement.value);        
            #endif
            if (IsFull())
            {
                Console.WriteLine("Exception: The Stack is already full.");
                return;
            }

            newElement.stack = this;
            newElement.level = count;
            newElement.below = topElement;
            topElement = newElement;
            count++;
        }

        public int Size()
        {
            return count;
        }
        public void Push(int value)
        {
            Push(new StackElement(value));
        }
        public StackElement Top()
        {
            return topElement;
        }

        private void PrintElement(StackElement element)
        {
            if (element == null) Console.Write("#");
            else element.Print();
        }
        public void Print(PrintOption option = PrintOption.Short)
        {
            StackElement[] a = new StackElement[capacity];
            int an = count - 1;
            for (StackElement cur = topElement; cur != null; cur = cur.below)
            {
                a[an] = cur;
                an--;
            }
            an = count;
            
            if (option == PrintOption.Full)
            {
                for (int i = count; i < capacity; i++)
                {
                    a[an] = null;
                    an++;
                }
            }
            Console.Write("[");
            for (int i = 0; i < an - 1; i++)
            {
                Console.Write(" ");
                PrintElement(a[i]);
                Console.Write(",");
            }
            Console.Write(" ");
            PrintElement(a[an-1]);
            if (option == PrintOption.Short) Console.WriteLine("]");
            else Console.WriteLine("] Room:{0}/{1}", count, capacity);
        }
    }
