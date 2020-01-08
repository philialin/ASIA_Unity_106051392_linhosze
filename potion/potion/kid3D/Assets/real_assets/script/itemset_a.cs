using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemset_a : MonoBehaviour
{
    public bool cancatch;
    public Rigidbody rig;
    public GameObject pati;
    public Animator ani;

    public Collider coli;

    public GameObject target;

    private void Awake()
    {
        cancatch = true;
        rig.useGravity = true;
        rig.isKinematic = false;
        pati.SetActive(false);
        ani.enabled = false;
        coli.enabled = true;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 v = Vector3.Lerp(target.transform.position, transform.position, 1);
            Vector3 vv = (target.transform.position - v) * Time.deltaTime * 3;
            transform.position += vv;
        }
    }


    public void iitem()
    {
        target = null;

        cancatch = false;
        rig.useGravity = false;
        rig.isKinematic = true;
        pati.SetActive(true);
        ani.enabled = true;
        coli.enabled = false;
    }
}
