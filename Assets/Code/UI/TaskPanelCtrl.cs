using Code.Gameplay.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TaskPanelCtrl : MonoBehaviour
{
    private ITask _task;
    
    private void Start()
    {
        transform.Find("Skip").GetComponent<Button>().onClick.AddListener(Buy);
    }

    private void Buy()
    {
        var b = LocalStore.Buy(ProgressData.DataType.Task, _task.Name, _task.Cost);
        if (b)
        {
            transform.Find("CompletedText").gameObject.SetActive(true);
            transform.Find("SkipText").gameObject.SetActive(false);
            transform.Find("Skip").gameObject.SetActive(false);
        } 
    }

    public void SetTask(ITask task, bool completed)
    {
        _task = task;
        transform.Find("TaskText").GetComponent<TextMeshProUGUI>().text = task.ToString();
        var btn = transform.Find("Skip").GetComponent<Button>();
        if (completed)
        {
            transform.Find("CompletedText").gameObject.SetActive(true);
            transform.Find("SkipText").gameObject.SetActive(false);
            btn.gameObject.SetActive(false);
        }
        else
        {
            btn.GetComponentInChildren<TextMeshProUGUI>().text = task.Cost.ToString();
        }
    }
}
