namespace PersonAPI.Models {
    public class Person {
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Profession { get; set; }
        public string ImageRef { get; set; }
        public Address ContactInfo { get; set; }
        public string WebsiteURL { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
    }
}
