namespace Common.SkillType
{
    public abstract class CharacterSkillType
    {
        public enum EPunchType
        {
            None = 0,
            Left1,
            Left2,
            Left3,
            Right1,
            Right2,
            Right3,
        }

        public enum ESwordAttackType
        {
            None = 0,
            Left1,
            Left2,
            Left3,
            Right1,
            Right2,
            Right3,
            Other1,
            Other2,
            Other3,
            Other4,
            Other5,
        }
    
        public static readonly EPunchType[] LeftPunches =
            { EPunchType.Left1, EPunchType.Left2, EPunchType.Left3 };    
        
        public static readonly EPunchType[] RightPunches =
            { EPunchType.Right1, EPunchType.Right2, EPunchType.Right3 };
    }
}