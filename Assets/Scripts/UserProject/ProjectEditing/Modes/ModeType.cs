using System.Collections.Generic;
using UnityEngine;

public class ModeType : MonoBehaviour
{
    public List<GameObject> EditorObjects;
    public List<GameObject> PlayerObjects;
    public List<DefaultPlayer> DefaultPlayers;

    public enum Mode
    {
        EditorMode,
        PlayerMode
    }

    public Mode CurrentMode;

    private struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }

    private Dictionary<SelectableObject, TransformData> _savedTransforms = new Dictionary<SelectableObject, TransformData>();

    void Start()
    {
        CurrentMode = Mode.EditorMode;
    }

    public void SwitchMode()
    {
        DefaultPlayers.Clear();
        DefaultPlayers.AddRange(FindObjectsOfType<DefaultPlayer>());
        bool isPlayersRuntime = DefaultPlayers.Count > 0 && DefaultPlayers[0].isRuntime;

        if (CurrentMode == Mode.EditorMode)
        {
            RememberEveryObjectTransformBeforeSwitch();

            CurrentMode = Mode.PlayerMode;
            SetActiveObjects(PlayerObjects, true);
            SetActiveObjects(EditorObjects, false);

            if (!isPlayersRuntime)
            {
                foreach (var player in DefaultPlayers)
                {
                    player.isRuntime = true;
                }
            }
        }
        else
        {
            CurrentMode = Mode.EditorMode;
            ResetEveryObjectTransformAfterSwitchingOnEditorMode();

            SetActiveObjects(PlayerObjects, false);
            SetActiveObjects(EditorObjects, true);

            if (isPlayersRuntime)
            {
                foreach (var player in DefaultPlayers)
                {
                    player.isRuntime = false;
                }
            }
        }
    }

    private void SetActiveObjects(List<GameObject> objects, bool isActive)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }

    public void RememberEveryObjectTransformBeforeSwitch()
    {
        _savedTransforms.Clear();
        var allSelectable = FindObjectsOfType<SelectableObject>();

        foreach (var sel in allSelectable)
        {
            TransformData data = new TransformData
            {
                Position = sel.transform.position,
                Rotation = sel.transform.rotation,
                Scale = sel.transform.localScale
            };
            _savedTransforms[sel] = data;
        }
    }

    public void ResetEveryObjectTransformAfterSwitchingOnEditorMode()
    {
        foreach (var kvp in _savedTransforms)
        {
            var sel = kvp.Key;
            var data = kvp.Value;

            if (sel != null)
            {
                sel.transform.position = data.Position;
                sel.transform.rotation = data.Rotation;
                sel.transform.localScale = data.Scale;
            }
        }

        //_savedTransforms.Clear();
    }
}
