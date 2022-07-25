using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class FlipOnJump : MonoBehaviour
{
    private UnityAction jumpListener;

    private float rot;
    private float targetRot;
    private float step;
    private float speed = 0.5f;
    private bool hasJumped = true;
    public bool goingUpsidown;

    private float timer;
    private float delay = 0.15f;

    void Start()
    {
        if(goingUpsidown)
        {
            targetRot = 180f;
        }
        else
        {
            targetRot = 0f;
        }

        jumpListener = new UnityAction(Flip);
    }

    void OnEnable()
    {
        EventManager.StartListening("jump", Flip);
    }

    void OnDisable()
    {
        EventManager.StopListening("jump", Flip);
    }

    private void Flip()
    {
        goingUpsidown = !goingUpsidown;
        hasJumped = true;

        timer = delay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (hasJumped && timer < 0f)
        {
            if (goingUpsidown)
            {
                step += Time.deltaTime * speed;
                rot = Mathf.Lerp(rot, targetRot, step);
                transform.eulerAngles = Vector3.forward * rot;
                if (rot >= targetRot - 1)
                {
                    rot = targetRot;
                    targetRot = 360f;
                    step = 0f;
                    transform.eulerAngles = Vector3.forward * rot;

                    hasJumped = false;
                    timer = delay;
                }
            }
            else if(!goingUpsidown)
            {
                step += Time.deltaTime * speed;
                rot = Mathf.Lerp(rot, targetRot, step);
                transform.eulerAngles = Vector3.forward * rot;
                if (rot >= targetRot - 1)
                {
                    rot = 0f;
                    targetRot = 180f;
                    step = 0f;
                    transform.eulerAngles = Vector3.forward * rot;

                    hasJumped = false;
                    timer = delay;

                }

            }
        }
    }

}
