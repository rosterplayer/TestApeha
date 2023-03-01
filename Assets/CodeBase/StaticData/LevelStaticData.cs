﻿using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public List<EnemySpawnerData> EnemySpawners;
    public List<LootSpawnerData> LootSpawners;
    public Vector3 InitialHeroPosition;
    public List<LevelTransferPoint> TransferPoints;
  }
}