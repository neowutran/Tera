namespace Tera.Game.Messages
{
    public class S_ACTION_END : ParsedMessage
    {
        internal S_ACTION_END(TeraMessageReader reader) : base(reader)
        {
            
            Entity = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            Model = reader.ReadUInt32();
            SkillId = new SkillId(reader).Id;
            EndType = reader.ReadInt32();
            Id = reader.ReadUInt32();
//            Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Start} {Heading} -> {Finish}, S:{Speed} ,{Ltype} {unk1} {unk2}" );
        }

        public uint Id { get; set; }
        public int EndType { get; set; }
                //# 0 = Finished
                //# 1 = Cancel (lockon)
                //# 2 = Cancel (movement/etc.)
                //# 3 = Special Interrupt (ex. Lancer: Shield Counter)
                //# 4 = Chain
                //# 5 = Retaliate
                //# 6 = Interrupt
                //# 10 = Button Release
                //# 11 = Button Release + Chain (ex. Mystic: Corruption Ring)
                //# 13 = Out of Stamina
                //# 19 = Invalid Target
                //# 25 = Unknown (ex. Command: Recall)
                //# 29 = Interrupted by Terrain (ex. entering water)
                //# 36 = Lockon Cast
                //# 37 = Interrupted by Loading
                //# 39 = Dash Finished
                //# 43 = Interrupted by Cutscene
                //# 49 = Unknown (HB uses this for Recall)
                //# 51 = Finished + Button Release (ex. Brawler: Counter)
        public int SkillId { get; set; }
        public uint Model { get; set; }
        public EntityId Entity { get; }
        public Vector3f Position { get; private set; }
        public Angle Heading { get; private set; }
    }
}