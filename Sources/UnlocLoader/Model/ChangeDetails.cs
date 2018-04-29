namespace UnlocLoader.Model
{
    /// <summary>
    /// Classifiers of the change details as specified in the "Secretary notes".
    /// </summary>
    public enum ChangeDetails
    {
        /// <summary>
        /// Change affecting or adding Coordinates.
        /// </summary>
        [ChangeDetailsMap("@Coo")]
        Coordinates,

        /// <summary>
        /// Change affecting the Function.
        /// </summary>
        [ChangeDetailsMap("@Fun")]
        Function,

        /// <summary>
        /// Change of status.
        /// </summary>
        [ChangeDetailsMap("@Sta")]
        Status,

        /// <summary>
        /// Addition or change of subdivision code.
        /// </summary>
        [ChangeDetailsMap("@Sub")]
        SubdivisionCode,

        /// <summary>
        /// Change in the location name.
        /// </summary>
        [ChangeDetailsMap("@Nam")]
        LocationName,

        /// <summary>
        /// Correction of spelling name.
        /// </summary>
        [ChangeDetailsMap("@Spe")]
        SpellingName
    }
}