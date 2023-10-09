using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

var companies = new List<Company>();
// First Company json format
builder.Configuration.AddJsonFile("google.json");

var googleCompany = builder.Configuration.GetSection("GoogleCompany").Get<Company>();
companies.Add(googleCompany);
//Second company. Ini file
builder.Configuration.AddIniFile("twitter.ini");

var twitterCompany = builder.Configuration.GetSection("TwitterCompany").Get<Company>();
companies.Add(twitterCompany);

//Third company. Xml file
builder.Configuration.AddXmlFile("samsung.xml");

var samsungCompany = new Company();
app.Configuration.Bind(samsungCompany);
companies.Add(samsungCompany);

//Json file for task 2
builder.Configuration.AddJsonFile("person.json");

var person = builder.Configuration.GetSection("Person").Get<Person>();

app.MapGet("/", () => "Enter '/task1' or '/task2'");

app.MapGet("/task1", () =>
{
    int maxEmployeesNumber = companies[0].Employees;
    string nameWithMaxEmployeesCompany = companies[0].Name;
    foreach (var company in companies)
    {
        if (maxEmployeesNumber < company.Employees)
        {
            maxEmployeesNumber = company.Employees;
            nameWithMaxEmployeesCompany = company.Name;
        }
    }

    return $"The company with the largest number of employees is {nameWithMaxEmployeesCompany}";
});

app.MapGet("/task2", async (HttpContext context) =>
{ 
    System.Text.StringBuilder stringBuilder = new();
    if (person != null)
    {
        stringBuilder.Append($"<p>Name: {person.Name}</p>");
        stringBuilder.Append($"<p>Age: {person.Age}</p>");
        stringBuilder.Append($"<p>Country: {person.Country}</p>");
        stringBuilder.Append($"<p>University name: {person.University?.Name}</p>");
        stringBuilder.Append($"<p>City: {person.University?.City}</p>");
        stringBuilder.Append($"<p>Course: {person.University?.Course}</p>");
        stringBuilder.Append("<h3>Languages</h3><ul>");
        foreach (string lang in person.Languages)
            stringBuilder.Append($"<li>{lang}</li>");
    }

    stringBuilder.Append("</ul>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});

app.Run();

public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; } = 0;
    public string Country { get; set; } = "";
    public List<string> Languages { get; set; } = new();
    public University? University { get; set; }
}

public class University
{
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
    public int Course { get; set; } = 0;
}

public class Company
{
    public string Name { get; set; } = "";
    public int Employees { get; set; } = 0;
}