using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CTAnimPlayer : MonoBehaviour
{ 
    public Animator animator;
    private NavMeshAgent agent;
    
    public enum CharacterAnimation
    {
        Interact,
        Stab,
        Wonder,
        Death,
        Land,
    }
    
    private const string Anim_Interact = "Interact";
    private const string Anim_Stab = "Stab";
    private const string Anim_Wonder = "Wonder";
    private const string Anim_Death = "Death";
    private const string Anim_Land = "Land";
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator && agent)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("speed", speed);
        }
    }
    
    public void PlayAnimation(CharacterAnimation AnimToPlay)
    {  
        animator.Play(ToAnimatorStateName(AnimToPlay));
        Debug.Log("Playing anim: " + AnimToPlay);
    }
    
    public static string ToAnimatorStateName(CharacterAnimation anim)
    {
        switch (anim)
        {
            case CharacterAnimation.Interact: return Anim_Interact;
            case CharacterAnimation.Stab: return Anim_Stab;
            case CharacterAnimation.Wonder: return Anim_Wonder;
            case CharacterAnimation.Death: return Anim_Death;
            case CharacterAnimation.Land: return Anim_Land;
            default: return string.Empty;
        }
    }
}