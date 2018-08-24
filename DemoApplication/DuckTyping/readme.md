# Duck typing
One of the things I personally miss in C#.

The example is very simple, and inheriting from ```IDisplayName``` would be an obveous solution to the concrete problem.\
However, in cases where:
* Not all object can/should be changed
* The class structure is huge


...duck typing is a good thing.

This example really needs a fallback feature. If the property ```ShortName``` is not supported, if should be possible to insert a default behaviour.\
Also an ```Implements<TInterface, TClass>()``` would be nice.