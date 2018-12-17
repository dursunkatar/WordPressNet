using WordPressNet;
using WordPressNet.Models;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WordPressClient(new WordpressConfig
            {
                BlogId = 1,
                User = "dursun",
                Password = "12345",
                Url = "http://wordpress.com/xmlrpc.php"
            });

            var categoryResult = client.NewCategory(new Category
            {
                Name = "my category",
                Description = "my description"
            });

            var fileUploadResult = client.UploadFile("C:\\image.png");
        }
    }
}
