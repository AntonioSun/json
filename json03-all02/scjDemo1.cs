////////////////////////////////////////////////////////////////////////////
// Porgram: Scriban Demo
// Purpose: C# Demo
// authors: Antonio Sun (c) 2017, All rights reserved
// Credits: as credited below
////////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
//using Newtonsoft.Json.Linq; // for JObject

using Scriban;
using Scriban.Runtime; // ScriptObject() & Import()

//using Util.Scriban;

namespace Demo1
{

    public class Owner
    {
        public string login { get; set; }
        public string type { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public Owner owner { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
        public int stargazers_count { get; set; }
        public int watchers_count { get; set; }
        public string language { get; set; }
        public int open_issues_count { get; set; }
        public double score { get; set; }
    }

    public class Repos
    {
        public int total_count { get; set; }
        public List<Item> items { get; set; }
    }

    class Program
    {
        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/blob/master/doc/language.md
        public static void Test1A()
        {
            var template = Template.Parse(@"{
                    ""total"": {{repos.total_count}},
                    ""items"": [
                    {
                {{ for item in repos.items }}
                        ""P"": {{item.full_name}},
                        ""O"": {{ item.owner.login | string.capitalize }}
                    },
                {{end}}
                    ]
                }");
            var repos = JsonConvert.DeserializeObject<Repos>(json);
            var result = template.Render(new { repos = repos });
            Console.WriteLine("\n## Test1A, json var");
            Console.WriteLine(result);
            ;
        }

        /// ////////////////////////////////////////////////////////////////////////////
         public static void Test1B()
        {
            Console.WriteLine("\n## Test1B, json obj");

            var repos = JsonConvert.DeserializeObject<Repos>(json);
            string repoStr = JsonConvert.SerializeObject(repos);
            Console.WriteLine(repoStr);
            var template = Template.Parse(@"
                {{
                repos = " + repoStr + @"
                }}
                {
                  ""total"": {{repos.total_count}},
                  ""items"": [
                    {
                {{ for item in repos.items }}
                      ""P"": {{item.full_name}},
                      ""O"": {{ item.owner.login | string.capitalize }}
                    },
                {{end}}
                  ]
                }");
            var result = template.Render();
            Console.WriteLine(result);
            //Console.ReadKey();
        }

         public static string OwnerToJSON(Item ii)
         {
             return JsonConvert.SerializeObject(ii.owner);
         }


         public static void Test2A()
         {
             var repos = JsonConvert.DeserializeObject<Repos>(json);

             var template = Template.Parse(@"{
                    ""total"": {{repos.total_count}},
                    ""items"": [
                    {
                {{ for item in repos.items }}
                        ""P"": {{item.full_name}},
                        ""O"": {{ item.owner.login | string.capitalize }}
                    },
                {{end}}
                    ]
                }");
             var model = new { repos = repos };
             Util.Scriban.globalFunctions.Import(model);

             Util.Scriban.globalContext.PushGlobal(Util.Scriban.globalFunctions);
             template.Render(Util.Scriban.globalContext);
             Util.Scriban.globalContext.PopGlobal();

             var result = Util.Scriban.globalContext.Output.ToString();

             Console.WriteLine("\n## Test2A, json var, NOK");
             Console.WriteLine(result);
             Console.ReadKey();
         }

         public static void TestDataTable()
         {
             Console.WriteLine("\n## TestC, DataTable");

             System.Data.DataTable dataTable = new System.Data.DataTable();
             dataTable.Columns.Add("Column1");
             dataTable.Columns.Add("Column2");

             System.Data.DataRow dataRow = dataTable.NewRow();
             dataRow["Column1"] = "Hello";
             dataRow["Column2"] = "World";
             dataTable.Rows.Add(dataRow);

             dataRow = dataTable.NewRow();
             dataRow["Column1"] = "Bonjour";
             dataRow["Column2"] = "le monde";
             dataTable.Rows.Add(dataRow);

             string json = JsonConvert.SerializeObject(dataTable);
             Console.WriteLine("Json: "+ json);

             {
                 string myTemplate = @"
                {{
                tb = " + json + @"
                }}
[
  { {{ for tbr in tb }}
    ""N"": {{tbr.Column1}},
    ""M"": {{tbr.Column2}}
  }{{if for.last; ; else; "",""; end}}{{end}}
]
{{tb}}
";

                 var template = Template.Parse(myTemplate);
                 var result = template.Render();
                 Console.WriteLine(result);
             }
             {
                 var parsed = JsonConvert.DeserializeObject(json);
                 Console.WriteLine("Parsed: "+ parsed);

                 string myTemplate = @"
[
  { {{ for tbr in tb }}
    ""N"": {{tbr.Column1}},
    ""M"": {{tbr.Column2}}
  }{{if for.last; ; else; "",""; end}}{{end}}
]
{{tb}}
";

                 var template = Template.Parse(myTemplate);
                 var result = template.Render(new { tb = parsed });
                 Console.WriteLine(result);
             }
         }

         /// ////////////////////////////////////////////////////////////////////////////
         public static string GetJsonTable()
         {
             DataTable dataTable = GetDataTable("SELECT top 5 name, recovery_model_desc FROM sys.databases order by name",
                 "Data Source=(local);Initial Catalog=master;Integrated Security=True");
             return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
         }

         public static DataTable GetDataTable(string _SqlCommand, string _ConnectionStr)
         {
             DataTable dataTable = new DataTable();

             using (SqlConnection connection = new SqlConnection(_ConnectionStr))
             using (SqlCommand cmd = connection.CreateCommand())
             {
                 connection.Open();
                 cmd.CommandText = _SqlCommand;
                 //cmd.CommandType = CommandType.StoredProcedure;
                 //cmd.Parameters.AddWithValue("@City", txtCity.Text);

                 using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     dataTable.Load(dr);
                 }
             }
             return dataTable;
         }
         public static void TestDBTable()
         {
             Console.WriteLine("\n## TestD, DataTable from DB");

             string json = GetJsonTable();
             Console.WriteLine(json);
             var parsed = JsonConvert.DeserializeObject(json);
             Console.WriteLine(parsed);

             {
                 string myTemplate = @"
{{tb}}
[
  { {{ for tbr in tb }}
    ""N"": {{tbr.name}},
    ""M"": {{tbr.recovery_model_desc}}
  }{{if for.last; ; else; "",""; end}}{{end}}
]
";
                 //Template.RegisterSafeType(typeof(JObject), Hash.FromAnonymousObject);

                 var template = Template.Parse(myTemplate);
                 var result = template.Render(new { tb = parsed });
                 Console.WriteLine(result);
             }
             {
                 string myTemplate = @"
                {{
                tb = " + json + @"
                }}
{{tb}}
[
  { {{ for tbr in tb }}
    ""N"": {{tbr.name}},
    ""M"": {{tbr.recovery_model_desc}}
  }{{if for.last; ; else; "",""; end}}{{end}}
]
";
                 //Template.RegisterSafeType(typeof(JObject), Hash.FromAnonymousObject);

                 var template = Template.Parse(myTemplate);
                 var result = template.Render();
                 Console.WriteLine(result);
             }
         }

        /// ////////////////////////////////////////////////////////////////////////////
        /// https://github.com/lunet-io/scriban/issues/9
        public static void Test3()
        {
            {
                var template = Template.Parse(@"{
  ""total"": {{repos.total_count}},
  ""items"": [
    {
{{ for item in repos.items }}
      ""P"": {{item.full_name}},
      ""O"": {{ item.owner.login | string.capitalize }}
    },
{{end}}
  ]
}");
            }
        }

        public static string json = @"{
  'total_count': 5,
  'incomplete_results': false,
  'items': [
    {
      'id': 3544424,
      'name': 'httpie',
      'full_name': 'jkbrzt/httpie',
      'owner': {
        'login': 'jkbrzt',
        'id': 326885,
        'avatar_url': 'https://avatars.githubusercontent.com/u/326885?v=3',
        'gravatar_id': '',
        'url': 'https://api.github.com/users/jkbrzt',
        'html_url': 'https://github.com/jkbrzt',
        'followers_url': 'https://api.github.com/users/jkbrzt/followers',
        'following_url': 'https://api.github.com/users/jkbrzt/following{/other_user}',
        'gists_url': 'https://api.github.com/users/jkbrzt/gists{/gist_id}',
        'starred_url': 'https://api.github.com/users/jkbrzt/starred{/owner}{/repo}',
        'subscriptions_url': 'https://api.github.com/users/jkbrzt/subscriptions',
        'organizations_url': 'https://api.github.com/users/jkbrzt/orgs',
        'repos_url': 'https://api.github.com/users/jkbrzt/repos',
        'events_url': 'https://api.github.com/users/jkbrzt/events{/privacy}',
        'received_events_url': 'https://api.github.com/users/jkbrzt/received_events',
        'type': 'User',
        'site_admin': false
      },
      'private': false,
      'html_url': 'https://github.com/jkbrzt/httpie',
      'description': 'Modern command line HTTP client  user-friendly curl alternative with intuitive UI, JSON support, syntax highlighting, wget-like downloads, extensions, etc. Follow https://twitter.com/clihttp for tips and updates.',
      'fork': false,
      'url': 'https://api.github.com/repos/jkbrzt/httpie',
      'forks_url': 'https://api.github.com/repos/jkbrzt/httpie/forks',
      'keys_url': 'https://api.github.com/repos/jkbrzt/httpie/keys{/key_id}',
      'collaborators_url': 'https://api.github.com/repos/jkbrzt/httpie/collaborators{/collaborator}',
      'teams_url': 'https://api.github.com/repos/jkbrzt/httpie/teams',
      'hooks_url': 'https://api.github.com/repos/jkbrzt/httpie/hooks',
      'issue_events_url': 'https://api.github.com/repos/jkbrzt/httpie/issues/events{/number}',
      'events_url': 'https://api.github.com/repos/jkbrzt/httpie/events',
      'assignees_url': 'https://api.github.com/repos/jkbrzt/httpie/assignees{/user}',
      'branches_url': 'https://api.github.com/repos/jkbrzt/httpie/branches{/branch}',
      'tags_url': 'https://api.github.com/repos/jkbrzt/httpie/tags',
      'blobs_url': 'https://api.github.com/repos/jkbrzt/httpie/git/blobs{/sha}',
      'git_tags_url': 'https://api.github.com/repos/jkbrzt/httpie/git/tags{/sha}',
      'git_refs_url': 'https://api.github.com/repos/jkbrzt/httpie/git/refs{/sha}',
      'trees_url': 'https://api.github.com/repos/jkbrzt/httpie/git/trees{/sha}',
      'statuses_url': 'https://api.github.com/repos/jkbrzt/httpie/statuses/{sha}',
      'languages_url': 'https://api.github.com/repos/jkbrzt/httpie/languages',
      'stargazers_url': 'https://api.github.com/repos/jkbrzt/httpie/stargazers',
      'contributors_url': 'https://api.github.com/repos/jkbrzt/httpie/contributors',
      'subscribers_url': 'https://api.github.com/repos/jkbrzt/httpie/subscribers',
      'subscription_url': 'https://api.github.com/repos/jkbrzt/httpie/subscription',
      'commits_url': 'https://api.github.com/repos/jkbrzt/httpie/commits{/sha}',
      'git_commits_url': 'https://api.github.com/repos/jkbrzt/httpie/git/commits{/sha}',
      'comments_url': 'https://api.github.com/repos/jkbrzt/httpie/comments{/number}',
      'issue_comment_url': 'https://api.github.com/repos/jkbrzt/httpie/issues/comments{/number}',
      'contents_url': 'https://api.github.com/repos/jkbrzt/httpie/contents/{+path}',
      'compare_url': 'https://api.github.com/repos/jkbrzt/httpie/compare/{base}...{head}',
      'merges_url': 'https://api.github.com/repos/jkbrzt/httpie/merges',
      'archive_url': 'https://api.github.com/repos/jkbrzt/httpie/{archive_format}{/ref}',
      'downloads_url': 'https://api.github.com/repos/jkbrzt/httpie/downloads',
      'issues_url': 'https://api.github.com/repos/jkbrzt/httpie/issues{/number}',
      'pulls_url': 'https://api.github.com/repos/jkbrzt/httpie/pulls{/number}',
      'milestones_url': 'https://api.github.com/repos/jkbrzt/httpie/milestones{/number}',
      'notifications_url': 'https://api.github.com/repos/jkbrzt/httpie/notifications{?since,all,participating}',
      'labels_url': 'https://api.github.com/repos/jkbrzt/httpie/labels{/name}',
      'releases_url': 'https://api.github.com/repos/jkbrzt/httpie/releases{/id}',
      'deployments_url': 'https://api.github.com/repos/jkbrzt/httpie/deployments',
      'created_at': '2012-02-25T12:39:13Z',
      'updated_at': '2017-01-23T20:36:31Z',
      'pushed_at': '2017-01-22T20:59:07Z',
      'git_url': 'git://github.com/jkbrzt/httpie.git',
      'ssh_url': 'git@github.com:jkbrzt/httpie.git',
      'clone_url': 'https://github.com/jkbrzt/httpie.git',
      'svn_url': 'https://github.com/jkbrzt/httpie',
      'homepage': 'https://httpie.org',
      'size': 3609,
      'stargazers_count': 27839,
      'watchers_count': 27839,
      'language': 'Python',
      'has_issues': true,
      'has_downloads': true,
      'has_wiki': false,
      'has_pages': false,
      'forks_count': 1801,
      'mirror_url': null,
      'open_issues_count': 90,
      'forks': 1801,
      'open_issues': 90,
      'watchers': 27839,
      'default_branch': 'master',
      'score': 7.144833
    },
    {
      'id': 3678731,
      'name': 'webpack',
      'full_name': 'webpack/webpack',
      'owner': {
        'login': 'webpack',
        'id': 2105791,
        'avatar_url': 'https://avatars.githubusercontent.com/u/2105791?v=3',
        'gravatar_id': '',
        'url': 'https://api.github.com/users/webpack',
        'html_url': 'https://github.com/webpack',
        'followers_url': 'https://api.github.com/users/webpack/followers',
        'following_url': 'https://api.github.com/users/webpack/following{/other_user}',
        'gists_url': 'https://api.github.com/users/webpack/gists{/gist_id}',
        'starred_url': 'https://api.github.com/users/webpack/starred{/owner}{/repo}',
        'subscriptions_url': 'https://api.github.com/users/webpack/subscriptions',
        'organizations_url': 'https://api.github.com/users/webpack/orgs',
        'repos_url': 'https://api.github.com/users/webpack/repos',
        'events_url': 'https://api.github.com/users/webpack/events{/privacy}',
        'received_events_url': 'https://api.github.com/users/webpack/received_events',
        'type': 'Organization',
        'site_admin': false
      },
      'private': false,
      'html_url': 'https://github.com/webpack/webpack',
      'description': 'A bundler for javascript and friends. Packs many modules into a few bundled assets. Code Splitting allows to load parts for the application on demand. Through \'loaders,\' modules can be CommonJs, AMD, ES6 modules, CSS, Images, JSON, Coffeescript, LESS, ... and your custom stuff.',
      'fork': false,
      'url': 'https://api.github.com/repos/webpack/webpack',
      'forks_url': 'https://api.github.com/repos/webpack/webpack/forks',
      'keys_url': 'https://api.github.com/repos/webpack/webpack/keys{/key_id}',
      'collaborators_url': 'https://api.github.com/repos/webpack/webpack/collaborators{/collaborator}',
      'teams_url': 'https://api.github.com/repos/webpack/webpack/teams',
      'hooks_url': 'https://api.github.com/repos/webpack/webpack/hooks',
      'issue_events_url': 'https://api.github.com/repos/webpack/webpack/issues/events{/number}',
      'events_url': 'https://api.github.com/repos/webpack/webpack/events',
      'assignees_url': 'https://api.github.com/repos/webpack/webpack/assignees{/user}',
      'branches_url': 'https://api.github.com/repos/webpack/webpack/branches{/branch}',
      'tags_url': 'https://api.github.com/repos/webpack/webpack/tags',
      'blobs_url': 'https://api.github.com/repos/webpack/webpack/git/blobs{/sha}',
      'git_tags_url': 'https://api.github.com/repos/webpack/webpack/git/tags{/sha}',
      'git_refs_url': 'https://api.github.com/repos/webpack/webpack/git/refs{/sha}',
      'trees_url': 'https://api.github.com/repos/webpack/webpack/git/trees{/sha}',
      'statuses_url': 'https://api.github.com/repos/webpack/webpack/statuses/{sha}',
      'languages_url': 'https://api.github.com/repos/webpack/webpack/languages',
      'stargazers_url': 'https://api.github.com/repos/webpack/webpack/stargazers',
      'contributors_url': 'https://api.github.com/repos/webpack/webpack/contributors',
      'subscribers_url': 'https://api.github.com/repos/webpack/webpack/subscribers',
      'subscription_url': 'https://api.github.com/repos/webpack/webpack/subscription',
      'commits_url': 'https://api.github.com/repos/webpack/webpack/commits{/sha}',
      'git_commits_url': 'https://api.github.com/repos/webpack/webpack/git/commits{/sha}',
      'comments_url': 'https://api.github.com/repos/webpack/webpack/comments{/number}',
      'issue_comment_url': 'https://api.github.com/repos/webpack/webpack/issues/comments{/number}',
      'contents_url': 'https://api.github.com/repos/webpack/webpack/contents/{+path}',
      'compare_url': 'https://api.github.com/repos/webpack/webpack/compare/{base}...{head}',
      'merges_url': 'https://api.github.com/repos/webpack/webpack/merges',
      'archive_url': 'https://api.github.com/repos/webpack/webpack/{archive_format}{/ref}',
      'downloads_url': 'https://api.github.com/repos/webpack/webpack/downloads',
      'issues_url': 'https://api.github.com/repos/webpack/webpack/issues{/number}',
      'pulls_url': 'https://api.github.com/repos/webpack/webpack/pulls{/number}',
      'milestones_url': 'https://api.github.com/repos/webpack/webpack/milestones{/number}',
      'notifications_url': 'https://api.github.com/repos/webpack/webpack/notifications{?since,all,participating}',
      'labels_url': 'https://api.github.com/repos/webpack/webpack/labels{/name}',
      'releases_url': 'https://api.github.com/repos/webpack/webpack/releases{/id}',
      'deployments_url': 'https://api.github.com/repos/webpack/webpack/deployments',
      'created_at': '2012-03-10T10:08:14Z',
      'updated_at': '2017-01-23T21:20:13Z',
      'pushed_at': '2017-01-23T19:37:38Z',
      'git_url': 'git://github.com/webpack/webpack.git',
      'ssh_url': 'git@github.com:webpack/webpack.git',
      'clone_url': 'https://github.com/webpack/webpack.git',
      'svn_url': 'https://github.com/webpack/webpack',
      'homepage': 'https://webpack.js.org',
      'size': 5965,
      'stargazers_count': 23426,
      'watchers_count': 23426,
      'language': 'JavaScript',
      'has_issues': true,
      'has_downloads': true,
      'has_wiki': true,
      'has_pages': false,
      'forks_count': 2685,
      'mirror_url': null,
      'open_issues_count': 690,
      'forks': 2685,
      'open_issues': 690,
      'watchers': 23426,
      'default_branch': 'master',
      'score': 5.859145
    },
    {
      'id': 14747598,
      'name': 'json-server',
      'full_name': 'typicode/json-server',
      'owner': {
        'login': 'typicode',
        'id': 5502029,
        'avatar_url': 'https://avatars.githubusercontent.com/u/5502029?v=3',
        'gravatar_id': '',
        'url': 'https://api.github.com/users/typicode',
        'html_url': 'https://github.com/typicode',
        'followers_url': 'https://api.github.com/users/typicode/followers',
        'following_url': 'https://api.github.com/users/typicode/following{/other_user}',
        'gists_url': 'https://api.github.com/users/typicode/gists{/gist_id}',
        'starred_url': 'https://api.github.com/users/typicode/starred{/owner}{/repo}',
        'subscriptions_url': 'https://api.github.com/users/typicode/subscriptions',
        'organizations_url': 'https://api.github.com/users/typicode/orgs',
        'repos_url': 'https://api.github.com/users/typicode/repos',
        'events_url': 'https://api.github.com/users/typicode/events{/privacy}',
        'received_events_url': 'https://api.github.com/users/typicode/received_events',
        'type': 'User',
        'site_admin': false
      },
      'private': false,
      'html_url': 'https://github.com/typicode/json-server',
      'description': 'Get a full fake REST API with zero coding in less than 30 seconds (seriously)',
      'fork': false,
      'url': 'https://api.github.com/repos/typicode/json-server',
      'forks_url': 'https://api.github.com/repos/typicode/json-server/forks',
      'keys_url': 'https://api.github.com/repos/typicode/json-server/keys{/key_id}',
      'collaborators_url': 'https://api.github.com/repos/typicode/json-server/collaborators{/collaborator}',
      'teams_url': 'https://api.github.com/repos/typicode/json-server/teams',
      'hooks_url': 'https://api.github.com/repos/typicode/json-server/hooks',
      'issue_events_url': 'https://api.github.com/repos/typicode/json-server/issues/events{/number}',
      'events_url': 'https://api.github.com/repos/typicode/json-server/events',
      'assignees_url': 'https://api.github.com/repos/typicode/json-server/assignees{/user}',
      'branches_url': 'https://api.github.com/repos/typicode/json-server/branches{/branch}',
      'tags_url': 'https://api.github.com/repos/typicode/json-server/tags',
      'blobs_url': 'https://api.github.com/repos/typicode/json-server/git/blobs{/sha}',
      'git_tags_url': 'https://api.github.com/repos/typicode/json-server/git/tags{/sha}',
      'git_refs_url': 'https://api.github.com/repos/typicode/json-server/git/refs{/sha}',
      'trees_url': 'https://api.github.com/repos/typicode/json-server/git/trees{/sha}',
      'statuses_url': 'https://api.github.com/repos/typicode/json-server/statuses/{sha}',
      'languages_url': 'https://api.github.com/repos/typicode/json-server/languages',
      'stargazers_url': 'https://api.github.com/repos/typicode/json-server/stargazers',
      'contributors_url': 'https://api.github.com/repos/typicode/json-server/contributors',
      'subscribers_url': 'https://api.github.com/repos/typicode/json-server/subscribers',
      'subscription_url': 'https://api.github.com/repos/typicode/json-server/subscription',
      'commits_url': 'https://api.github.com/repos/typicode/json-server/commits{/sha}',
      'git_commits_url': 'https://api.github.com/repos/typicode/json-server/git/commits{/sha}',
      'comments_url': 'https://api.github.com/repos/typicode/json-server/comments{/number}',
      'issue_comment_url': 'https://api.github.com/repos/typicode/json-server/issues/comments{/number}',
      'contents_url': 'https://api.github.com/repos/typicode/json-server/contents/{+path}',
      'compare_url': 'https://api.github.com/repos/typicode/json-server/compare/{base}...{head}',
      'merges_url': 'https://api.github.com/repos/typicode/json-server/merges',
      'archive_url': 'https://api.github.com/repos/typicode/json-server/{archive_format}{/ref}',
      'downloads_url': 'https://api.github.com/repos/typicode/json-server/downloads',
      'issues_url': 'https://api.github.com/repos/typicode/json-server/issues{/number}',
      'pulls_url': 'https://api.github.com/repos/typicode/json-server/pulls{/number}',
      'milestones_url': 'https://api.github.com/repos/typicode/json-server/milestones{/number}',
      'notifications_url': 'https://api.github.com/repos/typicode/json-server/notifications{?since,all,participating}',
      'labels_url': 'https://api.github.com/repos/typicode/json-server/labels{/name}',
      'releases_url': 'https://api.github.com/repos/typicode/json-server/releases{/id}',
      'deployments_url': 'https://api.github.com/repos/typicode/json-server/deployments',
      'created_at': '2013-11-27T13:21:13Z',
      'updated_at': '2017-01-23T21:19:41Z',
      'pushed_at': '2017-01-10T16:14:30Z',
      'git_url': 'git://github.com/typicode/json-server.git',
      'ssh_url': 'git@github.com:typicode/json-server.git',
      'clone_url': 'https://github.com/typicode/json-server.git',
      'svn_url': 'https://github.com/typicode/json-server',
      'homepage': '',
      'size': 501,
      'stargazers_count': 18677,
      'watchers_count': 18677,
      'language': 'JavaScript',
      'has_issues': true,
      'has_downloads': true,
      'has_wiki': false,
      'has_pages': false,
      'forks_count': 1323,
      'mirror_url': null,
      'open_issues_count': 129,
      'forks': 1323,
      'open_issues': 129,
      'watchers': 18677,
      'default_branch': 'master',
      'score': 35.188576
    },
    {
      'id': 20965586,
      'name': 'SwiftyJSON',
      'full_name': 'SwiftyJSON/SwiftyJSON',
      'owner': {
        'login': 'SwiftyJSON',
        'id': 8858017,
        'avatar_url': 'https://avatars.githubusercontent.com/u/8858017?v=3',
        'gravatar_id': '',
        'url': 'https://api.github.com/users/SwiftyJSON',
        'html_url': 'https://github.com/SwiftyJSON',
        'followers_url': 'https://api.github.com/users/SwiftyJSON/followers',
        'following_url': 'https://api.github.com/users/SwiftyJSON/following{/other_user}',
        'gists_url': 'https://api.github.com/users/SwiftyJSON/gists{/gist_id}',
        'starred_url': 'https://api.github.com/users/SwiftyJSON/starred{/owner}{/repo}',
        'subscriptions_url': 'https://api.github.com/users/SwiftyJSON/subscriptions',
        'organizations_url': 'https://api.github.com/users/SwiftyJSON/orgs',
        'repos_url': 'https://api.github.com/users/SwiftyJSON/repos',
        'events_url': 'https://api.github.com/users/SwiftyJSON/events{/privacy}',
        'received_events_url': 'https://api.github.com/users/SwiftyJSON/received_events',
        'type': 'Organization',
        'site_admin': false
      },
      'private': false,
      'html_url': 'https://github.com/SwiftyJSON/SwiftyJSON',
      'description': 'The better way to deal with JSON data in Swift',
      'fork': false,
      'url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON',
      'forks_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/forks',
      'keys_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/keys{/key_id}',
      'collaborators_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/collaborators{/collaborator}',
      'teams_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/teams',
      'hooks_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/hooks',
      'issue_events_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/issues/events{/number}',
      'events_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/events',
      'assignees_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/assignees{/user}',
      'branches_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/branches{/branch}',
      'tags_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/tags',
      'blobs_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/git/blobs{/sha}',
      'git_tags_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/git/tags{/sha}',
      'git_refs_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/git/refs{/sha}',
      'trees_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/git/trees{/sha}',
      'statuses_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/statuses/{sha}',
      'languages_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/languages',
      'stargazers_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/stargazers',
      'contributors_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/contributors',
      'subscribers_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/subscribers',
      'subscription_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/subscription',
      'commits_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/commits{/sha}',
      'git_commits_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/git/commits{/sha}',
      'comments_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/comments{/number}',
      'issue_comment_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/issues/comments{/number}',
      'contents_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/contents/{+path}',
      'compare_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/compare/{base}...{head}',
      'merges_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/merges',
      'archive_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/{archive_format}{/ref}',
      'downloads_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/downloads',
      'issues_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/issues{/number}',
      'pulls_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/pulls{/number}',
      'milestones_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/milestones{/number}',
      'notifications_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/notifications{?since,all,participating}',
      'labels_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/labels{/name}',
      'releases_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/releases{/id}',
      'deployments_url': 'https://api.github.com/repos/SwiftyJSON/SwiftyJSON/deployments',
      'created_at': '2014-06-18T14:41:15Z',
      'updated_at': '2017-01-23T20:38:27Z',
      'pushed_at': '2017-01-12T11:35:00Z',
      'git_url': 'git://github.com/SwiftyJSON/SwiftyJSON.git',
      'ssh_url': 'git@github.com:SwiftyJSON/SwiftyJSON.git',
      'clone_url': 'https://github.com/SwiftyJSON/SwiftyJSON.git',
      'svn_url': 'https://github.com/SwiftyJSON/SwiftyJSON',
      'homepage': '',
      'size': 813,
      'stargazers_count': 13171,
      'watchers_count': 13171,
      'language': 'Swift',
      'has_issues': true,
      'has_downloads': true,
      'has_wiki': false,
      'has_pages': false,
      'forks_count': 2442,
      'mirror_url': null,
      'open_issues_count': 46,
      'forks': 2442,
      'open_issues': 46,
      'watchers': 13171,
      'default_branch': 'master',
      'score': 36.65664
    },
    {
      'id': 54346799,
      'name': 'public-apis',
      'full_name': 'toddmotto/public-apis',
      'owner': {
        'login': 'toddmotto',
        'id': 1655968,
        'avatar_url': 'https://avatars.githubusercontent.com/u/1655968?v=3',
        'gravatar_id': '',
        'url': 'https://api.github.com/users/toddmotto',
        'html_url': 'https://github.com/toddmotto',
        'followers_url': 'https://api.github.com/users/toddmotto/followers',
        'following_url': 'https://api.github.com/users/toddmotto/following{/other_user}',
        'gists_url': 'https://api.github.com/users/toddmotto/gists{/gist_id}',
        'starred_url': 'https://api.github.com/users/toddmotto/starred{/owner}{/repo}',
        'subscriptions_url': 'https://api.github.com/users/toddmotto/subscriptions',
        'organizations_url': 'https://api.github.com/users/toddmotto/orgs',
        'repos_url': 'https://api.github.com/users/toddmotto/repos',
        'events_url': 'https://api.github.com/users/toddmotto/events{/privacy}',
        'received_events_url': 'https://api.github.com/users/toddmotto/received_events',
        'type': 'User',
        'site_admin': false
      },
      'private': false,
      'html_url': 'https://github.com/toddmotto/public-apis',
      'description': 'A collective list of public JSON APIs for use in web development.',
      'fork': false,
      'url': 'https://api.github.com/repos/toddmotto/public-apis',
      'forks_url': 'https://api.github.com/repos/toddmotto/public-apis/forks',
      'keys_url': 'https://api.github.com/repos/toddmotto/public-apis/keys{/key_id}',
      'collaborators_url': 'https://api.github.com/repos/toddmotto/public-apis/collaborators{/collaborator}',
      'teams_url': 'https://api.github.com/repos/toddmotto/public-apis/teams',
      'hooks_url': 'https://api.github.com/repos/toddmotto/public-apis/hooks',
      'issue_events_url': 'https://api.github.com/repos/toddmotto/public-apis/issues/events{/number}',
      'events_url': 'https://api.github.com/repos/toddmotto/public-apis/events',
      'assignees_url': 'https://api.github.com/repos/toddmotto/public-apis/assignees{/user}',
      'branches_url': 'https://api.github.com/repos/toddmotto/public-apis/branches{/branch}',
      'tags_url': 'https://api.github.com/repos/toddmotto/public-apis/tags',
      'blobs_url': 'https://api.github.com/repos/toddmotto/public-apis/git/blobs{/sha}',
      'git_tags_url': 'https://api.github.com/repos/toddmotto/public-apis/git/tags{/sha}',
      'git_refs_url': 'https://api.github.com/repos/toddmotto/public-apis/git/refs{/sha}',
      'trees_url': 'https://api.github.com/repos/toddmotto/public-apis/git/trees{/sha}',
      'statuses_url': 'https://api.github.com/repos/toddmotto/public-apis/statuses/{sha}',
      'languages_url': 'https://api.github.com/repos/toddmotto/public-apis/languages',
      'stargazers_url': 'https://api.github.com/repos/toddmotto/public-apis/stargazers',
      'contributors_url': 'https://api.github.com/repos/toddmotto/public-apis/contributors',
      'subscribers_url': 'https://api.github.com/repos/toddmotto/public-apis/subscribers',
      'subscription_url': 'https://api.github.com/repos/toddmotto/public-apis/subscription',
      'commits_url': 'https://api.github.com/repos/toddmotto/public-apis/commits{/sha}',
      'git_commits_url': 'https://api.github.com/repos/toddmotto/public-apis/git/commits{/sha}',
      'comments_url': 'https://api.github.com/repos/toddmotto/public-apis/comments{/number}',
      'issue_comment_url': 'https://api.github.com/repos/toddmotto/public-apis/issues/comments{/number}',
      'contents_url': 'https://api.github.com/repos/toddmotto/public-apis/contents/{+path}',
      'compare_url': 'https://api.github.com/repos/toddmotto/public-apis/compare/{base}...{head}',
      'merges_url': 'https://api.github.com/repos/toddmotto/public-apis/merges',
      'archive_url': 'https://api.github.com/repos/toddmotto/public-apis/{archive_format}{/ref}',
      'downloads_url': 'https://api.github.com/repos/toddmotto/public-apis/downloads',
      'issues_url': 'https://api.github.com/repos/toddmotto/public-apis/issues{/number}',
      'pulls_url': 'https://api.github.com/repos/toddmotto/public-apis/pulls{/number}',
      'milestones_url': 'https://api.github.com/repos/toddmotto/public-apis/milestones{/number}',
      'notifications_url': 'https://api.github.com/repos/toddmotto/public-apis/notifications{?since,all,participating}',
      'labels_url': 'https://api.github.com/repos/toddmotto/public-apis/labels{/name}',
      'releases_url': 'https://api.github.com/repos/toddmotto/public-apis/releases{/id}',
      'deployments_url': 'https://api.github.com/repos/toddmotto/public-apis/deployments',
      'created_at': '2016-03-20T23:49:42Z',
      'updated_at': '2017-01-23T20:54:55Z',
      'pushed_at': '2017-01-20T15:27:46Z',
      'git_url': 'git://github.com/toddmotto/public-apis.git',
      'ssh_url': 'git@github.com:toddmotto/public-apis.git',
      'clone_url': 'https://github.com/toddmotto/public-apis.git',
      'svn_url': 'https://github.com/toddmotto/public-apis',
      'homepage': 'https://ultimateangular.com',
      'size': 238,
      'stargazers_count': 11552,
      'watchers_count': 11552,
      'language': null,
      'has_issues': true,
      'has_downloads': true,
      'has_wiki': true,
      'has_pages': false,
      'forks_count': 803,
      'mirror_url': null,
      'open_issues_count': 6,
      'forks': 803,
      'open_issues': 6,
      'watchers': 11552,
      'default_branch': 'master',
      'score': 10.332432
    }
  ]
        }";
    }
}