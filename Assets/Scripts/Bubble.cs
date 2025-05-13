using System.Collections;
using DG.Tweening;
using UnityEngine;


public class Bubble : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    public float minDeceleration;
    public float maxDeceleration;

    public float linearDragMin;
    public float linearDragMax;

    public float upwardGravityMin;
    public float upwardGravityMax;

    public float bubbleMinSize;
    public float bubbleMaxSize;

    public float bubbleLifeTime;


    Rigidbody2D myRB;

    public GameObject bubbleDestroyParticle;

    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myRB.linearDamping = Random.Range(linearDragMin, linearDragMin);
    }

    void Start()
    {
        Destroy(gameObject, bubbleLifeTime);
    }



    public void Released(Vector2 playerDirection, Vector2 currentLinearVelocityPlayerX)
    {
        Expand()
; var targetSpeed = Random.Range(minSpeed, maxSpeed);
        myRB.linearVelocity = currentLinearVelocityPlayerX + (RandomizeDirection(playerDirection, Mathf.Abs(currentLinearVelocityPlayerX.y) * 6f) * targetSpeed);
        StartCoroutine(BubbleShot());
    }

    void Expand()
    {
        transform.localScale = Vector2.one * .5f;
        var targetSize = Random.Range(bubbleMinSize, bubbleMaxSize);
        transform.DOScale(targetSize, 1f).SetDelay(.05f).SetEase(Ease.OutBack);
    }

    void OnDestroy()
    {
        SoundManager.Instance.PlaySFX("popBurst", .3f);
        transform.DOKill();
        var ps = Instantiate(bubbleDestroyParticle, transform.position, Quaternion.identity);
        ps.transform.localScale = transform.localScale;
    }

    public static Vector2 RandomizeDirection(Vector2 direction, float angleRange = 15f)
    {
        // Ensure the direction is either left or right
        if (direction != Vector2.right && direction != Vector2.left)
        {
            Debug.LogWarning("Direction should be either Vector2.right or Vector2.left.");
            return direction;
        }
        float randomAngle = 0;
        if (direction == Vector2.right)
        {
            // Generate a random angle within the specified range
            randomAngle = Random.Range(-angleRange - 5, -angleRange);
        }
        else
        {
            randomAngle = Random.Range(angleRange - 5, angleRange);
        }

        // Rotate the direction by the random angle
        return Quaternion.Euler(0, 0, randomAngle) * direction;
    }

    IEnumerator BubbleShot()
    {
        while (myRB.linearVelocity.magnitude < .2f)
        {
            yield return null;
        }
        //yield return new WaitForSeconds(.7f);
        myRB.gravityScale = Random.Range(upwardGravityMin, upwardGravityMax);
    }

}
