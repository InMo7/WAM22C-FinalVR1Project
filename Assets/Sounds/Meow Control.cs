using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MeowDrumHit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Header("Hit Settings")]
    public float minHitVelocity = 1f;

    private XRGrabInteractable grab;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        grab = GetComponent<XRGrabInteractable>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return;
        
        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < minHitVelocity) return;

        audioSource.PlayOneShot(hitSound);
    }
}