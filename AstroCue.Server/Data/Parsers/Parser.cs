namespace AstroCue.Server.Data.Parsers
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using Entities;

    /// <summary>
    /// Shared abstract behaviour for astronomical catalogue parsers
    /// </summary>
    public abstract class Parser
    {
        /// <summary>
        /// Gets or sets the content read from the data catalogue
        /// </summary>
        protected string[] DataCatalogueContent { get; }

        /// <summary>
        /// Gets or sets the content read from the name catalogue
        /// </summary>
        protected string[] NameCatalogueContent { get; }

        /// <summary>
        /// Gets or sets an instance of <see cref="IFileSystem"/>
        /// </summary>
        private IFileSystem FileSystem { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Parser"/> class (called by implementers)
        /// </summary>
        /// <param name="fileSystem">Instance of <see cref="IFileSystem"/></param>
        /// <param name="dataCatalogueFileName">Filename of the data catalogue</param>
        /// <param name="nameCatalogueFileName">Filename of the name catalogue</param>
        protected Parser(IFileSystem fileSystem, string dataCatalogueFileName, string nameCatalogueFileName)
        {
            this.FileSystem = fileSystem;
            string dataCatalogueRelativePath = this.FileSystem.Path.Join("./Res", dataCatalogueFileName);
            string nameCatalogueRelativePath = this.FileSystem.Path.Join("./Res", nameCatalogueFileName);

            // check that catalogue files are present
            if (!this.FileSystem.File.Exists(dataCatalogueRelativePath))
            {
                throw new FileNotFoundException(dataCatalogueRelativePath);
            }

            if (!this.FileSystem.File.Exists(nameCatalogueRelativePath))
            {
                throw new FileNotFoundException(nameCatalogueRelativePath);
            }

            // at this point, the paths are set up and the files are confirmed to exist. Load them!
            this.DataCatalogueContent = this.FileSystem.File.ReadAllLines(dataCatalogueRelativePath);
            this.NameCatalogueContent = this.FileSystem.File.ReadAllLines(nameCatalogueRelativePath);
        }

        /// <summary>
        /// Parses a given catalogue
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="AstronomicalObject"/> instances</returns>
        public abstract IEnumerable<AstronomicalObject> ParseCatalogue();
    }
}
