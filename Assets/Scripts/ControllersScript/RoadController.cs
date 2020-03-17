using UnityEngine;

public class RoadController : BaseGameObject {

    public static GameObject LastRoad;

	// Use this for initialization
	void Start () {
	    LastRoad = gameObject;
	}

    void FixedUpdate() {
        var roadspeed = GameManager.Instance.RoadSpeed;
        transform.Translate(Vector3.right * roadspeed * Time.fixedDeltaTime);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
