using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interakcija : MonoBehaviour {
    public static string objectTag { get; set; }
    public static bool _triggered { get; set; }
    public bool is_table;

    public Item _holding_item;
    public Item _current_collided_item;

    //public GameObject zaSpawn;
    public GameObject _current_item;
    public GameObject _current_collided_object;

    public ObjectColor glowObject;
    public SimpleCharacterControl characterController;
    private Machine _curr_machine;

    // Use this for initialization
    void Start () {
        characterController = GetComponentInParent<SimpleCharacterControl>();
        if (_current_item)
        {
            GameObject spawn_object = this.gameObject.transform.Find("SpawnPoint").gameObject;
            Instantiate(_current_item, spawn_object.transform.position, Quaternion.identity, this.gameObject.transform);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.GetComponent<InteractableElement>() && !_triggered)
        {
            _triggered = true;

            _current_collided_object = collidedObject.gameObject;
            _current_collided_item = _current_collided_object.GetComponentInChildren<Item>();
            _curr_machine = _current_collided_object.GetComponentInChildren<Machine>();
            print(_curr_machine);

            glowObject = _current_collided_object.GetComponentInChildren<ObjectColor>();
            if (glowObject)
            {
                glowObject.GlowOnInteract();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _triggered = false;
        if (_current_collided_object && _current_collided_object.GetComponent<InteractableElement>().Type == "Table")
            _current_collided_object.GetComponentInChildren<ObjectColor>().StopInteractGlow();

        // if the character was working on something
        if (characterController.isWorking)
        {
            stopWork();
        }
        _current_collided_object = null;
        _current_collided_item = null;
    }

    public void Interact()
    {
        InteractableElement skript = _current_collided_object.GetComponent<InteractableElement>();
        if (skript)
        {
            if(skript.Type == "Table")
            {
                Table table = _current_collided_object.GetComponent<Table>();
                if(!table.isOccupied)
                {
                    //leave item
                    if (_current_item)
                    {
                        table.SpawnObject(_current_item);
                        _current_item = null;
                        _holding_item = null;
                        
                        Item item = GetComponentInChildren<Item>();
                        Destroy(item.gameObject);
                    }
                } else {
                    //take item
                    if (!_current_item)
                    {
                        takeItem(table.TakeObject());
                    }
                }
            }
            if(skript.Type == "spawn")
            {
                ItemSpawn spawn = _current_collided_object.GetComponent<ItemSpawn>();
                if (spawn._canSpawn && !_current_item)
                {
                    takeItem(spawn.getItem());
                }
            }

            //Machine
            if (_curr_machine)
            {
                if (!_curr_machine.is_occupied)
                {
                    _curr_machine.PutItem(_holding_item.gameObject);
                    _current_item = null;
                    _holding_item = null;
                        
                    Item item = GetComponentInChildren<Item>();
                    Destroy(item.gameObject);

                } else
                {
                    takeItem(_curr_machine.RemoveItem());
                }
            }
        }
    }

    private void takeItem(GameObject itemObject)
    {
        print("take item");
        _current_item = itemObject;
        _holding_item = itemObject.GetComponent<Item>();

        Vector3 spawn_object_position = this.gameObject.transform.Find("SpawnPoint").gameObject.transform.position;
        Instantiate(itemObject, spawn_object_position, Quaternion.identity, this.gameObject.transform);
    }
    /// <summary>
    /// Takes item from parameter, and spawns it in hands, maybe do an animation soon?
    /// </summary>
    /// <param name="item"></param>
    private void takeItem(Item item)
    {
        _current_item = item.gameObject;
        _holding_item = item;

        Vector3 spawn_object_position = this.gameObject.transform.Find("SpawnPoint").gameObject.transform.position;
        Instantiate(_current_item, spawn_object_position, Quaternion.identity, this.gameObject.transform);

    }

    public void Work()
    {
        // Machine
        if(_curr_machine && _curr_machine.is_occupied)
        {
            _curr_machine.Operate();
        }

        if (_current_collided_item && !_current_collided_item.complete)
        {
            _current_collided_item.Work();
        }
    }

    public void stopWork()
    {
        if(_curr_machine && _curr_machine.is_occupied)
        {
            //_curr_machine.ProcessStart();
        }
        if (_current_collided_item && !_current_collided_item.complete)
        {
            _current_collided_item.stopWork();
        }
    }


    public bool getTriggered()
    {
        return _triggered;
    }
}
