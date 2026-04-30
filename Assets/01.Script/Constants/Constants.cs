using UnityEngine;
public enum ItemType
{
    None,
    Potion,
    Ammo,
    Coin
}

public enum potionType
{
    None,
    NormalPotion,
    HighPotion
}

public enum TileType
{
    Empty,
    Breakable,
    Solid
}

public enum EnemyType
{
    None,
    Zombie,
    Skeleton,
    Boss
}

public enum DIRECTION
{
    Left, Right, Up, Down,
}
public static class Direction4
{
    public static Vector2 ToVector2(DIRECTION direction)
    {
        return direction switch
        {
            DIRECTION.Left => Vector2.left,
            DIRECTION.Right => Vector2.right,
            DIRECTION.Up => Vector2.up,
            DIRECTION.Down => Vector2.down,
            _ => Vector2.zero
        };
    }
}