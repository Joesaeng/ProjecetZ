using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public enum TextEffectType
    {
        Common,
        Hangul
    }

    public TextEffectType effectType = TextEffectType.Common;

    public RectTransform textBalloonRect;
    public TextMeshProUGUI uiText; // TextMeshProUGUI 컴포넌트
    public string targetText = "안녕하세요"; // 최종 타이핑할 텍스트
    public float typingSpeed = 0.1f; // 타이핑 속도 조절

    public void Typing()
    {
        switch (effectType)
        {
            case TextEffectType.Common:
                ExecuteTypingCommon(targetText).Forget();
                break;
            case TextEffectType.Hangul:
                ExecuteTyping(targetText).Forget(); // UniTask 사용 시 코루틴이 아닌 async 메서드 호출
                break;
        }

    }

    // 말풍선 초기화
    private void TextBalloonInit()
    {
        uiText.text = "";
        Vector2 balloonSize = new Vector2(100,100); // 기본사이즈로 100,100을 잡았다.
        textBalloonRect.sizeDelta = balloonSize;
    }

    // 말풍선 사이즈 조절
    // 현재 채워져있는 글의 너비를 받아오는 preferredWidth를 이용함
    private void SetBalloonSize()
    {
        Vector2 balloonSize = textBalloonRect.sizeDelta;
        balloonSize.x = uiText.preferredWidth + 90;     // 90은 현재 폰트 한글 한글자 사이즈가 이정도 되서 그냥 넣어줌
        textBalloonRect.sizeDelta = balloonSize;
    }

    async UniTaskVoid ExecuteTypingCommon(string fullText)
    {
        string displayedText = ""; // 실제로 화면에 보여줄 텍스트
        TextBalloonInit();

        foreach (char letter in fullText)
        {
            SetBalloonSize();
            string tempText;

            tempText = displayedText + letter;
            uiText.text = tempText;

            await UniTask.Delay(System.TimeSpan.FromSeconds(typingSpeed));

            displayedText += letter;
        }
    }

    // UniTask로 변환된 한글 타이핑 함수
    async UniTaskVoid ExecuteTyping(string fullText)
    {
        string displayedText = ""; // 실제로 화면에 보여줄 텍스트

        TextBalloonInit();
        // 문자열의 각 글자를 하나씩 처리
        foreach (char letter in fullText)
        {
            SetBalloonSize();

            string decompose = HangulTypingEffect.SplitHangul(letter); // 자모 분리

            string tempText;
            char curChar;

            // 자모가 순차적으로 추가되도록 반복
            // decompose의 Length는 3을 초과할 수 없다.(한글을 초성,중성,종성 으로 나누기 때문에 최대 3)
            for (int i = 0; i < decompose.Length; i++)
            {
                curChar = i switch
                {
                    0 => decompose[i],
                    1 => HangulTypingEffect.CombineHangul(decompose[i - 1], decompose[i]),
                    _ => HangulTypingEffect.CombineHangul(decompose[i - 2], decompose[i - 1], decompose[i])
                };

                tempText = displayedText + curChar;

                uiText.text = tempText; // TextMeshPro에 임시 텍스트 표시

                await UniTask.Delay(System.TimeSpan.FromSeconds(typingSpeed));
            }


            // 한 글자가 완성되었으니 displayedText에 추가
            displayedText += letter;
        }
    }
}
