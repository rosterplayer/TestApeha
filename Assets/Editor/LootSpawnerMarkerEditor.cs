using CodeBase.LootContainer;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(LootSpawnMarker))]
  public class LootSpawnerMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(LootSpawnMarker spawner, GizmoType gizmo)
    {
      Gizmos.color = Color.green;
      Gizmos.DrawCube(spawner.transform.position + Vector3.down/2, new Vector3(3,2,3));
    }
  }
}