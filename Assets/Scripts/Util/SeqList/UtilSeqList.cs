#define LogInfo
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

    class SeqElement : DataElement
    {        public SeqList list;
        public int pos;
        private const int Inf = (int)2e9;
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
        override public Vector2 Position() {
            return new Vector2(list.x + list.interval * pos, list.y);
        }
        public void UpdatePos(int pos, bool order = true)
        {
            this.pos = pos;
            x = list.x + list.interval * pos;
            y = list.y;
            if (image == null) return ;
            list.animationBuffer.Add(new UpdatePosAnimatorInfo(image, new Vector2(x, y), order));
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
        public VisualizedPointer pointer_i, pointer_j, pointer_pivot, pointer_l, pointer_r;
        bool noPointer;
        private void Wait(float sec, bool useSetting = true)
        {
            animationBuffer.Add(new WaitAnimatorInfo(image, sec, useSetting));
            Debug.Log("useSetting : " + useSetting + "  timeScale : " + Settings.animationTimeScale);
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
        private void AddElement(SeqElement newElement, int pos, float delay)
        {
            newElement.list = this;
            newElement.animationBuffer = this.animationBuffer;
            newElement.UpdatePos(pos, false);
            Wait(delay, false);
            newElement.PopOut();
        }
        public void Append(SeqElement newElement)
        {
#if LogInfo
            Console.Write("Append({0}) ", newElement.value);
#endif
            if (!RoomAvailable()) return;
            array[count] = newElement;
            AddElement(newElement, count, 0.03f);
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
                Wait(0.07f, false);
            }
            array[pos] = newElement;
            
            AddElement(newElement, pos, 0.05f);
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
            Wait(0.2f, false);
            for (int i = pos; i < count - 1; i++)
            {
                MoveFromTo(i + 1, i);
                Wait(0.07f, false);
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
        public void UpdatePointerPos(VisualizedPointer pointer, Vector2 pos0, bool animated)
        {
            if (noPointer) return;
            if (pointer == null) return;
            Vector2 pos = pointer.CalcPos(pos0);
            animationBuffer.Add(new UpdatePosAnimatorInfo(pointer.gameObject, pos, animated));
        }
        public void PointerAppear(VisualizedPointer pointer)
        {
            if (noPointer) return;
            if (pointer == null) return;
            //if (Mathf.Abs(Settings.animationTimeScale) < 0.001f) return;
            animationBuffer.Add(new PopAnimatorInfo(pointer.gameObject, PopAnimator.Type.Appear));
        }
        public void PointerDisappear(VisualizedPointer pointer)
        {
            if (noPointer) return;
            if (pointer == null) return;
            animationBuffer.Add(new PopAnimatorInfo(pointer.gameObject, PopAnimator.Type.Disappear));
        }
        private void WakeUpPointer(VisualizedPointer pointer, Vector2 offset, bool appear = true)
        {
            if (noPointer) return;
            if (pointer == null) return;
            if (appear) PointerAppear(pointer);
            pointer.offset = offset;
        }
        private void SetSortPointer()
        {
            if (Mathf.Abs(Settings.animationTimeScale) < 0.005f) {
                noPointer = true;
                return;
            }
            noPointer = false;
            float h = 0.3f, v = 1.1f;
            WakeUpPointer(pointer_i, new Vector2(-h, v), false);
            WakeUpPointer(pointer_j, new Vector2(h, v), false);
            WakeUpPointer(pointer_pivot, new Vector2(0, v));
            WakeUpPointer(pointer_l, new Vector2(-h, -v));
            WakeUpPointer(pointer_r, new Vector2(h, -v));
            UpdatePointerPos(pointer_i, array[0].Position(), false);
            UpdatePointerPos(pointer_j, array[0].Position(), false);
            UpdatePointerPos(pointer_pivot, array[(count-1)/2].Position(), false);
            UpdatePointerPos(pointer_l, array[0].Position(), false);
            UpdatePointerPos(pointer_r, array[count - 1].Position(), false);
        }
        public void Sort(bool animated = true)
        {
            if (animated) SetSortPointer();
            QuickSort(0, count - 1);
            PointerDisappear(pointer_pivot);
            PointerDisappear(pointer_l);
            PointerDisappear(pointer_r);

            isOrdered = true;
        }
        void SetPointer(ref int pointer, int target)
        {
            if (pointer >= 0 && pointer < Size() && pointer != target)
                array[pointer].SetColor(Palette.Normal);
            pointer = target;
            array[pointer].Highlight(true, Palette.Pointed);
        }
        private void QuickSort(int l, int r)
        {
            if (l >= r) return;
            UpdatePointerPos(pointer_l, array[l].Position(), true);
            UpdatePointerPos(pointer_r, array[r].Position(), true);
            UpdatePointerPos(pointer_pivot, array[(l+r)/2].Position(), true);
            array[(l+r)/2].Highlight(true, Palette.Pivot);
            Wait(1);
            if (l!=(l+r)/2) {
                Swap(l, (l+r)/2);
            }
            UpdatePointerPos(pointer_pivot, array[l].Position(), true);
            Wait(1);

            SeqElement pivot = array[l];

            int i = -1, j = -1;
            SetPointer(ref i, l + 1);
            UpdatePointerPos(pointer_i, array[i].Position(), false);
            SetPointer(ref j, r);
            UpdatePointerPos(pointer_j, array[j].Position(), false);
            PointerAppear(pointer_i);
            PointerAppear(pointer_j);
            Wait(1);
            while (i < j)
            {
                while (i < j && array[i].value <= pivot.value) {
                    SetPointer(ref i, i + 1);
                    UpdatePointerPos(pointer_i, array[i].Position(), true);
                    Wait(1);
                }
                while (i < j && array[j].value >= pivot.value) {
                    SetPointer(ref j, j - 1);
                    UpdatePointerPos(pointer_j, array[j].Position(), true);
                    Wait(1);
                }
                Swap(i, j);
                array[i].SetColor(Palette.Normal);
                array[j].SetColor(Palette.Normal);
                Wait(1);
            }
            array[i].SetColor(Palette.Normal);
            array[j].SetColor(Palette.Normal);

            PointerDisappear(pointer_j);
            
            int mid = -1;
            SetPointer(ref mid, i);
            Wait(1);
            if (array[mid].value > pivot.value) {
                SetPointer(ref mid, mid - 1);
                UpdatePointerPos(pointer_i, array[i - 1].Position(), true);
                Wait(1);
            }
            Swap(l, mid);
            Wait(1);
            array[mid].SetColor(Palette.Normal);
            array[l].SetColor(Palette.Normal);
            PointerDisappear(pointer_i);
            Wait(1);
            QuickSort(l, mid - 1);
            QuickSort(mid + 1, r);
        }
        
    }
