namespace BlazorAppRadzenLoading.Components;

public class LoadingIndicatorOptions
{
    public LoadingIndicatorOptions(
        bool startAfterRender = true,
        bool showStepNumbers = false, bool showPercentage = true,
        int currentStep = 0, int totalSteps = 1)
    {
        StartAfterRender = startAfterRender;
        ShowStepNumbers = showStepNumbers;
        ShowPercentage = showPercentage;
        CurrentStep = currentStep;
        TotalSteps = totalSteps;
    }

    public Action? UpdateAction { get; set; }

    private bool startAfterRender;

    public bool StartAfterRender
    {
        get => startAfterRender;
        set
        {
            startAfterRender = value;
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }

    private bool showStepNumbers;

    public bool ShowStepNumbers
    {
        get => showStepNumbers;
        set
        {
            showStepNumbers = value;
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }

    private bool showPercentage;

    public bool ShowPercentage
    {
        get => showPercentage;
        set
        {
            showPercentage = value;
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }

    private int currentStep;

    public int CurrentStep
    {
        get => currentStep;
        set
        {
            currentStep = value;
            CurrentPercent = (int)(((float)currentStep / (float)totalSteps) * 100);
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }

    private int currentPercent;

    public int CurrentPercent
    {
        get => currentPercent;
        set
        {
            currentPercent = value;
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }

    private int totalSteps = 1;

    public int TotalSteps
    {
        get => totalSteps;
        set
        {
            totalSteps = value;
            if (UpdateAction is not null) UpdateAction.Invoke();
        }
    }
}