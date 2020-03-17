using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers {
    /// <summary>
    /// Нужен с# 7.3 чтоб можно было использовать Enum в generic 
    /// </summary>
    //class BaseUnits<T> : MonoBehaviour where T:Enum {

    //    protected GameObject[] prefabs;

    //    protected int[] _units;

    //    private int _basePow;



    //    public static implicit operator int(BaseUnits<T> roadUnit)
    //    {
    //        var result = 0;
    //        var basePow = Enum.GetValues(typeof(T)).Length;
    //        var pow = 1;
    //        for (int idx = 0; idx < roadUnit._units.Length; idx++)
    //        {
    //            result += pow * (int)roadUnit._units[idx];
    //            pow *= basePow;
    //        }

    //        return result;
    //    }

    //    public static implicit operator BaseUnits<T>(int i)
    //    {
    //        var index = 0;
    //        int value = i;
    //        var basePow = Enum.GetValues(typeof(T)).Length;
    //        var roadType = new BaseUnits();
    //        while (value > 0)
    //        {
    //            roadType._units[index] = (T)(value % basePow);
    //            value /= basePow;
    //        }

    //        return roadType;
    //    }

    //    public static implicit operator BaseUnits<T>(BlockPosition blockPosition)
    //    {
    //        var blockPositionValues = Enum.GetValues(typeof(BlockPosition)).Cast<BlockPosition>().ToList();
    //        var roadType = new BaseUnits<T>();
    //        for (int idx = 0; idx < roadType._units.Length; idx++)
    //        {
    //            if (blockPosition.HasFlag(blockPositionValues[idx]))
    //            {
    //                roadType._units[idx] = 1;
    //            }
    //        }

    //        return roadType;
    //    }
    //}
}
