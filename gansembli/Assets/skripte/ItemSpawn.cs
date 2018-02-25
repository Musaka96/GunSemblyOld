using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : InteractableElement {

    public Item item;
    // maybe get blocket it item is put on top
    public bool canSpawn;

	// Use this for initialization
	void Start () {
        canSpawn = true;
        Type = "spawn";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Item getItem()
    {
        //animation 
        return item;

    }
}
