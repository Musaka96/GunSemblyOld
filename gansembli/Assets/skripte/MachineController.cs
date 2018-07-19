using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineController : Machine {

    public Image progress_bar;

    public bool button_smash = false;
    public bool middle = false;

    protected float percent_complete = 0;
    private Item curr_item;

    //private int button_treshold = 0;
    public float decrease_per_tick = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        print(percent_complete);
        if (curr_item)
        {
            print(curr_item.percent_complete);
            print(curr_item.name);
        }

        if (percent_complete < 100)
        {
        // x += (target-x) * .1;
            if (button_smash)
            {
                percent_complete = percent_complete - decrease_per_tick * Time.fixedDeltaTime * (percent_complete * 0.1f);
            }
            progress_bar.fillAmount = percent_complete / 100f;
        }

        if(percent_complete >= 100 && !curr_item.complete)
        //if(percent_complete >= 100)
        {
            print("complete");
            percent_complete = 100;
            progress_bar.fillAmount = percent_complete / 100f;
            curr_item.percent_complete = 100;
            curr_item.complete = true;
        }

        progress_bar.transform.parent.parent.eulerAngles = new Vector3(
            Camera.main.transform.eulerAngles.x,
            Camera.main.transform.gameObject.transform.eulerAngles.y,
            transform.eulerAngles.z);
	}


    public void Action(Item item)
    {
        print("action");
        if (!item)
        {
            curr_item = null;
            percent_complete = 0;
        }
        else
        {
            curr_item = item;
            //percent_complete = item.percentComplete;
            percent_complete += 20;
        }
    }
}
