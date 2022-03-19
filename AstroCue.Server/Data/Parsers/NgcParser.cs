namespace AstroCue.Server.Data.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using Entities;
    using Entities.Owned;

    public class NgcParser : Parser
    {
        /// <summary>
        /// NGC object type mapping to textual representations.
        /// Taken from VizieR VII/1B/catalog
        /// </summary>
        private static readonly Dictionary<char, string> NgcBaseTypes = new()
        {
            { '0', "Unverified southern object" },
            { '1', "Open Cluster" },
            { '2', "Globular Cluster" },
            { '3', "Diffuse Nebula" },
            { '4', "Planetary Nebula" },
            { '5', "Galaxy" },
            { '6', "Cluster associated with nebulosity" },
            // 7 = non existent, will be excluded during parsing
        };

        /// <summary>
        /// Suffix for the type of an NGC object if it lies inside the Large Magellanic Cloud.
        /// https://en.wikipedia.org/wiki/Magellanic_Clouds
        /// </summary>
        private const char ObjectInLmcType = '8';

        /// <summary>
        /// Suffix for the type of an NGC object if it lies inside the Small Magellanic Cloud.
        /// https://en.wikipedia.org/wiki/Magellanic_Clouds
        /// </summary>
        private const char ObjectInSmcType = '9';

        /// <summary>
        /// Initialises a new instance of the <see cref="NgcParser"/> class
        /// </summary>
        /// <param name="fs">Instance of <see cref="IFileSystem"/></param>
        public NgcParser(IFileSystem fs)
            : base(fs, "VII_1B_catalog.tsv", "VII_1B_catalog_names.tsv") { }

        /// <summary>
        /// Parse the New General Catalogue
        /// </summary>
        /// <returns>A list of <see cref="AstronomicalObject"/> instances</returns>
        public override List<AstronomicalObject> ParseCatalogue()
        {
            Dictionary<int, string> nameDictionary = new();
            List<AstronomicalObject> ngcObjects = new();

            // setup the name dictionary
            foreach (string nameEntry in this.NameCatalogueContent)
            {
                string name = nameEntry[..35].Trim();

                string rawId;

                // entries without an NGC catalogue ID are excluded
                try
                {
                    rawId = nameEntry[36..41];
                }
                catch (ArgumentException)
                {
                    continue;
                }

                // index catalogues are not being used so we can skip these names
                if (rawId[0] == 'I')
                {
                    continue;
                }

                int ngcId;

                try
                {
                    ngcId = Convert.ToInt32(rawId);
                }
                catch (FormatException)
                {
                    continue;
                }

                // some entries have >1 name, so we need to allow for that
                if (!nameDictionary.ContainsKey(ngcId))
                {
                    nameDictionary.Add(ngcId, name);
                }
                else
                {
                    // append to to the existing value with a comma
                    nameDictionary[ngcId] = $"{nameDictionary[ngcId]}, {name}";
                }
            }

            // now the main data catalogue can be parsed
            foreach (string ngcEntry in this.DataCatalogueContent)
            {
                // i.e. NGC 345
                int catalogueIdentifier;

                // Right ascension (07(h) 11(m) 29.89(s)) = RightAscension type
                RightAscension rightAscension;

                // Declination (+58(deg) 00(m) 55.2(s)) = Declination type
                Declination declination;

                // is the object one component of a multiple system?
                bool partOfMultipleSystem;

                // galaxy or nebula etc...
                string type;

                // Apparent magnitude (V from the UBV photometric system)
                // https://en.wikipedia.org/wiki/UBV_photometric_system
                float apparentMagnitude;

                try
                {
                    rightAscension = ParserUtils.ParseRightAscension(ngcEntry[..10]);
                    declination = ParserUtils.ParseDeclination(ngcEntry[11..20]);
                    catalogueIdentifier = Convert.ToInt32(ngcEntry[21..25]);
                    partOfMultipleSystem = ngcEntry[26..27] != " "; // "A", "B" etc denote a yes here
                    apparentMagnitude = float.Parse(ngcEntry[31..35]);

                    type = ngcEntry[28..30].Trim();
                    if (type == "7") // non-existent | VII/1B/catalog
                    {
                        continue;
                    }

                    type = ParseNgcType(type);

                }
                catch (Exception exc) when (
                    exc is FormatException or ArgumentOutOfRangeException)
                {
                    continue;
                }

                nameDictionary.TryGetValue(catalogueIdentifier, out string name);

                ngcObjects.Add(new NgcObject()
                {
                    CatalogueIdentifier = catalogueIdentifier,
                    RightAscension = rightAscension,
                    Declination = declination,
                    ApparentMagnitude = apparentMagnitude,
                    Name = name,
                    PartOfMultipleSystem = partOfMultipleSystem,
                    Type = type
                });
            }

            return ngcObjects;
        }

        /// <summary>
        /// Parses an NGC type value read from the catalogue
        /// </summary>
        /// <param name="unprocessed"></param>
        /// <returns></returns>
        private static string ParseNgcType(string unprocessed)
        {
            return unprocessed.Length switch
            {
                // regular base type - can be directly returned from dictionary
                1 => NgcBaseTypes[char.Parse(unprocessed)],
                2 => unprocessed[1] switch
                {
                    // e.g. type "38" = Diffuse Nebula inside Large Large Magellanic Cloud
                    ObjectInLmcType => $"{NgcBaseTypes[unprocessed[0]]} inside Large Large Magellanic Cloud",
                    // e.g. type "29" = Globular cluster in Small Magellanic Cloud
                    ObjectInSmcType => $"{NgcBaseTypes[unprocessed[0]]} inside Small Magellanic Cloud",
                    // e.g. "65" = Cluster associated with nebulosity inside Galaxy
                    // e.g. "35" = Diffuse Nebula inside Galaxy
                    // etc...
                    _ => $"{NgcBaseTypes[unprocessed[0]]} inside {NgcBaseTypes[unprocessed[1]]}"
                },
                _ => string.Empty
            };
        }
    }
}
