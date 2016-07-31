using UnityEngine;
using System.Collections;

public class ColorPicker : MonoBehaviour {

    public Color colorPicker;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = colorPicker;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
