using UnityEngine;
using System.Collections;

public class BaseGameObject : MonoBehaviour {

    protected Vector3 ObjectPosition {
        get { return gameObject.transform.position; }
        set { gameObject.transform.position = value; }
    }
}
