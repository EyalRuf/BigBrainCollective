using UnityEngine;
using System.Collections;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private TextMeshProUGUI notificationText = null;

    private const string ANIMATOR_SHOW_NOTIFICATION_PROP_NAME = "ShowNotification";
    private const int NOTIFICATION_DURATION = 3;
    public const string NOTIFICATION_TEXT_POST_SUBMITTED = 
        "Post shared. A fellow player will try to help shortly";
    public const string NOTIFICATION_TEXT_POST_ADDED =
        "A fellow player submitted a new post, Try to help them";
    public const string NOTIFICATION_TEXT_COMMENT_SUBMITTED = 
        "Comment sent. Thank you for trying to help";
    public const string NOTIFICATION_TEXT_COMMENT_APPROVED = 
        "One of your comments has been approved. Thank you for your help";
    public const string NOTIFICATION_TEXT_FINISHED_APPROVALS =
        "You've helped so many people today, Well done! Have a colorful day!";

    private IEnumerator Notify (string text)
    {
        this.notificationText.text = text;

        this.animator.SetBool(ANIMATOR_SHOW_NOTIFICATION_PROP_NAME, true);

        yield return new WaitForSeconds(NOTIFICATION_DURATION);

        this.animator.SetBool(ANIMATOR_SHOW_NOTIFICATION_PROP_NAME, false);
    }

    public void NotifyPostSubmitted()
    {
        this.StartCoroutine(this.Notify(NOTIFICATION_TEXT_POST_SUBMITTED));
    }

    public void NotifyPostAdded()
    {
        this.StartCoroutine(this.Notify(NOTIFICATION_TEXT_POST_ADDED));
    }

    public void NotifyCommentSubmitted()
    {
        this.StartCoroutine(this.Notify(NOTIFICATION_TEXT_COMMENT_SUBMITTED));
    }

    public void NotifyCommetOnPost(string postTitle)
    {
        if (postTitle.Length > 10)
        {
            postTitle = postTitle.Substring(0, 7) + "...";
        }
        this.StartCoroutine(this.Notify("A fellow player commented on your post: '" + postTitle + "', open 'My Posts' to check it out"));
    }

    public void NotifyApprovedComment()
    {
        this.StartCoroutine(this.Notify(NOTIFICATION_TEXT_COMMENT_APPROVED));
    }

    public void NotifyFinishedApprovals()
    {
        this.StartCoroutine(this.Notify(NOTIFICATION_TEXT_FINISHED_APPROVALS));
    }
}
