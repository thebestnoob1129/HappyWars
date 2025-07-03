using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PersistentStorage))]
public class Game : PersistableObject
{
    public static Game Instance { get; private set; }
    
    const int saveVersion = 7;

    readonly PlayerInput playerInput;

    public KeyCode createAction = KeyCode.C;
    public KeyCode newgameAction = KeyCode.N;
    public KeyCode saveAction = KeyCode.S;
    public KeyCode loadAction = KeyCode.L;
    public KeyCode destroyAction = KeyCode.X;

    // UI

    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }

    float creationProgress, destructionProgress;
    [SerializeField] float destroyDuration = 1;

    [SerializeField] Slider creationSpeedSlider;
    [SerializeField] Slider destructionSpeedSlider;

    // Storage
    [SerializeField] ShapeFactory[] shapeFactories;

    [SerializeField] PersistentStorage storage;
    PersistentStorage persistentStorage;

    List<Shape> shapes;
    List<ShapeInstance> killList, markAsDyingList;

    // Scene
    [SerializeField] int levelCount = 2;
    int loadedLevelBuildIndex;

    // Spawn Zone

    public SpawnZone SpawnZoneofLevel { get; set; }

    // Seed
    
    Random.State mainRandomState;

    [SerializeField] bool reseedOnLoad;

    bool inGameUpdateLoop;

    // Functions 

    private void OnEnable()
    {
        Instance = this;
        if (shapeFactories[0].FactoryId != 0)
        {
            for (int i = 0; i < shapeFactories.Length; i++)
            {
                shapeFactories[i].FactoryId = i;
            }
        }
    }

    private void Start()
    {
        persistentStorage = GetComponent<PersistentStorage>();
        
        mainRandomState = Random.state;

        shapes = new List<Shape>();

        killList = new List<ShapeInstance>();
        markAsDyingList = new List<ShapeInstance>();

        if (Application.isEditor)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name.Contains("Level "))
                {
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
                    return;
                }
            }
        }
        
        BeginNewGame();
        StartCoroutine(LoadLevel(1));

        if (storage == null){ storage = GetComponent<PersistentStorage>(); }
    }

    private void FixedUpdate()
    {
        inGameUpdateLoop = true;
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].GameUpdate();
        }

        inGameUpdateLoop = false;
        // Creation and Destruction

        creationProgress += Time.deltaTime * CreationSpeed;
        
        while (creationProgress >= 1f)
        {
            creationProgress -= 1f;
            GameLevel.Current.SpawnShapes();
        }

        destructionProgress += Time.deltaTime * DestructionSpeed;
        
        while (destructionProgress >= 1f)
        {
            destructionProgress -= 1f;
            DestroyShape();
        }

        // Should Probably Modernize later or if not working
        if (Input.GetKeyDown(createAction))
        {
            GameLevel.Current.SpawnShapes();
        }
        else if (Input.GetKeyDown(newgameAction))
        {
            BeginNewGame();
            StartCoroutine(LoadLevel(loadedLevelBuildIndex));
        }
        else if (Input.GetKeyDown(saveAction))
        {
            storage.Save(this, saveVersion);
        }
        else if (Input.GetKeyDown(loadAction))
        {
            storage.Load(this);
        }
        else if (Input.GetKeyDown(destroyAction))
        {
            DestroyShape();
        }
        else
        {
            for (int i = 1; i <= levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))// Load Level Key || Level Select Menu
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }

        int limit = GameLevel.Current.PopulationLimit;
        if (limit > 0)
        {
            while (shapes.Count - dyingShapeCount > limit)
            {
                DestroyShape();
            }
        }

        if (killList.Count > 0)
        {
            for (int i = 0; i < killList.Count; i++)
            {
                if (killList[i].IsValid)
                {
                    KillImmediately(killList[i].Shape);
                }
            }
            killList.Clear();
        }
    
        if (markAsDyingList.Count > 0)
        {
            for (int i = 0; i < markAsDyingList.Count; i++)
            {
                if (markAsDyingList[i].IsValid)
                {
                    MarkAsDyingImmediately(markAsDyingList[i].Shape);
                }
            }
            markAsDyingList.Clear();
        }
    }

    void DestroyShape()
    {
        if (shapes.Count - dyingShapeCount > 0)
        {
            Shape shape = shapes[Random.Range(dyingShapeCount, shapes.Count)];
            if (destroyDuration <= 0)
            {
                KillImmediately(shape);
            }
            else
            {
                shape.AddBehavior<DyingShapeBehavior>().Initialize(
                    shape, destroyDuration
                    );
            }
        }
    }

    void BeginNewGame()
    {
        Random.state = mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
        mainRandomState = Random.state;
        Random.InitState(seed);

        creationSpeedSlider.value = CreationSpeed = 0;
        destructionSpeedSlider.value = DestructionSpeed = 0;

        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].Recycle();
        }
        shapes.Clear();
        dyingShapeCount = 0;
    }
    
    public Shape GetShape(int index)
    {
        return shapes[index];
    }
    
    public void AddShape(Shape shape)
    {
        shape.SaveIndex = shapes.Count;
        shapes.Add(shape);
    }
    
    public void Kill(Shape shape)
    {
        if (inGameUpdateLoop)
        {
            killList.Add(shape);
        }
        else
        {
            KillImmediately(shape);
        }
    }

    void KillImmediately(Shape shape)
    {
        int index = shape.SaveIndex;
        shape.Recycle();

        if (index < dyingShapeCount && index < --dyingShapeCount)
        {
            shapes[dyingShapeCount].SaveIndex = index;
        }

        int lastIndex = shapes.Count - 1;

        if (index < lastIndex)
        {
            shapes[lastIndex].SaveIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
        }
        shapes.RemoveAt(lastIndex);

    }

    int dyingShapeCount;

    private void MarkAsDyingImmediately(Shape shape)
    {
        int index = shape.SaveIndex;
        if (index < dyingShapeCount)
        {
            return;
        }

        shapes[dyingShapeCount].SaveIndex = index;
        shapes[index] = shapes[dyingShapeCount];
        shape.SaveIndex = dyingShapeCount;
        shapes[dyingShapeCount++] = shape;
    }

    public void MarkAsDying(Shape shape)
    {
        if (inGameUpdateLoop)
        {
            markAsDyingList.Add(shape);
        }
        else
        {
            MarkAsDyingImmediately(shape);
        }
    }

    public bool IsMarkedAsDying(Shape shape)
    {
        return shape.SaveIndex < dyingShapeCount;
    }

    // Save

    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapes.Count);
        writer.Write(Random.state);

        writer.Write(CreationSpeed);
        writer.Write(creationProgress);
        
        writer.Write(DestructionSpeed);
        writer.Write(destructionProgress);
        
        writer.Write(loadedLevelBuildIndex);
        GameLevel.Current.Save(writer);

        for (int i = 0;i < shapes.Count; i++)
        {
            writer.Write(shapes[i].OriginFactory.FactoryId);
            writer.Write(shapes[i].ShapeId);
            writer.Write(Random.state);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        // Support Version
        if (version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }

        if (version >= 3)
        {
            Random.State state = reader.ReadRandomState();
            if (!reseedOnLoad)
            {
                Random.state = state;
            }
            
            creationSpeedSlider.value = CreationSpeed = reader.ReadFloat();
            destructionSpeedSlider.value = DestructionSpeed = reader.ReadFloat();
            
            StartCoroutine(LoadGame(reader));
        }
    }
    
    IEnumerator LoadGame(GameDataReader reader) {
        int version = reader.Version;
        int count =  version <= 0 ? -version: reader.ReadInt();
        
        yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());
        if (version >= 3)
        {
            GameLevel.Current.Load(reader);
        }
        
        for (int i = 0; i < count; i++)
        {
            int factoryId = version >= 5 ? reader.ReadInt() : 0;
            int shapeId = version > 0 ? reader.ReadInt() : 0;
            int materialId = version > 0 ? reader.ReadInt() : 0;

            Shape instance = shapeFactories[factoryId].Get(0);
            instance.Load(reader);
        }

        for (int i = 0;i < shapes.Count; i++)
        {
            shapes[i].ResolveShapeInstances();
        }

    }

    IEnumerator LoadLevel(int levelBuildIndex) // Load Screen Time
    {
        enabled = false;
        if (loadedLevelBuildIndex > 0)
        {
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        }
        yield return SceneManager.LoadSceneAsync(
            levelBuildIndex, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 1"));
       loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }
}