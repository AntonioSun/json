////////////////////////////////////////////////////////////////////////////
// Porgram: PTExtGeneric.cs
// Purpose: The Base Generic Request Plugins that other Custom ones based on
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
namespace Ext.Generic.Templating
{
    public delegate void delegateRegister(ScriptObject scriptObject);

    public static class ExtTplUtil
    {

        ///////////////////////////////////////////////////////////////////////////////
        /// !! WARNING !! WARNING !! WARNING !!
        /// Functions here are absolutely generic!
        /// Do NOT put your custom function here!
        /// Put them in your own sub-class instead!
        /// If you want to move your own custom function from your own sub-class to here, 
        /// consult your team first!
        /// !! WARNING !! WARNING !! WARNING !!
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Generic Json Object-Select Helper, available as "select_obj" in template
        /// Same as the generic "select" but for easy of debugging. Selecting from Json Object.
        /// </summary>
        /// <param name="inObj">input object</param>
        /// <param name="theFilter">any valid JPATH query string</param>
        /// <returns>the filtered result string (in form of Json array)</returns>
        public static string SelectObj(string theFilter, object inObj)
        {
            string jsStr = JsonConvert.SerializeObject(inObj);
            JObject o = JObject.Parse(jsStr);
            //JObject o = JObject.Parse(inObj.ToString());
            IEnumerable<JToken> selEnum = o.SelectTokens(theFilter);
            string ret = JsonConvert.SerializeObject(selEnum);
            return ret;
        }

        /// <summary>
        /// Generic Json Array-Select Helper, available as "select_array" in template
        /// Same as the generic "SelectObj" but from Json array.
        /// </summary>
        /// <param name="inObj">input object (in form of Json array)</param>
        /// <param name="theFilter">any valid JPATH query string</param>
        /// <returns>the filtered result string (in form of Json array)</returns>
        public static string SelectArray(string theFilter, object inObj)
        {
            string jsStr = JsonConvert.SerializeObject(inObj);
            JArray o = JsonConvert.DeserializeObject<JArray>(jsStr);
            IEnumerable<JToken> selEnum = o.SelectTokens(theFilter);
            string ret = JsonConvert.SerializeObject(selEnum);
            return ret;
        }

    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Template & Req/Resp Util Helper
    /// </summary>
    //-------------------------------------------------------------------------

    public class Util
    {
        public static void DefaultRegister(ScriptObject scriptObject)
        {
            // Default functions

            // select, selects a JSON array
            scriptObject.Import("select", new Func<string, Object, Object>((f, o) => 
                JsonConvert.SerializeObject(JObject.Parse(JsonConvert.SerializeObject(o)).SelectTokens(f))));
            // serialize
            scriptObject.Import("serialize", new Func<Object, string>(x => JsonConvert.SerializeObject(x)));

            // Register functions available through the object 'ghext' in scriban
            scriptObject.Import(typeof(ExtTplUtil));
            //var arrayObject = ScriptObject.From(typeof(ExtTplUtil));
            //scriptObject.SetValue("extjs", arrayObject, true);
        }

        /// <summary>
        /// Generic Json Select Helper
        /// </summary>
        /// <param name="inStr">input Json string</param>
        /// <param name="theFilter">any valid JPATH query string</param>
        /// <returns>the filtered result string (in form of Json array)</returns>
        public static string JsonSelectObj(string inStr, string theFilter)
        {
            JObject o = JObject.Parse(inStr);
            IEnumerable<JToken> selEnum = o.SelectTokens(theFilter);
            return JsonConvert.SerializeObject(selEnum);

            //string retStr = "[";
            //foreach (JToken item in selEnum)
            //{
            //    retStr += item.ToString()+",";
            //}
            //retStr = retStr.TrimEnd(',');
            //retStr += "]";
        }

        /// <summary>
        /// Transform the given data model (specified as JSON array string) according to theTemplate,
        /// with the help of functions defined in the Register
        /// </summary>
        /// <param name="theModel">the given data model</param>
        /// <param name="theTemplate">the transformation template</param>
        /// <param name="Register">of type delegateRegister</param>
        /// <returns>the transformaed string</returns>
        public static string Transform(string theModel, string theTemplate, delegateRegister Register)
        {
            // Init vars
            var template = Template.Parse(theTemplate);
            var parsed = JsonConvert.DeserializeObject<JArray>(theModel); // JObject
            var model = new { d = parsed };

            // Prepare Template
            var scriptObject = new ScriptObject();
            scriptObject.Import(model);
            Register(scriptObject);

            // Render from Template
            var context = new TemplateContext();
            context.PushGlobal(scriptObject);
            template.Render(context);
            context.PopGlobal();
            string result = context.Output.ToString();

            return result;
        }

