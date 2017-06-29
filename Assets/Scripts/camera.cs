using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

    public string player = "nero";

    // Use this for initialization
    void Start () {
	    
	}

	// Update is called once per frame
	void Update () {
		float player = GameObject.Find("nero").transform.position.x;
		if (player > -2.28) {
			transform.position = new Vector3 (player, 0, -10);
		}
	}
}
