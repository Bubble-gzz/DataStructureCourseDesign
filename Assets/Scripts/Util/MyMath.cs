using UnityEngine;
class MyMath{
    static public Vector2 LocaltoWorldPosition(Transform transform, Vector2 localPosition)
    {
        Vector2 originPosition = transform.position;
        transform.localPosition = localPosition;
        Vector2 result = transform.position;
        transform.position = originPosition;
        return result;
    }
}