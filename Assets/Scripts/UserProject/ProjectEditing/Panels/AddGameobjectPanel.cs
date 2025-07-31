using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGameobjectPanel : MonoBehaviour
{
    [Header("GameObject Settings")]
    public GameObjectType currentGameObjectType;

    public enum GameObjectType
    {
        Cube,
        Sphere,
        Capsule,
        Cylinder,
        Plane,
        Quad,
        Camera,
        Light,
        DefaultPlayer
    }
    public GameObject cubePrefab, spherePrefab, capsulePrefab, cylinderPrefab, planePrefab, quadPrefab, cameraPrefab, lightPrefab, defaultPlayerPrefab;
    [Header("Hierarchy List Settings")]
    [SerializeField] private GameObject inspectorListItemPrefab;
    [SerializeField] private Transform inspectorListParent;
    [Header("Debug")]
    public SelectionManager selectionManager;   

    private List<GameObject> inspectorItems = new List<GameObject>();
    private float defaultHierarchyPosY = 25f;
    private float defaultMinusY = 100f;

    public void SetTypeToCube() => changeGameobjectType(GameObjectType.Cube);
    public void SetTypeToSphere() => changeGameobjectType(GameObjectType.Sphere);
    public void SetTypeToCapsule() => changeGameobjectType(GameObjectType.Capsule);
    public void SetTypeToCylinder() => changeGameobjectType(GameObjectType.Cylinder);
    public void SetTypeToPlane() => changeGameobjectType(GameObjectType.Plane);
    public void SetTypeToQuad() => changeGameobjectType(GameObjectType.Quad);
    public void SetTypeToCamera() => changeGameobjectType(GameObjectType.Camera);
    public void SetTypeToLight() => changeGameobjectType(GameObjectType.Light);
    public void SetTypeToDefaultPlayer() => changeGameobjectType(GameObjectType.DefaultPlayer);

    public void changeGameobjectType(GameObjectType type)
    {
        currentGameObjectType = type;
        Debug.Log("Switched to: " + type);
        CreateGameObject();
    }

    public void AddInspectorItem(GameObject targetObject)
    {
        GameObject item = Instantiate(inspectorListItemPrefab, inspectorListParent);
        inspectorItems.Add(item); 

        var selectButton = item.transform.Find("SelectButton").GetComponent<Button>();
        var nameInput = item.transform.Find("NameInput").GetComponent<InputField>();
        var deleteButton = item.transform.Find("DeleteButton").GetComponent<Button>();
        var copyButton = item.transform.Find("CopyButton")?.GetComponent<Button>();
        var rt = item.GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, defaultHierarchyPosY - (inspectorItems.Count * defaultMinusY));

        nameInput.text = targetObject.name;
        nameInput.onEndEdit.AddListener(newName =>
        {
            targetObject.name = newName;
        });

        selectButton.onClick.AddListener(() =>
        {
            selectionManager.ForceSelect(targetObject.transform);
        });

        deleteButton.onClick.AddListener(() =>
        {
            Destroy(targetObject);
            inspectorItems.Remove(item);
            Destroy(item);
            RefreshInspectorPositions();
        });

        if (copyButton != null)
        {
            copyButton.onClick.AddListener(() =>
            {
                CopyGameObject(targetObject);
            });
        }
    }

    private void RefreshInspectorPositions()
    {
        for (int i = 0; i < inspectorItems.Count; i++)
        {
            var rt = inspectorItems[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, defaultHierarchyPosY - ((i + 1) * defaultMinusY));
        }
    }

    private void CopyGameObject(GameObject originalObject)
    {
        GameObject copiedObject = Instantiate(originalObject);
        
        copiedObject.transform.position = originalObject.transform.position;
        copiedObject.transform.rotation = originalObject.transform.rotation;
        copiedObject.transform.localScale = originalObject.transform.localScale;
        
        copiedObject.name = originalObject.name + "_Copy";

        AddInspectorItem(copiedObject);
        Debug.Log("Copied: " + copiedObject.name);
    }

    public void CreateGameObject()
    {
        GameObject newObject = null;

        switch (currentGameObjectType)
        {
            case GameObjectType.Cube:
                newObject = Instantiate(cubePrefab);
                break;
            case GameObjectType.Sphere:
                newObject = Instantiate(spherePrefab);
                break;
            case GameObjectType.Capsule:
                newObject = Instantiate(capsulePrefab);
                break;
            case GameObjectType.Cylinder:
                newObject = Instantiate(cylinderPrefab);
                break;
            case GameObjectType.Plane:
                newObject = Instantiate(planePrefab);
                break;
            case GameObjectType.Quad:
                newObject = Instantiate(quadPrefab);
                break;
            case GameObjectType.Camera:
                newObject = Instantiate(cameraPrefab);
                break;
            case GameObjectType.Light:
                newObject = Instantiate(lightPrefab);
                break;
            case GameObjectType.DefaultPlayer:
                newObject = Instantiate(defaultPlayerPrefab);
                break;
            
        }

        if (newObject != null)
        {
            newObject.transform.localScale = new Vector3(1, 1, 1);
            newObject.transform.position = Vector3.zero;
            newObject.AddComponent<SelectableObject>();
            newObject.AddComponent<BoxCollider>();
            AddInspectorItem(newObject);
            Debug.Log("Created: " + currentGameObjectType);
        }
    }
}