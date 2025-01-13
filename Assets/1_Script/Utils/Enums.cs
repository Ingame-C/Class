
namespace Class {

    public enum PropTypes
    {
        None = 0,
        Chair,
        Door,
        LockerDoor,
        Desk,
        Chalk,
        Pencil = 100,
        Crayons,
        Pallet,
        ColoredPencil,
        even_effects,
        Replica,
    }

    public enum EffectTypes
    {
        None = 0,
        DeskAndChairEffect,
        ApproachingWall,
        ArtToolReplicator,
        MirrorBreak,
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
        Add = 100,
    }

    public enum  SceneEnums
    {
        Game,
        Test
    }

}
