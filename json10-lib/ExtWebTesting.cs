////////////////////////////////////////////////////////////////////////////
// Porgram: ExtWebTesting.cs
// Purpose: Custom ExtractionRule Demo
// Authors: Antonio Sun (c) 2017, All rights reserved
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using System.ComponentModel; 

using Newtonsoft.Json;

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

using Microsoft.VisualStudio.TestTools.WebTesting;

// The namespace should be Ext.<Module>.<Project>
namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// GetGhRet: Github Extract Demo
    // This class creates a custom ExtractionRule named "Github Extract"
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-Extract Github")]
    /// Specify a description for use in the user interface.
    [Description("ExtractionRule for Github")]
    public class GetGhRet : ExtractionRule
    {

        [DisplayName("Comment")]
        [Description("What the plugin is for")]
        public string theComment { get; set; }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            var bodyString = e.Response.BodyString;
            var retStr = GetRet(bodyString);

            if (retStr == null)
            {
                e.Message = "Extraction failed for: " + Environment.NewLine +
                            e.Response.BodyString;
                e.Success = false;
                return;
            }

            e.Success = true;
            e.WebTest.Context.Add(ContextParameterName, retStr);
        }


        private static string GetRet(string bodyString)
        {
            var repos = JsonConvert.DeserializeObject<Repos>(bodyString);

            if (repos == null) { return null;  }

            return JsonConvert.SerializeObject(repos);
        }
    }
}

namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// GetGhRet: Github Extract Demo
    // This class creates a custom ExtractionRule named "Github Extract"
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-RequestPlugin Public")]
    /// Specify a description for use in the user interface.
    [Description("RequestPlugin for public services")]
    public class GhReq : WebTestRequestPlugin 
    {
        [DisplayName("Comment")]
        [Description("What the plugin is for")]
        public string theComment { get; set; }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            // TODO: Add code to execute  
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            // TODO: Add code to execute  
        }
    }
}