        /// <summary>
        /// PostRequestTransform - Post-Request Transform
        /// </summary>
        /// <param name="theModel">the given data model</param>
        /// <param name="theFilter">the filter string</param>
        /// <param name="theTemplate">the transformation template</param>
        /// <param name="Register">of type delegateRegister</param>
        /// <param name="e">PostRequestEventArgs</param>
        /// <returns>the transformaed string</returns>
        /// <returns></returns>
        public static string PostRequestTransform(string theModel, string theFilter, string theTemplate, delegateRegister Register, PostRequestEventArgs e)
        {
            string resp = e.Response.BodyString.ToString();
            string retStr = resp;
            if (!String.IsNullOrEmpty(theModel)) retStr = e.WebTest.Context[theModel].ToString();
            if (!String.IsNullOrEmpty(theFilter)) retStr = Util.JsonSelectObj(retStr, theFilter);
            // && !String.IsNullOrEmpty(e.WebTest.Context[theFilter])
            if (!String.IsNullOrEmpty(theTemplate))
            {
                retStr = Util.Transform(retStr, theTemplate, new delegateRegister(Register));
            }
            return retStr;
        }

        /// <summary>
        /// GetStringBody - Get StringBody for PreRequest
        /// </summary>
        /// <param name="e">PreRequestEventArgs</param>
        /// <returns>Request StringBody string</returns>
        public static string GetStringBody(PreRequestEventArgs e)
        {
            // Get String Body
            StringHttpBody body = e.Request.Body as StringHttpBody;
            return body.BodyString;
        }

        /// <summary>
        /// GetStringBody - Get StringBody for PostRequest
        /// </summary>
        /// <param name="e">PostRequestEventArgs</param>
        /// <returns>Request StringBody string</returns>
        public static string GetStringBody(PostRequestEventArgs e)
        {
            // Get String Body
            StringHttpBody body = e.Request.Body as StringHttpBody;
            return body.BodyString;
        }

        /// <summary>
        /// GetStringBodyTemplate - Get StringBody for PreRequest as Template, replacing mark Open & Close
        /// </summary>
        /// <param name="e">PreRequestEventArgs</param>
        /// <returns>Request StringBody string as Template</returns>
        public static string GetStringBodyTemplate(PreRequestEventArgs e, string markOpen, string markClose)
        {
            string bdStr = GetStringBody(e);
            bdStr = bdStr.Replace(markOpen, "{{");
            bdStr = bdStr.Replace(markClose, "}}");
            return bdStr;
        }

        /// <summary>
        /// GetStringBodyTemplate - Get StringBody for PostRequest as Template, replacing mark Open & Close
        /// </summary>
        /// <param name="e">PostRequestEventArgs</param>
        /// <returns>Request StringBody string as Template</returns>
        public static string GetStringBodyTemplate(PostRequestEventArgs e, string markOpen, string markClose)
        {
            string bdStr = GetStringBody(e);
            bdStr = bdStr.Replace(markOpen, "{{");
            bdStr = bdStr.Replace(markClose, "}}");
            return bdStr;
        }

