namespace Inbox.Help.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Inboxy.Core.DataAccess;
    using Inboxy.Help;
    using Inboxy.Ticket.Domain;
    using Xunit;

    public class DocumentationTests
    {
       [Fact]
        public void AllDocumentationListedFilesAreExists()
        {
            var assemblies = new List<Assembly>()
            {
                Assembly.GetAssembly(typeof(Ticket)),
                Assembly.GetAssembly(typeof(CoreDbContext)),
            };

            var files = this.GetAllDocumentationFiles(assemblies);

            Assert.All(files, f => CheckFileExistsOrException($"{this.helpPath}/{f}"));
        }


        /// <summary>
        /// Get list of files names for Forms that are decorated with <see cref="DocumentationAttribute"/>
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllDocumentationFiles(IEnumerable<Assembly> assemblies)
        {
            var documents = new List<string>();

            foreach (var assembly in assemblies)
            {
                //Get all forms decorated with documentation attribute
                var formsWithDocumentations = assembly.GetTypesDecoratedByAttribute<DocumentationAttribute>();

                //Get forms documentations
                var formsDocumentations = formsWithDocumentations.SelectMany(t => t.GetCustomAttributes<DocumentationAttribute>());

                //Get documentations files
                var files = formsDocumentations.SelectMany(t => t.SourceType == DocumentationSourceType.File ? t.Files.Append(t.Content) : t.Files);

                documents.AddRange(files);
            }

            return documents;
        }

        public readonly string helpPath = @"C://Projects//inboxy//Inboxy.Web//Help//";

        private static void CheckFileExistsOrException(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
        }
    }
}