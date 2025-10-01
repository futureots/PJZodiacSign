using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] Field field;

    public ITurn curState;
    public List<Command> commandList;

    public void Initialize(Field field)
    {
        this.field = field;
    }

    public void SetState(ITurn state)
    {

        curState = state;
    }

    public void SendCommands()
    {
        //field에 커맨드 전송 및 콜백 함수 설정
    }
    void OnCommandExecuted()
    {

    }
}

/*public interface ITurn
{
    public enum State
    {
        Attack,
        Action,
        Repair
    }
    /// <summary>
    /// 턴 상태
    /// </summary>
    public State curState { get; }
    /// <summary>
    /// 턴의 주인(없을 경우 NULL)
    /// </summary>
    public Agent owner {  get; }
    /// <summary>
    /// 몇 번째 턴인지 표시하는 변수
    /// </summary>
    public int Count { get; }
}*/