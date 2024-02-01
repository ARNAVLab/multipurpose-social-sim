using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Singleton GameObject component mostly responsible for pairing string identifiers with cursor sprites,
 * and statically allowing other scripts to change the cursor currently being used.
 */
public class CursorAtlas : MonoBehaviour
{
    // The singleton CursorAtlas instance.
    public static CursorAtlas instance;
    // The static Dictionary that pairs string IDs to cursor data.
    private static Dictionary<string, CursorData> cursorAtlas;
    // String keys by which to refer to the data in cursorAtlas.
    [SerializeField] private string[] cursorKeys;
    // Data about cursor sprites and their hotspot information.
    [SerializeField] private CursorData[] cursorData;

    [System.Serializable]
    private class CursorData
    {
        // The texture used for this particular cursor.
        public Texture2D cursorTex;
        // Which pixel of the above texture should be centered on the point of interaction.
        public Vector2 hotspot;
    }

    /**
     * On startup, assigns the static instance of the singleton and generates the static CursorAtlas.
     */
    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (cursorKeys.Length != cursorData.Length)
        {
            Debug.LogError("Key/Value arrays do not match for CursorAtlas!");
            return;
        }

        instance = this;
        cursorAtlas = new Dictionary<string, CursorData>();
        for (int i = 0; i < cursorKeys.Length; i++)
        {
            cursorAtlas.Add(cursorKeys[i], cursorData[i]);
        }
    }

    /**
     * Given a string and a cursor mode, searches the atlas for a cursor by that name and sets the current
     * cursor according to the corresponding CursorData.
     * 
     * @param type is the string ID for the desired CursorData
     * @param mode is the CursorMode the cursor should be set in
     */
    public void SetCursor(string type, CursorMode mode)
    {
        if (type.Equals("default"))
        {
            Cursor.SetCursor(null, Vector2.zero, mode);
            return;
        }

        CursorData targetCursor;
        cursorAtlas.TryGetValue(type, out targetCursor);

        if (targetCursor == null)
        {
            Debug.LogWarning("Invalid cursor type string!");
            return;
        }

        Cursor.SetCursor(targetCursor.cursorTex, targetCursor.hotspot, mode);
    }
}
