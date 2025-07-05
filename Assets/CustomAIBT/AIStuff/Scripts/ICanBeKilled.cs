public interface ICanBeKilled
{
    /// <summary>
    /// Returns true if this unit can currently be killed.
    /// </summary>
    bool CanBeKilled();

    /// <summary>
    /// Optional: Called when the unit is marked as killed.
    /// </summary>
    void OnKilled();
}