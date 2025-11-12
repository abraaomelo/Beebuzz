using System;
using System.Collections;
using UnityEngine;

public class BeeHiveExit : MonoBehaviour
{
    [Header("Scene")]
    public String sceneName;
    [Header("Shrink Settings")]
    public string targetTag = "Player";
    public float shrinkSpeed = 2f;     
    public float minScale = 0.2f;     
    public float moveSpeed = 3f;      
    public Transform hiveHole;        
    private GameObject player;
    private bool isShrinking = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(targetTag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject joystick = GameObject.FindGameObjectWithTag("Joystick");
            if (joystick != null) joystick.SetActive(false);

            StartCoroutine(MoveAndShrinkToHiveHole());
        }
    }

    private IEnumerator MoveAndShrinkToHiveHole()
    {
        isShrinking = true;
        Vector3 targetScale = Vector3.one * minScale;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        while (isShrinking && player != null && hiveHole != null)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                hiveHole.position,
                moveSpeed * Time.deltaTime
            );

            player.transform.localScale = Vector3.MoveTowards(
                player.transform.localScale,
                targetScale,
                shrinkSpeed * Time.deltaTime
            );

            float distance = Vector3.Distance(player.transform.position, hiveHole.position);
            if (distance < 0.05f)
            {
                player.transform.position = hiveHole.position;
                player.transform.localScale = targetScale;
                isShrinking = false;

                SceneFader.Instance.FadeToScene(sceneName);
            }

            yield return null;
        }
    }
}
