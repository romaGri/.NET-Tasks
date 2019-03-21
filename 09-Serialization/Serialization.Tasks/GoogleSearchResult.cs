

using System.Runtime.Serialization;


namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



    public class GoogleSearchResult
    {

        [DataMember(Name = "kind")]
        public string kind { get; set; }
        [DataMember(Name = "uri")]
        public Url url { get; set; }
        [DataMember(Name = "queries")]
        public Queries queries { get; set; }
        [DataMember(Name = "context")]
        public Context context { get; set; }
        [DataMember(Name = "items")]
        public Item[] items { get; set; }
    }
    [DataContract]
    public class Url
    {
        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "template")]
        public string template { get; set; }
    }
    [DataContract]
    public class Queries
    {
        [DataMember(Name = "nextPage")]
        public Nextpage[] nextPage { get; set; }
        [DataMember(Name = "request")]
        public Request[] request { get; set; }
    }
    [DataContract]
    public class Nextpage
    {
        [DataMember(Name = "title")]
        public string title { get; set; }
        [DataMember(Name = "totalResults")]
        public int totalResults { get; set; }
        [DataMember(Name = "searchTerms")]
        public string searchTerms { get; set; }
        [DataMember(Name = "count")]
        public int count { get; set; }
        [DataMember(Name = "startIndex")]
        public int startIndex { get; set; }
        [DataMember(Name = "inputEncoding")]
        public string inputEncoding { get; set; }
        [DataMember(Name = "outputEncoding")]
        public string outputEncoding { get; set; }
        [DataMember(Name = "cx")]
        public string cx { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember(Name = "title")]
        public string title { get; set; }
        [DataMember(Name = "totalResults")]
        public int totalResults { get; set; }
        [DataMember(Name = "searchTerms")]
        public string searchTerms { get; set; }
        [DataMember(Name = "count")]
        public int count { get; set; }
        [DataMember(Name = "startIndex")]
        public int startIndex { get; set; }
        [DataMember(Name = "inputEncoding")]
        public string inputEncoding { get; set; }
        [DataMember(Name = "outputEncoding")]
        public string outputEncoding { get; set; }
        [DataMember(Name = "cx")]
        public string cx { get; set; }
    }

    [DataContract]
    public class Context
    {
        public string title { get; set; }
    }
    [DataContract]
    public class Item
    {
        [DataMember(Name = "kind")]
        public string kind { get; set; }
        [DataMember(Name = "title")]
        public string title { get; set; }
        [DataMember(Name = "htmlTitle")]
        public string htmlTitle { get; set; }
        [DataMember(Name = "link")]
        public string link { get; set; }
        [DataMember(Name = "displayLink")]
        public string displayLink { get; set; }
        [DataMember(Name = "snippet")]
        public string snippet { get; set; }
        [DataMember(Name = "htmlSnippet")]
        public string htmlSnippet { get; set; }
        [DataMember(Name = "pagemap")]
        public Pagemap pagemap { get; set; }
    }
    [DataContract]
    public class Pagemap
    {
        public RTO[] RTO { get; set; }
    }

    public class RTO
    {
        [DataMember(Name = "format")]
        public string format { get; set; }
        [DataMember(Name = "group_impression_tag")]
        public string group_impression_tag { get; set; }
        [DataMember(Name = "Optmax_rank_top")]
        public string Optmax_rank_top { get; set; }
        [DataMember(Name = "Optthreshold_override")]
        public string Optthreshold_override { get; set; }
        [DataMember(Name = "Optdisallow_same_domain")]
        public string Optdisallow_same_domain { get; set; }
        [DataMember(Name = "Outputtitle")]
        public string Outputtitle { get; set; }
        [DataMember(Name = "Outputwant_title_on_right")]
        public string Outputwant_title_on_right { get; set; }
        [DataMember(Name = "Outputnum_lines1")]
        public string Outputnum_lines1 { get; set; }
        [DataMember(Name = "Outputtext1")]
        public string Outputtext1 { get; set; }
        [DataMember(Name = "Outputgray1b")]
        public string Outputgray1b { get; set; }
        [DataMember(Name = "Outputno_clip1b")]
        public string Outputno_clip1b { get; set; }
        [DataMember(Name = "UrlOutputurl2")]
        public string UrlOutputurl2 { get; set; }
        [DataMember(Name = "Outputlink2")]
        public string Outputlink2 { get; set; }
        [DataMember(Name = "Outputtext2b")]
        public string Outputtext2b { get; set; }
        [DataMember(Name = "UrlOutputurl2c")]
        public string UrlOutputurl2c { get; set; }
        [DataMember(Name = "Outputlink2c")]
        public string Outputlink2c { get; set; }
        [DataMember(Name = "result_group_header")]
        public string result_group_header { get; set; }
        [DataMember(Name = "Outputimage_url")]
        public string Outputimage_url { get; set; }
        [DataMember(Name = "image_size")]
        public string image_size { get; set; }
        [DataMember(Name = "Outputinline_image_width")]
        public string Outputinline_image_width { get; set; }
        [DataMember(Name = "Outputinline_image_height")]
        public string Outputinline_image_height { get; set; }
        [DataMember(Name = "Outputimage_border")]
        public string Outputimage_border { get; set; }
    }


}


