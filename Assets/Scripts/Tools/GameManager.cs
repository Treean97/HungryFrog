using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Inst;

    [SerializeField]
    public DataSaveLoad _DataSaveLoad;


    private void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
