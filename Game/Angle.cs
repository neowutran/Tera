using System;

namespace Tera.Game
{
    public struct Angle
    {
        private readonly short _raw;

        public Angle(short raw)
            : this()
        {
            _raw = raw;
        }

        public double Radians => _raw*(2*Math.PI/0x10000);
        public int Gradus => _raw*360/0x10000;

        public override string ToString()
        {
            return $"{Gradus}";
        }

        public HitDirection HitDirection(Angle target)
        {
            var diff = Math.Abs(target.Gradus - Gradus)%360;
            if (diff > 180) diff = 360 - diff;
            if (diff <= 45) return Game.HitDirection.Back;
            if (diff >= 135) return Game.HitDirection.Front;
            return Game.HitDirection.Side;
        }
    }

    public enum HitDirection
    {
        Back,
        Side,
        Front,
        Dot,
        Pet
    }
}