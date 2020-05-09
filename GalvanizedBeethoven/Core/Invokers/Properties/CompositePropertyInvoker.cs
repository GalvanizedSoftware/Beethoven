using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  internal class CompositePropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public CompositePropertyInvoker(IPropertyDefinition<T>[] definitions)
    {
      this.definitions = definitions;
    }

    public T InvokeGet(object master)
    {
      T returnValue = default;
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeGetter(master, ref returnValue))
          break;
      return returnValue;
    }

    public void InvokeSet(object master, T newValue)
    {
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeSetter(master, newValue))
          return;
    }
  }
}
