**Decorator Pattern**

This example is inspired be the *Gang of Four* decorator pattern.

It implements the pattern without using inheritance, therefore it looks a bit different from the original.

I've tried showing that the decorator pattern can be implemented without inheriyance or dummy-implementations, for example of the property ```Name```.

It concerns be that (*at the moment*) the ```Name``` property if reffered explicitly in the method ```Factory.AddGiftWrapping```.
I'm sure I'll find a way to solve that eventually,

Here's where the real magic is:\
Imagine you wanted to add a serial number property for the items. In a traditional implementation you would have to change:
* If there is an abstract base class (```OrderedItemBase```) that will have to change with a default implementation.
* If an interface is used instead of an abstract base class, the decorator would have to forward the new property.
