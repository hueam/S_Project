using UnityEngine;

[RequireComponent(typeof(FeedbackPlayer))]
public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedback(); //�ǵ�� ����
    public abstract void FinishFeedback();  //�ǵ�� ����
    
    protected virtual void OnDestroy()
    {
        FinishFeedback();
    }

    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
