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
    public float RecordAccuracy;
    public float RecordTooHighThresold;
    [Header("Items")]
    public float DistanceToCheck;
    public float SphereRadius;
    public float CheckHeight;
    [Header("Animations")]
    public AdvancedAnimation IdleAnimation;
    public AdvancedAnimation WalkAnimation;
    public AdvancedAnimation HoldAnimation;
    [Header("Voice lines")]
    public float Pitch;
    public AudioClip TooHigh;
    public AudioClip Stuck;
    public AudioClip NoPickup;
    public AudioClip NoUse;
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
    // Recording stuff
    private bool recording;
    private List<Action> recordedActions;
    private float count;
    private List<GameObject> recordIndicators;
    // Following record stuff
    private bool followingRecord;
    private int currentStep;
    private Vector2 previousInput;
    private float stuckTime;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        IdleAnimation.Activate();
        recordedActions = new List<Action>();
        recordIndicators = new List<GameObject>();
        if (active)
        {
            Current = this;
        }
    }
    protected virtual void Update()
    {
        if (Active)
        {
            if (!followingRecord)
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
                        RecordMove(true);
                    }
                }
                // Move
                Vector3 direction = new Vector3(Control.GetAxis(Control.Axis.X), 0, Control.GetAxis(Control.Axis.Y)).normalized;
                Move(direction);
                //direction = (direction + Move(direction)).normalized;
                if (direction.magnitude > 0.01f)
                {
                    transform.LookAt(transform.position + direction);
                }
                // Use items
                if (Control.GetButtonDown(Control.CB.Use))
                {
                    UseAction();
                }
                // Items
                if (Control.GetButtonDown(Control.CB.Pickup))
                {
                    PickupAction(direction);
                }
            }
            else
            {
                Vector2 control = new Vector2(Control.GetAxis(Control.Axis.X), Control.GetAxis(Control.Axis.Y));
                if (control != Vector2.zero && control != previousInput)
                {
                    followingRecord = false;
                    recordedActions.Clear();
                }
            }
        }
        if (followingRecord)
        {
            switch (recordedActions[currentStep].Type)
            {
                case ActionType.Move:
                    if (rigidbody.velocity.magnitude <= 0.2f)
                    {
                        stuckTime += Time.deltaTime;
                        if (stuckTime >= 0.5f)
                        {
                            SoundController.PlaySound(Stuck, Pitch);
                            followingRecord = false;
                            recordedActions.Clear();
                        }
                    }
                    else
                    {
                        stuckTime = 0;
                    }
                    Vector3 pos = recordedActions[currentStep].pos;
                    float y = pos.y;
                    pos.y = transform.position.y;
                    if (Vector3.Distance(pos, transform.position) <= RecordTooHighThresold + 1 && y - transform.position.y >= RecordTooHighThresold)
                    {
                        SoundController.PlaySound(TooHigh, Pitch);
                        followingRecord = false;
                        recordedActions.Clear();
                    }
                    transform.LookAt(pos);
                    Vector3 dir = transform.forward;
                    if (Vector3.Distance(transform.position, pos) <= RecordAccuracy)
                    {
                        currentStep++;
                        currentStep %= recordedActions.Count;
                    }
                    else
                    {
                        Move(dir);
                    }
                    break;
                case ActionType.Pick:
                    if (!PickupAction(recordedActions[currentStep].pos))
                    {
                        SoundController.PlaySound(NoPickup, Pitch);
                    }
                    currentStep++;
                    currentStep %= recordedActions.Count;
                    break;
                case ActionType.Use:
                    if (!UseAction())
                    {
                        SoundController.PlaySound(NoUse, Pitch);
                    }
                    currentStep++;
                    currentStep %= recordedActions.Count;
                    break;
                default:
                    break;
            }
        }
        else if (!Active)
        {
            Move(Vector3.zero);
        }
        previousInput = new Vector2(Control.GetAxis(Control.Axis.X), Control.GetAxis(Control.Axis.Y));
    }
    private Vector3 Move(Vector3 target)
    {
        float tempY = rigidbody.velocity.y;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if (rigidbody.velocity.magnitude > Speed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * Speed;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, tempY, rigidbody.velocity.z);
        Vector3 TargetVelocity = target.normalized * Speed;
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
        return temp.normalized;
    }
    private bool UseAction()
    {
        if (item != null)
        {
            if (recording)
            {
                RecordMove();
                RecordUse();
            }
            new List<Trigger>(item.GetComponents<Trigger>()).ForEach(a => a.Activate());
            return true;
        }
        return false;
    }
    private bool PickupAction(Vector3 direction)
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
                    RecordMove();
                    RecordPick(direction);
                }
                pickups[0].Pick(this);
                return true;
            }
        }
        else
        {
            if (recording)
            {
                RecordMove();
                RecordPick(direction);
            }
            Drop(item);
            return true;
        }
        return false;
    }
    public void Pickup(Pickup pickup)
    {
        item = pickup;
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
            count = 0;
            RecordMove(false);
            Record(Color.blue);
        }
        else
        {
            recordIndicators.ForEach(a => Destroy(a));
            recordIndicators.Clear();
            currentStep = 0;
            followingRecord = true;
        }
    }
    private void RecordMove(bool showIndicator = false)
    {
        recordedActions.Add(new Action(ActionType.Move, transform.position));
        if (showIndicator)
        {
            Record(Color.white);
        }
    }
    private void RecordPick(Vector3 direction)
    {
        recordedActions.Add(new Action(ActionType.Pick, direction));
        Record(Color.green);
    }
    private void RecordUse()
    {
        recordedActions.Add(new Action(ActionType.Use));
        Record(Color.yellow);
    }
    private void Record(Color color)
    {
        ParticleSystem particleSystem = Instantiate(RecordParticle, transform.position, Quaternion.identity);
        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = color;
        recordIndicators.Add(particleSystem.gameObject);
    }
    private bool DifferentDirection(float a, float b)
    {
        return Mathf.Abs(a) <= 0.01f || Mathf.Abs(b) <= 0.01f || Mathf.Sign(a) != Mathf.Sign(b);
    }
}
