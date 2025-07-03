using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    public float Age { get; private set; }
    
    public int InstanceId { get; private set; }

    public int SaveIndex { get; set; }

    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
            else
            {
                Debug.LogError("Not Allowed to change shapeId");
            }
        }
    }
    int shapeId = int.MinValue;

    public ShapeFactory OriginFactory
    {
        get
        {
            return originFactory;
        }
        set
        {
            if (originFactory == null)
            {
                originFactory = value;
            }
            else
            {
                Debug.LogError("Not allowed to change origin factory");
            }
        }
    }
    ShapeFactory originFactory;

    // Material

    [SerializeField]
    MeshRenderer[] meshRenderers;

    private Material originalMat;

    public int MaterialId
    {
        get;
        private set;
    }

    public void SetMaterial(Material material, int materialId)
    {
        
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = material;
        }
        originalMat = material;
        MaterialId = materialId;
    }

    // Color

    Color[] colors;

    static int colorPropertyId = Shader.PropertyToID("_Color");

    static MaterialPropertyBlock sharedPropertyBlock;

    public int ColorCount
    {
        get
        {
            return colors.Length;
        }
    }
    
    List<ShapeBehavior> behaviorList = new List<ShapeBehavior>();
   
    public bool IsMarkedAsDying
    { 
        get {
            return Game.Instance.IsMarkedAsDying(this);
        }
    }


    // Function
    private void Awake()
    {
        colors = new Color[meshRenderers.Length];
    }

    public void GameUpdate()
    {
        Age += Time.deltaTime;
        for (int i = 0; i < behaviorList.Count; i++)
        {
            if (!behaviorList[i].GameUpdate(this))
            {
                behaviorList[i].Recycle();
                behaviorList.RemoveAt(i--);
            }
        }

        transform.localScale = Vector3.one;
    }

    public void SetColor(Color color)
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }

        sharedPropertyBlock.SetColor(colorPropertyId, color);
        
        for (int i = 0; i  < meshRenderers.Length; i++)
        {
            colors[i] = color;
            meshRenderers[i].SetPropertyBlock(sharedPropertyBlock);
        }
    }

    public void SetColor(Color color, int index)
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        colors[index] = color;
        meshRenderers[index].SetPropertyBlock(sharedPropertyBlock);
    }

    public void LoadColors(GameDataReader reader)
    {
        int count = reader.ReadInt();
        int max = count <= colors.Length ? count : colors.Length;
        int i = 0;
        for (;  i < max; i++)
        {
            SetColor(reader.ReadColor(), i);
        }

        if (count > colors.Length)
        {
            for (; i < count; i++)
            {
                reader.ReadColor();
            }
        }
        else if (count < colors.Length)
        {
            for (; i < colors.Length; i++)
            {
                SetColor(Color.white, i);
            }
        }
    }

    public T AddBehavior<T> () where T : ShapeBehavior, new()
    {
        T behavior = ShapeBehaviorPool<T>.Get();
        behaviorList.Add(behavior);
        return behavior;
    }

    public void ResolveShapeInstances()
    {
        for (int i = 0; i < behaviorList.Count; i++)
        {
            behaviorList[i].ResolveShapeInstances();
        }
    }

    public void Recycle()
    {
        Age = 0f;
        InstanceId += 1;

        for (int i = 0; i < behaviorList.Count; i++)
        {
            behaviorList[i].Recycle();
        }
        behaviorList.Clear();
        OriginFactory.Reclaim(this);
    }

    public void MarkAsDying()
    {
        Game.Instance.MarkAsDying(this);
    }

    // Save Data

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(colors.Length);
        for (int i = 0;i < meshRenderers.Length; i++)
        {
            writer.Write(colors[i]);
        }

        writer.Write(Age);
        writer.Write(behaviorList.Count);
        for (int i = 0; i < behaviorList.Count; i++)
        {
            writer.Write((int)behaviorList[i].BehaviorType);// Enum to int
            behaviorList[i].Save(writer);
        }
        
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        if (reader.Version >= 5)
        {
            LoadColors(reader);
        }
        else
        {
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }

        if (reader.Version >= 6)
        {
            Age = reader.ReadFloat();
            int behaviorCount = reader.ReadInt();
            for (int i = 0; i < behaviorCount; i++)
            {
                ShapeBehavior behavior =
                    ((ShapeBehaviorType)reader.ReadInt()).GetInstance();
                behaviorList.Add(behavior);
                behavior.Load(reader);
            }
        }
        else if (reader.Version >= 4)
        {
            AddBehavior<RotationShapeBehavior>().AngularVelocity = reader.ReadVector3();
            AddBehavior<MovementShapeBehavior>().Velocity = reader.ReadVector3();
        }
    }

    public void Die() { Game.Instance.Kill(this); }
}
