// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2007 Intelligencia
// Copyright 2007 Seth Yates
// 

using System;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;
using DY.UrlRewriter.Actions;
using DY.UrlRewriter.Conditions;
using DY.UrlRewriter.Utilities;
using DY.UrlRewriter.Configuration;

namespace DY.UrlRewriter.Parsers
{
    /// <summary>
    /// Parses the IFNOT node.
    /// </summary>
    public class UnlessConditionActionParser : IfConditionActionParser
    {
        /// <summary>
        /// The name of the action.
        /// </summary>
        public override string Name
        {
            get
            {
                return Constants.ElementUnless;
            }
        }
    }
}
