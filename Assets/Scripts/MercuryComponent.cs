using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class MercuryComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Animator to controll the gameobject")]
    private Animator _Animator;
    private PlayableController _Controller;
    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    public Animator GetAnimator() {  return _Animator; }
    public bool TryGetAnimator(out Animator animator) { animator = _Animator;return _Animator != null; }
    public void PlayAnimation(AnimationClip clip)
        => _Controller.Play(clip);

}
