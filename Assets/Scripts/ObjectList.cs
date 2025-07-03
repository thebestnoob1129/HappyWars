using UnityEngine;

public static class ObjectList
{

    public enum BlockName
    {
        Brick,
        Fabric,
        Magma,
        Ice,
        Slime,
        Fake,
        Cloud,
        Metal
    }

    public enum BlockType // Out of 10
    {
        Normal, // 8
        Decoration, 
        Obstacle,
        Interactable,
        Objective,
        Entity
    }
}
