using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 public class DataElement
    {
        public float value;
        public bool exist;
        public object myObject;
        public float x, y;
        public GameObject image;
        public AnimationBuffer animationBuffer;
        public List<Color> colors;

        virtual public Vector2 Position() {
            return new Vector2(0, 0); 
        }
        public void PopOut(GameObject image = null)
        {
            if (image == null) image = this.image;
            if (image == null) return ;
            animationBuffer.Add(new PopAnimatorInfo(image, PopAnimator.Type.Appear));
        }
        public void SetColor(int colorType, bool animated = true, GameObject image = null)
        {
            if (image == null) image = this.image;
            if (image == null) return ;
            animationBuffer.Add(new ChangeColorAnimatorInfo(image, colors[colorType], animated));
        }
        public void Highlight(bool pop, int colorType, bool widthOnly = false, GameObject image = null)
        {
            if (image == null) image = this.image;            
            if (image == null) return ;
            PopAnimator.Type type = PopAnimator.Type.Emphasize;
            if (pop) animationBuffer.Add(new PopAnimatorInfo(image, type));
            SetColor(colorType);
        }
        public void UpdateValue(float value)
        {
            this.value = value;
            if (image == null) return ;
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, value.ToString("f0")));
        }
        public void SetText(string newText, bool animated = false)
        {
            if (image == null) return;
            animationBuffer.Add(new ChangeTextAnimatorInfo(image, newText));
        }
        public void Destroy(bool widthOnly = false)
        {
            this.exist = false;
            if (image == null) return ;
            animationBuffer.Add(new SelfDestroyAnimatorInfo(image, true, widthOnly));
        }
    }