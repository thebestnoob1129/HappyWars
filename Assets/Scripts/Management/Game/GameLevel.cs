using UnityEngine;

public partial class GameLevel : PersistableObject
{
    
    public static GameLevel Current {  get; private set; }
    
    [SerializeField]
    SpawnZone spawnZone;

    [UnityEngine.Serialization.FormerlySerializedAs("persistentObjects")]
    [SerializeField]
    GameLevelObject[] levelObjects;

    [SerializeField]
    int populationLimit;
    public int PopulationLimit
    {
        get
        {
            return populationLimit;
        }
    }

    public void GameUpdate()
    {
        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].GameUpdate();
        }
    }

    public void SpawnShapes()
    {
        spawnZone.SpawnShapes();
    }

    private void OnEnable()
    {
        Current = this;
        if (levelObjects == null)
        {
            levelObjects = new GameLevelObject[0];
        }
    }
    
    public override void Save(GameDataWriter writer)
    {
        writer.Write(levelObjects.Length);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int savedCount = reader.ReadInt();
        for (int i = 0;i < savedCount; i++)
        {
            levelObjects[i].Load(reader);
        }
    }
}
