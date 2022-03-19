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
        /// Relative path to the parser's data catalogue
        /// </summary>
        private readonly string _dataCatalogueRelativePath;

        /// <summary>
        /// Relative path to the parser's name catalogue
        /// </summary>
        private readonly string _nameCatalogueRelativePath;

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
            this._dataCatalogueRelativePath = this.FileSystem.Path.Join("./Res", dataCatalogueFileName);
            this._nameCatalogueRelativePath = this.FileSystem.Path.Join("./Res", nameCatalogueFileName);

            // check that catalogue files are present
            if (!this.FileSystem.File.Exists(this._dataCatalogueRelativePath))
            {
                throw new FileNotFoundException(this._dataCatalogueRelativePath);
            }

            if (!this.FileSystem.File.Exists(this._nameCatalogueRelativePath))
            {
                throw new FileNotFoundException(this._nameCatalogueRelativePath);
            }

            // at this point, the paths are set up and the files are confirmed to exist. Load them!
            this.DataCatalogueContent = this.FileSystem.File.ReadAllLines(this._dataCatalogueRelativePath);
            this.NameCatalogueContent = this.FileSystem.File.ReadAllLines(this._nameCatalogueRelativePath);
        }

        /// <summary>
        /// Parses the catalogue and returns a list of <see cref="AstronomicalObject"/> instances
        /// </summary>
        /// <returns></returns>
        public abstract List<AstronomicalObject> ParseCatalogue();
    }
}
