// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2007 Intelligencia
// Copyright 2007 Seth Yates
// 

using System;
using System.Xml;
using DY.UrlRewriter.Conditions;
using DY.UrlRewriter.Utilities;

namespace DY.UrlRewriter.Parsers
{
	/// <summary>
	/// Parser for url match conditions.
	/// </summary>
	public sealed class UrlMatchConditionParser : IRewriteConditionParser
	{
		/// <summary>
		/// Parses the condition.
		/// </summary>
		/// <param name="node">The node to parse.</param>
		/// <returns>The condition parsed, or null if nothing parsed.</returns>
		public IRewriteCondition Parse(XmlNode node)
		{
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            
            XmlNode matchAttr = node.Attributes.GetNamedItem(Constants.AttrUrl);
			if (matchAttr != null)
			{
				return new UrlMatchCondition(matchAttr.Value);
			}

			return null;
		}
	}
}
