using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarConsolePlugin;

public class CatAnimChanger : MonoBehaviour
{
    int currentIndex = 0;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        LunarConsole.RegisterAction("PlayNext", () => SendMessage("PlayNext")); ;
        LunarConsole.RegisterAction("PlayPrev", () => SendMessage("PlayPrev"));
    }

    void OnDestroy()
    {
        LunarConsole.UnregisterAllActions(this);
    }

    public void PlayNext()
    {
        currentIndex++;
        currentIndex %= animator.runtimeAnimatorController.animationClips.Length;
        Play();
    }

    public void PlayPrev()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex += animator.runtimeAnimatorController.animationClips.Length;
        }
        Play();
    }

    void Play()
    {
        animator.CrossFade(animator.runtimeAnimatorController.animationClips[currentIndex].name, 0.15f);
    }
}
