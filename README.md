# Beethoven
Composing classes in fluet-style programming.
Frequently used implementations can be reused without inheritance.
Not using inheritance enables inclusion of the code actually needed.
Inheritance can lead to a vast class inheritance strucure.

The starting point of this project was the fluent programming style to import code.
But it turned out there were amny more possibilities ...

## Fluent programming style
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

## The perfect solution
In a perfect world, I'd prefer to implement this in C# directly:
```
  public class Person : IPerson
  {
    public string FirstName
    {
      get; 
      set
        .SkipIfEqual()
        .SetterGetter()
        .NotifyChanged();      
    }

    public string LastName
    {
      get; 
      set
        .SkipIfEqual()
        .SetterGetter()
        .NotifyChanged();      
    }
  }
```

... I can always dream.

## Default implementation
The example in fluet programming had some _almost_ duplicated code,
the only difference is the name of the property, and that can be resolved automatically.
This example does the same thing, but resolves all property names automatically:
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

## Modifying existing implementation

This package can change parts of a implementation without changing the existing classes,
or doing a lot of passthrough coding.
```C#
  public interface IOrderedItem
  {
    string Name { get; }
    double Price { get; }
    double Weight { get; }
  }

  internal class GiftWrapping
  {
    public GiftWrapping(IOrderedItem mainItem)
    {
      Price = mainItem.Price + (mainItem.Price > 1000 ? 0 : 10);
      Weight = mainItem.Weight + 0.1;
    }
    public double Price { get; }
    public double Weight { get; }
  }

  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    public IOrderedItem AddGiftWrapping(IOrderedItem mainItem) =>
      beethovenFactory.Generate<IOrderedItem>(
        new LinkedObjects(
          mainItem,
          new GiftWrapping(mainItem)));
  }
``` 
Note that ```GiftWrapping``` has no need to implement the Name property,
it's only implementing the properties that are changed. 
