﻿using System;
using System.Globalization;
using RestSharp;
using InfluxDB.Net.Models;
using RestSharp.Serializers;
using System.Collections.Generic;

namespace InfluxDB.Net.Core
{
    internal class InfluxDbClient : IInfluxDbClient
    {
        public const String U = "u";
        public const String P = "p";
        public const String Q = "q";
        public const String ID = "id";
        public const String NAME = "name";
        public const String DATABASE = "database";
        public const String TIME_PRECISION = "time_precision";

        private readonly string _url;
        private readonly string _username;
        private readonly string _password;

        public InfluxDbClient(string url, string username, string password)
        {
            _url = url;
            _username = username;
            _password = password;
        }

        public Pong Ping()
        {
            IRestResponse response = Request(Method.GET, "/ping", null, null, null, false);
            return response.ReadAs<Pong>();
        }

        public IRestResponse Version()
        {
            return Request(Method.GET, "/interfaces", null, null, null, false);
        }

        public IRestResponse CreateDatabase(Database database)
        {
            return Request(Method.POST, "/db", database);
        }

        public IRestResponse CreateDatabase(string name, DatabaseConfiguration config)
        {
            return Request(Method.POST, "/cluster/database_configs/{name}", config, new Dictionary<string, string>
            {
                { NAME, name }
            });
        }

        public IRestResponse DeleteDatabase(string name)
        {
            return Request(Method.DELETE, "/db/{name}", null, new Dictionary<string, string>
            {
                { NAME, name }
            });
        }

        public List<Database> DescribeDatabases()
        {
            IRestResponse response = Request(Method.GET, "/db");
            return response.ReadAs<List<Database>>();
        }

        public IRestResponse Write(string name, Serie[] series, string timePrecision)
        {
            return Request(Method.POST, "/db/{name}/series", series, new Dictionary<string, string>
            {
                { NAME, name },
                
            }, new Dictionary<string, string>
            {
                {TIME_PRECISION,timePrecision}
            });
        }

        public List<Serie> Query(string name, string query, string timePrecision)
        {
            IRestResponse response = Request(Method.GET, "/db/{name}/series", null, new Dictionary<string, string>
            {
                {NAME, name}

            }, new Dictionary<string, string>
            {
                {Q, query},
                {TIME_PRECISION, timePrecision}
            });

            return response.ReadAs<List<Serie>>();
        }

        public IRestResponse CreateClusterAdmin(User user)
        {
            return Request(Method.POST, "/cluster_admins", user);
        }

        public IRestResponse DeleteClusterAdmin(string name)
        {
            return Request(Method.DELETE, "/cluster_admins/{name}", null, new Dictionary<string, string>
            {
                { NAME, name }
            });
        }

        public List<User> DescribeClusterAdmins()
        {
            IRestResponse response = Request(Method.GET, "/cluster_admins");

            return response.ReadAs<List<User>>();
        }

        public IRestResponse UpdateClusterAdmin(User user, string name)
        {
            return Request(Method.POST, "/cluster_admins", user, new Dictionary<string, string>
            {
                { NAME, name }
            });
        }

        public IRestResponse CreateDatabaseUser(string database, User user)
        {
            return Request(Method.POST, "/db/{database}/users", user, new Dictionary<string, string>
            {
                { DATABASE, database }
            });
        }

        public IRestResponse DeleteDatabaseUser(string database, string name)
        {
            return Request(Method.DELETE, "/db/{database}/users/{name}", null, new Dictionary<string, string>
            {
                { DATABASE, database },
                { NAME, name }
            });
        }

        public List<User> DescribeDatabaseUsers(string database)
        {
            IRestResponse response = Request(Method.GET, "/db/{database}/users", null, new Dictionary<string, string>
            {
                { DATABASE, database }
            });

            return response.ReadAs<List<User>>();
        }

        public IRestResponse UpdateDatabaseUser(string database, User user, string name)
        {
            return Request(Method.POST, "/db/{database}/users/{name}", user, new Dictionary<string, string>
            {
                { DATABASE, database },
                { NAME, name }
            });
        }

        public IRestResponse AuthenticateDatabaseUser(string database, string user, string password)
        {
            return Request(Method.GET, "/db/{database}/authenticate", null, new Dictionary<string, string>
            {
                { DATABASE, database }
            });
        }

