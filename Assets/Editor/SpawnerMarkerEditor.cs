using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(EnemySpawnMarker))]
  public class SpawnerMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(EnemySpawnMarker spawner, GizmoType gizmo)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
  }
}