using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TutorialProgressController : MonoBehaviour
{
    [Header("Content")] 
    [SerializeField] private Image firstPage;
    [SerializeField] private Image lastPage;
    [SerializeField] [Tooltip("Additional tutorial pages beyond the first one")] private Sprite[] additionalPages;
    private int currentPage;

    [UsedImplicitly]
    public void OnNextPage()
    {
        if (++currentPage == additionalPages.Length)
        {
            gameObject.SetActive(false);
            lastPage.gameObject.SetActive(true);
        }
        else
        {
            firstPage.sprite = additionalPages[currentPage];
        }
    }
}
