using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTrigger : MonoBehaviour
{
    public TextMeshProUGUI displayText; // Textコンポーネントを使用する
    [SerializeField] private float Time = 3.0f;//デフォルトは3秒

    private void Start()
    {
        displayText.enabled = false; // 開始時にテキストを非表示にする
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowText(); // プレイヤーがエリアに入っており、テキストがまだ表示されていない場合は表示する
            Invoke("HideText", Time); // Time秒後に非表示にする
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideText(); // プレイヤーがエリアから出たら非表示にする
        }
    }

    private void ShowText()
    {
        displayText.enabled = true; // テキストを表示する
    }

    private void HideText()
    {
        displayText.enabled = false; // テキストを非表示にする
    }
}