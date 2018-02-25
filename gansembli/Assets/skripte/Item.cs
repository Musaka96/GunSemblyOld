using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string itemName;
    public bool complete;
    public bool canHide;

    public int percentComplete;

    public int level; //1-3 ?
    public int levelLimit;

    public bool canHaveAmmo;
    public bool canHaveMagazine;
    public float secPerPercent = 0;
    private IEnumerator workCoroutine;
    public bool currentItemHasBar;

    void Start ()
    {
        // if its forgotten to set 
        // set default of 0.1 sec
        if (secPerPercent == 0) {
            secPerPercent = 0.1f;
        }

        workCoroutine = PerformWorking(secPerPercent);
        percentComplete = 0;
        complete = false;
    }

    void Update()
    {
        if (percentComplete == 100)
        {
            complete = true;
        }
    }

    public void Work()
    {
        print("zove work");
        StartCoroutine(workCoroutine);
    }

    public void stopWork()
    {
        StopCoroutine(workCoroutine);
    }

    IEnumerator PerformWorking(float timeToTick)
    {
        while (true) {
            if (!complete)
            {
                print("percent++");
                percentComplete = percentComplete + 1;
                yield return new WaitForSeconds(timeToTick);
            }
            else
            {
                print("else");
            }
        }
    }
}
