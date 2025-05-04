using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelConfig", menuName = "config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public List<LevelData> levelDatas;
}
[Serializable]
public class LevelData{
    public int LevelIndex;
    public int SceneIndex;
}
