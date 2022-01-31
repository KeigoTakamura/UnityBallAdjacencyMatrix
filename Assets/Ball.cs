using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボール本体にアタッチされるクラス
/// </summary>
public class Ball : MonoBehaviour
{
    public int BallNumber = -1;

    //ボールの当たり判定が発火した時に呼ばれるEvent型を定義
    [System.Serializable]
    public class BallColliderEventType : UnityEvent<int, int>
    { }

    // ボールがClickされたときに発火するEvent型を定義
    [System.Serializable]
    public class BallClickEvent : UnityEvent<int> { }

    public BallColliderEventType ColliderEnter = new BallColliderEventType();
    public BallColliderEventType ColliderExit = new BallColliderEventType();
    public BallClickEvent OnClick = new BallClickEvent();

    /// <summary>
    /// 当たったら隣接行列の要素を1(bool型なのでtrue)
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))//壁とか関係ないObjectに接触した時の為にtrygetComponentで判定する
        {
            ColliderEnter.Invoke(BallNumber, ball.BallNumber);
        }
    }

    /// <summary>
    /// 接触状態が外れたら隣接行列の要素0(boolガタなのでfalse)
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
        {
            ColliderExit.Invoke(BallNumber, ball.BallNumber);
        }
    }

    /// <summary>
    /// Clickされたときに呼ばれるEvent(serializefieldでアタッチ)
    /// </summary>
    public void ClickEvent()
    {
        OnClick.Invoke(BallNumber);
        //Debug.Log(BallNumber);
    }
}
