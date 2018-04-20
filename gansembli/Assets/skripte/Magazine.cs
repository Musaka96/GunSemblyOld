using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : Item {

    public float capacity;
    public Image progress_bar;

    private Item item;

	// Use this for initialization
	void Start () {
        item = GetComponentInParent<Item>();
        if(item)
        {
            item.currentItemHasBar = true;
            progress_bar.fillAmount = item.percent_complete / 100f;
        }

	}
	
	// Update is called once per frame
	void Update () {

        progress_bar.fillAmount = 0f;
        percent_complete = item.percent_complete;
        float completeToFillAmount = 0f;


        if (percent_complete != 0)
        {
            completeToFillAmount = percent_complete / 100f;
        }

        if(percent_complete == 100)
        {
            complete = true;
        }

        if (percent_complete > 0) {
            progress_bar.fillAmount = completeToFillAmount;
        } else {
            complete = false;
            progress_bar.fillAmount = 0;
        }

        // bar rotation stuff
        progress_bar.transform.parent.parent.eulerAngles = new Vector3(
            Camera.main.transform.eulerAngles.x,
            Camera.main.transform.gameObject.transform.eulerAngles.y,
            transform.eulerAngles.z);

	}
}
