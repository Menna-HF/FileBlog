using Microsoft.AspNetCore.Mvc;

public class GetAllPostsRequest
{
    public List<string> Tags { get; set; } = [];
    public List<string> Categories { get; set; } = [];
}