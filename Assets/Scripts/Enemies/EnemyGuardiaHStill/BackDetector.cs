using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDetector : MonoBehaviour
{
    public bool playerIsOnBack = false;

    private Animator anim;

    [Header("Back Detector Distance")]
    [SerializeField] private BoxCollider2D detectorCollider;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float detectorRangeY;
    [SerializeField] private float detectorRangeX;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private FourDirectionOnRanged onRanged;
    [SerializeField] private EnemyMeleeController onRangedMelee;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        detectorCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (PlayerDetectected())
        {
            anim.SetFloat("Horizontal", 0f);
            playerIsOnBack = true;
            anim.SetFloat("Vertical", 1f);
        }
        else
        {
            playerIsOnBack = false;
        }
    }

    public bool PlayerDetectected()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(detectorCollider.bounds.center + transform.right * detectorRangeX * transform.localScale.x * colliderDistance,
            new Vector3(detectorCollider.bounds.size.x * detectorRangeX, detectorCollider.bounds.size.y * detectorRangeY, detectorCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        // Range Weapon Detection
        if (onRanged != null)
        {
            if (onRanged.currentlyTargetObj == null || hit.collider == null) return false;

            return hit.collider.gameObject == onRanged.currentlyTargetObj;
        }

        // Melee Weapon Detection
        if (onRangedMelee.currentlyTargetObj == null || hit.collider == null) return false;


        return hit.collider.gameObject == onRangedMelee.currentlyTargetObj;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detectorCollider.bounds.center + transform.right * detectorRangeX * transform.localScale.x * colliderDistance,
            new Vector3(detectorCollider.bounds.size.x * detectorRangeX, detectorCollider.bounds.size.y * detectorRangeY, detectorCollider.bounds.size.z));
    }
}
