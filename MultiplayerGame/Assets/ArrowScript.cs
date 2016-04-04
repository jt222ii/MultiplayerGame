using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed; 
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
