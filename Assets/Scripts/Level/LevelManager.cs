using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Level> Levels;
    public int currentLevelIndex = 0;
    [HideInInspector] public List<GameObject> allSpawnedLadders;

    void Awake() 
    {
        Instance = this;
    }
    void Start() 
    {
        SpawnCurrentLevel();
    }
    
    public void SpawnCurrentLevel()
    {
        Levels[currentLevelIndex].CreateLevel();
    }

    public void NextLevel()
    {
        Levels[currentLevelIndex].DestroyLevel();
        if(currentLevelIndex == 2)
        {
            currentLevelIndex = Random.Range(0,2);
        }
        else{
            currentLevelIndex += 1;
        }
        
        Levels[currentLevelIndex].CreateLevel();
        DestroyAllLadder();
    }
    public void RetryLevel()
    {
        Levels[currentLevelIndex].DestroyLevel();
        Levels[currentLevelIndex].CreateLevel();
        DestroyAllLadder();
    }

    public void DestroyAllLadder()
    {
        for (int i = 0; i < allSpawnedLadders.Count; i++)
        {
            Destroy(allSpawnedLadders[i]);
        }
        allSpawnedLadders.Clear();
    }
}