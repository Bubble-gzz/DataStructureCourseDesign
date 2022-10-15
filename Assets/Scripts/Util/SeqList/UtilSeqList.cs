#define LogInfo
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

    class SeqElement
    {
        public float value;
        public SeqList list;
        public int pos;
        public bool exist;
        public object myObject;
        private const int Inf = (int)2e9;
        private float x, y;
//        public VisualizedSeqElement image;
        public GameObject image;
        public VisualizedSeqElement imageInfo;
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
        public void UpdatePos(int pos, bool order = true)
        {
            this.pos = pos;
            x = list.x + list.interval * pos;
            y = list.y;
            if (image == null) return ;
            list.animationBuffer.Add(new UpdatePosAnimatorInfo(image, new Vector2(x, y), order));
        }
        public void PopOut()
        {
            if (image == null) return ;
            list.animationBuffer.Add(new PopAnimatorInfo(image, PopAnimator.Type.Appear));
        }
        public void SetColor(VisualizedSeqElement.ColorType colorType, bool order = true)
        {
            if (image == null) return ;
            list.animationBuffer.Add(new ChangeColorAnimatorInfo(image, imageInfo.colors[(int)colorType], order));
        }
        public void Highlight(bool pop, VisualizedSeqElement.ColorType colorType)
        {
            if (image == null) return ;
            if (pop) list.animationBuffer.Add(new PopAnimatorInfo(image, PopAnimator.Type.Emphasize));
            SetColor(colorType);
        }
        public void UpdateValue(float value)
        {
            this.value = value;
            if (image == null) return ;
            list.animationBuffer.Add(new ChangeTextAnimatorInfo(image, value.ToString("f0")));
        }
        public void Destroy()
        {
            this.exist = false;
            if (image == null) return ;
            list.animationBuffer.Add(new SelfDestroyAnimatorInfo(image, true));
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
    class SeqList
    {
        public bool isOrdered;
        private int count, capacity;
        private SeqElement[] array;
        public float x, y, interval;
        public AnimationBuffer animationBuffer;
        public GameObject image;
        private void Wait(float sec)
        {
            animationBuffer.Add(new WaitAnimatorInfo(image, sec));
            //Wait(sec);
        }
        public void MoveFromTo(int i, int j)
        {
            array[j] = array[i];
            if (array[i] == null) return;
            array[i].UpdatePos(j);
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
            interval = 1.5f;
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
           // Debug.Log("Append");
            newElement.UpdatePos(count, false);
            Wait(0.03f);
            newElement.PopOut();
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
            {
                MoveFromTo(i-1, i);
                Wait(0.07f);
            }
            array[pos] = newElement;
            
            newElement.list = this;
            Wait(0.05f);
            newElement.UpdatePos(pos, false);
            newElement.PopOut();
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
            if (destroy) array[pos].Destroy();
            Wait(0.2f);
            for (int i = pos; i < count - 1; i++)
            {
                MoveFromTo(i + 1, i);
                Wait(0.07f);
            }
            count--;
        }

        public void Clear()
        {
#if LogInfo
            Console.Write("Clear ");
#endif
            for (int i = 0; i < count; i++)
            {
                array[i].Destroy();
                array[i] = null;
            }
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
            array[i].UpdatePos(i); array[j].UpdatePos(j);
        }

        public void Sort(float sortDelay)
        {
            QuickSort(0, count - 1, sortDelay);
            isOrdered = true;
        }
        void SetPointer(ref int pointer, int target)
        {
            if (pointer >= 0 && pointer < Size() && pointer != target)
                array[pointer].SetColor(VisualizedSeqElement.ColorType.Normal);
            pointer = target;
            array[pointer].Highlight(true, VisualizedSeqElement.ColorType.Pointed);
        }
        float sortDelay;
        private void QuickSort(int l, int r, float sortDelay)
        {
            if (l >= r) return;

            array[l].Highlight(true, VisualizedSeqElement.ColorType.Pointed);
            if ((l+r)/2 != l) array[(l+r)/2].Highlight(true, VisualizedSeqElement.ColorType.Pointed);
            Wait(sortDelay);
            Swap(l, (l+r)/2);
            Wait(sortDelay);
            array[l].SetColor(VisualizedSeqElement.ColorType.Normal);
            if ((l+r)/2 != l) array[(l+r)/2].SetColor(VisualizedSeqElement.ColorType.Normal);

            SeqElement pivot = array[l];
            array[l].Highlight(true, VisualizedSeqElement.ColorType.Pivot);
            Wait(sortDelay);
            int i = -1, j = -1;
            SetPointer(ref i, l + 1);
            SetPointer(ref j, r);
            Wait(sortDelay);
            while (i < j)
            {
                while (i < j && array[i].value <= pivot.value) {
                    SetPointer(ref i, i + 1);
                    Wait(sortDelay);
                }
                while (i < j && array[j].value >= pivot.value) {
                    SetPointer(ref j, j - 1);
                    Wait(sortDelay);
                }
                Swap(i, j);
                array[i].SetColor(VisualizedSeqElement.ColorType.Normal);
                array[j].SetColor(VisualizedSeqElement.ColorType.Normal);
                Wait(sortDelay);
            }
            array[i].SetColor(VisualizedSeqElement.ColorType.Normal);
            array[j].SetColor(VisualizedSeqElement.ColorType.Normal);
            int mid = i;
            if (array[mid].value > pivot.value) mid--;
            array[l].SetColor(VisualizedSeqElement.ColorType.Normal);
            Swap(l, mid);
            Wait(sortDelay);
            QuickSort(l, mid - 1, sortDelay);
            QuickSort(mid + 1, r, sortDelay);
        }
        
    }
