using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class CoordinatesManager : MonoBehaviour
{
    [Header("Dependencies")]
    public SelectionManager selectionManager;
    public GameObject contains;

    [Header("Panels and Buttons")]
    public string idk = "Привет, я координатный менеджер, меня ебали";
    public GameObject movePanel, rotatePanel, scalePanel;
    public Button moveButton, rotateButton, scaleButton;

    [Header("InputFields (Move)")]
    public string idkhello = "я это пишу чтобы не было ошибок показа переменных";
    public InputField MoveX, MoveY, MoveZ;
    public InputField RotateX, RotateY, RotateZ;
    public InputField ScaleX, ScaleY, ScaleZ;

    Transform currentObject;
    float cordX, cordY, cordZ;
    float rotX, rotY, rotZ;
    float scalX, scalY, scalZ;

    readonly CultureInfo inv = CultureInfo.InvariantCulture;
    bool isUIUpdating = false;

    void Start()
    {
        contains.SetActive(false);
        HideAllPanels();

        MoveX.onEndEdit.AddListener(_ => SetCoordinates());
        MoveY.onEndEdit.AddListener(_ => SetCoordinates());
        MoveZ.onEndEdit.AddListener(_ => SetCoordinates());
        RotateX.onEndEdit.AddListener(_ => SetCoordinates());
        RotateY.onEndEdit.AddListener(_ => SetCoordinates());
        RotateZ.onEndEdit.AddListener(_ => SetCoordinates());
        ScaleX.onEndEdit.AddListener(_ => SetCoordinates());
        ScaleY.onEndEdit.AddListener(_ => SetCoordinates());
        ScaleZ.onEndEdit.AddListener(_ => SetCoordinates());

        selectionManager.onSelect   += ShowEditor;
        selectionManager.onDeselect += HideEditor;
    }

    void Update()
    {
        if (currentObject == null) return;

        currentObject.position   = new Vector3(cordX, cordY, cordZ);
        currentObject.rotation   = Quaternion.Euler(rotX, rotY, rotZ);
        currentObject.localScale = new Vector3(scalX, scalY, scalZ);
    }

    public void ShowEditor(Transform target)
    {
        currentObject = target;
        if (currentObject == null)
        {
            HideEditor();
            return;
        }

        RefreshMoveFields();
        RefreshRotateFields();
        RefreshScaleFields();

        contains.SetActive(true);
        HideAllPanels();
        ShowMovePanel();
    }

    public void HideEditor()
    {
        currentObject = null;
        contains.SetActive(false);
    }

    public void SetCoordinates()
    {
        if (currentObject == null || isUIUpdating) 
            return;

        float.TryParse(MoveX.text,   NumberStyles.Float, inv, out cordX);
        float.TryParse(MoveY.text,   NumberStyles.Float, inv, out cordY);
        float.TryParse(MoveZ.text,   NumberStyles.Float, inv, out cordZ);

        float.TryParse(RotateX.text, NumberStyles.Float, inv, out rotX);
        float.TryParse(RotateY.text, NumberStyles.Float, inv, out rotY);
        float.TryParse(RotateZ.text, NumberStyles.Float, inv, out rotZ);

        float.TryParse(ScaleX.text,  NumberStyles.Float, inv, out scalX);
        float.TryParse(ScaleY.text,  NumberStyles.Float, inv, out scalY);
        float.TryParse(ScaleZ.text,  NumberStyles.Float, inv, out scalZ);
    }

    void RefreshMoveFields()
    {
        Vector3 p = currentObject.position;
        cordX = p.x; cordY = p.y; cordZ = p.z;

        isUIUpdating = true;
        MoveX.text = cordX.ToString("F2", inv);
        MoveY.text = cordY.ToString("F2", inv);
        MoveZ.text = cordZ.ToString("F2", inv);
        isUIUpdating = false;
    }

    void RefreshRotateFields()
    {
        Vector3 r = currentObject.rotation.eulerAngles;
        rotX = r.x; rotY = r.y; rotZ = r.z;

        isUIUpdating = true;
        RotateX.text = rotX.ToString("F2", inv);
        RotateY.text = rotY.ToString("F2", inv);
        RotateZ.text = rotZ.ToString("F2", inv);
        isUIUpdating = false;
    }

    void RefreshScaleFields()
    {
        Vector3 s = currentObject.localScale;
        scalX = s.x; scalY = s.y; scalZ = s.z;

        isUIUpdating = true;
        ScaleX.text = scalX.ToString("F2", inv);
        ScaleY.text = scalY.ToString("F2", inv);
        ScaleZ.text = scalZ.ToString("F2", inv);
        isUIUpdating = false;
    }

    public void HideAllPanels()
    {
        movePanel.SetActive(false);
        rotatePanel.SetActive(false);
        scalePanel.SetActive(false);
        moveButton.interactable   = true;
        rotateButton.interactable = true;
        scaleButton.interactable  = true;
    }

    public void ShowMovePanel()
    {
        if (currentObject == null) return;
        HideAllPanels();
        movePanel.SetActive(true);
        moveButton.interactable = false;
        RefreshMoveFields();
    }

    public void ShowRotatePanel()
    {
        if (currentObject == null) return;
        HideAllPanels();
        rotatePanel.SetActive(true);
        rotateButton.interactable = false;
        RefreshRotateFields();
    }

    public void ShowScalePanel()
    {
        if (currentObject == null) return;
        HideAllPanels();
        scalePanel.SetActive(true);
        scaleButton.interactable = false;
        RefreshScaleFields();
    }
}
