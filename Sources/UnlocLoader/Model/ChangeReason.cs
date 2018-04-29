namespace UnlocLoader.Model
{
    /// <summary>
    /// Classifiers in the change column as specified in the "Secretary notes".
    /// </summary>
    public enum ChangeReason
    {
        /// <summary>
        /// No change specified
        /// </summary>
        None,

        /// <summary>
        /// Marked for deletion in the next issue
        /// </summary>
        MarkedForDeletion,

        /// <summary>
        /// Change in location name
        /// </summary>
        ChangeLocationName,

        /// <summary>
        /// Other change in the entry
        /// </summary>
        ChangeOther,

        /// <summary>
        /// Entry added in the current issue
        /// </summary>
        Added,

        /// <summary>
        /// Reference entry
        /// </summary>
        Reference,

        /// <summary>
        /// Retained for certain entries in the USA code list ("controlled duplications")
        /// </summary>
        Retained
    }
}