using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractableElement {

    public bool isOccupied;
    private Item currentItem;
    public bool isHideable;
    public GameObject currentObject;


	// Use this for initialization
	void Start () {
        isOccupied = false;
        currentItem = null;

        if (currentObject)
        {
            SpawnObject(currentObject);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnObject(GameObject objekat)
    {
        print("dodje dodje");
        GameObject spawn_object = this.gameObject.transform.Find("SpawnPoint").gameObject;
        Instantiate(objekat, spawn_object.transform.position, Quaternion.identity, this.gameObject.transform);
        currentObject = objekat;

        isOccupied = true;
    }

    public GameObject TakeObject()
    {
        Item item = GetComponentInChildren<Item>();
        Destroy(item.gameObject);
        isOccupied = false;
        return currentObject;

        
    }

    public void IsTriggered(bool isTriggered)
    {

    }
}
