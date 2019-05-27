using Microsoft.Xna.Framework;

namespace Strategy
{
    public interface Aggressor
    {
        void LookForTarget(Computer p2);
        void LookForTarget(Player player);
    }
}