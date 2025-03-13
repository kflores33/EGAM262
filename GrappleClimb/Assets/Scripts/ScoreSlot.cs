using UnityEngine;
using TMPro;


public class ScoreSlot : MonoBehaviour
{
    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI rankLabel;

    public void Setup(NameAndScore s, int rank)
    {
        rankLabel.text = $"{rank}.";
        NameLabel.text = s.Name;
        ScoreLabel.text = $"{s.Score:N0}";
    }
}
