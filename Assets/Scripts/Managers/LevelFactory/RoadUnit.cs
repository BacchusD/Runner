using System;
using System.Linq;
using System.Security.Policy;
using UnityEngine;

namespace Assets.Scripts.Managers {
    /// <summary>
    /// Это сервисный слой от абстратного пепятсвии в генераторе уровня и физическими объектами-префабами
    /// </summary>
    class RoadUnit : MonoBehaviour {

        [SerializeField] private GameObject[] RoadPrefabs;

        private RoadType[] _units = new RoadType[6];

        public RoadType this[int index] {
            get { return (RoadType)_units[index]; }
        }

        public RoadType this[BlockPosition blockPosition] {
            get {
                var index = (int)Math.Log((int)blockPosition, 2);
                return (RoadType)_units[index];
            }
        }

        public RoadUnit() { }

        public RoadUnit(RoadType left, RoadType middle, RoadType right) : this(left, middle, right, 0, 0, 0) { }

        public RoadUnit(RoadType left, RoadType middle, RoadType right, RoadType leftTop, RoadType middleTop, RoadType rightTop) {
            _units[0] = left;
            _units[1] = middle;
            _units[2] = right;
            _units[3] = leftTop;
            _units[4] = middleTop;
            _units[5] = rightTop;
        }

        public void Instatiate(GameObject baseUnit) {

        }

        public static implicit operator int(RoadUnit roadUnit) {
            var result = 0;
            var basePow = Enum.GetValues(typeof(RoadType)).Length;
            var pow = 1;
            for (int idx = 0; idx < roadUnit._units.Length; idx++) {
                result += pow * (int) roadUnit._units[idx];
                pow *= basePow;
            }

            return result;
        }

        public static implicit operator RoadUnit(int i) {
            var index = 0;
            int value = i;
            var basePow = Enum.GetValues(typeof(RoadType)).Length;
            var roadUnit = new RoadUnit();
            while (value > 0) {
                roadUnit._units[index] = (RoadType)(value % basePow);
                value /= basePow;
            }

            return roadUnit;
        }

        public static implicit operator RoadUnit(BlockPosition blockPosition) {
            var blockPositionValues = Enum.GetValues(typeof(BlockPosition)).Cast<BlockPosition>().ToList();
            var roadUnit = new RoadUnit();
            for (int idx = 0; idx < roadUnit._units.Length; idx++) {
                if (blockPosition.HasFlag(blockPositionValues[idx]))
                {
                    roadUnit._units[idx] = RoadType.Road;
                }
            }

            return roadUnit;
        }

        public override int GetHashCode() {
            return this;
        }
    }

    /// <summary>
    /// Определяет разные типы дороги
    /// Каждый тип реазиван своим собственным префабом, возможно, со своим собственным контроллером
    /// </summary>
    enum RoadType {
        None = 0,
        Road,
        RoadSlopUp,
        RoadSlopDown,
        //Track
        //Drop
    }
}
