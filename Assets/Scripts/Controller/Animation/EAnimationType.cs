namespace Controller.Animation
{
    /// <summary>
    /// 애니메이션에서 사용하는 TriggerNumber 값을 기준으로 enum 값 설정
    /// </summary>
    public enum EAnimationType
    {
        None,
        Move,
        Attack = 4,
        TakeHit = 12,
    }
}