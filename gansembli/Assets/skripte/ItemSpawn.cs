using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : InteractableElement {

    public GameObject item;
    // maybe get blocket it item is put on top
    public bool _canSpawn;

	// Use this for initialization
	void Start () {
        _canSpawn = true;
        Type = "spawn";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject getItem()
    {
        //animation 
        Instantiate(item);
        return item;

    }
}
