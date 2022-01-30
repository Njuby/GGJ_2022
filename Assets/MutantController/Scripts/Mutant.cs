using UnityEngine;
using System;
using Assets.MutantController.Scripts;
using UnityAtoms.BaseAtoms;

public class Mutant : MonoBehaviour
{
    public IntEvent MutantLevelChange;

    public event Action OnMutationModeActivated;

    public event Action OnMutationModeDectivated;

    public float TimeValue = 1f;

    public float baseDecreaseStrengthBy = 1f;
    public float DecreaseStrengthBy = 1f;

    public MutantStrengthBar mutationStrengthBar;

    [SerializeField]
    public float maxMutantStrength = 50f;

    [SerializeField]
    public float CurrentMutantStrength;

    [SerializeField]
    private int mutantLevel = 4;

    [SerializeField]
    private bool isMutantMode = false;

    public int increaseOnCOllect;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentMutantStrength = 5;
        DecreaseStrengthBy = baseDecreaseStrengthBy;
        MutantLevelChange.Raise((int)CurrentMutantStrength);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMutantMode)
        {
            MutationModeDuration();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMutantMode)
            {
                MutationModeDectivated();

            }
            else
            {
                MutationModeActivated();
            }

        }
    }

    private void MutationModeActivated()
    {
        if (CurrentMutantStrength <= 0) return;

        isMutantMode = true;
        Debug.Log("Mutation Mode Activated");
        mutationStrengthBar.SetMutantStrength(CurrentMutantStrength);
        OnMutationModeActivated?.Invoke();
    }
    private void MutationModeDectivated()
    {
        isMutantMode = false;
        Debug.Log("Mutation Mode Deactivated");
        OnMutationModeDectivated?.Invoke();
        DecreaseStrengthBy = baseDecreaseStrengthBy;
    }
    private void MutationModeDuration()
    {
        TimeValue -= Time.deltaTime;
        if (TimeValue < 0)
        {
            DecreaseMutantStrength(DecreaseStrengthBy);
            DecreaseStrengthBy++;
            TimeValue = 1f;

            if (CurrentMutantStrength <= 0)
            {
                MutationModeDectivated();
            }
        }

    }
    public void IncreaseMutantStrength(float strength)
    {
        CurrentMutantStrength += strength;
        MutantLevelChange.Raise((int)CurrentMutantStrength);
        mutationStrengthBar.SetMutantStrength(CurrentMutantStrength);
    }

    public void DecreaseMutantStrength(float strength)
    {
        CurrentMutantStrength -= strength;
        MutantLevelChange.Raise((int)CurrentMutantStrength);
        mutationStrengthBar.SetMutantStrength(CurrentMutantStrength);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mutancy"))
        {
            //Increase matancy of player
            Destroy(other.transform.parent.gameObject);
            IncreaseMutantStrength(increaseOnCOllect);
        }
    }
}

