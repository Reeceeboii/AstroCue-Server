namespace AstroCue.Server.Data.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using Entities;
    using Entities.Owned;

    public class HipParser : Parser
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HipParser"/> class
        /// </summary>
        /// <param name="fs"></param>
        public HipParser(IFileSystem fs)
            : base(fs, "I239_hip_main.tsv", "I239_hip_main_names.tsv") { }

        /// <summary>
        /// Parses the Hipparcos catalogue
        /// </summary>
        /// <returns>A list of <see cref="HipparcosObject"/> instances</returns>
        public override List<AstronomicalObject> ParseCatalogue()
        {
            Dictionary<int, string> nameDictionary = new();
            List<AstronomicalObject> hipparcosObjects = new();

            // set up name dictionary
            foreach (string nameEntry in this.NameCatalogueContent)
            {
                // IAU name of the star
                string name;

                // the star's corresponding HIP catalogue identifier
                int hipId;

                try
                {
                    name = nameEntry[..18].Trim();
                    hipId = Convert.ToInt32(nameEntry[87..93]);
                }
                catch (FormatException)
                {
                    // any missing HIP catalogue IDs result in the entry being excluded from the dictionary
                    continue;
                }

                nameDictionary.Add(hipId, name);
            }

            // parse the main data file
            foreach (string hipparcosEntry in this.DataCatalogueContent)
            {
                // Catalogue entry number (HIP 12345) = 12345
                int catalogueIdentifier;

                // Right ascension (07(h) 11(m) 29.89(s)) = RightAscension type
                RightAscension rightAscension;

                // Declination (+58(deg) 00(m) 55.2(s)) = Declination type
                Declination declination;

                // Apparent magnitude (V from the UBV photometric system)
                // https://en.wikipedia.org/wiki/UBV_photometric_system
                float apparentMagnitude;

                // attempt to parse row from catalogue - deal with any formatting errors by excluding from resulting list
                try
                {
                    rightAscension = ParserUtils.ParseRightAscension(hipparcosEntry[..16]);
                    declination = ParserUtils.ParseDeclination(hipparcosEntry[17..33]);
                    catalogueIdentifier = Convert.ToInt32(hipparcosEntry[34..40]);
                    apparentMagnitude = float.Parse(hipparcosEntry[41..46]);
                }
                catch (Exception exc) when (
                    exc is FormatException or ArgumentOutOfRangeException)
                {
                    continue;
                }

                // attempt to extract the IAU name from the dictionary
                nameDictionary.TryGetValue(catalogueIdentifier, out string name);

                hipparcosObjects.Add(new HipObject()
                {
                    CatalogueIdentifier = catalogueIdentifier,
                    RightAscension = rightAscension,
                    Declination = declination,
                    ApparentMagnitude = apparentMagnitude,
                    Name = name // nullable
                });
            }

            return hipparcosObjects;
        }

    }
}
