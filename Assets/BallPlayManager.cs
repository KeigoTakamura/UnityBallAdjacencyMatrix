using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボールを生成したり管理したりするクラス
/// </summary>
public class BallPlayManager : MonoBehaviour
{
    [SerializeField]
    private int _maxBall = 900;

    [SerializeField]
    private Transform _ballGenratePos; //ballの生成位置をtransformで定義
    private bool[,] _adjacencyMatrix; //隣接行列でBallの接続関係を表現する無向グラフを表現する
    private List<Ball> _balls;//各ballの格納するリスト
    private string _ballPrefabPath = "TestBall";

    private void Start()
    {
        _balls = new List<Ball>(_maxBall);
        _adjacencyMatrix = new bool[_maxBall, _maxBall];//0クリアされたInt配列を生成
        BallGenerate();
    }

    /// <summary>
    /// ボール生成クラス、Eventの追加
    /// </summary>
    private void BallGenerate()
    {
        GameObject obj = (GameObject)Resources.Load(_ballPrefabPath);

        for (int i = 0; i < _maxBall; i++)
        {
            _balls.Add(Instantiate(obj, _ballGenratePos).GetComponent<Ball>());
            _balls[i].BallNumber = i;//ballの要素番号をballのIDとして使うよ
            _balls[i].ColliderEnter.AddListener(AdjacencyMatrixEnterUpdateEvent);
            _balls[i].ColliderExit.AddListener(AdjacencyMatrixExitUpdateEvent);
            _balls[i].OnClick.AddListener(BallClickd);
        }
    }

    /// <summary>
    /// 接触状態になったときに反映されるメソッド
    /// </summary>
    /// <param name="mynumber"></param>
    /// <param name="ballnumber"></param>
    private void AdjacencyMatrixEnterUpdateEvent(int mynumber, int ballnumber)
    {
        _adjacencyMatrix[mynumber, ballnumber] = true;
    }

    /// <summary>
    /// 接触状態が解除になったときに反映されるメソッド
    /// </summary>
    /// <param name="mynumber"></param>
    /// <param name="ballnumber"></param>
    private void AdjacencyMatrixExitUpdateEvent(int mynumber, int ballnumber)
    {
        _adjacencyMatrix[mynumber, ballnumber] = false;
    }

    /// <summary>
    /// 生成したballがClickされときに呼ばれるメソッド
    /// </summary>
    private void BallClickd(int ballnumber)
    {
        for (int i = 0; i < _maxBall; i++)
        {
            //Debug.Log(_adjacencyMatrix[ballnumber, i]);
            if (_adjacencyMatrix[ballnumber, i] && i != ballnumber)
            {
                Destroy(_balls[i].gameObject); //隣接行列で接触状態にあるボールを破壊
            }
        }
        Destroy(_balls[ballnumber].gameObject); //最後にClickされたボールを破壊
    }

}
