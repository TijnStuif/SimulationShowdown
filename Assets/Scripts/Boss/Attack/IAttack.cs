namespace Boss.Attack
{
    public enum Type
    {
        Environment,
        Direct,
    }

    public interface IAttack
    {
        Type Type { get; }

        // the Execute method is called when the attack is executed
        void Execute();
    }
}