using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcHacker.FakeData
{
	class Processor
	{
		public string Name { get; private set; }
		public Processor(string _name) => Name = _name;
		public static implicit operator Processor(string _name) => new Processor(_name);
		public override bool Equals(Object processor)
		{
			if(!(processor is Processor))
				return false;
			return ((Processor)processor).Name == Name;
		}
	}

}
