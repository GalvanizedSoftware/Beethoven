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
If the signature matches, methods and properties are mapped automatically.
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

## Duck typing
One of the important missing features in C#.

My solution is to wrap the object if possible
(the code will run if it's not possible, but will give a runtime exception)
```C#
  public interface IDisplayName
  {
    string ShortName { get; }
    string LongName { get; }
  }
```
Two classes **not** implementing the interface and with no relations:
```C#
  internal class Person
  {
    public Person(string firstName, string lastName)
    {
      FirstName = firstName;
      LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }

    public string ShortName => $"{FirstName.FirstOrDefault()}. {LastName.FirstOrDefault()}.";
    public string LongName => FirstName + " " + LastName;
  }

  internal class Company
  {
    public Company(string name, string abreviation)
    {
      Name = name;
      Abreviation = abreviation;
    }

    public string Name { get; }
    public string Abreviation { get; }

    public string ShortName => Abreviation;
    public string LongName => Name;
  }
```
And finally the code needed to do duck-typing by wrapping (duck wrapping?):
```C#
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IDisplayName CreateDisplayName(object instance) =>
      beethovenFactory.Generate<IDisplayName>(instance);
  }
```
Will work for both classes.

## Interface convertion
With the automapping functionality, it is easy to change an interface,
but only implementing the changes needed to convert from one version to another.

Old style uses a string for birthdate:
```C#
  public interface IPerson
  { 
    string FirstName { get; set; }
    string LastName { get; set; }
    string BirthDate { get;set;}
  }
```
New is more robust to localization differences using a ```DateTime```:
```C#
  public interface IPerson
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime BirthDate{ get;set; }
  }
```
So the chalange is, if the server part has new new version,
but some clients are using the new version, some the old,
how can we convert from new version to old version.

First of all a helper class to do the work:
```C#
  internal class PersonV2ToV1Converter
  {
    private readonly IPerson personV2;
    private readonly CultureInfo cultureInfo;

    public PersonV2ToV1Converter(IPerson personV2, CultureInfo cultureInfo)
    {
      this.personV2 = personV2;
      this.cultureInfo = cultureInfo;
    }

    internal string GetBirthDateString() => 
      personV2.BirthDate.ToString("d", cultureInfo);

    internal void SetBirthDateDateTime(string dateTimeString) => 
      personV2.BirthDate = DateTime.Parse(dateTimeString, cultureInfo);
  }
```
Wrapping it is relatively simple:
```C#
        PersonV2ToV1Converter converter = new PersonV2ToV1Converter(person, cultureInfo);
        return factory.Generate<T>(
          personV2,
          new Property<string>("BirthDate")
            .DelegatedGetter(converter.GetBirthDateString)
            .DelegatedSetter(converter.SetBirthDateDateTime)
        );
```
```T``` is IPerson, v1.
Parsing is it in like this makes it possible to refer to the class in an 
external assembly with a different version.\
The interface might have the same name and namespace, 
which makes it difficult to refer to both.
