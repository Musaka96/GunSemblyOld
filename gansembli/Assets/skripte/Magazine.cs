using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : Item {

    public float capacity;
    public float currentCapacity;

    public Image progressBar;
    private Item item;

	// Use this for initialization
	void Start () {
        item = GetComponentInParent<Item>();
        if(item)
        {
            item.currentItemHasBar = true;
        }

	}
	
	// Update is called once per frame
	void Update () {

        percentComplete = item.percentComplete;
        print("percentConmp" + percentComplete);
        float completeToFillAmount = 0f;


        if (percentComplete != 0)
        {
            completeToFillAmount = percentComplete / 100f;
        }

        if(completeToFillAmount == 100)
        {
            complete = true;
        }
        print("update magazin" + completeToFillAmount);
        if (completeToFillAmount > 0) {
            //float fillAmout = currentCapacity * complete;
            //print("fillAmout" + fillAmout);
            print("complete");
            progressBar.fillAmount = completeToFillAmount;
        } else {
            complete = false;
            progressBar.fillAmount = 0;
        }

        // bar rotation stuff
        progressBar.transform.parent.parent.eulerAngles = new Vector3(
            Camera.main.transform.eulerAngles.x,
            Camera.main.transform.gameObject.transform.eulerAngles.y,
            transform.eulerAngles.z);

	}
}
