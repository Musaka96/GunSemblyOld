using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interakcija : MonoBehaviour {
    public static string objectTag { get; set; }
    public static bool Triggered { get; set; }
    public bool isTable;

    public Item holdingItem;
    public Item currentCollidedItem;

    public GameObject zaSpawn;
    public GameObject currentItem;
    public GameObject currentCollidedObject;

    public ObjectColor glowObject;
    public SimpleCharacterControl characterController;

    // Use this for initialization
    void Start () {
        characterController = GetComponentInParent<SimpleCharacterControl>();
        if (currentItem)
        {
            GameObject spawn_object = this.gameObject.transform.Find("SpawnPoint").gameObject;
            Instantiate(currentItem, spawn_object.transform.position, Quaternion.identity, this.gameObject.transform);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.GetComponent<InteractableElement>() && !Triggered)
        {
            Triggered = true;

            currentCollidedObject = collidedObject.gameObject;
            currentCollidedItem = currentCollidedObject.GetComponentInChildren<Item>();

            glowObject = currentCollidedObject.GetComponentInChildren<ObjectColor>();
            if (glowObject)
            {
                glowObject.GlowOnInteract();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Triggered = false;
        if (currentCollidedObject && currentCollidedObject.GetComponent<InteractableElement>().Type == "Table")
            currentCollidedObject.GetComponentInChildren<ObjectColor>().StopInteractGlow();

        // if the character was working on something
        if (characterController.isWorking)
        {
            stopWork();
        }
        currentCollidedObject = null;
        currentCollidedItem = null;
    }

    public void Interact()
    {
        InteractableElement skript = currentCollidedObject.GetComponent<InteractableElement>();
        if (skript)
        {
            if(skript.Type == "Table")
            {
                Table table = currentCollidedObject.GetComponent<Table>();
                if(!table.isOccupied)
                {
                    //leave item
                    if (currentItem)
                    {
                        table.SpawnObject(currentItem);
                        currentItem = null;
                        holdingItem = null;
                        
                        Item item = GetComponentInChildren<Item>();
                        Destroy(item.gameObject);
                    }
                } else {
                    //take item
                    if (!currentItem)
                    {
                        takeItem(table.TakeObject());
                    }
                }
            }
            if(skript.Type == "spawn")
            {
                ItemSpawn spawn = currentCollidedObject.GetComponent<ItemSpawn>();
                if (spawn.canSpawn && !currentItem)
                {
                    takeItem(spawn.getItem());
                }
            }
        }
    }

    private void takeItem(GameObject itemObject)
    {
        print("take item");
        currentItem = itemObject;
        holdingItem = itemObject.GetComponent<Item>();

        Vector3 spawn_object_position = this.gameObject.transform.Find("SpawnPoint").gameObject.transform.position;
        Instantiate(itemObject, spawn_object_position, Quaternion.identity, this.gameObject.transform);
    }
    /// <summary>
    /// Takes item from parameter, and spawns it in hands, maybe do an animation soon?
    /// </summary>
    /// <param name="item"></param>
    private void takeItem(Item item)
    {
        currentItem = item.gameObject;
        holdingItem = item;

        Vector3 spawn_object_position = this.gameObject.transform.Find("SpawnPoint").gameObject.transform.position;
        Instantiate(currentItem, spawn_object_position, Quaternion.identity, this.gameObject.transform);

    }

    public void Work()
    {
            print("work interakcija");
            if (currentCollidedItem && !currentCollidedItem.complete)
            {
                print("zove work interakcija");
                currentCollidedItem.Work();
            }
    }
    public void stopWork()
    {
            if (currentCollidedItem && !currentCollidedItem.complete)
            {
                currentCollidedItem.stopWork();
            }
    }


    public bool getTriggered()
    {
        return Triggered;
    }
}
