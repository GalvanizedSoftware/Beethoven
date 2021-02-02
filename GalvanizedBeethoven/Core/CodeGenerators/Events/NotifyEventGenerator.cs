using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
	internal class NotifyEventGenerator : ICodeGenerator
	{
		private readonly EventInfo[] eventInfos;

		public NotifyEventGenerator(EventInfo[] eventInfos)
		{
			this.eventInfos = eventInfos;
		}

		public IEnumerable<(CodeType, string)?> Generate() =>
			EventsCode.EnumerateCode
			(
				"",
				$"object {ClassGenerator.GeneratedClassName}.NotifyEvent(string eventName, object[] values)",
				"{",
				"	switch (eventName)",
				"	{",
				GenerateNotifyCode(),
				"		default: return null;",
				"	}",
				"}"
			);

		private IEnumerable<string> GenerateNotifyCode() =>
			eventInfos
				.Select(eventInfo => eventInfo.Name)
				.Select(name => @$"case ""{name}"": return {name}?.DynamicInvoke(values);")
				.Select(line => line.Format(2));
	}
}

