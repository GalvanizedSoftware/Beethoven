﻿using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven
{
  internal class BindingParents : IBindingParent
  {
    private readonly IBindingParent[] bindingParents;

    public BindingParents(IEnumerable<object> allPartDefinitions)
    {
      bindingParents = allPartDefinitions
        .OfType<IBindingParent>()
        .ToArray();
    }

    public void Bind(object target)
    {
      foreach (IBindingParent bindingParent in bindingParents)
        bindingParent.Bind(target);
    }
  }
}