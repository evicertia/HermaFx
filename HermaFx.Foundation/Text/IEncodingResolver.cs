using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.Text
{
	public interface IEncodingResolver
	{
		Encoding GetEncoding(string name);
	}
}
