using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [SerializeField] GameObject Sunset;
    [SerializeField] GameObject Night;
    [SerializeField] GameObject BeforeSunset;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sunset.SetActive(false);
        Night.SetActive(false);
        BeforeSunset.SetActive(false);
        DayPhase end = GameManager.Instance.currentTime;

        switch (end) {
            case (DayPhase.Morning):
                BeforeSunset.SetActive(true);
                break;
            case (DayPhase.Noon):
                BeforeSunset.SetActive(true);
                break;
            case (DayPhase.Sunset):
                Sunset.SetActive(true);
                break;
            case (DayPhase.Night):
                Night.SetActive(true);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
