using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlacementSetup))]
public class PlacementTool : Editor {
    PlacementSetup setup;

    Vector3 spawnPos; 

    public void OnEnable(){
        setup = target as PlacementSetup;

        Tools.hidden = true;
    }
    
    public void OnSceneGUI()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            spawnPos = hit.point;

            SceneView.RepaintAll();
        }

        Handles.color = Color.white;

        Handles.DrawWireDisc(spawnPos, Vector3.up, setup.gizmoRadius);

        HandleUtility.AddDefaultControl(0);

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            //Should we add or remove objects?
            if (setup.currentAction == PlacementSetup.Actions.AddObject)
            {
                AddNewPrefabs();

                MarkSceneAsDirty();
            }
            else if (setup.currentAction == PlacementSetup.Actions.RemoveObject)
            {
                setup.RemoveObject(spawnPos);

                MarkSceneAsDirty();
            }
        }
    }

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        if(GUILayout.Button("Remove ALL Gameobjects")){
            if(EditorUtility.DisplayDialog("Warning Message", "Are you sure you want to delete ALL spawned nodes?", "Yes", "Cancel")){
                setup.RemoveAllObjects();
                MarkSceneAsDirty();
            }
        }
    }


    private void MarkSceneAsDirty()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }

    private void AddNewPrefabs()
    {
        GameObject objToInstatiate = PrefabUtility.InstantiatePrefab(setup.objectToInstantiate) as GameObject;

        setup.AddPrefab(objToInstatiate, spawnPos);
    }
}