using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    public float speed;
    float length;

    BoxCollider2D collider;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        length = collider.size.x;
    }

    void Update()
    {
        if(GameManager.Instance.GameOver == false)
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);

            if (transform.position.x <= -length)
            {
                transform.position = new Vector2(transform.position.x + (2 * length), transform.position.y);
            }
        }
    }
}
