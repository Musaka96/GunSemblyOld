using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string item_name;
    public bool complete;
    public bool can_hide;

    public int percent_complete;

    public int level; //1-3 ?
    public int levelLimit;

    public bool canHaveAmmo;
    public bool canHaveMagazine;
    public float secPerPercent = 0.1f;
    private IEnumerator workCoroutine;
    public bool currentItemHasBar;

    void Start ()
    {
        workCoroutine = PerformWorking(secPerPercent);
        percent_complete = 0;
        complete = false;
    }

    void Update()
    {
        if (percent_complete == 100)
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
                percent_complete = percent_complete + 1;
                yield return new WaitForSeconds(timeToTick);
            }
            else
            {
                print("else");
            }
        }
    }
}
