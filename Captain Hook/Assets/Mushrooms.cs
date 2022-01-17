using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour
{
    public MushroomType type;

    private Animator animator;

    private bool entering = true;

    void Start()
    {
        animator = GetComponent<Animator>();

        if(type == MushroomType.Blue)
        {
            animator.SetBool("Blue", true);
        }
        else if(type == MushroomType.Red)
        {
            animator.SetBool("Red", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && entering)
        {
            animator.SetTrigger("Brush");

            entering = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !entering)
        {
            animator.SetTrigger("Brush");

            entering = true;
        }
    }
}

    public enum MushroomType
    {
        Blue,
        Red,
        Green,
    }
