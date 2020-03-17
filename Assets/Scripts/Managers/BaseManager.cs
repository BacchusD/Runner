using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за генерацию базового элемента дороги. Определяет его позицию.
/// </summary>
public class BaseManager : MonoBehaviour {

    public static BaseManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        //В будущем эта фабрика будет получать параметры с такими настройками как визуальный тип объектов,
        //сложность уровня, использовать принипиально разные схемы для генерации уровня
        _levelFactory = new LevelFactory();
        _baseObject = new GameObject("baseObject");
    }

    [SerializeField]
    private GameObject _baseObject;

    List<GameObject> Road = new List<GameObject>();

    readonly Vector3 roadOffset = new Vector3(-5, 0, 0);

    private LevelFactory _levelFactory;


    private void Start() {
        for (int pos = -2; pos < 15; pos++)  {
            var position = _baseObject.transform.position + roadOffset * pos;
            var road = Instantiate(_baseObject, position, Quaternion.identity);
            _levelFactory.AddStartRoadUnit(road);
            Road.Add(road.gameObject);
        }
    }

    void FixedUpdate() {
        //Spawn();
    }

    void Spawn() {

        while (Road[0].transform.position.x > 15) {
            var LastRoadObject = Road[Road.Count - 1];

            var baseObject = Instantiate(_baseObject, LastRoadObject.transform.position + roadOffset, Quaternion.identity);
            _levelFactory.AddNextUnit(baseObject);
            Road.Add(baseObject);
            Destroy(Road[0]);
            Road.RemoveAt(0);
        }
    }

    void Update() {
        //Spawn();
    }

    private void LateUpdate() {
       Spawn();
    }
}