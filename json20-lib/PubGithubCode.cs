////////////////////////////////////////////////////////////////////////////
// Porgram: PubGithubCode.cs
// Purpose: Custom ExtractionRule & Plugin Demo
// Authors: Antonio Sun (c) 2017, All rights reserved
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;
using System.ComponentModel; 

using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // for JObject

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

using Microsoft.VisualStudio.TestTools.WebTesting;

// !!Rule!!
// The namespace should be Ext.<Module>.<Project>
namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    // The extension functions are define here
    // Usually in another spearated file

    /// <summary>
    /// The template functions available through the object 'ghext' in scriban.
    /// </summary>
    public static class GhExt
    {
        public static string UpCase(string text)
        {
            return text.ToUpperInvariant();
        }

        public static string DownCase(string text)
        {
            return text.ToLowerInvariant();
        }


        /// ////////////////////////////////////////////////////////////////////////////
        public static void Register(ScriptObject scriptObject)
        {
            // Import the following (single) delegate (would be accessible as a global function)
            // myfunction. MB, register as "ext_gh.myfunction" will not work!
            scriptObject.Import("myfunction", new Func<string>(() => "\"MyFunction\": \"Hello Func\""));

            // Register (a group of) functions available through the object 'ghext' in scriban
            var libObject = ScriptObject.From(typeof(GhExt));
            scriptObject.SetValue("pub_gh", libObject, true);
        }
    }
}


// !!Rule!!
// The namespace should be Ext.<Module>.<Project>
namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    /// !!Rule!!
    /// Note: To make this code work, the corresponding data structures definition
    /// should be available, usually in spearated .cs files.

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


        public static string GetRet(string bodyString)
        {
            var repos = JsonConvert.DeserializeObject<Repos>(bodyString);

            if (repos == null) { return null;  }

            return JsonConvert.SerializeObject(repos);
        }
    }
}

// !!Rule!!
// The namespace should be Ext.<Module>.<Project>
namespace Ext.Pub.Github
{
    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// GhReq: Github PreRequest Demo
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PreRequest Public Github")]
    /// Specify a description for use in the user interface.
    [Description("PreRequest Plugin for Github public services")]
    public class GhReq : Ext.Generic.Templating.PreRequestPluginBasic
    {
        public override void Register(ScriptObject scriptObject)
        {
            base.Register(scriptObject);
            Ext.Pub.Github.GhExt.Register(scriptObject);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// GhResp: Github Extract Demo
    // This class creates a custom PostRequest Plugin as ExtractionRule
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PostRequest Github Extraction")]
    /// Specify a description for use in the user interface.
    [Description("PostRequest Plugin for Github ExtractionRule")]
    public class GhResp : Ext.Generic.Templating.PostRequestPluginBasic
    {
        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            string resp = e.Response.BodyString.ToString();
            string retStr = GetGhRet.GetRet(resp);
            e.WebTest.Context.Add(theTarget, retStr);
        }
    }

}
