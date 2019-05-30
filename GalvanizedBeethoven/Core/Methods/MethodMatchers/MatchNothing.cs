﻿using System;

namespace GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers
{
  public class MatchNothing : IMethodMatcher
  {
    public bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType) =>
      false;
  }
}