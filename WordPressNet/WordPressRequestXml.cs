using System;
using System.IO;
using WordPressNet.Models;

namespace WordPressNet
{
    internal class WordPressRequestXml
    {
        private readonly string[,] fileTypes;
        private readonly WordpressConfig wpConfig;

        public WordPressRequestXml(WordpressConfig config)
        {
            wpConfig = config;
            fileTypes = FileTypes();
        }

        public string NewCategory(Category category)
        {
            return BuildXml(
                     "newCategory",
                     $@"<param><value><struct>
                     {Member("name", Value("string", category.Name))}
                     {Member("description", Value("string", category.Description))}
                     {Member("parent_id", Value("int", category.ParentId.ToString()))}
                     {Member("slug", Value("string", category.Slug))}
                     </struct></value></param>"
                     );
        }

        public string UploadFile(string filePath, bool overwrite)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var (fileName, fileType, fileBase64) = GetFileInfo(filePath);

            if (fileType == null)
                throw new ArgumentException("invalid file type");

            return BuildXml(
                     "uploadFile",
                     $@"<param><value><struct>
                     {Member("name", Value("string", fileName))}
                     {Member("type", Value("string", fileType))}
                     {Member("bits", Value("base64", fileBase64))}
                     {Member("overwrite", Value("bool", overwrite.ToString()))}
                     </struct></value></param>"
                     );
        }
        private string BuildXml(string methodName, string methodParams)
        {
            return $@"<?xml version=""1.0""?>
                    <methodCall>
                    <methodName>wp.{methodName}</methodName>
                    <params>
                    {ParamValue("int", wpConfig.BlogId.ToString())}
                    {ParamValue("string", wpConfig.User)}
                    {ParamValue("string", wpConfig.Password)}
                    {methodParams}
                    </params>
                    </methodCall>";
        }

        private string[,] FileTypes()
        {
            return new string[9, 2] {
                {"wav","audio/wav" },
                {"gif","image/gif"},
                {"jpeg","image/jpeg"},
                {"jpg","image/jpeg"},
                {"png","image/png"},
                {"bmp","image/bmp"},
                {"avi","video/avi"},
                {"mpeg","video/mpeg"},
                {"pdf","application/pdf"}
            };
        }
        private string GetFileType(string file)
        {
            int length = fileTypes.Length / 2;
            for (int i = 0; i < length; i++)
                if (fileTypes[i, 0] == file)
                    return fileTypes[i, 1];
            return null;
        }
        private string Value(string valueType, string value)
        {
            return $"<value><{valueType}>{value}</{valueType}></value>";
        }
        private string Member(string memberName, string value)
        {
            return $"<member><name>{memberName}</name>{value}</member>";
        }
        private string ParamValue(string valueType, string value)
        {
            return $"<param>{Value(valueType, value) }</param>";
        }
        private (string fileName, string fileType, string fileBase64) GetFileInfo(string filePath)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1),
                   fileType = GetFileType(fileName.Substring(fileName.LastIndexOf(".") + 1)),
                   fileBase64 = Convert.ToBase64String(File.ReadAllBytes(filePath));
            return (fileName, fileType, fileBase64);
        }
    }
}
