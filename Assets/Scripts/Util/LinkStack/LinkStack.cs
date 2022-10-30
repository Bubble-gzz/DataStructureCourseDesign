#define LogInfo
    using System;
    using System.Threading;
    using UnityEngine;

    public class StackElement : DataElement
    {
        public LinkStack stack;
        public int level;
        public StackElement below;
        public int bracketID;
    
        public StackElement()
        {
            value = 0;
            below = null;
            exist = true;
            stack = null;
            level = 0;
        }

        public StackElement(int _value, int _level = 0)
        {
            this.value = _value;
            this.level = _level;
            below = null;
            exist = true;
            stack = null;

        }
        override public Vector2 Position() {
            //Debug.Log("stack.pos : " + stack.pos);
            return new Vector2(stack.pos.x, stack.pos.y + stack.interval * level);
        }
        public void UpdatePos(bool animated = true)
        {
            if (image == null) return ;
            stack.animationBuffer.Add(new UpdatePosAnimatorInfo(image, Position(), animated));
            Debug.Log("UpdatePos : " + Position());
        }
        public void Print()
        {
            Console.Write(value);
        }
        public static bool operator < (StackElement A, StackElement B)
        {
            return A.value < B.value;
        }
        public static bool operator <= (StackElement A, StackElement B)
        {
            return A.value <= B.value;
        }
        public static bool operator >= (StackElement A, StackElement B)
        {
            return A.value >= B.value;
        }
        public static bool operator > (StackElement A, StackElement B)
        {
            return A.value > B.value;
        }
    }
    public class LinkStack
    {
        private StackElement topElement;
        private int count, capacity;
        public Vector2 pos;
        public float interval = 0.7f;
        public VisualizedPointer pointer_top;
        public AnimationBuffer animationBuffer;
        public GameObject image;
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
        public Vector2 CalcPos(int level) {
            //Debug.Log("stack.pos : " + stack.pos);
            return new Vector2(pos.x, pos.y + interval * level);
        }

        public bool IsEmpty() {
            return count < 1;
        }

        public bool IsFull() {
            return count >= capacity;
        }
        private void Wait(float sec, bool useSetting = true)
        {
            animationBuffer.Add(new WaitAnimatorInfo(image, sec, useSetting));
        }

        public StackElement Pop(float delay = 0.24f, bool destroy = true)
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
            if (destroy) {
                result.exist = false;
                result.image.GetComponent<VisualizedStackElement>().alive = false;
                result.Destroy();
            }
            topElement = topElement.below;
            count--;
            Wait(delay, false);
            pointer_top.ChangePos(CalcPos(count - 1), false, true);
            return result;
        }

        public void Clear()
        {
            #if LogInfo
                Console.Write("Clear  ");        
            #endif
            while (!IsEmpty()) Pop(0.1f);
        }
        public StackElement Push(StackElement newElement)
        {
            #if LogInfo
                    Console.Write("Push {0}  ", newElement.value);        
            #endif
            if (IsFull())
            {
                Console.WriteLine("Exception: The Stack is already full.");
                return null;
            }

            newElement.stack = this;
            newElement.level = count;
            newElement.below = topElement;
            newElement.animationBuffer = this.animationBuffer;
            Debug.Log("StackElement animationBuffer : " + newElement.animationBuffer.Name);
            topElement = newElement;
            newElement.UpdatePos();
            newElement.PopOut();
            count++;
            pointer_top.ChangePos(CalcPos(count - 1), false);
            return newElement;
        }

        public int Size()
        {
            return count;
        }
        public StackElement Push(int value)
        {
            return Push(new StackElement(value));
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
