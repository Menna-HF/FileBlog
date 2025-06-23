using Microsoft.AspNetCore.Mvc;

public class GetAllPostsRequest
{
    [FromQuery]
    public List<string> Tags { get; set; } = [];
    [FromQuery]
    public List<string> Categories { get; set; } = [];
}