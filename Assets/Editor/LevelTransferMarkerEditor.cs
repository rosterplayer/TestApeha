using CodeBase.Logic.LevelTransfer;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(LevelTransferMarker))]
  public class LevelTransferMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(LevelTransferMarker transferMarker, GizmoType gizmo)
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawCube(transferMarker.transform.position, new Vector3(2f, 1.5f, 1.8f));
    }
  }
}