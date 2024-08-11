using BlazorAppRadzenRoleBasedAuthwithIdentity.Models;
using BlazorAppRadzenRoleBasedAuthwithIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlazorAppRadzenRoleBasedAuthwithIdentity.Data;

public class SeedData
{
    private readonly ApplicationDbContext _context;

    public SeedData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateInitialData()
    {
        var posts = GetAllBlogPosts();
        await _context.BlogPosts.AddRangeAsync(posts);
        await _context.SaveChangesAsync();
    }

    public async Task CreateInitialIdentityData(UserManager<ApplicationUser> userManager, UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim> userStore)
    {
        await _context.ApplicationRoles.AddRangeAsync(applicationRoles);
        await _context.SaveChangesAsync();

        var emailStore = (IUserEmailStore<ApplicationUser>)userStore;

        // add default users
        var admin = new ApplicationUser();
        string adminUserName = "admin@admin.com";
        string adminEmail = "admin@admin.com";
        string adminPass = "123456";
        await userStore.SetUserNameAsync(admin, adminUserName, CancellationToken.None);
        await emailStore.SetEmailAsync(admin, adminEmail, CancellationToken.None);
        admin.EmailConfirmed = true;
        var resultAdmin = await userManager.CreateAsync(admin, adminPass);
        if (!resultAdmin.Succeeded) throw new Exception($"Error on Adding Admin user. Errors:{string.Join(",", resultAdmin.Errors)}");
        await userManager.AddToRoleAsync(admin, "User");
        await userManager.AddToRoleAsync(admin, "Admin");

        await _context.SaveChangesAsync();
    }

    private static IEnumerable<BlogPost> GetAllBlogPosts()
    {
        List<BlogPost> posts = new();
        for (int i = 0; i < 50; i++)
        {
            BlogPost post = new() { Id = i + 1, Title = titles[i], Content = contents[i % 10] };
            posts.Add(post);
        }

        return posts;
    }

    private static readonly string[] titles = {
        "Introduction to Object-Oriented Programming",
        "Mastering Data Structures and Algorithms",
        "Building Web Applications with ASP.NET",
        "Creating Mobile Apps with Xamarin",
        "Exploring Artificial Intelligence and Machine Learning",
        "Understanding Functional Programming Concepts",
        "Developing Games with Unity",
        "Securing Web Applications from Cyber Attacks",
        "Optimizing Code Performance for Better Efficiency",
        "Implementing Design Patterns in Software Development",
        "Testing and Debugging Strategies for Reliable Software",
        "Working with Databases and SQL",
        "Building Responsive User Interfaces with HTML and CSS",
        "Exploring Cloud Computing and Serverless Architecture",
        "Developing Cross-Platform Applications with React Native",
        "Introduction to Internet of Things (IoT)",
        "Creating Scalable Microservices with Docker and Kubernetes",
        "Understanding Network Protocols and TCP/IP",
        "Building RESTful APIs with Node.js and Express",
        "Exploring Big Data Analytics and Apache Hadoop",
        "Mastering Version Control with Git and GitHub",
        "Developing Desktop Applications with WPF",
        "Securing Mobile Applications from Malicious Attacks",
        "Optimizing Database Performance with Indexing",
        "Implementing Continuous Integration and Deployment",
        "Testing Mobile Apps on Different Platforms",
        "Working with NoSQL Databases like MongoDB",
        "Building Progressive Web Apps with React",
        "Exploring Quantum Computing and Quantum Algorithms",
        "Introduction to Cybersecurity and Ethical Hacking",
        "Creating Chatbots with Natural Language Processing",
        "Understanding Software Development Life Cycle",
        "Developing Augmented Reality (AR) Applications",
        "Securing Web APIs with OAuth and JWT",
        "Optimizing Front-End Performance for Better User Experience",
        "Implementing Machine Learning Models with TensorFlow",
        "Testing Web Applications for Cross-Browser Compatibility",
        "Working with Blockchain Technology and Smart Contracts",
        "Building Real-Time Applications with SignalR",
        "Exploring Cryptography and Encryption Techniques",
        "Introduction to Agile Software Development",
        "Creating Voice User Interfaces with Amazon Alexa",
        "Understanding Web Accessibility and Inclusive Design",
        "Developing Natural Language Processing Applications",
        "Securing Cloud Infrastructure and Services",
        "Optimizing Backend Performance for Scalability",
        "Implementing Continuous Monitoring and Alerting",
        "Testing APIs with Postman and Swagger",
        "Working with Data Visualization Libraries like D3.js",
        "Building E-commerce Applications with Shopify",
        "Exploring Robotic Process Automation (RPA)",
        "Introduction to DevOps and CI/CD Pipelines"
    };

    private static readonly string[] contents = new string[]
       {
        "Lorem ipsum dolor sit amet, consectetur t.",
        "Sed ut perspiciatis unde omnis iste natuccusantium doloremque laudantium.",
        "Nemo enim ipsam voluptatem quia voluptas aut fugit.",
        "Quis autem vel eum iure reprehenderit quesse quam nihil molestiae consequatur.",
        "At vero eos et accusamus et iusto odio d.",
        "Similique sunt in culpa qui officia de.",
        "Et harum quidem rerum facilis est et expio.",
        "Nam libero tempore, cum soluta nobis est.",
        "Omnis voluptas assumenda est, omnis dolo",
        "Temporibus autem quibusdam et aut offic"
       };

    private static readonly ApplicationRole[] applicationRoles =
        [
            new ApplicationRole{ Id = "58F5B081FBD84EF49BDFC5172F37E4C3", Name = "User",NormalizedName = "USER", Description = "User Role"},
            new ApplicationRole{ Id = "80DCD20A284F417DAD10D5A0D2D9C936", Name = "Admin",NormalizedName = "ADMIN", Description = "Admin Role"}
        ];
}