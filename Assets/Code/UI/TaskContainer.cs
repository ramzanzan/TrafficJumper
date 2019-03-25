using UnityEngine;


public class TaskContainer : MonoBehaviour
{
    public bool ShowCompleted;
    public GameObject PrefabTaskPanel;
    
    public void Refresh(Level lvl)
    {
        var pd = ProgressData.GetInstance();
        var ca = GetComponent<ContentArranger>();
        ca.Clear();
        foreach (var t in lvl.Tasks)
        {
            var completed = pd[ProgressData.DataType.Task, t.Name];
            if (!completed || ShowCompleted)
            {
                var tp = Instantiate(PrefabTaskPanel);
                tp.SetActive(true);
                var tpc = tp.GetComponent<TaskPanelCtrl>();
                tpc.SetTask(t,completed);
                ca.Add((RectTransform)tp.transform);   
            }
            
        }
    }
}
