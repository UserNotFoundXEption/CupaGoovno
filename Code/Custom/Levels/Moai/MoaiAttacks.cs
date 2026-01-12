namespace CupaGoovno;

public class MoaiAttacks
{
    public Main main;
    public Support support;

    public MoaiAttacks(Main main, Support support)
    {
        this.main = main;
        this.support = support;
    }

    public enum Main
    {
        Laser,
        Shitlings,
        GiantStone,
        Rockets,
        Pusher
    }

    public enum Support
    {
        Pollen,
        Spikes,
        Crackhead,
        Bouncers,
        Baseball
    }
}
