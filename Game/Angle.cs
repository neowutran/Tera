using System;

namespace Tera.Game
{
    public struct Angle
    {
        private readonly int _raw;

        public Angle(int raw)
            : this()
        {
            _raw = raw;
        }

        public double Radians => _raw*(2*Math.PI/0x10000);
        public int Gradus => _raw*360/0x10000;
        public int Raw => _raw;

        public static Angle operator + (Angle x, Angle y) { return new Angle(x.Raw + y.Raw); }
        public static Angle operator - (Angle x, Angle y) { return new Angle(x.Raw - y.Raw); }
        public static Angle operator - (Angle x) { return new Angle(-x.Raw ); }
        public static bool operator > (Angle x, Angle y) { return x.Raw > y.Raw; }
        public static bool operator < (Angle x, Angle y) { return x.Raw < y.Raw; }
        public static bool operator ==(Angle x, Angle y) { return x.Raw == y.Raw; }
        public override bool Equals(object obj) { return _raw == (obj as Angle?)?.Raw; }
        public override int GetHashCode() { return _raw; }
        public static bool operator !=(Angle x, Angle y) { return x.Raw != y.Raw; }
        public static bool operator >=(Angle x, Angle y) { return x.Raw >= y.Raw; }
        public static bool operator <=(Angle x, Angle y) { return x.Raw <= y.Raw; }
        public static Angle operator +(Angle x, int y) { return new Angle(x.Raw + y); }
        public static Angle operator -(Angle x, int y) { return new Angle(x.Raw - y); }

        public override string ToString()
        {
            return $"{Gradus}";
        }

        public static Angle Normalize(Angle angle)
        {
            return new Angle((angle.Raw + 0x8000) % 0x10000 - 0x8000);
        }

        public static bool CheckSide(Angle posAngle, Angle attAngle)
        {
            posAngle = Normalize(posAngle);
            attAngle = Normalize(attAngle);
            if (posAngle.Raw < 0) { posAngle = -posAngle; attAngle = -attAngle; }
            if (posAngle.Raw > 0x4000) return false;
            if ((-0x6000 >= attAngle.Raw) && (attAngle > posAngle - 0xA000)) return true;
            if (posAngle.Raw < 0x2000 && attAngle >= Normalize(posAngle - 0xA000)) return true;
            return false;
        }

        public static HitDirection HitDirection(Vector3f myPos, Angle myAngle, Vector3f bossPos, Angle bossAngle)
        {
            var posAngle = bossPos.GetHeading(myPos) - bossAngle;
            var attAngle = myAngle - bossAngle;
            if (CheckSide(posAngle - 0x8000, attAngle - 0x8000))  return Game.HitDirection.Back;
            if (CheckSide(posAngle - 0x4000, attAngle - 0x4000))  return Game.HitDirection.Right | Game.HitDirection.Side;
            if (CheckSide(posAngle + 0x4000, attAngle + 0x4000))  return Game.HitDirection.Left  | Game.HitDirection.Side;
            return Game.HitDirection.Front;
        }

        public HitDirection HitDirection(Angle target)
        {
            var diff = (target.Gradus - Gradus + 720)%360;
            var side = diff > 180 ? Game.HitDirection.Left : Game.HitDirection.Right;
            if (diff > 180) diff = 360 - diff;
            if (diff <= 55) return Game.HitDirection.Back;
            if (diff >= 125) return Game.HitDirection.Front;
            return Game.HitDirection.Side|side;
        }
    }

    /*
        *  The enum value NEED to be set manually
        *  without that, converting the enum to int will cause massive weird bug, like:
        *  https://github.com/neowutran/ShinraMeter/issues/184
        * */
    [Flags]
    public enum HitDirection
    {
        Back = 1,
        Left = 2,
        Right = 4,
        Side = 8,
        Front = 16,
        Dot = 32,
        Pet = 64
    }
}