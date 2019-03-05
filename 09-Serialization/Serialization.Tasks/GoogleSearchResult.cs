using System.Collections;

using System.Runtime.Serialization;


namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



    public class GoogleSearchResult
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "url")]
        public Url Url { get; set; }
        [DataMember(Name = "queries")]
        public Queue Queries { get; set; }
        [DataMember(Name = "context")]
        public Context Context { get; set; }
        [DataMember(Name = "items")]
        public Item[] Items { get; set; }

    }
     public class Url
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "template")]
        public string Template { get; set; }
    }
    public class Reqwest
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        public int StartIndex { get; set; }


    }
    public class Queries
    {
        [DataMember(Name = "nextPage")]
        public string NextPage { get; set; }
        [DataMember(Name = "prewiestPage")]
        public string PrewiestPage { get; set; }
        [DataMember(Name = "reqwest")]
        public string Reqwest { get; set; }

    }
    public class Context
    {
        [DataMember(Name = "")]
        public string Title { get; set; }
    }
    public class Item
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "htmlTitle")]
        public string HtmlTitle { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "displayLink")]
        public string DisplayLink { get; set; }
        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }
        [DataMember(Name = "htmlSnippet")]
        public string HtmlSnippet { get; set; }
    }

}
