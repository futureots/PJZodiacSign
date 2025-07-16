using System;
using UnityEngine;


public abstract class Agent : MonoBehaviour 
{
    public bool isInputStop;
    public Team team;
    public abstract void SetMode(Mode mode, Action call = null);
    /// <summary>
    /// 현재 controller가 보유중인 커맨드 반환 및 초기화
    /// </summary>
    /// <returns>입력한 커맨드</returns>
    public abstract Command GetCommand();
}
public enum Mode
{
    Repair,//정비 입력(상점창 오픈, 정비용 카메라 무브, 보유 기물 인스턴트 필드)
    Move,//이동 입력(드래그&드롭)
    Active,//스킬 입력(클릭)
    None // 입력 X, 정보만 표시
}
