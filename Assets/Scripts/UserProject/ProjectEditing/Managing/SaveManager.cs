using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("TEST ONLY")]
    public List<GameObject> everyObjectInScene;

    void FixedUpdate()
    {
        everyObjectInScene.Clear();
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.activeInHierarchy && obj.transform.parent == null)
            {
                everyObjectInScene.Add(obj);
            }
        }
    }
    public void SaveScene()
    {
        Debug.Log("Scene saved with " + everyObjectInScene.Count + " objects.");
    }
}
