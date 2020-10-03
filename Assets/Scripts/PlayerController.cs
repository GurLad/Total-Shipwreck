using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController Current;
    [Header("Movement")]
    public float Speed;
    public float Force;
    public float ResetForce;
    public float Accuracy;
    public float RecordRate;
    [Header("Items")]
    public float DistanceToCheck;
    public float SphereRadius;
    public float CheckHeight;
    [Header("Animations")]
    public AdvancedAnimation IdleAnimation;
    public AdvancedAnimation WalkAnimation;
    public AdvancedAnimation HoldAnimation;
    [Header("Misc")]
    public ParticleSystem RecordParticle;
    [HideInInspector]
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            if (active == value)
            {
                return;
            }
            active = value;
            if (active)
            {
                Current.Active = false;
                if (Current.recording)
                {
                    Current.SetRecording(false);
                }
                Current = this;
            }
        }
    }
    [SerializeField]
    private bool active;
    private Rigidbody rigidbody;
    private Pickup item;
    private bool recording;
    private List<Action> recordedActions;
    private float count;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        IdleAnimation.Activate();
        recordedActions = new List<Action>();
        if (active)
        {
            Current = this;
        }
    }
    protected virtual void Update()
    {
        if (Active)
        {
            // Set record
            if (Control.GetButtonDown(Control.CB.Record))
            {
                SetRecording(!recording);
            }
            // Record move
            if (recording)
            {
                count += Time.deltaTime * RecordRate;
                if (count >= 1)
                {
                    count -= 1;
                    RecordMove();
                }
            }
            // Move
            Vector3 direction = Move();
            if (direction.magnitude > 0.01f)
            {
                transform.LookAt(transform.position + direction);
            }
            // Use items
            if (Control.GetButtonDown(Control.CB.Use) && item != null)
            {
                if (recording)
                {
                    RecordUse();
                }
                new List<Trigger>(item.GetComponents<Trigger>()).ForEach(a => a.Activate());
            }
            // Items
            if (Control.GetButtonDown(Control.CB.Pickup))
            {
                if (item == null)
                {
                    Vector3 checkPos = transform.position + direction.normalized * DistanceToCheck;
                    checkPos.y = transform.position.y;
                    Collider[] colliders = Physics.OverlapBox(checkPos, new Vector3(SphereRadius, CheckHeight, SphereRadius));
                    List<Pickup> pickups = new List<Pickup>();
                    foreach (var item in colliders)
                    {
                        pickups.AddRange(item.GetComponents<Pickup>());
                    }
                    if (pickups.Count > 0)
                    {
                        if (recording)
                        {
                            RecordPick();
                        }
                        Pickup(pickups[0]);
                    }
                }
                else
                {
                    if (recording)
                    {
                        RecordPick();
                    }
                    Drop(item);
                }
            }
            // Record

        }
    }
    private Vector3 Move()
    {
        float tempY = rigidbody.velocity.y;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if (rigidbody.velocity.magnitude > Speed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * Speed;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, tempY, rigidbody.velocity.z);
        Vector3 TargetVelocity = new Vector3(Control.GetAxis(Control.Axis.X), 0, Control.GetAxis(Control.Axis.Y)).normalized * Speed;
        if (TargetVelocity == Vector3.zero)
        {
            Vector3 force = -rigidbody.velocity * ResetForce;
            force.y = 0;
            rigidbody.AddForce(force);
            if (!IdleAnimation.Active)
            {
                IdleAnimation.Activate(true);
                WalkAnimation.Active = false;
            }
            return Vector3.zero;
        }
        if (!WalkAnimation.Active)
        {
            WalkAnimation.Activate(true);
            IdleAnimation.Active = false;
        }
        TargetVelocity.y = rigidbody.velocity.y;
        if ((rigidbody.velocity - TargetVelocity).magnitude <= 1 / Accuracy)
        {
            rigidbody.velocity = TargetVelocity;
        }
        else
        {
            Vector3 force = TargetVelocity * Force;
            force.y = 0;
            rigidbody.AddForce(force);
        }
        Vector3 temp = rigidbody.velocity;
        temp.y = 0;
        return temp;
    }
    public void Pickup(Pickup pickup)
    {
        item = pickup;
        pickup.Pick();
        pickup.transform.parent = transform;
        pickup.transform.localPosition = new Vector3(0, 0, 1);
        HoldAnimation.Activate(true);
    }
    public void Drop(Pickup pickup)
    {
        item = null;
        pickup.transform.parent = null;
        pickup.Drop();
        HoldAnimation.Active = false;
    }
    private void SetRecording(bool value)
    {
        recording = value;
        GameController.Current.SetRecording(recording);
        if (recording)
        {
            recordedActions.Clear();
            count = 1;
        }
    }
    private void RecordMove()
    {
        recordedActions.Add(new Action(ActionType.Move, transform.position));
        Record(Color.white);
    }
    private void RecordPick()
    {
        recordedActions.Add(new Action(ActionType.Pick));
        Record(Color.green);
    }
    private void RecordUse()
    {
        recordedActions.Add(new Action(ActionType.Use));
        Record(Color.yellow);
    }
    private void Record(Color color)
    {
        ParticleSystem.MainModule particleSystem = Instantiate(RecordParticle, transform.position, Quaternion.identity).main;
        particleSystem.startColor = color;
    }
}