        /// <summary>
        /// SetStringBody - sets the Request StringBody
        /// </summary>
        /// <param name="result">string to set StringBody with</param>
        /// <param name="e">PreRequestEventArgs</param>
        public static void SetStringBody(string result, PreRequestEventArgs e)
        {
            // Set Request String Body
            var stringBody = new StringHttpBody();
            stringBody.BodyString = result;
            stringBody.ContentType = "application/json";

            e.Request.Body = stringBody;
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Base Generic PreRequest Plugin that others based on
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PreRequest =Base=")]
    /// Specify a description for use in the user interface.
    [Description("The Base Generic PreRequest Plugin (Do NOT use this)")]
    public class PreRequestPluginBase : Microsoft.VisualStudio.TestTools.WebTesting.WebTestRequestPlugin
    {
        [DisplayName("0- Comment")]
        [Description("What the plugin is for")]
        public string theComment { get; set; }

        [DisplayName("b- Model Name")]
        [Description("The name of Context Parameter (holding JSON array string) used as data model to drive the template")]
        public string theModel { get; set; }

        public virtual void Register(ScriptObject scriptObject)
        {
            Util.DefaultRegister(scriptObject);
        }

    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Basic Generic PreRequest Plugin without any custom functions
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PreRequest Basic")]
    /// Specify a description for use in the user interface.
    [Description("The Basic Generic PreRequest Plugin to set StringBody without any custom functions")]
    public class PreRequestPluginBasic : PreRequestPluginBase 
    {
        [DisplayName("a- Template String")]
        [Description("The template used to populate request StringBody")]
        public string theTemplate { get; set; }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            Util.SetStringBody(Util.Transform(e.WebTest.Context[theModel].ToString(), 
                theTemplate, new delegateRegister(Register)), e);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Advanced Generic PreRequest Plugin without any custom functions
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PreRequest Advanced")]
    /// Specify a description for use in the user interface.
    [Description("The Advanced Generic PreRequest Plugin to set StringBody without any custom functions, and the template is defined in StringBody using Template opening & Template closing")]
    public class PreRequestPluginAdvanced : PreRequestPluginBase
    {
        [DisplayName("c- Template opening")]
        [Description("The template-def opening mark")]
        [DefaultValue("[[")]
        public string markOpen { get; set; }

        [DisplayName("d- Template closing")]
        [Description("The template-def closing mark")]
        [DefaultValue("]]")]
        public string markClose { get; set; }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            // Init vars
            // The template from request StringBody
            string theTemplate = Util.GetStringBodyTemplate(e, markOpen, markClose);

            Util.SetStringBody(Util.Transform(e.WebTest.Context[theModel].ToString(),
                theTemplate, new delegateRegister(Register)), e);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Base Generic PostRequest Plugin that others based on
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PostRequest =Base=")]
    /// Specify a description for use in the user interface.
    [Description("The Base Generic PostRequest Plugin (Do NOT use this)")]
    public class PostRequestPluginBase : Microsoft.VisualStudio.TestTools.WebTesting.WebTestRequestPlugin
    {
        [DisplayName("0- Comment")]
        [Description("What the plugin is for")]
        public string theComment { get; set; }

        [DisplayName("a- Source")]
        [Description("The name of Context Parameter (holding JSON string) used as data model to drive the template. Use the responce StringBody if empty.")]
        public string theModel { get; set; }

        [DisplayName("b- Filter")]
        [Description("The JSON Path selection query to filter data source. No filtering if empty.")]
        public string theFilter { get; set; }

        [DisplayName("d- Target")]
        [Description("The name of Context Parameter to hold the cooked result")]
        public string theTarget { get; set; }

        public virtual void Register(ScriptObject scriptObject)
        {
            Util.DefaultRegister(scriptObject);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Basic Generic PostRequest Plugin without any custom functions
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PostRequest Basic")]
    /// Specify a description for use in the user interface.
    [Description("The Basic Generic PostRequest Plugin to update Context Parameter without any custom functions")]
    public class PostRequestPluginBasic : PostRequestPluginBase
    {
        [DisplayName("c- Template")]
        [Description("The template to futher transform the data. No transformation if empty.")]
        public string theTemplate { get; set; }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            string retStr = Util.PostRequestTransform(theModel, theFilter, theTemplate, 
                new delegateRegister(Register), e);
            e.WebTest.Context.Add(theTarget, retStr);
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------
    /// <summary>
    /// The Advanced Generic PostRequest Plugin without any custom functions
    /// </summary>
    //-------------------------------------------------------------------------

    /// The user sees the following in the Add Extraction Rule dialog box.
    /// Specify a name for use in the user interface.
    [DisplayName("Ext-PostRequest Advanced")]
    /// Specify a description for use in the user interface.
    [Description("The Advanced Generic PostRequest Plugin to update Context Parameter without any custom functions")]
    public class PostRequestPluginAdvanced : PostRequestPluginBase
    {
        [DisplayName("e- Template opening")]
        [Description("The template-def opening mark")]
        [DefaultValue("[[")]
        public string markOpen { get; set; }

        [DisplayName("f- Template closing")]
        [Description("The template-def closing mark")]
        [DefaultValue("]]")]
        public string markClose { get; set; }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            string theTemplate = Util.GetStringBodyTemplate(e, markOpen, markClose);
            string retStr = Util.PostRequestTransform(theModel, theFilter, theTemplate,
                new delegateRegister(Register), e);
            e.WebTest.Context.Add(theTarget, retStr);
        }
    }

}
