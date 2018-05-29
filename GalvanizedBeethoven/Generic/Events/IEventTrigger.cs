namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  public interface IEventTrigger
  {
    object Notify(params object[] args);
  }
}
