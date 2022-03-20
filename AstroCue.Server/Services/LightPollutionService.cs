namespace AstroCue.Server.Services
{
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using Astronomy;
    using MaxRev.Gdal.Core;
    using Models.Misc;
    using OSGeo.GDAL;

    /// <summary>
    /// Service class to manage the retrieval of light pollution data from the World Atlas GeoTIFF file
    /// </summary>
    public class LightPollutionService : ILightPollutionService
    {
        /// <summary>
        /// Instance of <see cref="Dataset"/>
        /// </summary>
        private readonly Dataset _worldAtlasDataset;

        /// <summary>
        /// Name of the World Atlas dataset file
        /// </summary>
        private const string DataFileName = "World Atlas.tif";

        /// <summary>
        /// The World Atlas dataset's geo transform matrix
        /// </summary>
        private readonly double[] _geoTransformMatrix;

        /// <summary>
        /// Initialises a new instance of the <see cref="LightPollutionService"/> class
        /// </summary>
        /// <param name="fileSystem">Instance of <see cref="IFileSystem"/></param>
        public LightPollutionService(IFileSystem fileSystem)
        {
            GdalBase.ConfigureAll();

            string dataFilePath = fileSystem.Path.Join("./Res", DataFileName);
            if (!fileSystem.File.Exists(dataFilePath))
            {
                throw new FileNotFoundException(nameof(DataFileName));
            }

            this._worldAtlasDataset = Gdal.Open(dataFilePath, Access.GA_ReadOnly);

            if (this._worldAtlasDataset == null)
            {
                throw new Exception("World Atlas dataset loaded via GDAL is null");
            }

            if (this._worldAtlasDataset.GetDriver() == null)
            {
                throw new Exception("Driver is null");
            }

            // extract the dataset's geo transform matrix
            this._geoTransformMatrix = new double[6];
            this._worldAtlasDataset.GetGeoTransform(this._geoTransformMatrix);
        }

        /// <summary>
        /// Retrieve light pollution data for a given set of coordinates
        /// </summary>
        /// <param name="longitude">The longitude value to query</param>
        /// <param name="latitude">The latitude value to query</param>
        /// <returns>An instance of <see cref="LightPollution"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">If the coordinates lie outside of the dataset</exception>
        public LightPollution GetLightPollutionForCoords(float longitude, float latitude)
        {
            Band band = this._worldAtlasDataset.GetRasterBand(1);

            // calculate pixel offsets
            int xOffset = (int)((longitude - this._geoTransformMatrix[0]) / this._geoTransformMatrix[1]);
            int yOffset = (int)((latitude - this._geoTransformMatrix[3]) / this._geoTransformMatrix[5]);

            // check that calculated offsets are in bounds
            if (OffsetsOutOfBounds(band, xOffset, yOffset))
            {
                throw new ArgumentOutOfRangeException(nameof(band), "Given coordinates lie outside of dataset");
            }

            // read band 1 data at x and y offsets [mcd/m^2]
            float[] milicandellaPerM2 = new float[1];
            band.ReadRaster(xOffset, yOffset, 1, 1, milicandellaPerM2, 1, 1, 0, 0);

            int bortle = Bortle.McdM2ToBortle(milicandellaPerM2[0]);

            return new LightPollution()
            {
                BortleValue = bortle,
                BortleDesc = Bortle.ScaleToDescription(bortle),
                RawMilicandella = milicandellaPerM2[0]
            };
        }

        /// <summary>
        /// Given a GeoTIFF band a two pixel offsets, run a check to ensure that
        /// the offsets lie inside the band's bounds
        /// </summary>
        /// <param name="band">Instance of <see cref="Band"/></param>
        /// <param name="xOffset">X offset value, in pixels</param>
        /// <param name="yOffset">Y offset value, in pixels</param>
        /// <returns></returns>
        private static bool OffsetsOutOfBounds(Band band, int xOffset, int yOffset)
        {
            if (xOffset < 0 || xOffset > band.XSize)
            {
                return true;
            }

            return yOffset < 0 || yOffset > band.YSize;
        }
    }
}
