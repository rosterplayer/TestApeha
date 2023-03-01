using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class LootSpawnerData
  {
    public string Id;
    public LootTypeId LootTypeId;
    public Vector3 Position;

    public LootSpawnerData(string id, LootTypeId lootTypeId, Vector3 position)
    {
      Id = id;
      LootTypeId = lootTypeId;
      Position = position;
    }
  }
}