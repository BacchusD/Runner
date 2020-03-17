using UnityEngine;
using Assets.Scripts.Managers;

    class RoadManager: MonoBehaviour {

        public static RoadManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        [SerializeField]
        private GameObject[] RoadPrefabs;

        public void Instantiate(GameObject baseObject, RoadUnit roadUnit) {

            for (int i = 0; i < 6; i++) {
                var roadType = roadUnit[i];
                if (roadType > RoadType.None) {
                    var prefab = RoadPrefabs[(int)roadType - 1];
                    var vertOffset = Vector3.up * (i / 3) * 2;
                    var zOffset = Vector3.forward * (i % 3 - 1);
                    var roadBlock = Instantiate(prefab, baseObject.transform, false);
                    roadBlock.transform.position += prefab.transform.position + vertOffset + zOffset;
                }
            }
        }
    }

