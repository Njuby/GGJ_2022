using UnityEngine;
using System;
using Assets.MutantController.Scripts;

public class Mutant : MonoBehaviour
{

    public event Action OnMutationModeActivated;

    public event Action OnMutationModeDectivated;

    public float TimeValue = 1f;

    public float DecreaseStrengthBy = 10f;

    public MutantStrengthBar mutationStrengthBar;

    [SerializeField]
    private float maxMutantStrength = 50f;

    [SerializeField]
    private float CurrentMutantStrength;

    [SerializeField]
    private int mutantLevel = 4;

    [SerializeField]
    private bool isMutantMode = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentMutantStrength = maxMutantStrength;
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
    }
    private void MutationModeDuration()
    {
        TimeValue -= Time.deltaTime;
        if (TimeValue < 0)
        {
            DecreaseMutantStrength(DecreaseStrengthBy);
            TimeValue = 1f;
        }

    }
    private void IncreaseMutantStrength(float strength)
    {
        CurrentMutantStrength += strength;
        mutationStrengthBar.SetMutantStrength(CurrentMutantStrength);
        
    }

    private void DecreaseMutantStrength(float strength)
    {
        CurrentMutantStrength -= strength;
        mutationStrengthBar.SetMutantStrength(CurrentMutantStrength);
    }

}
