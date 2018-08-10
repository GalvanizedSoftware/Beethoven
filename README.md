# Beethoven
Composing classes in fluet-style programming.
Frequently used implementations can be reused without inheritance.
Not using inheritance enables inclusion of the code actually needed, not a vast base-class, or a library of classes.

Fluent programming har so far been limited to methods on classes. This projects motivation is to extent the fluent-style to class code.

**For example:**
```C#
public interface IPerson : INotifyPropertyChanged
{
  string FirstName { get; set; }
  string LastName { get; set; }
}

class Factory
{
  private readonly BeethovenFactory factory = new BeethovenFactory();

  public IPerson CreatePerson()
  {
    return factory.Generate<IPerson>(
      new Property<string>("FirstName")
        .SkipIfEqual()
        .SetterGetter()
        .NotifyChanged(),
      new Property<string>("LastName")
        .SkipIfEqual()
        .SetterGetter()
        .NotifyChanged()
    );
  }
}
```

This is aqually enough to implement the class. There is also a default scheme, where the property names are read with reflection:
```C#
public IPerson CreatePerson()
{
  return factory.Generate<IPerson>(
  new DefaultProperty
    .SkipIfEqual()
    .SetterGetter()
    .NotifyChanged()
  );
}
```
