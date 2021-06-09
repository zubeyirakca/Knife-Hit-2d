using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed;
    public Rigidbody2D myRigidbody2D;

    public bool IsReleased { get; set; }
    public bool Hit { get; set; }


    // Update is called once per frame


    public void FireKnife()
    {
        if (!IsReleased)
        {
            IsReleased = true;
            myRigidbody2D.AddForce(new Vector2(x: 0f, y: speed), ForceMode2D.Impulse);
            SoundManager.Instance.PlayKnifeFireHit();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wheel") && !Hit && !GameManager.Instance.IsGameOver && IsReleased)
        {


            other.gameObject.GetComponent<Wheel>().KnifeHit(this);
            GameManager.Instance.Score++;
            SoundManager.Instance.PlayWheelHit();
        }

        else if (other.gameObject.CompareTag("Knife") && !Hit && !GameManager.Instance.IsGameOver && other.gameObject.GetComponent<Knife>().IsReleased)
        {
            Hit = true;
            transform.SetParent(other.transform);
            myRigidbody2D.velocity = Vector2.zero;
            myRigidbody2D.isKinematic = true;

            SoundManager.Instance.PlayGameOver();
            SoundManager.Instance.Vibrate();
            GameManager.Instance.IsGameOver = true;
            Invoke(nameof(GameOver), time: 0.5f);
        }
    }
     
    private void GameOver()
    {
        UIManager.Instance.GameOver();
    }
}
