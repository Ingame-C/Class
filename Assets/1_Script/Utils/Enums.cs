
namespace Class {

    public enum PropTypes
    {
        None = 0,
        Chair,
        Door,
        LockerDoor,
        Desk,
        Lectern,
        Chalk,
        Pencil = 100,
        Crayons,
        Pallet,
        ColoredPencil,
        even_effects,
        Replica,
        OMRCard,
        TestPaper
    }

    public enum EffectTypes
    {
        None = 0,
        DeskAndChairEffect,
        MirrorBreak,
        LightBlink,
        Laugh,
        BloodyRain,
        CommonEnd,              // 공용 효과의 끝을 나타냅니다.
        ApproachingWall = 100,      
        ArtToolReplicator,
    }


    public enum SfxClipTypes { 
        Sweep = 0,
        Door_open = 1,
        TV_Noise = 2,
        Locker_open = 3,
        Locker_close = 4,
        Wall_move = 5,
        Change_chair_size = 6,
        Replica = 7,
        Blink = 8,
        Thisman_Laugh = 9,
        Thisman_Walk = 10,
        Hint = 11,
        Firework = 12,
        Player_Walk_1 = 13,
        Player_Walk_2 = 14,
        Player_Walk_3 = 15,
        Release_Object = 16,
        Grab_Object = 17,
        Mirror_Break = 18,
        Player_Rain_Walk = 19,
        Add = 100,
    }

    public enum  SceneEnums
    {
        Game,
        Test
    }

    public enum ThisManEnums
    {
        // 경비원 디스맨이 0번 입니다.
        bouncer = 0,
        
    }

}
