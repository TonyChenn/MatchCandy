using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Sound : MonoBehaviour
{
    [SerializeField]
    Trigger trigger = Trigger.OnClick;
    [SerializeField]
    UI_SoundType type = UI_SoundType.common;
    public enum Trigger
    {
        OnClick,
        OnPress,
        OnRelease,
        OnEnable,
        OnDisable,
    }

    void OnClick()
    {
        if (trigger == Trigger.OnClick)
            Play();
    }
    void OnPress(bool isPressed)
    {
        if ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease))
            Play();
    }

    void OnEnable()
    {
        if (trigger == Trigger.OnEnable)
            Play();
    }
    void OnDisable()
    {
        if (trigger == Trigger.OnDisable)
            Play();
    }

    void Play()
    {
        AudioManger.Singlton.OnPlayUIButtonSound(UI_SoundType.common);
    }
}
