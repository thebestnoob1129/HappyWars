using UnityEngine;
using static Shape;

public abstract class ShapeBehavior
#if UNITY_EDITOR
    : ScriptableObject
#endif
{
    public abstract ShapeBehaviorType BehaviorType { get; }

    public abstract bool GameUpdate(Shape shape);

    public abstract void Save(GameDataWriter writer);

    public abstract void Load(GameDataReader reader);

    public abstract void Recycle();

    public virtual void ResolveShapeInstances() { }

    public bool IsReclaimed { get; set; }

#if UNITY_EDITOR
    void OnEnable()
    {
        if (IsReclaimed)
        {
            Recycle();
        }
    }
#endif
}