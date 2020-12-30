using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PlacementSetup : MonoBehaviour{
    public GameObject objectToInstantiate;
    public float gizmoRadius;
    public Transform parent;

    void Update(){
        
    }

    public enum Actions 
    {
        AddObject,
        RemoveObject
    }

    public Actions currentAction;

    public void Log(string _message){
        print(_message);
    }

    public void AddPrefab(GameObject _newPrefabObj, Vector3 _position){
        _newPrefabObj.transform.position = _position;
        _newPrefabObj.transform.parent = parent;
    }

    public void RemoveObject(Vector3 _hitPoint){

        List<GameObject> allChildren = GetAllChildren();

        foreach(GameObject child in allChildren){
            if((child.transform.position - _hitPoint).magnitude < gizmoRadius * 3){
                DestroyImmediate(child.gameObject);
            }
        }
    }

    public void RemoveAllObjects(){
        List<GameObject> allObjects = GetAllChildren();

        foreach(GameObject child in allObjects){
            DestroyImmediate(child);
        }
    }

    private List<GameObject> GetAllChildren()
    {
        List<GameObject> allChildren = new List<GameObject>();

        foreach(Transform child in parent){
            allChildren.Add(child.gameObject);
        }

        return allChildren;
    }
}