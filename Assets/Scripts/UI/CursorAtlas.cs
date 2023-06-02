using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAtlas : MonoBehaviour
{
    public static CursorAtlas instance;
    [SerializeField] private List<CursorData> cursorData;

    [System.Serializable]
    private class CursorData
    {
        public Texture2D cursorTex;
        public Vector2 hotspot;
    }

    public enum CursorType
    {
        DEFAULT,
        POINTER,
        ARROW_HORI,
        ARROW_VERT,
        ARROW_DIAG_POS,
        ARROW_DIAG_NEG,
        ARROW_QUAD,
        TEXT,
        HAND_IDLE,
        HAND_GRAB
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCursor(CursorType type, CursorMode mode)
    {
        if (type == CursorType.DEFAULT)
        {
            Cursor.SetCursor(null, Vector2.zero, mode);
        }
        else
        {
            CursorData targetCursor = cursorData[(int)type - 1];
            Cursor.SetCursor(targetCursor.cursorTex, targetCursor.hotspot, mode);
        }
    }
}
