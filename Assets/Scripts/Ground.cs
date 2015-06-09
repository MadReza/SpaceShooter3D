using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    public float MaxScrollSpeed = 1.0f;
    private float scrollSpeed;
    private Vector2 savedOffset;
    private Renderer _renderer;
    private float initialTime;

    // Use this for initialization
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        savedOffset = _renderer.sharedMaterial.GetTextureOffset("_MainTex");
        initialTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (scrollSpeed < MaxScrollSpeed)
            scrollSpeed = (Time.time - initialTime)*0.10f;

        float y = Mathf.Repeat((Time.time - initialTime)*scrollSpeed, 1);
        Vector2 offset = new Vector2(savedOffset.x, y);
        _renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    private void OnDisable()
    {
        _renderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

}
