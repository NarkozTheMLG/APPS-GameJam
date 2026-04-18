using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIParticleBurst : MonoBehaviour
{
    public void Play(Color color, Sprite shardSprite)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject shard = new GameObject("Shard");
            shard.transform.SetParent(this.transform);
            
            Image img = shard.AddComponent<Image>();
            img.sprite = shardSprite;
            img.color = color;
            img.raycastTarget = false;

            RectTransform rt = shard.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(20, 20);
            rt.anchoredPosition = Vector2.zero;

            var logic = shard.AddComponent<UIPhysicalPiece>();
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f)).normalized;
            logic.Setup(dir * Random.Range(400f, 700f));
        }
        Destroy(gameObject, 1.5f); 
    }
}

public class UIPhysicalPiece : MonoBehaviour
{
    private Vector2 vel;
    private float gravity = -1800f;
    private float rot;

    public void Setup(Vector2 startVel)
    {
        vel = startVel;
        rot = Random.Range(-300f, 300f);
    }

    void Update()
    {
        vel.y += gravity * Time.deltaTime;
        transform.Translate(vel * Time.deltaTime);
        transform.Rotate(0, 0, rot * Time.deltaTime);
        
        Image img = GetComponent<Image>();
        Color c = img.color;
        c.a -= Time.deltaTime;
        img.color = c;
    }
}