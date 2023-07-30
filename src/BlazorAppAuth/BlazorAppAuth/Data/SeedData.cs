using BlazorAppAuth.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorAppAuth.Data;

public class SeedData
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;

    public SeedData(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
    {
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
    }

    public async Task CreateInitialData()
    {
        foreach (var projectRole in ProjectRoles.Roles)
        {
            var applicationRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = projectRole.RoleName, NormalizedName = projectRole.NormalizedName, Description = projectRole.Description };
            _context.Roles.Add(applicationRole);
        }
        await _context.SaveChangesAsync();

        var adminRole = _context.Roles.First(x => x.Name == ProjectRoles.AdminRole.RoleName);
        foreach (var claims in ProjectClaims.GetAllClaims)
            foreach (var claim in claims)
                _context.RoleClaims.Add(new ApplicationRoleClaim { RoleId = adminRole.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
        await _context.SaveChangesAsync();

        var userRole = _context.Roles.First(x => x.Name == ProjectRoles.UserRole.RoleName);
        _context.RoleClaims.Add(new ApplicationRoleClaim { RoleId = userRole.Id, ClaimType = ProjectClaims.BlogPostRead.Type, ClaimValue = ProjectClaims.BlogPostRead.Value });
        await _context.SaveChangesAsync();

        var admin = new ApplicationUser();
        string adminEmail = "admin@admin.com";
        string adminPass = "123456";
        await _userStore.SetUserNameAsync(admin, adminEmail, CancellationToken.None);
        await _emailStore.SetEmailAsync(admin, adminEmail, CancellationToken.None);
        var resultAdmin = await _userManager.CreateAsync(admin, adminPass);
        if (!resultAdmin.Succeeded) throw new Exception($"Error on Adding Admin user. Errors:{string.Join(",", resultAdmin.Errors)}");
        foreach (var role in ProjectRoles.Roles)
            await _userManager.AddToRoleAsync(admin, role.RoleName);
        await _context.SaveChangesAsync();

        var user = new ApplicationUser();
        string userEmail = "user@user.com";
        string userPass = "123456";
        await _userStore.SetUserNameAsync(user, userEmail, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, userEmail, CancellationToken.None);
        var resultUser = await _userManager.CreateAsync(user, userPass);
        if (!resultUser.Succeeded) throw new Exception($"Error on Adding User user. Errors:{string.Join(",", resultUser.Errors)}");
        await _userManager.AddToRoleAsync(user, ProjectRoles.UserRole.RoleName);

        // add BlogPosts
        var posts = GetAllBlogPosts();
        await _context.BlogPosts.AddRangeAsync(posts);
        await _context.SaveChangesAsync();
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
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
}