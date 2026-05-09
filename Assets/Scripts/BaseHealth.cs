using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public static BaseHealth Instance;
    public int hp = 100;
    private int _maxHP;

    [Header("Audio")]
    public AudioClip hitSound;
    private float _lastHitSoundTime = -Mathf.Infinity;
    public float hitSoundCooldown = 2f;

    void Awake()
    {
        Instance = this;
        _maxHP = hp;
    }

    public void ResetHP()
    {
        hp = _maxHP;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Base HP: " + hp);

        if (hitSound != null && Time.time > _lastHitSoundTime + hitSoundCooldown)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            _lastHitSoundTime = Time.time;
        }

        if (hp <= 0)
            GameManager.Instance.ChangeState(GameState.GameOver);
    }
}