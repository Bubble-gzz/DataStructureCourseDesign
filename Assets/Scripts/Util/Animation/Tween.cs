class Tween{
    static public float EaseOut(float x)
    {
        return 1-(x-1)*(x-1);
    }
    static public float EaseIn(float x)
    {
        return x*x;
    }
    static public float EaseInOut(float x)
    {
        if (x<0.5f) return 2*x*x;
        else return 1-2*(1-x)*(1-x);
    }
}