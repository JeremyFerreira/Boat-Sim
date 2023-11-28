using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtension
{
    public static void DestroyChildren(this GameObject t)
    {
        t.transform.Cast<Transform>().ToList().ForEach(c => Object.Destroy(c.gameObject));
    }

    public static void DestroyChildrenImmediate(this GameObject t)
    {
        t.transform.Cast<Transform>().ToList().ForEach(c => Object.DestroyImmediate(c.gameObject));
    }
}
