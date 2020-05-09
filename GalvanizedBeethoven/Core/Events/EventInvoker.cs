namespace GalvanizedSoftware.Beethoven.Core.Events
{
  internal class EventInvoker
  {
    private readonly string name;

    public EventInvoker(string name)
    {
      this.name = name;
    }

    public object Invoke(object master, params object[] arguments) =>
      (master as IGeneratedClass)?.NotifyEvent(name, arguments);
  }
}