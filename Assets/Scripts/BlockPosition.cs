using System;
using UnityEngine;

[Flags]
public enum BlockPosition {
    LeftBottom = 1,
    MiddleBottom = 1 << 1,
    RightBottom = 1 << 2,
    LeftMiddle = 1 << 3,
    Center = 1 << 4,
    RightMiddle = 1 << 5,
    LeftTop = 1 << 6,
    MiddleTop = 1 << 7,
    RightTop = 1 << 8
}

public static class BlockPositionCombine {
    public const BlockPosition LeftWall = BlockPosition.LeftBottom | BlockPosition.LeftMiddle | BlockPosition.LeftTop;
    public const BlockPosition CenterWall = BlockPosition.MiddleBottom | BlockPosition.Center | BlockPosition.MiddleTop;
    public const BlockPosition RightWall = BlockPosition.RightBottom | BlockPosition.RightMiddle | BlockPosition.RightTop;
    public const BlockPosition Bottom = BlockPosition.LeftBottom | BlockPosition.MiddleBottom | BlockPosition.RightBottom;
    public const BlockPosition Middle = BlockPosition.LeftMiddle | BlockPosition.Center | BlockPosition.RightMiddle;
    public const BlockPosition Top = BlockPosition.LeftTop | BlockPosition.MiddleTop | BlockPosition.RightTop;
    public const BlockPosition LeftDoubleBottom = BlockPosition.LeftBottom | BlockPosition.MiddleBottom;
    public const BlockPosition RightDoubleBottom = BlockPosition.MiddleBottom | BlockPosition.RightBottom;
    public const BlockPosition LeftAndRightBottom = BlockPosition.LeftBottom | BlockPosition.RightBottom;
}

public static class BlockPositionExt {

    public static bool HasFlag(this BlockPosition first, BlockPosition second) {
        return (first & second) == second;
    }

    public static bool Any(this BlockPosition first, BlockPosition second) {
        return (first & second) > 0;
    }

    public static Vector3 Position(this BlockPosition block) {
        return new Vector3(0, block.PositionY(), block.PositionZ());
    }

    //public static Quaternion Rotate(this BlockPosition block) {
    //    return Quaternion.Euler(0, 0, block.SlopRotationZ());
    //}
    
    private static float PositionZ(this BlockPosition blockPosition) {
        if (blockPosition.Any(BlockPositionCombine.LeftWall)) {
            return -1f;
        }  else if (blockPosition.Any(BlockPositionCombine.CenterWall)) {
            return 0f;
        } else {
            return 1f; ;
        }
    }

    private static float PositionY(this BlockPosition blockPosition) {
        if (blockPosition.Any(BlockPositionCombine.Bottom)) {
            return 0f;
        } else if (blockPosition.Any(BlockPositionCombine.Middle)) {
            return 1f;
        } else {
            return 2f;
        }
    }

    //public static Vector3 Scale(this BlockPosition block) {
    //    return new Vector3(block.SlopScale(), 0, 0);
    //}

    //private static float SlopScale(this BlockPosition blockPosition) {
    //    return blockPosition.Any(BlockPosition.SlopDown | BlockPosition.SlopUp) ? 0.539f : 0f;
    //}

    //private static float SlopRotationZ(this BlockPosition blockPosition) {
    //    return blockPosition.HasFlag(BlockPosition.SlopDown)
    //        ? 21.8f 
    //        : blockPosition.HasFlag(BlockPosition.SlopUp) ? -21.8f : 0f;
    //}
}
