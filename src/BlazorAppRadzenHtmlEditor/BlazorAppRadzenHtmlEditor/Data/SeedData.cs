using BlazorAppRadzenHtmlEditor.Models;

namespace BlazorAppRadzenHtmlEditor.Data;

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

    private static IEnumerable<BlogPost> GetAllBlogPosts()
    {
        List<BlogPost> posts = new();
        for (int i = 0; i < 50; i++)
        {
            BlogPost post = new() { Id = i + 1, Title = titles[i], Content = contents[i % 6] };
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

        "<div><img src=\"/upload-2024-05-12-3d2c9bc2-4ab6-4ede-acc9-4b6763081996.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Lorem ipsum dolor sit amet, consectetur adipiscing elit.</span> \r\nDuis ultricies posuere leo nec ornare. Etiam eu tempus tellus. Aliquam bibendum justo diam, et tincidunt ipsum tempus eu. Aenean vel sem ante. Vivamus maximus ornare imperdiet. Curabitur varius, arcu vitae pretium lobortis, dolor nisi facilisis orci, nec elementum nibh mi vel nisl. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed a venenatis nisi. Nullam sit amet ipsum maximus, tempus mi eget, sollicitudin diam.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-3d2c9bc2-4ab6-4ede-acc9-4b6763081996.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Sed faucibus, ex et pulvinar vulputate, erat diam sollicitudin odio, ut euismod enim odio nec nunc.</span> \r\nCurabitur nec dolor sem. Vivamus euismod mi risus, in bibendum erat cursus sed. Integer et nisi metus. In hac habitasse platea dictumst. Ut faucibus pretium sapien non ullamcorper. Etiam pretium mollis neque. Phasellus congue suscipit elit vitae porttitor. Mauris eget tellus non eros ultricies vehicula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Fusce sed tortor in enim luctus sagittis et consequat odio. Phasellus eu sapien quis turpis consequat tempus at quis mi. Aenean non justo lorem. Morbi tempus nec arcu a ullamcorper.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-b43d01df-f3d0-42d0-9fe2-1844c78ae414.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Vestibulum non eros risus.</span> \r\nSuspendisse orci diam, rhoncus vitae sollicitudin eu, porta at mauris. Mauris varius euismod dictum. Donec turpis diam, ultrices sed libero sit amet, tempor cursus urna. Pellentesque ut mattis nisi. Aenean luctus at sem id facilisis. Suspendisse tempus pellentesque sapien nec aliquam. Sed id mi bibendum, posuere elit in, viverra purus. Etiam id ultricies elit. Praesent a molestie felis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Sed vulputate ex id neque congue, id condimentum dui dignissim. Nulla pellentesque libero id leo fringilla bibendum. Suspendisse id aliquam quam.\r\n<br></div>",

        "<div><img src=\"/upload-2024-05-12-6777339d-9aa3-4f6a-af0d-015e671fd67b.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Duis facilisis nisi at sapien mattis, eu tincidunt massa aliquam.</span> \r\nAenean eu erat pretium, tempus massa at, auctor lectus. Cras faucibus turpis ex, ut pretium diam condimentum vitae. Fusce blandit nisl at suscipit maximus. Donec tristique tellus dolor, et sollicitudin tellus porttitor ac. Suspendisse iaculis molestie congue. Mauris non mattis urna. Integer cursus orci arcu, non pellentesque felis porttitor sit amet. Nulla facilisi.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-6777339d-9aa3-4f6a-af0d-015e671fd67b.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Duis consequat mi nec diam faucibus tincidunt.</span> \r\nNunc ut feugiat dui. Proin malesuada nisl sit amet nulla scelerisque, in mollis mi rhoncus. Ut vel tristique lorem, ut feugiat enim. Pellentesque vel sapien feugiat, porttitor elit consequat, tristique eros. Aenean feugiat ex eget libero aliquet, a egestas dui ornare. Vestibulum dolor ligula, molestie eu lacus quis, pulvinar egestas turpis. Suspendisse a lobortis leo, at finibus magna. Nam nec tristique nisi, blandit dapibus neque. Aenean in velit a leo condimentum eleifend. Duis posuere urna lacus, id blandit nisl laoreet sed. Curabitur ligula turpis, pellentesque in purus eu, tincidunt posuere libero. Nulla finibus, erat ac ultrices posuere, tellus enim blandit nisl, at consectetur tellus nibh eget est. Donec gravida tortor quis fringilla pharetra. Pellentesque aliquam pellentesque risus nec rhoncus. Nunc mattis elit tellus, vel congue lectus mollis sed.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-5cb5f731-72ee-4b10-831c-f976eed6fef0.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Aenean tristique, nulla ac vehicula dignissim, turpis est accumsan urna, sed finibus lacus dui eget nulla.</span> \r\nSed finibus elit magna, vitae iaculis dui ornare a. Praesent tempor ligula sapien, id hendrerit diam cursus eget. Nam eu fermentum justo. Etiam lobortis lacinia erat, in dapibus est lacinia eu. Mauris sed quam sapien. Aenean maximus, nibh tincidunt tincidunt molestie, nisl arcu convallis mauris, at accumsan magna ex eu libero. Praesent vel vehicula massa. Suspendisse sodales augue id dui rutrum, at commodo lectus elementum. Nulla sed ante viverra, rhoncus erat ac, varius sem.\r\n<br></div>",

        "<div><img src=\"/upload-2024-05-12-221b63e2-d7c8-4d7b-aa24-dd0c54461015.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Mauris scelerisque quam at posuere imperdiet.</span> \r\nMorbi porta vitae lacus ac hendrerit. Suspendisse posuere nulla ut finibus aliquet. Integer pretium pulvinar erat in efficitur. Proin justo ex, tincidunt eu congue non, mollis et nisl. Nunc nec rhoncus leo, eget eleifend diam. Phasellus feugiat nisl nec enim volutpat pulvinar. Maecenas ac urna in eros imperdiet dictum.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-221b63e2-d7c8-4d7b-aa24-dd0c54461015.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Duis varius in mauris id posuere.</span> \r\nEtiam lobortis purus augue, vitae ultricies purus consequat maximus. Mauris tincidunt molestie ligula, sit amet pulvinar nibh consectetur sit amet. Aliquam pellentesque malesuada orci id maximus. Curabitur lacinia, orci sed consectetur dignissim, ex dolor pulvinar orci, a consequat massa lectus vitae ipsum. Duis et augue vel odio placerat feugiat vitae id mauris. In mi turpis, consequat quis gravida sit amet, suscipit sit amet felis. Nam feugiat lobortis lectus ac bibendum. Aliquam id consequat sem, vel vehicula turpis. Pellentesque id dui malesuada, tincidunt felis id, faucibus mauris. Vestibulum ut ligula in enim vulputate euismod. Cras vel diam egestas, ultricies nisl ut, ornare magna.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-e627b879-a260-470d-b0fb-527ddc44af7d.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Vivamus erat diam, rutrum at nibh id, rhoncus tincidunt nunc.</span> \r\nMorbi sed libero condimentum, pharetra mauris id, facilisis felis. Pellentesque varius hendrerit rhoncus. In hac habitasse platea dictumst. Phasellus ex nisl, tincidunt quis lacus ac, suscipit sollicitudin mi. Nunc ultrices tempor mi. Aliquam ante urna, vestibulum in pellentesque nec, tempus sed sem. Nunc elementum arcu pretium nisl euismod condimentum sed eget urna. Praesent vitae tellus quis ante sollicitudin bibendum. Praesent a leo eros. Praesent pellentesque, odio sed consectetur maximus, diam nibh scelerisque nulla, et semper nisl libero consectetur ex. Vestibulum eget placerat nunc. Curabitur posuere justo quis massa venenatis, sit amet aliquam nisi scelerisque. Proin dignissim dui vulputate, dignissim nisl ac, mattis nibh.\r\n<br></div>",

        "<div><img src=\"/upload-2024-05-12-ef3cb0a3-020e-4a00-8a79-f1706550f445.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Donec sed sapien in sapien imperdiet vestibulum in eget risus.</span> \r\nNulla metus purus, vehicula in neque id, ullamcorper finibus eros. Maecenas felis neque, molestie vitae urna at, lacinia sodales metus. Etiam cursus mi eget justo tincidunt, et cursus tortor efficitur. Aliquam fermentum lacinia ipsum, et maximus ante mattis vel. Cras rhoncus id dui sit amet finibus. Quisque ac tempus augue. Vestibulum at rhoncus velit. Nullam eget nulla bibendum, suscipit augue sed, accumsan nulla. Nam suscipit nunc id ipsum convallis, et egestas ipsum dictum. Integer at ante augue. Aenean sit amet cursus risus. Duis vel risus non mauris accumsan dictum.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-ef3cb0a3-020e-4a00-8a79-f1706550f445.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Cras semper eros a scelerisque vehicula.</span> \r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Proin malesuada eros non sem fermentum, in auctor sem venenatis. Sed sollicitudin rhoncus orci, vitae fermentum leo condimentum eget. Mauris id lacinia neque. Etiam arcu purus, laoreet ac iaculis eu, sagittis a libero. Phasellus in nisl sit amet urna mattis bibendum. In condimentum pharetra est.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-35621ccb-ff3c-46c8-b510-405d9abb87ca.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Morbi posuere felis pretium, fringilla lacus a, ultricies urna.</span> \r\nCurabitur non diam vel dolor commodo dapibus ac vel urna. Duis dui ligula, sodales id lorem in, maximus molestie turpis. Donec vitae urna rutrum, cursus diam sed, vestibulum dui. Fusce id aliquet justo. Mauris lobortis, lacus ut eleifend lobortis, orci odio sollicitudin leo, ac sodales dui metus eget sapien. Etiam rutrum lorem id blandit vulputate. Vivamus suscipit augue sed augue efficitur commodo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Donec vitae orci non sem molestie fermentum. Nunc lobortis diam ligula. Phasellus imperdiet est vel sodales lacinia. Sed at dui eu magna lacinia fermentum.\r\n<br></div>",

        "<div><img src=\"/upload-2024-05-12-6bb077bb-4a31-4f8c-b275-499ca2ec6e4b.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Nam ac massa vel ipsum consequat pretium et sit amet erat.</span> \r\nAenean risus lectus, varius a dolor quis, auctor accumsan felis. Donec dignissim quam in lacus auctor, a luctus mauris suscipit. Donec risus tellus, maximus nec odio vel, faucibus aliquet eros. Integer ut elit quis lacus vehicula consequat. Etiam porta ornare varius. Praesent non hendrerit neque. Vestibulum vehicula elementum libero quis interdum. Aliquam at nulla nisi. Ut vulputate lacus eget tortor pretium, vel luctus orci commodo.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-6bb077bb-4a31-4f8c-b275-499ca2ec6e4b.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Nunc ut tortor maximus, dapibus sem vel, tincidunt ligula.</span> \r\nSuspendisse sed tortor magna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec at urna metus. Integer at maximus sapien, vitae imperdiet tellus. Nunc mollis ultrices rhoncus. Nullam ullamcorper ante sit amet sollicitudin aliquam. Proin sit amet urna non nulla hendrerit tempus. Proin sodales enim at est interdum aliquam.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-bd2672cb-495e-41b9-aa3d-6e52189f5876.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Sed egestas blandit nisl, non gravida est.</span> \r\nPraesent aliquam auctor nunc, a blandit lorem varius at. Vestibulum dictum turpis sit amet lorem iaculis, posuere tempor turpis scelerisque. Pellentesque nec lacus mauris. Aenean tincidunt facilisis mauris sit amet sollicitudin. Vivamus et ligula eu metus tempus facilisis nec sed ligula. Nam ultricies urna non elit suscipit placerat. Etiam imperdiet egestas sem nec lacinia. Fusce malesuada neque in tortor sodales, vitae sagittis purus eleifend.\r\n<br></div>",

        "<div><img src=\"/upload-2024-05-12-1d87cac3-25db-4ef2-9646-0f57d4ea3bcd.jpeg\" width=\"100\" style=\"border-radius: 25px;\"></div> \r\n<span style=\"font-weight: bold;\">Cras nec nulla sed nulla elementum imperdiet.</span> \r\nNulla dapibus eget augue id ullamcorper. Donec euismod fringilla ante, a scelerisque arcu. Nulla facilisi. Nullam dolor turpis, ornare nec molestie non, volutpat sed est. Sed nulla sapien, dignissim sit amet ex eu, tempor pharetra enim. Vestibulum ac augue vel mi egestas ullamcorper. Ut at lectus congue, viverra eros eu, placerat ex. Pellentesque iaculis venenatis sapien, vel congue tortor. In hac habitasse platea dictumst.\r\n<div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-1d87cac3-25db-4ef2-9646-0f57d4ea3bcd.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Ut porta venenatis nulla, eu laoreet nisl interdum dignissim.</span> \r\nPraesent in porttitor nulla. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam et pellentesque turpis, non venenatis orci. Sed a bibendum odio. Pellentesque eget nisi nec nisl varius porta eget quis metus. Sed iaculis justo pharetra est interdum, sit amet dignissim diam auctor. Maecenas et augue velit. Maecenas nec varius mi. Vivamus viverra metus sed nulla tristique, vitae lacinia nunc commodo. Integer id lorem feugiat, auctor nibh sed, viverra mauris. Fusce ac luctus nisi. Aenean sit amet dolor laoreet, laoreet tortor eget, scelerisque nisi. Sed auctor rhoncus rutrum. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Mauris at mollis ligula.\r\n<br></div><div><br></div>\r\n<div style=\"text-align: center;\">\r\n<img src=\"/upload-2024-05-12-a8425c11-a159-40a1-92c4-d353d9ae068d.jpeg\" width=\"250\" style=\"border-radius: 25px;\">\r\n<br></div><div><br></div><div>\r\n<span style=\"font-weight: bold;\">Interdum et malesuada fames ac ante ipsum primis in faucibus.</span> \r\nUt ut tincidunt elit. Mauris ultrices nisi in eros varius, semper feugiat sapien euismod. Suspendisse laoreet molestie urna et tincidunt. Etiam commodo lacus a dignissim ornare. Aenean eget consequat purus. In hac habitasse platea dictumst. Integer dapibus nibh vel est gravida, congue tempor neque venenatis.\r\n<br></div>",

       };
}