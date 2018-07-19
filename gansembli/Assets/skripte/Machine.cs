using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : InteractableElement {


    public bool is_occupied = false;
    private Item current_item;
    private GameObject _current_object;
    private MachineController _controller;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(!is_occupied)
        {
            current_item = null;
        }
		
	}

    public void PutItem(GameObject obj)
    {
        current_item = obj.GetComponentInChildren<Item>();
        _current_object = obj;
        is_occupied = true;
        Vector3 spawn_object_position = gameObject.transform.Find("SpawnPoint").gameObject.transform.position;
        Instantiate(obj, spawn_object_position, Quaternion.identity, this.gameObject.transform);
    }

    public GameObject RemoveItem()
    {
        is_occupied = false;
        _controller.Action(current_item);
        _controller.Action(null);

        
        return _current_object;
    }

    public void Operate()
    {
        if (!_controller)
        {
            _controller = gameObject.GetComponent<MachineController>();
        }
        _controller.Action(current_item);
    }
}
