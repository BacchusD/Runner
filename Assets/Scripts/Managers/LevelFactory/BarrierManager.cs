using System;
using UnityEngine;



    class BarrierManager: MonoBehaviour {

        public static BarrierManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        [SerializeField]
        private GameObject[] BarrierPrefabs;

        public void InstantiateWall(GameObject baseObject, BlockPosition wallType) {
            var prefab = BarrierPrefabs[(int) BarrierType.Wall - 1];
            var blockPositions = Enum.GetValues(typeof(BlockPosition));

            foreach (BlockPosition wallBlock in blockPositions) {
                if (wallType.HasFlag(wallBlock)) {
                    var roadBlock = Instantiate(prefab, baseObject.transform, false);
                    roadBlock.transform.position += prefab.transform.position + wallBlock.Position();
                }
            }
        }
    }

    public enum BarrierType {
        None = 0,
        Wall = 1,
        //Trap = 2
        //Guillotine = 3,
    }

