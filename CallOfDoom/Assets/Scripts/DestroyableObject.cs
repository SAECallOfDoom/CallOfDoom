using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public void ExplodeObject()
    {
        gameObject.AddComponent<TriangleExplosion>();
        StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
    }
}
