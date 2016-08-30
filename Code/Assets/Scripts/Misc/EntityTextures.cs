using UnityEngine;
using System.Collections;

public class EntityTextures : ScriptableObject 
{
    [SerializeField]
    private Texture[] textures;

    public Texture this[Element element]
    {
        get
        {
            int index = (int)element;
            return textures[index];
        }
    }

    public Texture GetTexture(Element element)
    {
        int index = (int)element;

        return textures[index];
    }
}
