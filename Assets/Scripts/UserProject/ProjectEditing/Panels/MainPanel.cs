using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [Header("GameObject Button Settings")]
    public Button ObjToggleButton;
    public GameObject ObjPanel;
    public Button DeleteObjButton;
    public SelectionManager selectionManager;
    [Header("ScriptableObject Settings")]
    public Button scriptButton;
    public Button editScriptButton;
    [Header("Other Settings")]
    public Button lightingButton;
    public GameObject lightingPanel;
    public GameObject inspectorPanel;
    private int isToggle = 0;
    private void Start()
    {
        ObjPanel.SetActive(false);
    }

    void FixedUpdate()
    {
        DeleteObjButtonToggle();
        if (inspectorPanel.activeSelf)
        {
            lightingButton.interactable = false;
            lightingPanel.SetActive(false);
            lightingButton.image.color = new Color(1f, 1f, 1f, 1f);
            if (editScriptButton.interactable == false)
            {
                scriptButton.interactable = true;
            }
            else
            {
                scriptButton.interactable = false;
                editScriptButton.interactable = true;
            }
        }
        else
        {
            lightingButton.interactable = true;
            scriptButton.interactable = false;
            editScriptButton.interactable = false;
        }
    }
    public void LightingButtonToggle()
    {
        if (lightingPanel.activeSelf)
        {
            lightingPanel.SetActive(false);
            lightingButton.image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            lightingPanel.SetActive(true);
            lightingButton.image.color = new Color(0.854037f, 1f, 0.8459119f, 1f);
        }
    }
    public void ObjButtonToggle()
    {
        if (isToggle == 0)
        {
            isToggle = 1;
            ObjToggleButton.image.color = new Color(0.854037f, 1f, 0.8459119f, 1f);
            ObjPanel.SetActive(true);
        }
        else
        {
            isToggle = 0;
            ObjToggleButton.image.color = new Color(1f, 1f, 1f, 1f);
            ObjPanel.SetActive(false);
        }
    }
    public void DeleteObjButtonToggle()
    {
        if (selectionManager.CurrentSelect != null)
        {
            DeleteObjButton.interactable = true;
        }
        else
        {
            DeleteObjButton.interactable = false;
            return;
        }
    }
    public void DeleteSelectedObject()
    {
        Destroy(selectionManager.CurrentSelect);
        selectionManager.Deselect();
    }
}
