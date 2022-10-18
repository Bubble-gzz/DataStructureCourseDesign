using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 public class DataElement
    {
        public float value;
        public bool exist;
        public object myObject;
        protected float x, y;
        public GameObject image;
        public AnimationBuffer animationBuffer;
        public List<Color> colors;

        virtual public Vector2 Position() {
            return new Vector2(0, 0); 
        }
        public void PopOut()
        {
            if (image == null) return ;
            animationBuffer.Add(new PopAnimatorInfo(image, PopAnimator.Type.Appear));
        }
        public void SetColor(int colorType, bool animated = true)
        {
            if (image == null) return ;
            animationBuffer.Add(new ChangeColorAnimatorInfo(image, colors[colorType], animated));
        }
        public void Highlight(bool pop, int colorType, bool widthOnly = false)
        {
            if (image == null) return ;
            PopAnimator.Type type = PopAnimator.Type.Emphasize;
            if (widthOnly) type = PopAnimator.Type.EmphasizeLine;
            if (pop) animationBuffer.Add(new PopAnimatorInfo(image, type));
            SetColor(colorType);
        }
        public void UpdateValue(float value)
        {
            this.value = value;
            if (image == null) return ;
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, value.ToString("f0")));
        }
        public void SetText(string newText)
        {
            if (image == null) return;
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, newText));
        }
        public void Destroy()
        {
            this.exist = false;
            if (image == null) return ;
            animationBuffer.Add(new SelfDestroyAnimatorInfo(image, true));
        }
    }