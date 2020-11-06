using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{    
    public float moveSpeed = 10f;
    public TextMeshPro notification;
    public AudioClip collectAudio;
    public AudioClip errorAudio;

    private int _health = 3;
    private int _points;
    private float _moveMin, _moveMax;
    private Animator _animator;
    private AudioSource _audioSource;

    //touch input variables
    private Vector2 startPos;
    private Vector2 direction;

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            GameManager.Instance.UpdateHealth(value);
        }
    }

    public int Points
    {
        get => _points;
        set
        {
            _points = value;
            GameManager.Instance.UpdatePoints(value);
            if (value > 50)
            {
                GameManager.Instance.spawnDelay = 0.15f;
            }
        }
    }

    private void Start()
    {
        Vector3 min = Camera.main.ScreenToWorldPoint(new Vector2(100, 0));
        Vector3 max = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - 100, 0));

        _moveMin = min.x;
        _moveMax = max.x;

        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.DisableInput) return;

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Ended:
                    direction = Vector2.zero;
                    _animator.ResetTrigger("Walk");
                    _animator.SetTrigger("Idle");
                    break;
            }

            if (direction.x < 0)
            {
                if (transform.position.x > _moveMin)
                {
                    _animator.ResetTrigger("Idle");
                    _animator.SetTrigger("Walk");
                    transform.Translate(Vector3.left * 10 * Time.deltaTime);
                }
            }
            else if (direction.x > 0)
            {
                if (transform.position.x < _moveMax)
                {
                    _animator.ResetTrigger("Idle");
                    _animator.SetTrigger("Walk");
                    transform.Translate(Vector3.right * 10 * Time.deltaTime);
                }
            }
        }
#else
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(transform.position.x > _moveMin)
            {
                _animator.ResetTrigger("Idle");
                _animator.SetTrigger("Walk");
                transform.position += Vector3.left * Time.deltaTime * moveSpeed;
            }            
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            if(transform.position.x < _moveMax)
            {
                _animator.ResetTrigger("Idle");
                _animator.SetTrigger("Walk");
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            }            
        }
        else
        {
            _animator.ResetTrigger("Walk");
            _animator.SetTrigger("Idle");
        }
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Mesh")
        {            
            ShapeGenerator mesh = collision.GetComponent<ShapeGenerator>();
            
            if (mesh.GetSides == 6)
            {
                Health--;

                TextMeshPro textMesh = Instantiate(notification, mesh.transform.position, Quaternion.identity);
                textMesh.text = "-1 heart";
                textMesh.color = Color.red;
                Destroy(textMesh.gameObject, 1f);

                _audioSource.PlayOneShot(errorAudio);
            }
            else
            {
                Points += mesh.GetSides;

                TextMeshPro textMesh = Instantiate(notification, mesh.transform.position, Quaternion.identity);
                textMesh.text = "+" + mesh.GetSides.ToString();
                Destroy(textMesh.gameObject, 1f);

                _audioSource.PlayOneShot(collectAudio);
            }

            MeshSpawner.Instance.AddToPool(mesh);
        }
    }
}
