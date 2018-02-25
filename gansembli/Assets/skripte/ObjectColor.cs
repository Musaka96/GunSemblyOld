using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColor : MonoBehaviour {

    public Shader presetShader;
    private MeshRenderer _renderer;
    Material[] materijali;

    // Use this for initialization
    void Start () {
        _renderer = gameObject.GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

		
	}

    public void GlowOnInteract()
    {
        foreach (Material mat in _renderer.materials)
        {
            mat.shader = presetShader;
        }
    }
    public void StopInteractGlow()
    {
        foreach (Material mat in _renderer.materials)
        {
            mat.shader = Shader.Find("Standard");
        }
    }
}