        public List<ContinuousQuery> GetContinuousQueries(string database)
        {
            IRestResponse response = Request(Method.GET, "/db/{database}/continuous_queries", null, new Dictionary<string, string>
            {
                { DATABASE, database }
            });

            return response.ReadAs<List<ContinuousQuery>>();
        }

        public IRestResponse DeleteContinuousQuery(string database, int id)
        {
            return Request(Method.DELETE, "/db/{database}/continuous_queries/{id}", null, new Dictionary<string, string>
            {
                { DATABASE, database },
                { ID, id.ToString(CultureInfo.InvariantCulture) }
            });
        }

        public IRestResponse DeleteSeries(string database, string name)
        {
            return Request(Method.DELETE, "/db/{database}/series/{name}", null, new Dictionary<string, string>
            {
                { DATABASE, database },
                { NAME, name }
            });
        }

        public IRestResponse ForceRaftCompaction()
        {
            return Request(Method.POST, "/raft/force_compaction");
        }

        public List<string> Interfaces()
        {
            IRestResponse response = Request(Method.GET, "/interfaces");

            return response.ReadAs<List<string>>();
        }

        public bool Sync()
        {
            IRestResponse response = Request(Method.GET, "/sync");

            return response.ReadAs<bool>();
        }

        public List<Server> ListServers()
        {
            IRestResponse response = Request(Method.GET, "/cluster/servers");

            return response.ReadAs<List<Server>>();
        }

        public IRestResponse RemoveServers(int id)
        {
            return Request(Method.DELETE, "/cluster/servers/{id}", null, new Dictionary<string, string> { { ID, id.ToString(CultureInfo.InvariantCulture) } });
        }

        public IRestResponse CreateShard(Shard shard)
        {
            return Request(Method.POST, "/cluster/shards", shard);
        }

        public Shards GetShards()
        {
            IRestResponse response = Request(Method.GET, "/cluster/shards");

            return response.ReadAs<Shards>();
        }

        public IRestResponse DropShard(int id, Shard.Member servers)
        {
            return Request(Method.DELETE, "/cluster/shards/{id}", servers, new Dictionary<string, string> { { ID, id.ToString(CultureInfo.InvariantCulture) } });
        }

        public List<ShardSpace> GetShardSpaces()
        {
            IRestResponse response = Request(Method.GET, "/cluster/shard_spaces");

            return response.ReadAs<List<ShardSpace>>();
        }

        public IRestResponse DropShardSpace(string database, string name)
        {
            return Request(Method.DELETE, "/cluster/shard_spaces/{database}/{name}", null, new Dictionary<string, string> { { DATABASE, database }, { NAME, name } });
        }

        public IRestResponse CreateShardSpace(string database, ShardSpace shardSpace)
        {
            return Request(Method.POST, "/cluster/shard_spaces/{database}", shardSpace, new Dictionary<string, string> { { DATABASE, database } });
        }

        private IRestResponse Request(Method requestMethod, string path, object body = null, Dictionary<string, string> segmentParams = null, Dictionary<string, string> extraParams = null, bool includeAuthToQuery = true)
        {
            try
            {
                RestRequest request;
                IRestClient client = PrepareClient(requestMethod, path, body, segmentParams, extraParams, includeAuthToQuery, out request);
                return client.Execute(request);
            }
            catch (Exception ex)
            {
                throw new InfluxDbException(string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, requestMethod), ex);
            }
        }

        private IRestClient PrepareClient(Method requestMethod, string path, object body, Dictionary<string, string> segmentParams, Dictionary<string, string> extraParams, bool includeAuthToQuery, out RestRequest request)
        {
            string url = string.Format("{0}{1}", _url, path);

            var client = new RestClient(url);
            request = new RestRequest(requestMethod)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new JsonSerializer()
            };

            if (includeAuthToQuery)
            {
                request.AddParameter(U, _username);
                request.AddParameter(P, _password);
            }

            if (segmentParams != null && segmentParams.Count > 0)
            {
                foreach (KeyValuePair<string, string> param in segmentParams)
                {
                    request.AddUrlSegment(param.Key, param.Value);
                }
            }
            if (extraParams != null && extraParams.Count > 0)
            {
                foreach (KeyValuePair<string, string> param in extraParams)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            if (body != null)
            {
                request.AddBody(body);
            }

            return client;
        }
    }
}