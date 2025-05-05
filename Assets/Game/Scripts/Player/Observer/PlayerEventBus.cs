using System;

public interface IPlayerEventNotifier
{
    event Action OnAttack;
    event Action OnInteract;
    event Action OnJump;
    event Action OnLand;
    event Action OnDash;
}

public class PlayerEventBus : IPlayerEventNotifier
{
    public event Action OnAttack;
    public event Action OnInteract;
    public event Action OnJump;
    public event Action OnLand;
    public event Action OnDash;

    public void RaiseAttack() => OnAttack?.Invoke();
    public void RaiseInteract() => OnInteract?.Invoke();
    public void RaiseJump() => OnJump?.Invoke();
    public void RaiseLand() => OnLand?.Invoke();
    public void RaiseDash() => OnDash?.Invoke();
}
