#define LogInfo
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace  DataStructureCourseDesign.SeqListSpace
{
    public class SeqElement
    {
        public float value;
        public SeqList list;
        public int pos;
        public bool exist;
        public object myObject;
        private const int Inf = (int)2e9;
        private float x, y;
        
        public SeqElement()
        {
            value = 0;
            list = null;
            pos = 0;
            exist = true;
        }

        public SeqElement(float _value, object _myObject = null)
        {
            value = _value;
            myObject = _myObject;
            list = null;
            pos = 0;
            exist = true;
        }

        public static SeqElement BigConst()
        {
            return new SeqElement(Inf);
        }
        public static SeqElement SmallConst()
        {
            return new SeqElement(-Inf);
        }
        public void UpdatePos()
        {
            
        }

        public static bool operator < (SeqElement A, SeqElement B)
        {
            return A.value < B.value;
        }
        public static bool operator <=(SeqElement A, SeqElement B)
        {
            return A.value <= B.value;
        }
        public static bool operator >= (SeqElement A, SeqElement B)
        {
            return A.value >= B.value;
        }
        public static bool operator > (SeqElement A, SeqElement B)
        {
            return A.value > B.value;
        }
    }
    public class SeqList
    {
        public bool isOrdered;
        private int count, capacity;
        private SeqElement[] array;
        private float x, y;

        public void MoveFromTo(int i, int j)
        {
            array[j] = array[i];
            if (array[i] == null) return;
            array[i].pos = j;
            array[i].UpdatePos();
        }
        
        public enum PrintOption
        {
            Short,
            Full
        }

        public SeqList(int size = 100)
        {
            capacity = size;
            array = new SeqElement[capacity + 1];
            count = 0;
            isOrdered = false;
        }
        public int Size()
        {
            return count;
        }

        public bool IsEmpty()
        {
            return count == 0;
        }
        private bool RoomAvailable()
        {
            if (count >= capacity)
            {
                Console.WriteLine("Exception: List is full!");
                return false;               
            }

            return true;
        }

        private bool IsPositionValid(int pos)
        {
            if (pos < 0 || pos >= count)
            {
                Console.WriteLine("Exception: Position is out of range! It should be in [{0},{1})",0,count);
                return false;
            }

            return true;
        }
        public void Append(SeqElement newElement)
        {
#if LogInfo
            Console.Write("Append({0}) ", newElement.value);
#endif
            if (!RoomAvailable()) return;
            array[count] = newElement;
            newElement.list = this;
            newElement.pos = count;
            count++;
        }

        public void Insert(int pos, SeqElement newElement)
        {
            #if LogInfo
                Console.Write("Insert( {0}, {1}) ", pos, newElement.value);
            #endif
            if (!RoomAvailable()) return;
            if (!IsPositionValid(pos)) return;
            for (int i = count; i > pos; i--)
                MoveFromTo(i-1, i);
            array[pos] = newElement;
            array[pos].list = this;
            count++;
        }

        public void Insert(SeqElement newElement)
        {
            if (!isOrdered)
            {
                Console.WriteLine("Unordered list, please specify the position to insert at.");
                return;
            }

            for (int i = 0; i < count; i++)
                if (newElement.value <= array[i].value)
                {
                    Insert(i, newElement);
                    return;
                }
            Append(newElement);
    }
        
        public void Delete(int pos, bool destroy = true)
        {
#if LogInfo
            Console.Write("Delete({0}) ", pos);
#endif
            if (!IsPositionValid(pos)) return;
            if (destroy) array[pos].exist = false;
            for (int i = pos; i < count - 1; i++)
                MoveFromTo(i + 1, i);
            count--;
        }

        public void Clear()
        {
#if LogInfo
            Console.Write("Clear ");
#endif
            for (int i = 0; i < count; i++)
                array[i].exist = false;
            count = 0;
        }

        public SeqElement GetElement(int pos)
        {
            if (!IsPositionValid(pos)) return null;
            return array[pos];
        }

        public void Print(PrintOption option = PrintOption.Short)
        {
            if (option == PrintOption.Short)
            {
                Console.Write("[");
                for (int i = 0; i < count - 1; i++)
                    Console.Write(" {0},", array[i].value);
                Console.WriteLine(" {0}]", array[count - 1].value);   
            }
            else
            {
                Console.Write("[");
                for (int i = 0; i < capacity; i++)
                {       
                    if (i < count) Console.Write(" {0}", array[i].value);
                    else Console.Write(" #");
                    if (i < capacity - 1) Console.Write(",");
                    else Console.WriteLine("] Room:{0}/{1}", count, capacity);
                } 
            }
        }
        public static SeqList Merge(SeqList A, SeqList B)
        {
            if (!A.isOrdered || !B.isOrdered)
            {
                Console.WriteLine("Cannot merge unordered lists.");
                return null;
            }
            SeqList res = new SeqList(A.Size() + B.Size());
            res.isOrdered = true;
            while (!A.IsEmpty() || !B.IsEmpty())
            {
                SeqElement A0, B0;
                if (A.IsEmpty()) A0 = SeqElement.BigConst();
                else A0 = A.GetElement(0);
                if (B.IsEmpty()) B0 = SeqElement.BigConst();
                else B0 = B.GetElement(0);
                if (A0 <= B0)
                {
                    res.Append(A0);
                    A.Delete(0);
                }
                else
                {
                    res.Append(B0);
                    B.Delete(0);
                }
            }

            return res;
        }
        public int BinarySearch(SeqElement target)
        {
            if (!isOrdered)
            {
                Console.WriteLine("Cannot do binary search on an unordered list.");
                return -1;
            }

            int l = 0, r = count - 1, mid;
            while (l <= r)
            {
                mid = (l + r) / 2;
                if (Math.Abs(array[mid].value - target.value) < 1e-6) return mid;
                if (array[mid].value < target.value) l = mid + 1;
                else r = mid - 1;
            }

            return -1;
        }

        public void Swap(int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        public void Sort()
        {
            QuickSort(0, count - 1);
            isOrdered = true;
        }
        private void QuickSort(int l, int r)
        {
            if (l >= r) return;
            Swap(l, (l+r)/2);
            SeqElement pivot = array[l];
            int i = l, j = r;
            while (i < j)
            {
                while (i < j && array[i].value <= pivot.value) i++;
                while (i < j && array[j].value >= pivot.value) j--;
                Swap(i, j);
            }

            int mid = i;
            if (array[mid].value > pivot.value) mid--;
            Swap(l, mid);
            QuickSort(l, mid - 1);
            QuickSort(mid + 1, r);
        }
    }
}
