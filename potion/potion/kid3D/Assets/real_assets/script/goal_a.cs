using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal_a : MonoBehaviour
{
    private int n = 0;

    public GameObject cam;
    public GameObject point;
    public GameObject tt;
    public GameObject wt;
    public GameObject c;
    public Animator cani;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item")
        {
            other.transform.parent.parent.GetComponent<itemset_a>().iitem();
            n++;
            if (n >= 3)
            {
                cam.transform.position = point.transform.position;
                cam.transform.rotation = point.transform.rotation;
                cam.GetComponent<camaset>().enabled = false;
                tt.SetActive(false);
                wt.SetActive(true);
                c.SetActive(true);
                cani.enabled = true;

            }
        }
    }
}
