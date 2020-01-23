using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.Site
{
    public abstract class Expressabstract
    {
        public Expressabstract()
		{
			
		}
        public abstract string CreatePostHttpResponse(IDictionary<string, string> parameters);
    }
}
