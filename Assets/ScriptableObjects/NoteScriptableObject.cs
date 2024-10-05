using UnityEngine;

[CreateAssetMenu(fileName = "NoteScriptableObject", menuName = "ScriptableObjects/Note")]
public class NoteScriptableObject : ScriptableObject
{
    [TextArea]
    [SerializeField] string scriptureText = "yo, waddup";
    [TextArea]
    [SerializeField] string loreText = "omg lol";
    
    public string GetScriptureText()
    {
        return scriptureText;
    }

    public string GetLoreText()
    {
        return loreText;
    }
}
