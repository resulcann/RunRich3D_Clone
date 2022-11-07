using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName ="New Level", menuName ="Levels", order = 1)]
    public class Level : ScriptableObject
    {
        public GameObject levelPrefab;
        public int levelIndex;
        private GameObject _spawnedLevelPrefab;

        public void CreateLevel()
        {
            _spawnedLevelPrefab = Instantiate(levelPrefab);
        }

        public void DestroyLevel()
        {
            DestroyImmediate(_spawnedLevelPrefab);
        }
    }
}