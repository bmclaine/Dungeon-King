using UnityEngine;
using System.Collections;

public class SlidingTexture : MonoBehaviour 
{
    [SerializeField]
    private float xSpeed;
    [SerializeField]
    private float ySpeed;
    [SerializeField]
    new private Renderer renderer;

	private void Start () 
    {
        Material mat = new Material(renderer.material);
        renderer.material = mat;
	}
	
	private void Update () 
    {
        Vector2 textureOffset = renderer.material.mainTextureOffset;
        textureOffset.y += ySpeed * Time.deltaTime;
        textureOffset.x += ySpeed * Time.deltaTime;

        renderer.material.mainTextureOffset = textureOffset;
	}
}
