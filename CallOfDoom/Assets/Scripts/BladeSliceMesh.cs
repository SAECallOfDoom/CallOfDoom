using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class BladeSliceMesh : MonoBehaviour
{
    [SerializeField]
    private Transform cutPlane;

    [SerializeField]
    private LayerMask cuttableObjectMask;

    [SerializeField]
    private Material crossMaterial;

    [HideInInspector]
    public bool isBladeMode;

    private WeaponSwap weaponSwap;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwap = GetComponentInChildren<WeaponSwap>();
        isBladeMode = false;   
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        RotatePlane();
        

        if (isBladeMode && weaponSwap.isSwordActive)
        {
            SetTimeScale(0.5f);
            cutPlane.gameObject.SetActive(true);
        }
        
        else
        {
            SetTimeScale(1f);
            cutPlane.gameObject.SetActive(false);
        }


        if (isBladeMode && weaponSwap.isSwordActive)
        {
             if (Input.GetMouseButtonDown(0))
             {
                 Slice();
             }
        }

    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            isBladeMode = true;

        if (Input.GetKeyUp(KeyCode.LeftAlt))
            isBladeMode = false;
    }

    

    void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void RotatePlane()
    {
        cutPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }

    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(cutPlane.position, new Vector3(5, 0.1f, 5), cutPlane.rotation, cuttableObjectMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 14;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(cutPlane.position, cutPlane.up, crossSectionMaterial);
    }
}
