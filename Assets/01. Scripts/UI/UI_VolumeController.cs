using UnityEngine;

public class UI_VolumeController : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private GameObject m_SliderPrefab;
    [SerializeField] private Transform m_Content;

    private void Start()
    {
        int soundCount = System.Enum.GetValues(typeof(SoundType)).Length;

        for (int i = 1; i < soundCount; i++)
        {
            var type = (SoundType)i;

            GameObject newObj = Instantiate(m_SliderPrefab, m_Content);

            newObj.name = $"Slider_{type}";
            var slider = newObj.GetComponent<UI_SoundSlider>();
            slider.TypeSet(type);
            slider.Init();
        }
    }
}